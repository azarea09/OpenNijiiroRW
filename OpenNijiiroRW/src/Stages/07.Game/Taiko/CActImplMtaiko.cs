using System.Runtime.InteropServices;
using FDK;
using Rectangle = System.Drawing.Rectangle;

namespace OpenNijiiroRW;

internal class CActImplMtaiko : CActivity
{
	/// <summary>
	/// mtaiko部分を描画するクラス。左側だけ。
	///
	/// </summary>
	public CActImplMtaiko()
	{
		base.IsDeActivated = true;
	}

	public override void Activate()
	{
		for (int i = 0; i < 25; i++)
		{
			STパッド状態 stパッド状態 = new STパッド状態();
			stパッド状態.n明るさ = 0;
			this.stパッド状態[i] = stパッド状態;
		}

		this.ctレベルアップダウン = new CCounter[5];
		ctSymbolFlash = new CCounter[5];
		this.After = new CTja.ECourse[5];
		this.Before = new CTja.ECourse[5];
		for (int i = 0; i < 5; i++)
		{
			this.ctレベルアップダウン[i] = new CCounter();
			BackSymbolEvent(i);
		}

		base.Activate();
	}

	public override void DeActivate()
	{
		this.ctレベルアップダウン = null;

		base.DeActivate();
	}

	public override void CreateManagedResource()
	{
		base.CreateManagedResource();
	}

	public override void ReleaseManagedResource()
	{
		base.ReleaseManagedResource();
	}

	public override int Draw()
	{
		if (base.IsFirstDraw)
		{
			this.nフラッシュ制御タイマ = SoundManager.PlayTimer.NowTimeMs;
			base.IsFirstDraw = false;
		}

		long num = SoundManager.PlayTimer.NowTimeMs;
		if (num < this.nフラッシュ制御タイマ)
		{
			this.nフラッシュ制御タイマ = num;
		}
		while ((num - this.nフラッシュ制御タイマ) >= 20)
		{
			for (int j = 0; j < 25; j++)
			{
				if (this.stパッド状態[j].n明るさ > 0)
				{
					this.stパッド状態[j].n明るさ--;
				}
			}
			this.nフラッシュ制御タイマ += 20;
		}


		//this.nHS = TJAPlayer3.ConfigIni.nScrollSpeed.Drums < 8 ? TJAPlayer3.ConfigIni.nScrollSpeed.Drums : 7;



		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			//int bg_x = OpenNijiiroRW.Skin.Game_Taiko_Background_X[i];
			//int bg_y = OpenNijiiroRW.Skin.Game_Taiko_Background_Y[i];
			int bg_x = 0;
			int bg_y = i == 0 ? 276 : 540;
			CTexture tex = null;

			switch (i)
			{
				case 0:
					{
						if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan)
						{
							tex = OpenNijiiroRW.Tx.Taiko_Background[2];
						}
						else
						{
							if (OpenNijiiroRW.P1IsBlue())
								tex = OpenNijiiroRW.Tx.Taiko_Background[1];
							else
								tex = OpenNijiiroRW.Tx.Taiko_Background[0];
						}
					}
					break;
				case 1:
					{
						if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
						{
							tex = OpenNijiiroRW.Tx.Taiko_Background[3];
						}
						else
						{
							tex = OpenNijiiroRW.Tx.Taiko_Background[1];
						}
					}
					break;
			}

			tex?.t2D描画(bg_x, bg_y);

