using Silk.NET.Core;
using Silk.NET.GLFW;
using Silk.NET.Maths;
using Silk.NET.OpenGLES;
using Silk.NET.Windowing;
using SkiaSharp;

namespace FDK;

public abstract class Game : IDisposable
{
	public static GL Gl { get; private set; }
	public static Silk.NET.Core.Contexts.IGLContext Context { get; private set; }
	public static AnglePlatformType GraphicsDeviceType_ = AnglePlatformType.OpenGL;
	public static string strIconFileName;
	public static List<Action> AsyncActions { get; private set; } = new();
	public static int MainThreadID { get; private set; }
	public static long TimeMs;

	public IWindow WindowContext;

	private string _windowTitle = "OpenNijiiroRW";
	public string WindowTitle
	{
		get
		{
			return _windowTitle;
		}
		set
		{
			_windowTitle = value;
			if (WindowContext != null)
			{
				WindowContext.Title = value;
			}
		}
	}

	private Vector2D<int> _windowSize = new(1280, 720);
	public Vector2D<int> WindowSize
	{
		get
		{
			return _windowSize;
		}
		set
		{
			_windowSize = value;
			if (WindowContext != null)
			{
				WindowContext.Size = value;
			}
		}
	}

	private Vector2D<int> _windowPosition = new(0, 0);
	public Vector2D<int> WindowPosition
	{
		get
		{
			return _windowPosition;
		}
		set
		{
			_windowPosition = value;
			if (WindowContext != null)
			{
				WindowContext.Position = value;
			}
		}
	}

	private int _framerate;
	public int Framerate
	{
		get
		{
			return _framerate;
		}
		set
		{
			_framerate = value;
			if (WindowContext != null)
			{
				UpdateWindowFramerate(VSync, value);
			}
		}
	}

	private bool _fullScreen;
	public bool FullScreen
	{
		get
		{
			return _fullScreen;
		}
		set
		{
			_fullScreen = value;
			if (WindowContext != null)
			{
				WindowContext.WindowState = value ? WindowState.Fullscreen : WindowState.Normal;
			}
		}
	}

	private bool _vSync = true;
	public bool VSync
	{
		get
		{
			return _vSync;
		}
		set
		{
			_vSync = value;
			if (WindowContext != null)
			{
				UpdateWindowFramerate(value, Framerate);
				WindowContext.VSync = value;
			}
		}
	}

	private Vector2D<int> ViewPortSize = new Vector2D<int>();
	private Vector2D<int> ViewPortOffset = new Vector2D<int>();
	private RenderTexture renderTexture;

	protected virtual void Configuration()
	{

	}

	protected virtual void Initialize()
	{

	}


	protected virtual void LoadContent()
	{

	}

	protected virtual void UnloadContent()
	{

	}

	protected virtual void OnExiting()
	{

	}

	protected virtual void Update()
	{

	}

