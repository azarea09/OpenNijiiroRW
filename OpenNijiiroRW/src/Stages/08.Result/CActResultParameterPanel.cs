using System.Drawing;
using System.Runtime.InteropServices;
using FDK;

namespace OpenNijiiroRW;

internal class CActResultParameterPanel : CActivity
{
	// Constructor

	public CActResultParameterPanel()
	{
		ST文字位置[] st文字位置Array = new ST文字位置[11];
		ST文字位置 st文字位置 = new ST文字位置();
		st文字位置.ch = '0';
		st文字位置.pt = new Point(0, 0);
		st文字位置Array[0] = st文字位置;
		ST文字位置 st文字位置2 = new ST文字位置();
		st文字位置2.ch = '1';
		st文字位置2.pt = new Point(32, 0);
		st文字位置Array[1] = st文字位置2;
		ST文字位置 st文字位置3 = new ST文字位置();
		st文字位置3.ch = '2';
		st文字位置3.pt = new Point(64, 0);
		st文字位置Array[2] = st文字位置3;
		ST文字位置 st文字位置4 = new ST文字位置();
		st文字位置4.ch = '3';
		st文字位置4.pt = new Point(96, 0);
		st文字位置Array[3] = st文字位置4;
		ST文字位置 st文字位置5 = new ST文字位置();
		st文字位置5.ch = '4';
		st文字位置5.pt = new Point(128, 0);
		st文字位置Array[4] = st文字位置5;
		ST文字位置 st文字位置6 = new ST文字位置();
		st文字位置6.ch = '5';
		st文字位置6.pt = new Point(160, 0);
		st文字位置Array[5] = st文字位置6;
		ST文字位置 st文字位置7 = new ST文字位置();
		st文字位置7.ch = '6';
		st文字位置7.pt = new Point(192, 0);
		st文字位置Array[6] = st文字位置7;
		ST文字位置 st文字位置8 = new ST文字位置();
		st文字位置8.ch = '7';
		st文字位置8.pt = new Point(224, 0);
		st文字位置Array[7] = st文字位置8;
		ST文字位置 st文字位置9 = new ST文字位置();
		st文字位置9.ch = '8';
		st文字位置9.pt = new Point(256, 0);
		st文字位置Array[8] = st文字位置9;
		ST文字位置 st文字位置10 = new ST文字位置();
		st文字位置10.ch = '9';
		st文字位置10.pt = new Point(288, 0);
		st文字位置Array[9] = st文字位置10;
		ST文字位置 st文字位置11 = new ST文字位置();
		st文字位置11.ch = ' ';
		st文字位置11.pt = new Point(0, 0);
		st文字位置Array[10] = st文字位置11;
		this.st小文字位置 = st文字位置Array;

		ST文字位置[] st文字位置Array2 = new ST文字位置[11];
		ST文字位置 st文字位置12 = new ST文字位置();
		st文字位置12.ch = '0';
		st文字位置12.pt = new Point(0, 0);
		st文字位置Array2[0] = st文字位置12;
		ST文字位置 st文字位置13 = new ST文字位置();
		st文字位置13.ch = '1';
		st文字位置13.pt = new Point(32, 0);
		st文字位置Array2[1] = st文字位置13;
		ST文字位置 st文字位置14 = new ST文字位置();
		st文字位置14.ch = '2';
		st文字位置14.pt = new Point(64, 0);
		st文字位置Array2[2] = st文字位置14;
		ST文字位置 st文字位置15 = new ST文字位置();
		st文字位置15.ch = '3';
		st文字位置15.pt = new Point(96, 0);
		st文字位置Array2[3] = st文字位置15;
		ST文字位置 st文字位置16 = new ST文字位置();
		st文字位置16.ch = '4';
		st文字位置16.pt = new Point(128, 0);
		st文字位置Array2[4] = st文字位置16;
		ST文字位置 st文字位置17 = new ST文字位置();
		st文字位置17.ch = '5';
		st文字位置17.pt = new Point(160, 0);
		st文字位置Array2[5] = st文字位置17;
		ST文字位置 st文字位置18 = new ST文字位置();
		st文字位置18.ch = '6';
		st文字位置18.pt = new Point(192, 0);
		st文字位置Array2[6] = st文字位置18;
		ST文字位置 st文字位置19 = new ST文字位置();
		st文字位置19.ch = '7';
		st文字位置19.pt = new Point(224, 0);
		st文字位置Array2[7] = st文字位置19;
		ST文字位置 st文字位置20 = new ST文字位置();
		st文字位置20.ch = '8';
		st文字位置20.pt = new Point(256, 0);
		st文字位置Array2[8] = st文字位置20;
		ST文字位置 st文字位置21 = new ST文字位置();
		st文字位置21.ch = '9';
		st文字位置21.pt = new Point(288, 0);
		st文字位置Array2[9] = st文字位置21;
		ST文字位置 st文字位置22 = new ST文字位置();
		st文字位置22.ch = '%';
		st文字位置22.pt = new Point(0x37, 0);
		st文字位置Array2[10] = st文字位置22;
		this.st大文字位置 = st文字位置Array2;

		ST文字位置[] stScore文字位置Array = new ST文字位置[10];
		ST文字位置 stScore文字位置 = new ST文字位置();
		stScore文字位置.ch = '0';
		stScore文字位置.pt = new Point(0, 0);
		stScore文字位置Array[0] = stScore文字位置;
		ST文字位置 stScore文字位置2 = new ST文字位置();
		stScore文字位置2.ch = '1';
		stScore文字位置2.pt = new Point(51, 0);
		stScore文字位置Array[1] = stScore文字位置2;
		ST文字位置 stScore文字位置3 = new ST文字位置();
		stScore文字位置3.ch = '2';
		stScore文字位置3.pt = new Point(102, 0);
		stScore文字位置Array[2] = stScore文字位置3;
		ST文字位置 stScore文字位置4 = new ST文字位置();
		stScore文字位置4.ch = '3';
		stScore文字位置4.pt = new Point(153, 0);
		stScore文字位置Array[3] = stScore文字位置4;
		ST文字位置 stScore文字位置5 = new ST文字位置();
		stScore文字位置5.ch = '4';
		stScore文字位置5.pt = new Point(204, 0);
		stScore文字位置Array[4] = stScore文字位置5;
		ST文字位置 stScore文字位置6 = new ST文字位置();
		stScore文字位置6.ch = '5';
		stScore文字位置6.pt = new Point(255, 0);
		stScore文字位置Array[5] = stScore文字位置6;
		ST文字位置 stScore文字位置7 = new ST文字位置();
		stScore文字位置7.ch = '6';
		stScore文字位置7.pt = new Point(306, 0);
		stScore文字位置Array[6] = stScore文字位置7;
		ST文字位置 stScore文字位置8 = new ST文字位置();
		stScore文字位置8.ch = '7';
		stScore文字位置8.pt = new Point(357, 0);
		stScore文字位置Array[7] = stScore文字位置8;
		ST文字位置 stScore文字位置9 = new ST文字位置();
		stScore文字位置9.ch = '8';
		stScore文字位置9.pt = new Point(408, 0);
		stScore文字位置Array[8] = stScore文字位置9;
		ST文字位置 stScore文字位置10 = new ST文字位置();
		stScore文字位置10.ch = '9';
		stScore文字位置10.pt = new Point(459, 0);
		stScore文字位置Array[9] = stScore文字位置10;
		this.stScoreFont = stScore文字位置Array;

		base.ChildActivities.Add(this.PuchiChara = new PuchiChara());

		this.ptFullCombo位置 = new Point[] { new Point(0x80, 0xed), new Point(0xdf, 0xed), new Point(0x141, 0xed) };
		base.IsDeActivated = true;
	}


	// メソッド

	public void tアニメを完了させる()
	{
		this.ct表示用.CurrentValue = (int)this.ct表示用.EndValue;
	}


	public void tSkipResultAnimations()
	{
		OpenNijiiroRW.stageResults.Background.SkipAnimation();
		ctMainCounter.CurrentValue = (int)MountainAppearValue;

		for (int i = 0; i < b音声再生.Length; i++)
		{
			b音声再生[i] = true;
		}

		for (int i = 0; i < 5; i++)
		{
			if (!ctゲージアニメ[i].IsTicked)
				ctゲージアニメ[i].Start(0, gaugeValues[i] / 2, 59, OpenNijiiroRW.Timer);
			ctゲージアニメ[i].CurrentValue = (int)ctゲージアニメ[i].EndValue;
		}

		OpenNijiiroRW.Skin.soundGauge.tStop();
	}

	// CActivity 実装

