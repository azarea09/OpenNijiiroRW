using System.Diagnostics;
using FDK;

namespace OpenNijiiroRW;

internal class CStage曲読み込み : CStage
{
	// コンストラクタ

	public CStage曲読み込み()
	{
		base.eStageID = CStage.EStage.SongLoading;
		base.ePhaseID = CStage.EPhase.Common_NORMAL;
		base.IsDeActivated = true;
		//base.list子Activities.Add( this.actFI = new CActFIFOBlack() );	// #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
		//base.list子Activities.Add( this.actFO = new CActFIFOBlack() );
	}


	// CStage 実装

	public override void Activate()
	{
		Trace.TraceInformation("曲読み込みステージを活性化します。");
		Trace.Indent();
		try
		{
			this.str曲タイトル = "";
			this.strSTAGEFILE = "";
			this.nBGM再生開始時刻 = -1;
			this.nBGMの総再生時間ms = 0;
			if (this.sd読み込み音 != null)
			{
				OpenNijiiroRW.SoundManager.tDisposeSound(this.sd読み込み音);
				this.sd読み込み音 = null;
			}

			if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] >= 5 || OpenNijiiroRW.ConfigIni.nPlayerCount != 1)
			{
				OpenNijiiroRW.ConfigIni.bTokkunMode = false;
			}

			string strDTXファイルパス = OpenNijiiroRW.stageSongSelect.r確定されたスコア.ファイル情報.ファイルの絶対パス;

			var strフォルダ名 = Path.GetDirectoryName(strDTXファイルパス) + Path.DirectorySeparatorChar;

			var 譜面情報 = OpenNijiiroRW.stageSongSelect.r確定されたスコア.譜面情報;
			this.str曲タイトル = 譜面情報.タイトル;
			this.strサブタイトル = 譜面情報.strサブタイトル;

			this.strSTAGEFILE = CSkin.Path(@$"Graphics{Path.DirectorySeparatorChar}4_SongLoading{Path.DirectorySeparatorChar}Background.png");


