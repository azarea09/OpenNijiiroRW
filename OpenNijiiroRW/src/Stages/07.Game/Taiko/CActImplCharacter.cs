using FDK;


namespace OpenNijiiroRW;
//クラスの設置位置は必ず演奏画面共通に置くこと。
//そうしなければBPM変化に対応できません。

//完成している部分は以下のとおり。(画像完成+動作確認完了で完成とする)
//_通常モーション
//_ゴーゴータイムモーション
//_クリア時モーション
//
internal class CActImplCharacter : CActivity
{
	public CActImplCharacter()
	{

	}

	public override void Activate()
	{
		for (int i = 0; i < 5; i++)
		{

			/*
            ctChara_Normal[i] = new CCounter();
            ctChara_Miss[i] = new CCounter();
            ctChara_MissDown[i] = new CCounter();
            ctChara_GoGo[i] = new CCounter();
            ctChara_Clear[i] = new CCounter();

            this.ctキャラクターアクション_10コンボ[i] = new CCounter();
            this.ctキャラクターアクション_10コンボMAX[i] = new CCounter();
            this.ctキャラクターアクション_ゴーゴースタート[i] = new CCounter();
            this.ctキャラクターアクション_ゴーゴースタートMAX[i] = new CCounter();
            this.ctキャラクターアクション_ノルマ[i] = new CCounter();
            this.ctキャラクターアクション_魂MAX[i] = new CCounter();
            this.ctキャラクターアクション_Return[i] = new CCounter();
            */

			//CharaAction_Balloon_Breaking[i] = new CCounter();
			//CharaAction_Balloon_Broke[i] = new CCounter();
			//CharaAction_Balloon_Miss[i] = new CCounter();
			CharaAction_Balloon_Delay[i] = new CCounter();
			ctKusuIn[i] = new();

			// Currently used character
			int p = OpenNijiiroRW.GetActualPlayer(i);

			this.iCurrentCharacter[i] = Math.Max(0, Math.Min(OpenNijiiroRW.SaveFileInstances[p].data.Character, OpenNijiiroRW.Skin.Characters_Ptn - 1));

			if (OpenNijiiroRW.Skin.Characters_Normal_Ptn[this.iCurrentCharacter[i]] != 0) ChangeAnime(i, Anime.Normal, true);
			else ChangeAnime(i, Anime.None, true);

			this.b風船連打中[i] = false;
			this.b演奏中[i] = false;

			// CharaAction_Balloon_FadeOut[i] = new Animations.FadeOut(TJAPlayer3.Skin.Game_Chara_Balloon_FadeOut);
			CharaAction_Balloon_FadeOut[i] = new Animations.FadeOut(OpenNijiiroRW.Skin.Characters_Balloon_FadeOut[this.iCurrentCharacter[i]]);

			var tick = OpenNijiiroRW.Skin.Characters_Balloon_Timer[this.iCurrentCharacter[i]];

			var balloonBrokePtn = OpenNijiiroRW.Skin.Characters_Balloon_Broke_Ptn[this.iCurrentCharacter[i]];
			var balloonMissPtn = OpenNijiiroRW.Skin.Characters_Balloon_Miss_Ptn[this.iCurrentCharacter[i]];

			CharaAction_Balloon_FadeOut_StartMs[i] = new int[2];

			CharaAction_Balloon_FadeOut_StartMs[i][0] = (balloonBrokePtn * tick) - OpenNijiiroRW.Skin.Characters_Balloon_FadeOut[this.iCurrentCharacter[i]];
			CharaAction_Balloon_FadeOut_StartMs[i][1] = (balloonMissPtn * tick) - OpenNijiiroRW.Skin.Characters_Balloon_FadeOut[this.iCurrentCharacter[i]];

			if (balloonBrokePtn > 1) CharaAction_Balloon_FadeOut_StartMs[i][0] /= balloonBrokePtn - 1;
			if (balloonMissPtn > 1) CharaAction_Balloon_FadeOut_StartMs[i][1] /= balloonMissPtn - 1; // - 1はタイマー用

			if (CharaAction_Balloon_Delay[i] != null) CharaAction_Balloon_Delay[i].CurrentValue = (int)CharaAction_Balloon_Delay[i].EndValue;
		}

		base.Activate();
	}

	public override void DeActivate()
	{
		for (int i = 0; i < 5; i++)
		{
			/*
            ctChara_Normal[i] = null;
            ctChara_Miss[i] = null;
            ctChara_MissDown[i] = null;
            ctChara_GoGo[i] = null;
            ctChara_Clear[i] = null;
            this.ctキャラクターアクション_10コンボ[i] = null;
            this.ctキャラクターアクション_10コンボMAX[i] = null;
            this.ctキャラクターアクション_ゴーゴースタート[i] = null;
            this.ctキャラクターアクション_ゴーゴースタートMAX[i] = null;
            this.ctキャラクターアクション_ノルマ[i] = null;
            this.ctキャラクターアクション_魂MAX[i] = null;
            this.ctキャラクターアクション_Return[i] = null;
            */

			//CharaAction_Balloon_Breaking[i] = null;
			//CharaAction_Balloon_Broke[i] = null;
			//CharaAction_Balloon_Miss[i] = null;
			CharaAction_Balloon_Delay[i] = null;

			CharaAction_Balloon_FadeOut[i] = null;
		}

		base.DeActivate();
	}

