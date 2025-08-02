﻿using System.Diagnostics;
using System.Text;
using FDK;

namespace OpenNijiiroRW;

class CStage段位選択 : CStage {
	public CStage段位選択() {
		base.eStageID = EStage.DanDojoSelect;
		base.ePhaseID = CStage.EPhase.Common_NORMAL;

		base.ChildActivities.Add(this.段位リスト = new CActSelect段位リスト());

		base.ChildActivities.Add(this.actFOtoNowLoading = new CActFIFOStart());
		base.ChildActivities.Add(this.段位挑戦選択画面 = new CActSelect段位挑戦選択画面());
		base.ChildActivities.Add(this.actFOtoTitle = new CActFIFOBlack());
		base.ChildActivities.Add(this.actPlayOption = new CActPlayOption());
		base.ChildActivities.Add(this.PuchiChara = new PuchiChara());
	}

	public override void Activate() {
		if (base.IsActivated)
			return;

		this.b選択した = false;

		base.ePhaseID = CStage.EPhase.Common_NORMAL;
		this.eフェードアウト完了時の戻り値 = CStageSongSelect.EReturnValue.継続;

		ct待機 = new CCounter();
		ctChara_In = new CCounter();

		// ctChara_Normal = new CCounter(0, TJAPlayer3.Tx.SongSelect_Chara_Normal.Length - 1, 1000 / 45, TJAPlayer3.Timer);
		CMenuCharacter.tMenuResetTimer(CMenuCharacter.ECharacterAnimation.NORMAL);


		bInSongPlayed = false;

		this.PuchiChara.IdleAnimation();

		Background = new ScriptBG(CSkin.Path($"{TextureLoader.BASE}{TextureLoader.DANISELECT}Script.lua"));
		Background.Init();

		base.Activate();
	}

	public override void DeActivate() {
		OpenNijiiroRW.tDisposeSafely(ref Background);

		base.DeActivate();
	}

	public override void CreateManagedResource() {

		base.CreateManagedResource();
	}

	public override void ReleaseManagedResource() {

		base.ReleaseManagedResource();
	}

