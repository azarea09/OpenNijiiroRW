namespace OpenNijiiroRW;

class HScenePreset
{
	public static DBSkinPreset.SkinScene GetBGPreset()
	{
		string presetSection = "";
		if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Tower)
		{
			presetSection = "Tower";
		}
		else if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan)
		{
			presetSection = "Dan";
		}
		else if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
		{
			presetSection = "AI";
		}
		else
		{
			presetSection = "Regular";
		}

		Dictionary<string, DBSkinPreset.SkinScene> _ps = [];

		switch (presetSection)
		{
			case "Regular":
				_ps = OpenNijiiroRW.Skin.Game_SkinScenes.Regular;
				break;
			case "Dan":
				_ps = OpenNijiiroRW.Skin.Game_SkinScenes.Dan;
				break;
			case "Tower":
				_ps = OpenNijiiroRW.Skin.Game_SkinScenes.Tower;
				break;
			case "AI":
				_ps = OpenNijiiroRW.Skin.Game_SkinScenes.AI;
				break;
		}
		;

		bool sectionIsValid = _ps.Count > 0;

		if (sectionIsValid && _ps.TryGetValue(OpenNijiiroRW.TJA?.scenePreset ?? OpenNijiiroRW.stageSongSelect.rChoosenSong.strScenePreset ?? "", out var result_song))
		{
			return result_song;
		}
		else if (sectionIsValid && _ps.TryGetValue(OpenNijiiroRW.stageSongSelect.rChoosenSong.strScenePreset ?? "", out var result_box))
		{
			return result_box;
		}
		else if (sectionIsValid && _ps.TryGetValue("", out var result_fallback))
		{
			return result_fallback;
		}
		else if (sectionIsValid)
		{
			Random rand = new Random();
			return _ps.ElementAt(rand.Next(0, _ps.Count)).Value;
		}

		return null;
	}
}