	protected virtual void Draw()
	{

	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Game"/> class.
	/// </summary>
	protected Game(string iconFileName)
	{
		strIconFileName = iconFileName;

		MainThreadID = Thread.CurrentThread.ManagedThreadId;
		Configuration();

		WindowOptions options = WindowOptions.Default;

		options.Size = WindowSize;
		options.Position = WindowPosition;
		options.UpdatesPerSecond = VSync ? 0 : Framerate;
		options.FramesPerSecond = VSync ? 0 : Framerate;
		options.WindowState = FullScreen ? WindowState.Fullscreen : WindowState.Normal;
		options.VSync = VSync;

		if (!OperatingSystem.IsMacOS()) options.API = GraphicsAPI.None;

		options.WindowBorder = WindowBorder.Resizable;
		options.Title = WindowTitle;

		Silk.NET.Windowing.Glfw.GlfwWindowing.Use();

		WindowContext = Window.Create(options);

		ViewPortSize.X = WindowContext.Size.X;
		ViewPortSize.Y = WindowContext.Size.Y;
		ViewPortOffset.X = 0;
		ViewPortOffset.Y = 0;

		WindowContext.Load += Window_Load;
		WindowContext.Closing += Window_Closing;
		WindowContext.Update += Window_Update;
		WindowContext.Render += Window_Render;
		WindowContext.Resize += Window_Resize;
		WindowContext.Move += Window_Move;
		WindowContext.FramebufferResize += Window_FramebufferResize;
	}

	private void UpdateWindowFramerate(bool vsync, int value)
	{
		if (vsync)
		{
			WindowContext.UpdatesPerSecond = 0;
			WindowContext.FramesPerSecond = 0;
			Context.SwapInterval(1);
		}
		else
		{
			WindowContext.UpdatesPerSecond = value;
			WindowContext.FramesPerSecond = value;
			Context.SwapInterval(0);
		}
	}

	/// <summary>
	/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
	/// </summary>
	public void Dispose()
	{
		WindowContext.Dispose();
	}

	public void Exit()
	{
		WindowContext.Close();
	}

	protected void ToggleFullScreen()
	{
		FullScreen = !FullScreen;
	}

	/// <summary>
	/// Runs the game.
	/// </summary>
	public void Run()
	{
		WindowContext.Run();
	}

	public void Window_Load()
	{
		WindowContext.SetWindowIcon(new ReadOnlySpan<RawImage>(GetIconData(strIconFileName)));

		if (OperatingSystem.IsMacOS())
		{
			if (WindowContext.GLContext == null)
			{
				throw new Exception("No native OpenGL context available");
			}
			Context = WindowContext.GLContext;
		}
		else
		{
			Context = new AngleContext(GraphicsDeviceType_, WindowContext);
			Context.MakeCurrent();
		}

		Gl = GL.GetApi(Context);

		Gl.Enable(GLEnum.Blend);
		BlendHelper.SetBlend(BlendType.Normal);
		CTexture.Init();

		// ScreenRendererを初期化
		ScreenRenderer.Init();

		// レンダーテクスチャを作成（1920x1080固定）
		renderTexture = new RenderTexture(RenderSurfaceSize.Width, RenderSurfaceSize.Height);

		Gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

		// 初期ビューポートサイズを設定
		Window_Resize(WindowContext.Size);

		Context.SwapInterval(VSync ? 1 : 0);

		Initialize();
		LoadContent();
	}

	public void Window_Closing()
	{
		// リソースを解放
		renderTexture?.Dispose();
		ScreenRenderer.Terminate();

		CTexture.Terminate();
		UnloadContent();
		OnExiting();
		Context.Dispose();
	}

	public void Window_Update(double deltaTime)
	{
		TimeMs = (long)(WindowContext.Time * 1000);
		Update();
	}

	public void Window_Render(double deltaTime)
	{
		if (AsyncActions.Count > 0)
		{
			AsyncActions[0]?.Invoke();
			AsyncActions.Remove(AsyncActions[0]);
		}

		// レンダーテクスチャに描画開始
		renderTexture.BeginDraw();

		// ゲームの描画処理を実行
		Draw();

		// レンダーテクスチャへの描画終了
		renderTexture.EndDraw();

		// メインフレームバッファに戻す
		// ウィンドウ全体のビューポートを設定してクリア
		Gl.Viewport(0, 0, (uint)WindowContext.Size.X, (uint)WindowContext.Size.Y);
		Gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f); // 黒で塗りつぶし
		Gl.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

		// レンダーテクスチャを適切なサイズとポジションで描画（バイリニアフィルタリング付き）
		ScreenRenderer.DrawToScreen(renderTexture, ViewPortSize.X, ViewPortSize.Y, ViewPortOffset.X, ViewPortOffset.Y);

		if (!OperatingSystem.IsMacOS()) Context.SwapBuffers();
	}

	public void Window_Resize(Vector2D<int> size)
	{
		if (size.X <= 0 || size.Y <= 0) return;

		float gameAspect = (float)RenderSurfaceSize.Width / RenderSurfaceSize.Height; // 16:9 = 1.777...
		float windowAspect = (float)size.X / size.Y;

		if (windowAspect > gameAspect)
		{
			// ウィンドウが横長の場合：高さに合わせてスケール
			ViewPortSize.Y = size.Y;
			ViewPortSize.X = (int)(size.Y * gameAspect);
			ViewPortOffset.X = (size.X - ViewPortSize.X) / 2;
			ViewPortOffset.Y = 0;
		}
		else
		{
			// ウィンドウが縦長の場合：幅に合わせてスケール
			ViewPortSize.X = size.X;
			ViewPortSize.Y = (int)(size.X / gameAspect);
			ViewPortOffset.X = 0;
			ViewPortOffset.Y = (size.Y - ViewPortSize.Y) / 2;
		}

		// デバッグ出力（必要に応じて）
		Console.WriteLine($"Window: {size.X}x{size.Y}, Viewport: {ViewPortSize.X}x{ViewPortSize.Y}, Offset: ({ViewPortOffset.X}, {ViewPortOffset.Y})");
	}

