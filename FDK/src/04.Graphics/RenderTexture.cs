using Silk.NET.OpenGLES;
using Silk.NET.Maths;
using System;

namespace FDK;

public class RenderTexture : IDisposable {
	private uint framebuffer;
	private uint colorTexture;
	private uint depthRenderbuffer;
	public int Width { get; private set; }
	public int Height { get; private set; }
	public uint TextureId => colorTexture;

	/// <summary>
	/// レンダーテクスチャを作成
	/// </summary>
	/// <param name="width">テクスチャの幅</param>
	/// <param name="height">テクスチャの高さ</param>
	public RenderTexture(int width, int height) {
		Width = width;
		Height = height;
		CreateFramebuffer();
	}

	private void CreateFramebuffer() {
		var gl = Game.Gl;

		// フレームバッファを作成
		framebuffer = gl.GenFramebuffer();
		gl.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);

		// カラーテクスチャを作成
		colorTexture = gl.GenTexture();
		gl.BindTexture(TextureTarget.Texture2D, colorTexture);
		unsafe {
			if (OperatingSystem.IsMacOS()) {
				// macOSの場合はsized internal formatを使用
				gl.TexImage2D(TextureTarget.Texture2D, 0, (int)InternalFormat.Rgba8,
					(uint)Width, (uint)Height, 0, PixelFormat.Rgba, GLEnum.UnsignedByte, null);
			} else {
				// OpenGL ESの場合はunsized formatを使用
				gl.TexImage2D(TextureTarget.Texture2D, 0, (int)PixelFormat.Rgba,
					(uint)Width, (uint)Height, 0, PixelFormat.Rgba, GLEnum.UnsignedByte, null);
			}
		}

		// バイリニアフィルタリングを設定
		gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMinFilter, (int)TextureMinFilter.Linear);
		gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureMagFilter, (int)TextureMagFilter.Linear);
		gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
		gl.TexParameterI(GLEnum.Texture2D, GLEnum.TextureWrapT, (int)TextureWrapMode.ClampToEdge);

		// カラーテクスチャをフレームバッファにアタッチ
		gl.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
			TextureTarget.Texture2D, colorTexture, 0);

		// デプスレンダーバッファを作成
		depthRenderbuffer = gl.GenRenderbuffer();
		gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depthRenderbuffer);
		gl.RenderbufferStorage(RenderbufferTarget.Renderbuffer, InternalFormat.DepthComponent16,
			(uint)Width, (uint)Height);
		gl.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment,
			RenderbufferTarget.Renderbuffer, depthRenderbuffer);

		// フレームバッファの完成度をチェック
		var status = gl.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
		if (status != GLEnum.FramebufferComplete) {
			throw new Exception($"フレームバッファが不完全です: {status}");
		}

		// デフォルトフレームバッファに戻す
		gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
		gl.BindTexture(TextureTarget.Texture2D, 0);
		gl.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
	}

	/// <summary>
	/// このレンダーテクスチャをアクティブにして描画開始
	/// </summary>
	public void BeginDraw() {
		var gl = Game.Gl;
		gl.BindFramebuffer(FramebufferTarget.Framebuffer, framebuffer);
		gl.Viewport(0, 0, (uint)Width, (uint)Height);

		// 透明な黒でクリア（重要：アルファを0にする）
		gl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
		gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		// プリマルチプライドアルファブレンディングを設定
		gl.Enable(GLEnum.Blend);
		gl.BlendFuncSeparate(GLEnum.One, GLEnum.OneMinusSrcAlpha, GLEnum.One, GLEnum.OneMinusSrcAlpha);
	}

	/// <summary>
	/// レンダーテクスチャへの描画を終了
	/// </summary>
	public void EndDraw() {
		var gl = Game.Gl;

		// 通常のブレンドモードに戻す
		gl.BlendFunc(GLEnum.SrcAlpha, GLEnum.OneMinusSrcAlpha);

		gl.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
	}

	public void Dispose() {
		var gl = Game.Gl;
		gl.DeleteFramebuffer(framebuffer);
		gl.DeleteTexture(colorTexture);
		gl.DeleteRenderbuffer(depthRenderbuffer);
	}
}
