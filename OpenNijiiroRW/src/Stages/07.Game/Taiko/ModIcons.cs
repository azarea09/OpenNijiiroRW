namespace OpenNijiiroRW;

class ModIcons {
	static Dictionary<int, Action<int, int, int, int>> __methods = new Dictionary<int, Action<int, int, int, int>>()
	{
		{0, (x, y, a, p) => tDisplayHSIcon(x, y, a) },
		{1, (x, y, a, p) => tDisplayDoronIcon(x, y, a) },
		{2, (x, y, a, p) => tDisplayRandomIcon(x, y, a) },
		{3, (x, y, a, p) => tDisplayFunModIcon(x, y, a) },
		{4, (x, y, a, p) => tDisplayJustIcon(x, y, a) },
		{5, (x, y, a, p) => tDisplayTimingIcon(x, y, a) },
		{6, (x, y, a, p) => tDisplaySongSpeedIcon(x, y, p) },
		{7, (x, y, a, p) => tDisplayAutoIcon(x, y, p) },
	};

	static public void tDisplayMods(int x, int y, int player) {
		// +30 x/y
		int actual = OpenNijiiroRW.GetActualPlayer(player);

		for (int i = 0; i < 8; i++) {
			__methods[i](x + OpenNijiiroRW.Skin.ModIcons_OffsetX[i], y + OpenNijiiroRW.Skin.ModIcons_OffsetY[i], actual, player);
		}
	}

	static public void tDisplayModsMenu(int x, int y, int player) {
		if (OpenNijiiroRW.Tx.Mod_None != null)
			OpenNijiiroRW.Tx.Mod_None.Opacity = 0;

		int actual = OpenNijiiroRW.GetActualPlayer(player);

		for (int i = 0; i < 8; i++) {
			__methods[i](x + OpenNijiiroRW.Skin.ModIcons_OffsetX_Menu[i], y + OpenNijiiroRW.Skin.ModIcons_OffsetY_Menu[i], actual, player);
		}

		if (OpenNijiiroRW.Tx.Mod_None != null)
			OpenNijiiroRW.Tx.Mod_None.Opacity = 255;
	}

	#region [Displayables]

	static private void tDisplayHSIcon(int x, int y, int player) {
		// TO DO : Add HS x0.5 icon (_vals == 4)
		var _vals = new int[] { 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 24, 29, 34, 39 };
		int _i = -1;

		for (int j = 0; j < _vals.Length; j++) {
			if (OpenNijiiroRW.ConfigIni.nScrollSpeed[player] >= _vals[j] && j < OpenNijiiroRW.Tx.HiSp.Length)
				_i = j;
			else
				break;
		}

		if (_i >= 0)
			OpenNijiiroRW.Tx.HiSp[_i]?.t2D描画(x, y);
		else
			OpenNijiiroRW.Tx.Mod_None?.t2D描画(x, y);
	}

	static private void tDisplayAutoIcon(int x, int y, int player) {
		bool _displayed = false;

		if (OpenNijiiroRW.ConfigIni.bAutoPlay[player])
			_displayed = true;

		if (_displayed == true)
			OpenNijiiroRW.Tx.Mod_Auto?.t2D描画(x, y);
		else
			OpenNijiiroRW.Tx.Mod_None?.t2D描画(x, y);
	}

	static private void tDisplayDoronIcon(int x, int y, int player) {
		var conf_ = OpenNijiiroRW.ConfigIni.eSTEALTH[player];

		if (conf_ == EStealthMode.Doron)
			OpenNijiiroRW.Tx.Mod_Doron?.t2D描画(x, y);
		else if (conf_ == EStealthMode.Stealth)
			OpenNijiiroRW.Tx.Mod_Stealth?.t2D描画(x, y);
		else
			OpenNijiiroRW.Tx.Mod_None?.t2D描画(x, y);
	}

	static private void tDisplayJustIcon(int x, int y, int player) {
		var conf_ = OpenNijiiroRW.ConfigIni.bJust[player];

		if (conf_ == 1)
			OpenNijiiroRW.Tx.Mod_Just?.t2D描画(x, y);
		else if (conf_ == 2)
			OpenNijiiroRW.Tx.Mod_Safe?.t2D描画(x, y);
		else
			OpenNijiiroRW.Tx.Mod_None?.t2D描画(x, y);
	}

