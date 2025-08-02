namespace OpenNijiiroRW;

internal class CLuaFps {
	public double deltaTime => OpenNijiiroRW.FPS.DeltaTime;
	public int fps => OpenNijiiroRW.FPS.NowFPS;
}
