using System.Drawing;
using FDK;

namespace OpenNijiiroRW;

class CActSelect段位挑戦選択画面 : CActivity {
	public override void Activate() {
		ctBarIn = new CCounter();
		ctBarOut = new CCounter();
		ctBarOut.CurrentValue = 255;
		OpenNijiiroRW.stageDanSongSelect.bDifficultyIn = false;
		bOption = false;

		base.Activate();
	}

	public override void DeActivate() {
		base.DeActivate();
	}

	public override void CreateManagedResource() {
		base.CreateManagedResource();
	}

	public override void ReleaseManagedResource() {
		base.ReleaseManagedResource();
	}

	public override int Draw() {
		if (OpenNijiiroRW.stageDanSongSelect.bDifficultyIn || ctBarOut.CurrentValue < ctBarOut.EndValue) {
			ctBarIn.Tick();
			ctBarOut.Tick();

			OpenNijiiroRW.Tx.Challenge_Select[0].Opacity = OpenNijiiroRW.stageDanSongSelect.bDifficultyIn ? ctBarIn.CurrentValue : 255 - ctBarOut.CurrentValue;
			OpenNijiiroRW.Tx.Challenge_Select[1].Opacity = OpenNijiiroRW.stageDanSongSelect.bDifficultyIn ? ctBarIn.CurrentValue : 255 - ctBarOut.CurrentValue;
			OpenNijiiroRW.Tx.Challenge_Select[2].Opacity = OpenNijiiroRW.stageDanSongSelect.bDifficultyIn ? ctBarIn.CurrentValue : 255 - ctBarOut.CurrentValue;

			OpenNijiiroRW.Tx.Challenge_Select[0].t2D描画(0, 0);

			int selectIndex = (2 - n現在の選択行);
			int[] challenge_select_rect = OpenNijiiroRW.Skin.DaniSelect_Challenge_Select_Rect[selectIndex];

			OpenNijiiroRW.Tx.Challenge_Select[2].t2D描画(OpenNijiiroRW.Skin.DaniSelect_Challenge_Select_X[selectIndex], OpenNijiiroRW.Skin.DaniSelect_Challenge_Select_Y[selectIndex],
				new Rectangle(challenge_select_rect[0], challenge_select_rect[1], challenge_select_rect[2], challenge_select_rect[3]));

			OpenNijiiroRW.Tx.Challenge_Select[1].t2D描画(0, 0);


			if (OpenNijiiroRW.stageDanSongSelect.ct待機.IsStarted)
				return base.Draw();

			#region [Key bindings]

			if (ctBarIn.IsEnded && !OpenNijiiroRW.stageDanSongSelect.b選択した && bOption == false) {
				if (OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.RightArrow) ||
					OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.RBlue)) {
					if (n現在の選択行 - 1 >= 0) {
						OpenNijiiroRW.Skin.soundChangeSFX.tPlay();
						n現在の選択行--;
					}
				}

				if (OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.LeftArrow) ||
					OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.LBlue)) {
					if (n現在の選択行 + 1 <= 2) {
						OpenNijiiroRW.Skin.soundChangeSFX.tPlay();
						n現在の選択行++;
					}
				}

				if (OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Return) ||
					OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.LRed) ||
					OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.RRed)) {
					if (n現在の選択行 == 0) {
						this.ctBarOut.Start(0, 255, 0.5f, OpenNijiiroRW.Timer);
						OpenNijiiroRW.Skin.soundCancelSFX.tPlay();
						OpenNijiiroRW.stageDanSongSelect.bDifficultyIn = false;
					} else if (n現在の選択行 == 1) {
						//TJAPlayer3.Skin.soundDanSongSelect.t再生する();
						OpenNijiiroRW.ConfigIni.bTokkunMode = false;
						OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
						OpenNijiiroRW.Skin.voiceMenuDanSelectConfirm[OpenNijiiroRW.SaveFile]?.tPlay();
						OpenNijiiroRW.stageDanSongSelect.ct待機.Start(0, 3000, 1, OpenNijiiroRW.Timer);
					} else if (n現在の選択行 == 2) {
						bOption = true;
					}
				}
			}

			#endregion
		}

		return base.Draw();
	}

	public CCounter ctBarIn;
	public CCounter ctBarOut;

	public bool bOption;

	private int n現在の選択行;
}