	public override int Draw() {
		// ctChara_Normal.t進行Loop();
		ctChara_In.Tick();
		ct待機.Tick();

		int stamp = this.段位リスト.ctDaniIn.CurrentValue;

		float zoom = Math.Min(1.14f, Math.Max(1f, (float)Math.Pow(stamp / 3834f, 0.5f)));

		Background.Update();
		Background.Draw();

		//TJAPlayer3.Tx.Dani_Background.vc拡大縮小倍率.X = zoom;
		//TJAPlayer3.Tx.Dani_Background.vc拡大縮小倍率.Y = zoom;
		//TJAPlayer3.Tx.Dani_Background.t2D拡大率考慮中央基準描画(TJAPlayer3.Skin.Resolution[0] / 2, TJAPlayer3.Skin.Resolution[1] / 2);

		this.段位リスト.Draw();

		if (stamp < 6000) {
			#region [Dan intro anim]

			if (!bInSongPlayed) {
				this.段位リスト.ctDaniIn = new CCounter(0, 6000, 1, OpenNijiiroRW.Timer);
				OpenNijiiroRW.Skin.soundDanSongSelectIn.tPlay();
				bInSongPlayed = true;
			}

			/*
            int dani_dan_in_width = TJAPlayer3.Tx.Dani_Dan_In.szテクスチャサイズ.Width / 2;
            int dani_dan_in_height = TJAPlayer3.Tx.Dani_Dan_In.szテクスチャサイズ.Height;

            int doorLeft = 0;
            int doorRight = dani_dan_in_width;
            if (stamp >= 3834)
            {
                double screen_ratio = TJAPlayer3.Skin.Resolution[0] / 1280.0;

                doorLeft -= (int)((stamp - 3834) * screen_ratio);
                doorRight += (int)((stamp - 3834) * screen_ratio);
            }

            TJAPlayer3.Tx.Dani_Dan_In.t2D描画(doorLeft, 0, new Rectangle(0, 0, dani_dan_in_width, dani_dan_in_height));
            TJAPlayer3.Tx.Dani_Dan_In.t2D描画(doorRight, 0, new Rectangle(dani_dan_in_width, 0, dani_dan_in_width, dani_dan_in_height));
            */
			if (stamp <= 3834) {
				#region [Dan intro letters]

				//int quarter = TJAPlayer3.Tx.Dani_Dan_Text.szテクスチャサイズ.Width / 4;

				/*
                int[] xAxis = { 300, 980 };
                int[] yAxis = { 198, 522 };
                */

				/*
                int[] appearStamps = { 1645, 2188, 2646, 3152 };

                for (int i = 0; i < 4; i++)
                {
                    int x = TJAPlayer3.Skin.DaniSelect_Dan_Text_X[i];
                    int y = TJAPlayer3.Skin.DaniSelect_Dan_Text_Y[i];

                    if (stamp < appearStamps[i])
                        break;

                    TJAPlayer3.Tx.Dani_Dan_Text.Opacity = Math.Min(255, stamp - appearStamps[i]);

                    float ratio = (255 - TJAPlayer3.Tx.Dani_Dan_Text.Opacity) / 400f + 1f;

                    TJAPlayer3.Tx.Dani_Dan_Text.vc拡大縮小倍率.X = ratio;
                    TJAPlayer3.Tx.Dani_Dan_Text.vc拡大縮小倍率.Y = ratio;

                    TJAPlayer3.Tx.Dani_Dan_Text.t2D拡大率考慮中央基準描画(x, y,
                        new Rectangle(quarter * i, 0, quarter, TJAPlayer3.Tx.Dani_Dan_Text.szテクスチャサイズ.Height));
                }
                */

				#endregion
			}

			#endregion
		} else if (stamp == 6000) {
			if (!ctChara_In.IsStarted) {
				//TJAPlayer3.Skin.soundDanSelectStart.t再生する();
				OpenNijiiroRW.Skin.voiceMenuDanSelectStart[OpenNijiiroRW.SaveFile]?.tPlay();
				OpenNijiiroRW.Skin.soundDanSelectBGM.tPlay();
				ctChara_In.Start(0, 180, 1.25f, OpenNijiiroRW.Timer);
			}

			OpenNijiiroRW.NamePlate.tNamePlateDraw(OpenNijiiroRW.Skin.SongSelect_NamePlate_X[0], OpenNijiiroRW.Skin.SongSelect_NamePlate_Y[0], 0);
			ModIcons.tDisplayModsMenu(OpenNijiiroRW.Skin.SongSelect_ModIcons_X[0], OpenNijiiroRW.Skin.SongSelect_ModIcons_Y[0], 0); ;

			#region [ キー関連 ]

			if (!this.段位リスト.bスクロール中 && !b選択した && !bDifficultyIn) {
				int returnTitle() {
					OpenNijiiroRW.Skin.soundDanSelectBGM.tStop();
					OpenNijiiroRW.Skin.soundCancelSFX.tPlay();
					this.eフェードアウト完了時の戻り値 = CStageSongSelect.EReturnValue.BackToTitle;
					this.actFOtoTitle.tフェードアウト開始();
					base.ePhaseID = CStage.EPhase.Common_FADEOUT;
					return 0;
				}

				if (OpenNijiiroRW.InputManager.Keyboard.KeyPressing((int)SlimDXKeys.Key.RightArrow) ||
					OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.RightChange)) {
					this.段位リスト.t右に移動();
				}

				if (OpenNijiiroRW.InputManager.Keyboard.KeyPressing((int)SlimDXKeys.Key.LeftArrow) ||
					OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.LeftChange)) {
					this.段位リスト.t左に移動();
				}

				if (OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Return) ||
					OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.Decide)) {
					switch (段位リスト.currentBar.nodeType) {
						case CSongListNode.ENodeType.SCORE:
						case CSongListNode.ENodeType.RANDOM: {
								//this.t段位を選択する();
								//TJAPlayer3.Skin.soundDanSongSelectCheck.t再生する();
								OpenNijiiroRW.Skin.voiceMenuDanSelectPrompt[OpenNijiiroRW.SaveFile]?.tPlay();
								this.bDifficultyIn = true;
								this.段位挑戦選択画面.ctBarIn.Start(0, 255, 1, OpenNijiiroRW.Timer);
							}
							break;
						case CSongListNode.ENodeType.BOX: {
								OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
								段位リスト.tOpenFolder(段位リスト.currentBar);
							}
							break;
						case CSongListNode.ENodeType.BACKBOX: {
								if (OpenNijiiroRW.Songs管理.list曲ルート.Contains(段位リスト.currentBar.rParentNode) && 段位リスト.currentBar.rParentNode.songGenre == "段位道場") {
									return returnTitle();
								} else {
									OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
									段位リスト.tCloseFolder(段位リスト.currentBar);
								}
							}
							break;
					}
				}

				if (OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Escape) ||
					OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.Cancel)) {
					this.段位リスト.n現在の選択行 = 0;
					return returnTitle();
				}
			}

			#endregion

			#region [ キャラ関連 ]

			if (ctChara_In.CurrentValue != 90) {
				float CharaX = 0f, CharaY = 0f;

				CharaX = (float)Math.Sin(ctChara_In.CurrentValue / 2 * (Math.PI / 180)) * 200f;
				CharaY = ((float)Math.Sin((90 + (ctChara_In.CurrentValue / 2)) * (Math.PI / 180)) * 150f);

				// TJAPlayer3.Tx.SongSelect_Chara_Normal[ctChara_Normal.n現在の値].Opacity = ctChara_In.n現在の値 * 2;
				// TJAPlayer3.Tx.SongSelect_Chara_Normal[ctChara_Normal.n現在の値].t2D描画(-200 + CharaX, 336 - CharaY);

				//CMenuCharacter.tMenuDisplayCharacter(0, (int)(-200 + CharaX), (int)(336 - CharaY), CMenuCharacter.ECharacterAnimation.NORMAL);

				int chara_x = OpenNijiiroRW.Skin.SongSelect_NamePlate_X[0] + OpenNijiiroRW.Tx.NamePlateBase.szTextureSize.Width / 2;
				int chara_y = OpenNijiiroRW.Skin.SongSelect_NamePlate_Y[0];

				CMenuCharacter.tMenuDisplayCharacter(
					0,
					chara_x,
					chara_y,
					CMenuCharacter.ECharacterAnimation.NORMAL);

				#region [PuchiChara]

				int puchi_x = chara_x + OpenNijiiroRW.Skin.Adjustments_MenuPuchichara_X[0];
				int puchi_y = chara_y + OpenNijiiroRW.Skin.Adjustments_MenuPuchichara_Y[0];

				//this.PuchiChara.On進行描画(0 + 100, 336 + 230, false);
				this.PuchiChara.On進行描画(puchi_x, puchi_y, false);

				#endregion
			}

			#endregion

			this.段位挑戦選択画面.Draw();
		}

		this.actPlayOption.On進行描画(1, [this.段位挑戦選択画面.bOption]);

		if (ct待機.CurrentValue >= 3000) {
			if (段位リスト.currentBar.nodeType == CSongListNode.ENodeType.RANDOM) {
				if (!tSelectSongRandomly()) {
					bDifficultyIn = false;
					b選択した = false;
					OpenNijiiroRW.Skin.soundError.tPlay();
				}
			} else {
				OpenNijiiroRW.stageDanSongSelect.t段位を選択する();
			}
			ct待機.CurrentValue = 0;
			ct待機.Stop();
		}

		switch (base.ePhaseID) {
			case CStage.EPhase.SongSelect_FadeOutToNowLoading:
				if (this.actFOtoNowLoading.Draw() == 0) {
					break;
				}
				return (int)this.eフェードアウト完了時の戻り値;

			case CStage.EPhase.Common_FADEOUT:
				if (this.actFOtoTitle.Draw() == 0) {
					break;
				}
				return (int)this.eフェードアウト完了時の戻り値;

		}

		return 0;
	}

	public void t段位を選択する() {
		this.b選択した = true;
		OpenNijiiroRW.stageSongSelect.rChoosenSong = 段位リスト.listSongs[段位リスト.n現在の選択行];
		OpenNijiiroRW.stageSongSelect.r確定されたスコア = 段位リスト.listSongs[段位リスト.n現在の選択行].score[(int)Difficulty.Dan];
		OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] = (int)Difficulty.Dan;
		OpenNijiiroRW.stageSongSelect.str確定された曲のジャンル = 段位リスト.listSongs[段位リスト.n現在の選択行].songGenre;
		if ((OpenNijiiroRW.stageSongSelect.rChoosenSong != null) && (OpenNijiiroRW.stageSongSelect.r確定されたスコア != null)) {
			this.eフェードアウト完了時の戻り値 = CStageSongSelect.EReturnValue.SongSelected;
			this.actFOtoNowLoading.tフェードアウト開始();                // #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
			base.ePhaseID = CStage.EPhase.SongSelect_FadeOutToNowLoading;
		}
		// TJAPlayer3.Skin.bgm選曲画面.t停止する();
		CSongSelectSongManager.stopSong();
	}

	private bool tSelectSongRandomly() {
		this.b選択した = true;
		var mandatoryDiffs = new List<int>();
		CSongListNode song = 段位リスト.currentBar;

		List<CSongListNode> songs = new List<CSongListNode>();
		OpenNijiiroRW.stageSongSelect.t指定された曲の子リストの曲を列挙する_孫リスト含む(song.rParentNode, ref songs, ref mandatoryDiffs, true);
		song.randomList = songs;

		int selectableSongCount = song.randomList.Count;

		if (selectableSongCount == 0) {
			return false;
		}

		int randomSongIndex = OpenNijiiroRW.Random.Next(selectableSongCount);

		if (OpenNijiiroRW.ConfigIni.bOutputDetailedDTXLog) {
			StringBuilder builder = new StringBuilder(0x400);
			builder.Append(string.Format("Total number of songs to randomly choose from {0}. Randomly selected index {0}.", selectableSongCount, randomSongIndex));
			Trace.TraceInformation(builder.ToString());
		}

		// Third assignment
		OpenNijiiroRW.stageSongSelect.rChoosenSong = song.randomList[randomSongIndex];
		OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] = (int)Difficulty.Dan;

		OpenNijiiroRW.stageSongSelect.r確定されたスコア = OpenNijiiroRW.stageSongSelect.rChoosenSong.score[OpenNijiiroRW.stageSongSelect.actSongList.n現在のアンカ難易度レベルに最も近い難易度レベルを返す(OpenNijiiroRW.stageSongSelect.rChoosenSong)];
		OpenNijiiroRW.stageSongSelect.str確定された曲のジャンル = OpenNijiiroRW.stageSongSelect.rChoosenSong.songGenre;

		//TJAPlayer3.Skin.sound曲決定音.t再生する();

		this.eフェードアウト完了時の戻り値 = CStageSongSelect.EReturnValue.SongSelected;
		this.actFOtoNowLoading.tフェードアウト開始();                    // #27787 2012.3.10 yyagi 曲決定時の画面フェードアウトの省略
		base.ePhaseID = CStage.EPhase.SongSelect_FadeOutToNowLoading;

		CSongSelectSongManager.stopSong();

		return true;
	}

	private ScriptBG Background;

	public CCounter ct待機;

	public bool b選択した;
	public bool bDifficultyIn;

	public bool bInSongPlayed;

	private CCounter ctChara_In;
	// private CCounter ctChara_Normal;

	private PuchiChara PuchiChara;

	public CStageSongSelect.EReturnValue eフェードアウト完了時の戻り値;

	public CActFIFOStart actFOtoNowLoading;
	public CActFIFOBlack actFOtoTitle;
	public CActSelect段位リスト 段位リスト;
	public CActSelect段位挑戦選択画面 段位挑戦選択画面;
	public CActPlayOption actPlayOption;
}