	public void Window_Move(Vector2D<int> size)
	{
		WindowPosition = size;
	}

	public void Window_FramebufferResize(Vector2D<int> size) { }


	#region [Helper Function]

	public unsafe SKBitmap GetScreenShot()
	{
		int ViewportWidth = ViewPortSize.X;
		int ViewportHeight = ViewPortSize.Y;
		fixed (uint* pixels = new uint[(uint)ViewportWidth * (uint)ViewportHeight])
		{
			Gl.ReadBuffer(GLEnum.Front);
			Gl.ReadPixels(ViewPortOffset.X, ViewPortOffset.Y, (uint)ViewportWidth, (uint)ViewportHeight, PixelFormat.Bgra, GLEnum.UnsignedByte, pixels);

			fixed (uint* pixels2 = new uint[(uint)ViewportWidth * (uint)ViewportHeight])
			{
				for (int x = 0; x < ViewportWidth; x++)
				{
					for (int y = 1; y < ViewportHeight; y++)
					{
						int pos = x + ((y - 1) * ViewportWidth);
						int pos2 = x + ((ViewportHeight - y) * ViewportWidth);
						var p = pixels[pos2];
						pixels2[pos] = p;
					}
				}

				using SKBitmap sKBitmap = new(ViewportWidth, ViewportHeight - 1);
				sKBitmap.SetPixels((IntPtr)pixels2);
				return sKBitmap.Copy();
			}
		}
	}

	public unsafe void GetScreenShotAsync(Action<SKBitmap> action)
	{
		int ViewportWidth = ViewPortSize.X;
		int ViewportHeight = ViewPortSize.Y;
		byte[] pixels = new byte[(uint)ViewportWidth * (uint)ViewportHeight * 4];
		Gl.ReadBuffer(GLEnum.Front);
		fixed (byte* pix = pixels)
		{
			Gl.ReadPixels(ViewPortOffset.X, ViewPortOffset.Y, (uint)ViewportWidth, (uint)ViewportHeight, PixelFormat.Bgra, GLEnum.UnsignedByte, pix);
		}

		Task.Run(() =>
		{
			fixed (byte* pixels2 = new byte[(uint)ViewportWidth * (uint)ViewportHeight * 4])
			{
				for (int x = 0; x < ViewportWidth; x++)
				{
					for (int y = 1; y < ViewportHeight; y++)
					{
						int pos = x + ((y - 1) * ViewportWidth);
						int pos2 = x + ((ViewportHeight - y) * ViewportWidth);
						pixels2[(pos * 4) + 0] = pixels[(pos2 * 4) + 0];
						pixels2[(pos * 4) + 1] = pixels[(pos2 * 4) + 1];
						pixels2[(pos * 4) + 2] = pixels[(pos2 * 4) + 2];
						pixels2[(pos * 4) + 3] = 255;
					}
				}

				using SKBitmap sKBitmap = new(ViewportWidth, ViewportHeight - 1);
				sKBitmap.SetPixels((IntPtr)pixels2);

				using SKBitmap scaledBitmap = new(RenderSurfaceSize.Width, RenderSurfaceSize.Height);
				if (sKBitmap.ScalePixels(scaledBitmap, SKFilterQuality.High)) action(scaledBitmap);
				else action(sKBitmap);
			}
		});
	}

	private RawImage GetIconData(string fileName)
	{
		SKCodec codec = SKCodec.Create(fileName);
		using SKBitmap bitmap = SKBitmap.Decode(codec, new SKImageInfo(codec.Info.Width, codec.Info.Height, SKColorType.Rgba8888));
		return new RawImage(bitmap.Width, bitmap.Height, bitmap.GetPixelSpan().ToArray());
	}

	#endregion
}
