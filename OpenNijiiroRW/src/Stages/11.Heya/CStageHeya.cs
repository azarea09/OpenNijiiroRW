using FDK;
using Color = System.Drawing.Color;

namespace OpenNijiiroRW;

class CStageHeya : CStage {
	public CStageHeya() {
		base.eStageID = EStage.Heya;
		base.ePhaseID = CStage.EPhase.Common_NORMAL;

		base.ChildActivities.Add(this.actFOtoTitle = new CActFIFOBlack());

		base.ChildActivities.Add(this.PuchiChara = new PuchiChara());
	}

	private enum CurrentMenu : int {
		ReturnToMenu = -1,
		Puchi,
		Chara,
		Dan,
		Title,
		Name
	}

	public override void Activate() {
		if (base.IsActivated)
			return;

		base.ePhaseID = CStage.EPhase.Common_NORMAL;
		this.eフェードアウト完了時の戻り値 = E戻り値.継続;

		ctChara_In = new CCounter();
		//ctChara_Normal = new CCounter(0, TJAPlayer3.Tx.SongSelect_Chara_Normal.Length - 1, 1000 / 45, TJAPlayer3.Timer);

		CMenuCharacter.tMenuResetTimer(CMenuCharacter.ECharacterAnimation.NORMAL);

		bInSongPlayed = false;

		this.pfHeyaFont = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.Heya_Font_Scale);

		ScrollCounter = new CCounter(0, 1000, 0.15f, OpenNijiiroRW.Timer);

		// 1P, configure later for default 2P
		iPlayer = OpenNijiiroRW.SaveFile;

		#region [Main menu]

		this.ttkMainMenuOpt = new TitleTextureKey[6];

		this.ttkMainMenuOpt[0] = new TitleTextureKey(CLangManager.LangInstance.GetString("MENU_RETURN"), this.pfHeyaFont, Color.White, Color.DarkGreen, 1000);
		this.ttkMainMenuOpt[1] = new TitleTextureKey(CLangManager.LangInstance.GetString("HEYA_PUCHI"), this.pfHeyaFont, Color.White, Color.DarkGreen, 1000);
		this.ttkMainMenuOpt[2] = new TitleTextureKey(CLangManager.LangInstance.GetString("HEYA_CHARA"), this.pfHeyaFont, Color.White, Color.DarkGreen, 1000);
		this.ttkMainMenuOpt[3] = new TitleTextureKey(CLangManager.LangInstance.GetString("HEYA_DAN"), this.pfHeyaFont, Color.White, Color.DarkGreen, 1000);
		this.ttkMainMenuOpt[4] = new TitleTextureKey(CLangManager.LangInstance.GetString("HEYA_NAMEPLATE"), this.pfHeyaFont, Color.White, Color.DarkGreen, 1000);
		this.ttkMainMenuOpt[5] = new TitleTextureKey(CLangManager.LangInstance.GetString("HEYA_NAME"), this.pfHeyaFont, Color.White, Color.DarkGreen, 1000);

		this.textInputInfo = new TitleTextureKey(CLangManager.LangInstance.GetString("HEYA_NAME_INFO"), this.pfHeyaFont, Color.White, Color.Black, 1000);

		#endregion

		#region [Dan title]

		int amount = 1;
		if (OpenNijiiroRW.SaveFileInstances[iPlayer].data.DanTitles != null)
			amount += OpenNijiiroRW.SaveFileInstances[iPlayer].data.DanTitles.Count;

		this.ttkDanTitles = new TitleTextureKey[amount];
		this.sDanTitles = new string[amount];

		// Silver Shinjin (default rank) always avaliable by default
		this.ttkDanTitles[0] = new TitleTextureKey("新人", this.pfHeyaFont, Color.White, Color.Black, 1000);
		this.sDanTitles[0] = "新人";

		int idx = 1;
		if (OpenNijiiroRW.SaveFileInstances[iPlayer].data.DanTitles != null) {
			foreach (var item in OpenNijiiroRW.SaveFileInstances[iPlayer].data.DanTitles) {
				this.sDanTitles[idx] = item.Key;
				if (item.Value.isGold == true)
					this.ttkDanTitles[idx] = new TitleTextureKey($"<g.#FFE34A.#EA9622>{item.Key}</g>", this.pfHeyaFont, Color.Gold, Color.Black, 1000);
				else
					this.ttkDanTitles[idx] = new TitleTextureKey(item.Key, this.pfHeyaFont, Color.White, Color.Black, 1000);
				idx++;
			}
		}

		#endregion

		#region [Plate title]

		amount = 1;
		if (OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedNameplateIds != null)
			amount += OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedNameplateIds.Count;

		this.ttkTitles = new TitleTextureKey[amount];
		this.titlesKeys = new int[amount];

		// Wood shoshinsha (default title) always avaliable by default
		this.ttkTitles[0] = new TitleTextureKey("初心者", this.pfHeyaFont, Color.Black, Color.Transparent, 1000);
		this.titlesKeys[0] = 0; // Regular nameplate unlockable start by 1 (Important)

		idx = 1;
		if (OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedNameplateIds != null) {
			foreach (var _ref in OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedNameplateIds) {
				var item = OpenNijiiroRW.Databases.DBNameplateUnlockables.data?[_ref];
				if (item != null) {
					this.ttkTitles[idx] = new TitleTextureKey(item.nameplateInfo.cld.GetString(""), this.pfHeyaFont, Color.Black, Color.Transparent, 1000);
					this.titlesKeys[idx] = _ref;
					idx++;
				}

			}
		}

		#endregion

		// -1 : Main Menu, >= 0 : See Main menu opt
		iCurrentMenu = CurrentMenu.ReturnToMenu;
		iMainMenuCurrent = 0;

		#region [PuchiChara stuff]

		iPuchiCharaCount = OpenNijiiroRW.Skin.Puchichara_Ptn;

		ttkPuchiCharaNames = new TitleTextureKey[iPuchiCharaCount];
		ttkPuchiCharaAuthors = new TitleTextureKey[iPuchiCharaCount];

