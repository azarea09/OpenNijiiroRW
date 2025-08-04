using System.Drawing;
using FDK;

namespace OpenNijiiroRW;

class CActNewHeya : CActivity {
	public bool IsOpend { get; private set; }
	private CCachedFontRenderer MenuFont;

	private TitleTextureKey[] MenuTitleKeys = new TitleTextureKey[5];
	private TitleTextureKey[] ttkPuchiCharaNames;
	private TitleTextureKey[] ttkPuchiCharaAuthors;
	private TitleTextureKey[] ttkCharacterNames;
	private TitleTextureKey[] ttkCharacterAuthors;
	private TitleTextureKey ttkInfoSection;
	private TitleTextureKey[] ttkDanTitles;
	private TitleTextureKey[] ttkTitles;
	private string[] titlesKeys;

	public CCounter InFade;

	public CCounter CharaBoxAnime;

	private int CurrentIndex;

	private int CurrentMaxIndex;

	private int CurrentPlayer;

	private enum SelectableInfo {
		PlayerSelect,
		ModeSelect,
		Select
	}

	private enum ModeType {
		None = -1,
		PuchiChara,
		Chara,
		DanTitle,
		SubTitle
	}

	private SelectableInfo CurrentState;

	private ModeType CurrentMode = ModeType.None;

	private void SetState(SelectableInfo selectableInfo) {
		CurrentState = selectableInfo;
		switch (selectableInfo) {
			case SelectableInfo.PlayerSelect:
				CurrentIndex = 1;
				CurrentMaxIndex = OpenNijiiroRW.ConfigIni.nPlayerCount + 1;
				break;
			case SelectableInfo.ModeSelect:
				CurrentIndex = 1;
				CurrentMaxIndex = 5;
				break;
			case SelectableInfo.Select:
				CurrentMode = (ModeType)(CurrentIndex - 1);
				switch (CurrentMode) {
					case ModeType.Chara:
						CurrentMaxIndex = OpenNijiiroRW.Skin.Characters_Ptn;
						break;
					case ModeType.PuchiChara:
						CurrentMaxIndex = OpenNijiiroRW.Skin.Puchichara_Ptn;
						break;
					case ModeType.DanTitle: {
							int amount = 1;
							if (OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.DanTitles != null)
								amount += OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.DanTitles.Count;

							this.ttkDanTitles = new TitleTextureKey[amount];

							// Silver Shinjin (default rank) always avaliable by default
							this.ttkDanTitles[0] = new TitleTextureKey("新人", this.MenuFont, Color.White, Color.Black, 1000);

							int idx = 1;
							if (OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.DanTitles != null) {
								foreach (var item in OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.DanTitles) {
									if (item.Value.isGold == true)
										this.ttkDanTitles[idx] = new TitleTextureKey(item.Key, this.MenuFont, Color.Gold, Color.Black, 1000);
									else
										this.ttkDanTitles[idx] = new TitleTextureKey(item.Key, this.MenuFont, Color.White, Color.Black, 1000);
									idx++;
								}
							}

							CurrentMaxIndex = amount;
						}
						break;
					case ModeType.SubTitle: {
							int amount = 1;
							if (OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedNameplateIds != null)
								amount += OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedNameplateIds.Count;

							this.ttkTitles = new TitleTextureKey[amount];
							this.titlesKeys = new string[amount];

							// Wood shojinsha (default title) always avaliable by default
							this.ttkTitles[0] = new TitleTextureKey("初心者", this.MenuFont, Color.Black, Color.Transparent, 1000);
							this.titlesKeys[0] = "初心者";

							int idx = 1;
							if (OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedNameplateIds != null) {
								foreach (var item in OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedNameplateIds) {
									var name = OpenNijiiroRW.Databases.DBNameplateUnlockables.data[item];
									this.ttkTitles[idx] = new TitleTextureKey(name.nameplateInfo.cld.GetString(""), this.MenuFont, Color.Black, Color.Transparent, 1000);
									this.titlesKeys[idx] = name.nameplateInfo.cld.GetString("");
									idx++;
								}
							}

							CurrentMaxIndex = amount;
						}
						break;
				}
				CurrentIndex = 0;
				break;
		}
	}

