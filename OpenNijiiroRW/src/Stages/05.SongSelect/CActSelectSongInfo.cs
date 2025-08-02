using System.Drawing;
using FDK;

// Minimalist menu class to use for custom menus
namespace OpenNijiiroRW;

class CActSelectSongInfo : CStage {
	public CActSelectSongInfo() {
		base.IsDeActivated = true;
	}

	public override void Activate() {
		// On activation

		if (base.IsActivated)
			return;



		base.Activate();
	}

	public override void DeActivate() {
		// On de-activation

		base.DeActivate();
	}

	public override void CreateManagedResource() {
		// Ressource allocation

		base.CreateManagedResource();
	}

	public override void ReleaseManagedResource() {
		// Ressource freeing

		base.ReleaseManagedResource();
	}

	public override int Draw() {
		if (OpenNijiiroRW.stageSongSelect.rNowSelectedSong != null && OpenNijiiroRW.stageSongSelect.rNowSelectedSong.nodeType == CSongListNode.ENodeType.SCORE) {
			int[] bpms = new int[3] {
				(int)OpenNijiiroRW.stageSongSelect.rNowSelectedSong.score[OpenNijiiroRW.stageSongSelect.actSongList.tFetchDifficulty(OpenNijiiroRW.stageSongSelect.rNowSelectedSong)].譜面情報.BaseBpm,
				(int)OpenNijiiroRW.stageSongSelect.rNowSelectedSong.score[OpenNijiiroRW.stageSongSelect.actSongList.tFetchDifficulty(OpenNijiiroRW.stageSongSelect.rNowSelectedSong)].譜面情報.MinBpm,
				(int)OpenNijiiroRW.stageSongSelect.rNowSelectedSong.score[OpenNijiiroRW.stageSongSelect.actSongList.tFetchDifficulty(OpenNijiiroRW.stageSongSelect.rNowSelectedSong)].譜面情報.MaxBpm
			};
			for (int i = 0; i < 3; i++) {
				tBPMNumberDraw(OpenNijiiroRW.Skin.SongSelect_Bpm_X[i], OpenNijiiroRW.Skin.SongSelect_Bpm_Y[i], bpms[i]);
			}

			if (OpenNijiiroRW.stageSongSelect.actSongList.ttkSelectedSongMaker != null && OpenNijiiroRW.Skin.SongSelect_Maker_Show) {
				TitleTextureKey.ResolveTitleTexture(OpenNijiiroRW.stageSongSelect.actSongList.ttkSelectedSongMaker).t2D拡大率考慮描画(CTexture.RefPnt.Left, OpenNijiiroRW.Skin.SongSelect_Maker[0], OpenNijiiroRW.Skin.SongSelect_Maker[1]);
			}
			if (OpenNijiiroRW.stageSongSelect.actSongList.ttkSelectedSongBPM != null && OpenNijiiroRW.Skin.SongSelect_BPM_Text_Show) {
				TitleTextureKey.ResolveTitleTexture(OpenNijiiroRW.stageSongSelect.actSongList.ttkSelectedSongBPM).t2D拡大率考慮描画(CTexture.RefPnt.Left, OpenNijiiroRW.Skin.SongSelect_BPM_Text[0], OpenNijiiroRW.Skin.SongSelect_BPM_Text[1]);
			}
			if (OpenNijiiroRW.stageSongSelect.rNowSelectedSong.bExplicit)
				OpenNijiiroRW.Tx.SongSelect_Explicit?.t2D描画(OpenNijiiroRW.Skin.SongSelect_Explicit[0], OpenNijiiroRW.Skin.SongSelect_Explicit[1]);
			if (OpenNijiiroRW.stageSongSelect.rNowSelectedSong.bMovie)
				OpenNijiiroRW.Tx.SongSelect_Movie?.t2D描画(OpenNijiiroRW.Skin.SongSelect_Movie[0], OpenNijiiroRW.Skin.SongSelect_Movie[1]);
		}


		return 0;
	}

	#region [Private]

	private void tBPMNumberDraw(float originx, float originy, int num) {
		int[] nums = CConversion.SeparateDigits(num);

		for (int j = 0; j < nums.Length; j++) {
			if (OpenNijiiroRW.Skin.SongSelect_Bpm_Show && OpenNijiiroRW.Tx.SongSelect_Bpm_Number != null) {
				float offset = j;
				float x = originx - (OpenNijiiroRW.Skin.SongSelect_Bpm_Interval[0] * offset);
				float y = originy - (OpenNijiiroRW.Skin.SongSelect_Bpm_Interval[1] * offset);

				float width = OpenNijiiroRW.Tx.SongSelect_Bpm_Number.sz画像サイズ.Width / 10.0f;
				float height = OpenNijiiroRW.Tx.SongSelect_Bpm_Number.sz画像サイズ.Height;

				OpenNijiiroRW.Tx.SongSelect_Bpm_Number.t2D描画(x, y, new RectangleF(width * nums[j], 0, width, height));
			}
		}
	}

	#endregion
}