		for (int i = 0; i < iPuchiCharaCount; i++) {
			var textColor = HRarity.tRarityToColor(OpenNijiiroRW.Tx.Puchichara[i].metadata.Rarity);
			ttkPuchiCharaNames[i] = new TitleTextureKey(OpenNijiiroRW.Tx.Puchichara[i].metadata.tGetName(), this.pfHeyaFont, textColor, Color.Black, 1000);
			ttkPuchiCharaAuthors[i] = new TitleTextureKey(OpenNijiiroRW.Tx.Puchichara[i].metadata.tGetAuthor(), this.pfHeyaFont, Color.White, Color.Black, 1000);
		}

		#endregion

		#region [Character stuff]

		iCharacterCount = OpenNijiiroRW.Skin.Characters_Ptn;

		ttkCharacterAuthors = new TitleTextureKey[iCharacterCount];
		ttkCharacterNames = new TitleTextureKey[iCharacterCount];

		for (int i = 0; i < iCharacterCount; i++) {
			var textColor = HRarity.tRarityToColor(OpenNijiiroRW.Tx.Characters[i].metadata.Rarity);
			ttkCharacterNames[i] = new TitleTextureKey(OpenNijiiroRW.Tx.Characters[i].metadata.tGetName(), this.pfHeyaFont, textColor, Color.Black, 1000);
			ttkCharacterAuthors[i] = new TitleTextureKey(OpenNijiiroRW.Tx.Characters[i].metadata.tGetAuthor(), this.pfHeyaFont, Color.White, Color.Black, 1000);
		}

		#endregion

		#region [Name stuff]
		textInput = new CTextInput("", 64);
		textInput.Text = OpenNijiiroRW.SaveFileInstances[iPlayer].data.Name;
		textInputTitle = new TitleTextureKey("", pfHeyaFont, Color.White, Color.Black, 1000);
		#endregion

		this.tResetOpts();

		this.PuchiChara.IdleAnimation();

		Background = new ScriptBG(CSkin.Path($"{TextureLoader.BASE}{TextureLoader.HEYA}Script.lua"));
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
		//ctChara_Normal.t進行Loop();
		ctChara_In.Tick();

		ScrollCounter.Tick();

		Background.Update();
		Background.Draw();
		//Heya_Background.t2D描画(0, 0);

		#region [Main menu (Side bar)]

		for (int i = 0; i < this.ttkMainMenuOpt.Length; i++) {
			CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(this.ttkMainMenuOpt[i]);

			if (iCurrentMenu != CurrentMenu.ReturnToMenu || iMainMenuCurrent != i) {
				tmpTex.color4 = CConversion.ColorToColor4(Color.DarkGray);
				OpenNijiiroRW.Tx.Heya_Side_Menu?.tUpdateColor4(CConversion.ColorToColor4(Color.DarkGray));
			} else {
				tmpTex.color4 = CConversion.ColorToColor4(Color.White);
				OpenNijiiroRW.Tx.Heya_Side_Menu?.tUpdateColor4(CConversion.ColorToColor4(Color.White));
			}

			OpenNijiiroRW.Tx.Heya_Side_Menu?.t2D拡大率考慮上中央基準描画(OpenNijiiroRW.Skin.Heya_Main_Menu_X[i], OpenNijiiroRW.Skin.Heya_Main_Menu_Y[i]);
			tmpTex.t2D拡大率考慮上中央基準描画(OpenNijiiroRW.Skin.Heya_Main_Menu_X[i] + OpenNijiiroRW.Skin.Heya_Main_Menu_Font_Offset[0], OpenNijiiroRW.Skin.Heya_Main_Menu_Y[i] + OpenNijiiroRW.Skin.Heya_Main_Menu_Font_Offset[1]);
		}

		#endregion

		#region [Background center]

		if (iCurrentMenu >= 0) {
			OpenNijiiroRW.Tx.Heya_Center_Menu_Background?.t2D描画(0, 0);
		}

		#endregion

		#region [Render field]

		float renderRatioX = 1.0f;
		float renderRatioY = 1.0f;

		if (OpenNijiiroRW.Skin.Characters_Resolution[iCharacterCurrent] != null) {
			renderRatioX = OpenNijiiroRW.Skin.Resolution[0] / (float)OpenNijiiroRW.Skin.Characters_Resolution[iCharacterCurrent][0];
			renderRatioY = OpenNijiiroRW.Skin.Resolution[1] / (float)OpenNijiiroRW.Skin.Characters_Resolution[iCharacterCurrent][1];
		}