			float wait = 600f;
			if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan)
				wait = 1000f;
			else if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Tower)
				wait = 1200f;

			this.ct待機 = new CCounter(0, wait, 5, OpenNijiiroRW.Timer);
			this.ct曲名表示 = new CCounter(1, 30, 30, OpenNijiiroRW.Timer);
			try
			{
				// When performing calibration, inform the player that
				// calibration is about to begin, rather than
				// displaying the song title and subtitle as usual.

				var タイトル = this.str曲タイトル;

				var サブタイトル = this.strサブタイトル;

				if (!string.IsNullOrEmpty(タイトル))
				{
					//this.txタイトル = new CTexture( CDTXMania.app.Device, image, CDTXMania.TextureFormat );
					//this.txタイトル.vc拡大縮小倍率 = new Vector3( 0.5f, 0.5f, 1f );


					using (var bmpSongTitle = this.pfTITLE.DrawText(タイトル, OpenNijiiroRW.Skin.SongLoading_Title_ForeColor, OpenNijiiroRW.Skin.SongLoading_Title_BackColor, null, 30))
					{
						this.txタイトル = new CTexture(bmpSongTitle);
						txタイトル.Scale.X = OpenNijiiroRW.GetSongNameXScaling(ref txタイトル, OpenNijiiroRW.Skin.SongLoading_Title_MaxSize);
					}

					using (var bmpSongSubTitle = this.pfSUBTITLE.DrawText(サブタイトル, OpenNijiiroRW.Skin.SongLoading_SubTitle_ForeColor, OpenNijiiroRW.Skin.SongLoading_SubTitle_BackColor, null, 30))
					{
						this.txサブタイトル = new CTexture(bmpSongSubTitle);
						txサブタイトル.Scale.X = OpenNijiiroRW.GetSongNameXScaling(ref txサブタイトル, OpenNijiiroRW.Skin.SongLoading_SubTitle_MaxSize);
					}
				}
				else
				{
					this.txタイトル = null;
					this.txサブタイトル = null;
				}

			}
			catch (CTextureCreateFailedException e)
			{
				Trace.TraceError(e.ToString());
				Trace.TraceError("テクスチャの生成に失敗しました。({0})", new object[] { this.strSTAGEFILE });
				this.txタイトル = null;
				this.txサブタイトル = null;
			}

			base.Activate();
		}
		finally
		{
			Trace.TraceInformation("曲読み込みステージの活性化を完了しました。");
			Trace.Unindent();
		}
	}
	public override void DeActivate()
	{
		Trace.TraceInformation("曲読み込みステージを非活性化します。");
		Trace.Indent();
		try
		{
			OpenNijiiroRW.tテクスチャの解放(ref this.txタイトル);
			//CDTXMania.tテクスチャの解放( ref this.txSongnamePlate );
			OpenNijiiroRW.tテクスチャの解放(ref this.txサブタイトル);
			base.DeActivate();
		}
		finally
		{
			Trace.TraceInformation("曲読み込みステージの非活性化を完了しました。");
			Trace.Unindent();
		}
	}
	public override void CreateManagedResource()
	{
		this.pfTITLE = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.SongLoading_Title_FontSize);
		this.pfSUBTITLE = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.SongLoading_SubTitle_FontSize);
		pfDanTitle = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.Game_DanC_Title_Size);
		pfDanSubTitle = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.Game_DanC_SubTitle_Size);

		//this.txSongnamePlate = CDTXMania.tテクスチャの生成( CSkin.Path( @$"Graphics{Path.DirectorySeparatorChar}6_SongnamePlate.png" ) );
		base.CreateManagedResource();
	}
	public override void ReleaseManagedResource()
	{
		OpenNijiiroRW.tDisposeSafely(ref this.pfTITLE);
		OpenNijiiroRW.tDisposeSafely(ref this.pfSUBTITLE);

		pfDanTitle?.Dispose();
		pfDanSubTitle?.Dispose();

		base.ReleaseManagedResource();
	}
	public override int Draw()
	{
		string str;

		if (base.IsDeActivated)
			return 0;

		#region [ 初めての進行描画 ]
		//-----------------------------
		if (base.IsFirstDraw)
		{
			CScore cスコア1 = OpenNijiiroRW.stageSongSelect.r確定されたスコア;
			if (this.sd読み込み音 != null)
			{
				if (OpenNijiiroRW.Skin.sound曲読込開始音.bExclusive && (CSkin.CSystemSound.r最後に再生した排他システムサウンド != null))
				{
					CSkin.CSystemSound.r最後に再生した排他システムサウンド.tStop();
				}
				this.sd読み込み音.PlayStart();
				this.nBGM再生開始時刻 = SoundManager.PlayTimer.NowTimeMs;
				this.nBGMの総再生時間ms = this.sd読み込み音.TotalPlayTime;
			}
			else
			{
				OpenNijiiroRW.Skin.sound曲読込開始音.tPlay();
				this.nBGM再生開始時刻 = SoundManager.PlayTimer.NowTimeMs;
				this.nBGMの総再生時間ms = OpenNijiiroRW.Skin.sound曲読込開始音.n長さ_現在のサウンド;
			}
			//this.actFI.tフェードイン開始();							// #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
			base.ePhaseID = CStage.EPhase.Common_FADEIN;
			base.IsFirstDraw = false;

			nWAVcount = 1;
		}
		//-----------------------------
		#endregion
		this.ct待機.Tick();

		#region [ Cancel loading with esc ]
		if (tキー入力())
		{
			if (this.sd読み込み音 != null)
			{
				this.sd読み込み音.tStopSound();
				this.sd読み込み音.tDispose();
			}
			return (int)ESongLoadingScreenReturnValue.LoadCanceled;
		}
		#endregion

		if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Dan)
		{
			void drawPlate()
			{
				if (OpenNijiiroRW.Tx.SongLoading_Plate != null)
				{
					OpenNijiiroRW.Tx.SongLoading_Plate.bスクリーン合成 = OpenNijiiroRW.Skin.SongLoading_Plate_ScreenBlend; //あまりにも出番が無い
					OpenNijiiroRW.Tx.SongLoading_Plate.Opacity = 255;
					if (OpenNijiiroRW.Skin.SongLoading_Plate_ReferencePoint == CSkin.ReferencePoint.Left)
					{
						OpenNijiiroRW.Tx.SongLoading_Plate.t2D描画(OpenNijiiroRW.Skin.SongLoading_Plate_X, OpenNijiiroRW.Skin.SongLoading_Plate_Y - (OpenNijiiroRW.Tx.SongLoading_Plate.sz画像サイズ.Height / 2));
					}
					else if (OpenNijiiroRW.Skin.SongLoading_Plate_ReferencePoint == CSkin.ReferencePoint.Right)
					{
						OpenNijiiroRW.Tx.SongLoading_Plate.t2D描画(OpenNijiiroRW.Skin.SongLoading_Plate_X - OpenNijiiroRW.Tx.SongLoading_Plate.sz画像サイズ.Width, OpenNijiiroRW.Skin.SongLoading_Plate_Y - (OpenNijiiroRW.Tx.SongLoading_Plate.sz画像サイズ.Height / 2));
					}
					else
					{
						OpenNijiiroRW.Tx.SongLoading_Plate.t2D描画(OpenNijiiroRW.Skin.SongLoading_Plate_X - (OpenNijiiroRW.Tx.SongLoading_Plate.sz画像サイズ.Width / 2), OpenNijiiroRW.Skin.SongLoading_Plate_Y - (OpenNijiiroRW.Tx.SongLoading_Plate.sz画像サイズ.Height / 2));
					}
				}
				//CDTXMania.act文字コンソール.tPrint( 0, 16, C文字コンソール.Eフォント種別.灰, C変換.nParsentTo255( ( this.ct曲名表示.n現在の値 / 30.0 ) ).ToString() );


				int y = 720 - 45;
				if (this.txタイトル != null)
				{
					int nサブタイトル補正 = string.IsNullOrEmpty(OpenNijiiroRW.stageSongSelect.r確定されたスコア.譜面情報.strサブタイトル) ? 15 : 0;

					this.txタイトル.Opacity = 255;
					if (OpenNijiiroRW.Skin.SongLoading_Title_ReferencePoint == CSkin.ReferencePoint.Left)
					{
						this.txタイトル.t2D描画(OpenNijiiroRW.Skin.SongLoading_Title_X, OpenNijiiroRW.Skin.SongLoading_Title_Y - (this.txタイトル.sz画像サイズ.Height / 2) + nサブタイトル補正);
					}
					else if (OpenNijiiroRW.Skin.SongLoading_Title_ReferencePoint == CSkin.ReferencePoint.Right)
					{
						this.txタイトル.t2D描画(OpenNijiiroRW.Skin.SongLoading_Title_X - (this.txタイトル.sz画像サイズ.Width * txタイトル.Scale.X), OpenNijiiroRW.Skin.SongLoading_Title_Y - (this.txタイトル.sz画像サイズ.Height / 2) + nサブタイトル補正);
					}
					else
					{
						this.txタイトル.t2D描画((OpenNijiiroRW.Skin.SongLoading_Title_X - ((this.txタイトル.sz画像サイズ.Width * txタイトル.Scale.X) / 2)), OpenNijiiroRW.Skin.SongLoading_Title_Y - (this.txタイトル.sz画像サイズ.Height / 2) + nサブタイトル補正);
					}
				}
				if (this.txサブタイトル != null)
				{
					this.txサブタイトル.Opacity = 255;
					if (OpenNijiiroRW.Skin.SongLoading_SubTitle_ReferencePoint == CSkin.ReferencePoint.Left)
					{
						this.txサブタイトル.t2D描画(OpenNijiiroRW.Skin.SongLoading_SubTitle_X, OpenNijiiroRW.Skin.SongLoading_SubTitle_Y - (this.txサブタイトル.sz画像サイズ.Height / 2));
					}
					else if (OpenNijiiroRW.Skin.SongLoading_Title_ReferencePoint == CSkin.ReferencePoint.Right)
					{
						this.txサブタイトル.t2D描画(OpenNijiiroRW.Skin.SongLoading_SubTitle_X - (this.txサブタイトル.sz画像サイズ.Width * txタイトル.Scale.X), OpenNijiiroRW.Skin.SongLoading_SubTitle_Y - (this.txサブタイトル.sz画像サイズ.Height / 2));
					}
					else
					{
						this.txサブタイトル.t2D描画((OpenNijiiroRW.Skin.SongLoading_SubTitle_X - ((this.txサブタイトル.sz画像サイズ.Width * txサブタイトル.Scale.X) / 2)), OpenNijiiroRW.Skin.SongLoading_SubTitle_Y - (this.txサブタイトル.sz画像サイズ.Height / 2));
					}
				}
			}

			void drawPlate_AI()
			{
				if (OpenNijiiroRW.Tx.SongLoading_Plate_AI != null)
				{
					OpenNijiiroRW.Tx.SongLoading_Plate_AI.bスクリーン合成 = OpenNijiiroRW.Skin.SongLoading_Plate_ScreenBlend; //あまりにも出番が無い
					OpenNijiiroRW.Tx.SongLoading_Plate_AI.Opacity = 255;
					if (OpenNijiiroRW.Skin.SongLoading_Plate_ReferencePoint == CSkin.ReferencePoint.Left)
					{
						OpenNijiiroRW.Tx.SongLoading_Plate_AI.t2D描画(OpenNijiiroRW.Skin.SongLoading_Plate_X_AI, OpenNijiiroRW.Skin.SongLoading_Plate_Y_AI - (OpenNijiiroRW.Tx.SongLoading_Plate_AI.sz画像サイズ.Height / 2));
					}
					else if (OpenNijiiroRW.Skin.SongLoading_Plate_ReferencePoint == CSkin.ReferencePoint.Right)
					{
						OpenNijiiroRW.Tx.SongLoading_Plate_AI.t2D描画(OpenNijiiroRW.Skin.SongLoading_Plate_X_AI - OpenNijiiroRW.Tx.SongLoading_Plate_AI.sz画像サイズ.Width, OpenNijiiroRW.Skin.SongLoading_Plate_Y_AI - (OpenNijiiroRW.Tx.SongLoading_Plate_AI.sz画像サイズ.Height / 2));
					}
					else
					{
						OpenNijiiroRW.Tx.SongLoading_Plate_AI.t2D描画(OpenNijiiroRW.Skin.SongLoading_Plate_X_AI - (OpenNijiiroRW.Tx.SongLoading_Plate_AI.sz画像サイズ.Width / 2), OpenNijiiroRW.Skin.SongLoading_Plate_Y_AI - (OpenNijiiroRW.Tx.SongLoading_Plate_AI.sz画像サイズ.Height / 2));
					}
				}
				//CDTXMania.act文字コンソール.tPrint( 0, 16, C文字コンソール.Eフォント種別.灰, C変換.nParsentTo255( ( this.ct曲名表示.n現在の値 / 30.0 ) ).ToString() );


				int y = 720 - 45;
				if (this.txタイトル != null)
				{
					int nサブタイトル補正 = string.IsNullOrEmpty(OpenNijiiroRW.stageSongSelect.r確定されたスコア.譜面情報.strサブタイトル) ? 15 : 0;

					this.txタイトル.Opacity = 255;
					if (OpenNijiiroRW.Skin.SongLoading_Title_ReferencePoint == CSkin.ReferencePoint.Left)
					{
						this.txタイトル.t2D描画(OpenNijiiroRW.Skin.SongLoading_Title_X_AI, OpenNijiiroRW.Skin.SongLoading_Title_Y_AI - (this.txタイトル.sz画像サイズ.Height / 2) + nサブタイトル補正);
					}
					else if (OpenNijiiroRW.Skin.SongLoading_Title_ReferencePoint == CSkin.ReferencePoint.Right)
					{
						this.txタイトル.t2D描画(OpenNijiiroRW.Skin.SongLoading_Title_X_AI - (this.txタイトル.sz画像サイズ.Width * txタイトル.Scale.X), OpenNijiiroRW.Skin.SongLoading_Title_Y_AI - (this.txタイトル.sz画像サイズ.Height / 2) + nサブタイトル補正);
					}
					else
					{
						this.txタイトル.t2D描画((OpenNijiiroRW.Skin.SongLoading_Title_X_AI - ((this.txタイトル.sz画像サイズ.Width * txタイトル.Scale.X) / 2)), OpenNijiiroRW.Skin.SongLoading_Title_Y_AI - (this.txタイトル.sz画像サイズ.Height / 2) + nサブタイトル補正);
					}
				}
				if (this.txサブタイトル != null)
				{
					this.txサブタイトル.Opacity = 255;
					if (OpenNijiiroRW.Skin.SongLoading_SubTitle_ReferencePoint == CSkin.ReferencePoint.Left)
					{
						this.txサブタイトル.t2D描画(OpenNijiiroRW.Skin.SongLoading_SubTitle_X_AI, OpenNijiiroRW.Skin.SongLoading_SubTitle_Y_AI - (this.txサブタイトル.sz画像サイズ.Height / 2));
					}
					else if (OpenNijiiroRW.Skin.SongLoading_Title_ReferencePoint == CSkin.ReferencePoint.Right)
					{
						this.txサブタイトル.t2D描画(OpenNijiiroRW.Skin.SongLoading_SubTitle_X_AI - (this.txサブタイトル.sz画像サイズ.Width * txタイトル.Scale.X), OpenNijiiroRW.Skin.SongLoading_SubTitle_Y_AI - (this.txサブタイトル.sz画像サイズ.Height / 2));
					}
					else
					{
						this.txサブタイトル.t2D描画((OpenNijiiroRW.Skin.SongLoading_SubTitle_X_AI - ((this.txサブタイトル.sz画像サイズ.Width * txサブタイトル.Scale.X) / 2)), OpenNijiiroRW.Skin.SongLoading_SubTitle_Y_AI - (this.txサブタイトル.sz画像サイズ.Height / 2));
					}
				}
			}

			#region [ Loading screen (except dan) ]
			//-----------------------------
			this.ct曲名表示.Tick();

			if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Tower)
			{
				#region [Tower loading screen]

				if (OpenNijiiroRW.Skin.Game_Tower_Ptn_Result > 0)
				{
					int xFactor = 0;
					float yFactor = 1f;

					int currentTowerType = Array.IndexOf(OpenNijiiroRW.Skin.Game_Tower_Names, OpenNijiiroRW.stageSongSelect.rChoosenSong.score[5].譜面情報.nTowerType);

					if (currentTowerType < 0 || currentTowerType >= OpenNijiiroRW.Skin.Game_Tower_Ptn)
						currentTowerType = 0;

					if (OpenNijiiroRW.Tx.TowerResult_Background != null && currentTowerType < OpenNijiiroRW.Tx.TowerResult_Tower.Length && OpenNijiiroRW.Tx.TowerResult_Tower[currentTowerType] != null)
					{
						xFactor = (OpenNijiiroRW.Tx.TowerResult_Background.szTextureSize.Width - OpenNijiiroRW.Tx.TowerResult_Tower[currentTowerType].szTextureSize.Width) / 2;
						yFactor = OpenNijiiroRW.Tx.TowerResult_Tower[currentTowerType].szTextureSize.Height / (float)OpenNijiiroRW.Tx.TowerResult_Background.szTextureSize.Height;
					}

					float pos = (OpenNijiiroRW.Tx.TowerResult_Background.szTextureSize.Height - OpenNijiiroRW.Skin.Resolution[1]) -
								((ct待機.CurrentValue <= 1200 ? ct待機.CurrentValue / 10f : 120) / 120f * (OpenNijiiroRW.Tx.TowerResult_Background.szTextureSize.Height - OpenNijiiroRW.Skin.Resolution[1]));

					OpenNijiiroRW.Tx.TowerResult_Background?.t2D描画(0, -1 * pos);

					if (currentTowerType < OpenNijiiroRW.Tx.TowerResult_Tower.Length)
						OpenNijiiroRW.Tx.TowerResult_Tower[currentTowerType]?.t2D描画(xFactor, -1 * yFactor * pos);
				}

				#endregion
				drawPlate();
			}
			else if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
			{
				OpenNijiiroRW.ConfigIni.tInitializeAILevel();
				OpenNijiiroRW.Tx.SongLoading_Bg_AI_Wait.t2D描画(0, 0);
				drawPlate_AI();
			}
			else
			{
				#region [Ensou loading screen]

				if (OpenNijiiroRW.Tx.SongLoading_BgWait != null) OpenNijiiroRW.Tx.SongLoading_BgWait.t2D描画(0, 0);
				if (OpenNijiiroRW.Tx.SongLoading_Chara != null) OpenNijiiroRW.Tx.SongLoading_Chara.t2D描画(0, 0);

				drawPlate();

				#endregion
			}

			//CDTXMania.act文字コンソール.tPrint( 0, 0, C文字コンソール.Eフォント種別.灰, this.ct曲名表示.n現在の値.ToString() );

			//-----------------------------
			#endregion
		}
		else
		{
			#region [ Dan Loading screen　]

			OpenNijiiroRW.Tx.SongLoading_Bg_Dan.t2D描画(0, 0 - (ct待機.CurrentValue <= 600 ? ct待機.CurrentValue / 10f : 60));

			CTexture dp = (OpenNijiiroRW.stageDanSongSelect.段位リスト.stバー情報 != null)
				? OpenNijiiroRW.stageDanSongSelect.段位リスト.stバー情報[OpenNijiiroRW.stageDanSongSelect.段位リスト.n現在の選択行].txDanPlate
				: null;

			CActSelect段位リスト.tDisplayDanPlate(dp,
				null,
				OpenNijiiroRW.Skin.SongLoading_DanPlate[0],
				OpenNijiiroRW.Skin.SongLoading_DanPlate[1]);

			if (OpenNijiiroRW.Tx.Tile_Black != null)
			{
				OpenNijiiroRW.Tx.Tile_Black.Opacity = (int)(ct待機.CurrentValue <= 51 ? (255 - ct待機.CurrentValue / 0.2f) : (this.ct待機.CurrentValue - 949) / 0.2);
				for (int i = 0; i <= (RenderSurfaceSize.Width / OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Width); i++)      // #23510 2010.10.31 yyagi: change "clientSize.Width" to "640" to fix FIFO drawing size
				{
					for (int j = 0; j <= (RenderSurfaceSize.Height / OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Height); j++) // #23510 2010.10.31 yyagi: change "clientSize.Height" to "480" to fix FIFO drawing size
					{
						OpenNijiiroRW.Tx.Tile_Black.t2D描画(i * OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Width, j * OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Height);
					}
				}
			}

			#endregion
		}

		switch (base.ePhaseID)
		{
			case CStage.EPhase.Common_FADEIN:
				//if( this.actFI.On進行描画() != 0 )			    // #27787 2012.3.10 yyagi 曲読み込み画面のフェードインの省略
				// 必ず一度「CStaeg.Eフェーズ.共通_フェードイン」フェーズを経由させること。
				// さもないと、曲読み込みが完了するまで、曲読み込み画面が描画されない。
				base.ePhaseID = CStage.EPhase.SongLoading_LoadDTXFile;
				return (int)ESongLoadingScreenReturnValue.Continue;

			case CStage.EPhase.SongLoading_LoadDTXFile:
				{
					timeBeginLoad = DateTime.Now;
					TimeSpan span;
					str = OpenNijiiroRW.stageSongSelect.r確定されたスコア.ファイル情報.ファイルの絶対パス;

					if ((OpenNijiiroRW.TJA != null) && OpenNijiiroRW.TJA.IsActivated)
						OpenNijiiroRW.TJA.DeActivate();

					//if( CDTXMania.DTX == null )
					{
						for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; ++i)
							OpenNijiiroRW.SetTJA(i, new CTja(str, false, 0, i, true, OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[i]));

						Trace.TraceInformation("---- Song information -----------------");
						Trace.TraceInformation("TITLE: {0}", OpenNijiiroRW.TJA.TITLE.GetString(""));
						Trace.TraceInformation("FILE: {0}", OpenNijiiroRW.TJA.strFullPath);
						Trace.TraceInformation("---------------------------");

						span = (TimeSpan)(DateTime.Now - timeBeginLoad);
						Trace.TraceInformation("Chart loading time:           {0}", span.ToString());

						// 段位認定モード用。
						#region [dan setup]
						if (OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] == (int)Difficulty.Dan && OpenNijiiroRW.TJA.List_DanSongs != null)
						{

							var titleForeColor = OpenNijiiroRW.Skin.Game_DanC_Title_ForeColor;
							var titleBackColor = OpenNijiiroRW.Skin.Game_DanC_Title_BackColor;
							var subtitleForeColor = OpenNijiiroRW.Skin.Game_DanC_SubTitle_ForeColor;
							var subtitleBackColor = OpenNijiiroRW.Skin.Game_DanC_SubTitle_BackColor;

							for (int i = 0; i < OpenNijiiroRW.TJA.List_DanSongs.Count; i++)
							{
								if (!string.IsNullOrEmpty(OpenNijiiroRW.TJA.List_DanSongs[i].Title))
								{
									using (var bmpSongTitle = pfDanTitle.DrawText(OpenNijiiroRW.TJA.List_DanSongs[i].Title, titleForeColor, titleBackColor, null, 30))
									{
										OpenNijiiroRW.TJA.List_DanSongs[i].TitleTex = OpenNijiiroRW.tテクスチャの生成(bmpSongTitle, false);
										OpenNijiiroRW.TJA.List_DanSongs[i].TitleTex.Scale.X = OpenNijiiroRW.GetSongNameXScaling(ref OpenNijiiroRW.TJA.List_DanSongs[i].TitleTex, OpenNijiiroRW.Skin.Game_DanC_Title_MaxWidth);
									}
								}

								if (!string.IsNullOrEmpty(OpenNijiiroRW.TJA.List_DanSongs[i].SubTitle))
								{
									using (var bmpSongSubTitle = pfDanSubTitle.DrawText(OpenNijiiroRW.TJA.List_DanSongs[i].SubTitle, subtitleForeColor, subtitleBackColor, null, 30))
									{
										OpenNijiiroRW.TJA.List_DanSongs[i].SubTitleTex = OpenNijiiroRW.tテクスチャの生成(bmpSongSubTitle, false);
										OpenNijiiroRW.TJA.List_DanSongs[i].SubTitleTex.Scale.X = OpenNijiiroRW.GetSongNameXScaling(ref OpenNijiiroRW.TJA.List_DanSongs[i].SubTitleTex, OpenNijiiroRW.Skin.Game_DanC_SubTitle_MaxWidth);
									}
								}

							}
						}
						#endregion
					}

					base.ePhaseID = CStage.EPhase.SongLoading_WaitToLoadWAVFile;
					timeBeginLoadWAV = DateTime.Now;
					return (int)ESongLoadingScreenReturnValue.Continue;
				}

			case CStage.EPhase.SongLoading_WaitToLoadWAVFile:
				{
					if (this.ct待機.CurrentValue > 260)
					{
						base.ePhaseID = CStage.EPhase.SongLoading_LoadWAVFile;
					}
					return (int)ESongLoadingScreenReturnValue.Continue;
				}

			case CStage.EPhase.SongLoading_LoadWAVFile:
				{
					int looptime = (OpenNijiiroRW.ConfigIni.bEnableVSync) ? 3 : 1; // VSyncWait=ON時は1frame(1/60s)あたり3つ読むようにする
					for (int i = 0; i < looptime && nWAVcount <= OpenNijiiroRW.TJA.listWAV.Count; i++)
					{
						if (OpenNijiiroRW.TJA.listWAV[nWAVcount].listこのWAVを使用するチャンネル番号の集合.Count > 0)   // #28674 2012.5.8 yyagi
						{
							OpenNijiiroRW.TJA.tWAVの読み込み(OpenNijiiroRW.TJA.listWAV[nWAVcount]);
						}
						nWAVcount++;
					}
					if (nWAVcount > OpenNijiiroRW.TJA.listWAV.Count)
					{
						TimeSpan span = (TimeSpan)(DateTime.Now - timeBeginLoadWAV);
						Trace.TraceInformation("Song loading time({0,4}):     {1}", OpenNijiiroRW.TJA.listWAV.Count, span.ToString());
						timeBeginLoadWAV = DateTime.Now;

						if (OpenNijiiroRW.ConfigIni.bDynamicBassMixerManagement)
						{
							OpenNijiiroRW.TJA.PlanToAddMixerChannel();
						}

						for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
						{
							var _dtx = OpenNijiiroRW.GetTJA(i);
							_dtx?.tInitLocalStores(i);
							_dtx?.tRandomizeTaikoChips(i);
							_dtx?.tApplyFunMods(i);
							OpenNijiiroRW.ReplayInstances[i] = new CSongReplay(_dtx.strFullPath, i);
						}

						OpenNijiiroRW.stageGameScreen.Activate();

						span = (TimeSpan)(DateTime.Now - timeBeginLoadWAV);

						base.ePhaseID = CStage.EPhase.SongLoading_LoadBMPFile;
					}
					return (int)ESongLoadingScreenReturnValue.Continue;
				}

			case CStage.EPhase.SongLoading_LoadBMPFile:
				{
					TimeSpan span;
					DateTime timeBeginLoadBMPAVI = DateTime.Now;

					if (OpenNijiiroRW.ConfigIni.bEnableAVI)
						OpenNijiiroRW.TJA.tAVIの読み込み();
					span = (TimeSpan)(DateTime.Now - timeBeginLoadBMPAVI);

					span = (TimeSpan)(DateTime.Now - timeBeginLoad);
					Trace.TraceInformation("総読込時間:                {0}", span.ToString());

					if (OpenNijiiroRW.ConfigIni.FastRender)
					{
						var fastRender = new FastRender();
						fastRender.Render();
						fastRender = null;
					}


					OpenNijiiroRW.Timer.Update();
					//CSound管理.rc演奏用タイマ.t更新();
					base.ePhaseID = CStage.EPhase.SongLoading_WaitForSoundSystemBGM;
					return (int)ESongLoadingScreenReturnValue.Continue;
				}

			case CStage.EPhase.SongLoading_WaitForSoundSystemBGM:
				{
					long nCurrentTime = OpenNijiiroRW.Timer.NowTimeMs;
					if (nCurrentTime < this.nBGM再生開始時刻)
						this.nBGM再生開始時刻 = nCurrentTime;

					//						if ( ( nCurrentTime - this.nBGM再生開始時刻 ) > ( this.nBGMの総再生時間ms - 1000 ) )
					if ((nCurrentTime - this.nBGM再生開始時刻) >= (this.nBGMの総再生時間ms))    // #27787 2012.3.10 yyagi 1000ms == フェードイン分の時間
					{
						base.ePhaseID = CStage.EPhase.Common_FADEOUT;
					}
					return (int)ESongLoadingScreenReturnValue.Continue;
				}

			case CStage.EPhase.Common_FADEOUT:
				if (this.ct待機.IsUnEnded)        // DTXVモード時は、フェードアウト省略
					return (int)ESongLoadingScreenReturnValue.Continue;

				if (this.sd読み込み音 != null)
				{
					this.sd読み込み音.tDispose();
				}
				return (int)ESongLoadingScreenReturnValue.LoadComplete;
		}
		return (int)ESongLoadingScreenReturnValue.Continue;
	}

	/// <summary>
	/// ESC押下時、trueを返す
	/// </summary>
	/// <returns></returns>
	protected bool tキー入力()
	{
		IInputDevice keyboard = OpenNijiiroRW.InputManager.Keyboard;
		if (keyboard.KeyPressed((int)SlimDXKeys.Key.Escape))        // escape (exit)
		{
			return true;
		}
		return false;
	}

	// その他

	#region [ private ]
	//-----------------
	//private CActFIFOBlack actFI;
	//private CActFIFOBlack actFO;
	private long nBGMの総再生時間ms;
	private long nBGM再生開始時刻;
	private CSound sd読み込み音;
	private string strSTAGEFILE;
	private string str曲タイトル;
	private string strサブタイトル;
	private CTexture txタイトル;
	private CTexture txサブタイトル;
	//private CTexture txSongnamePlate;
	private DateTime timeBeginLoad;
	private DateTime timeBeginLoadWAV;
	private int nWAVcount;
	private CCounter ct待機;
	private CCounter ct曲名表示;

	private CCachedFontRenderer pfTITLE;
	private CCachedFontRenderer pfSUBTITLE;

	private CCachedFontRenderer pfDanTitle = null;
	private CCachedFontRenderer pfDanSubTitle = null;
	//-----------------
	#endregion
}
