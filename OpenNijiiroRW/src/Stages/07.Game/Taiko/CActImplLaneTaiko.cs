using System.Runtime.InteropServices;
using FDK;
using Rectangle = System.Drawing.Rectangle;

namespace OpenNijiiroRW;

internal class CActImplLaneTaiko : CActivity
{
	/// <summary>
	/// レーンを描画するクラス。
	///
	///
	/// </summary>
	public CActImplLaneTaiko()
	{
		base.IsDeActivated = true;
	}

	public override void Activate()
	{
		for (int i = 0; i < 5; i++)
		{
			this.st状態[i].ct進行 = new CCounter();
			this.stBranch[i].ct分岐アニメ進行 = new CCounter();
			this.stBranch[i].nフラッシュ制御タイマ = -1;
			this.stBranch[i].nBranchレイヤー透明度 = 0;
			this.stBranch[i].nBranch文字透明度 = 0;
			this.stBranch[i].nY座標 = 0;

			this.ResetPlayStates();
		}
		this.ctゴーゴー = new CCounter();


		this.ctゴーゴー炎 = new CCounter(0, 6, 50, OpenNijiiroRW.Timer);
		base.Activate();
	}

	public override void DeActivate()
	{
		for (int i = 0; i < 5; i++)
		{
			this.st状態[i].ct進行 = null;
			this.stBranch[i].ct分岐アニメ進行 = null;
		}
		this.ctゴーゴー = null;

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
			for (int i = 0; i < 5; i++)
				this.stBranch[i].nフラッシュ制御タイマ = SoundManager.PlayTimer.NowTimeMs;
			base.IsFirstDraw = false;
		}

		//それぞれが独立したレイヤーでないといけないのでforループはパーツごとに分離すること。

		if (OpenNijiiroRW.ConfigIni.nPlayerCount <= 2 && !OpenNijiiroRW.ConfigIni.bAIBattleMode) OpenNijiiroRW.stageGameScreen.actMtaiko.DrawBackSymbol();

		#region[ レーン本体 ]


		int[] x = new int[5];
		int[] y = new int[5];

		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
			{
				x[i] = OpenNijiiroRW.Skin.Game_Lane_5P[0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
				y[i] = OpenNijiiroRW.Skin.Game_Lane_5P[1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);
			}
			else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 4 || OpenNijiiroRW.ConfigIni.nPlayerCount == 3)
			{
				x[i] = OpenNijiiroRW.Skin.Game_Lane_4P[0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
				y[i] = OpenNijiiroRW.Skin.Game_Lane_4P[1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);
			}
			else
			{
				x[i] = OpenNijiiroRW.Skin.Game_Lane_X[i];
				y[i] = OpenNijiiroRW.Skin.Game_Lane_Y[i];
			}
		}

		#endregion

		if (OpenNijiiroRW.ConfigIni.nPlayerCount > 2 && !OpenNijiiroRW.ConfigIni.bAIBattleMode) OpenNijiiroRW.stageGameScreen.actMtaiko.DrawBackSymbol();

		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			#region[ 分岐アニメ制御タイマー ]
			long num = FDK.SoundManager.PlayTimer.NowTimeMs;
			if (num < this.stBranch[i].nフラッシュ制御タイマ)
			{
				this.stBranch[i].nフラッシュ制御タイマ = num;
			}
			while ((num - this.stBranch[i].nフラッシュ制御タイマ) >= 30)
			{
				if (this.stBranch[i].nBranchレイヤー透明度 <= 255)
				{
					this.stBranch[i].nBranchレイヤー透明度 += 10;
				}

				if (this.stBranch[i].nBranch文字透明度 >= 0)
				{
					this.stBranch[i].nBranch文字透明度 -= 10;
				}

				if (this.stBranch[i].nY座標 != 0 && this.stBranch[i].nY座標 <= 20)
				{
					this.stBranch[i].nY座標++;
				}

				this.stBranch[i].nフラッシュ制御タイマ += 8;
			}

