using Silk.NET.OpenGLES;
using Silk.NET.Maths;
using System;

namespace FDK;

public class ScreenRenderer : IDisposable {
	private static uint VAO;
	private static uint VBO;
	private static uint EBO;
	private static uint ShaderProgram;
	private static int TextureLocation;

	private static bool initialized = false;

	public static void Init() {
		if (initialized) return;

		var gl = Game.Gl;

		// シェーダープログラムを作成
		ShaderProgram = ShaderHelper.CreateShaderProgramFromSource(
			// 頂点シェーダー
			@"#version 100
            precision mediump float;
            
            attribute vec3 aPosition;
            attribute vec2 aTexCoord;
            
            varying vec2 vTexCoord;
            
            void main() {
                vTexCoord = aTexCoord;
                gl_Position = vec4(aPosition, 1.0);
            }",

			// フラグメントシェーダー
			@"#version 100
            precision mediump float;
            
            uniform sampler2D uTexture;
            varying vec2 vTexCoord;
            
            void main() {
                gl_FragColor = texture2D(uTexture, vTexCoord);
            }"
		);

		TextureLocation = gl.GetUniformLocation(ShaderProgram, "uTexture");

		// フルスクリーンクワッドの頂点データ（テクスチャ座標を反転して上下反転を修正）
		float[] vertices = {
            // Position     // TexCoord
            -1.0f,  1.0f, 0.0f, 0.0f, 1.0f,  // 左上
             1.0f,  1.0f, 0.0f, 1.0f, 1.0f,  // 右上
            -1.0f, -1.0f, 0.0f, 0.0f, 0.0f,  // 左下
             1.0f, -1.0f, 0.0f, 1.0f, 0.0f   // 右下
        };

		uint[] indices = {
			0, 1, 2,
			2, 1, 3
		};

		// VAOを作成
		VAO = gl.GenVertexArray();
		gl.BindVertexArray(VAO);

		// VBOを作成
		VBO = gl.GenBuffer();
		gl.BindBuffer(BufferTargetARB.ArrayBuffer, VBO);
		unsafe {
			fixed (float* data = vertices) {
				gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint)(vertices.Length * sizeof(float)),
					data, BufferUsageARB.StaticDraw);
			}
		}

		// EBOを作成
		EBO = gl.GenBuffer();
		gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, EBO);
		unsafe {
			fixed (uint* data = indices) {
				gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint)(indices.Length * sizeof(uint)),
					data, BufferUsageARB.StaticDraw);
			}
		}

		// 頂点属性を設定
		uint positionLocation = (uint)gl.GetAttribLocation(ShaderProgram, "aPosition");
		gl.EnableVertexAttribArray(positionLocation);
		unsafe {
			gl.VertexAttribPointer(positionLocation, 3, VertexAttribPointerType.Float, false,
				5 * sizeof(float), (void*)0);
		}

		uint texCoordLocation = (uint)gl.GetAttribLocation(ShaderProgram, "aTexCoord");
		gl.EnableVertexAttribArray(texCoordLocation);
		unsafe {
			gl.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false,
				5 * sizeof(float), (void*)(3 * sizeof(float)));
		}

		// バインドを解除
		gl.BindVertexArray(0);
		gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
		gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);

		initialized = true;
	}

	/// <summary>
	/// レンダーテクスチャをゲームのビューポートサイズに合わせて描画
	/// Gameクラスで計算されたViewPortSizeとViewPortOffsetを使用
	/// </summary>
	/// <param name="renderTexture">描画するレンダーテクスチャ</param>
	/// <param name="viewportWidth">ビューポートの幅</param>
	/// <param name="viewportHeight">ビューポートの高さ</param>
	/// <param name="viewportX">ビューポートのX座標</param>
	/// <param name="viewportY">ビューポートのY座標</param>
	public static void DrawToScreen(RenderTexture renderTexture, int viewportWidth, int viewportHeight, int viewportX = 0, int viewportY = 0) {
		var gl = Game.Gl;

		gl.Disable(GLEnum.Blend);

		// ビューポートを設定（ゲームの描画領域のみ）
		gl.Viewport(viewportX, viewportY, (uint)viewportWidth, (uint)viewportHeight);

		// シェーダーを使用
		gl.UseProgram(ShaderProgram);

		// テクスチャをバインドしてバイリニアフィルタリングを設定
		gl.ActiveTexture(TextureUnit.Texture0);
		gl.BindTexture(TextureTarget.Texture2D, renderTexture.TextureId);
		
		// バイリニアフィルタリングを有効にする
		gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.Linear);
		gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.Linear);
		
		gl.Uniform1(TextureLocation, 0);

		// VAOをバインドして描画
		gl.BindVertexArray(VAO);
		unsafe {
			gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*)0);
		}
		gl.BindVertexArray(0);

		// テクスチャのバインドを解除
		gl.BindTexture(TextureTarget.Texture2D, 0);

		gl.Enable(GLEnum.Blend);
	}

	public static void Terminate() {
		if (!initialized) return;

		var gl = Game.Gl;
		gl.DeleteVertexArray(VAO);
		gl.DeleteBuffer(VBO);
		gl.DeleteBuffer(EBO);
		gl.DeleteProgram(ShaderProgram);

		initialized = false;
	}

	public void Dispose() {
		Terminate();
	}
}
