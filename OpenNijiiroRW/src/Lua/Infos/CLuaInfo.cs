namespace OpenNijiiroRW;

internal class CLuaInfo
{
	public int playerCount => OpenNijiiroRW.ConfigIni.nPlayerCount;
	public string lang => OpenNijiiroRW.ConfigIni.sLang;
	public bool simplemode => OpenNijiiroRW.ConfigIni.SimpleMode;
	public bool p1IsBlue => OpenNijiiroRW.P1IsBlue();
	public bool online => OpenNijiiroRW.app.bInternetConnectionSuccess;

	public string dir { get; init; }

	public CLuaInfo(string dir)
	{
		this.dir = dir;
	}
}