			if (!OpenNijiiroRW.ConfigIni.bTokkunMode)
				OpenNijiiroRW.Tx.Score_Background.t2D描画(bg_x, bg_y + 12);
		}

		int getMTaikoOpacity(int brightness)
		{
			if (OpenNijiiroRW.ConfigIni.SimpleMode)
			{
				return brightness <= 0 ? 0 : 255;
			}
			else
			{
				return brightness * 73;
			}
		}

		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			int taiko_x;
			int taiko_y;
			if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
			{
				taiko_x = OpenNijiiroRW.Skin.Game_Taiko_5P[0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
				taiko_y = OpenNijiiroRW.Skin.Game_Taiko_5P[1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);
			}
			else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 4 || OpenNijiiroRW.ConfigIni.nPlayerCount == 3)
			{
				taiko_x = OpenNijiiroRW.Skin.Game_Taiko_4P[0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
				taiko_y = OpenNijiiroRW.Skin.Game_Taiko_4P[1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);
			}
			else
			{
				taiko_x = OpenNijiiroRW.Skin.Game_Taiko_X[i];
				taiko_y = OpenNijiiroRW.Skin.Game_Taiko_Y[i];
			}

			int _actual = OpenNijiiroRW.GetActualPlayer(i);
			EGameType _gt = OpenNijiiroRW.ConfigIni.nGameType[_actual];
			int playerShift = i * 5;

			// Drum base
			OpenNijiiroRW.Tx.Taiko_Base[(int)_gt]?.t2D描画(taiko_x, taiko_y);

			// Taiko hits
			if (_gt == EGameType.Taiko)
			{
				if (OpenNijiiroRW.Tx.Taiko_Don_Left != null && OpenNijiiroRW.Tx.Taiko_Don_Right != null && OpenNijiiroRW.Tx.Taiko_Ka_Left != null && OpenNijiiroRW.Tx.Taiko_Ka_Right != null)
				{
					OpenNijiiroRW.Tx.Taiko_Ka_Left.Opacity = getMTaikoOpacity(this.stパッド状態[playerShift].n明るさ);
					OpenNijiiroRW.Tx.Taiko_Ka_Right.Opacity = getMTaikoOpacity(this.stパッド状態[1 + playerShift].n明るさ);
					OpenNijiiroRW.Tx.Taiko_Don_Left.Opacity = getMTaikoOpacity(this.stパッド状態[2 + playerShift].n明るさ);
					OpenNijiiroRW.Tx.Taiko_Don_Right.Opacity = getMTaikoOpacity(this.stパッド状態[3 + playerShift].n明るさ);

					OpenNijiiroRW.Tx.Taiko_Ka_Left.t2D描画(taiko_x, taiko_y, new Rectangle(0, 0, OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Width / 2, OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Height));
					OpenNijiiroRW.Tx.Taiko_Ka_Right.t2D描画(taiko_x + OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Width / 2, taiko_y, new Rectangle(OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Width / 2, 0, OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Width / 2, OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Height));
					OpenNijiiroRW.Tx.Taiko_Don_Left.t2D描画(taiko_x, taiko_y, new Rectangle(0, 0, OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Width / 2, OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Height));
					OpenNijiiroRW.Tx.Taiko_Don_Right.t2D描画(taiko_x + OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Width / 2, taiko_y, new Rectangle(OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Width / 2, 0, OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Width / 2, OpenNijiiroRW.Tx.Taiko_Ka_Right.szTextureSize.Height));
				}
			}
			else if (_gt == EGameType.Konga)
			{
				if (OpenNijiiroRW.Tx.Taiko_Konga_Clap != null && OpenNijiiroRW.Tx.Taiko_Konga_Don != null && OpenNijiiroRW.Tx.Taiko_Konga_Ka != null)
				{
					OpenNijiiroRW.Tx.Taiko_Konga_Clap.Opacity = getMTaikoOpacity(this.stパッド状態[4 + playerShift].n明るさ);
					OpenNijiiroRW.Tx.Taiko_Konga_Don.Opacity = getMTaikoOpacity(Math.Max(this.stパッド状態[2 + playerShift].n明るさ, this.stパッド状態[3 + playerShift].n明るさ));
					OpenNijiiroRW.Tx.Taiko_Konga_Ka.Opacity = getMTaikoOpacity(Math.Max(this.stパッド状態[playerShift].n明るさ, this.stパッド状態[1 + playerShift].n明るさ));

					OpenNijiiroRW.Tx.Taiko_Konga_Ka.t2D描画(taiko_x, taiko_y);
					OpenNijiiroRW.Tx.Taiko_Konga_Don.t2D描画(taiko_x, taiko_y);
					OpenNijiiroRW.Tx.Taiko_Konga_Clap.t2D描画(taiko_x, taiko_y);
				}
			}

		}


		int[] nLVUPY = new int[] { 127, 127, 0, 0 };

		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			if (OpenNijiiroRW.ConfigIni.nPlayerCount > 2 || OpenNijiiroRW.ConfigIni.SimpleMode) break;

			if (!this.ctレベルアップダウン[i].IsStoped)
			{
				this.ctレベルアップダウン[i].Tick();
				if (this.ctレベルアップダウン[i].IsEnded)
				{
					this.ctレベルアップダウン[i].Stop();
				}
			}
			if ((this.ctレベルアップダウン[i].IsTicked && (OpenNijiiroRW.Tx.Taiko_LevelUp != null && OpenNijiiroRW.Tx.Taiko_LevelDown != null)) && !OpenNijiiroRW.ConfigIni.bNoInfo)
			{
				//this.ctレベルアップダウン[ i ].n現在の値 = 110;

				//2017.08.21 kairera0467 t3D描画に変更。
				float fScale = 1.0f;
				int nAlpha = 255;
				float[] fY = new float[] { 206, -206, 0, 0 };
				if (this.ctレベルアップダウン[i].CurrentValue >= 0 && this.ctレベルアップダウン[i].CurrentValue <= 20)
				{
					nAlpha = 60;
					fScale = 1.14f;
				}
				else if (this.ctレベルアップダウン[i].CurrentValue >= 21 && this.ctレベルアップダウン[i].CurrentValue <= 40)
				{
					nAlpha = 60;
					fScale = 1.19f;
				}
				else if (this.ctレベルアップダウン[i].CurrentValue >= 41 && this.ctレベルアップダウン[i].CurrentValue <= 60)
				{
					nAlpha = 220;
					fScale = 1.23f;
				}
				else if (this.ctレベルアップダウン[i].CurrentValue >= 61 && this.ctレベルアップダウン[i].CurrentValue <= 80)
				{
					nAlpha = 230;
					fScale = 1.19f;
				}
				else if (this.ctレベルアップダウン[i].CurrentValue >= 81 && this.ctレベルアップダウン[i].CurrentValue <= 100)
				{
					nAlpha = 240;
					fScale = 1.14f;
				}
				else if (this.ctレベルアップダウン[i].CurrentValue >= 101 && this.ctレベルアップダウン[i].CurrentValue <= 120)
				{
					nAlpha = 255;
					fScale = 1.04f;
				}
				else
				{
					nAlpha = 255;
					fScale = 1.0f;
				}

				if (OpenNijiiroRW.ConfigIni.nPlayerCount > 2) continue;

				int levelChange_x = OpenNijiiroRW.Skin.Game_Taiko_LevelChange_X[i];
				int levelChange_y = OpenNijiiroRW.Skin.Game_Taiko_LevelChange_Y[i];

				if (this.After[i] - this.Before[i] >= 0)
				{
					//レベルアップ
					OpenNijiiroRW.Tx.Taiko_LevelUp.Scale.X = fScale;
					OpenNijiiroRW.Tx.Taiko_LevelUp.Scale.Y = fScale;
					OpenNijiiroRW.Tx.Taiko_LevelUp.Opacity = nAlpha;
					OpenNijiiroRW.Tx.Taiko_LevelUp.t2D拡大率考慮中央基準描画(levelChange_x,
						levelChange_y);
				}
				else
				{
					OpenNijiiroRW.Tx.Taiko_LevelDown.Scale.X = fScale;
					OpenNijiiroRW.Tx.Taiko_LevelDown.Scale.Y = fScale;
					OpenNijiiroRW.Tx.Taiko_LevelDown.Opacity = nAlpha;
					OpenNijiiroRW.Tx.Taiko_LevelDown.t2D拡大率考慮中央基準描画(levelChange_x,
						levelChange_y);
				}
			}
		}

		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			if (OpenNijiiroRW.ConfigIni.bAIBattleMode && i == 1) break;


			int modIcons_x;
			int modIcons_y;
			int couse_symbol_x;
			int couse_symbol_y;
			if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
			{
				modIcons_x = OpenNijiiroRW.Skin.Game_Taiko_ModIcons_5P[0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
				modIcons_y = OpenNijiiroRW.Skin.Game_Taiko_ModIcons_5P[1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);

				couse_symbol_x = OpenNijiiroRW.Skin.Game_CourseSymbol_5P[0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
				couse_symbol_y = OpenNijiiroRW.Skin.Game_CourseSymbol_5P[1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);
			}
			else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 4 || OpenNijiiroRW.ConfigIni.nPlayerCount == 3)
			{
				modIcons_x = OpenNijiiroRW.Skin.Game_Taiko_ModIcons_4P[0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
				modIcons_y = OpenNijiiroRW.Skin.Game_Taiko_ModIcons_4P[1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);

				couse_symbol_x = OpenNijiiroRW.Skin.Game_CourseSymbol_4P[0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
				couse_symbol_y = OpenNijiiroRW.Skin.Game_CourseSymbol_4P[1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);
			}
			else
			{
				modIcons_x = OpenNijiiroRW.Skin.Game_Taiko_ModIcons_X[i];
				modIcons_y = OpenNijiiroRW.Skin.Game_Taiko_ModIcons_Y[i];

				couse_symbol_x = OpenNijiiroRW.Skin.Game_CourseSymbol_X[i];
				couse_symbol_y = OpenNijiiroRW.Skin.Game_CourseSymbol_Y[i];
			}

			ModIcons.tDisplayMods(modIcons_x, modIcons_y, i);

			if (OpenNijiiroRW.Tx.Couse_Symbol[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]] != null)
			{
				OpenNijiiroRW.Tx.Couse_Symbol[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]].t2D描画(
					couse_symbol_x,
					couse_symbol_y
				);
			}


			if (OpenNijiiroRW.ConfigIni.ShinuchiMode)
			{
				if (OpenNijiiroRW.Tx.Couse_Symbol[(int)Difficulty.Total] != null)
				{
					OpenNijiiroRW.Tx.Couse_Symbol[(int)Difficulty.Total].t2D描画(
						couse_symbol_x,
						couse_symbol_y
					);
				}

			}
		}

		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			int namePlate_x;
			int namePlate_y;
			int playerNumber_x;
			int playerNumber_y;
			if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
			{
				namePlate_x = OpenNijiiroRW.Skin.Game_Taiko_NamePlate_5P[0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
				namePlate_y = OpenNijiiroRW.Skin.Game_Taiko_NamePlate_5P[1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);

				playerNumber_x = OpenNijiiroRW.Skin.Game_Taiko_PlayerNumber_5P[0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
				playerNumber_y = OpenNijiiroRW.Skin.Game_Taiko_PlayerNumber_5P[1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);
			}
			else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 4 || OpenNijiiroRW.ConfigIni.nPlayerCount == 3)
			{
				namePlate_x = OpenNijiiroRW.Skin.Game_Taiko_NamePlate_4P[0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
				namePlate_y = OpenNijiiroRW.Skin.Game_Taiko_NamePlate_4P[1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);

				playerNumber_x = OpenNijiiroRW.Skin.Game_Taiko_PlayerNumber_4P[0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
				playerNumber_y = OpenNijiiroRW.Skin.Game_Taiko_PlayerNumber_4P[1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);
			}
			else
			{
				namePlate_x = OpenNijiiroRW.Skin.Game_Taiko_NamePlate_X[i];
				namePlate_y = OpenNijiiroRW.Skin.Game_Taiko_NamePlate_Y[i];

				playerNumber_x = OpenNijiiroRW.Skin.Game_Taiko_PlayerNumber_X[i];
				playerNumber_y = OpenNijiiroRW.Skin.Game_Taiko_PlayerNumber_Y[i];
			}

			OpenNijiiroRW.NamePlate.tNamePlateDraw(namePlate_x, namePlate_y, i);

			if (OpenNijiiroRW.Tx.Taiko_PlayerNumber[i] != null)
			{
				OpenNijiiroRW.Tx.Taiko_PlayerNumber[i].t2D描画(playerNumber_x, playerNumber_y);
			}
		}
		return base.Draw();
	}

	public void tMtaikoEvent(int nChannel, int nHand, int nPlayer)
	{
		CConfigIni configIni = OpenNijiiroRW.ConfigIni;
		bool bAutoPlay = configIni.bAutoPlay[nPlayer];
		int playerShift = 5 * nPlayer;
		var _gt = configIni.nGameType[OpenNijiiroRW.GetActualPlayer(nPlayer)];

		switch (nPlayer)
		{
			case 1:
				bAutoPlay = configIni.bAutoPlay[nPlayer] || OpenNijiiroRW.ConfigIni.bAIBattleMode;
				break;
		}

		if (!bAutoPlay)
		{
			switch (nChannel)
			{
				case 0x11:
				case 0x13:
				case 0x15:
				case 0x16:
				case 0x17:
					{
						this.stパッド状態[2 + nHand + playerShift].n明るさ = 8;
					}
					break;
				case 0x12:
					{
						this.stパッド状態[nHand + playerShift].n明るさ = 8;
					}
					break;
				case 0x14:
					{
						if (_gt == EGameType.Konga)
						{
							this.stパッド状態[4 + playerShift].n明るさ = 8;
						}
						else
						{
							this.stパッド状態[nHand + playerShift].n明るさ = 8;
						}
					}
					break;

			}
		}
		else
		{
			switch (nChannel)
			{
				case 0x11:
				case 0x15:
				case 0x16:
				case 0x17:
				case 0x1F:
					{
						this.stパッド状態[2 + nHand + playerShift].n明るさ = 8;
					}
					break;

				case 0x13:
				case 0x1A:
					{
						if (_gt == EGameType.Konga)
						{
							this.stパッド状態[0 + playerShift].n明るさ = 8;
							this.stパッド状態[2 + playerShift].n明るさ = 8;
						}
						else
						{
							this.stパッド状態[2 + playerShift].n明るさ = 8;
							this.stパッド状態[3 + playerShift].n明るさ = 8;
						}
					}
					break;

				case 0x12:
					{
						this.stパッド状態[nHand + playerShift].n明るさ = 8;
					}
					break;

				case 0x14:
				case 0x1B:
					{
						if (_gt == EGameType.Konga)
						{
							this.stパッド状態[4 + playerShift].n明るさ = 8;
						}
						else
						{
							this.stパッド状態[0 + playerShift].n明るさ = 8;
							this.stパッド状態[1 + playerShift].n明るさ = 8;
						}

					}
					break;

				case 0x101:
					{
						this.stパッド状態[nHand + playerShift].n明るさ = 8;
						this.stパッド状態[2 + (nHand == 0 ? 1 : 0) + playerShift].n明るさ = 8;
						break;
					}
			}
		}

	}

	public void tBranchEvent(CTja.ECourse Before, CTja.ECourse After, int player)
	{
		if (After != Before)
			this.ctレベルアップダウン[player] = new CCounter(0, 1000, 1, OpenNijiiroRW.Timer);

		this.After[player] = After;
		this.Before[player] = Before;
	}


	public void BackSymbolEvent(int player)
	{
		ctSymbolFlash[player] = new CCounter(0, 1000, 0.2f, OpenNijiiroRW.Timer);
	}

	public void DrawBackSymbol()
	{
		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			ctSymbolFlash[i].Tick();

			int couse_symbol_x;
			int couse_symbol_y;
			if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
			{
				couse_symbol_x = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_5P[0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
				couse_symbol_y = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_5P[1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);
			}
			else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 4 || OpenNijiiroRW.ConfigIni.nPlayerCount == 3)
			{
				couse_symbol_x = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_4P[0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
				couse_symbol_y = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_4P[1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);
			}
			else
			{
				couse_symbol_x = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_X[i];
				couse_symbol_y = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Y[i];
			}


			if (OpenNijiiroRW.Tx.Couse_Symbol_Back[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]] != null)
			{
				int originX = 0;
				int originY = 0;
				int width = OpenNijiiroRW.Tx.Couse_Symbol_Back[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]].szTextureSize.Width;
				int height = OpenNijiiroRW.Tx.Couse_Symbol_Back[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]].szTextureSize.Height;

				if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
				{
					originX = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_5P[0];
					originY = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_5P[1];
					width = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_5P[2];
					height = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_5P[3];
				}
				else if (OpenNijiiroRW.ConfigIni.nPlayerCount > 2)
				{
					originX = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_4P[0];
					originY = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_4P[1];
					width = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_4P[2];
					height = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_4P[3];
				}

				OpenNijiiroRW.Tx.Couse_Symbol_Back[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]].t2D描画(
					couse_symbol_x,
					couse_symbol_y,
					new System.Drawing.RectangleF(originX, originY, width, height));
			}

			if (OpenNijiiroRW.Tx.Couse_Symbol_Back_Flash[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]] != null && !OpenNijiiroRW.ConfigIni.SimpleMode)
			{
				int originX = 0;
				int originY = 0;
				int width = OpenNijiiroRW.Tx.Couse_Symbol_Back[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]].szTextureSize.Width;
				int height = OpenNijiiroRW.Tx.Couse_Symbol_Back[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]].szTextureSize.Height;

				if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
				{
					originX = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_5P[0];
					originY = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_5P[1];
					width = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_5P[2];
					height = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_5P[3];
				}
				else if (OpenNijiiroRW.ConfigIni.nPlayerCount > 2)
				{
					originX = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_4P[0];
					originY = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_4P[1];
					width = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_4P[2];
					height = OpenNijiiroRW.Skin.Game_CourseSymbol_Back_Rect_4P[3];
				}

				OpenNijiiroRW.Tx.Couse_Symbol_Back_Flash[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]].Opacity = 255 - (int)((ctSymbolFlash[i].CurrentValue / 1000.0) * 255);
				OpenNijiiroRW.Tx.Couse_Symbol_Back_Flash[OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]].t2D描画(
					couse_symbol_x,
					couse_symbol_y,
					new System.Drawing.RectangleF(originX, originY, width, height));
			}
		}
	}


	#region[ private ]
	//-----------------
	//構造体
	[StructLayout(LayoutKind.Sequential)]
	private struct STパッド状態
	{
		public int n明るさ;
	}

	//太鼓
	private STパッド状態[] stパッド状態 = new STパッド状態[5 * 5];
	private long nフラッシュ制御タイマ;

	//private CTexture[] txコースシンボル = new CTexture[ 6 ];
	private string[] strCourseSymbolFileName;

	//オプション
	private CTexture txオプションパネル_HS;
	private CTexture txオプションパネル_RANMIR;
	private CTexture txオプションパネル_特殊;
	private int nHS;

	//譜面分岐
	private CCounter[] ctレベルアップダウン;
	public CTja.ECourse[] After;
	public CTja.ECourse[] Before;
	private CCounter[] ctSymbolFlash = new CCounter[5];
	//-----------------
	#endregion

}