			if (!this.stBranch[i].ct分岐アニメ進行.IsStoped)
			{
				this.stBranch[i].ct分岐アニメ進行.Tick();
				if (this.stBranch[i].ct分岐アニメ進行.IsEnded)
				{
					this.stBranch[i].ct分岐アニメ進行.Stop();
				}
			}
			#endregion
		}
		#region[ 分岐レイヤー ]
		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			if (OpenNijiiroRW.stageGameScreen.bUseBranch[i] == true)
			{
				#region[ 動いていない ]
				switch (OpenNijiiroRW.stageGameScreen.nDisplayedBranchLane[i])
				{
					case CTja.ECourse.eNormal:
						if (OpenNijiiroRW.Tx.Lane_Base[0] != null)
						{
							OpenNijiiroRW.Tx.Lane_Base[0].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Base[0].t2D描画(x[i], y[i]);
						}
						break;
					case CTja.ECourse.eExpert:
						if (OpenNijiiroRW.Tx.Lane_Base[1] != null)
						{
							OpenNijiiroRW.Tx.Lane_Base[1].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Base[1].t2D描画(x[i], y[i]);
						}
						break;
					case CTja.ECourse.eMaster:
						if (OpenNijiiroRW.Tx.Lane_Base[2] != null)
						{
							OpenNijiiroRW.Tx.Lane_Base[2].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Base[2].t2D描画(x[i], y[i]);
						}
						break;
				}
				#endregion

				if (OpenNijiiroRW.ConfigIni.nBranchAnime == 1)
				{
					#region[ AC7～14風の背後レイヤー ]
					if (this.stBranch[i].ct分岐アニメ進行.IsTicked)
					{
						int n透明度 = ((100 - this.stBranch[i].ct分岐アニメ進行.CurrentValue) * 0xff) / 100;

						if (this.stBranch[i].ct分岐アニメ進行.IsEnded)
						{
							n透明度 = 255;
							this.stBranch[i].ct分岐アニメ進行.Stop();
						}

						#region[ 普通譜面_レベルアップ ]
						//普通→玄人
						if (this.stBranch[i].nBefore == CTja.ECourse.eNormal && this.stBranch[i].nAfter == CTja.ECourse.eExpert)
						{
							if (OpenNijiiroRW.Tx.Lane_Base[0] != null && OpenNijiiroRW.Tx.Lane_Base[1] != null)
							{
								OpenNijiiroRW.Tx.Lane_Base[0].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Base[1].Opacity = this.stBranch[i].nBranchレイヤー透明度;
								OpenNijiiroRW.Tx.Lane_Base[1].t2D描画(x[i], y[i]);
							}
						}
						//普通→達人
						if (this.stBranch[i].nBefore == CTja.ECourse.eNormal && this.stBranch[i].nAfter == CTja.ECourse.eMaster)
						{
							if (this.stBranch[i].ct分岐アニメ進行.CurrentValue < 100)
							{
								n透明度 = ((100 - this.stBranch[i].ct分岐アニメ進行.CurrentValue) * 0xff) / 100;
							}
							if (OpenNijiiroRW.Tx.Lane_Base[0] != null && OpenNijiiroRW.Tx.Lane_Base[2] != null)
							{
								OpenNijiiroRW.Tx.Lane_Base[0].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Base[2].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Base[2].Opacity = this.stBranch[i].nBranchレイヤー透明度;
							}
						}
						#endregion
						#region[ 玄人譜面_レベルアップ ]
						if (this.stBranch[i].nBefore == CTja.ECourse.eExpert && this.stBranch[i].nAfter == CTja.ECourse.eMaster)
						{
							if (OpenNijiiroRW.Tx.Lane_Base[1] != null && OpenNijiiroRW.Tx.Lane_Base[2] != null)
							{
								OpenNijiiroRW.Tx.Lane_Base[1].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Base[2].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Base[2].Opacity = this.stBranch[i].nBranchレイヤー透明度;
							}
						}
						#endregion
						#region[ 玄人譜面_レベルダウン ]
						if (this.stBranch[i].nBefore == CTja.ECourse.eExpert && this.stBranch[i].nAfter == CTja.ECourse.eNormal)
						{
							if (OpenNijiiroRW.Tx.Lane_Base[1] != null && OpenNijiiroRW.Tx.Lane_Base[0] != null)
							{
								OpenNijiiroRW.Tx.Lane_Base[1].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Base[0].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Base[0].Opacity = this.stBranch[i].nBranchレイヤー透明度;
							}
						}
						#endregion
						#region[ 達人譜面_レベルダウン ]
						if (this.stBranch[i].nBefore == CTja.ECourse.eMaster && this.stBranch[i].nAfter == CTja.ECourse.eNormal)
						{
							if (OpenNijiiroRW.Tx.Lane_Base[2] != null && OpenNijiiroRW.Tx.Lane_Base[0] != null)
							{
								OpenNijiiroRW.Tx.Lane_Base[2].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Base[0].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Base[0].Opacity = this.stBranch[i].nBranchレイヤー透明度;
							}
						}
						#endregion
					}
					#endregion
				}
				else if (OpenNijiiroRW.ConfigIni.nBranchAnime == 0)
				{
					OpenNijiiroRW.stageGameScreen.actLane.Draw();
				}
			}
		}
		#endregion

		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			#region[ ゴーゴータイムレーン背景レイヤー ]
			if (OpenNijiiroRW.Tx.Lane_Background_GoGo != null && OpenNijiiroRW.stageGameScreen.bIsGOGOTIME[i])
			{
				if (!this.ctゴーゴー.IsStoped)
				{
					this.ctゴーゴー.Tick();
				}

				if (this.ctゴーゴー.CurrentValue <= 4)
				{
					OpenNijiiroRW.Tx.Lane_Background_GoGo.Scale.Y = 0.2f;
					OpenNijiiroRW.Tx.Lane_Background_GoGo.t2D描画(x[i], y[i] + 54);
				}
				else if (this.ctゴーゴー.CurrentValue <= 5)
				{
					OpenNijiiroRW.Tx.Lane_Background_GoGo.Scale.Y = 0.4f;
					OpenNijiiroRW.Tx.Lane_Background_GoGo.t2D描画(x[i], y[i] + 40);
				}
				else if (this.ctゴーゴー.CurrentValue <= 6)
				{
					OpenNijiiroRW.Tx.Lane_Background_GoGo.Scale.Y = 0.6f;
					OpenNijiiroRW.Tx.Lane_Background_GoGo.t2D描画(x[i], y[i] + 26);
				}
				else if (this.ctゴーゴー.CurrentValue <= 8)
				{
					OpenNijiiroRW.Tx.Lane_Background_GoGo.Scale.Y = 0.8f;
					OpenNijiiroRW.Tx.Lane_Background_GoGo.t2D描画(x[i], y[i] + 13);
				}
				else if (this.ctゴーゴー.CurrentValue >= 9)
				{
					OpenNijiiroRW.Tx.Lane_Background_GoGo.Scale.Y = 1.0f;
					OpenNijiiroRW.Tx.Lane_Background_GoGo.t2D描画(x[i], y[i]);
				}
			}
			#endregion
		}

		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			if (OpenNijiiroRW.stageGameScreen.bUseBranch[i] == true)
			{
				#region NullCheck

				bool _laneNull = false;

				for (int j = 0; j < OpenNijiiroRW.Tx.Lane_Text.Length; j++)
				{
					if (OpenNijiiroRW.Tx.Lane_Text[j] == null)
					{
						_laneNull = true;
						break;
					}
				}

				#endregion

				if (OpenNijiiroRW.ConfigIni.SimpleMode)
				{
					switch (OpenNijiiroRW.stageGameScreen.nDisplayedBranchLane[i])
					{
						case CTja.ECourse.eNormal:
							OpenNijiiroRW.Tx.Lane_Text[0].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], y[i]);
							break;
						case CTja.ECourse.eExpert:
							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i]);
							break;
						case CTja.ECourse.eMaster:
							OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], y[i]);
							break;
					}
				}
				else if (OpenNijiiroRW.ConfigIni.nBranchAnime == 0 && !_laneNull)
				{
					if (!this.stBranch[i].ct分岐アニメ進行.IsTicked)
					{
						switch (OpenNijiiroRW.stageGameScreen.nDisplayedBranchLane[i])
						{
							case CTja.ECourse.eNormal:
								OpenNijiiroRW.Tx.Lane_Text[0].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], y[i]);
								break;
							case CTja.ECourse.eExpert:
								OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i]);
								break;
							case CTja.ECourse.eMaster:
								OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], y[i]);
								break;
						}
					}
					if (this.stBranch[i].ct分岐アニメ進行.IsTicked)
					{
						#region[ 普通譜面_レベルアップ ]
						//普通→玄人
						if (this.stBranch[i].nBefore == 0 && this.stBranch[i].nAfter == CTja.ECourse.eExpert)
						{
							OpenNijiiroRW.Tx.Lane_Text[0].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;

							OpenNijiiroRW.Tx.Lane_Text[0].Opacity = this.stBranch[i].ct分岐アニメ進行.CurrentValue > 100 ? 0 : (255 - ((this.stBranch[i].ct分岐アニメ進行.CurrentValue * 0xff) / 60));
							//CDTXMania.Tx.Lane_Text[1].n透明度 = this.ct分岐アニメ進行.n現在の値 > 100 ? 255 : ( ( ( this.ct分岐アニメ進行.n現在の値 * 0xff ) / 60 ) );
							if (this.stBranch[i].ct分岐アニメ進行.CurrentValue < 60)
							{
								this.stBranch[i].nY = this.stBranch[i].ct分岐アニメ進行.CurrentValue / 2;
								OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], y[i] + this.stBranch[i].nY);
								OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], (y[i] - 30) + this.stBranch[i].nY);
							}
							else
							{
								OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i]);
							}

						}

						//普通→達人
						if (this.stBranch[i].nBefore == 0 && this.stBranch[i].nAfter == CTja.ECourse.eMaster)
						{
							OpenNijiiroRW.Tx.Lane_Text[0].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;
							if (this.stBranch[i].ct分岐アニメ進行.CurrentValue < 60)
							{
								this.stBranch[i].nY = this.stBranch[i].ct分岐アニメ進行.CurrentValue / 2;
								OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], (y[i] - 12) + this.stBranch[i].nY);
								OpenNijiiroRW.Tx.Lane_Text[0].Opacity = this.stBranch[i].ct分岐アニメ進行.CurrentValue > 100 ? 0 : (255 - ((this.stBranch[i].ct分岐アニメ進行.CurrentValue * 0xff) / 100));
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], (y[i] - 20) + this.stBranch[i].nY);
							}
							//if( this.stBranch[ i ].ct分岐アニメ進行.n現在の値 >= 5 && this.stBranch[ i ].ct分岐アニメ進行.n現在の値 < 60 )
							//{
							//    this.stBranch[ i ].nY = this.stBranch[ i ].ct分岐アニメ進行.n現在の値 / 2;
							//    this.tx普通譜面[ 1 ].t2D描画(CDTXMania.app.Device, 333, CDTXMania.Skin.nScrollFieldY[ i ] + this.stBranch[ i ].nY);
							//    this.tx普通譜面[ 1 ].n透明度 = this.stBranch[ i ].ct分岐アニメ進行.n現在の値 > 100 ? 0 : ( 255 - ( ( this.stBranch[ i ].ct分岐アニメ進行.n現在の値 * 0xff) / 100));
							//    this.tx玄人譜面[ 1 ].t2D描画(CDTXMania.app.Device, 333, ( CDTXMania.Skin.nScrollFieldY[ i ] - 10 ) + this.stBranch[ i ].nY);
							//}
							else if (this.stBranch[i].ct分岐アニメ進行.CurrentValue >= 60 && this.stBranch[i].ct分岐アニメ進行.CurrentValue < 150)
							{
								this.stBranch[i].nY = 21;
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;
							}
							else if (this.stBranch[i].ct分岐アニメ進行.CurrentValue >= 150 && this.stBranch[i].ct分岐アニメ進行.CurrentValue < 210)
							{
								this.stBranch[i].nY = ((this.stBranch[i].ct分岐アニメ進行.CurrentValue - 150) / 2);
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i] + this.stBranch[i].nY);
								OpenNijiiroRW.Tx.Lane_Text[1].Opacity = this.stBranch[i].ct分岐アニメ進行.CurrentValue > 100 ? 0 : (255 - ((this.stBranch[i].ct分岐アニメ進行.CurrentValue * 0xff) / 100));
								OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], (y[i] - 20) + this.stBranch[i].nY);
							}
							else
							{
								OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], y[i]);
							}
						}
						#endregion
						#region[ 玄人譜面_レベルアップ ]
						//玄人→達人
						if (this.stBranch[i].nBefore == CTja.ECourse.eExpert && this.stBranch[i].nAfter == CTja.ECourse.eMaster)
						{
							OpenNijiiroRW.Tx.Lane_Text[0].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;

							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = this.stBranch[i].ct分岐アニメ進行.CurrentValue > 100 ? 0 : (255 - ((this.stBranch[i].ct分岐アニメ進行.CurrentValue * 0xff) / 60));
							if (this.stBranch[i].ct分岐アニメ進行.CurrentValue < 60)
							{
								this.stBranch[i].nY = this.stBranch[i].ct分岐アニメ進行.CurrentValue / 2;
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i] + this.stBranch[i].nY);
								OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], (y[i] - 20) + this.stBranch[i].nY);
							}
							else
							{
								OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], y[i]);
							}
						}
						#endregion
						#region[ 玄人譜面_レベルダウン ]
						if (this.stBranch[i].nBefore == CTja.ECourse.eExpert && this.stBranch[i].nAfter == CTja.ECourse.eNormal)
						{
							OpenNijiiroRW.Tx.Lane_Text[0].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;

							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = this.stBranch[i].ct分岐アニメ進行.CurrentValue > 100 ? 0 : (255 - ((this.stBranch[i].ct分岐アニメ進行.CurrentValue * 0xff) / 60));
							if (this.stBranch[i].ct分岐アニメ進行.CurrentValue < 60)
							{
								this.stBranch[i].nY = this.stBranch[i].ct分岐アニメ進行.CurrentValue / 2;
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i] - this.stBranch[i].nY);
								OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], (y[i] + 30) - this.stBranch[i].nY);
							}
							else
							{
								OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], y[i]);
							}
						}
						#endregion
						#region[ 達人譜面_レベルダウン ]
						if (this.stBranch[i].nBefore == CTja.ECourse.eMaster && this.stBranch[i].nAfter == CTja.ECourse.eNormal)
						{
							OpenNijiiroRW.Tx.Lane_Text[0].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;

							if (this.stBranch[i].ct分岐アニメ進行.CurrentValue < 60)
							{
								this.stBranch[i].nY = this.stBranch[i].ct分岐アニメ進行.CurrentValue / 2;
								OpenNijiiroRW.Tx.Lane_Text[2].Opacity = this.stBranch[i].ct分岐アニメ進行.CurrentValue > 100 ? 0 : (255 - ((this.stBranch[i].ct分岐アニメ進行.CurrentValue * 0xff) / 60));
								OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], y[i] - this.stBranch[i].nY);
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], (y[i] + 30) - this.stBranch[i].nY);
							}
							else if (this.stBranch[i].ct分岐アニメ進行.CurrentValue >= 60 && this.stBranch[i].ct分岐アニメ進行.CurrentValue < 150)
							{
								this.stBranch[i].nY = 21;
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i]);
								OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;
							}
							else if (this.stBranch[i].ct分岐アニメ進行.CurrentValue >= 150 && this.stBranch[i].ct分岐アニメ進行.CurrentValue < 210)
							{
								this.stBranch[i].nY = ((this.stBranch[i].ct分岐アニメ進行.CurrentValue - 150) / 2);
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i] - this.stBranch[i].nY);
								OpenNijiiroRW.Tx.Lane_Text[1].Opacity = this.stBranch[i].ct分岐アニメ進行.CurrentValue > 100 ? 0 : (255 - ((this.stBranch[i].ct分岐アニメ進行.CurrentValue * 0xff) / 100));
								OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], (y[i] + 30) - this.stBranch[i].nY);
							}
							else if (this.stBranch[i].ct分岐アニメ進行.CurrentValue >= 210)
							{
								OpenNijiiroRW.Tx.Lane_Text[0].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], y[i]);
							}
						}
						if (this.stBranch[i].nBefore == CTja.ECourse.eMaster && this.stBranch[i].nAfter == CTja.ECourse.eExpert)
						{
							OpenNijiiroRW.Tx.Lane_Text[0].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
							OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;

							OpenNijiiroRW.Tx.Lane_Text[2].Opacity = this.stBranch[i].ct分岐アニメ進行.CurrentValue > 100 ? 0 : (255 - ((this.stBranch[i].ct分岐アニメ進行.CurrentValue * 0xff) / 60));
							if (this.stBranch[i].ct分岐アニメ進行.CurrentValue < 60)
							{
								this.stBranch[i].nY = this.stBranch[i].ct分岐アニメ進行.CurrentValue / 2;
								OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], y[i] - this.stBranch[i].nY);
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], (y[i] + 30) - this.stBranch[i].nY);
							}
							else
							{
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i]);
							}

						}
						#endregion
					}
				}
				else if (!_laneNull)
				{
					if (this.stBranch[i].nY座標 == 21)
					{
						this.stBranch[i].nY座標 = 0;
					}

					if (this.stBranch[i].nY座標 == 0)
					{
						switch (OpenNijiiroRW.stageGameScreen.nDisplayedBranchLane[i])
						{
							case CTja.ECourse.eNormal:
								OpenNijiiroRW.Tx.Lane_Text[0].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], y[i]);
								break;
							case CTja.ECourse.eExpert:
								OpenNijiiroRW.Tx.Lane_Text[1].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i]);
								break;
							case CTja.ECourse.eMaster:
								OpenNijiiroRW.Tx.Lane_Text[2].Opacity = 255;
								OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], y[i]);
								break;
						}
					}

					if (this.stBranch[i].nY座標 != 0)
					{
						#region[ 普通譜面_レベルアップ ]
						//普通→玄人
						if (this.stBranch[i].nBefore == CTja.ECourse.eNormal && this.stBranch[i].nAfter == CTja.ECourse.eExpert)
						{
							OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], y[i] - this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], (y[i] + 20) - this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[0].Opacity = this.stBranch[i].nBranchレイヤー透明度;
						}
						//普通→達人
						if (this.stBranch[i].nBefore == CTja.ECourse.eNormal && this.stBranch[i].nAfter == CTja.ECourse.eMaster)
						{
							OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], y[i] - this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], (y[i] + 20) - this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[0].Opacity = this.stBranch[i].nBranchレイヤー透明度;
						}
						#endregion
						#region[ 玄人譜面_レベルアップ ]
						//玄人→達人
						if (this.stBranch[i].nBefore == CTja.ECourse.eExpert && this.stBranch[i].nAfter == CTja.ECourse.eMaster)
						{
							OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i] - this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], (y[i] + 20) - this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = this.stBranch[i].nBranchレイヤー透明度;
						}
						#endregion
						#region[ 玄人譜面_レベルダウン ]
						if (this.stBranch[i].nBefore == CTja.ECourse.eExpert && this.stBranch[i].nAfter == CTja.ECourse.eNormal)
						{
							OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], y[i] + this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], (y[i] - 24) + this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[1].Opacity = this.stBranch[i].nBranchレイヤー透明度;
						}
						#endregion
						#region[ 達人譜面_レベルダウン ]
						if (this.stBranch[i].nBefore == CTja.ECourse.eMaster && this.stBranch[i].nAfter == CTja.ECourse.eNormal)
						{
							OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], y[i] + this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[0].t2D描画(x[i], (y[i] - 24) + this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[2].Opacity = this.stBranch[i].nBranchレイヤー透明度;
						}
						if (this.stBranch[i].nBefore == CTja.ECourse.eMaster && this.stBranch[i].nAfter == CTja.ECourse.eExpert)
						{
							OpenNijiiroRW.Tx.Lane_Text[2].t2D描画(x[i], y[i] + this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[1].t2D描画(x[i], (y[i] - 24) + this.stBranch[i].nY座標);
							OpenNijiiroRW.Tx.Lane_Text[2].Opacity = this.stBranch[i].nBranchレイヤー透明度;
						}
						#endregion
					}
				}

			}
		}


		OpenNijiiroRW.stageGameScreen.actTaikoLaneFlash.Draw();



		if (OpenNijiiroRW.Tx.Lane[0] != null)
		{
			for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
			{
				int frame_x = 498;
				int frame_y = i == 0 ? 276 : 540;

				if (OpenNijiiroRW.ConfigIni.bAIBattleMode)
				{
					OpenNijiiroRW.Tx.Lane[1]?.t2D拡大率考慮描画(CTexture.RefPnt.UpLeft, frame_x, frame_y);
				}
				else
				{
					OpenNijiiroRW.Tx.Lane[0].Scale.X = 177.75f;
					OpenNijiiroRW.Tx.Lane[0]?.t2D拡大率考慮描画(CTexture.RefPnt.UpLeft, frame_x, frame_y);
				}
			}
		}

		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			if (this.n総移動時間[i] == -1)
			{
				continue;
			}
			var nTime = (int)(long)OpenNijiiroRW.GetTJA(i)!.GameTimeToTjaTime(SoundManager.PlayTimer.NowTimeMs);
			if (nTime < this.n移動開始時刻[i])
			{ // in case of rewinding
				OpenNijiiroRW.stageGameScreen.JPOSCROLLX[i] = this.n移動開始X[i];
				OpenNijiiroRW.stageGameScreen.JPOSCROLLY[i] = this.n移動開始Y[i];
			}
			else if (nTime < this.n移動開始時刻[i] + this.n総移動時間[i])
			{
				OpenNijiiroRW.stageGameScreen.JPOSCROLLX[i] = this.n移動開始X[i] + (((nTime - this.n移動開始時刻[i]) / (double)this.n総移動時間[i]) * this.n移動距離px[i]);
				OpenNijiiroRW.stageGameScreen.JPOSCROLLY[i] = this.n移動開始Y[i] + (((nTime - this.n移動開始時刻[i]) / (double)this.n総移動時間[i]) * this.nVerticalJSPos[i]);
			}
			else
			{
				this.n総移動時間[i] = -1;
				OpenNijiiroRW.stageGameScreen.JPOSCROLLX[i] = this.n移動目的場所X[i];
				OpenNijiiroRW.stageGameScreen.JPOSCROLLY[i] = this.n移動目的場所Y[i];
			}
		}




		if (OpenNijiiroRW.ConfigIni.bEnableAVI && OpenNijiiroRW.TJA.listVD.Count > 0 && OpenNijiiroRW.stageGameScreen.ShowVideo)
		{
			if (OpenNijiiroRW.Tx.Lane_Background_GoGo != null) OpenNijiiroRW.Tx.Lane_Background_GoGo.Opacity = OpenNijiiroRW.ConfigIni.nBGAlpha;
		}
		else
		{
			if (OpenNijiiroRW.Tx.Lane_Background_GoGo != null) OpenNijiiroRW.Tx.Lane_Background_GoGo.Opacity = 255;
		}

		return base.Draw();
	}

	public void ResetPlayStates()
	{
		for (int i = 0; i < 5; ++i)
		{
			this.n総移動時間[i] = -1;
		}
	}

	public void ゴーゴー炎()
	{
		//判定枠
		if (OpenNijiiroRW.Tx.Judge_Frame != null)
		{
			OpenNijiiroRW.Tx.Judge_Frame.b加算合成 = OpenNijiiroRW.Skin.Game_JudgeFrame_AddBlend;

			for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
			{
				//Assets.Sprite.Enso_Lane[2].BlendMode = BlendMode.Add;
				//Assets.Sprite.Enso_Lane[2].Draw(510.0, 278.0);
				OpenNijiiroRW.Tx.Judge_Frame.Opacity = 150;
				OpenNijiiroRW.Tx.Judge_Frame.t2D描画(OpenNijiiroRW.stageGameScreen.NoteOriginX[i] - 13, OpenNijiiroRW.stageGameScreen.NoteOriginY[i] - 10);
			}
		}


		#region[ ゴーゴー炎 ]
		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			if (OpenNijiiroRW.stageGameScreen.bIsGOGOTIME[i] && !OpenNijiiroRW.ConfigIni.SimpleMode)
			{
				this.ctゴーゴー炎.TickLoop();

				if (OpenNijiiroRW.Tx.Effects_Fire != null)
				{
					float f倍率 = 1.0f;

					float[] ar倍率 = new float[] { 0.8f, 1.2f, 1.7f, 2.5f, 2.3f, 2.2f, 2.0f, 1.8f, 1.7f, 1.6f, 1.6f, 1.5f, 1.5f, 1.4f, 1.3f, 1.2f, 1.1f, 1.0f };

					f倍率 = ar倍率[this.ctゴーゴー.CurrentValue];

					/*
                    Matrix mat = Matrix.Identity;
                    mat *= Matrix.Scaling(f倍率, f倍率, 1.0f);
                    mat *= Matrix.Translation(TJAPlayer3.Skin.nScrollFieldX[i] - SampleFramework.RenderSurfaceSize.Width / 2.0f, -(TJAPlayer3.Skin.nJudgePointY[i] - SampleFramework.RenderSurfaceSize.Height / 2.0f), 0f);
                    */
					//this.txゴーゴー炎.b加算合成 = true;

					//this.ctゴーゴー.n現在の値 = 6;

					int width = OpenNijiiroRW.Tx.Effects_Fire.szTextureSize.Width / 7;
					int height = OpenNijiiroRW.Tx.Effects_Fire.szTextureSize.Height;

					float x = -(width * (f倍率 - 1.0f) / 2.0f);
					float y = -(height * (f倍率 - 1.0f) / 2.0f);

					if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
					{
						x += OpenNijiiroRW.Skin.Game_Effect_Fire_5P[0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
						y += OpenNijiiroRW.Skin.Game_Effect_Fire_5P[1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);
					}
					else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 4 || OpenNijiiroRW.ConfigIni.nPlayerCount == 3)
					{
						x += OpenNijiiroRW.Skin.Game_Effect_Fire_4P[0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
						y += OpenNijiiroRW.Skin.Game_Effect_Fire_4P[1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);
					}
					else
					{
						x += OpenNijiiroRW.Skin.Game_Effect_Fire_X[i];
						y += OpenNijiiroRW.Skin.Game_Effect_Fire_Y[i];
					}

					OpenNijiiroRW.Tx.Effects_Fire.Scale.X = f倍率;
					OpenNijiiroRW.Tx.Effects_Fire.Scale.Y = f倍率;

					OpenNijiiroRW.Tx.Effects_Fire.t2D描画(x, y,
						new Rectangle(width * (this.ctゴーゴー炎.CurrentValue), 0, width, height));
				}
			}
		}
		#endregion
		for (int i = 0; i < OpenNijiiroRW.ConfigIni.nPlayerCount; i++)
		{
			if (!this.st状態[i].ct進行.IsStoped)
			{
				this.st状態[i].ct進行.Tick();
				if (this.st状態[i].ct進行.IsEnded)
				{
					this.st状態[i].ct進行.Stop();
				}
				//if( this.txアタックエフェクトLower != null )
				{
					//this.txアタックエフェクトLower.b加算合成 = true;
					int n = this.st状態[i].nIsBig == 1 ? 520 : 0;

					float x = 0;
					float y = 0;

					if (OpenNijiiroRW.ConfigIni.nPlayerCount == 5)
					{
						x = OpenNijiiroRW.Skin.Game_Effects_Hit_Explosion_5P[0] + (OpenNijiiroRW.Skin.Game_UIMove_5P[0] * i);
						y = OpenNijiiroRW.Skin.Game_Effects_Hit_Explosion_5P[1] + (OpenNijiiroRW.Skin.Game_UIMove_5P[1] * i);
					}
					else if (OpenNijiiroRW.ConfigIni.nPlayerCount == 4 || OpenNijiiroRW.ConfigIni.nPlayerCount == 3)
					{
						x = OpenNijiiroRW.Skin.Game_Effects_Hit_Explosion_4P[0] + (OpenNijiiroRW.Skin.Game_UIMove_4P[0] * i);
						y = OpenNijiiroRW.Skin.Game_Effects_Hit_Explosion_4P[1] + (OpenNijiiroRW.Skin.Game_UIMove_4P[1] * i);
					}
					else
					{
						x = OpenNijiiroRW.Skin.Game_Effects_Hit_Explosion_X[i];
						y = OpenNijiiroRW.Skin.Game_Effects_Hit_Explosion_Y[i];
					}
					x += OpenNijiiroRW.stageGameScreen.GetJPOSCROLLX(i);
					y += OpenNijiiroRW.stageGameScreen.GetJPOSCROLLY(i);

					switch (st状態[i].judge)
					{
						case ENoteJudge.Perfect:
						case ENoteJudge.Great:
						case ENoteJudge.Auto:
							if (!OpenNijiiroRW.ConfigIni.SimpleMode)
							{
								//this.txアタックエフェクトLower.t2D描画( CDTXMania.app.Device, 285, 127, new Rectangle( this.st状態[ i ].ct進行.n現在の値 * 260, n, 260, 260 ) );
								if (this.st状態[i].nIsBig == 1 && OpenNijiiroRW.Tx.Effects_Hit_Great_Big[this.st状態[i].ct進行.CurrentValue] != null)
									OpenNijiiroRW.Tx.Effects_Hit_Great_Big[this.st状態[i].ct進行.CurrentValue].t2D描画(x, y);
								else if (OpenNijiiroRW.Tx.Effects_Hit_Great[this.st状態[i].ct進行.CurrentValue] != null)
									OpenNijiiroRW.Tx.Effects_Hit_Great[this.st状態[i].ct進行.CurrentValue].t2D描画(x, y);
							}
							break;

						case ENoteJudge.Good:
							//this.txアタックエフェクトLower.t2D描画( CDTXMania.app.Device, 285, 127, new Rectangle( this.st状態[ i ].ct進行.n現在の値 * 260, n + 260, 260, 260 ) );
							if (this.st状態[i].nIsBig == 1 && OpenNijiiroRW.Tx.Effects_Hit_Good_Big[this.st状態[i].ct進行.CurrentValue] != null)
								OpenNijiiroRW.Tx.Effects_Hit_Good_Big[this.st状態[i].ct進行.CurrentValue].t2D描画(x, y);
							else if (OpenNijiiroRW.Tx.Effects_Hit_Good[this.st状態[i].ct進行.CurrentValue] != null)
								OpenNijiiroRW.Tx.Effects_Hit_Good[this.st状態[i].ct進行.CurrentValue].t2D描画(x, y);
							break;

						case ENoteJudge.Miss:
						case ENoteJudge.Bad:
							break;
					}
				}
			}
		}


	}

	public virtual void Start(int nLane, ENoteJudge judge, bool b両手入力, int nPlayer)
	{
		//2017.08.15 kairera0467 排他なので番地をそのまま各レーンの状態として扱う

		//for( int n = 0; n < 1; n++ )
		{
			this.st状態[nPlayer].ct進行 = new CCounter(0, 14, 20, OpenNijiiroRW.Timer);
			this.st状態[nPlayer].judge = judge;
			this.st状態[nPlayer].nPlayer = nPlayer;

			switch (nLane)
			{
				case 0x11:
				case 0x12:
					this.st状態[nPlayer].nIsBig = 0;
					break;
				case 0x13:
				case 0x14:
				case 0x1A:
				case 0x1B:
					{
						if (b両手入力)
							this.st状態[nPlayer].nIsBig = 1;
						else
							this.st状態[nPlayer].nIsBig = 0;
					}
					break;
			}
		}
	}


	public void GOGOSTART()
	{
		this.ctゴーゴー = new CCounter(0, 17, 18, OpenNijiiroRW.Timer);
		if (OpenNijiiroRW.ConfigIni.nPlayerCount == 1 && OpenNijiiroRW.stageSongSelect.nChoosenSongDifficulty[0] != (int)Difficulty.Dan) OpenNijiiroRW.stageGameScreen.GoGoSplash.StartSplash();
	}


	public void t分岐レイヤー_コース変化(CTja.ECourse n現在, CTja.ECourse n次回, int nPlayer)
	{
		if (n現在 == n次回)
		{
			return;
		}
		this.stBranch[nPlayer].ct分岐アニメ進行 = new CCounter(0, 300, 2, OpenNijiiroRW.Timer);

		this.stBranch[nPlayer].nBranchレイヤー透明度 = 6;
		this.stBranch[nPlayer].nY座標 = 1;

		this.stBranch[nPlayer].nBefore = n現在;
		this.stBranch[nPlayer].nAfter = n次回;

		OpenNijiiroRW.stageGameScreen.actLane.t分岐レイヤー_コース変化(n現在, n次回, nPlayer);
	}

	public void t判定枠移動(int nPlayer, CTja.CJPOSSCROLL jposscroll, int msTimeNote)
	{
		this.n移動開始時刻[nPlayer] = msTimeNote;
		this.n移動開始X[nPlayer] = jposscroll.pxOrigX;
		this.n移動開始Y[nPlayer] = jposscroll.pxOrigY;
		this.n総移動時間[nPlayer] = (int)jposscroll.msMoveDt;
		double pxMoveDx = this.n移動距離px[nPlayer] = jposscroll.pxMoveDx;
		double pxMoveDy = this.nVerticalJSPos[nPlayer] = jposscroll.pxMoveDy;
		this.n移動目的場所X[nPlayer] = jposscroll.pxOrigX + pxMoveDx;
		this.n移動目的場所Y[nPlayer] = jposscroll.pxOrigY + pxMoveDy;
	}

	#region[ private ]
	//-----------------
	//private CTexture txLane;
	//private CTexture txLaneB;
	//private CTexture tx枠線;
	//private CTexture tx判定枠;
	//private CTexture txゴーゴー;
	//private CTexture txゴーゴー炎;
	//private CTexture[] txArゴーゴー炎;
	//private CTexture[] txArアタックエフェクトLower_A;
	//private CTexture[] txArアタックエフェクトLower_B;
	//private CTexture[] txArアタックエフェクトLower_C;
	//private CTexture[] txArアタックエフェクトLower_D;

	//private CTexture[] txLaneFlush = new CTexture[3];

	//private CTexture[] tx普通譜面 = new CTexture[2];
	//private CTexture[] tx玄人譜面 = new CTexture[2];
	//private CTexture[] tx達人譜面 = new CTexture[2];

	//private CTextureAf txアタックエフェクトLower;

	protected STSTATUS[] st状態 = new STSTATUS[5];

	//private CTexture[] txゴーゴースプラッシュ;

	[StructLayout(LayoutKind.Sequential)]
	protected struct STSTATUS
	{
		public bool b使用中;
		public CCounter ct進行;
		public ENoteJudge judge;
		public int nIsBig;
		public int n透明度;
		public int nPlayer;
	}
	private CCounter ctゴーゴー;
	private CCounter ctゴーゴー炎;



	public STBRANCH[] stBranch = new STBRANCH[5];
	[StructLayout(LayoutKind.Sequential)]
	public struct STBRANCH
	{
		public CCounter ct分岐アニメ進行;
		public CTja.ECourse nBefore;
		public CTja.ECourse nAfter;

		public long nフラッシュ制御タイマ;
		public int nBranchレイヤー透明度;
		public int nBranch文字透明度;
		public int nY座標;
		public int nY;
	}


	private int[] n総移動時間 = new int[5];
	private double[] n移動開始X = new double[5];
	private double[] n移動開始Y = new double[5];
	private int[] n移動開始時刻 = new int[5];
	private double[] n移動距離px = new double[5];
	private double[] nVerticalJSPos = new double[5];
	private double[] n移動目的場所X = new double[5];
	private double[] n移動目的場所Y = new double[5];

	//-----------------
	#endregion
}