	public override void CreateManagedResource()
	{
		for (int i = 0; i < 5; i++)
		{
			//this.arモーション番号[i] = C変換.ar配列形式のstringをint配列に変換して返す(TJAPlayer3.Skin.Characters_Motion_Normal[this.iCurrentCharacter[i]]);
			//this.arMissモーション番号[i] = C変換.ar配列形式のstringをint配列に変換して返す(TJAPlayer3.Skin.Characters_Motion_Miss[this.iCurrentCharacter[i]]);
			//this.arMissDownモーション番号[i] = C変換.ar配列形式のstringをint配列に変換して返す(TJAPlayer3.Skin.Characters_Motion_MissDown[this.iCurrentCharacter[i]]);
			//this.arゴーゴーモーション番号[i] = C変換.ar配列形式のstringをint配列に変換して返す(TJAPlayer3.Skin.Characters_Motion_GoGo[this.iCurrentCharacter[i]]);
			//this.arクリアモーション番号[i] = C変換.ar配列形式のstringをint配列に変換して返す(TJAPlayer3.Skin.Characters_Motion_Clear[this.iCurrentCharacter[i]]);

			//if (arモーション番号[i] == null) this.arモーション番号[i] = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
			//if (arMissモーション番号[i] == null) this.arMissモーション番号[i] = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
			//if (arMissDownモーション番号[i] == null) this.arMissDownモーション番号[i] = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
			//if (arゴーゴーモーション番号[i] == null) this.arゴーゴーモーション番号[i] = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
			//if (arクリアモーション番号[i] == null) this.arクリアモーション番号[i] = C変換.ar配列形式のstringをint配列に変換して返す("0,0");
		}
		base.CreateManagedResource();
	}

	public override void ReleaseManagedResource()
	{
		base.ReleaseManagedResource();
	}

	public override int Draw()
	{
		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			int Character = this.iCurrentCharacter[i];

			if (OpenNijiiroRW.Skin.Characters_Ptn == 0)
				break;

			// Blinking animation during invincibility frames
			if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Tower)
			{
				if (CFloorManagement.isBlinking() == true)
					break;
			}

			CTexture nowChara = null;

			void updateNormal()
			{
				if (!OpenNijiiroRW.stageGameScreen.bPAUSE)
				{
					nNowCharaCounter[i] += ((Math.Abs((float)OpenNijiiroRW.stageGameScreen.actPlayInfo.dbBPM[i]) / 60.0f) * (float)OpenNijiiroRW.FPS.DeltaTime) / nCharaBeat[i];
				}
			}
			void updateBalloon()
			{
				if (!OpenNijiiroRW.stageGameScreen.bPAUSE)
				{
					nNowCharaCounter[i] += (float)OpenNijiiroRW.FPS.DeltaTime / nCharaBeat[i];
				}
			}

			ctKusuIn[i].Tick();

			bool endAnime = nNowCharaCounter[i] >= 1;

			// Notice that nCharaFrameCount is -1 when no frames are defined
			void getNowCharaFrame(double speed = 1.0)
			{
				if (endAnime)
					nNowCharaFrame[i] = Math.Max(0, nCharaFrameCount[i]); // to prevent a huge-value counter from overflowing the int
				else
					nNowCharaFrame[i] = (int)(nNowCharaCounter[i] * speed * (nCharaFrameCount[i] + 1));
				nNowCharaFrame[i] = Math.Max(0, Math.Min(nNowCharaFrame[i], nCharaFrameCount[i]));
			}

			void getNowCharaFrameLooped()
			{
				if (double.IsFinite(nNowCharaCounter[i]))
					nNowCharaCounter[i] %= 1;
				else
					nNowCharaCounter[i] = 0;
				nNowCharaFrame[i] = (int)(nNowCharaCounter[i] * (nCharaFrameCount[i] + 1));
				nNowCharaFrame[i] = Math.Max(0, Math.Min(nNowCharaFrame[i], nCharaFrameCount[i]));
			}