		if (OpenNijiiroRW.Tx.Characters_Heya_Render[iCharacterCurrent] != null) {
			OpenNijiiroRW.Tx.Characters_Heya_Render[iCharacterCurrent].vcScaleRatio.X = renderRatioX;
			OpenNijiiroRW.Tx.Characters_Heya_Render[iCharacterCurrent].vcScaleRatio.Y = renderRatioY;
		}
		if (iCurrentMenu == CurrentMenu.Puchi || iCurrentMenu == CurrentMenu.Chara) OpenNijiiroRW.Tx.Heya_Render_Field?.t2D描画(0, 0);
		if (iCurrentMenu == CurrentMenu.Puchi) OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].render?.t2D描画(0, 0);
		if (iCurrentMenu == CurrentMenu.Chara) OpenNijiiroRW.Tx.Characters_Heya_Render[iCharacterCurrent]?.t2D描画(OpenNijiiroRW.Skin.Characters_Heya_Render_Offset[iCharacterCurrent][0] * renderRatioX, OpenNijiiroRW.Skin.Characters_Heya_Render_Offset[iCharacterCurrent][1] * renderRatioY);

		#endregion

		#region [Menus display]

		#region [Petit chara]

		if (iCurrentMenu == CurrentMenu.Puchi) {
			for (int i = -(OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Count / 2); i < (OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Count / 2) + 1; i++) {
				int pos = (iPuchiCharaCount * 5 + iPuchiCharaCurrent + i) % iPuchiCharaCount;

				if (i != 0) {
					OpenNijiiroRW.Tx.Puchichara[pos].tx?.tUpdateColor4(CConversion.ColorToColor4(Color.DarkGray));
					OpenNijiiroRW.Tx.Heya_Center_Menu_Box_Slot?.tUpdateColor4(CConversion.ColorToColor4(Color.DarkGray));
					OpenNijiiroRW.Tx.Heya_Lock?.tUpdateColor4(CConversion.ColorToColor4(Color.DarkGray));
				} else {
					OpenNijiiroRW.Tx.Puchichara[pos].tx?.tUpdateColor4(CConversion.ColorToColor4(Color.White));
					OpenNijiiroRW.Tx.Heya_Center_Menu_Box_Slot?.tUpdateColor4(CConversion.ColorToColor4(Color.White));
					OpenNijiiroRW.Tx.Heya_Lock?.tUpdateColor4(CConversion.ColorToColor4(Color.White));
				}

				var scroll = DrawBox_Slot(i + (OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Count / 2));

				PuchiChara.DrawPuchichara(pos,
					scroll.Item1 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Item_Offset[0], scroll.Item2 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Item_Offset[1],
				OpenNijiiroRW.Skin.Resolution[1] / 1080.0f, 255, true);

				OpenNijiiroRW.Tx.Puchichara[pos].tx?.tUpdateColor4(CConversion.ColorToColor4(Color.White));

				#region [Database related values]

				if (ttkPuchiCharaNames[pos] != null) {
					CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(ttkPuchiCharaNames[pos]);

					tmpTex.t2D拡大率考慮上中央基準描画(scroll.Item1 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Name_Offset[0],
						scroll.Item2 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Name_Offset[1]);
				}

				if (ttkPuchiCharaAuthors[pos] != null) {
					CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(ttkPuchiCharaAuthors[pos]);

					tmpTex.t2D拡大率考慮上中央基準描画(scroll.Item1 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Authors_Offset[0],
						scroll.Item2 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Authors_Offset[1]);
				}

				if (OpenNijiiroRW.Tx.Puchichara[pos].unlock != null
					&& !OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedPuchicharas.Contains(OpenNijiiroRW.Skin.Puchicharas_Name[pos]))
					OpenNijiiroRW.Tx.Heya_Lock?.t2D拡大率考慮上中央基準描画(scroll.Item1, scroll.Item2);

				#endregion


			}
		}

		#endregion

		#region [Character]

		if (iCurrentMenu == CurrentMenu.Chara) {
			for (int i = -(OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Count / 2); i < (OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Count / 2) + 1; i++) {
				int pos = (iCharacterCount * 5 + iCharacterCurrent + i) % iCharacterCount;

				float charaRatioX = 1.0f;
				float charaRatioY = 1.0f;

				if (i != 0) {
					OpenNijiiroRW.Tx.Characters_Heya_Preview[pos]?.tUpdateColor4(CConversion.ColorToColor4(Color.DarkGray));
					OpenNijiiroRW.Tx.Heya_Center_Menu_Box_Slot?.tUpdateColor4(CConversion.ColorToColor4(Color.DarkGray));
					OpenNijiiroRW.Tx.Heya_Lock?.tUpdateColor4(CConversion.ColorToColor4(Color.DarkGray));
				} else {
					OpenNijiiroRW.Tx.Characters_Heya_Preview[pos]?.tUpdateColor4(CConversion.ColorToColor4(Color.White));
					OpenNijiiroRW.Tx.Heya_Center_Menu_Box_Slot?.tUpdateColor4(CConversion.ColorToColor4(Color.White));
					OpenNijiiroRW.Tx.Heya_Lock?.tUpdateColor4(CConversion.ColorToColor4(Color.White));
				}

				var scroll = DrawBox_Slot(i + (OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Count / 2));

				if (OpenNijiiroRW.Skin.Characters_Resolution[pos] != null) {
					charaRatioX = OpenNijiiroRW.Skin.Resolution[0] / (float)OpenNijiiroRW.Skin.Characters_Resolution[pos][0];
					charaRatioY = OpenNijiiroRW.Skin.Resolution[1] / (float)OpenNijiiroRW.Skin.Characters_Resolution[pos][1];
				}

				if (OpenNijiiroRW.Tx.Characters_Heya_Preview[pos] != null) {
					OpenNijiiroRW.Tx.Characters_Heya_Preview[pos].vcScaleRatio.X = charaRatioX;
					OpenNijiiroRW.Tx.Characters_Heya_Preview[pos].vcScaleRatio.Y = charaRatioY;
				}

				OpenNijiiroRW.Tx.Characters_Heya_Preview[pos]?.t2D拡大率考慮中央基準描画(scroll.Item1 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Item_Offset[0],
					scroll.Item2 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Item_Offset[1]);

				OpenNijiiroRW.Tx.Characters_Heya_Preview[pos]?.tUpdateColor4(CConversion.ColorToColor4(Color.White));

				#region [Database related values]

				if (ttkCharacterNames[pos] != null) {
					CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(ttkCharacterNames[pos]);

					tmpTex.t2D拡大率考慮上中央基準描画(scroll.Item1 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Name_Offset[0],
						scroll.Item2 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Name_Offset[1]);
				}

				if (ttkCharacterAuthors[pos] != null) {
					CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(ttkCharacterAuthors[pos]);

					tmpTex.t2D拡大率考慮上中央基準描画(scroll.Item1 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Authors_Offset[0],
						scroll.Item2 + OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Authors_Offset[1]);
				}

				if (OpenNijiiroRW.Tx.Characters[pos].unlock != null
					&& !OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedCharacters.Contains(OpenNijiiroRW.Skin.Characters_DirName[pos]))
					OpenNijiiroRW.Tx.Heya_Lock?.t2D拡大率考慮上中央基準描画(scroll.Item1, scroll.Item2);

				#endregion
			}
		}

		#endregion

		#region [Dan title]

		if (iCurrentMenu == CurrentMenu.Dan) {
			for (int i = -(OpenNijiiroRW.Skin.Heya_Side_Menu_Count / 2); i < (OpenNijiiroRW.Skin.Heya_Side_Menu_Count / 2) + 1; i++) {
				int pos = (this.ttkDanTitles.Length * 5 + iDanTitleCurrent + i) % this.ttkDanTitles.Length;

				CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(this.ttkDanTitles[pos]);

				if (i != 0) {
					tmpTex.color4 = CConversion.ColorToColor4(Color.DarkGray);
					OpenNijiiroRW.Tx.Heya_Side_Menu?.tUpdateColor4(CConversion.ColorToColor4(Color.DarkGray));
					//TJAPlayer3.Tx.NamePlateBase.color4 = CConversion.ColorToColor4(Color.DarkGray);
				} else {
					tmpTex.color4 = CConversion.ColorToColor4(Color.White);
					OpenNijiiroRW.Tx.Heya_Side_Menu?.tUpdateColor4(CConversion.ColorToColor4(Color.White));
					//TJAPlayer3.Tx.NamePlateBase.color4 = CConversion.ColorToColor4(Color.White);
				}

				int danGrade = 0;
				if (pos > 0) {
					danGrade = OpenNijiiroRW.SaveFileInstances[iPlayer].data.DanTitles[this.sDanTitles[pos]].clearStatus;
				}

				var scroll = DrawSide_Menu(i + (OpenNijiiroRW.Skin.Heya_Side_Menu_Count / 2));

				/*
                TJAPlayer3.NamePlate.tNamePlateDisplayNamePlateBase(
                    scroll.Item1 - TJAPlayer3.Tx.NamePlateBase.szTextureSize.Width / 2,
                    scroll.Item2 - TJAPlayer3.Tx.NamePlateBase.szTextureSize.Height / 24,
                    (8 + danGrade));
                TJAPlayer3.Tx.NamePlateBase.color4 = CConversion.ColorToColor4(Color.White);

                tmpTex.t2D拡大率考慮上中央基準描画(scroll.Item1 + TJAPlayer3.Skin.Heya_Side_Menu_Font_Offset[0], scroll.Item2 + TJAPlayer3.Skin.Heya_Side_Menu_Font_Offset[1]);
                */
				OpenNijiiroRW.NamePlate.lcNamePlate.DrawDan(scroll.Item1, scroll.Item2, 255, danGrade, tmpTex);

			}
		}

		#endregion

		#region [Title plate]

		if (iCurrentMenu == CurrentMenu.Title) {
			for (int i = -(OpenNijiiroRW.Skin.Heya_Side_Menu_Count / 2); i < (OpenNijiiroRW.Skin.Heya_Side_Menu_Count / 2) + 1; i++) {
				int pos = (this.ttkTitles.Length * 5 + iTitleCurrent + i) % this.ttkTitles.Length;

				CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(this.ttkTitles[pos]);

				if (i != 0) {
					tmpTex.color4 = CConversion.ColorToColor4(Color.DarkGray);
					OpenNijiiroRW.Tx.Heya_Side_Menu.color4 = CConversion.ColorToColor4(Color.DarkGray);
				} else {
					tmpTex.color4 = CConversion.ColorToColor4(Color.White);
					OpenNijiiroRW.Tx.Heya_Side_Menu.color4 = CConversion.ColorToColor4(Color.White);
				}

				var scroll = DrawSide_Menu(i + (OpenNijiiroRW.Skin.Heya_Side_Menu_Count / 2));

				int iType = -1;
				int _rarity = 1;
				int _titleid = -1;

				if (OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedNameplateIds != null &&
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedNameplateIds.Contains(this.titlesKeys[pos])) {
					var _dc = OpenNijiiroRW.Databases.DBNameplateUnlockables.data[this.titlesKeys[pos]];
					iType = _dc.nameplateInfo.iType;
					_rarity = HRarity.tRarityToLangInt(_dc.rarity);
					_titleid = this.titlesKeys[pos];
					//iType = TJAPlayer3.SaveFileInstances[iPlayer].data.NamePlateTitles[this.titlesKeys[pos]].iType;
				} else if (pos == 0)
					iType = 0;

				/*
                if (iType >= 0 && iType < TJAPlayer3.Skin.Config_NamePlate_Ptn_Title)
                {
                    TJAPlayer3.Tx.NamePlate_Title[iType][TJAPlayer3.NamePlate.ctAnimatedNamePlateTitle.CurrentValue % TJAPlayer3.Skin.Config_NamePlate_Ptn_Title_Boxes[iType]].t2D拡大率考慮上中央基準描画(
                        scroll.Item1,
                        scroll.Item2);

                }
                */
				OpenNijiiroRW.NamePlate.lcNamePlate.DrawTitlePlate(scroll.Item1, scroll.Item2, 255, iType, tmpTex, _rarity, _titleid);

				//tmpTex.t2D拡大率考慮上中央基準描画(scroll.Item1 + TJAPlayer3.Skin.Heya_Side_Menu_Font_Offset[0], scroll.Item2 + TJAPlayer3.Skin.Heya_Side_Menu_Font_Offset[1]);

			}
		}

		if (iCurrentMenu == CurrentMenu.Name) {
			textInput.Update();

			HBlackBackdrop.Draw(191);

			if (textInput.DisplayText != textInputTitle.str) { textInputTitle = new TitleTextureKey(textInput.DisplayText, pfHeyaFont, Color.White, Color.Black, 1000); }
			CTexture text_tex = TitleTextureKey.ResolveTitleTexture(textInputTitle);
			CTexture text_info = TitleTextureKey.ResolveTitleTexture(textInputInfo);

			text_info.t2D_DisplayImage_AnchorCenter(RenderSurfaceSize.Width / 2, RenderSurfaceSize.Height / 2);
			text_tex.t2D_DisplayImage_AnchorCenter(RenderSurfaceSize.Width / 2, RenderSurfaceSize.Height / 2 + text_info.szTextureSize.Height);
		}

		#endregion

		#endregion

		#region [Description area]

		if (iCurrentMenu >= 0) {
			#region [Unlockable information zone]

			if (this.ttkInfoSection != null && this.ttkInfoSection.str != "")
				OpenNijiiroRW.Tx.Heya_Box?.t2D描画(0, 0);

			if (this.ttkInfoSection != null)
				TitleTextureKey.ResolveTitleTexture(this.ttkInfoSection)
					.t2D拡大率考慮上中央基準描画(OpenNijiiroRW.Skin.Heya_InfoSection[0], OpenNijiiroRW.Skin.Heya_InfoSection[1]);

			#endregion

			#region [Asset description]

			if (this.ttkInfoSection == null || this.ttkInfoSection.str == "") {
				if (iCurrentMenu == CurrentMenu.Puchi) CHeyaDisplayAssetInformations.DisplayPuchicharaInfo(this.pfHeyaFont, OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent]);
				if (iCurrentMenu == CurrentMenu.Chara) CHeyaDisplayAssetInformations.DisplayCharacterInfo(this.pfHeyaFont, OpenNijiiroRW.Tx.Characters[iCharacterCurrent]);
			}

			#endregion
		}

		#endregion

		#region [General Chara animations]

		if (!ctChara_In.IsStarted) {
			OpenNijiiroRW.Skin.soundHeyaBGM.tPlay();
			ctChara_In.Start(0, 180, 1.25f, OpenNijiiroRW.Timer);
		}

		#region [ キャラ関連 ]

		if (ctChara_In.CurrentValue != 90) {
			float CharaX = 0f, CharaY = 0f;

			CharaX = -200 + (float)Math.Sin(ctChara_In.CurrentValue / 2 * (Math.PI / 180)) * 200f;
			CharaY = ((float)Math.Sin((90 + (ctChara_In.CurrentValue / 2)) * (Math.PI / 180)) * 150f);

			//int _charaId = TJAPlayer3.NamePlateConfig.data.Character[TJAPlayer3.GetActualPlayer(0)];

			//int chara_x = (int)(TJAPlayer3.Skin.Characters_Menu_X[_charaId][0] + (-200 + CharaX));
			//int chara_y = (int)(TJAPlayer3.Skin.Characters_Menu_Y[_charaId][0] - CharaY);

			int chara_x = (int)CharaX + OpenNijiiroRW.Skin.SongSelect_NamePlate_X[0] + OpenNijiiroRW.Tx.NamePlateBase.szTextureSize.Width / 2;
			int chara_y = OpenNijiiroRW.Skin.SongSelect_NamePlate_Y[0] - (int)CharaY;

			int puchi_x = chara_x + OpenNijiiroRW.Skin.Adjustments_MenuPuchichara_X[0];
			int puchi_y = chara_y + OpenNijiiroRW.Skin.Adjustments_MenuPuchichara_Y[0];

			//TJAPlayer3.Tx.SongSelect_Chara_Normal[ctChara_Normal.n現在の値].Opacity = ctChara_In.n現在の値 * 2;
			//TJAPlayer3.Tx.SongSelect_Chara_Normal[ctChara_Normal.n現在の値].t2D描画(-200 + CharaX, 336 - CharaY);

			CMenuCharacter.tMenuDisplayCharacter(0, chara_x, chara_y, CMenuCharacter.ECharacterAnimation.NORMAL);

			#region [PuchiChara]

			this.PuchiChara.On進行描画(puchi_x, puchi_y, false);

			#endregion
		}

		#endregion

		OpenNijiiroRW.NamePlate.tNamePlateDraw(OpenNijiiroRW.Skin.SongSelect_NamePlate_X[0], OpenNijiiroRW.Skin.SongSelect_NamePlate_Y[0] + 5, 0);

		#endregion



		#region [ Inputs ]

		if (OpenNijiiroRW.InputManager.Keyboard.KeyPressing((int)SlimDXKeys.Key.RightArrow) ||
			OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.RightChange)) {
			if (this.tMove(1)) {
				OpenNijiiroRW.Skin.soundChangeSFX.tPlay();
			}
		} else if (OpenNijiiroRW.InputManager.Keyboard.KeyPressing((int)SlimDXKeys.Key.LeftArrow) ||
				   OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.LeftChange)) {
			if (this.tMove(-1)) {
				OpenNijiiroRW.Skin.soundChangeSFX.tPlay();
			}
		} else if (iCurrentMenu != CurrentMenu.Name && (OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Return) ||
					 OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.Decide))) {

			#region [Decide]

			ESelectStatus ess = ESelectStatus.SELECTED;

			// Return to main menu
			if (iCurrentMenu == CurrentMenu.ReturnToMenu && iMainMenuCurrent == 0) {
				OpenNijiiroRW.Skin.soundHeyaBGM.tStop();
				this.eフェードアウト完了時の戻り値 = E戻り値.タイトルに戻る;
				this.actFOtoTitle.tフェードアウト開始();
				base.ePhaseID = CStage.EPhase.Common_FADEOUT;
			} else if (iCurrentMenu == CurrentMenu.ReturnToMenu) {
				iCurrentMenu = (CurrentMenu)iMainMenuCurrent - 1;

				if (iCurrentMenu == CurrentMenu.Puchi) {
					this.tUpdateUnlockableTextChara();
					this.tUpdateUnlockableTextPuchi();
				} else if (iCurrentMenu == CurrentMenu.Name) {
					textInput.Text = OpenNijiiroRW.SaveFileInstances[iPlayer].data.Name;
				}
			} else if (iCurrentMenu == CurrentMenu.Puchi) {
				ess = this.tSelectPuchi();

				if (ess == ESelectStatus.SELECTED) {
					//PuchiChara.tGetPuchiCharaIndexByName(p);
					//TJAPlayer3.NamePlateConfig.data.PuchiChara[iPlayer] = TJAPlayer3.Skin.Puchicharas_Name[iPuchiCharaCurrent];// iPuchiCharaCurrent;
					//TJAPlayer3.NamePlateConfig.tApplyHeyaChanges();
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.PuchiChara = OpenNijiiroRW.Skin.Puchicharas_Name[iPuchiCharaCurrent];// iPuchiCharaCurrent;
					OpenNijiiroRW.SaveFileInstances[iPlayer].tApplyHeyaChanges();
					OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].welcome.tPlay();

					iCurrentMenu = CurrentMenu.ReturnToMenu;
					this.tResetOpts();
				} else if (ess == ESelectStatus.SUCCESS) {
					//TJAPlayer3.NamePlateConfig.data.UnlockedPuchicharas[iPlayer].Add(TJAPlayer3.Skin.Puchicharas_Name[iPuchiCharaCurrent]);
					//TJAPlayer3.NamePlateConfig.tSpendCoins(TJAPlayer3.Tx.Puchichara[iPuchiCharaCurrent].unlock.Values[0], iPlayer);
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedPuchicharas.Add(OpenNijiiroRW.Skin.Puchicharas_Name[iPuchiCharaCurrent]);
					DBSaves.RegisterStringUnlockedAsset(OpenNijiiroRW.SaveFileInstances[iPlayer].data.SaveId, "unlocked_puchicharas", OpenNijiiroRW.Skin.Puchicharas_Name[iPuchiCharaCurrent]);
					if (OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].unlock is CUnlockCH)
						OpenNijiiroRW.SaveFileInstances[iPlayer].tSpendCoins(OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].unlock.Values[0]);
					else if (OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].unlock is CUnlockAndComb || OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].unlock is CUnlockOrComb)
						OpenNijiiroRW.SaveFileInstances[iPlayer].tSpendCoins(OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].unlock.CoinStack);

				}
			} else if (iCurrentMenu == CurrentMenu.Chara) {
				ess = this.tSelectChara();

				if (ess == ESelectStatus.SELECTED) {
					//TJAPlayer3.Tx.Loading?.t2D描画(18, 7);

					// Reload character, a bit time expensive but with a O(N) memory complexity instead of O(N * M)
					OpenNijiiroRW.Tx.ReloadCharacter(OpenNijiiroRW.SaveFileInstances[iPlayer].data.Character, iCharacterCurrent, iPlayer);
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.Character = iCharacterCurrent;

					// Update the character
					OpenNijiiroRW.SaveFileInstances[iPlayer].tUpdateCharacterName(OpenNijiiroRW.Skin.Characters_DirName[iCharacterCurrent]);

					// Welcome voice using Sanka
					OpenNijiiroRW.Skin.voiceTitleSanka[iPlayer]?.tPlay();

					CMenuCharacter.tMenuResetTimer(CMenuCharacter.ECharacterAnimation.NORMAL);

					OpenNijiiroRW.SaveFileInstances[iPlayer].tApplyHeyaChanges();

					iCurrentMenu = CurrentMenu.ReturnToMenu;
					this.tResetOpts();
				} else if (ess == ESelectStatus.SUCCESS) {
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedCharacters.Add(OpenNijiiroRW.Skin.Characters_DirName[iCharacterCurrent]);
					DBSaves.RegisterStringUnlockedAsset(OpenNijiiroRW.SaveFileInstances[iPlayer].data.SaveId, "unlocked_characters", OpenNijiiroRW.Skin.Characters_DirName[iCharacterCurrent]);
					if (OpenNijiiroRW.Tx.Characters[iCharacterCurrent].unlock is CUnlockCH)
						OpenNijiiroRW.SaveFileInstances[iPlayer].tSpendCoins(OpenNijiiroRW.Tx.Characters[iCharacterCurrent].unlock.Values[0]);
					else if (OpenNijiiroRW.Tx.Characters[iCharacterCurrent].unlock is CUnlockAndComb || OpenNijiiroRW.Tx.Characters[iCharacterCurrent].unlock is CUnlockOrComb)
						OpenNijiiroRW.SaveFileInstances[iPlayer].tSpendCoins(OpenNijiiroRW.Tx.Characters[iCharacterCurrent].unlock.CoinStack);
					// Play modal animation here ?
				}
			} else if (iCurrentMenu == CurrentMenu.Dan) {
				bool iG = false;
				int cs = 0;

				if (iDanTitleCurrent > 0) {
					iG = OpenNijiiroRW.SaveFileInstances[iPlayer].data.DanTitles[this.sDanTitles[iDanTitleCurrent]].isGold;
					cs = OpenNijiiroRW.SaveFileInstances[iPlayer].data.DanTitles[this.sDanTitles[iDanTitleCurrent]].clearStatus;
				}

				OpenNijiiroRW.SaveFileInstances[iPlayer].data.Dan = this.sDanTitles[iDanTitleCurrent];
				OpenNijiiroRW.SaveFileInstances[iPlayer].data.DanGold = iG;
				OpenNijiiroRW.SaveFileInstances[iPlayer].data.DanType = cs;

				OpenNijiiroRW.NamePlate.tNamePlateRefreshTitles(0);

				OpenNijiiroRW.SaveFileInstances[iPlayer].tApplyHeyaChanges();

				iCurrentMenu = CurrentMenu.ReturnToMenu;
				this.tResetOpts();
			} else if (iCurrentMenu == CurrentMenu.Title) {
				OpenNijiiroRW.SaveFileInstances[iPlayer].data.Title = this.ttkTitles[iTitleCurrent].str;

				if (OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedNameplateIds != null
					&& OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedNameplateIds.Contains(this.titlesKeys[iTitleCurrent])) {
					var _dc = OpenNijiiroRW.Databases.DBNameplateUnlockables.data[this.titlesKeys[iTitleCurrent]];
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.TitleType = _dc.nameplateInfo.iType;
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.TitleId = this.titlesKeys[iTitleCurrent];
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.TitleRarityInt = HRarity.tRarityToLangInt(_dc.rarity);
				} else if (iTitleCurrent == 0) {
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.TitleType = 0;
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.TitleId = -1;
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.TitleRarityInt = 1;
				} else {
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.TitleType = -1;
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.TitleId = -1;
					OpenNijiiroRW.SaveFileInstances[iPlayer].data.TitleRarityInt = 1;
				}


				OpenNijiiroRW.NamePlate.tNamePlateRefreshTitles(0);

				OpenNijiiroRW.SaveFileInstances[iPlayer].tApplyHeyaChanges();

				iCurrentMenu = CurrentMenu.ReturnToMenu;
				this.tResetOpts();
			}

			if (ess == ESelectStatus.SELECTED)
				OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
			else if (ess == ESelectStatus.FAILED)
				OpenNijiiroRW.Skin.soundError.tPlay();
			else
				OpenNijiiroRW.Skin.SoundBanapas.tPlay(); // To change with a more appropriate sfx sooner or later

			#endregion
		} else if (iCurrentMenu != CurrentMenu.Name && (OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Escape) ||
					 OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.Cancel))) {

			OpenNijiiroRW.Skin.soundCancelSFX.tPlay();

			if (iCurrentMenu == CurrentMenu.ReturnToMenu) {
				OpenNijiiroRW.Skin.soundHeyaBGM.tStop();
				this.eフェードアウト完了時の戻り値 = E戻り値.タイトルに戻る;
				this.actFOtoTitle.tフェードアウト開始();
				base.ePhaseID = CStage.EPhase.Common_FADEOUT;
			} else {
				iCurrentMenu = CurrentMenu.ReturnToMenu;
				this.ttkInfoSection = null;
				this.tResetOpts();
			}


			return 0;
		} else if (iCurrentMenu == CurrentMenu.Name && OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Return)) {
			OpenNijiiroRW.SaveFileInstances[iPlayer].data.Name = textInput.Text;
			OpenNijiiroRW.SaveFileInstances[iPlayer].tApplyHeyaChanges();
			OpenNijiiroRW.NamePlate.tNamePlateRefreshTitles(iPlayer);

			iCurrentMenu = CurrentMenu.ReturnToMenu;
			this.tResetOpts();
			OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
			return 0;
		} else if (iCurrentMenu == CurrentMenu.Name && OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Escape)) {
			OpenNijiiroRW.Skin.soundCancelSFX.tPlay();
			iCurrentMenu = CurrentMenu.ReturnToMenu;
			this.ttkInfoSection = null;
			this.tResetOpts();
			return 0;
		}

		#endregion

		switch (base.ePhaseID) {
			case CStage.EPhase.Common_FADEOUT:
				if (this.actFOtoTitle.Draw() == 0) {
					break;
				}
				return (int)this.eフェードアウト完了時の戻り値;

		}

		return 0;
	}

	public enum E戻り値 : int {
		継続,
		タイトルに戻る,
		選曲した
	}

	public bool bInSongPlayed;

	private CCounter ctChara_In;
	//private CCounter ctChara_Normal;

	private PuchiChara PuchiChara;

	private int iPlayer;

	private int iMainMenuCurrent;
	private int iPuchiCharaCurrent;

	private TitleTextureKey[] ttkPuchiCharaNames;
	private TitleTextureKey[] ttkPuchiCharaAuthors;
	private TitleTextureKey[] ttkCharacterNames;
	private TitleTextureKey[] ttkCharacterAuthors;
	private TitleTextureKey ttkInfoSection;

	private int iCharacterCurrent;
	private int iDanTitleCurrent;
	private int iTitleCurrent;

	private CurrentMenu iCurrentMenu;

	private void tResetOpts() {
		// Retrieve titles if they exist
		//var _titles = TJAPlayer3.SaveFileInstances[iPlayer].data.NamePlateTitles;
		var _titles = OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedNameplateIds;
		var _title = OpenNijiiroRW.SaveFileInstances[iPlayer].data.Title;
		var _dans = OpenNijiiroRW.SaveFileInstances[iPlayer].data.DanTitles;
		var _dan = OpenNijiiroRW.SaveFileInstances[iPlayer].data.Dan;

		iTitleCurrent = 0;

		// Think of a replacement later
		/*
        if (_titles != null && _titles.ContainsKey(_title)) { }
            iTitleCurrent = _titles.Keys.ToList().IndexOf(_title) + 1;
        */

		iDanTitleCurrent = 0;

		if (_dans != null && _dans.ContainsKey(_dan))
			iDanTitleCurrent = _dans.Keys.ToList().IndexOf(_dan) + 1;

		foreach (var plate in _titles.Select((value, i) => new { i, value })) {
			if (OpenNijiiroRW.SaveFileInstances[iPlayer].data.TitleId == plate.value)
				iTitleCurrent = plate.i + 1;
		}

		iCharacterCurrent = Math.Max(0, Math.Min(OpenNijiiroRW.Skin.Characters_Ptn - 1, OpenNijiiroRW.SaveFileInstances[iPlayer].data.Character));

		//iPuchiCharaCurrent = Math.Max(0, Math.Min(TJAPlayer3.Skin.Puchichara_Ptn - 1, TJAPlayer3.NamePlateConfig.data.PuchiChara[this.iPlayer]));
		iPuchiCharaCurrent = PuchiChara.tGetPuchiCharaIndexByName(this.iPlayer);
	}



	private bool tMove(int off) {
		if (ScrollCounter.CurrentValue < ScrollCounter.EndValue
			&& (OpenNijiiroRW.InputManager.Keyboard.KeyPressing((int)SlimDXKeys.Key.RightArrow)
				|| OpenNijiiroRW.InputManager.Keyboard.KeyPressing((int)SlimDXKeys.Key.LeftArrow)))
			return false;

		ScrollMode = off;
		ScrollCounter.CurrentValue = 0;

		switch (iCurrentMenu) {
			case CurrentMenu.ReturnToMenu:
				iMainMenuCurrent = (this.ttkMainMenuOpt.Length + iMainMenuCurrent + off) % this.ttkMainMenuOpt.Length;
				break;
			case CurrentMenu.Puchi:
				iPuchiCharaCurrent = (iPuchiCharaCount + iPuchiCharaCurrent + off) % iPuchiCharaCount;
				tUpdateUnlockableTextPuchi();
				break;
			case CurrentMenu.Chara:
				iCharacterCurrent = (iCharacterCount + iCharacterCurrent + off) % iCharacterCount;
				tUpdateUnlockableTextChara();
				break;
			case CurrentMenu.Dan:
				iDanTitleCurrent = (this.ttkDanTitles.Length + iDanTitleCurrent + off) % this.ttkDanTitles.Length;
				break;
			case CurrentMenu.Title:
				iTitleCurrent = (this.ttkTitles.Length + iTitleCurrent + off) % this.ttkTitles.Length;
				break;
			default:
				return false;

		}
		return true;
	}

	private (int, int) DrawBox_Slot(int i) {
		double value = (1.0 - Math.Sin((((ScrollCounter.CurrentValue) / 2000.0)) * Math.PI));

		int nextIndex = i + ScrollMode;
		nextIndex = Math.Min(OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Count - 1, nextIndex);
		nextIndex = Math.Max(0, nextIndex);

		int x = OpenNijiiroRW.Skin.Heya_Center_Menu_Box_X[i] + (int)((OpenNijiiroRW.Skin.Heya_Center_Menu_Box_X[nextIndex] - OpenNijiiroRW.Skin.Heya_Center_Menu_Box_X[i]) * value);
		int y = OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Y[i] + (int)((OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Y[nextIndex] - OpenNijiiroRW.Skin.Heya_Center_Menu_Box_Y[i]) * value);

		OpenNijiiroRW.Tx.Heya_Center_Menu_Box_Slot?.t2D拡大率考慮上中央基準描画(x, y);
		return (x, y);
	}

	private (int, int) DrawSide_Menu(int i) {
		double value = (1.0 - Math.Sin((((ScrollCounter.CurrentValue) / 2000.0)) * Math.PI));

		int nextIndex = i + ScrollMode;
		nextIndex = Math.Min(OpenNijiiroRW.Skin.Heya_Side_Menu_Count - 1, nextIndex);
		nextIndex = Math.Max(0, nextIndex);

		int x = OpenNijiiroRW.Skin.Heya_Side_Menu_X[i] + (int)((OpenNijiiroRW.Skin.Heya_Side_Menu_X[nextIndex] - OpenNijiiroRW.Skin.Heya_Side_Menu_X[i]) * value);
		int y = OpenNijiiroRW.Skin.Heya_Side_Menu_Y[i] + (int)((OpenNijiiroRW.Skin.Heya_Side_Menu_Y[nextIndex] - OpenNijiiroRW.Skin.Heya_Side_Menu_Y[i]) * value);

		OpenNijiiroRW.Tx.Heya_Side_Menu.t2D拡大率考慮上中央基準描画(x, y);
		return (x, y);
	}

	#region [Unlockables]

	/*
	 *  FAILED : Selection/Purchase failed (failed condition)
	 *  SUCCESS : Purchase succeed (without selection)
	 *  SELECTED : Selection succeed
	 */
	private enum ESelectStatus {
		FAILED,
		SUCCESS,
		SELECTED
	};


	#region [Chara unlockables]

	private void tUpdateUnlockableTextChara() {
		#region [Check unlockable]

		if (OpenNijiiroRW.Tx.Characters[iCharacterCurrent].unlock != null
			&& !OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedCharacters.Contains(OpenNijiiroRW.Skin.Characters_DirName[iCharacterCurrent])) {
			string _cond = OpenNijiiroRW.Tx.Characters[iCharacterCurrent].unlock.tConditionMessage();
			this.ttkInfoSection = new TitleTextureKey(_cond, this.pfHeyaFont, Color.White, Color.Black, 1000);
		} else
			this.ttkInfoSection = null;

		#endregion
	}
	private ESelectStatus tSelectChara() {
		// Add "If unlocked" to select directly

		if (OpenNijiiroRW.Tx.Characters[iCharacterCurrent].unlock != null
			&& !OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedCharacters.Contains(OpenNijiiroRW.Skin.Characters_DirName[iCharacterCurrent])) {
			(bool, string?) response = OpenNijiiroRW.Tx.Characters[iCharacterCurrent].unlock.tConditionMet(OpenNijiiroRW.SaveFile);
			//TJAPlayer3.Tx.Characters[iCharacterCurrent].unlock.tConditionMet(
			//new int[] { TJAPlayer3.SaveFileInstances[TJAPlayer3.SaveFile].data.Medals });

			Color responseColor = (response.Item1) ? Color.Lime : Color.Red;

			// Send coins here for the unlock, considering that only coin-paid puchicharas can be unlocked directly from the Heya menu

			this.ttkInfoSection = new TitleTextureKey(response.Item2 ?? this.ttkInfoSection.str, this.pfHeyaFont, responseColor, Color.Black, 1000);

			return (response.Item1) ? ESelectStatus.SUCCESS : ESelectStatus.FAILED;
		}

		this.ttkInfoSection = null;
		return ESelectStatus.SELECTED;
	}

	#endregion

	#region [Puchi unlockables]
	private void tUpdateUnlockableTextPuchi() {
		#region [Check unlockable]

		if (OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].unlock != null
			&& !OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedPuchicharas.Contains(OpenNijiiroRW.Skin.Puchicharas_Name[iPuchiCharaCurrent])) {
			string _cond = OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].unlock.tConditionMessage();
			this.ttkInfoSection = new TitleTextureKey(_cond, this.pfHeyaFont, Color.White, Color.Black, 1000);
		} else
			this.ttkInfoSection = null;

		#endregion
	}

	private ESelectStatus tSelectPuchi() {
		// Add "If unlocked" to select directly

		if (OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].unlock != null
			&& !OpenNijiiroRW.SaveFileInstances[iPlayer].data.UnlockedPuchicharas.Contains(OpenNijiiroRW.Skin.Puchicharas_Name[iPuchiCharaCurrent])) {
			(bool, string?) response = OpenNijiiroRW.Tx.Puchichara[iPuchiCharaCurrent].unlock.tConditionMet(OpenNijiiroRW.SaveFile);
			//tConditionMet(
			//new int[] { TJAPlayer3.SaveFileInstances[TJAPlayer3.SaveFile].data.Medals });

			Color responseColor = (response.Item1) ? Color.Lime : Color.Red;

			// Send coins here for the unlock, considering that only coin-paid puchicharas can be unlocked directly from the Heya menu

			this.ttkInfoSection = new TitleTextureKey(response.Item2 ?? this.ttkInfoSection.str, this.pfHeyaFont, responseColor, Color.Black, 1000);

			return (response.Item1) ? ESelectStatus.SUCCESS : ESelectStatus.FAILED;
		}

		this.ttkInfoSection = null;
		return ESelectStatus.SELECTED;
	}

	#endregion

	#endregion

	private ScriptBG Background;

	private TitleTextureKey[] ttkMainMenuOpt;
	private CCachedFontRenderer pfHeyaFont;

	private TitleTextureKey[] ttkDanTitles;
	private string[] sDanTitles;

	private TitleTextureKey[] ttkTitles;
	private int[] titlesKeys;

	private int iPuchiCharaCount;
	private int iCharacterCount;

	private TitleTextureKey textInputInfo;
	private TitleTextureKey textInputTitle;
	private CTextInput textInput;
	private bool isInputtingText;

	private CCounter ScrollCounter;
	private const int SideInterval_X = 10;
	private const int SideInterval_Y = 70;
	private int ScrollMode;

	public E戻り値 eフェードアウト完了時の戻り値;

	public CActFIFOBlack actFOtoTitle;
}
