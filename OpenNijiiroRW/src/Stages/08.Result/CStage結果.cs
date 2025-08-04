using System.Diagnostics;
using System.Drawing;
using System.Text;
using DiscordRPC;
using FDK;

namespace OpenNijiiroRW;

internal class CStage結果 : CStage
{

	// Properties

	public STDGBVALUE<float> fPerfect率;
	public STDGBVALUE<float> fGreat率;
	public STDGBVALUE<float> fGood率;
	public STDGBVALUE<float> fPoor率;
	public STDGBVALUE<float> fMiss率;
	public STDGBVALUE<bool> bオート;        // #23596 10.11.16 add ikanick
										 //        10.11.17 change (int to bool) ikanick

	public STDGBVALUE<int> nランク値;
	public STDGBVALUE<int> n演奏回数;
	public STDGBVALUE<int> nScoreRank;
	public int n総合ランク値;

	public int[] nクリア = { 0, 0, 0, 0, 0 };        //0:Not-cleared 1:Assisted-cleared 2:Cleared 3:Full-combo 4:Perfect
	public int[] nスコアランク = { 0, 0, 0, 0, 0 };  //0:未取得 1:白粋 2:銅粋 3:銀粋 4:金雅 5:桃雅 6:紫雅 7:虹極
	public int[] nHighScore = { 0, 0, 0, 0, 0 };
	public bool[] IsScoreValid = [false, false, false, false, false];
	public int[] ClearStatusesSaved = [0, 0, 0, 0, 0];
	public int[] ScoreRanksSaved = [0, 0, 0, 0, 0];

	public CChip[] r空うちドラムチップ;
	public STDGBVALUE<CScoreIni.C演奏記録> st演奏記録;


	// Constructor

	public CStage結果()
	{
		this.st演奏記録.Drums = new CScoreIni.C演奏記録();
		this.r空うちドラムチップ = new CChip[10];
		this.n総合ランク値 = -1;
		base.eStageID = CStage.EStage.Results;
		base.ePhaseID = CStage.EPhase.Common_NORMAL;
		base.IsDeActivated = true;
		base.ChildActivities.Add(this.actParameterPanel = new CActResultParameterPanel());
		base.ChildActivities.Add(this.actSongBar = new CActResultSongBar());
		base.ChildActivities.Add(this.actOption = new CActオプションパネル());
		base.ChildActivities.Add(this.actFIFromGameplay = new CActFIFOResult());
		base.ChildActivities.Add(this.actFO = new CActFIFOBlack());
	}


	public bool isAutoDisabled(int player)
	{
		return ((player != 1 && !OpenNijiiroRW.ConfigIni.bAutoPlay[player])
				|| (player == 1 && !OpenNijiiroRW.ConfigIni.bAutoPlay[player] && !OpenNijiiroRW.ConfigIni.bAIBattleMode));
	}


	public int GetTowerScoreRank()
	{
		int tmpClear = 0;
		double progress = CFloorManagement.LastRegisteredFloor / ((double)OpenNijiiroRW.stageSongSelect.rChoosenSong.score[5].譜面情報.nTotalFloor);

		// Clear badges : 10% (E), 25% (D), 50% (C), 75% (B), Clear (A), FC (S), DFC (X)
		bool[] conditions =
		{
			progress >= 0.1,
			progress >= 0.25,
			progress >= 0.5,
			progress >= 0.75,
			progress == 1 && CFloorManagement.CurrentNumberOfLives > 0,
			OpenNijiiroRW.stageGameScreen.CChartScore[0].nMiss == 0 && OpenNijiiroRW.stageGameScreen.CChartScore[0].nMine == 0,
			OpenNijiiroRW.stageGameScreen.CChartScore[0].nGood == 0
		};

		for (int i = 0; i < conditions.Length; i++)
		{
			if (conditions[i] == true)
				tmpClear++;
			else
				break;
		}

		return tmpClear;
	}

	// CStage 実装