	private void ChangeIndex(int change) {
		CurrentIndex += change;

		if (CurrentIndex < 0) CurrentIndex = CurrentMaxIndex - 1;
		else if (CurrentIndex >= CurrentMaxIndex) CurrentIndex = 0;
		if (CurrentState == SelectableInfo.Select) {
			switch (CurrentMode) {
				case ModeType.PuchiChara:
					tUpdateUnlockableTextPuchi();
					break;
				case ModeType.Chara:
					tUpdateUnlockableTextChara();
					break;
				case ModeType.DanTitle:
					break;
				case ModeType.SubTitle:
					break;
			}
		}
	}

	public void Open() {
		InFade = new CCounter(0, 255, 1.0, OpenNijiiroRW.Timer);
		IsOpend = true;
		CurrentMode = ModeType.None;

		SetState(SelectableInfo.PlayerSelect);
	}

	public void Close() {
		IsOpend = false;
	}

	public override void Activate() {
		InFade = new CCounter();
		CharaBoxAnime = new CCounter();

		MenuTitleKeys[0] = new TitleTextureKey(CLangManager.LangInstance.GetString("MENU_RETURN"), MenuFont, Color.White, Color.Black, 9999);
		MenuTitleKeys[1] = new TitleTextureKey(CLangManager.LangInstance.GetString("HEYA_PUCHI"), MenuFont, Color.White, Color.Black, 9999);
		MenuTitleKeys[2] = new TitleTextureKey(CLangManager.LangInstance.GetString("HEYA_CHARA"), MenuFont, Color.White, Color.Black, 9999);
		MenuTitleKeys[3] = new TitleTextureKey(CLangManager.LangInstance.GetString("HEYA_DAN"), MenuFont, Color.White, Color.Black, 9999);
		MenuTitleKeys[4] = new TitleTextureKey(CLangManager.LangInstance.GetString("HEYA_NAMEPLATE"), MenuFont, Color.White, Color.Black, 9999);

		ttkPuchiCharaNames = new TitleTextureKey[OpenNijiiroRW.Skin.Puchichara_Ptn];
		ttkPuchiCharaAuthors = new TitleTextureKey[OpenNijiiroRW.Skin.Puchichara_Ptn];

		for (int i = 0; i < OpenNijiiroRW.Skin.Puchichara_Ptn; i++) {
			var textColor = HRarity.tRarityToColor(OpenNijiiroRW.Tx.Puchichara[i].metadata.Rarity);
			ttkPuchiCharaNames[i] = new TitleTextureKey(OpenNijiiroRW.Tx.Puchichara[i].metadata.tGetName(), this.MenuFont, textColor, Color.Black, 1000);
			ttkPuchiCharaAuthors[i] = new TitleTextureKey(OpenNijiiroRW.Tx.Puchichara[i].metadata.tGetAuthor(), this.MenuFont, Color.White, Color.Black, 1000);
		}


		ttkCharacterAuthors = new TitleTextureKey[OpenNijiiroRW.Skin.Characters_Ptn];
		ttkCharacterNames = new TitleTextureKey[OpenNijiiroRW.Skin.Characters_Ptn];

		for (int i = 0; i < OpenNijiiroRW.Skin.Characters_Ptn; i++) {
			var textColor = HRarity.tRarityToColor(OpenNijiiroRW.Tx.Characters[i].metadata.Rarity);
			ttkCharacterNames[i] = new TitleTextureKey(OpenNijiiroRW.Tx.Characters[i].metadata.tGetName(), this.MenuFont, textColor, Color.Black, 1000);
			ttkCharacterAuthors[i] = new TitleTextureKey(OpenNijiiroRW.Tx.Characters[i].metadata.tGetAuthor(), this.MenuFont, Color.White, Color.Black, 1000);
		}


		base.Activate();
	}

	public override void DeActivate() {

		base.DeActivate();
	}