	public override void Activate()
	{
		this.sdDTXで指定されたフルコンボ音 = null;

		ttkAISection = new TitleTextureKey[OpenNijiiroRW.stageGameScreen.AIBattleSections.Count];
		for (int i = 0; i < ttkAISection.Length; i++)
		{
			ttkAISection[i] = new TitleTextureKey(CLangManager.LangInstance.GetString("AI_SECTION", i + 1), pfAISectionText, Color.White, Color.Black, 1280);

		}

		for (int i = 0; i < 5; i++)
		{
			ttkSpeechText[i] = new TitleTextureKey[6];

			int _charaId = OpenNijiiroRW.SaveFileInstances[OpenNijiiroRW.GetActualPlayer(i)].data.Character;

			for (int j = 0; j < 6; j++)
			{
				// { "simplestyleSweat", "...", "○", "◎", "★", "!!!!" }
				ttkSpeechText[i][j] = new TitleTextureKey(
					OpenNijiiroRW.Tx.Characters[_charaId].metadata.SpeechText[j].GetString(""),
					pfSpeechText, Color.White, Color.Black, OpenNijiiroRW.Skin.Result_Speech_Text_MaxWidth);
			}
		}

		ctMainCounter = new CCounter(0, 50000, 1, OpenNijiiroRW.Timer);

		ctゲージアニメ = new CCounter[5];
		for (int i = 0; i < 5; i++)
			ctゲージアニメ[i] = new CCounter();

		ct虹ゲージアニメ = new CCounter();

		ctSoul = new CCounter();

		ctEndAnime = new CCounter();
		ctBackgroundAnime = new CCounter(0, 1000, 1, OpenNijiiroRW.Timer);
		ctBackgroundAnime_Clear = new CCounter(0, 1000, 1, OpenNijiiroRW.Timer);
		ctMountain_ClearIn = new CCounter();

		RandomText = OpenNijiiroRW.Random.Next(3);

		ctFlash_Icon = new CCounter(0, 3000, 1, OpenNijiiroRW.Timer);
		ctRotate_Flowers = new CCounter(0, 1500, 1, OpenNijiiroRW.Timer);
		ctShine_Plate = new CCounter(0, 1000, 1, OpenNijiiroRW.Timer);

		ctAISectionChange = new CCounter(0, 2000, 1, OpenNijiiroRW.Timer);
		ctAISectionChange.CurrentValue = 255;

		ctUIMove = new CCounter();

		for (int i = 0; i < 5; i++)
		{
			CResultCharacter.tMenuResetTimer(CResultCharacter.ECharacterResult.NORMAL);
			CResultCharacter.tDisableCounter(CResultCharacter.ECharacterResult.CLEAR);
			CResultCharacter.tDisableCounter(CResultCharacter.ECharacterResult.FAILED);
			CResultCharacter.tDisableCounter(CResultCharacter.ECharacterResult.FAILED_IN);
		}

		gaugeValues = new int[5];
		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			gaugeValues[i] = (int)OpenNijiiroRW.stageGameScreen.actGauge.db現在のゲージ値[i];
		}

		// Replace by max between 2 gauges if 2p
		GaugeFactor = Math.Max(Math.Max(Math.Max(Math.Max(gaugeValues[0], gaugeValues[1]), gaugeValues[2]), gaugeValues[3]), gaugeValues[4]) / 2;

		MountainAppearValue = 10275 + (66 * GaugeFactor);

		this.PuchiChara.IdleAnimation();

