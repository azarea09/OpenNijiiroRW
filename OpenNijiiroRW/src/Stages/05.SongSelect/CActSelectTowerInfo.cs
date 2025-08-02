using System.Drawing;
using FDK;

// Minimalist menu class to use for custom menus
namespace OpenNijiiroRW;

class CActSelectTowerInfo : CStage {
	public CActSelectTowerInfo() {
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
		if (OpenNijiiroRW.stageSongSelect.rNowSelectedSong.score[5] != null)
			tFloorNumberDraw(OpenNijiiroRW.Skin.SongSelect_FloorNum_X, OpenNijiiroRW.Skin.SongSelect_FloorNum_Y, OpenNijiiroRW.stageSongSelect.rNowSelectedSong.score[5].譜面情報.nTotalFloor);

		return 0;
	}

	#region [Private]

	private void tFloorNumberDraw(float originx, float originy, int num) {
		int[] nums = CConversion.SeparateDigits(num);

		for (int j = 0; j < nums.Length; j++) {
			if (OpenNijiiroRW.Skin.SongSelect_FloorNum_Show && OpenNijiiroRW.Tx.SongSelect_Floor_Number != null) {
				float offset = j;
				float x = originx - (OpenNijiiroRW.Skin.SongSelect_FloorNum_Interval[0] * offset);
				float y = originy - (OpenNijiiroRW.Skin.SongSelect_FloorNum_Interval[1] * offset);

				float width = OpenNijiiroRW.Tx.SongSelect_Floor_Number.sz画像サイズ.Width / 10.0f;
				float height = OpenNijiiroRW.Tx.SongSelect_Floor_Number.sz画像サイズ.Height;

				OpenNijiiroRW.Tx.SongSelect_Floor_Number.t2D描画(x, y, new RectangleF(width * nums[j], 0, width, height));
			}
		}
	}

	#endregion
}