	public override void CreateManagedResource() {
		this.MenuFont = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.Heya_Font_Scale);
		base.CreateManagedResource();
	}

	public override void ReleaseManagedResource() {
		MenuFont.Dispose();

		base.ReleaseManagedResource();
	}

	public override int Draw() {
		if ((OpenNijiiroRW.Pad.bPressedDGB(EPad.Decide)) || ((OpenNijiiroRW.ConfigIni.bEnterIsNotUsedInKeyAssignments && OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Return)))) {
			switch (CurrentState) {
				case SelectableInfo.PlayerSelect: {
						switch (CurrentIndex) {
							case 0:
								Close();
								OpenNijiiroRW.Skin.soundCancelSFX.tPlay();
								break;
							default: {
									CurrentPlayer = OpenNijiiroRW.GetActualPlayer(CurrentIndex - 1);
									SetState(SelectableInfo.ModeSelect);
									OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
								}
								break;
						}
					}
					break;
				case SelectableInfo.ModeSelect: {
						switch (CurrentIndex) {
							case 0:
								SetState(SelectableInfo.PlayerSelect);
								OpenNijiiroRW.Skin.soundCancelSFX.tPlay();
								break;
							default: {
									SetState(SelectableInfo.Select);
									OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
								}
								break;
						}
					}
					break;
				case SelectableInfo.Select: {
						switch (CurrentMode) {
							case ModeType.PuchiChara: {
									var ess = this.tSelectPuchi();

									if (ess == ESelectStatus.SELECTED) {
										//PuchiChara.tGetPuchiCharaIndexByName(p);
										//TJAPlayer3.NamePlateConfig.data.PuchiChara[iPlayer] = TJAPlayer3.Skin.Puchicharas_Name[iPuchiCharaCurrent];// iPuchiCharaCurrent;
										//TJAPlayer3.NamePlateConfig.tApplyHeyaChanges();
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.PuchiChara = OpenNijiiroRW.Skin.Puchicharas_Name[CurrentIndex];// iPuchiCharaCurrent;
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].tApplyHeyaChanges();
										OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
										OpenNijiiroRW.Tx.Puchichara[CurrentIndex].welcome.tPlay();

										SetState(SelectableInfo.PlayerSelect);
									} else if (ess == ESelectStatus.SUCCESS) {
										//TJAPlayer3.NamePlateConfig.data.UnlockedPuchicharas[iPlayer].Add(TJAPlayer3.Skin.Puchicharas_Name[iPuchiCharaCurrent]);
										//TJAPlayer3.NamePlateConfig.tSpendCoins(TJAPlayer3.Tx.Puchichara[iPuchiCharaCurrent].unlock.Values[0], iPlayer);
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedPuchicharas.Add(OpenNijiiroRW.Skin.Puchicharas_Name[CurrentIndex]);
										if (OpenNijiiroRW.Tx.Puchichara[CurrentIndex].unlock is CUnlockCH)
											OpenNijiiroRW.SaveFileInstances[CurrentPlayer].tSpendCoins(OpenNijiiroRW.Tx.Puchichara[CurrentIndex].unlock.Values[0]);
										else if (OpenNijiiroRW.Tx.Puchichara[CurrentIndex].unlock is CUnlockAndComb || OpenNijiiroRW.Tx.Puchichara[CurrentIndex].unlock is CUnlockOrComb)
											OpenNijiiroRW.SaveFileInstances[CurrentPlayer].tSpendCoins(OpenNijiiroRW.Tx.Puchichara[CurrentIndex].unlock.CoinStack);
										OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
									} else {
										OpenNijiiroRW.Skin.soundError.tPlay();
									}
								}
								break;
							case ModeType.Chara: {
									var ess = this.tSelectChara();

									if (ess == ESelectStatus.SELECTED) {
										//TJAPlayer3.Tx.Loading?.t2D描画(18, 7);

										// Reload character, a bit time expensive but with a O(N) memory complexity instead of O(N * M)
										OpenNijiiroRW.Tx.ReloadCharacter(OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.Character, CurrentIndex, CurrentPlayer);
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.Character = CurrentIndex;

										// Update the character
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].tUpdateCharacterName(OpenNijiiroRW.Skin.Characters_DirName[CurrentIndex]);

										// Welcome voice using Sanka
										OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
										OpenNijiiroRW.Skin.voiceTitleSanka[CurrentPlayer]?.tPlay();

										CMenuCharacter.tMenuResetTimer(CMenuCharacter.ECharacterAnimation.NORMAL);

										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].tApplyHeyaChanges();

										SetState(SelectableInfo.PlayerSelect);
										CurrentMode = ModeType.None;
									} else if (ess == ESelectStatus.SUCCESS) {
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedCharacters.Add(OpenNijiiroRW.Skin.Characters_DirName[CurrentIndex]);
										if (OpenNijiiroRW.Tx.Characters[CurrentIndex].unlock is CUnlockCH)
											OpenNijiiroRW.SaveFileInstances[CurrentPlayer].tSpendCoins(OpenNijiiroRW.Tx.Characters[CurrentIndex].unlock.Values[0]);
										else if (OpenNijiiroRW.Tx.Characters[CurrentIndex].unlock is CUnlockAndComb || OpenNijiiroRW.Tx.Characters[CurrentIndex].unlock is CUnlockOrComb)
											OpenNijiiroRW.SaveFileInstances[CurrentPlayer].tSpendCoins(OpenNijiiroRW.Tx.Characters[CurrentIndex].unlock.CoinStack);
										OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
									} else {
										OpenNijiiroRW.Skin.soundError.tPlay();
									}
								}
								break;
							case ModeType.DanTitle: {
									bool iG = false;
									int cs = 0;

									if (CurrentIndex > 0) {
										iG = OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.DanTitles[this.ttkDanTitles[CurrentIndex].str].isGold;
										cs = OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.DanTitles[this.ttkDanTitles[CurrentIndex].str].clearStatus;
									}

									OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.Dan = this.ttkDanTitles[CurrentIndex].str;
									OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.DanGold = iG;
									OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.DanType = cs;

									OpenNijiiroRW.NamePlate.tNamePlateRefreshTitles(CurrentPlayer);

									OpenNijiiroRW.SaveFileInstances[CurrentPlayer].tApplyHeyaChanges();

									OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
									SetState(SelectableInfo.PlayerSelect);
								}
								break;
							case ModeType.SubTitle: {

									if (CurrentIndex == 0) {
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.TitleType = 0;
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.TitleId = -1;
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.TitleRarityInt = 1;
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.Title = "初心者";
									} else if (OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedNameplateIds != null &&
											   OpenNijiiroRW.Databases.DBNameplateUnlockables.data.ContainsKey(OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedNameplateIds[CurrentIndex - 1])) {
										var id = OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedNameplateIds[CurrentIndex - 1];
										var nameplate = OpenNijiiroRW.Databases.DBNameplateUnlockables.data[id];

										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.TitleId = id;
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.Title = nameplate.nameplateInfo.cld.GetString("");
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.TitleRarityInt = HRarity.tRarityToLangInt(nameplate.rarity);
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.TitleType = nameplate.nameplateInfo.iType;
									} else {
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.TitleType = -1;
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.TitleId = -1;
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.TitleRarityInt = 1;
										OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.Title = "";
									}

									OpenNijiiroRW.NamePlate.tNamePlateRefreshTitles(CurrentPlayer);

									OpenNijiiroRW.SaveFileInstances[CurrentPlayer].tApplyHeyaChanges();

									OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
									SetState(SelectableInfo.PlayerSelect);
								}
								break;
						}
					}
					break;
			}
		} else if ((OpenNijiiroRW.Pad.bPressedDGB(EPad.Cancel) || OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Escape))) {
			Close();
			OpenNijiiroRW.Skin.soundCancelSFX.tPlay();
		} else if (OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.LeftChange)
				   || OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.LeftArrow)) {
			ChangeIndex(-1);
			OpenNijiiroRW.Skin.soundChangeSFX.tPlay();
		} else if (OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.RightChange)
				   || OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.RightArrow)) {
			ChangeIndex(1);
			OpenNijiiroRW.Skin.soundChangeSFX.tPlay();
		}

		InFade.Tick();

		if (OpenNijiiroRW.Tx.Tile_Black != null) {
			OpenNijiiroRW.Tx.Tile_Black.Opacity = InFade.CurrentValue / 2;
			for (int i = 0; i <= (RenderSurfaceSize.Width / OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Width); i++)        // #23510 2010.10.31 yyagi: change "clientSize.Width" to "640" to fix FIFO drawing size
			{
				for (int j = 0; j <= (RenderSurfaceSize.Height / OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Height); j++)  // #23510 2010.10.31 yyagi: change "clientSize.Height" to "480" to fix FIFO drawing size
				{
					OpenNijiiroRW.Tx.Tile_Black.t2D描画(i * OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Width, j * OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Height);
				}
			}
		}


		switch (CurrentState) {
			case SelectableInfo.PlayerSelect:
				if (CurrentIndex == 0) {
					OpenNijiiroRW.Tx.NewHeya_Close_Select.t2D描画(OpenNijiiroRW.Skin.SongSelect_NewHeya_Close_Select[0], OpenNijiiroRW.Skin.SongSelect_NewHeya_Close_Select[1]);
				} else {
					OpenNijiiroRW.Tx.NewHeya_PlayerPlate_Select.t2D描画(OpenNijiiroRW.Skin.SongSelect_NewHeya_PlayerPlate_X[CurrentIndex - 1], OpenNijiiroRW.Skin.SongSelect_NewHeya_PlayerPlate_Y[CurrentIndex - 1]);
				}
				break;
			case SelectableInfo.ModeSelect: {
					OpenNijiiroRW.Tx.NewHeya_ModeBar_Select.t2D描画(OpenNijiiroRW.Skin.SongSelect_NewHeya_ModeBar_X[CurrentIndex], OpenNijiiroRW.Skin.SongSelect_NewHeya_ModeBar_Y[CurrentIndex]);
				}
				break;
			case SelectableInfo.Select: {
					switch (CurrentMode) {
						case ModeType.Chara:
							for (int i = 1; i < OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Count - 1; i++) {
								int x = OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_X[i];
								int y = OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Y[i];
								int index = i - (OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Count / 2) + CurrentIndex;
								while (index < 0) {
									index += CurrentMaxIndex;
								}
								while (index >= CurrentMaxIndex) {
									index -= CurrentMaxIndex;
								}
								OpenNijiiroRW.Tx.NewHeya_Box.t2D描画(x, y);


								float charaRatioX = 1.0f;
								float charaRatioY = 1.0f;
								if (OpenNijiiroRW.Skin.Characters_Resolution[index] != null) {
									charaRatioX = OpenNijiiroRW.Skin.Resolution[0] / (float)OpenNijiiroRW.Skin.Characters_Resolution[index][0];
									charaRatioY = OpenNijiiroRW.Skin.Resolution[1] / (float)OpenNijiiroRW.Skin.Characters_Resolution[index][1];
								}

								if (OpenNijiiroRW.Tx.Characters_Heya_Preview[index] != null) {
									OpenNijiiroRW.Tx.Characters_Heya_Preview[index].Scale.X = charaRatioX;
									OpenNijiiroRW.Tx.Characters_Heya_Preview[index].Scale.Y = charaRatioY;
								}

								OpenNijiiroRW.Tx.Characters_Heya_Preview[index]?.t2D拡大率考慮中央基準描画(x + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Chara_Offset[0], y + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Chara_Offset[1]);
								OpenNijiiroRW.Tx.Characters_Heya_Preview[index]?.tUpdateColor4(CConversion.ColorToColor4(Color.White));

								if (ttkCharacterNames[index] != null) {
									CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(ttkCharacterNames[index]);

									tmpTex.t2D拡大率考慮上中央基準描画(x + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Name_Offset[0], y + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Name_Offset[1]);
								}

								if (ttkCharacterAuthors[index] != null) {
									CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(ttkCharacterAuthors[index]);

									tmpTex.t2D拡大率考慮上中央基準描画(x + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Author_Offset[0], y + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Author_Offset[1]);
								}

								if (OpenNijiiroRW.Tx.Characters[index].unlock != null
									&& !OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedCharacters.Contains(OpenNijiiroRW.Skin.Characters_DirName[index])) {
									OpenNijiiroRW.Tx.NewHeya_Lock?.t2D描画(x + OpenNijiiroRW.Skin.SongSelect_NewHeya_Lock_Offset[0], y + OpenNijiiroRW.Skin.SongSelect_NewHeya_Lock_Offset[1]);

									if (this.ttkInfoSection != null)
										TitleTextureKey.ResolveTitleTexture(this.ttkInfoSection)
											.t2D拡大率考慮上中央基準描画(x + OpenNijiiroRW.Skin.SongSelect_NewHeya_InfoSection_Offset[0], y + OpenNijiiroRW.Skin.SongSelect_NewHeya_InfoSection_Offset[1]);
								}
							}
							break;
						case ModeType.PuchiChara:
							for (int i = 1; i < OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Count - 1; i++) {
								int x = OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_X[i];
								int y = OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Y[i];
								int index = i - (OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Count / 2) + CurrentIndex;
								while (index < 0) {
									index += CurrentMaxIndex;
								}
								while (index >= CurrentMaxIndex) {
									index -= CurrentMaxIndex;
								}
								OpenNijiiroRW.Tx.NewHeya_Box.t2D描画(x, y);

								OpenNijiiroRW.stageSongSelect.PuchiChara.DrawPuchichara(index,
									x + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Chara_Offset[0], y + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Chara_Offset[1],
									OpenNijiiroRW.Skin.Resolution[1] / 1080.0f, 255, true);

								OpenNijiiroRW.Tx.Puchichara[index].tx?.tUpdateColor4(CConversion.ColorToColor4(Color.White));


								if (ttkPuchiCharaNames[index] != null) {
									CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(ttkPuchiCharaNames[index]);

									tmpTex.t2D拡大率考慮上中央基準描画(x + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Name_Offset[0], y + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Name_Offset[1]);
								}

								if (ttkPuchiCharaAuthors[index] != null) {
									CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(ttkPuchiCharaAuthors[index]);

									tmpTex.t2D拡大率考慮上中央基準描画(x + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Author_Offset[0], y + OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Author_Offset[1]);
								}

								if (OpenNijiiroRW.Tx.Puchichara[index].unlock != null
									&& !OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedPuchicharas.Contains(OpenNijiiroRW.Skin.Puchicharas_Name[index])) {
									OpenNijiiroRW.Tx.NewHeya_Lock?.t2D描画(x + OpenNijiiroRW.Skin.SongSelect_NewHeya_Lock_Offset[0], y + OpenNijiiroRW.Skin.SongSelect_NewHeya_Lock_Offset[1]);

									if (this.ttkInfoSection != null)
										TitleTextureKey.ResolveTitleTexture(this.ttkInfoSection)
											.t2D拡大率考慮上中央基準描画(x + OpenNijiiroRW.Skin.SongSelect_NewHeya_InfoSection_Offset[0], y + OpenNijiiroRW.Skin.SongSelect_NewHeya_InfoSection_Offset[1]);
								}
							}
							break;
						case ModeType.SubTitle:
							for (int i = 1; i < OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Count - 1; i++) {
								int x = OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_X[i];
								int y = OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Y[i];
								int index = i - (OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Count / 2) + CurrentIndex;
								while (index < 0) {
									index += CurrentMaxIndex;
								}
								while (index >= CurrentMaxIndex) {
									index -= CurrentMaxIndex;
								}
								CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(this.ttkTitles[index]);

								if (i != 0) {
									tmpTex.color4 = CConversion.ColorToColor4(Color.DarkGray);
								} else {
									tmpTex.color4 = CConversion.ColorToColor4(Color.White);
								}

								OpenNijiiroRW.Tx.NewHeya_Box.t2D描画(x, y);

								x += OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Chara_Offset[0];
								y += OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Chara_Offset[1];

								int iType = -1;
								int rarity = 1;
								int id = -1;

								if (index == 0) {
									iType = 0;
								} else if (OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedNameplateIds != null &&
										   OpenNijiiroRW.Databases.DBNameplateUnlockables.data.ContainsKey(OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedNameplateIds[index - 1])) {
									id = OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedNameplateIds[index - 1];
									var nameplate = OpenNijiiroRW.Databases.DBNameplateUnlockables.data[id];
									iType = nameplate.nameplateInfo.iType;
									rarity = HRarity.tRarityToLangInt(nameplate.rarity);
								}



								tmpTex.t2D拡大率考慮上中央基準描画(x + OpenNijiiroRW.Skin.Heya_Side_Menu_Font_Offset[0], y + OpenNijiiroRW.Skin.Heya_Side_Menu_Font_Offset[1]);

								OpenNijiiroRW.NamePlate.lcNamePlate.DrawTitlePlate(x, y, 255, iType, tmpTex, rarity, id);

							}
							break;
						case ModeType.DanTitle:
							for (int i = 1; i < OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Count - 1; i++) {
								int x = OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_X[i];
								int y = OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Y[i];
								int index = i - (OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Count / 2) + CurrentIndex;
								while (index < 0) {
									index += CurrentMaxIndex;
								}
								while (index >= CurrentMaxIndex) {
									index -= CurrentMaxIndex;
								}
								CTexture tmpTex = TitleTextureKey.ResolveTitleTexture(this.ttkDanTitles[index]);

								if (i != 0) {
									tmpTex.color4 = CConversion.ColorToColor4(Color.DarkGray);
								} else {
									tmpTex.color4 = CConversion.ColorToColor4(Color.White);
								}

								OpenNijiiroRW.Tx.NewHeya_Box.t2D描画(x, y);

								x += OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Chara_Offset[0];
								y += OpenNijiiroRW.Skin.SongSelect_NewHeya_Box_Chara_Offset[1];

								int danGrade = 0;
								if (index > 0) {
									danGrade = OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.DanTitles[this.ttkDanTitles[index].str].clearStatus;
								}

								OpenNijiiroRW.NamePlate.lcNamePlate.DrawDan(x, y, 255, danGrade, tmpTex);

								/*
								TJAPlayer3.NamePlate.tNamePlateDisplayNamePlateBase(
									x - TJAPlayer3.Tx.NamePlateBase.szTextureSize.Width / 2,
									y - TJAPlayer3.Tx.NamePlateBase.szTextureSize.Height / 24,
									(8 + danGrade));
								TJAPlayer3.Tx.NamePlateBase.color4 = CConversion.ColorToColor4(Color.White);

								tmpTex.t2D拡大率考慮上中央基準描画(x + TJAPlayer3.Skin.Heya_Side_Menu_Font_Offset[0], y + TJAPlayer3.Skin.Heya_Side_Menu_Font_Offset[1]);
								*/
							}
							break;
					}
				}
				break;
		}

		OpenNijiiroRW.Tx.NewHeya_Close.t2D描画(0, 0);

		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++) {
			OpenNijiiroRW.Tx.NewHeya_PlayerPlate[OpenNijiiroRW.GetActualPlayer(i)].t2D描画(OpenNijiiroRW.Skin.SongSelect_NewHeya_PlayerPlate_X[i], OpenNijiiroRW.Skin.SongSelect_NewHeya_PlayerPlate_Y[i]);
		}

		for (int i = 0; i < 5; i++) {
			OpenNijiiroRW.Tx.NewHeya_ModeBar.t2D描画(OpenNijiiroRW.Skin.SongSelect_NewHeya_ModeBar_X[i], OpenNijiiroRW.Skin.SongSelect_NewHeya_ModeBar_Y[i]);
			int title_x = OpenNijiiroRW.Skin.SongSelect_NewHeya_ModeBar_X[i] + OpenNijiiroRW.Skin.SongSelect_NewHeya_ModeBar_Font_Offset[0];
			int title_y = OpenNijiiroRW.Skin.SongSelect_NewHeya_ModeBar_Y[i] + OpenNijiiroRW.Skin.SongSelect_NewHeya_ModeBar_Font_Offset[1];
			TitleTextureKey.ResolveTitleTexture(MenuTitleKeys[i], false).t2D拡大率考慮中央基準描画(title_x, title_y);
		}

		return base.Draw();
	}

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

	private ESelectStatus tSelectPuchi() {
		// Add "If unlocked" to select directly

		if (OpenNijiiroRW.Tx.Puchichara[CurrentIndex].unlock != null
			&& !OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedPuchicharas.Contains(OpenNijiiroRW.Skin.Puchicharas_Name[CurrentIndex])) {
			(bool, string?) response = OpenNijiiroRW.Tx.Puchichara[CurrentIndex].unlock.tConditionMet(CurrentPlayer);
			//tConditionMet(
			//new int[] { TJAPlayer3.SaveFileInstances[TJAPlayer3.SaveFile].data.Medals });

			Color responseColor = (response.Item1) ? Color.Lime : Color.Red;

			// Send coins here for the unlock, considering that only coin-paid puchicharas can be unlocked directly from the Heya menu

			this.ttkInfoSection = new TitleTextureKey(response.Item2 ?? this.ttkInfoSection.str, this.MenuFont, responseColor, Color.Black, 1000);

			return (response.Item1) ? ESelectStatus.SUCCESS : ESelectStatus.FAILED;
		}

		this.ttkInfoSection = null;
		return ESelectStatus.SELECTED;
	}

	private void tUpdateUnlockableTextPuchi() {
		#region [Check unlockable]

		if (OpenNijiiroRW.Tx.Puchichara[CurrentIndex].unlock != null
			&& !OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedPuchicharas.Contains(OpenNijiiroRW.Skin.Puchicharas_Name[CurrentIndex])) {
			this.ttkInfoSection = new TitleTextureKey(OpenNijiiroRW.Tx.Puchichara[CurrentIndex].unlock.tConditionMessage()
				, this.MenuFont, Color.White, Color.Black, 1000);
		} else
			this.ttkInfoSection = null;

		#endregion
	}
	private void tUpdateUnlockableTextChara() {
		#region [Check unlockable]

		if (OpenNijiiroRW.Tx.Characters[CurrentIndex].unlock != null
			&& !OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedCharacters.Contains(OpenNijiiroRW.Skin.Characters_DirName[CurrentIndex])) {
			this.ttkInfoSection = new TitleTextureKey(OpenNijiiroRW.Tx.Characters[CurrentIndex].unlock.tConditionMessage()
				, this.MenuFont, Color.White, Color.Black, 1000);
		} else
			this.ttkInfoSection = null;

		#endregion
	}

	private ESelectStatus tSelectChara() {
		// Add "If unlocked" to select directly

		if (OpenNijiiroRW.Tx.Characters[CurrentIndex].unlock != null
			&& !OpenNijiiroRW.SaveFileInstances[CurrentPlayer].data.UnlockedCharacters.Contains(OpenNijiiroRW.Skin.Characters_DirName[CurrentIndex])) {
			(bool, string?) response = OpenNijiiroRW.Tx.Characters[CurrentIndex].unlock.tConditionMet(CurrentPlayer);
			//TJAPlayer3.Tx.Characters[iCharacterCurrent].unlock.tConditionMet(
			//new int[] { TJAPlayer3.SaveFileInstances[TJAPlayer3.SaveFile].data.Medals });

			Color responseColor = (response.Item1) ? Color.Lime : Color.Red;

			// Send coins here for the unlock, considering that only coin-paid puchicharas can be unlocked directly from the Heya menu

			this.ttkInfoSection = new TitleTextureKey(response.Item2 ?? this.ttkInfoSection.str, this.MenuFont, responseColor, Color.Black, 1000);

			return (response.Item1) ? ESelectStatus.SUCCESS : ESelectStatus.FAILED;
		}

		this.ttkInfoSection = null;
		return ESelectStatus.SELECTED;
	}
}