	static private void tDisplayRandomIcon(int x, int y, int player) {
		var rand_ = OpenNijiiroRW.ConfigIni.eRandom[player];

		if (rand_ == ERandomMode.Mirror)
			OpenNijiiroRW.Tx.Mod_Mirror?.t2D描画(x, y);
		else if (rand_ == ERandomMode.Random)
			OpenNijiiroRW.Tx.Mod_Random?.t2D描画(x, y);
		else if (rand_ == ERandomMode.SuperRandom)
			OpenNijiiroRW.Tx.Mod_Super?.t2D描画(x, y);
		else if (rand_ == ERandomMode.MirrorRandom)
			OpenNijiiroRW.Tx.Mod_Hyper?.t2D描画(x, y);
		else
			OpenNijiiroRW.Tx.Mod_None?.t2D描画(x, y);
	}

	static private void tDisplaySongSpeedIcon(int x, int y, int player) {
		if (OpenNijiiroRW.ConfigIni.nSongSpeed > 20)
			OpenNijiiroRW.Tx.Mod_SongSpeed[1]?.t2D描画(x, y);
		else if (OpenNijiiroRW.ConfigIni.nSongSpeed < 20)
			OpenNijiiroRW.Tx.Mod_SongSpeed[0]?.t2D描画(x, y);
		else
			OpenNijiiroRW.Tx.Mod_None?.t2D描画(x, y);
	}

	static private void tDisplayFunModIcon(int x, int y, int player) {
		int nFun = (int)OpenNijiiroRW.ConfigIni.nFunMods[player];

		if (nFun > 0)
			OpenNijiiroRW.Tx.Mod_Fun[nFun]?.t2D描画(x, y);
		else
			OpenNijiiroRW.Tx.Mod_None?.t2D描画(x, y);
	}

	static private void tDisplayTimingIcon(int x, int y, int player) {
		int zones = OpenNijiiroRW.ConfigIni.nTimingZones[player];

		if (zones != 2)
			OpenNijiiroRW.Tx.Mod_Timing[zones]?.t2D描画(x, y);
		else
			OpenNijiiroRW.Tx.Mod_None?.t2D描画(x, y);
	}

	static private void PLACEHOLDER_tDisplayNoneIcon(int x, int y, int player) {
		OpenNijiiroRW.Tx.Mod_None?.t2D描画(x, y);
	}

	#endregion

	#region [Mod flags]

	static public bool tPlayIsStock(int player) {
		int actual = OpenNijiiroRW.GetActualPlayer(player);

		if (OpenNijiiroRW.ConfigIni.nFunMods[actual] != EFunMods.None) return false;
		if (OpenNijiiroRW.ConfigIni.bJust[actual] != 0) return false;
		if (OpenNijiiroRW.ConfigIni.nTimingZones[actual] != 2) return false;
		if (OpenNijiiroRW.ConfigIni.nSongSpeed != 20) return false;
		if (OpenNijiiroRW.ConfigIni.eRandom[actual] != ERandomMode.Off) return false;
		if (OpenNijiiroRW.ConfigIni.eSTEALTH[actual] != EStealthMode.Off) return false;
		if (OpenNijiiroRW.ConfigIni.nScrollSpeed[actual] != 9) return false;

		return true;
	}
	static public Int64 tModsToPlayModsFlags(int player) {
		byte[] _flags = new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 };
		int actual = OpenNijiiroRW.GetActualPlayer(player);

		_flags[0] = (byte)Math.Min(255, OpenNijiiroRW.ConfigIni.nScrollSpeed[actual]);
		_flags[1] = (byte)OpenNijiiroRW.ConfigIni.eSTEALTH[actual];
		_flags[2] = (byte)OpenNijiiroRW.ConfigIni.eRandom[actual];
		_flags[3] = (byte)Math.Min(255, OpenNijiiroRW.ConfigIni.nSongSpeed);
		_flags[4] = (byte)OpenNijiiroRW.ConfigIni.nTimingZones[actual];
		_flags[5] = (byte)OpenNijiiroRW.ConfigIni.bJust[actual];
		_flags[7] = (byte)OpenNijiiroRW.ConfigIni.nFunMods[actual];

		return BitConverter.ToInt64(_flags, 0);
	}


	#endregion
}
