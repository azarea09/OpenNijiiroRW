using System.Runtime.InteropServices;
using FDK;
using Rectangle = System.Drawing.Rectangle;

namespace OpenNijiiroRW;

internal class CActImplChipEffects : CActivity
{
	// コンストラクタ

	public CActImplChipEffects()
	{
		//base.b活性化してない = true;
	}


	// メソッド
	public virtual void Start(int nPlayer, int Lane)
	{
		if (OpenNijiiroRW.Tx.Gauge_Soul_Explosion != null && OpenNijiiroRW.ConfigIni.nPlayerCount <= 2 && !OpenNijiiroRW.ConfigIni.bAIBattleMode)
		{
			for (int i = 0; i < 128; i++)
			{
				if (!st[i].b使用中)
				{
					st[i].b使用中 = true;
					st[i].ct進行 = new CCounter(0, OpenNijiiroRW.Skin.Game_Effect_NotesFlash[2], OpenNijiiroRW.Skin.Game_Effect_NotesFlash_Timer, OpenNijiiroRW.Timer);
					st[i].ctChipEffect = new CCounter(0, 24, 17, OpenNijiiroRW.Timer);
					st[i].nプレイヤー = nPlayer;
					st[i].Lane = Lane;
					break;
				}
			}
		}
	}

	// CActivity 実装

	public override void Activate()
	{
		for (int i = 0; i < 128; i++)
		{
			st[i] = new STチップエフェクト
			{
				b使用中 = false,
				ct進行 = new CCounter(),
				ctChipEffect = new CCounter()
			};
		}
		base.Activate();
	}
	public override void DeActivate()
	{
		for (int i = 0; i < 128; i++)
		{
			st[i].ct進行 = null;
			st[i].ctChipEffect = null;
			st[i].b使用中 = false;
		}
		base.DeActivate();
	}
	public override int Draw()
	{
		for (int i = 0; i < 128; i++)
		{
			if (st[i].b使用中)
			{
				st[i].ct進行.Tick();
				st[i].ctChipEffect.Tick();
				if (st[i].ct進行.IsEnded)
				{
					st[i].ct進行.Stop();
					st[i].b使用中 = false;
				}

				switch (st[i].nプレイヤー)
				{
					case 0:
						OpenNijiiroRW.Tx.Gauge_Soul_Explosion[OpenNijiiroRW.P1IsBlue() ? 1 : 0]?.t2D中心基準描画(OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_X[0], OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_Y[0], new Rectangle(st[i].ct進行.CurrentValue * OpenNijiiroRW.Skin.Game_Effect_NotesFlash[0], 0, OpenNijiiroRW.Skin.Game_Effect_NotesFlash[0], OpenNijiiroRW.Skin.Game_Effect_NotesFlash[1]));

						if (this.st[i].ctChipEffect.CurrentValue < 13)
							NotesManager.DisplayNote(
								st[i].nプレイヤー,
								OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_X[0],
								OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_Y[0],
								st[i].Lane);
						break;

					case 1:
						OpenNijiiroRW.Tx.Gauge_Soul_Explosion[1]?.t2D中心基準描画(OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_X[1], OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_Y[1], new Rectangle(st[i].ct進行.CurrentValue * OpenNijiiroRW.Skin.Game_Effect_NotesFlash[0], 0, OpenNijiiroRW.Skin.Game_Effect_NotesFlash[0], OpenNijiiroRW.Skin.Game_Effect_NotesFlash[1]));
						if (this.st[i].ctChipEffect.CurrentValue < 13)
							NotesManager.DisplayNote(
								st[i].nプレイヤー,
								OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_X[1],
								OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_Y[1],
								st[i].Lane);
						break;
				}

				if (OpenNijiiroRW.Tx.ChipEffect != null)
				{
					if (this.st[i].ctChipEffect.CurrentValue < 12)
					{
						OpenNijiiroRW.Tx.ChipEffect.color4 = new Color4(1.0f, 1.0f, 0.0f, 1.0f);
						OpenNijiiroRW.Tx.ChipEffect.Opacity = (int)(this.st[i].ctChipEffect.CurrentValue * (float)(225 / 11));
						OpenNijiiroRW.Tx.ChipEffect.t2D中心基準描画(OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_X[st[i].nプレイヤー], OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_Y[st[i].nプレイヤー], new Rectangle(st[i].Lane * OpenNijiiroRW.Skin.Game_Notes_Size[0], 0, OpenNijiiroRW.Skin.Game_Notes_Size[0], OpenNijiiroRW.Skin.Game_Notes_Size[1]));
					}
					if (this.st[i].ctChipEffect.CurrentValue > 12 && this.st[i].ctChipEffect.CurrentValue < 24)
					{
						OpenNijiiroRW.Tx.ChipEffect.color4 = new Color4(1.0f, 1.0f, 1.0f, 1.0f);
						OpenNijiiroRW.Tx.ChipEffect.Opacity = 255 - (int)((this.st[i].ctChipEffect.CurrentValue - 10) * (float)(255 / 14));
						OpenNijiiroRW.Tx.ChipEffect.t2D中心基準描画(OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_X[st[i].nプレイヤー], OpenNijiiroRW.Skin.Game_Effect_FlyingNotes_EndPoint_Y[st[i].nプレイヤー], new Rectangle(st[i].Lane * OpenNijiiroRW.Skin.Game_Notes_Size[0], 0, OpenNijiiroRW.Skin.Game_Notes_Size[0], OpenNijiiroRW.Skin.Game_Notes_Size[1]));
					}
				}

			}
		}
		return 0;
	}


	// その他

	#region [ private ]
	//-----------------
	//private CTexture[] txChara;

	[StructLayout(LayoutKind.Sequential)]
	private struct STチップエフェクト
	{
		public bool b使用中;
		public CCounter ct進行;
		public CCounter ctChipEffect;
		public int nプレイヤー;
		public int Lane;
	}
	private STチップエフェクト[] st = new STチップエフェクト[128];

	//-----------------
	#endregion
}