		base.Activate();
	}
	public override void DeActivate()
	{
		if (this.ct表示用 != null)
		{
			this.ct表示用 = null;
		}

		for (int i = 0; i < this.b音声再生.Length; i++)
		{
			b音声再生[i] = false;
		}

		if (this.sdDTXで指定されたフルコンボ音 != null)
		{
			OpenNijiiroRW.SoundManager.tDisposeSound(this.sdDTXで指定されたフルコンボ音);
			this.sdDTXで指定されたフルコンボ音 = null;
		}
		base.DeActivate();
	}
	public override void CreateManagedResource()
	{
		pfSpeechText = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.Result_Speech_Text_Size);
		pfAISectionText = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.Result_AIBattle_SectionText_Scale);

		base.CreateManagedResource();
	}
	public override void ReleaseManagedResource()
	{
		OpenNijiiroRW.tDisposeSafely(ref pfSpeechText);
		OpenNijiiroRW.tDisposeSafely(ref pfAISectionText);

		base.ReleaseManagedResource();
	}
	public override int Draw()
	{
		if (base.IsDeActivated)
		{
			return 0;
		}
		if (base.IsFirstDraw)
		{
			this.ct表示用 = new CCounter(0, 0x3e7, 2, OpenNijiiroRW.Timer);
			base.IsFirstDraw = false;
		}
		this.ct表示用.Tick();

		ctMainCounter.Tick();

		for (int i = 0; i < 5; i++)
			ctゲージアニメ[i].Tick();

		ctEndAnime.Tick();
		ctBackgroundAnime.TickLoop();
		ctMountain_ClearIn.Tick();

		ctFlash_Icon.TickLoop();
		ctRotate_Flowers.TickLoop();
		ctShine_Plate.TickLoop();

		ctAISectionChange.Tick();

		// this.PuchiChara.IdleAnimation();

		int nDrawnPlayers = OpenNijiiroRW.ConfigIni.bAIBattleMode ? 1 : OpenNijiiroRW.ConfigIni.nPlayerCount;
		if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Dan && OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Tower)
		{
			int[] namePlate_x = new int[5];
			int[] namePlate_y = new int[5];

			for (int i = 0; i < nDrawnPlayers; i++)
			{
				if (nDrawnPlayers == 5)
				{
					namePlate_x[i] = OpenNijiiroRW.Skin.Result_NamePlate_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[i];
					namePlate_y[i] = OpenNijiiroRW.Skin.Result_NamePlate_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[i];
				}
				else if (nDrawnPlayers == 4 || nDrawnPlayers == 3)
				{
					namePlate_x[i] = OpenNijiiroRW.Skin.Result_NamePlate_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[i];
					namePlate_y[i] = OpenNijiiroRW.Skin.Result_NamePlate_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[i];
				}
				else
				{
					int pos = i;
					if (OpenNijiiroRW.P1IsBlue())
						pos = 1;
					namePlate_x[pos] = OpenNijiiroRW.Skin.Result_NamePlate_X[pos];
					namePlate_y[pos] = OpenNijiiroRW.Skin.Result_NamePlate_Y[pos];
				}
			}

			ctUIMove.Tick();

			#region [ Ensou result contents ]

			int AnimeCount = 3000 + GaugeFactor * 59;
			int ScoreApparitionTimeStamp = AnimeCount + 420 * 4 + 840;

			bool is1P = (nDrawnPlayers == 1);
			bool is2PSide = OpenNijiiroRW.P1IsBlue();

			int shift = 635;

			int uioffset_x = 0;
			double uioffset_value = Math.Sin((ctUIMove.CurrentValue / 1000.0) * Math.PI / 2.0);
			if (is1P)
			{
				uioffset_x = (int)(uioffset_value * OpenNijiiroRW.Skin.Resolution[0] / 2.0);
				if (is2PSide) uioffset_x *= -1;
			}

			for (int i = 0; i < nDrawnPlayers; i++)
			{
				// 1 if right, 0 if left
				int shiftPos = (i == 1 || is2PSide) ? 1 : i;
				int pos = i;
				if (is2PSide)
					pos = 1;


				#region [General plate animations]

				if (nDrawnPlayers <= 2)
				{
					if (shiftPos == 0)
						OpenNijiiroRW.Tx.Result_Panel.t2D描画(0 + uioffset_x, 0);
					else
						OpenNijiiroRW.Tx.Result_Panel_2P.t2D描画(0 + uioffset_x, 0);
				}
				else
				{
					if (nDrawnPlayers == 5)
					{
						OpenNijiiroRW.Tx.Result_Panel_5P[i].t2D描画(OpenNijiiroRW.Skin.Result_UIMove_5P_X[i], OpenNijiiroRW.Skin.Result_UIMove_5P_Y[i]);
					}
					else
					{
						OpenNijiiroRW.Tx.Result_Panel_4P[i].t2D描画(OpenNijiiroRW.Skin.Result_UIMove_4P_X[i], OpenNijiiroRW.Skin.Result_UIMove_4P_Y[i]);
					}
				}

				//if (TJAPlayer3.ConfigIni.nPlayerCount <= 2)
				var _frame = OpenNijiiroRW.Tx.Result_Gauge_Frame;
				if (_frame != null)
				{
					int bar_x;
					int bar_y;
					int gauge_base_x;
					int gauge_base_y;


					if (nDrawnPlayers == 5)
					{
						_frame.Scale.X = 0.5f;
						bar_x = OpenNijiiroRW.Skin.Result_DifficultyBar_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
						bar_y = OpenNijiiroRW.Skin.Result_DifficultyBar_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];
						gauge_base_x = OpenNijiiroRW.Skin.Result_Gauge_Base_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
						gauge_base_y = OpenNijiiroRW.Skin.Result_Gauge_Base_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];
					}
					else if (nDrawnPlayers == 4 || nDrawnPlayers == 3)
					{
						_frame.Scale.X = 0.5f;
						bar_x = OpenNijiiroRW.Skin.Result_DifficultyBar_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
						bar_y = OpenNijiiroRW.Skin.Result_DifficultyBar_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];
						gauge_base_x = OpenNijiiroRW.Skin.Result_Gauge_Base_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
						gauge_base_y = OpenNijiiroRW.Skin.Result_Gauge_Base_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];
					}
					else
					{
						_frame.Scale.X = 1.0f;
						bar_x = OpenNijiiroRW.Skin.Result_DifficultyBar_X[pos] + uioffset_x;
						bar_y = OpenNijiiroRW.Skin.Result_DifficultyBar_Y[pos];
						gauge_base_x = OpenNijiiroRW.Skin.Result_Gauge_Base_X[pos] + uioffset_x;
						gauge_base_y = OpenNijiiroRW.Skin.Result_Gauge_Base_Y[pos];
					}

					OpenNijiiroRW.Tx.Result_Diff_Bar.t2D描画(bar_x, bar_y,
						new RectangleF(0, OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i] * OpenNijiiroRW.Skin.Result_DifficultyBar_Size[1], OpenNijiiroRW.Skin.Result_DifficultyBar_Size[0], OpenNijiiroRW.Skin.Result_DifficultyBar_Size[1]));

					_frame.t2D描画(gauge_base_x, gauge_base_y);
					_frame.Scale.X = 1.0f;
				}

				if (ctMainCounter.CurrentValue >= 2000)
				{
					#region [ Gauge updates ]

					if (!b音声再生[0])
					{
						OpenNijiiroRW.Skin.soundGauge.tPlay();
						b音声再生[0] = true;
					}

					// Split gauge counter, one for each player in two
					if (!ctゲージアニメ[i].IsTicked)
					{
						ctゲージアニメ[i].Start(0, gaugeValues[i] / 2, 59, OpenNijiiroRW.Timer);
						if (ctMainCounter.CurrentValue >= MountainAppearValue)
							ctゲージアニメ[i].CurrentValue = (int)ctゲージアニメ[i].EndValue;
					}


					if (ctゲージアニメ[i].IsEnded)
					{
						if (ctゲージアニメ[i].CurrentValue != 50)
						{
							// Gauge didn't reach rainbow
							if (nDrawnPlayers < 2
								|| ctゲージアニメ[(i == 0) ? 1 : 0].IsEnded)
								OpenNijiiroRW.Skin.soundGauge.tStop();
						}
						else
						{
							// Gauge reached rainbow
							if (!OpenNijiiroRW.Skin.soundGauge.bIsPlaying)
							{
								OpenNijiiroRW.Skin.soundGauge.tStop();
							}

							if (!ct虹ゲージアニメ.IsTicked)
								ct虹ゲージアニメ.Start(0, OpenNijiiroRW.Skin.Result_Gauge_Rainbow_Ptn - 1, OpenNijiiroRW.Skin.Result_Gauge_Rainbow_Interval, OpenNijiiroRW.Timer);

							if (!ctSoul.IsTicked)
								ctSoul.Start(0, 8, 33, OpenNijiiroRW.Timer);

							ct虹ゲージアニメ.TickLoop();
							ctSoul.TickLoop();
						}
					}

					HGaugeMethods.UNSAFE_DrawResultGaugeFast(i, shiftPos, pos, ctゲージアニメ[i].CurrentValue, ct虹ゲージアニメ.CurrentValue, ctSoul.CurrentValue, uioffset_x);

					#endregion
				}

				if (ctMainCounter.CurrentValue >= 2000)
				{
					// Change score kiroku to total scores to have the contents for both players, unbloat it
					{
						#region [ Separate results display (excluding score) ]

						int Interval = 420;

						float AddCount = 135;

						int[] scoresArr =
						{
							OpenNijiiroRW.stageGameScreen.CChartScore[i].nGreat,
							OpenNijiiroRW.stageGameScreen.CChartScore[i].nGood,
							OpenNijiiroRW.stageGameScreen.CChartScore[i].nMiss,
							OpenNijiiroRW.stageGameScreen.CChartScore[i].nRoll,
							OpenNijiiroRW.stageGameScreen.actCombo.nCurrentCombo.最高値[i],
							OpenNijiiroRW.stageGameScreen.CChartScore[i].nADLIB,
							OpenNijiiroRW.stageGameScreen.CChartScore[i].nMine,
						};

						int[][] num_x;

						int[][] num_y;
						if (nDrawnPlayers == 5)
						{
							num_x = new int[][] { new int[5], new int[5], new int[5], new int[5], new int[5], new int[5], new int[5] };
							num_y = new int[][] { new int[5], new int[5], new int[5], new int[5], new int[5], new int[5], new int[5] };

							num_x[0][pos] = OpenNijiiroRW.Skin.Result_Perfect_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
							num_y[0][pos] = OpenNijiiroRW.Skin.Result_Perfect_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];

							num_x[1][pos] = OpenNijiiroRW.Skin.Result_Good_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
							num_y[1][pos] = OpenNijiiroRW.Skin.Result_Good_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];

							num_x[2][pos] = OpenNijiiroRW.Skin.Result_Miss_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
							num_y[2][pos] = OpenNijiiroRW.Skin.Result_Miss_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];

							num_x[3][pos] = OpenNijiiroRW.Skin.Result_Roll_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
							num_y[3][pos] = OpenNijiiroRW.Skin.Result_Roll_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];

							num_x[4][pos] = OpenNijiiroRW.Skin.Result_MaxCombo_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
							num_y[4][pos] = OpenNijiiroRW.Skin.Result_MaxCombo_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];

							num_x[5][pos] = OpenNijiiroRW.Skin.Result_ADLib_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
							num_y[5][pos] = OpenNijiiroRW.Skin.Result_ADLib_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];

							num_x[6][pos] = OpenNijiiroRW.Skin.Result_Bomb_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
							num_y[6][pos] = OpenNijiiroRW.Skin.Result_Bomb_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];
						}
						else if (nDrawnPlayers > 2)
						{
							num_x = new int[][] { new int[5], new int[5], new int[5], new int[5], new int[5], new int[5], new int[5] };
							num_y = new int[][] { new int[5], new int[5], new int[5], new int[5], new int[5], new int[5], new int[5] };

							num_x[0][pos] = OpenNijiiroRW.Skin.Result_Perfect_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
							num_y[0][pos] = OpenNijiiroRW.Skin.Result_Perfect_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];

							num_x[1][pos] = OpenNijiiroRW.Skin.Result_Good_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
							num_y[1][pos] = OpenNijiiroRW.Skin.Result_Good_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];

							num_x[2][pos] = OpenNijiiroRW.Skin.Result_Miss_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
							num_y[2][pos] = OpenNijiiroRW.Skin.Result_Miss_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];

							num_x[3][pos] = OpenNijiiroRW.Skin.Result_Roll_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
							num_y[3][pos] = OpenNijiiroRW.Skin.Result_Roll_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];

							num_x[4][pos] = OpenNijiiroRW.Skin.Result_MaxCombo_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
							num_y[4][pos] = OpenNijiiroRW.Skin.Result_MaxCombo_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];

							num_x[5][pos] = OpenNijiiroRW.Skin.Result_ADLib_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
							num_y[5][pos] = OpenNijiiroRW.Skin.Result_ADLib_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];

							num_x[6][pos] = OpenNijiiroRW.Skin.Result_Bomb_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
							num_y[6][pos] = OpenNijiiroRW.Skin.Result_Bomb_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];
						}
						else
						{
							num_x = new int[][] {
								OpenNijiiroRW.Skin.Result_Perfect_X,
								OpenNijiiroRW.Skin.Result_Good_X,
								OpenNijiiroRW.Skin.Result_Miss_X,
								OpenNijiiroRW.Skin.Result_Roll_X,
								OpenNijiiroRW.Skin.Result_MaxCombo_X,
								OpenNijiiroRW.Skin.Result_ADLib_X,
								OpenNijiiroRW.Skin.Result_Bomb_X
							};

							num_y = new int[][] {
								OpenNijiiroRW.Skin.Result_Perfect_Y,
								OpenNijiiroRW.Skin.Result_Good_Y,
								OpenNijiiroRW.Skin.Result_Miss_Y,
								OpenNijiiroRW.Skin.Result_Roll_Y,
								OpenNijiiroRW.Skin.Result_MaxCombo_Y,
								OpenNijiiroRW.Skin.Result_ADLib_Y,
								OpenNijiiroRW.Skin.Result_Bomb_Y
							};
						}

						for (int k = 0; k < 7; k++)
						{
							if (ctMainCounter.CurrentValue >= AnimeCount + (Interval * k))
							{
								float numScale = 1.0f;

								if (nDrawnPlayers == 5)
								{
									numScale = OpenNijiiroRW.Skin.Result_Number_Scale_5P;
								}
								else if (nDrawnPlayers == 3 || nDrawnPlayers == 4)
								{
									numScale = OpenNijiiroRW.Skin.Result_Number_Scale_4P;
								}
								OpenNijiiroRW.Tx.Result_Number.Scale.X = ctMainCounter.CurrentValue <= AnimeCount + (Interval * k) + AddCount ? 1.3f - (float)Math.Sin((ctMainCounter.CurrentValue - (AnimeCount + (Interval * k))) / (AddCount / 90) * (Math.PI / 180)) * 0.3f : 1.0f;
								OpenNijiiroRW.Tx.Result_Number.Scale.Y = ctMainCounter.CurrentValue <= AnimeCount + (Interval * k) + AddCount ? 1.3f - (float)Math.Sin((ctMainCounter.CurrentValue - (AnimeCount + (Interval * k))) / (AddCount / 90) * (Math.PI / 180)) * 0.3f : 1.0f;

								if ((k != 5 || OpenNijiiroRW.Skin.Result_ADLib_Show) && (k != 6 || OpenNijiiroRW.Skin.Result_Bomb_Show))
								{
									this.t小文字表示(num_x[k][pos] + uioffset_x, num_y[k][pos], scoresArr[k], numScale);
								}

								OpenNijiiroRW.Tx.Result_Number.Scale.X = 1f;
								OpenNijiiroRW.Tx.Result_Number.Scale.Y = 1f;

								if (!this.b音声再生[1 + k])
								{
									if ((k != 5 || OpenNijiiroRW.Skin.Result_ADLib_Show) && (k != 6 || OpenNijiiroRW.Skin.Result_Bomb_Show))
									{
										OpenNijiiroRW.Skin.soundPon.tPlay();
									}
									this.b音声再生[1 + k] = true;
								}
							}
							else
								break;
						}

						#endregion

						#region [ Score display ]

						if (ctMainCounter.CurrentValue >= AnimeCount + Interval * 4 + 840)
						{
							float numScale = 1.0f;
							int score_x;
							int score_y;
							if (nDrawnPlayers == 5)
							{
								numScale = OpenNijiiroRW.Skin.Result_Score_Scale_5P;
								score_x = OpenNijiiroRW.Skin.Result_Score_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
								score_y = OpenNijiiroRW.Skin.Result_Score_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];
							}
							else if (nDrawnPlayers == 4 || nDrawnPlayers == 3)
							{
								numScale = OpenNijiiroRW.Skin.Result_Score_Scale_4P;
								score_x = OpenNijiiroRW.Skin.Result_Score_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
								score_y = OpenNijiiroRW.Skin.Result_Score_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];
							}
							else
							{
								score_x = OpenNijiiroRW.Skin.Result_Score_X[pos] + uioffset_x;
								score_y = OpenNijiiroRW.Skin.Result_Score_Y[pos];
							}

							int AnimeCount1 = AnimeCount + Interval * 4 + 840;

							OpenNijiiroRW.Tx.Result_Score_Number.Scale.X = ctMainCounter.CurrentValue <= AnimeCount1 + 270 ? 1.0f + (float)Math.Sin((ctMainCounter.CurrentValue - AnimeCount1) / 1.5f * (Math.PI / 180)) * 0.65f :
								ctMainCounter.CurrentValue <= AnimeCount1 + 360 ? 1.0f - (float)Math.Sin((ctMainCounter.CurrentValue - AnimeCount1 - 270) * (Math.PI / 180)) * 0.1f : 1.0f;
							OpenNijiiroRW.Tx.Result_Score_Number.Scale.Y = ctMainCounter.CurrentValue <= AnimeCount1 + 270 ? 1.0f + (float)Math.Sin((ctMainCounter.CurrentValue - AnimeCount1) / 1.5f * (Math.PI / 180)) * 0.65f :
								ctMainCounter.CurrentValue <= AnimeCount1 + 360 ? 1.0f - (float)Math.Sin((ctMainCounter.CurrentValue - AnimeCount1 - 270) * (Math.PI / 180)) * 0.1f : 1.0f;

							this.tスコア文字表示(score_x, score_y, (int)OpenNijiiroRW.stageGameScreen.actScore.Get(i), numScale);// TJAPlayer3.stage演奏ドラム画面.CChartScore[i].nScore.ToString()));

							if (!b音声再生[8])
							{
								OpenNijiiroRW.Skin.soundScoreDon.tPlay();
								b音声再生[8] = true;
							}
						}

						#endregion
					}
				}

				#endregion

			}

			if (ctAISectionChange.CurrentValue == ctAISectionChange.EndValue && OpenNijiiroRW.stageGameScreen.AIBattleSections.Count > 5)
			{
				NextAISection();
			}
			else if (nNowAISection > 0 && OpenNijiiroRW.stageGameScreen.AIBattleSections.Count <= 5)
			{
				// Fix locked sections
				nNowAISection = 0;
			}

			if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
			{
				OpenNijiiroRW.Tx.Result_AIBattle_Panel_AI.t2D描画(0, 0);

				int batch_width = OpenNijiiroRW.Tx.Result_AIBattle_Batch.szTextureSize.Width / 3;
				int batch_height = OpenNijiiroRW.Tx.Result_AIBattle_Batch.szTextureSize.Height;


				for (int i = 0; i < OpenNijiiroRW.stageGameScreen.AIBattleSections.Count; i++)
				{
					int nowIndex = (i / 5);
					int drawCount = Math.Min(OpenNijiiroRW.stageGameScreen.AIBattleSections.Count - (nowIndex * 5), 5);

					int drawPos = i % 5;
					int batch_total_width = OpenNijiiroRW.Skin.Result_AIBattle_Batch_Move[0] * drawCount;

					var section = OpenNijiiroRW.stageGameScreen.AIBattleSections[i];
					int upDown = (drawPos % 2);

					int x = OpenNijiiroRW.Skin.Result_AIBattle_Batch[0] + (OpenNijiiroRW.Skin.Result_AIBattle_Batch_Move[0] * drawPos) - (batch_total_width / 2);
					int y = OpenNijiiroRW.Skin.Result_AIBattle_Batch[1] + (OpenNijiiroRW.Skin.Result_AIBattle_Batch_Move[1] * upDown);

					int opacityCounter = Math.Min(ctAISectionChange.CurrentValue, 255);

					if (nowIndex == nNowAISection)
					{
						OpenNijiiroRW.Tx.Result_AIBattle_Batch.Opacity = opacityCounter;
						OpenNijiiroRW.Tx.Result_AIBattle_SectionPlate.Opacity = opacityCounter;
						if (TitleTextureKey.ResolveTitleTexture(ttkAISection[i]) != null)
							TitleTextureKey.ResolveTitleTexture(ttkAISection[i]).Opacity = opacityCounter;
					}
					else
					{
						OpenNijiiroRW.Tx.Result_AIBattle_Batch.Opacity = 255 - opacityCounter;
						OpenNijiiroRW.Tx.Result_AIBattle_SectionPlate.Opacity = 255 - opacityCounter;
						if (TitleTextureKey.ResolveTitleTexture(ttkAISection[i]) != null)
							TitleTextureKey.ResolveTitleTexture(ttkAISection[i]).Opacity = 255 - opacityCounter;
					}

					OpenNijiiroRW.Tx.Result_AIBattle_Batch.t2D描画(x, y, new RectangleF(batch_width * 0, 0, batch_width, batch_height));

					switch (section.End)
					{
						case CStage演奏画面共通.AIBattleSection.EndType.Clear:
							OpenNijiiroRW.Tx.Result_AIBattle_Batch.t2D描画(x, y, new Rectangle(batch_width * 1, 0, batch_width, batch_height));
							break;
						case CStage演奏画面共通.AIBattleSection.EndType.Lose:
							OpenNijiiroRW.Tx.Result_AIBattle_Batch.t2D描画(x, y, new Rectangle(batch_width * 2, 0, batch_width, batch_height));
							break;
					}

					OpenNijiiroRW.Tx.Result_AIBattle_Batch.Opacity = 255;

					OpenNijiiroRW.Tx.Result_AIBattle_SectionPlate.t2D描画(x + OpenNijiiroRW.Skin.Result_AIBattle_SectionPlate_Offset[0], y + OpenNijiiroRW.Skin.Result_AIBattle_SectionPlate_Offset[1]);

					TitleTextureKey.ResolveTitleTexture(ttkAISection[i])?.t2D中心基準描画(x + OpenNijiiroRW.Skin.Result_AIBattle_SectionText_Offset[0], y + OpenNijiiroRW.Skin.Result_AIBattle_SectionText_Offset[1]);
				}

				if (ctMainCounter.CurrentValue >= MountainAppearValue)
				{
					float flagScale = 2.0f - (Math.Min(Math.Max(ctMainCounter.CurrentValue - MountainAppearValue, 0), 200) / 200.0f);

					CTexture tex = OpenNijiiroRW.stageResults.bClear[0] ? OpenNijiiroRW.Tx.Result_AIBattle_WinFlag_Clear : OpenNijiiroRW.Tx.Result_AIBattle_WinFlag_Lose;

					tex.Scale.X = flagScale;
					tex.Scale.Y = flagScale;

					tex.t2D拡大率考慮中央基準描画(OpenNijiiroRW.Skin.Result_AIBattle_WinFlag[0], OpenNijiiroRW.Skin.Result_AIBattle_WinFlag[1]);
				}
			}


			// Should be Score + 4000, to synchronize with Stage Kekka

			// MountainAppearValue = 2000 + (ctゲージアニメ.n終了値 * 66) + 8360 - 85;



			#region [Character related animations]

			for (int p = 0; p < nDrawnPlayers; p++)
			{
				int pos = p;
				if (is2PSide)
					pos = 1;

				if (ctMainCounter.CurrentValue >= MountainAppearValue)
				{
					#region [Mountain animation counter setup]

					if (!this.ctMountain_ClearIn.IsTicked)
						this.ctMountain_ClearIn.Start(0, 515, 3, OpenNijiiroRW.Timer);

					if (ctUIMove.EndValue != 1000 && OpenNijiiroRW.Skin.Result_Use1PUI && is1P) ctUIMove = new CCounter(0, 1000, 0.5, OpenNijiiroRW.Timer);

					if (OpenNijiiroRW.stageResults.bClear[p])
					{
						if (!CResultCharacter.tIsCounterProcessing(p, CResultCharacter.ECharacterResult.CLEAR))
							CResultCharacter.tMenuResetTimer(p, CResultCharacter.ECharacterResult.CLEAR);
					}
					else
					{
						if (!CResultCharacter.tIsCounterProcessing(p, CResultCharacter.ECharacterResult.FAILED_IN))
							CResultCharacter.tMenuResetTimer(p, CResultCharacter.ECharacterResult.FAILED_IN);
						else if (CResultCharacter.tIsCounterEnded(p, CResultCharacter.ECharacterResult.FAILED_IN)
								 && !CResultCharacter.tIsCounterProcessing(p, CResultCharacter.ECharacterResult.FAILED))
							CResultCharacter.tMenuResetTimer(p, CResultCharacter.ECharacterResult.FAILED);
					}


					#endregion

					/* TO DO */

					// Alter Mountain appear value/Crown appear value if no Score Rank/no Crown
				}

				#region [Character Animations]

				int _charaId = OpenNijiiroRW.SaveFileInstances[OpenNijiiroRW.GetActualPlayer(p)].data.Character;

				//int chara_x = TJAPlayer3.Skin.Characters_Result_X[_charaId][pos];
				//int chara_y = TJAPlayer3.Skin.Characters_Result_Y[_charaId][pos];

				int chara_x = namePlate_x[pos] - (OpenNijiiroRW.Skin.Characters_UseResult1P[_charaId] ? uioffset_x : 0) + OpenNijiiroRW.Tx.NamePlateBase.szTextureSize.Width / 2;
				int chara_y = namePlate_y[pos];

				int p1chara_x = is2PSide ? OpenNijiiroRW.Skin.Resolution[0] / 2 : 0;
				int p1chara_y = OpenNijiiroRW.Skin.Resolution[1] - (int)(uioffset_value * OpenNijiiroRW.Skin.Resolution[1]);
				float renderRatioX = OpenNijiiroRW.Skin.Resolution[0] / (float)OpenNijiiroRW.Skin.Characters_Resolution[_charaId][0];
				float renderRatioY = OpenNijiiroRW.Skin.Resolution[1] / (float)OpenNijiiroRW.Skin.Characters_Resolution[_charaId][1];

				if (CResultCharacter.tIsCounterProcessing(p, CResultCharacter.ECharacterResult.CLEAR))
				{
					CResultCharacter.tMenuDisplayCharacter(p, chara_x, chara_y, CResultCharacter.ECharacterResult.CLEAR, pos);

					var tex = pos == 0 ? OpenNijiiroRW.Tx.Characters_Result_Clear_1P[_charaId] : OpenNijiiroRW.Tx.Characters_Result_Clear_2P[_charaId];
					if (OpenNijiiroRW.Skin.Characters_UseResult1P[_charaId] && OpenNijiiroRW.Skin.Result_Use1PUI && tex != null)
					{
						tex.Scale.X = renderRatioX;
						tex.Scale.Y = renderRatioY;
						if (is2PSide)
						{
							tex.t2D左右反転描画(p1chara_x, p1chara_y);
						}
						else
						{
							tex.t2D描画(p1chara_x, p1chara_y);
						}
					}
				}
				else if (CResultCharacter.tIsCounterProcessing(p, CResultCharacter.ECharacterResult.FAILED))
				{
					CResultCharacter.tMenuDisplayCharacter(p, chara_x, chara_y, CResultCharacter.ECharacterResult.FAILED, pos);
					if (OpenNijiiroRW.Skin.Characters_UseResult1P[_charaId] && OpenNijiiroRW.Skin.Result_Use1PUI && OpenNijiiroRW.Tx.Characters_Result_Failed_1P[_charaId] != null)
					{
						OpenNijiiroRW.Tx.Characters_Result_Failed_1P[_charaId].Scale.X = renderRatioX;
						OpenNijiiroRW.Tx.Characters_Result_Failed_1P[_charaId].Scale.Y = renderRatioY;
						if (is2PSide)
						{
							OpenNijiiroRW.Tx.Characters_Result_Failed_1P[_charaId].t2D左右反転描画(p1chara_x, p1chara_y);
						}
						else
						{
							OpenNijiiroRW.Tx.Characters_Result_Failed_1P[_charaId].t2D描画(p1chara_x, p1chara_y);
						}
					}
				}
				else if (CResultCharacter.tIsCounterProcessing(p, CResultCharacter.ECharacterResult.FAILED_IN) && OpenNijiiroRW.Tx.Characters_Result_Failed_1P[_charaId] != null)
				{
					CResultCharacter.tMenuDisplayCharacter(p, chara_x, chara_y, CResultCharacter.ECharacterResult.FAILED_IN, pos);
					if (OpenNijiiroRW.Skin.Characters_UseResult1P[_charaId] && OpenNijiiroRW.Skin.Result_Use1PUI)
					{
						OpenNijiiroRW.Tx.Characters_Result_Failed_1P[_charaId].Scale.X = renderRatioX;
						OpenNijiiroRW.Tx.Characters_Result_Failed_1P[_charaId].Scale.Y = renderRatioY;
						if (is2PSide)
						{
							OpenNijiiroRW.Tx.Characters_Result_Failed_1P[_charaId].t2D左右反転描画(p1chara_x, p1chara_y);
						}
						else
						{
							OpenNijiiroRW.Tx.Characters_Result_Failed_1P[_charaId].t2D描画(p1chara_x, p1chara_y);
						}
					}
				}
				else
					CResultCharacter.tMenuDisplayCharacter(p, chara_x, chara_y, CResultCharacter.ECharacterResult.NORMAL, pos);

				#endregion


				#region [PuchiChara]

				int puchi_x = chara_x + OpenNijiiroRW.Skin.Adjustments_MenuPuchichara_X[nDrawnPlayers <= 2 ? pos : 0];
				int puchi_y = chara_y + OpenNijiiroRW.Skin.Adjustments_MenuPuchichara_Y[nDrawnPlayers <= 2 ? pos : 0];

				//int ttdiff = 640 - 152;
				//int ttps = 640 + ((pos == 1) ? ttdiff + 60 : -ttdiff);

				//this.PuchiChara.On進行描画(ttps, 562, false, 255, false, p);

				this.PuchiChara.On進行描画(puchi_x, puchi_y, false, 255, false, p);

				#endregion

				if (ctMainCounter.CurrentValue >= MountainAppearValue)
				{
					float AddCount = 135;

					int baseX = (pos == 1) ? 1280 - 182 : 182;
					int baseY = 602;

					#region [Cherry blossom animation]

					if (OpenNijiiroRW.stageResults.nクリア[p] >= 1 && nDrawnPlayers <= 2)
					{
						OpenNijiiroRW.Tx.Result_Flower.Scale.X = 0.6f * (ctMainCounter.CurrentValue <= MountainAppearValue + AddCount ? 1.3f - (float)Math.Sin((ctMainCounter.CurrentValue - MountainAppearValue) / (AddCount / 90) * (Math.PI / 180)) * 0.3f : 1.0f);
						OpenNijiiroRW.Tx.Result_Flower.Scale.Y = 0.6f * (ctMainCounter.CurrentValue <= MountainAppearValue + AddCount ? 1.3f - (float)Math.Sin((ctMainCounter.CurrentValue - MountainAppearValue) / (AddCount / 90) * (Math.PI / 180)) * 0.3f : 1.0f);

						int flower_width = OpenNijiiroRW.Tx.Result_Flower.szTextureSize.Width;
						int flower_height = OpenNijiiroRW.Tx.Result_Flower.szTextureSize.Height / 2;

						OpenNijiiroRW.Tx.Result_Flower.t2D拡大率考慮中央基準描画(OpenNijiiroRW.Skin.Result_Flower_X[pos], OpenNijiiroRW.Skin.Result_Flower_Y[pos],
							new Rectangle(0, 0, flower_width, flower_height));
					}

					#endregion

					#region [Cherry blossom Rotating flowers]

					if (OpenNijiiroRW.stageResults.nクリア[p] >= 1 && nDrawnPlayers <= 2)
					{
						float FlowerTime = ctRotate_Flowers.CurrentValue;

						for (int i = 0; i < 5; i++)
						{

							if ((int)FlowerTime < ApparitionTimeStamps[i] || (int)FlowerTime > ApparitionTimeStamps[i] + 2 * ApparitionFade + ApparitionDuration)
								OpenNijiiroRW.Tx.Result_Flower_Rotate[i].Opacity = 0;
							else if ((int)FlowerTime <= ApparitionTimeStamps[i] + ApparitionDuration + ApparitionFade && (int)FlowerTime >= ApparitionTimeStamps[i] + ApparitionFade)
								OpenNijiiroRW.Tx.Result_Flower_Rotate[i].Opacity = 255;
							else
							{
								int CurrentGradiant = 0;
								if ((int)FlowerTime >= ApparitionTimeStamps[i] + ApparitionFade + ApparitionDuration)
									CurrentGradiant = ApparitionFade - ((int)FlowerTime - ApparitionTimeStamps[i] - ApparitionDuration - ApparitionFade);
								else
									CurrentGradiant = (int)FlowerTime - ApparitionTimeStamps[i];


								OpenNijiiroRW.Tx.Result_Flower_Rotate[i].Opacity = (255 * CurrentGradiant) / ApparitionFade;
							}

							OpenNijiiroRW.Tx.Result_Flower_Rotate[i].Scale.X = 0.6f;
							OpenNijiiroRW.Tx.Result_Flower_Rotate[i].Scale.Y = 0.6f;
							OpenNijiiroRW.Tx.Result_Flower_Rotate[i].Rotation = (float)(FlowerTime - ApparitionTimeStamps[i]) / (FlowerRotationSpeeds[i] * 360f);

							OpenNijiiroRW.Tx.Result_Flower_Rotate[i].t2D中心基準描画(OpenNijiiroRW.Skin.Result_Flower_Rotate_X[pos][i], OpenNijiiroRW.Skin.Result_Flower_Rotate_Y[pos][i]);
						}

					}

					#endregion

					#region [Panel shines]

					if (OpenNijiiroRW.stageResults.nクリア[p] >= 1 && nDrawnPlayers <= 2)
					{
						int ShineTime = (int)ctShine_Plate.CurrentValue;
						int Quadrant500 = ShineTime % 500;

						for (int i = 0; i < OpenNijiiroRW.Skin.Result_PlateShine_Count; i++)
						{
							if (i < 3 && ShineTime >= 500 || i >= 3 && ShineTime < 500)
								OpenNijiiroRW.Tx.Result_Shine.Opacity = 0;
							else if (Quadrant500 >= ShinePFade && Quadrant500 <= 500 - ShinePFade)
								OpenNijiiroRW.Tx.Result_Shine.Opacity = 255;
							else
								OpenNijiiroRW.Tx.Result_Shine.Opacity = (255 * Math.Min(Quadrant500, 500 - Quadrant500)) / ShinePFade;

							OpenNijiiroRW.Tx.Result_Shine.Scale.X = 0.15f;
							OpenNijiiroRW.Tx.Result_Shine.Scale.Y = 0.15f;

							OpenNijiiroRW.Tx.Result_Shine.t2D中心基準描画(OpenNijiiroRW.Skin.Result_PlateShine_X[pos][i], OpenNijiiroRW.Skin.Result_PlateShine_Y[pos][i]);
						}

					}


					#endregion

					#region [Speech bubble animation]
					// Speech Bubble

					int Mood = 0;
					int MoodV2 = 0;

					if (OpenNijiiroRW.stageResults.nクリア[p] == 4)
					{
						Mood = 3;
						MoodV2 = 5;
					}
					else if (OpenNijiiroRW.stageResults.nクリア[p] == 3)
					{
						Mood = 3;
						MoodV2 = 4;
					}
					else if (OpenNijiiroRW.stageResults.nクリア[p] >= 1)
					{
						if (gaugeValues[p] >= 100.0f)
						{
							Mood = MoodV2 = 3;
						}
						else
						{
							Mood = MoodV2 = 2;
						}
					}
					else if (OpenNijiiroRW.stageResults.nクリア[p] == 0)
					{
						if (gaugeValues[p] >= 40.0f)
						{
							Mood = MoodV2 = 1;
						}
						else
						{
							Mood = MoodV2 = 0;
						}
					}

					if (nDrawnPlayers <= 2)
					{
						int speechBuddle_width = OpenNijiiroRW.Tx.Result_Speech_Bubble[pos].szTextureSize.Width / 4;
						int speechBuddle_height = OpenNijiiroRW.Tx.Result_Speech_Bubble[pos].szTextureSize.Height / 3;

						OpenNijiiroRW.Tx.Result_Speech_Bubble[pos].Scale.X = 0.9f * (ctMainCounter.CurrentValue <= MountainAppearValue + AddCount ? 1.3f - (float)Math.Sin((ctMainCounter.CurrentValue - MountainAppearValue) / (AddCount / 90) * (Math.PI / 180)) * 0.3f : 1.0f);
						OpenNijiiroRW.Tx.Result_Speech_Bubble[pos].Scale.Y = 0.9f * (ctMainCounter.CurrentValue <= MountainAppearValue + AddCount ? 1.3f - (float)Math.Sin((ctMainCounter.CurrentValue - MountainAppearValue) / (AddCount / 90) * (Math.PI / 180)) * 0.3f : 1.0f);
						OpenNijiiroRW.Tx.Result_Speech_Bubble[pos].t2D拡大率考慮中央基準描画(OpenNijiiroRW.Skin.Result_Speech_Bubble_X[pos], OpenNijiiroRW.Skin.Result_Speech_Bubble_Y[pos],
							new Rectangle(Mood * speechBuddle_width, RandomText * speechBuddle_height, speechBuddle_width, speechBuddle_height));
					}
					int speech_vubble_index = nDrawnPlayers <= 2 ? pos : 2;
					if (OpenNijiiroRW.Tx.Result_Speech_Bubble_V2[speech_vubble_index] != null)
					{
						int speechBuddle_width = OpenNijiiroRW.Tx.Result_Speech_Bubble_V2[speech_vubble_index].szTextureSize.Width;
						int speechBuddle_height = OpenNijiiroRW.Tx.Result_Speech_Bubble_V2[speech_vubble_index].szTextureSize.Height / 6;

						int speech_bubble_x;
						int speech_bubble_y;
						float scale;
						if (nDrawnPlayers == 5)
						{
							speech_bubble_x = OpenNijiiroRW.Skin.Result_Speech_Bubble_V2_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
							speech_bubble_y = OpenNijiiroRW.Skin.Result_Speech_Bubble_V2_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];
							scale = 0.5f;
						}
						else if (nDrawnPlayers == 4 || nDrawnPlayers == 3)
						{
							speech_bubble_x = OpenNijiiroRW.Skin.Result_Speech_Bubble_V2_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
							speech_bubble_y = OpenNijiiroRW.Skin.Result_Speech_Bubble_V2_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];
							scale = 0.5f;
						}
						else if (nDrawnPlayers == 2)
						{
							speech_bubble_x = OpenNijiiroRW.Skin.Result_Speech_Bubble_V2_2P_X[pos];
							speech_bubble_y = OpenNijiiroRW.Skin.Result_Speech_Bubble_V2_2P_Y[pos];
							scale = 0.5f;
						}
						else
						{
							speech_bubble_x = OpenNijiiroRW.Skin.Result_Speech_Bubble_V2_X[pos];
							speech_bubble_y = OpenNijiiroRW.Skin.Result_Speech_Bubble_V2_Y[pos];
							scale = 1.0f;
						}

						OpenNijiiroRW.Tx.Result_Speech_Bubble_V2[speech_vubble_index].Scale.X = 0.9f * scale * (ctMainCounter.CurrentValue <= MountainAppearValue + AddCount ? 1.3f - (float)Math.Sin((ctMainCounter.CurrentValue - MountainAppearValue) / (AddCount / 90) * (Math.PI / 180)) * 0.3f : 1.0f);
						OpenNijiiroRW.Tx.Result_Speech_Bubble_V2[speech_vubble_index].Scale.Y = 0.9f * scale * (ctMainCounter.CurrentValue <= MountainAppearValue + AddCount ? 1.3f - (float)Math.Sin((ctMainCounter.CurrentValue - MountainAppearValue) / (AddCount / 90) * (Math.PI / 180)) * 0.3f : 1.0f);
						OpenNijiiroRW.Tx.Result_Speech_Bubble_V2[speech_vubble_index].t2D拡大率考慮中央基準描画(speech_bubble_x, speech_bubble_y,
							new Rectangle(0, MoodV2 * speechBuddle_height, speechBuddle_width, speechBuddle_height));

						TitleTextureKey.ResolveTitleTexture(ttkSpeechText[p][MoodV2]).Scale.X = scale;
						TitleTextureKey.ResolveTitleTexture(ttkSpeechText[p][MoodV2]).Scale.Y = scale;
						TitleTextureKey.ResolveTitleTexture(ttkSpeechText[p][MoodV2]).t2D拡大率考慮中央基準描画(
							speech_bubble_x + (int)(OpenNijiiroRW.Skin.Result_Speech_Text_Offset[0] * scale),
							speech_bubble_y + (int)(OpenNijiiroRW.Skin.Result_Speech_Text_Offset[1] * scale));
					}
					if (!b音声再生[11])
					{
						if (OpenNijiiroRW.stageResults.nクリア[p] >= 1)
						{
							//TJAPlayer3.Skin.soundDonClear.t再生する();
							OpenNijiiroRW.Skin.voiceResultClearSuccess[OpenNijiiroRW.GetActualPlayer(p)]?.tPlay();
						}
						else
						{
							//TJAPlayer3.Skin.soundDonFailed.t再生する();
							OpenNijiiroRW.Skin.voiceResultClearFailed[OpenNijiiroRW.GetActualPlayer(p)]?.tPlay();
						}

						if (p == nDrawnPlayers - 1)
							b音声再生[11] = true;
					}

					#endregion
				}






				if (ctMainCounter.CurrentValue >= ScoreApparitionTimeStamp + 1000)
				{
					//if (TJAPlayer3.ConfigIni.nPlayerCount <= 2)
					{
						#region [Score rank apparition]

						int scoreRank_width = OpenNijiiroRW.Tx.Result_ScoreRankEffect.szTextureSize.Width / 7;
						int scoreRank_height = OpenNijiiroRW.Tx.Result_ScoreRankEffect.szTextureSize.Height / 4;

						if (ctMainCounter.CurrentValue <= ScoreApparitionTimeStamp + 1180)
						{
							OpenNijiiroRW.Tx.Result_ScoreRankEffect.Opacity = (int)((ctMainCounter.CurrentValue - (ScoreApparitionTimeStamp + 1000)) / 180.0f * 255.0f);
							OpenNijiiroRW.Tx.Result_ScoreRankEffect.Scale.X = 1.0f + (float)Math.Sin((float)(ctMainCounter.CurrentValue - (ScoreApparitionTimeStamp + 910)) / 1.5f * (Math.PI / 180)) * 1.4f;
							OpenNijiiroRW.Tx.Result_ScoreRankEffect.Scale.Y = 1.0f + (float)Math.Sin((float)(ctMainCounter.CurrentValue - (ScoreApparitionTimeStamp + 910)) / 1.5f * (Math.PI / 180)) * 1.4f;
						}
						else if (ctMainCounter.CurrentValue <= ScoreApparitionTimeStamp + 1270)
						{
							OpenNijiiroRW.Tx.Result_ScoreRankEffect.Scale.X = 0.5f + (float)Math.Sin((float)(ctMainCounter.CurrentValue - (ScoreApparitionTimeStamp + 1180)) * (Math.PI / 180)) * 0.5f;
							OpenNijiiroRW.Tx.Result_ScoreRankEffect.Scale.Y = 0.5f + (float)Math.Sin((float)(ctMainCounter.CurrentValue - (ScoreApparitionTimeStamp + 1180)) * (Math.PI / 180)) * 0.5f;
						}
						else
						{
							OpenNijiiroRW.Tx.Result_ScoreRankEffect.Opacity = 255;
							OpenNijiiroRW.Tx.Result_ScoreRankEffect.Scale.X = 1f;
							OpenNijiiroRW.Tx.Result_ScoreRankEffect.Scale.Y = 1f;
						}

						if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Dan && OpenNijiiroRW.stageResults.nスコアランク[p] > 0)
						{
							int CurrentFlash = 0;
							int[] FlashTimes = { 1500, 1540, 1580, 1620, 1660, 1700, 1740, 1780 };

							if (ctFlash_Icon.CurrentValue >= FlashTimes[0] && ctFlash_Icon.CurrentValue <= FlashTimes[1] || ctFlash_Icon.CurrentValue >= FlashTimes[4] && ctFlash_Icon.CurrentValue <= FlashTimes[5])
								CurrentFlash = 1;
							else if (ctFlash_Icon.CurrentValue >= FlashTimes[1] && ctFlash_Icon.CurrentValue <= FlashTimes[2] || ctFlash_Icon.CurrentValue >= FlashTimes[5] && ctFlash_Icon.CurrentValue <= FlashTimes[6])
								CurrentFlash = 2;
							else if (ctFlash_Icon.CurrentValue >= FlashTimes[2] && ctFlash_Icon.CurrentValue <= FlashTimes[3] || ctFlash_Icon.CurrentValue >= FlashTimes[6] && ctFlash_Icon.CurrentValue <= FlashTimes[7])
								CurrentFlash = 3;


							int scoreRankEffect_x;
							int scoreRankEffect_y;
							if (nDrawnPlayers == 5)
							{
								scoreRankEffect_x = OpenNijiiroRW.Skin.Result_ScoreRankEffect_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
								scoreRankEffect_y = OpenNijiiroRW.Skin.Result_ScoreRankEffect_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];
							}
							else if (nDrawnPlayers == 4 || nDrawnPlayers == 3)
							{
								scoreRankEffect_x = OpenNijiiroRW.Skin.Result_ScoreRankEffect_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
								scoreRankEffect_y = OpenNijiiroRW.Skin.Result_ScoreRankEffect_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];
							}
							else
							{
								scoreRankEffect_x = OpenNijiiroRW.Skin.Result_ScoreRankEffect_X[pos] + uioffset_x;
								scoreRankEffect_y = OpenNijiiroRW.Skin.Result_ScoreRankEffect_Y[pos];
							}

							OpenNijiiroRW.Tx.Result_ScoreRankEffect.t2D拡大率考慮中央基準描画(scoreRankEffect_x, scoreRankEffect_y,
								new Rectangle((OpenNijiiroRW.stageResults.nスコアランク[p] - 1) * scoreRank_width, CurrentFlash * scoreRank_height, scoreRank_width, scoreRank_height));

							if (!b音声再生[9] && ctMainCounter.CurrentValue >= ScoreApparitionTimeStamp + 1180)
							{
								OpenNijiiroRW.Skin.soundRankIn.tPlay();
								b音声再生[9] = true;
							}
						}

						#endregion
					}
				}


				if (ctMainCounter.CurrentValue >= ScoreApparitionTimeStamp + 2500)
				{
					//if (TJAPlayer3.ConfigIni.nPlayerCount <= 2)
					{
						#region [Crown apparition]

						int crownEffect_width = OpenNijiiroRW.Tx.Result_CrownEffect.szTextureSize.Width / 4;
						int crownEffect_height = OpenNijiiroRW.Tx.Result_CrownEffect.szTextureSize.Height / 4;

						if (ctMainCounter.CurrentValue <= ScoreApparitionTimeStamp + 2680)
						{
							OpenNijiiroRW.Tx.Result_CrownEffect.Opacity = (int)((ctMainCounter.CurrentValue - (ScoreApparitionTimeStamp + 2500)) / 180.0f * 255.0f);
							OpenNijiiroRW.Tx.Result_CrownEffect.Scale.X = 1.0f + (float)Math.Sin((float)(ctMainCounter.CurrentValue - (ScoreApparitionTimeStamp + 2410)) / 1.5f * (Math.PI / 180)) * 1.4f;
							OpenNijiiroRW.Tx.Result_CrownEffect.Scale.Y = 1.0f + (float)Math.Sin((float)(ctMainCounter.CurrentValue - (ScoreApparitionTimeStamp + 2410)) / 1.5f * (Math.PI / 180)) * 1.4f;
						}
						else if (ctMainCounter.CurrentValue <= ScoreApparitionTimeStamp + 2770)
						{
							OpenNijiiroRW.Tx.Result_CrownEffect.Scale.X = 0.5f + (float)Math.Sin((float)(ctMainCounter.CurrentValue - (ScoreApparitionTimeStamp + 2680)) * (Math.PI / 180)) * 0.5f;
							OpenNijiiroRW.Tx.Result_CrownEffect.Scale.Y = 0.5f + (float)Math.Sin((float)(ctMainCounter.CurrentValue - (ScoreApparitionTimeStamp + 2680)) * (Math.PI / 180)) * 0.5f;
						}
						else
						{
							OpenNijiiroRW.Tx.Result_CrownEffect.Opacity = 255;
							OpenNijiiroRW.Tx.Result_CrownEffect.Scale.X = 1f;
							OpenNijiiroRW.Tx.Result_CrownEffect.Scale.Y = 1f;
						}

						int ClearType = OpenNijiiroRW.stageResults.nクリア[p] - 1;

						if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)(Difficulty.Dan) && ClearType >= 0)
						{
							int CurrentFlash = 0;
							int[] FlashTimes = { 2000, 2040, 2080, 2120, 2160, 2200, 2240, 2280 };

							if (ctFlash_Icon.CurrentValue >= FlashTimes[0] && ctFlash_Icon.CurrentValue <= FlashTimes[1] || ctFlash_Icon.CurrentValue >= FlashTimes[4] && ctFlash_Icon.CurrentValue <= FlashTimes[5])
								CurrentFlash = 1;
							else if (ctFlash_Icon.CurrentValue >= FlashTimes[1] && ctFlash_Icon.CurrentValue <= FlashTimes[2] || ctFlash_Icon.CurrentValue >= FlashTimes[5] && ctFlash_Icon.CurrentValue <= FlashTimes[6])
								CurrentFlash = 2;
							else if (ctFlash_Icon.CurrentValue >= FlashTimes[2] && ctFlash_Icon.CurrentValue <= FlashTimes[3] || ctFlash_Icon.CurrentValue >= FlashTimes[6] && ctFlash_Icon.CurrentValue <= FlashTimes[7])
								CurrentFlash = 3;


							int crownEffect_x;
							int crownEffect_y;
							if (nDrawnPlayers == 5)
							{
								crownEffect_x = OpenNijiiroRW.Skin.Result_CrownEffect_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
								crownEffect_y = OpenNijiiroRW.Skin.Result_CrownEffect_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];
							}
							else if (nDrawnPlayers == 4 || nDrawnPlayers == 3)
							{
								crownEffect_x = OpenNijiiroRW.Skin.Result_CrownEffect_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
								crownEffect_y = OpenNijiiroRW.Skin.Result_CrownEffect_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];
							}
							else
							{
								crownEffect_x = OpenNijiiroRW.Skin.Result_CrownEffect_X[pos] + uioffset_x;
								crownEffect_y = OpenNijiiroRW.Skin.Result_CrownEffect_Y[pos];
							}

							OpenNijiiroRW.Tx.Result_CrownEffect.t2D拡大率考慮中央基準描画(crownEffect_x, crownEffect_y,
								new Rectangle(ClearType * crownEffect_width, CurrentFlash * crownEffect_height, crownEffect_width, crownEffect_height));

							if (!b音声再生[10] && ctMainCounter.CurrentValue >= ScoreApparitionTimeStamp + 2680)
							{
								OpenNijiiroRW.Skin.soundCrownIn.tPlay();
								b音声再生[10] = true;
							}
						}

						#endregion
					}
				}
			}

			#endregion



			#endregion
		}

		if (!this.ct表示用.IsEnded)
		{
			return 0;
		}
		return 1;
	}



	// その他

	#region [ private ]
	//-----------------
	[StructLayout(LayoutKind.Sequential)]
	private struct ST文字位置
	{
		public char ch;
		public Point pt;
	}

	public CCounter ctMainCounter;
	public CCounter[] ctゲージアニメ;
	private CCounter ct虹ゲージアニメ;
	private CCounter ctSoul;

	public CCounter ctEndAnime;
	public CCounter ctMountain_ClearIn;
	public CCounter ctBackgroundAnime;
	public CCounter ctBackgroundAnime_Clear;

	private int RandomText;

	private CCounter ctFlash_Icon;
	private CCounter ctRotate_Flowers;
	private CCounter ctShine_Plate;

	public PuchiChara PuchiChara;

	public float MountainAppearValue;
	private int GaugeFactor;

	/// <summary>
	/// Indexes:
	/// 0: soundGauge, 1~7: soundPon (Perfect_X, Good_X, Miss_X, Roll_X, MaxCombo_X, ADLib_X, Bomb),
	/// 8: soundScoreDon, 9: soundRankIn, 10: soundCrownIn, 11: voiceResultClearSuccess / voiceResultClearFailed
	/// </summary>
	public bool[] b音声再生 = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };

	// Cherry blossom flowers variables
	/*
	public int[] FlowerXPos = { -114, -37, 114, 78, -75 };
	public int[] FlowerYPos = { -33, 3, -36, -81, -73 };
	*/
	public float[] FlowerRotationSpeeds = { 5f, 3f, -6f, 4f, -2f };
	public int[] ApparitionTimeStamps = { 10, 30, 50, 100, 190 };
	public int ApparitionFade = 100;
	public int ApparitionDuration = 300;

	// Plate shine variables
	public int[] ShinePXPos = { 114 - 25, 114 - 16, -37 - 23, -37 - 9, -75 + 20, 78 - 13 };
	public int[] ShinePYPos = { -36 + 52, -36 + 2, 3 - 7, 3 + 30, -73 - 23, -81 - 31 };
	public int ShinePFade = 100;

	public int[] gaugeValues;

	private CCounter ct表示用;
	private readonly Point[] ptFullCombo位置;
	private CSound sdDTXで指定されたフルコンボ音;
	private readonly ST文字位置[] st小文字位置;
	private readonly ST文字位置[] st大文字位置;
	private ST文字位置[] stScoreFont;

	private TitleTextureKey[] ttkAISection;

	private TitleTextureKey[][] ttkSpeechText = new TitleTextureKey[5][];

	private CCachedFontRenderer pfSpeechText;
	private CCachedFontRenderer pfAISectionText;

	private CCounter ctAISectionChange;

	private CCounter ctUIMove;

	private int nNowAISection;

	private void NextAISection()
	{
		ctAISectionChange = new CCounter(0, 2000, 1, OpenNijiiroRW.Timer);
		ctAISectionChange.CurrentValue = 0;

		nNowAISection++;
		if (nNowAISection >= Math.Ceiling(OpenNijiiroRW.stageGameScreen.AIBattleSections.Count / 5.0))
		{
			nNowAISection = 0;

		}
	}

	public void t小文字表示(int x, int y, int num, float scale)
	{
		OpenNijiiroRW.Tx.Result_Number.Scale.X *= scale;
		OpenNijiiroRW.Tx.Result_Number.Scale.Y *= scale;
		int[] nums = CConversion.SeparateDigits(num);
		for (int j = 0; j < nums.Length; j++)
		{
			float offset = j;

			float width = (OpenNijiiroRW.Tx.Result_Number.sz画像サイズ.Width / 11.0f);
			float height = (OpenNijiiroRW.Tx.Result_Number.sz画像サイズ.Height / 2.0f);

			float _x = x - ((OpenNijiiroRW.Skin.Result_Number_Interval[0] * scale) * offset) + (width * 2);
			float _y = y - ((OpenNijiiroRW.Skin.Result_Number_Interval[1] * scale) * offset);

			OpenNijiiroRW.Tx.Result_Number.t2D拡大率考慮中央基準描画(_x + (width * scale / 2), _y + (height * scale / 2),
				new RectangleF(width * nums[j], 0, width, height));
		}
	}
	private void t大文字表示(int x, int y, string str)
	{
		this.t大文字表示(x, y, str, false);
	}
	private void t大文字表示(int x, int y, string str, bool b強調)
	{
		foreach (char ch in str)
		{
			for (int i = 0; i < this.st大文字位置.Length; i++)
			{
				if (this.st大文字位置[i].ch == ch)
				{
					Rectangle rectangle = new Rectangle(this.st大文字位置[i].pt.X, this.st大文字位置[i].pt.Y, 11, 0x10);
					if (ch == '.')
					{
						rectangle.Width -= 2;
						rectangle.Height -= 2;
					}
					if (OpenNijiiroRW.Tx.Result_Number != null)
					{
						OpenNijiiroRW.Tx.Result_Number.t2D描画(x, y, rectangle);
					}
					break;
				}
			}
			x += 8;
		}
	}

	public void tスコア文字表示(int x, int y, int num, float scale)
	{
		OpenNijiiroRW.Tx.Result_Score_Number.Scale.X *= scale;
		OpenNijiiroRW.Tx.Result_Score_Number.Scale.Y *= scale;
		int[] nums = CConversion.SeparateDigits(num);
		for (int j = 0; j < nums.Length; j++)
		{
			float offset = j;
			float _x = x - (OpenNijiiroRW.Skin.Result_Score_Number_Interval[0] * scale * offset);
			float _y = y - (OpenNijiiroRW.Skin.Result_Score_Number_Interval[1] * scale * offset);

			float width = (OpenNijiiroRW.Tx.Result_Score_Number.sz画像サイズ.Width / 10.0f);
			float height = (OpenNijiiroRW.Tx.Result_Score_Number.sz画像サイズ.Height);

			OpenNijiiroRW.Tx.Result_Score_Number.t2D拡大率考慮中央基準描画(_x, _y + (height * scale / 2), new RectangleF(width * nums[j], 0, width, height));
		}
	}
	//-----------------
	#endregion
}