			if (eNowAnime[i] != Anime.None)
			{
				switch (eNowAnime[i])
				{
					case Anime.None:
						{
							ReturnDefaultAnime(i, false);
						}
						break;
					case Anime.Normal:
						{
							getNowCharaFrameLooped();
							updateNormal();
							ReturnDefaultAnime(i, false);
							nowChara = OpenNijiiroRW.Tx.Characters_Normal[Character][OpenNijiiroRW.Skin.Characters_Motion_Normal[Character][nNowCharaFrame[i]]];
						}
						break;
					case Anime.Miss:
						{
							getNowCharaFrameLooped();
							updateNormal();
							ReturnDefaultAnime(i, false);
							nowChara = OpenNijiiroRW.Tx.Characters_Normal_Missed[Character][OpenNijiiroRW.Skin.Characters_Motion_Miss[Character][nNowCharaFrame[i]]];
						}
						break;
					case Anime.MissDown:
						{
							getNowCharaFrameLooped();
							updateNormal();
							ReturnDefaultAnime(i, false);
							nowChara = OpenNijiiroRW.Tx.Characters_Normal_MissedDown[Character][OpenNijiiroRW.Skin.Characters_Motion_MissDown[Character][nNowCharaFrame[i]]];
						}
						break;
					case Anime.Cleared:
						{
							getNowCharaFrameLooped();
							updateNormal();
							ReturnDefaultAnime(i, false);
							nowChara = OpenNijiiroRW.Tx.Characters_Normal_Cleared[Character][OpenNijiiroRW.Skin.Characters_Motion_Clear[Character][nNowCharaFrame[i]]];
						}
						break;
					case Anime.Maxed:
						{
							getNowCharaFrameLooped();
							updateNormal();
							ReturnDefaultAnime(i, false);
							nowChara = OpenNijiiroRW.Tx.Characters_Normal_Maxed[Character][OpenNijiiroRW.Skin.Characters_Motion_ClearMax[Character][nNowCharaFrame[i]]];
						}
						break;
					case Anime.MissIn:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_MissIn[Character] != null && OpenNijiiroRW.Skin.Characters_MissIn_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_MissIn[Character][OpenNijiiroRW.Skin.Characters_Motion_MissIn[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.MissDownIn:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_MissDownIn[Character] != null && OpenNijiiroRW.Skin.Characters_MissDownIn_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_MissDownIn[Character][OpenNijiiroRW.Skin.Characters_Motion_MissDownIn[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.GoGoTime:
						{
							getNowCharaFrameLooped();
							updateNormal();
							ReturnDefaultAnime(i, false);
							nowChara = OpenNijiiroRW.Tx.Characters_GoGoTime[Character][OpenNijiiroRW.Skin.Characters_Motion_GoGo[Character][nNowCharaFrame[i]]];
						}
						break;
					case Anime.GoGoTime_Maxed:
						{
							getNowCharaFrameLooped();
							updateNormal();
							ReturnDefaultAnime(i, false);
							nowChara = OpenNijiiroRW.Tx.Characters_GoGoTime_Maxed[Character][OpenNijiiroRW.Skin.Characters_Motion_GoGoMax[Character][nNowCharaFrame[i]]];
						}
						break;
					case Anime.Combo10:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_10Combo[Character] != null && OpenNijiiroRW.Skin.Characters_10Combo_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_10Combo[Character][OpenNijiiroRW.Skin.Characters_Motion_10Combo[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.Combo10_Clear:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_10Combo_Clear[Character] != null && OpenNijiiroRW.Skin.Characters_10Combo_Clear_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_10Combo_Clear[Character][OpenNijiiroRW.Skin.Characters_Motion_10Combo_Clear[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.Combo10_Max:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_10Combo_Maxed[Character] != null && OpenNijiiroRW.Skin.Characters_10Combo_Maxed_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_10Combo_Maxed[Character][OpenNijiiroRW.Skin.Characters_Motion_10ComboMax[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.GoGoStart:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_GoGoStart[Character] != null && OpenNijiiroRW.Skin.Characters_GoGoStart_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_GoGoStart[Character][OpenNijiiroRW.Skin.Characters_Motion_GoGoStart[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.GoGoStart_Clear:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_GoGoStart_Clear[Character] != null && OpenNijiiroRW.Skin.Characters_GoGoStart_Clear_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_GoGoStart_Clear[Character][OpenNijiiroRW.Skin.Characters_Motion_GoGoStart_Clear[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.GoGoStart_Max:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_GoGoStart_Maxed[Character] != null && OpenNijiiroRW.Skin.Characters_GoGoStart_Maxed_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_GoGoStart_Maxed[Character][OpenNijiiroRW.Skin.Characters_Motion_GoGoStartMax[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.Become_Cleared:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_Become_Cleared[Character] != null && OpenNijiiroRW.Skin.Characters_Become_Cleared_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_Become_Cleared[Character][OpenNijiiroRW.Skin.Characters_Motion_ClearIn[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.Become_Maxed:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_Become_Maxed[Character] != null && OpenNijiiroRW.Skin.Characters_Become_Maxed_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_Become_Maxed[Character][OpenNijiiroRW.Skin.Characters_Motion_SoulIn[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.SoulOut:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_SoulOut[Character] != null && OpenNijiiroRW.Skin.Characters_SoulOut_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_SoulOut[Character][OpenNijiiroRW.Skin.Characters_Motion_SoulOut[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.ClearOut:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_ClearOut[Character] != null && OpenNijiiroRW.Skin.Characters_ClearOut_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_ClearOut[Character][OpenNijiiroRW.Skin.Characters_Motion_ClearOut[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.Return:
						{
							getNowCharaFrame();
							updateNormal();
							if (OpenNijiiroRW.Tx.Characters_Return[Character] != null && OpenNijiiroRW.Skin.Characters_Return_Ptn[Character] != 0)
							{
								nowChara = OpenNijiiroRW.Tx.Characters_Return[Character][OpenNijiiroRW.Skin.Characters_Motion_Return[Character][nNowCharaFrame[i]]];
							}
							if (endAnime)
							{
								ReturnDefaultAnime(i, true);
							}
						}
						break;
					case Anime.Balloon_Breaking:
					case Anime.Balloon_Broke:
					case Anime.Balloon_Miss:
					case Anime.Kusudama_Idle:
					case Anime.Kusudama_Breaking:
					case Anime.Kusudama_Broke:
						{
							getNowCharaFrame();
							updateBalloon();
						}
						break;
					case Anime.Kusudama_Miss:
						{
							getNowCharaFrame(2); // ?
							updateBalloon();
						}
						break;
				}
			}

			float chara_x;
			float chara_y;

			float charaScale = 1.0f;

			if (nowChara != null)
			{
				bool flipX = OpenNijiiroRW.ConfigIni.bAIBattleMode ? (i == 1) : false;

				float resolutionScaleX = OpenNijiiroRW.Skin.Resolution[0] / (float)OpenNijiiroRW.Skin.Characters_Resolution[Character][0];
				float resolutionScaleY = OpenNijiiroRW.Skin.Resolution[1] / (float)OpenNijiiroRW.Skin.Characters_Resolution[Character][1];

				if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
				{
					chara_x = (OpenNijiiroRW.Skin.Characters_X_AI[Character][i] * resolutionScaleX);
					chara_y = (OpenNijiiroRW.Skin.Characters_Y_AI[Character][i] * resolutionScaleY);

					if (nowChara != null)
					{
						charaScale = 0.58f;
					}
				}
				else if (OpenNijiiroRW.ConfigIni.nPlayerCount <= 2)
				{
					chara_x = (OpenNijiiroRW.Skin.Characters_X[Character][i] * resolutionScaleX);
					chara_y = (OpenNijiiroRW.Skin.Characters_Y[Character][i] * resolutionScaleY);

					if (nowChara != null)
					{
						charaScale = 1.0f;
					}
				}
				else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
				{
					chara_x = (OpenNijiiroRW.Skin.Characters_5P[Character][0] * resolutionScaleX) + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
					chara_y = (OpenNijiiroRW.Skin.Characters_5P[Character][1] * resolutionScaleY) + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);

					if (nowChara != null)
					{
						charaScale = 0.58f;
					}
				}
				else
				{
					chara_x = (OpenNijiiroRW.Skin.Characters_4P[Character][0] * resolutionScaleX) + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
					chara_y = (OpenNijiiroRW.Skin.Characters_4P[Character][1] * resolutionScaleY) + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);

					if (nowChara != null)
					{
						charaScale = 0.58f;
					}
				}

				charaScale *= resolutionScaleY;
				//chara_x *= resolutionScaleX;
				//chara_y *= resolutionScaleY;

				if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
				{
					chara_x += OpenNijiiroRW.Skin.Game_AIBattle_CharaMove * OpenNijiiroRW.stageGameScreen.AIBattleState;
					chara_y -= nowChara.szTextureSize.Height * charaScale; // Center down
				}

				nowChara.Scale.X = charaScale;
				nowChara.Scale.Y = charaScale;

				if (flipX)
				{
					nowChara.t2D左右反転描画(chara_x, chara_y);
				}
				else
				{
					nowChara.t2D描画(chara_x, chara_y);
				}

				nowChara.Scale.X = 1.0f;
				nowChara.Scale.Y = 1.0f;
			}

			if ((this.b風船連打中[i] != true && CharaAction_Balloon_Delay[i].IsEnded) || OpenNijiiroRW.ConfigIni.nPlayerCount > 2)
			{
				if (OpenNijiiroRW.ConfigIni.nPlayerCount <= 2)
				{
					OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(OpenNijiiroRW.Skin.Game_PuchiChara_X[i], OpenNijiiroRW.Skin.Game_PuchiChara_Y[i], OpenNijiiroRW.stageGameScreen.bIsAlreadyMaxed[i], player: i);
				}
				else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
				{
					OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(OpenNijiiroRW.Skin.Game_PuchiChara_5P[0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i), OpenNijiiroRW.Skin.Game_PuchiChara_5P[1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i), OpenNijiiroRW.stageGameScreen.bIsAlreadyMaxed[i], player: i, scale: 0.5f);
				}
				else
				{
					OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(OpenNijiiroRW.Skin.Game_PuchiChara_4P[0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i), OpenNijiiroRW.Skin.Game_PuchiChara_4P[1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i), OpenNijiiroRW.stageGameScreen.bIsAlreadyMaxed[i], player: i, scale: 0.5f);
				}
			}
		}
		return base.Draw();
	}

	public void OnDraw_Balloon()
	{
		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			//if (TJAPlayer3.Skin.Characters_Balloon_Breaking_Ptn[iCurrentCharacter[i]] != 0) CharaAction_Balloon_Breaking[i]?.t進行();
			//if (TJAPlayer3.Skin.Characters_Balloon_Broke_Ptn[iCurrentCharacter[i]] != 0) CharaAction_Balloon_Broke[i]?.t進行();
			CharaAction_Balloon_Delay[i]?.Tick();
			//if (TJAPlayer3.Skin.Characters_Balloon_Miss_Ptn[iCurrentCharacter[i]] != 0) CharaAction_Balloon_Miss[i]?.t進行();
			//CharaAction_Balloon_FadeOut[i].Tick();

			{
				bool endAnime = nNowCharaCounter[i] >= 1;
				var nowOpacity = 255;

				float resolutionScaleX = OpenNijiiroRW.Skin.Resolution[0] / (float)OpenNijiiroRW.Skin.Characters_Resolution[this.iCurrentCharacter[i]][0];
				float resolutionScaleY = OpenNijiiroRW.Skin.Resolution[1] / (float)OpenNijiiroRW.Skin.Characters_Resolution[this.iCurrentCharacter[i]][1];

				float chara_x = 0;
				float chara_y = 0;
				float kusu_chara_x = OpenNijiiroRW.Skin.Characters_Kusudama_X[this.iCurrentCharacter[i]][i] * resolutionScaleX;
				float kusu_chara_y = OpenNijiiroRW.Skin.Characters_Kusudama_Y[this.iCurrentCharacter[i]][i] * resolutionScaleY;

				if (OpenNijiiroRW.ConfigIni.nPlayerCount <= 2)
				{
					chara_x = OpenNijiiroRW.Skin.Characters_Balloon_X[this.iCurrentCharacter[i]][i];
					chara_y = OpenNijiiroRW.Skin.Characters_Balloon_Y[this.iCurrentCharacter[i]][i];
				}
				else
				{
					if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
					{
						chara_x = OpenNijiiroRW.Skin.Characters_Balloon_5P[this.iCurrentCharacter[i]][0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
						chara_y = OpenNijiiroRW.Skin.Characters_Balloon_5P[this.iCurrentCharacter[i]][1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);
					}
					else
					{
						chara_x = OpenNijiiroRW.Skin.Characters_Balloon_4P[this.iCurrentCharacter[i]][0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
						chara_y = OpenNijiiroRW.Skin.Characters_Balloon_4P[this.iCurrentCharacter[i]][1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);
					}
				}

				chara_x *= resolutionScaleX;
				chara_y *= resolutionScaleY;

				float charaScale = resolutionScaleY;


				if (eNowAnime[i] == Anime.Balloon_Broke)
				{
					if (CharaAction_Balloon_FadeOut[i].Counter.IsStoped && nNowCharaFrame[i] > CharaAction_Balloon_FadeOut_StartMs[i][0])
					{
						CharaAction_Balloon_FadeOut[i].Start();
					}

					if (OpenNijiiroRW.Skin.Characters_Balloon_Broke_Ptn[this.iCurrentCharacter[i]] != 0 && OpenNijiiroRW.Tx.Characters_Balloon_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]] != null)
					{
						OpenNijiiroRW.Tx.Characters_Balloon_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Opacity = nowOpacity;
						OpenNijiiroRW.Tx.Characters_Balloon_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.X = charaScale;
						OpenNijiiroRW.Tx.Characters_Balloon_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.Y = charaScale;
						OpenNijiiroRW.Tx.Characters_Balloon_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D描画(
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLX(i) + chara_x,
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLY(i) + chara_y);
					}

					if (OpenNijiiroRW.ConfigIni.nPlayerCount <= 2)
						OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLX(i) + OpenNijiiroRW.Skin.Game_PuchiChara_BalloonX[i],
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLY(i) + OpenNijiiroRW.Skin.Game_PuchiChara_BalloonY[i], false, nowOpacity, true, player: i);

					if (endAnime)
					{
						this.b風船連打中[i] = false;
						ReturnDefaultAnime(i, true);
					}
				}
				else if (eNowAnime[i] == Anime.Balloon_Miss)
				{
					if (CharaAction_Balloon_FadeOut[i].Counter.IsStoped && nNowCharaFrame[i] > CharaAction_Balloon_FadeOut_StartMs[i][1])
					{
						CharaAction_Balloon_FadeOut[i].Start();
					}

					if (OpenNijiiroRW.Skin.Characters_Balloon_Miss_Ptn[this.iCurrentCharacter[i]] != 0 && OpenNijiiroRW.Tx.Characters_Balloon_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]] != null)
					{
						OpenNijiiroRW.Tx.Characters_Balloon_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Opacity = nowOpacity;
						OpenNijiiroRW.Tx.Characters_Balloon_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.X = charaScale;
						OpenNijiiroRW.Tx.Characters_Balloon_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.Y = charaScale;
						OpenNijiiroRW.Tx.Characters_Balloon_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D描画(
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLX(i) + chara_x,
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLY(i) + chara_y);
					}

					if (OpenNijiiroRW.ConfigIni.nPlayerCount <= 2)
						OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLX(i) + OpenNijiiroRW.Skin.Game_PuchiChara_BalloonX[i],
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLY(i) + OpenNijiiroRW.Skin.Game_PuchiChara_BalloonY[i], false, nowOpacity, true, player: i);

					if (endAnime)
					{
						this.b風船連打中[i] = false;
						ReturnDefaultAnime(i, true);
					}
				}
				else if (eNowAnime[i] == Anime.Balloon_Breaking)
				{
					if (OpenNijiiroRW.Skin.Characters_Balloon_Breaking_Ptn[this.iCurrentCharacter[i]] != 0 && OpenNijiiroRW.Tx.Characters_Balloon_Breaking[this.iCurrentCharacter[i]][nNowCharaFrame[i]] != null)
					{
						OpenNijiiroRW.Tx.Characters_Balloon_Breaking[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.X = charaScale;
						OpenNijiiroRW.Tx.Characters_Balloon_Breaking[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.Y = charaScale;
						OpenNijiiroRW.Tx.Characters_Balloon_Breaking[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D描画(
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLX(i) + chara_x,
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLY(i) + chara_y);
					}

					if (OpenNijiiroRW.ConfigIni.nPlayerCount <= 2)
						OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLX(i) + OpenNijiiroRW.Skin.Game_PuchiChara_BalloonX[i],
							OpenNijiiroRW.stageGameScreen.GetJPOSCROLLY(i) + OpenNijiiroRW.Skin.Game_PuchiChara_BalloonY[i], false, 255, true, player: i);
				}
				else if (eNowAnime[i] == Anime.Kusudama_Broke)
				{
					if (CharaAction_Balloon_FadeOut[i].Counter.IsStoped && nNowCharaFrame[i] > CharaAction_Balloon_FadeOut_StartMs[i][0])
					{
						CharaAction_Balloon_FadeOut[i].Start();
					}
					float kusuOutX = ((1.0f - MathF.Cos(Math.Min(1, nNowCharaCounter[i]) * MathF.PI)) * OpenNijiiroRW.Skin.Resolution[0] / 2.0f) * resolutionScaleX;
					float kusuOutY = (MathF.Sin(Math.Min(1, nNowCharaCounter[i]) * MathF.PI / 2) * OpenNijiiroRW.Skin.Resolution[1] / 2.0f) * resolutionScaleY;

					if (OpenNijiiroRW.Skin.Characters_Kusudama_Broke_Ptn[this.iCurrentCharacter[i]] != 0 && OpenNijiiroRW.Tx.Characters_Kusudama_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]] != null)
					{
						OpenNijiiroRW.Tx.Characters_Kusudama_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Opacity = nowOpacity;
						OpenNijiiroRW.Tx.Characters_Kusudama_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.X = charaScale;
						OpenNijiiroRW.Tx.Characters_Kusudama_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.Y = charaScale;
						if (i % 2 == 0)
						{
							OpenNijiiroRW.Tx.Characters_Kusudama_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D描画(kusu_chara_x - kusuOutX, kusu_chara_y - kusuOutY);
						}
						else
						{
							OpenNijiiroRW.Tx.Characters_Kusudama_Broke[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D左右反転描画(kusu_chara_x + kusuOutX, kusu_chara_y - kusuOutY);
						}
					}
					if (i % 2 == 0)
					{
						OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaX[i] - (int)kusuOutX,
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaY[i] - (int)kusuOutY, false, nowOpacity, true, player: i);
					}
					else
					{
						OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaX[i] + (int)kusuOutX,
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaY[i] - (int)kusuOutY, false, nowOpacity, true, player: i);
					}

					if (endAnime)
					{
						this.b風船連打中[i] = false;
						ReturnDefaultAnime(i, true);
					}
				}
				else if (eNowAnime[i] == Anime.Kusudama_Miss)
				{
					if (CharaAction_Balloon_FadeOut[i].Counter.IsStoped && nNowCharaFrame[i] > CharaAction_Balloon_FadeOut_StartMs[i][1])
					{
						CharaAction_Balloon_FadeOut[i].Start();
					}

					float kusuOutY = (Math.Max(Math.Min(1, nNowCharaCounter[i]) - 0.5f, 0) * OpenNijiiroRW.Skin.Resolution[1] * 2) * resolutionScaleY;

					if (OpenNijiiroRW.Skin.Characters_Kusudama_Miss_Ptn[this.iCurrentCharacter[i]] != 0 && OpenNijiiroRW.Tx.Characters_Kusudama_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]] != null)
					{
						OpenNijiiroRW.Tx.Characters_Kusudama_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Opacity = nowOpacity;
						OpenNijiiroRW.Tx.Characters_Kusudama_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.X = charaScale;
						OpenNijiiroRW.Tx.Characters_Kusudama_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.Y = charaScale;


						if (i % 2 == 0)
						{
							OpenNijiiroRW.Tx.Characters_Kusudama_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D描画(kusu_chara_x, kusu_chara_y + kusuOutY);
						}
						else
						{
							OpenNijiiroRW.Tx.Characters_Kusudama_Miss[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D左右反転描画(kusu_chara_x, kusu_chara_y + kusuOutY);
						}
					}

					OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(
						OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaX[i],
						OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaY[i] + (int)kusuOutY, false, nowOpacity, true, player: i);

					if (endAnime)
					{
						this.b風船連打中[i] = false;
						ReturnDefaultAnime(i, true);
					}
				}
				else if (eNowAnime[i] == Anime.Kusudama_Breaking)
				{
					float kusuInX = ((1.0f - MathF.Sin(ctKusuIn[i].CurrentValue / 2000.0f * MathF.PI)) * OpenNijiiroRW.Skin.Resolution[0] / 2.0f) * resolutionScaleX;
					float kusuInY = -((MathF.Cos(ctKusuIn[i].CurrentValue / 1000.0f * MathF.PI / 2)) * OpenNijiiroRW.Skin.Resolution[1] / 2.0f) * resolutionScaleY;


					if (OpenNijiiroRW.Skin.Characters_Kusudama_Breaking_Ptn[this.iCurrentCharacter[i]] != 0 && OpenNijiiroRW.Tx.Characters_Kusudama_Breaking[this.iCurrentCharacter[i]][nNowCharaFrame[i]] != null)
					{
						OpenNijiiroRW.Tx.Characters_Kusudama_Breaking[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.X = charaScale;
						OpenNijiiroRW.Tx.Characters_Kusudama_Breaking[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.Y = charaScale;
						if (i % 2 == 0)
						{
							OpenNijiiroRW.Tx.Characters_Kusudama_Breaking[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D描画(kusu_chara_x - kusuInX, kusu_chara_y + kusuInY);
						}
						else
						{
							OpenNijiiroRW.Tx.Characters_Kusudama_Breaking[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D左右反転描画(kusu_chara_x + kusuInX, kusu_chara_y + kusuInY);
						}
					}

					if (i % 2 == 0)
					{
						OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaX[i] - (int)kusuInX,
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaY[i] + (int)kusuInY, false, 255, true, player: i);
					}
					else
					{
						OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaX[i] + (int)kusuInX,
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaY[i] + (int)kusuInY, false, 255, true, player: i);
					}

					if (endAnime)
					{
						ChangeAnime(i, Anime.Kusudama_Idle, true);
					}
				}
				else if (eNowAnime[i] == Anime.Kusudama_Idle)
				{
					float kusuInX = ((1.0f - MathF.Sin(ctKusuIn[i].CurrentValue / 2000.0f * MathF.PI)) * OpenNijiiroRW.Skin.Resolution[0] / 2.0f) * resolutionScaleX;
					float kusuInY = -((MathF.Cos(ctKusuIn[i].CurrentValue / 1000.0f * MathF.PI / 2)) * OpenNijiiroRW.Skin.Resolution[1] / 2.0f) * resolutionScaleY;

					if (OpenNijiiroRW.Skin.Characters_Kusudama_Idle_Ptn[this.iCurrentCharacter[i]] != 0 && OpenNijiiroRW.Tx.Characters_Kusudama_Idle[this.iCurrentCharacter[i]][nNowCharaFrame[i]] != null)
					{
						OpenNijiiroRW.Tx.Characters_Kusudama_Idle[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.X = charaScale;
						OpenNijiiroRW.Tx.Characters_Kusudama_Idle[this.iCurrentCharacter[i]][nNowCharaFrame[i]].Scale.Y = charaScale;
						if (i % 2 == 0)
						{
							OpenNijiiroRW.Tx.Characters_Kusudama_Idle[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D描画(kusu_chara_x - kusuInX, kusu_chara_y + kusuInY);
						}
						else
						{
							OpenNijiiroRW.Tx.Characters_Kusudama_Idle[this.iCurrentCharacter[i]][nNowCharaFrame[i]].t2D左右反転描画(kusu_chara_x + kusuInX, kusu_chara_y + kusuInY);
						}
					}

					if (i % 2 == 0)
					{
						OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaX[i] - (int)kusuInX,
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaY[i] + (int)kusuInY, false, 255, true, player: i);
					}
					else
					{
						OpenNijiiroRW.stageGameScreen.PuchiChara.On進行描画(
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaX[i] + (int)kusuInX,
							OpenNijiiroRW.Skin.Game_PuchiChara_KusudamaY[i] + (int)kusuInY, false, 255, true, player: i);
					}

					if (endAnime)
					{
						ChangeAnime(i, Anime.Kusudama_Idle, true);
					}
				}
			}
		}
	}


	public void ReturnDefaultAnime(int player, bool resetCounter)
	{
		if (OpenNijiiroRW.stageGameScreen.bIsGOGOTIME[player] && OpenNijiiroRW.Skin.Characters_GoGoTime_Ptn[this.iCurrentCharacter[player]] != 0)
		{
			if (OpenNijiiroRW.stageGameScreen.bIsAlreadyMaxed[player] && OpenNijiiroRW.Skin.Characters_GoGoTime_Maxed_Ptn[this.iCurrentCharacter[player]] != 0)
			{
				ChangeAnime(player, Anime.GoGoTime_Maxed, resetCounter);
			}
			else
			{
				ChangeAnime(player, Anime.GoGoTime, resetCounter);
			}
		}
		else
		{
			if (OpenNijiiroRW.stageGameScreen.bIsMiss[player] && OpenNijiiroRW.Skin.Characters_Normal_Missed_Ptn[this.iCurrentCharacter[player]] != 0)
			{
				if (OpenNijiiroRW.stageGameScreen.Chara_MissCount[player] >= 6 && OpenNijiiroRW.Skin.Characters_Normal_MissedDown_Ptn[this.iCurrentCharacter[player]] != 0)
				{
					ChangeAnime(player, Anime.MissDown, resetCounter);
				}
				else
				{
					ChangeAnime(player, Anime.Miss, resetCounter);
				}
			}
			else
			{
				if (OpenNijiiroRW.stageGameScreen.bIsAlreadyMaxed[player] && OpenNijiiroRW.Skin.Characters_Normal_Maxed_Ptn[this.iCurrentCharacter[player]] != 0)
				{
					ChangeAnime(player, Anime.Maxed, resetCounter);
				}
				else if (OpenNijiiroRW.stageGameScreen.bIsAlreadyCleared[player] && OpenNijiiroRW.Skin.Characters_Normal_Cleared_Ptn[this.iCurrentCharacter[player]] != 0)
				{
					ChangeAnime(player, Anime.Cleared, resetCounter);
				}
				else if (OpenNijiiroRW.Skin.Characters_Normal_Ptn[this.iCurrentCharacter[player]] != 0)
				{
					ChangeAnime(player, Anime.Normal, resetCounter);
				}
				else
				{
					ChangeAnime(player, Anime.None, resetCounter);
				}
			}
		}
	}

	public int[][] arモーション番号 = new int[5][];
	public int[][] arMissモーション番号 = new int[5][];
	public int[][] arMissDownモーション番号 = new int[5][];
	public int[][] arゴーゴーモーション番号 = new int[5][];
	public int[][] arクリアモーション番号 = new int[5][];

	private float[] nNowCharaCounter = new float[5];
	private int[] nNowCharaFrame = new int[5];
	private int[] nCharaFrameCount = new int[5];
	private float[] nCharaBeat = new float[5];

	public enum Anime
	{
		None,
		Normal,
		Miss,
		MissDown,
		Cleared,
		Maxed,
		MissIn,
		MissDownIn,
		GoGoTime,
		GoGoTime_Maxed,
		Combo10,
		Combo10_Clear,
		Combo10_Max,
		GoGoStart,
		GoGoStart_Clear,
		GoGoStart_Max,
		Become_Cleared,
		Become_Maxed,
		SoulOut,
		ClearOut,
		Return,
		Balloon_Breaking,
		Balloon_Broke,
		Balloon_Miss,
		Kusudama_Idle,
		Kusudama_Breaking,
		Kusudama_Broke,
		Kusudama_Miss
	}


	public Anime[] eNowAnime = new Anime[5];

	public CCounter[] ctKusuIn = new CCounter[5];

	public void KusuIn()
	{
		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			ChangeAnime(i, Anime.Kusudama_Idle, true);
			ctKusuIn[i] = new CCounter(0, 1000, 0.4f, OpenNijiiroRW.Timer);
		}
	}

	public void ChangeAnime(int player, Anime anime, bool resetCounter)
	{
		eNowAnime[player] = anime;

		if (resetCounter)
		{
			nNowCharaCounter[player] = 0;
			nNowCharaFrame[player] = 0;
		}

		switch (anime)
		{
			case Anime.None:
				break;
			case Anime.Normal:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_Normal[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_Normal[iCurrentCharacter[player]];
				break;
			case Anime.Miss:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_Miss[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_Miss[iCurrentCharacter[player]];
				break;
			case Anime.MissDown:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_MissDown[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_MissDown[iCurrentCharacter[player]];
				break;
			case Anime.Cleared:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_Clear[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_Clear[iCurrentCharacter[player]];
				break;
			case Anime.Maxed:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_ClearMax[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_ClearMax[iCurrentCharacter[player]];
				break;
			case Anime.MissIn:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_MissIn[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_MissIn[iCurrentCharacter[player]];
				break;
			case Anime.MissDownIn:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_MissDownIn[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_MissDownIn[iCurrentCharacter[player]];
				break;
			case Anime.GoGoTime:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_GoGo[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_GoGo[iCurrentCharacter[player]];
				break;
			case Anime.GoGoTime_Maxed:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_GoGoMax[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_GoGoMax[iCurrentCharacter[player]];
				break;
			case Anime.Combo10:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_10Combo[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_10Combo[iCurrentCharacter[player]];
				break;
			case Anime.Combo10_Clear:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_10Combo_Clear[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_10Combo_Clear[iCurrentCharacter[player]];
				break;
			case Anime.Combo10_Max:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_10ComboMax[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_10ComboMax[iCurrentCharacter[player]];
				break;
			case Anime.GoGoStart:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_GoGoStart[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_GoGoStart[iCurrentCharacter[player]];
				break;
			case Anime.GoGoStart_Clear:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_GoGoStart_Clear[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_GoGoStart_Clear[iCurrentCharacter[player]];
				break;
			case Anime.GoGoStart_Max:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_GoGoStartMax[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_GoGoStartMax[iCurrentCharacter[player]];
				break;
			case Anime.Become_Cleared:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_ClearIn[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_ClearIn[iCurrentCharacter[player]];
				break;
			case Anime.Become_Maxed:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_SoulIn[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_SoulIn[iCurrentCharacter[player]];
				break;
			case Anime.SoulOut:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_SoulOut[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_SoulOut[iCurrentCharacter[player]];
				break;
			case Anime.ClearOut:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_ClearOut[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_ClearOut[iCurrentCharacter[player]];
				break;
			case Anime.Return:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Motion_Return[iCurrentCharacter[player]].Length - 1;
				nCharaBeat[player] = OpenNijiiroRW.Skin.Characters_Beat_Return[iCurrentCharacter[player]];
				break;
			case Anime.Balloon_Breaking:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Balloon_Breaking_Ptn[iCurrentCharacter[player]] - 1;
				nCharaBeat[player] = 0.2f;
				break;
			case Anime.Balloon_Broke:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Balloon_Broke_Ptn[iCurrentCharacter[player]] - 1;
				nCharaBeat[player] = 0.2f;
				break;
			case Anime.Balloon_Miss:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Balloon_Miss_Ptn[iCurrentCharacter[player]] - 1;
				nCharaBeat[player] = 0.2f;
				break;
			case Anime.Kusudama_Idle:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Kusudama_Idle_Ptn[iCurrentCharacter[player]] - 1;
				nCharaBeat[player] = 0.4f;
				break;
			case Anime.Kusudama_Breaking:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Kusudama_Breaking_Ptn[iCurrentCharacter[player]] - 1;
				nCharaBeat[player] = 0.2f;
				break;
			case Anime.Kusudama_Broke:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Kusudama_Broke_Ptn[iCurrentCharacter[player]] - 1;
				nCharaBeat[player] = 1f;
				break;
			case Anime.Kusudama_Miss:
				nCharaFrameCount[player] = OpenNijiiroRW.Skin.Characters_Kusudama_Miss_Ptn[iCurrentCharacter[player]] - 1;
				nCharaBeat[player] = 0.5f;
				break;
		}
	}

	public CCounter[] CharaAction_Balloon_Delay = new CCounter[5];

	public Animations.FadeOut[] CharaAction_Balloon_FadeOut = new Animations.FadeOut[5];
	//private readonly int[] CharaAction_Balloon_FadeOut_StartMs = new int[5];
	private readonly int[][] CharaAction_Balloon_FadeOut_StartMs = new int[5][];

	//public bool[] bキャラクターアクション中 = new bool[5];

	public bool[] b風船連打中 = new bool[5];
	public bool[] b演奏中 = new bool[5];

	public int[] iCurrentCharacter = new int[5] { 0, 0, 0, 0, 0 };
}