	public override void Activate()
	{

		Trace.TraceInformation("結果ステージを活性化します。");
		Trace.Indent();
		bAddedToRecentlyPlayedSongs = false;
		try
		{
			/*
			 * Notes about the difference between Replay - Save statuses and the "Assisted clear" clear status
			 *
			 * - The values for replay files are 0 if no status, while for save files they start by -1
			 * - The "Assisted clear" status is used on the save files, but NOT on the replay files
			 * - The "Assisted clear" status is also not used in the coins evaluations
			 */
			int[] ClearStatus_Replay = new int[5] { 0, 0, 0, 0, 0 };
			int[] ScoreRank_Replay = new int[5] { 0, 0, 0, 0, 0 };

			int[] clearStatuses =
			{
				-1,
				-1,
				-1,
				-1,
				-1
			};

			int[] scoreRanks =
			{
				-1,
				-1,
				-1,
				-1,
				-1
			};

			bool[] assistedClear =
			{
				(OpenNijiiroRW.stageSongSelect.actPlayOption.tGetModMultiplier(CActPlayOption.EBalancingType.SCORE, false, 0) < 1f),
				(OpenNijiiroRW.stageSongSelect.actPlayOption.tGetModMultiplier(CActPlayOption.EBalancingType.SCORE, false, 1) < 1f),
				(OpenNijiiroRW.stageSongSelect.actPlayOption.tGetModMultiplier(CActPlayOption.EBalancingType.SCORE, false, 2) < 1f),
				(OpenNijiiroRW.stageSongSelect.actPlayOption.tGetModMultiplier(CActPlayOption.EBalancingType.SCORE, false, 3) < 1f),
				(OpenNijiiroRW.stageSongSelect.actPlayOption.tGetModMultiplier(CActPlayOption.EBalancingType.SCORE, false, 4) < 1f)
			};

			this.IsScoreValid = [false, false, false, false, false];
			this.ClearStatusesSaved = [0, 0, 0, 0, 0];
			this.ScoreRanksSaved = [0, 0, 0, 0, 0];

			{
				#region [ 初期化 ]
				//---------------------
				this.eフェードアウト完了時の戻り値 = E戻り値.継続;
				this.bアニメが完了 = false;
				this.bIsCheckedWhetherResultScreenShouldSaveOrNot = false;              // #24609 2011.3.14 yyagi

				//---------------------
				#endregion

				#region [ Results calculus ]
				//---------------------

				if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Dan && OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Tower)
				{
					for (int p = 0; p < OpenNijiiroRW.ConfigIni.nPlayerCount; p++)
					{
						var ccf = OpenNijiiroRW.stageGameScreen.CChartScore[p];

						this.nクリア[p] = 0;
						if (HGaugeMethods.UNSAFE_FastNormaCheck(p))
						{
							this.nクリア[p] = 2;
							if (ccf.nMiss == 0 && ccf.nMine == 0)
							{
								this.nクリア[p] = 3;
								if (ccf.nGood == 0) this.nクリア[p] = 4;
							}

							if (assistedClear[p]) this.nクリア[p] = 1;

							clearStatuses[p] = this.nクリア[p] - 1;

						}

						if ((int)OpenNijiiroRW.stageGameScreen.actScore.Get(p) < 500000)
						{
							this.nスコアランク[p] = 0;
						}
						else
						{
							var sr = OpenNijiiroRW.stageGameScreen.ScoreRank.ScoreRank[p];

							for (int i = 0; i < 7; i++)
							{
								if ((int)OpenNijiiroRW.stageGameScreen.actScore.Get(p) >= sr[i])
								{
									this.nスコアランク[p] = i + 1;
								}
							}
						}
						scoreRanks[p] = this.nスコアランク[p] - 1;

					}


				}

				//---------------------
				#endregion

				#region [ Saves calculus ]
				//---------------------

				// Clear and score ranks



				if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Dan && OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Tower)
				{
					// Regular (Ensou game) Score and Score Rank saves

					#region [Regular saves]

					for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
					{
						int diff = OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i];

						ClearStatus_Replay[i] = this.nクリア[i];
						ScoreRank_Replay[i] = this.nスコアランク[i];
					}

					#endregion

				}
				else if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan)
				{
					/* == Specific format for DaniDoujou charts ==
					 **
					 ** Higher is better, takes the Clear0 spot (Usually the spot allocated for Kantan Clear crowns)
					 **
					 ** 0 (Fugoukaku, no insign)
					 ** Silver Iki (Clear) : 1 (Red Goukaku) / 2 (Gold Goukaku)
					 ** Gold Iki (Full Combo) : 3 (Red Goukaku) / 4 (Gold Goukaku)
					 ** Rainbow Iki (Donda Full Combo) : 5 (Red Goukaku) / 6 (Gold Goukaku)
					 **
					 */

					#region [Dan scores]

					Exam.Status examStatus = OpenNijiiroRW.stageGameScreen.actDan.GetResultExamStatus(this.st演奏記録.Drums.Dan_C, OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs);

					int clearValue = 0;

					if (examStatus != Exam.Status.Failure)
					{
						// Red Goukaku
						clearValue += 1;

						// Gold Goukaku
						if (examStatus == Exam.Status.Better_Success)
							clearValue += 1;

						// Gold Iki
						if (this.st演奏記録.Drums.nBadCount == 0)
						{
							clearValue += 2;

							// Rainbow Iki
							if (this.st演奏記録.Drums.nOkCount == 0)
								clearValue += 2;
						}

						if (assistedClear[0]) clearStatuses[0] = (examStatus == Exam.Status.Better_Success) ? 1 : 0;
						else clearStatuses[0] = clearValue + 1;

					}

					if (isAutoDisabled(0))
					{
						ClearStatus_Replay[0] = clearValue;
					}

					// this.st演奏記録[0].nクリア[0] = Math.Max(ini[0].stセクション[0].nクリア[0], clearValue);

					// Unlock dan grade
					if (clearValue > 0 && !OpenNijiiroRW.ConfigIni.bAutoPlay[0])
					{
						/*
						this.newGradeGranted = TJAPlayer3.NamePlateConfig.tUpdateDanTitle(TJAPlayer3.stage選曲.r確定された曲.strタイトル.Substring(0, 2),
							clearValue % 2 == 0,
							(clearValue - 1) / 2,
							TJAPlayer3.SaveFile);
						*/

						this.newGradeGranted = OpenNijiiroRW.SaveFileInstances[OpenNijiiroRW.SaveFile].tUpdateDanTitle(OpenNijiiroRW.stageSongSelect.rChoosenSong.ldTitle.GetString("").RemoveTags().Substring(0, 2),
							clearValue % 2 == 0,
							(clearValue - 1) / 2);
					}

					#endregion

				}
				else if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Tower)
				{
					// Clear if top reached, then FC or DFC like any regular chart
					// Score Rank cointains highest reached floor

					#region [Tower scores]

					int tmpClear = GetTowerScoreRank();

					if (tmpClear != 0) clearStatuses[0] = assistedClear[0] ? 0 : tmpClear;

					if (isAutoDisabled(0))
					{
						ClearStatus_Replay[0] = tmpClear;
					}

					#endregion

				}

				//---------------------
				#endregion

			}

			string diffToString(int diff)
			{
				string[] diffArr =
				{
					" Easy ",
					" Normal ",
					" Hard ",
					" Extreme ",
					" Extra ",
					" Tower ",
					" Dan "
				};
				string[] diffArrIcon =
				{
					"-",
					"",
					"+"
				};

				int level = OpenNijiiroRW.stageSongSelect.rChoosenSong.nLevel[diff];
				CTja.ELevelIcon levelIcon = OpenNijiiroRW.stageSongSelect.rChoosenSong.nLevelIcon[diff];

				return (diffArr[Math.Min(diff, 6)] + "Lv." + level + diffArrIcon[(int)levelIcon]);
			}

			string details = OpenNijiiroRW.ConfigIni.SendDiscordPlayingInformation ? OpenNijiiroRW.stageSongSelect.rChoosenSong.ldTitle.GetString("")
				+ diffToString(OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0]) : "";

			// Byte count must be used instead of String.Length.
			// The byte count is what Discord is concerned with. Some chars are greater than one byte.
			if (Encoding.UTF8.GetBytes(details).Length > 128)
			{
				byte[] details_byte = Encoding.UTF8.GetBytes(details);
				Array.Resize(ref details_byte, 128);
				details = Encoding.UTF8.GetString(details_byte);
			}

			// Discord Presenseの更新
			OpenNijiiroRW.DiscordClient?.SetPresence(new RichPresence()
			{
				Details = details,
				State = "Result" + (OpenNijiiroRW.ConfigIni.bAutoPlay[0] == true ? " (Auto)" : ""),
				Timestamps = new Timestamps(OpenNijiiroRW.StartupTime),
				Assets = new Assets()
				{
					LargeImageKey = OpenNijiiroRW.LargeImageKey,
					LargeImageText = OpenNijiiroRW.LargeImageText,
				}
			});


			#region [Earned medals]

			this.nEarnedMedalsCount[0] = 0;
			this.nEarnedMedalsCount[1] = 0;
			this.nEarnedMedalsCount[2] = 0;
			this.nEarnedMedalsCount[3] = 0;
			this.nEarnedMedalsCount[4] = 0;



			// Medals

			int nTotalHits = this.st演奏記録.Drums.nOkCount + this.st演奏記録.Drums.nBadCount + this.st演奏記録.Drums.nGoodCount;

			double dAccuracyRate = Math.Pow((50 * this.st演奏記録.Drums.nOkCount + 100 * this.st演奏記録.Drums.nGoodCount) / (double)(100 * nTotalHits), 3);

			int diffModifier;
			float starRate;
			float redStarRate;


			float[] modMultipliers =
			{
				OpenNijiiroRW.stageSongSelect.actPlayOption.tGetModMultiplier(CActPlayOption.EBalancingType.COINS, false, 0),
				OpenNijiiroRW.stageSongSelect.actPlayOption.tGetModMultiplier(CActPlayOption.EBalancingType.COINS, false, 1),
				OpenNijiiroRW.stageSongSelect.actPlayOption.tGetModMultiplier(CActPlayOption.EBalancingType.COINS, false, 2),
				OpenNijiiroRW.stageSongSelect.actPlayOption.tGetModMultiplier(CActPlayOption.EBalancingType.COINS, false, 3),
				OpenNijiiroRW.stageSongSelect.actPlayOption.tGetModMultiplier(CActPlayOption.EBalancingType.COINS, false, 4)
			};

			float getCoinMul(int player)
			{
				var chara = OpenNijiiroRW.Tx.Characters[OpenNijiiroRW.SaveFileInstances[OpenNijiiroRW.GetActualPlayer(player)].data.Character];
				var puchichara = OpenNijiiroRW.Tx.Puchichara[PuchiChara.tGetPuchiCharaIndexByName(OpenNijiiroRW.GetActualPlayer(player))];


				return chara.GetEffectCoinMultiplier() * puchichara.GetEffectCoinMultiplier();
			}

			if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Tower)
			{
				diffModifier = 3;

				int stars = OpenNijiiroRW.stageSongSelect.rChoosenSong.score[(int)Difficulty.Tower].譜面情報.nレベル[(int)Difficulty.Tower];

				starRate = Math.Min(10, stars) / 2;
				redStarRate = Math.Max(0, stars - 10) * 4;

				int maxFloors = OpenNijiiroRW.stageSongSelect.rChoosenSong.score[(int)Difficulty.Tower].譜面情報.nTotalFloor;

				double floorRate = Math.Pow(CFloorManagement.LastRegisteredFloor / (double)maxFloors, 2);
				double lengthBonus = Math.Max(1, maxFloors / 140.0);

				#region [Clear modifier]

				int clearModifier = 0;

				if (this.st演奏記録.Drums.nBadCount == 0)
				{
					clearModifier = (int)(5 * lengthBonus);
					if (this.st演奏記録.Drums.nOkCount == 0)
					{
						clearModifier = (int)(12 * lengthBonus);
					}
				}

				#endregion

				// this.nEarnedMedalsCount[0] = stars;
				this.nEarnedMedalsCount[0] = 5 + (int)((diffModifier * (starRate + redStarRate)) * (floorRate * lengthBonus)) + clearModifier;
				this.nEarnedMedalsCount[0] = Math.Max(5, (int)(this.nEarnedMedalsCount[0] * modMultipliers[0] * getCoinMul(0)));
			}
			else if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan)
			{
				int partialScore = 0;

				#region [Clear and Goukaku modifier]

				Exam.Status examStatus = OpenNijiiroRW.stageGameScreen.actDan.GetResultExamStatus(this.st演奏記録.Drums.Dan_C, OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs);

				int clearModifier = -1;
				int goukakuModifier = 0;

				if (examStatus != Exam.Status.Failure)
				{
					clearModifier = 0;
					if (this.st演奏記録.Drums.nBadCount == 0)
					{
						clearModifier = 4;
						if (this.st演奏記録.Drums.nOkCount == 0)
							clearModifier = 6;
					}

					if (examStatus == Exam.Status.Better_Success)
						goukakuModifier = 20;
				}

				#endregion

				#region [Partial scores]

				for (int i = 0; i < OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs.Count; i++)
				{
					if (OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs[i] != null)
					{
						int diff = OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs[i].Difficulty;
						int stars = OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs[i].Level;

						diffModifier = Math.Max(1, Math.Min(3, diff));

						starRate = Math.Min(10, stars) / 2;
						redStarRate = Math.Max(0, stars - 10) * 4;

						partialScore += (int)(diffModifier * (starRate + redStarRate));
					}

				}

				#endregion

				if (clearModifier < 0)
					this.nEarnedMedalsCount[0] = 10;
				else
				{
					this.nEarnedMedalsCount[0] = 10 + goukakuModifier + clearModifier + (int)(partialScore * dAccuracyRate);
					this.nEarnedMedalsCount[0] = Math.Max(10, (int)(this.nEarnedMedalsCount[0] * modMultipliers[0] * getCoinMul(0)));
				}
			}
			else
			{
				for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
				{
					int diff = OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i];
					int stars = OpenNijiiroRW.stageSongSelect.rChoosenSong.score[diff].譜面情報.nレベル[diff];

					diffModifier = Math.Max(1, Math.Min(3, diff));

					starRate = Math.Min(10, stars) / 2;
					redStarRate = Math.Max(0, stars - 10) * 4;

					#region [Clear modifier]

					int[] modifiers = { -1, 0, 2, 3 };

					int clearModifier = modifiers[0];

					if (HGaugeMethods.UNSAFE_FastNormaCheck(i))
					{
						clearModifier = modifiers[1] * diffModifier;
						if (OpenNijiiroRW.stageGameScreen.CChartScore[i].nMiss == 0)
						{
							clearModifier = modifiers[2] * diffModifier;
							if (OpenNijiiroRW.stageGameScreen.CChartScore[i].nGood == 0)
								clearModifier = modifiers[3] * diffModifier;
						}
					}

					#endregion

					#region [Score rank modifier]

					int[] srModifiers = { 0, 0, 0, 0, 1, 1, 2, 3 };

					// int s = TJAPlayer3.stage演奏ドラム画面.ScoreRank.ScoreRank[1];

					int scoreRankModifier = srModifiers[0] * diffModifier;

					for (int j = 1; j < 8; j++)
					{
						if (OpenNijiiroRW.stageGameScreen.actScore.GetScore(i) >= OpenNijiiroRW.stageGameScreen.ScoreRank.ScoreRank[i][j - 1])
							scoreRankModifier = srModifiers[j] * diffModifier;
					}

					#endregion

					nTotalHits = OpenNijiiroRW.stageGameScreen.CChartScore[i].nGood + OpenNijiiroRW.stageGameScreen.CChartScore[i].nMiss + OpenNijiiroRW.stageGameScreen.CChartScore[i].nGreat;

					dAccuracyRate = Math.Pow((50 * OpenNijiiroRW.stageGameScreen.CChartScore[i].nGood + 100 * OpenNijiiroRW.stageGameScreen.CChartScore[i].nGreat) / (double)(100 * nTotalHits), 3);

					if (clearModifier < 0)
						this.nEarnedMedalsCount[i] = 5;
					else
					{
						this.nEarnedMedalsCount[i] = 5 + (int)((diffModifier * (starRate + redStarRate)) * dAccuracyRate) + clearModifier + scoreRankModifier;
						this.nEarnedMedalsCount[i] = Math.Max(5, (int)(this.nEarnedMedalsCount[i] * modMultipliers[i] * getCoinMul(i)));
					}
				}
			}

			// ADLIB bonuses : 1 coin per ADLIB
			for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
			{
				// Too broken on some charts, ADLibs should get either no bonus or just extra stats
				//this.nEarnedMedalsCount[i] += Math.Min(10, TJAPlayer3.stage演奏ドラム画面.CChartScore[i].nADLIB);

				if (OpenNijiiroRW.ConfigIni.bAutoPlay[i])
					this.nEarnedMedalsCount[i] = 0;
				if (OpenNijiiroRW.ConfigIni.bAIBattleMode && i == 1)
					this.nEarnedMedalsCount[i] = 0;

				var _sf = OpenNijiiroRW.SaveFileInstances[OpenNijiiroRW.GetActualPlayer(i)];

				if (OpenNijiiroRW.ConfigIni.bAIBattleMode && i == 0)
				{
					_sf.tRegisterAIBattleModePlay(bClear[0]);
				}

				if (this.nEarnedMedalsCount[i] > 0)
					_sf.tEarnCoins(this.nEarnedMedalsCount[i]);

				if (!OpenNijiiroRW.ConfigIni.bAutoPlay[i]
					&& !(OpenNijiiroRW.ConfigIni.bAIBattleMode && i == 1))
				{
					int _cs = -1;
					if (HGaugeMethods.UNSAFE_FastNormaCheck(i))
					{
						_cs = 0;
						if (OpenNijiiroRW.stageGameScreen.CChartScore[i].nMiss == 0)
						{
							_cs = 1;
							if (OpenNijiiroRW.stageGameScreen.CChartScore[i].nGood == 0)
								_cs = 2;
						}
					}

					// Unsafe function, it is the only appropriate place to call it
					if (DBSaves.RegisterPlay(i, clearStatuses[i], scoreRanks[i]))
					{
						this.IsScoreValid[i] = true;
						this.ClearStatusesSaved[i] = clearStatuses[i];
						this.ScoreRanksSaved[i] = scoreRanks[i];
					}
				}
			}


			//TJAPlayer3.NamePlateConfig.tEarnCoins(this.nEarnedMedalsCount);

			#endregion

			#region [Replay files generation]

			for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
			{
				if (OpenNijiiroRW.ConfigIni.bAutoPlay[i])
					continue;
				if (OpenNijiiroRW.ConfigIni.bAIBattleMode && i == 1)
					continue;
				OpenNijiiroRW.ReplayInstances[i].tResultsRegisterReplayInformations(this.nEarnedMedalsCount[i], ClearStatus_Replay[i], ScoreRank_Replay[i]);
				OpenNijiiroRW.ReplayInstances[i].tSaveReplayFile();
			}

			#endregion

			#region [Modals preprocessing]

			for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
			{
				if (OpenNijiiroRW.ConfigIni.bAutoPlay[i] || OpenNijiiroRW.ConfigIni.bAIBattleMode && i == 1) continue;

				if (this.nEarnedMedalsCount[i] > 0)
					OpenNijiiroRW.ModalManager.rModalQueue.tAddModal(
						new Modal(
							Modal.EModalType.Coin,
							0,
							(long)this.nEarnedMedalsCount[i],
							OpenNijiiroRW.SaveFileInstances[OpenNijiiroRW.GetActualPlayer(i)].data.Medals
						),
						i);

				// Check unlockables
				{
					OpenNijiiroRW.Databases.DBNameplateUnlockables.tGetUnlockedItems(i, OpenNijiiroRW.ModalManager.rModalQueue);
					OpenNijiiroRW.Databases.DBSongUnlockables.tGetUnlockedItems(i, OpenNijiiroRW.ModalManager.rModalQueue);

					foreach (var puchi in OpenNijiiroRW.Tx.Puchichara)
					{
						puchi.tGetUnlockedItems(i, OpenNijiiroRW.ModalManager.rModalQueue);
					}

					foreach (var chara in OpenNijiiroRW.Tx.Characters)
					{
						chara.tGetUnlockedItems(i, OpenNijiiroRW.ModalManager.rModalQueue);
					}
				}

			}

			#endregion

			OpenNijiiroRW.stageSongSelect.actSongList.bFirstCrownLoad = false;

			this.ctPhase1 = null;
			this.ctPhase2 = null;
			this.ctPhase3 = null;
			examsShift = 0;

			Dan_Plate = OpenNijiiroRW.tテクスチャの生成(Path.GetDirectoryName(OpenNijiiroRW.TJA.strFullPath) + @$"{Path.DirectorySeparatorChar}Dan_Plate.png");

			base.Activate();


			ctShine_Plate = new CCounter(0, 1000, 1, OpenNijiiroRW.Timer);
			ctWork_Plate = new CCounter(0, 4000, 1, OpenNijiiroRW.Timer);

			if (OpenNijiiroRW.Tx.TowerResult_Background != null)
				ctTower_Animation = new CCounter(0, OpenNijiiroRW.Tx.TowerResult_Background.szTextureSize.Height - OpenNijiiroRW.Skin.Resolution[1], 25, OpenNijiiroRW.Timer);
			else
				ctTower_Animation = new CCounter();


			ctDanSongInfoChange = new CCounter(0, 3000, 1, OpenNijiiroRW.Timer);
			ctDanSongInfoChange.CurrentValue = 255;

			b音声再生 = false;
			this.EndAnime = false;

			if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Tower)
			{
				this.ttkMaxFloors = new TitleTextureKey("/" + OpenNijiiroRW.stageSongSelect.rChoosenSong.score[5].譜面情報.nTotalFloor.ToString() + CLangManager.LangInstance.GetString("TOWER_FLOOR_INITIAL"), pfTowerText48, Color.Black, Color.Transparent, 700);
				this.ttkToutatsu = new TitleTextureKey(CLangManager.LangInstance.GetString("TOWER_FLOOR_REACHED"), pfTowerText48, Color.White, Color.Black, 700);
				this.ttkTen = new TitleTextureKey(CLangManager.LangInstance.GetString("TOWER_SCORE_INITIAL"), pfTowerText, Color.Black, Color.Transparent, 700);
				this.ttkReachedFloor = new TitleTextureKey(CFloorManagement.LastRegisteredFloor.ToString(), pfTowerText72, Color.Orange, Color.Black, 700);
				this.ttkScore = new TitleTextureKey(CLangManager.LangInstance.GetString("TOWER_SCORE"), pfTowerText, Color.Black, Color.Transparent, 700);
				this.ttkRemaningLifes = new TitleTextureKey(CFloorManagement.CurrentNumberOfLives.ToString() + " / " + CFloorManagement.MaxNumberOfLives.ToString(), pfTowerText, Color.Black, Color.Transparent, 700);
				this.ttkScoreCount = new TitleTextureKey(OpenNijiiroRW.stageGameScreen.actScore.GetScore(0).ToString(), pfTowerText, Color.Black, Color.Transparent, 700);
			}
			else if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan)
			{
				Background = new ResultBG(CSkin.Path($@"{TextureLoader.BASE}{TextureLoader.DANRESULT}Script.lua"));
				Background.Init();
			}
			else if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
			{
				Background = new ResultBG(CSkin.Path($@"{TextureLoader.BASE}{TextureLoader.RESULT}AIBattle{Path.DirectorySeparatorChar}Script.lua"));
				Background.Init();
			}
			else
			{
				//Luaに移植する時にコメントアウトを解除
				Background = new ResultBG(CSkin.Path($@"{TextureLoader.BASE}{TextureLoader.RESULT}{Path.DirectorySeparatorChar}Script.lua"));
				Background.Init();
			}

			this.ttkDanTitles = new TitleTextureKey[OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs.Count];

			for (int i = 0; i < OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs.Count; i++)
			{
				this.ttkDanTitles[i] = new TitleTextureKey(OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs[i].bTitleShow
						? "???"
						: OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs[i].Title,
					pfDanTitles,
					Color.White,
					Color.Black,
					700);
			}
		}
		finally
		{
			Trace.TraceInformation("結果ステージの活性化を完了しました。");
			Trace.Unindent();
		}

		if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Dan && OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Tower)
			bgmResultIn.tPlay();
	}
	public override void DeActivate()
	{
		OpenNijiiroRW.tDisposeSafely(ref Background);

		if (this.rResultSound != null)
		{
			OpenNijiiroRW.SoundManager.tDisposeSound(this.rResultSound);
			this.rResultSound = null;
		}

		if (this.ct登場用 != null)
		{
			this.ct登場用 = null;
		}
		Dan_Plate?.Dispose();

		base.DeActivate();
	}
	public override void CreateManagedResource()
	{
		this.pfTowerText = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.TowerResult_Font_TowerText);
		this.pfTowerText48 = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.TowerResult_Font_TowerText48);
		this.pfTowerText72 = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.TowerResult_Font_TowerText72);

		this.pfDanTitles = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.DanResult_Font_DanTitles_Size);

		base.CreateManagedResource();
	}
	public override void ReleaseManagedResource()
	{

		if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Tower)
		{
			OpenNijiiroRW.tDisposeSafely(ref pfTowerText);
			OpenNijiiroRW.tDisposeSafely(ref pfTowerText48);
			OpenNijiiroRW.tDisposeSafely(ref pfTowerText72);
		}
		else if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan)
		{
			OpenNijiiroRW.tDisposeSafely(ref pfDanTitles);
		}

		base.ReleaseManagedResource();
	}
	public override int Draw()
	{
		if (!base.IsDeActivated)
		{

			ctShine_Plate.TickLoop();

			// int num;

			if (base.IsFirstDraw)
			{
				this.ct登場用 = new CCounter(0, 100, 5, OpenNijiiroRW.Timer);
				this.actFIFromGameplay.tフェードイン開始();
				base.ePhaseID = CStage.EPhase.Common_FADEIN;

				if (this.rResultSound != null)
				{
					this.rResultSound.PlayStart();
				}

				base.IsFirstDraw = false;
			}
			this.bアニメが完了 = true;
			if (this.ct登場用.IsTicked)
			{
				this.ct登場用.Tick();
				if (this.ct登場用.IsEnded)
				{
					this.ct登場用.Stop();
				}
				else
				{
					this.bアニメが完了 = false;
				}
			}

			// 描画

			Background?.Update();
			Background?.Draw();

			if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Dan && OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Tower)
			{
				#region [Ensou game result screen]

				if (!b音声再生 && !bgmResultIn.bIsPlaying)
				{
					bgmResultLoop.tPlay();
					b音声再生 = true;
				}
				if (!OpenNijiiroRW.ConfigIni.bAIBattleMode)
				{
					/*
					if (TJAPlayer3.Tx.Result_Background != null)
					{
						bool is1P = (TJAPlayer3.ConfigIni.nPlayerCount == 1);
						bool is2PSide = TJAPlayer3.P1IsBlue();
						int mountainTexId = (is2PSide && is1P) ? 2 : 0;

						int CloudType = 0;
						float MountainAppearValue = this.actParameterPanel.MountainAppearValue;

						//2000 + (this.actParameterPanel.ctゲージアニメ.n終了値 * 66) + 8360 - 85;
						int gaugeAnimFactors = 0;

						if (this.actParameterPanel.ct全体進行.n現在の値 >= MountainAppearValue && is1P)
						{
							#region [Mountain Bump (1P only)]

							if (bClear[0])
							{
								//int gaugeAnimationFactor = (this.actParameterPanel.ct全体進行.n現在の値 - (10275 + ((int)this.actParameterPanel.ctゲージアニメ[0].n終了値 * 66))) * 3;

								gaugeAnimFactors = (this.actParameterPanel.ct全体進行.n現在の値 - (int)MountainAppearValue) * 3;

								TJAPlayer3.Tx.Result_Background[1].Opacity = gaugeAnimFactors;

								TJAPlayer3.Tx.Result_Mountain[mountainTexId + 1].Opacity = gaugeAnimFactors;
								TJAPlayer3.Tx.Result_Mountain[mountainTexId + 0].Opacity = 255 - gaugeAnimFactors;

								if (this.actParameterPanel.ctMountain_ClearIn.n現在の値 <= 90)
								{
									TJAPlayer3.Tx.Result_Mountain[mountainTexId + 1].vc拡大縮小倍率.Y = 1.0f - (float)Math.Sin((float)this.actParameterPanel.ctMountain_ClearIn.n現在の値 * (Math.PI / 180)) * 0.18f;
								}
								else if (this.actParameterPanel.ctMountain_ClearIn.n現在の値 <= 225)
								{
									TJAPlayer3.Tx.Result_Mountain[mountainTexId + 1].vc拡大縮小倍率.Y = 0.82f + (float)Math.Sin((float)(this.actParameterPanel.ctMountain_ClearIn.n現在の値 - 90) / 1.5f * (Math.PI / 180)) * 0.58f;
								}
								else if (this.actParameterPanel.ctMountain_ClearIn.n現在の値 <= 245)
								{
									TJAPlayer3.Tx.Result_Mountain[mountainTexId + 1].vc拡大縮小倍率.Y = 1.4f;
								}
								else if (this.actParameterPanel.ctMountain_ClearIn.n現在の値 <= 335)
								{
									TJAPlayer3.Tx.Result_Mountain[mountainTexId + 1].vc拡大縮小倍率.Y = 0.9f + (float)Math.Sin((float)(this.actParameterPanel.ctMountain_ClearIn.n現在の値 - 155) * (Math.PI / 180)) * 0.5f;
								}
								else if (this.actParameterPanel.ctMountain_ClearIn.n現在の値 <= 515)
								{
									TJAPlayer3.Tx.Result_Mountain[mountainTexId + 1].vc拡大縮小倍率.Y = 0.9f + (float)Math.Sin((float)(this.actParameterPanel.ctMountain_ClearIn.n現在の値 - 335) * (Math.PI / 180)) * 0.4f;
								}
							}

							#endregion
						}
						else if (is1P)
						{

							TJAPlayer3.Tx.Result_Background[1].Opacity = 0;
							TJAPlayer3.Tx.Result_Mountain[mountainTexId + 0].Opacity = 255;
							TJAPlayer3.Tx.Result_Mountain[mountainTexId + 1].Opacity = 0;
						}

						#region [Display background]

						if (is1P)
						{
							if (is2PSide)
								TJAPlayer3.Tx.Result_Background[2].t2D描画(0, 0);
							else
								TJAPlayer3.Tx.Result_Background[0].t2D描画(0, 0);
							TJAPlayer3.Tx.Result_Background[1].t2D描画(0, 0);
						}
						else
						{
							if (TJAPlayer3.ConfigIni.nPlayerCount <= 2)
							{
								gaugeAnimFactors = (this.actParameterPanel.ct全体進行.n現在の値 - (int)MountainAppearValue) * 3;

								int width1 = TJAPlayer3.Tx.Result_Background[1].szテクスチャサイズ.Width / 2;
								int height1 = TJAPlayer3.Tx.Result_Background[1].szテクスチャサイズ.Height;

								for (int i = 0; i < 2; i++)
								{
									int width2 = TJAPlayer3.Tx.Result_Background[2 * i].szテクスチャサイズ.Width / 2;
									int height2 = TJAPlayer3.Tx.Result_Background[2 * i].szテクスチャサイズ.Height;
									TJAPlayer3.Tx.Result_Background[2 * i].t2D描画(width2 * i, 0, new Rectangle(width2 * i, 0, width2, height2));

									if (bClear[i])
									{
										TJAPlayer3.Tx.Result_Background[1].Opacity = gaugeAnimFactors;
										TJAPlayer3.Tx.Result_Background[1].t2D描画(width1 * i, 0, new Rectangle(width1 * i, 0, width1, height1));
									}
								}
							}
							else
							{
								CTexture[] texs = new CTexture[5] {
								TJAPlayer3.Tx.Result_Background[0],
								TJAPlayer3.Tx.Result_Background[2],
								TJAPlayer3.Tx.Result_Background[3],
								TJAPlayer3.Tx.Result_Background[4],
								TJAPlayer3.Tx.Result_Background[5]
							};
								int count = Math.Max(TJAPlayer3.ConfigIni.nPlayerCount, 4);
								for (int i = 0; i < count; i++)
								{
									if (bClear[i])
									{
										gaugeAnimFactors = (this.actParameterPanel.ct全体進行.n現在の値 - (int)MountainAppearValue) * 3;
										TJAPlayer3.Tx.Result_Background[1].Opacity = gaugeAnimFactors;
									}
									int width = texs[i].szテクスチャサイズ.Width / count;
									texs[i].t2D描画(width * i, 0, new RectangleF(width * i, 0, width, texs[i].szテクスチャサイズ.Height));
									if (bClear[i])
										TJAPlayer3.Tx.Result_Background[1].t2D描画(width * i, 0, new RectangleF(width * i, 0, width, texs[i].szテクスチャサイズ.Height));
								}
							}

						}

						#endregion

						if (is1P)
						{
							TJAPlayer3.Tx.Result_Mountain[mountainTexId + 0].t2D描画(0, 0);
							TJAPlayer3.Tx.Result_Mountain[mountainTexId + 1].t2D拡大率考慮下基準描画(0, TJAPlayer3.Tx.Result_Mountain[mountainTexId + 1].szテクスチャサイズ.Height);

							// TJAPlayer3.act文字コンソール.tPrint(0, 0, C文字コンソール.Eフォント種別.白, ctShine_Plate.n現在の値.ToString());
							// TJAPlayer3.act文字コンソール.tPrint(10, 10, C文字コンソール.Eフォント種別.白, this.actParameterPanel.ct全体進行.n現在の値.ToString());

							#region [Background Clouds]

							if (bClear[0] && this.actParameterPanel.ct全体進行.n現在の値 >= MountainAppearValue)
							{
								CloudType = Math.Min(255, Math.Max(0, (int)this.actParameterPanel.ct全体進行.n現在の値 - (int)MountainAppearValue));
							}

							int cloud_width = TJAPlayer3.Tx.Result_Cloud.szテクスチャサイズ.Width / TJAPlayer3.Skin.Result_Cloud_Count;
							int cloud_height = TJAPlayer3.Tx.Result_Cloud.szテクスチャサイズ.Height / 3;

							for (int i = TJAPlayer3.Skin.Result_Cloud_Count - 1; i >= 0; i--)
							{
								int CurMoveRed = (int)((double)TJAPlayer3.Skin.Result_Cloud_MaxMove[i] * Math.Tanh((double)this.actParameterPanel.ct全体進行.n現在の値 / 10000));
								int CurMoveGold = (int)((double)TJAPlayer3.Skin.Result_Cloud_MaxMove[i] * Math.Tanh(Math.Max(0, (double)this.actParameterPanel.ct全体進行.n現在の値 - (double)MountainAppearValue) / 10000));

								int cloudOffset = (is2PSide) ? cloud_height * 2 : 0;

								TJAPlayer3.Tx.Result_Cloud.vc拡大縮小倍率.X = 0.65f;
								TJAPlayer3.Tx.Result_Cloud.vc拡大縮小倍率.Y = 0.65f;
								TJAPlayer3.Tx.Result_Cloud.Opacity = CloudType;

								TJAPlayer3.Tx.Result_Cloud.t2D拡大率考慮中央基準描画(TJAPlayer3.Skin.Result_Cloud_X[i] - CurMoveGold, TJAPlayer3.Skin.Result_Cloud_Y[i],
									new Rectangle(i * cloud_width, cloud_height, cloud_width, cloud_height));

								TJAPlayer3.Tx.Result_Cloud.Opacity = 255 - CloudType;

								TJAPlayer3.Tx.Result_Cloud.t2D拡大率考慮中央基準描画(TJAPlayer3.Skin.Result_Cloud_X[i] - CurMoveRed, TJAPlayer3.Skin.Result_Cloud_Y[i],
									new Rectangle(i * cloud_width, cloudOffset, cloud_width, cloud_height));
							}

							#endregion

							if (bClear[0] && this.actParameterPanel.ct全体進行.n現在の値 >= MountainAppearValue)
							{

								#region [Background shines]

								int ShineTime = (int)ctShine_Plate.n現在の値;
								int Quadrant500 = ShineTime % 500;

								for (int i = 0; i < TJAPlayer3.Skin.Result_Shine_Count; i++)
								{
									if (i < 2 && ShineTime >= 500 || i >= 2 && ShineTime < 500)
										TJAPlayer3.Tx.Result_Shine.Opacity = 0;
									else if (Quadrant500 >= ShinePFade && Quadrant500 <= 500 - ShinePFade)
										TJAPlayer3.Tx.Result_Shine.Opacity = 255;
									else
										TJAPlayer3.Tx.Result_Shine.Opacity = (255 * Math.Min(Quadrant500, 500 - Quadrant500)) / ShinePFade;

									TJAPlayer3.Tx.Result_Shine.vc拡大縮小倍率.X = TJAPlayer3.Skin.Result_Shine_Size[i];
									TJAPlayer3.Tx.Result_Shine.vc拡大縮小倍率.Y = TJAPlayer3.Skin.Result_Shine_Size[i];

									TJAPlayer3.Tx.Result_Shine.t2D中心基準描画(TJAPlayer3.Skin.Result_Shine_X[is2PSide ? 1 : 0][i], TJAPlayer3.Skin.Result_Shine_Y[is2PSide ? 1 : 0][i]);
								}

								#endregion

								#region [Fireworks]

								// Primary pop
								if (this.actParameterPanel.ct全体進行.n現在の値 <= MountainAppearValue + 1000)
								{
									for (int i = 0; i < 3; i++)
									{
										if (this.actParameterPanel.ct全体進行.n現在の値 <= MountainAppearValue + 255)
										{
											int TmpTimer = (int)(this.actParameterPanel.ct全体進行.n現在の値 - MountainAppearValue);

											TJAPlayer3.Tx.Result_Work[i].Opacity = TmpTimer;
											TJAPlayer3.Tx.Result_Work[i].vc拡大縮小倍率.X = 0.6f * ((float)TmpTimer / 225f);
											TJAPlayer3.Tx.Result_Work[i].vc拡大縮小倍率.Y = 0.6f * ((float)TmpTimer / 225f);
										}
										else
										{
											int TmpTimer = Math.Max(0, (2 * 255) - (int)(this.actParameterPanel.ct全体進行.n現在の値 - MountainAppearValue - 255));

											TJAPlayer3.Tx.Result_Work[i].Opacity = TmpTimer / 2;
											TJAPlayer3.Tx.Result_Work[i].vc拡大縮小倍率.X = 0.6f;
											TJAPlayer3.Tx.Result_Work[i].vc拡大縮小倍率.Y = 0.6f;
										}

										TJAPlayer3.Tx.Result_Work[i].t2D拡大率考慮中央基準描画(TJAPlayer3.Skin.Result_Work_X[is2PSide ? 1 : 0][i], TJAPlayer3.Skin.Result_Work_Y[is2PSide ? 1 : 0][i]);
									}
								}
								else
								{
									ctWork_Plate.t進行Loop();

									for (int i = 0; i < 3; i++)
									{
										int TmpStamp = WorksTimeStamp[i];

										if (ctWork_Plate.n現在の値 <= TmpStamp + 255)
										{
											int TmpTimer = (int)(ctWork_Plate.n現在の値 - TmpStamp);

											TJAPlayer3.Tx.Result_Work[i].Opacity = TmpTimer;
											TJAPlayer3.Tx.Result_Work[i].vc拡大縮小倍率.X = 0.6f * ((float)TmpTimer / 225f);
											TJAPlayer3.Tx.Result_Work[i].vc拡大縮小倍率.Y = 0.6f * ((float)TmpTimer / 225f);
										}
										else
										{
											int TmpTimer = Math.Max(0, (2 * 255) - (int)(ctWork_Plate.n現在の値 - TmpStamp - 255));

											TJAPlayer3.Tx.Result_Work[i].Opacity = TmpTimer / 2;
											TJAPlayer3.Tx.Result_Work[i].vc拡大縮小倍率.X = 0.6f;
											TJAPlayer3.Tx.Result_Work[i].vc拡大縮小倍率.Y = 0.6f;
										}

										TJAPlayer3.Tx.Result_Work[i].t2D拡大率考慮中央基準描画(TJAPlayer3.Skin.Result_Work_X[is2PSide ? 1 : 0][i], TJAPlayer3.Skin.Result_Work_Y[is2PSide ? 1 : 0][i]);
									}
								}

								#endregion

							}
						}
					}
					*/
					if (OpenNijiiroRW.Tx.Result_Header != null)
					{
						OpenNijiiroRW.Tx.Result_Header.t2D描画(0, 0);
					}
				}

				if (this.ct登場用.IsTicked && (OpenNijiiroRW.Tx.Result_Header != null))
				{
					double num2 = ((double)this.ct登場用.CurrentValue) / 100.0;
					double num3 = Math.Sin(Math.PI / 2 * num2);

					// num = ((int)(TJAPlayer3.Tx.Result_Header.sz画像サイズ.Height * num3)) - TJAPlayer3.Tx.Result_Header.sz画像サイズ.Height;
				}
				/*
				else
				{
					num = 0;
				}
				*/

				if (!b音声再生 && !bgmResultIn.bIsPlaying)
				{
					bgmResultLoop.tPlay();
					b音声再生 = true;
				}


				#endregion

			}
			else
			{
				if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan)
				{
					double screen_ratio_x = OpenNijiiroRW.Skin.Resolution[0] / 1280.0;

					#region [Counter processings]

					int songCount = OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs.Count;

					/*
					 **	1600 => Dan plate
					 **  3200 + 300 * count => Songs display
					 **  5500 + 300 * count => Exams plate display
					 **	8200 + 300 * count => Goukaku/Fugoukaku display => Step 2 (Prompt the user to tap enter and let them swaping between informations hitting kas)
					 **  ??? => Success/Fail animation
					 */
					if (ctPhase1 == null)
					{
						ctPhase1 = new CCounter(0, 8200 + songCount * 300, 0.5f, OpenNijiiroRW.Timer);
						ctPhase1.CurrentValue = 0;
					}

					ctPhase1.Tick();

					if (ctPhase2 != null)
						ctPhase2.Tick();

					#endregion


					#region [DaniDoujou result screen]

					if (!b音声再生 && !OpenNijiiroRW.Skin.bgmDanResult.bIsPlaying)
					{
						OpenNijiiroRW.Skin.bgmDanResult.tPlay();
						b音声再生 = true;
					}

					//DanResult_Background.t2D描画(0, 0);
					OpenNijiiroRW.Tx.DanResult_SongPanel_Base.t2D描画(0, 0);

					#region [DanPlate]

					// To add : Animation at 1 sec

					Dan_Plate?.t2D中心基準描画(138, 220);

					int plateOffset = Math.Max(0, 1600 - ctPhase1.CurrentValue) * 2;

					CActSelect段位リスト.tDisplayDanPlate(Dan_Plate,
						null,
						138,
						220 - plateOffset);

					#endregion

					#region [Charts Individual Results]

					ctDanSongInfoChange.Tick();

					if (ctDanSongInfoChange.CurrentValue == ctDanSongInfoChange.EndValue && songCount > 3)
					{
						NextDanSongInfo();
					}
					else if (nNowDanSongInfo > 0 && songCount <= 3)
					{
						nNowDanSongInfo = 0;
					}

					for (int i = 0; i < songCount; i++)
					{
						int songOffset = (int)(Math.Max(0, 3200 + 300 * i - ctPhase1.CurrentValue) * screen_ratio_x);

						int quadrant = i / 3;
						if (quadrant == nNowDanSongInfo)
							ftDanDisplaySongInfo(i, songOffset);
					}

					#endregion

					#region [Exam informations]

					int examsOffset = 0;

					if (ctPhase2 != null && examsShift != 0)
						examsOffset = (examsShift < 0) ? 1280 - ctPhase2.CurrentValue : ctPhase2.CurrentValue;
					else if (ctPhase1.IsUnEnded)
						examsOffset = Math.Max(0, 5500 + 300 * songCount - ctPhase1.CurrentValue) * 2;

					examsOffset = (int)(examsOffset * screen_ratio_x);

					ftDanDisplayExamInfo(examsOffset);

					#endregion

					#region [PassLogo]

					Exam.Status examStatus = OpenNijiiroRW.stageGameScreen.actDan.GetResultExamStatus(this.st演奏記録.Drums.Dan_C, OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs);

					int unitsBeforeAppearance = Math.Max(0, 8200 + 300 * songCount - ctPhase1.CurrentValue);

					if (unitsBeforeAppearance <= 270)
					{
						OpenNijiiroRW.Tx.DanResult_Rank.Opacity = 255;

						int rank_width = OpenNijiiroRW.Tx.DanResult_Rank.szTextureSize.Width / 7;
						int rank_height = OpenNijiiroRW.Tx.DanResult_Rank.szTextureSize.Height;

						if (examStatus != Exam.Status.Failure)
						{
							#region [Goukaku]

							#region [ Appear animation ]

							if (unitsBeforeAppearance >= 90)
							{
								OpenNijiiroRW.Tx.DanResult_Rank.Opacity = (int)((270 - unitsBeforeAppearance) / 180.0f * 255.0f);
								OpenNijiiroRW.Tx.DanResult_Rank.Scale.X = 1.0f + (float)Math.Sin((360 - unitsBeforeAppearance) / 1.5f * (Math.PI / 180)) * 1.4f;
								OpenNijiiroRW.Tx.DanResult_Rank.Scale.Y = 1.0f + (float)Math.Sin((360 - unitsBeforeAppearance) / 1.5f * (Math.PI / 180)) * 1.4f;
							}
							else if (unitsBeforeAppearance > 0)
							{
								OpenNijiiroRW.Tx.Result_ScoreRankEffect.Scale.X = 0.5f + (float)Math.Sin((float)(90 - unitsBeforeAppearance) * (Math.PI / 180)) * 0.5f;
								OpenNijiiroRW.Tx.Result_ScoreRankEffect.Scale.Y = 0.5f + (float)Math.Sin((float)(90 - unitsBeforeAppearance) * (Math.PI / 180)) * 0.5f;
							}
							else
							{
								OpenNijiiroRW.Tx.DanResult_Rank.Scale.X = 1f;
								OpenNijiiroRW.Tx.DanResult_Rank.Scale.Y = 1f;
							}

							#endregion

							#region [ Goukaku plate type calculus ]

							int successType = 0;

							if (examStatus == Exam.Status.Better_Success)
								successType += 1;

							int comboType = 0;
							if (this.st演奏記録.Drums.nBadCount == 0)
							{
								comboType += 1;

								if (this.st演奏記録.Drums.nOkCount == 0)
									comboType += 1;
							}

							#endregion

							OpenNijiiroRW.Tx.DanResult_Rank.t2D拡大率考慮中央基準描画(OpenNijiiroRW.Skin.DanResult_Rank[0], OpenNijiiroRW.Skin.DanResult_Rank[1],
								new Rectangle(rank_width * (2 * comboType + successType + 1), 0, rank_width, rank_height));

							#endregion
						}
						else
						{
							#region [Fugoukaku]

							#region [ Appear animation ]

							if (unitsBeforeAppearance >= 90)
							{
								OpenNijiiroRW.Tx.DanResult_Rank.Opacity = (int)((270 - unitsBeforeAppearance) / 180.0f * 255.0f);
							}

							OpenNijiiroRW.Tx.DanResult_Rank.Scale.X = 1f;
							OpenNijiiroRW.Tx.DanResult_Rank.Scale.Y = 1f;

							#endregion

							OpenNijiiroRW.Tx.DanResult_Rank.t2D拡大率考慮中央基準描画(OpenNijiiroRW.Skin.DanResult_Rank[0], OpenNijiiroRW.Skin.DanResult_Rank[1] - (unitsBeforeAppearance / 10f),
								new Rectangle(0, 0, rank_width, rank_height));

							#endregion
						}
					}

					#endregion

					if (!b音声再生 && !OpenNijiiroRW.Skin.bgmDanResult.bIsPlaying)
					{
						OpenNijiiroRW.Skin.bgmDanResult.tPlay();
						b音声再生 = true;
					}

					#endregion

				}
				else
				{
					#region [Tower result screen]

					if (!b音声再生 && !OpenNijiiroRW.Skin.bgmTowerResult.bIsPlaying)
					{
						OpenNijiiroRW.Skin.bgmTowerResult.tPlay();
						b音声再生 = true;
					}

					// Pictures here

					this.ctTower_Animation.Tick();

					#region [Tower background]

					if (OpenNijiiroRW.Skin.Game_Tower_Ptn_Result > 0)
					{
						int xFactor = 0;
						float yFactor = 1f;

						int currentTowerType = Array.IndexOf(OpenNijiiroRW.Skin.Game_Tower_Names, OpenNijiiroRW.stageSongSelect.rChoosenSong.score[5].譜面情報.nTowerType);

						if (currentTowerType < 0 || currentTowerType >= OpenNijiiroRW.Skin.Game_Tower_Ptn_Result)
							currentTowerType = 0;

						if (OpenNijiiroRW.Tx.TowerResult_Background != null && OpenNijiiroRW.Tx.TowerResult_Tower[currentTowerType] != null)
						{
							xFactor = (OpenNijiiroRW.Tx.TowerResult_Background.szTextureSize.Width - OpenNijiiroRW.Tx.TowerResult_Tower[currentTowerType].szTextureSize.Width) / 2;
							yFactor = OpenNijiiroRW.Tx.TowerResult_Tower[currentTowerType].szTextureSize.Height / (float)OpenNijiiroRW.Tx.TowerResult_Background.szTextureSize.Height;
						}

						OpenNijiiroRW.Tx.TowerResult_Background?.t2D描画(0, -1 * this.ctTower_Animation.CurrentValue);
						OpenNijiiroRW.Tx.TowerResult_Tower[currentTowerType]?.t2D描画(xFactor, -1 * yFactor * this.ctTower_Animation.CurrentValue);
					}

					#endregion

					OpenNijiiroRW.Tx.TowerResult_Panel?.t2D描画(0, 0);

					#region [Score Rank]

					int sc = GetTowerScoreRank() - 1;

					int y = 2 * OpenNijiiroRW.actTextConsole.fontHeight + 8;
					OpenNijiiroRW.actTextConsole.Print(0, y, CTextConsole.EFontType.White, sc.ToString());

					if (sc >= 0 && OpenNijiiroRW.Tx.TowerResult_ScoreRankEffect != null)
					{
						int scoreRankEffect_width = OpenNijiiroRW.Tx.TowerResult_ScoreRankEffect.szTextureSize.Width / 7;
						int scoreRankEffect_height = OpenNijiiroRW.Tx.TowerResult_ScoreRankEffect.szTextureSize.Height;

						OpenNijiiroRW.Tx.TowerResult_ScoreRankEffect.Opacity = 255;
						OpenNijiiroRW.Tx.TowerResult_ScoreRankEffect.Scale.X = 1f;
						OpenNijiiroRW.Tx.TowerResult_ScoreRankEffect.Scale.Y = 1f;
						OpenNijiiroRW.Tx.TowerResult_ScoreRankEffect.t2D拡大率考慮中央基準描画(
							OpenNijiiroRW.Skin.TowerResult_ScoreRankEffect[0],
							OpenNijiiroRW.Skin.TowerResult_ScoreRankEffect[1],
							new Rectangle(sc * scoreRankEffect_width, 0, scoreRankEffect_width, scoreRankEffect_height));
					}


					#endregion


					#region [Text elements]

					TitleTextureKey.ResolveTitleTexture(this.ttkToutatsu)?.t2D描画(OpenNijiiroRW.Skin.TowerResult_Toutatsu[0], OpenNijiiroRW.Skin.TowerResult_Toutatsu[1]);
					TitleTextureKey.ResolveTitleTexture(this.ttkMaxFloors)?.t2D描画(OpenNijiiroRW.Skin.TowerResult_MaxFloors[0], OpenNijiiroRW.Skin.TowerResult_MaxFloors[1]);
					TitleTextureKey.ResolveTitleTexture(this.ttkTen)?.t2D描画(OpenNijiiroRW.Skin.TowerResult_Ten[0], OpenNijiiroRW.Skin.TowerResult_Ten[1]);
					TitleTextureKey.ResolveTitleTexture(this.ttkScore)?.t2D描画(OpenNijiiroRW.Skin.TowerResult_Score[0], OpenNijiiroRW.Skin.TowerResult_Score[1]);

					CTexture tmpScoreCount = TitleTextureKey.ResolveTitleTexture(this.ttkScoreCount);
					CTexture tmpCurrentFloor = TitleTextureKey.ResolveTitleTexture(this.ttkReachedFloor);
					CTexture tmpRemainingLifes = TitleTextureKey.ResolveTitleTexture(this.ttkRemaningLifes);

					tmpCurrentFloor?.t2D描画(OpenNijiiroRW.Skin.TowerResult_CurrentFloor[0] - tmpCurrentFloor.szTextureSize.Width, OpenNijiiroRW.Skin.TowerResult_CurrentFloor[1]);
					tmpScoreCount?.t2D描画(OpenNijiiroRW.Skin.TowerResult_ScoreCount[0] - tmpScoreCount.szTextureSize.Width, OpenNijiiroRW.Skin.TowerResult_ScoreCount[1]);
					tmpRemainingLifes?.t2D描画(OpenNijiiroRW.Skin.TowerResult_RemainingLifes[0] - tmpRemainingLifes.szTextureSize.Width, OpenNijiiroRW.Skin.TowerResult_RemainingLifes[1]);

					int soul_width = OpenNijiiroRW.Tx.Gauge_Soul.szTextureSize.Width;
					int soul_height = OpenNijiiroRW.Tx.Gauge_Soul.szTextureSize.Height / 2;

					OpenNijiiroRW.Tx.Gauge_Soul?.t2D描画(OpenNijiiroRW.Skin.TowerResult_Gauge_Soul[0], OpenNijiiroRW.Skin.TowerResult_Gauge_Soul[1], new Rectangle(0, 0, soul_width, soul_height));

					#endregion

					if (!b音声再生 && !OpenNijiiroRW.Skin.bgmTowerResult.bIsPlaying)
					{
						OpenNijiiroRW.Skin.bgmTowerResult.tPlay();
						b音声再生 = true;
					}


					#endregion
				}


			}

			// Display medals debug

			// TJAPlayer3.act文字コンソール.tPrint(0, 12, C文字コンソール.Eフォント種別.白, this.nEarnedMedalsCount[0].ToString());
			//TJAPlayer3.act文字コンソール.tPrint(0, 25, C文字コンソール.Eフォント種別.白, this.nEarnedMedalsCount[1].ToString());



			if (this.actParameterPanel.Draw() == 0)
			{
				this.bアニメが完了 = false;
			}

			if (this.actSongBar.Draw() == 0)
			{
				this.bアニメが完了 = false;
			}

			#region Nameplate

			for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
			{
				if (OpenNijiiroRW.ConfigIni.bAIBattleMode && i == 1) break;

				int pos = i;
				if (OpenNijiiroRW.P1IsBlue() && OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] < (int)Difficulty.Tower)
					pos = 1;

				int namePlate_x;
				int namePlate_y;
				int modIcons_x;
				int modIcons_y;

				if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
				{
					namePlate_x = OpenNijiiroRW.Skin.Result_NamePlate_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
					namePlate_y = OpenNijiiroRW.Skin.Result_NamePlate_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];
					modIcons_x = OpenNijiiroRW.Skin.Result_ModIcons_5P[0] + OpenNijiiroRW.Skin.Result_UIMove_5P_X[pos];
					modIcons_y = OpenNijiiroRW.Skin.Result_ModIcons_5P[1] + OpenNijiiroRW.Skin.Result_UIMove_5P_Y[pos];
				}
				else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 4 || OpenNijiiroRW.ConfigIni.nPlayerCount == 3)
				{
					namePlate_x = OpenNijiiroRW.Skin.Result_NamePlate_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
					namePlate_y = OpenNijiiroRW.Skin.Result_NamePlate_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];
					modIcons_x = OpenNijiiroRW.Skin.Result_ModIcons_4P[0] + OpenNijiiroRW.Skin.Result_UIMove_4P_X[pos];
					modIcons_y = OpenNijiiroRW.Skin.Result_ModIcons_4P[1] + OpenNijiiroRW.Skin.Result_UIMove_4P_Y[pos];
				}
				else
				{
					namePlate_x = OpenNijiiroRW.Skin.Result_NamePlate_X[pos];
					namePlate_y = OpenNijiiroRW.Skin.Result_NamePlate_Y[pos];
					modIcons_x = OpenNijiiroRW.Skin.Result_ModIcons_X[pos];
					modIcons_y = OpenNijiiroRW.Skin.Result_ModIcons_Y[pos];
				}

				OpenNijiiroRW.NamePlate.tNamePlateDraw(namePlate_x, namePlate_y, i);

				#region Mods

				ModIcons.tDisplayModsMenu(modIcons_x, modIcons_y, i);

				#endregion
			}

			#endregion




			#region [Display modals]

			OpenNijiiroRW.ModalManager.Draw();

			#endregion

			if (base.ePhaseID == CStage.EPhase.Common_FADEIN)
			{
				if (this.actFIFromGameplay.Draw() != 0)
				{
					base.ePhaseID = CStage.EPhase.Common_NORMAL;
				}
			}
			else if ((base.ePhaseID == CStage.EPhase.Common_FADEOUT))         //&& ( this.actFO.On進行描画() != 0 ) )
			{
				if (this.actFO.Draw() != 0)
				{
					bgmResultLoop.tStop();
					OpenNijiiroRW.Skin.bgmDanResult.tStop();
					OpenNijiiroRW.Skin.bgmTowerResult.tStop();
					return (int)this.eフェードアウト完了時の戻り値;
				}
			}

			#region [ #24609 2011.3.14 yyagi ランク更新or演奏型スキル更新時、リザルト画像をpngで保存する ]
			if (this.bアニメが完了 == true && this.bIsCheckedWhetherResultScreenShouldSaveOrNot == false  // #24609 2011.3.14 yyagi; to save result screen in case BestRank or HiSkill.
									 && OpenNijiiroRW.ConfigIni.bIsAutoResultCapture)                                               // #25399 2011.6.9 yyagi
			{
				string strFullPath =
					Path.Combine(OpenNijiiroRW.strEXEのあるフォルダ, "Capture_img");
				strFullPath = Path.Combine(strFullPath, DateTime.Now.ToString("yyyyMMddHHmmss") + ".png");
				OpenNijiiroRW.app.SaveResultScreen(strFullPath);

				this.bIsCheckedWhetherResultScreenShouldSaveOrNot = true;
			}
			#endregion

			// キー入力

			if (base.ePhaseID == CStage.EPhase.Common_NORMAL)
			{
				if (OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Escape))
				{
					#region [ Return to song select screen (Faster method) ]

					OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
					actFO.tフェードアウト開始();

					if (OpenNijiiroRW.latestSongSelect == OpenNijiiroRW.stageSongSelect)// TJAPlayer3.stage選曲.n確定された曲の難易度[0] != (int)Difficulty.Dan)
						if (OpenNijiiroRW.stageSongSelect.rNowSelectedSong.rParentNode != null)
							OpenNijiiroRW.stageSongSelect.actSongList.tCloseBOX();

					tPostprocessing();
					base.ePhaseID = CStage.EPhase.Common_FADEOUT;
					this.eフェードアウト完了時の戻り値 = E戻り値.完了;

					#endregion
				}
				if (((OpenNijiiroRW.Pad.bPressedDGB(EPad.CY)
					  || OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.RD))
					 || (OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.LC)
						 || (OpenNijiiroRW.Pad.bPressedDGB(EPad.Decide)
							 || OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Return)))))
				{


					#region [ Skip animations ]

					if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] < (int)Difficulty.Tower
						&& this.actParameterPanel.ctMainCounter.CurrentValue < this.actParameterPanel.MountainAppearValue)
					{
						OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
						this.actParameterPanel.tSkipResultAnimations();
					}
					else if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan
							   && (ctPhase1 != null && ctPhase1.IsUnEnded))
					{
						OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
						ctPhase1.CurrentValue = (int)ctPhase1.EndValue;
					}

					#endregion

					else
					{

						bool _modalsProcessed = OpenNijiiroRW.ModalManager.InputManagement();

						if (_modalsProcessed == true)
						{
							#region [ Return to song select screen ]

							actFO.tフェードアウト開始();

							tPostprocessing();

							{
								base.ePhaseID = CStage.EPhase.Common_FADEOUT;
								this.eフェードアウト完了時の戻り値 = E戻り値.完了;
							}

							#endregion
						}

					}
				}


				if (OpenNijiiroRW.InputManager.Keyboard.KeyPressing((int)SlimDXKeys.Key.LeftArrow) ||
					OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.LeftChange) ||
					OpenNijiiroRW.InputManager.Keyboard.KeyPressing((int)SlimDXKeys.Key.RightArrow) ||
					OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.RightChange))
				{
					if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan)
					{
						#region [ Phase 2 (Swap freely between Exams and Songs) ]

						if (ctPhase1 != null && ctPhase1.IsEnded && (ctPhase2 == null || ctPhase2.IsEnded))
						{
							ctPhase2 = new CCounter(0, 1280, 0.5f, OpenNijiiroRW.Timer);
							ctPhase2.CurrentValue = 0;

							if (examsShift == 0)
								examsShift = 1;
							else
								examsShift = -examsShift;

							OpenNijiiroRW.Skin.soundChangeSFX.tPlay();
						}

						#endregion
					}
				}
			}
		}
		return 0;
	}

	#region [Dan result exam information]

	private void ftDanDisplayExamInfo(int offset = 0)
	{
		int baseX = OpenNijiiroRW.Skin.DanResult_StatePanel[0] + offset;
		int baseY = OpenNijiiroRW.Skin.DanResult_StatePanel[1];

		OpenNijiiroRW.Tx.DanResult_StatePanel_Base.t2D描画(baseX, baseY);
		OpenNijiiroRW.Tx.DanResult_StatePanel_Main.t2D描画(baseX, baseY);

		#region [ Global scores ]

		int totalHit = OpenNijiiroRW.stageGameScreen.CChartScore[0].nGreat
					   + OpenNijiiroRW.stageGameScreen.CChartScore[0].nGood
					   + OpenNijiiroRW.stageGameScreen.GetRoll(0);

		// Small digits
		this.actParameterPanel.t小文字表示(OpenNijiiroRW.Skin.DanResult_Perfect[0] + offset, OpenNijiiroRW.Skin.DanResult_Perfect[1],
			OpenNijiiroRW.stageGameScreen.CChartScore[0].nGreat, 1.0f);

		this.actParameterPanel.t小文字表示(OpenNijiiroRW.Skin.DanResult_Good[0] + offset, OpenNijiiroRW.Skin.DanResult_Good[1],
			OpenNijiiroRW.stageGameScreen.CChartScore[0].nGood, 1.0f);

		this.actParameterPanel.t小文字表示(OpenNijiiroRW.Skin.DanResult_Miss[0] + offset, OpenNijiiroRW.Skin.DanResult_Miss[1],
			OpenNijiiroRW.stageGameScreen.CChartScore[0].nMiss, 1.0f);

		this.actParameterPanel.t小文字表示(OpenNijiiroRW.Skin.DanResult_Roll[0] + offset, OpenNijiiroRW.Skin.DanResult_Roll[1],
			OpenNijiiroRW.stageGameScreen.CChartScore[0].nRoll, 1.0f);

		this.actParameterPanel.t小文字表示(OpenNijiiroRW.Skin.DanResult_MaxCombo[0] + offset, OpenNijiiroRW.Skin.DanResult_MaxCombo[1],
			OpenNijiiroRW.stageGameScreen.actCombo.nCurrentCombo.最高値[0], 1.0f);

		this.actParameterPanel.t小文字表示(OpenNijiiroRW.Skin.DanResult_TotalHit[0] + offset, OpenNijiiroRW.Skin.DanResult_TotalHit[1],
			totalHit, 1.0f);

		// Large digits
		this.actParameterPanel.tスコア文字表示(OpenNijiiroRW.Skin.DanResult_Score[0] + offset, OpenNijiiroRW.Skin.DanResult_Score[1], (int)OpenNijiiroRW.stageGameScreen.actScore.Get(0), 1.0f);

		#endregion

		#region [ Display exams ]

		OpenNijiiroRW.stageGameScreen.actDan.DrawExam(this.st演奏記録.Drums.Dan_C, OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs, true, offset);

		#endregion
	}

	#endregion


	#region [Dan result individual song information]

	private void ftDanDisplaySongInfo(int i, int offset = 0)
	{
		int drawPos = i % 3;
		int nowIndex = (i / 3);

		int opacityCounter = Math.Min(ctDanSongInfoChange.CurrentValue, 255);
		int opacity;

		if (nowIndex == nNowDanSongInfo)
		{
			opacity = opacityCounter;
		}
		else
		{
			opacity = 255 - opacityCounter;
		}

		/*
		int baseX = 255 + offset;
		int baseY = 100 + 183 * i;
		*/

		var song = OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs[i];

		// TJAPlayer3.Tx.Dani_Difficulty_Cymbol.t2D中心基準描画(scroll + 377, 180 + i * 73, new Rectangle(song.Difficulty * 53, 0, 53, 53));

		int songPanel_main_width = OpenNijiiroRW.Tx.DanResult_SongPanel_Main.szTextureSize.Width;
		int songPanel_main_height = OpenNijiiroRW.Tx.DanResult_SongPanel_Main.szTextureSize.Height / 3;

		OpenNijiiroRW.Tx.DanResult_SongPanel_Main.Opacity = opacity;
		OpenNijiiroRW.Tx.DanResult_SongPanel_Main.t2D描画(OpenNijiiroRW.Skin.DanResult_SongPanel_Main_X[drawPos] + offset, OpenNijiiroRW.Skin.DanResult_SongPanel_Main_Y[drawPos], new Rectangle(0, songPanel_main_height * Math.Min(i, 2), songPanel_main_width, songPanel_main_height));

		int difficulty_cymbol_width = OpenNijiiroRW.Tx.Dani_Difficulty_Cymbol.szTextureSize.Width / 5;
		int difficulty_cymbol_height = OpenNijiiroRW.Tx.Dani_Difficulty_Cymbol.szTextureSize.Height;

		OpenNijiiroRW.Tx.Dani_Difficulty_Cymbol.Opacity = opacity;
		OpenNijiiroRW.Tx.Dani_Difficulty_Cymbol.t2D中心基準描画(OpenNijiiroRW.Skin.DanResult_Difficulty_Cymbol_X[drawPos] + offset, OpenNijiiroRW.Skin.DanResult_Difficulty_Cymbol_Y[drawPos], new Rectangle(song.Difficulty * difficulty_cymbol_width, 0, difficulty_cymbol_width, difficulty_cymbol_height));
		OpenNijiiroRW.Tx.Dani_Difficulty_Cymbol.Opacity = 255;

		OpenNijiiroRW.Tx.Dani_Level_Number.Opacity = opacity;
		OpenNijiiroRW.stageDanSongSelect.段位リスト.tLevelNumberDraw(OpenNijiiroRW.Skin.DanResult_Level_Number_X[drawPos] + offset, OpenNijiiroRW.Skin.DanResult_Level_Number_Y[drawPos], song.Level);
		OpenNijiiroRW.Tx.Dani_Level_Number.Opacity = 255;

		int[] scoresArr =
		{
			OpenNijiiroRW.stageGameScreen.DanSongScore[i].nGreat,
			OpenNijiiroRW.stageGameScreen.DanSongScore[i].nGood,
			OpenNijiiroRW.stageGameScreen.DanSongScore[i].nMiss,
			OpenNijiiroRW.stageGameScreen.DanSongScore[i].nRoll
		};

		int[] num_x = {
			OpenNijiiroRW.Skin.DanResult_Sections_Perfect_X[drawPos],
			OpenNijiiroRW.Skin.DanResult_Sections_Good_X[drawPos],
			OpenNijiiroRW.Skin.DanResult_Sections_Miss_X[drawPos],
			OpenNijiiroRW.Skin.DanResult_Sections_Roll_X[drawPos],
		};

		int[] num_y = {
			OpenNijiiroRW.Skin.DanResult_Sections_Perfect_Y[drawPos],
			OpenNijiiroRW.Skin.DanResult_Sections_Good_Y[drawPos],
			OpenNijiiroRW.Skin.DanResult_Sections_Miss_Y[drawPos],
			OpenNijiiroRW.Skin.DanResult_Sections_Roll_Y[drawPos],
		};

		OpenNijiiroRW.Tx.Result_Number.Opacity = opacity;
		for (int j = 0; j < 4; j++)
			this.actParameterPanel.t小文字表示(num_x[j] + offset, num_y[j], scoresArr[j], 1.0f);
		OpenNijiiroRW.Tx.Result_Number.Opacity = 255;

		TitleTextureKey.ResolveTitleTexture(this.ttkDanTitles[i]).Opacity = opacity;
		TitleTextureKey.ResolveTitleTexture(this.ttkDanTitles[i]).t2D描画(OpenNijiiroRW.Skin.DanResult_DanTitles_X[drawPos] + offset, OpenNijiiroRW.Skin.DanResult_DanTitles_Y[drawPos]);

		CActSelect段位リスト.tDisplayDanIcon(i + 1, OpenNijiiroRW.Skin.DanResult_DanIcon_X[drawPos] + offset, OpenNijiiroRW.Skin.DanResult_DanIcon_Y[drawPos], opacity, 1.0f);

	}

	#endregion


	public void tPostprocessing()
	{

		if (!bAddedToRecentlyPlayedSongs)
		{
			// Song added to recently added songs here

			OpenNijiiroRW.RecentlyPlayedSongs.tAddChart(OpenNijiiroRW.stageSongSelect.rChoosenSong.uniqueId.data.id);

			bAddedToRecentlyPlayedSongs = true;
		}

	}

	public enum E戻り値 : int
	{
		継続,
		完了
	}

	// その他

	#region [ private ]
	//-----------------

	public bool bAddedToRecentlyPlayedSongs;
	public bool b音声再生;
	public bool EndAnime;

	private CCounter ct登場用;
	private E戻り値 eフェードアウト完了時の戻り値;
	private CActFIFOResult actFIFromGameplay;
	private CActFIFOBlack actFO;
	private CActオプションパネル actOption;
	private CActResultParameterPanel actParameterPanel;

	private CActResultSongBar actSongBar;
	private bool bアニメが完了;
	private bool bIsCheckedWhetherResultScreenShouldSaveOrNot;              // #24509 2011.3.14 yyagi
	private CSound rResultSound;
	public ResultBG Background;

	public bool[] bClear
	{
		get
		{
			if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
			{
				int clearCount = 0;
				for (int i = 0; i < OpenNijiiroRW.stageGameScreen.AIBattleSections.Count; i++)
				{
					if (OpenNijiiroRW.stageGameScreen.AIBattleSections[i].End == CStage演奏画面共通.AIBattleSection.EndType.Clear)
					{
						clearCount++;
					}
				}
				return new bool[] { clearCount >= OpenNijiiroRW.stageGameScreen.AIBattleSections.Count / 2.0, false };
			}
			else
			{
				return new bool[] { OpenNijiiroRW.stageGameScreen.bIsAlreadyCleared[0], OpenNijiiroRW.stageGameScreen.bIsAlreadyCleared[1], OpenNijiiroRW.stageGameScreen.bIsAlreadyCleared[2], OpenNijiiroRW.stageGameScreen.bIsAlreadyCleared[3], OpenNijiiroRW.stageGameScreen.bIsAlreadyCleared[4] };
			}
		}
	}

	private CCounter ctDanSongInfoChange;

	private int nNowDanSongInfo;

	private void NextDanSongInfo()
	{
		ctDanSongInfoChange = new CCounter(0, 2000, 1, OpenNijiiroRW.Timer);
		ctDanSongInfoChange.CurrentValue = 0;

		nNowDanSongInfo++;
		if (nNowDanSongInfo >= Math.Ceiling(OpenNijiiroRW.stageSongSelect.rChoosenSong.DanSongs.Count / 3.0))
		{
			nNowDanSongInfo = 0;
		}
	}

	// Cloud informations
	/*
	private int[] CloudXPos = { 642, 612, 652, 1148, 1180, 112, 8, 1088, 1100, 32, 412 };
	private int[] CloudYPos = { 202, 424, 636, 530, 636, 636, 102, 52, 108, 326, 644 };
	private int[] CloudMaxMove = { 150, 120, 180, 60, 90, 150, 120, 50, 45, 120, 180 };
	*/

	// Shines informations
	private CCounter ctShine_Plate;

	/*
	private int[] ShinePXPos = { 805, 1175, 645, 810, 1078, 1060 };
	private int[] ShinePYPos = { 650, 405, 645, 420, 202, 585 };

	private float[] ShinePSize = { 0.44f, 0.6f, 0.4f, 0.15f, 0.35f, 0.6f };
	*/

	private int ShinePFade = 100;

	// Fireworks informations
	private CCounter ctWork_Plate;

	/*
	private int[] WorksPosX = { 800, 900, 1160 };
	private int[] WorksPosY = { 435, 185, 260 };
	*/

	private int[] WorksTimeStamp = { 1000, 2000, 3000 };

	// Dan informations
	private CTexture Dan_Plate;
	private TitleTextureKey[] ttkDanTitles;
	private CCachedFontRenderer pfDanTitles;
	private CCounter ctPhase1; // Info display
	private CCounter ctPhase2; // Free swipe
	private CCounter ctPhase3; // Background & grade granted if changes
	private int examsShift = 0;
	private bool newGradeGranted = false;

	// Tower informations
	private CCounter ctTower_Animation;
	private TitleTextureKey ttkMaxFloors;
	private TitleTextureKey ttkToutatsu;
	private TitleTextureKey ttkTen;
	private TitleTextureKey ttkReachedFloor;
	private TitleTextureKey ttkScore;
	private TitleTextureKey ttkRemaningLifes;
	private TitleTextureKey ttkScoreCount;
	private CCachedFontRenderer pfTowerText;
	private CCachedFontRenderer pfTowerText48;
	private CCachedFontRenderer pfTowerText72;

	private CSkin.CSystemSound bgmResultIn
	{
		get
		{
			if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
			{
				return OpenNijiiroRW.Skin.bgmResultIn_AI;
			}
			else
			{
				return OpenNijiiroRW.Skin.bgmリザルトイン音;
			}
		}
	}

	private CSkin.CSystemSound bgmResultLoop
	{
		get
		{
			if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
			{
				return OpenNijiiroRW.Skin.bgmResult_AI;
			}
			else
			{
				return OpenNijiiroRW.Skin.bgmリザルト音;
			}
		}
	}

	// Coins information
	private int[] nEarnedMedalsCount = { 0, 0, 0, 0, 0 };


	//-----------------
	#endregion
}
