using System.Drawing;
using FDK;

namespace OpenNijiiroRW;

class GoGoSplash : CActivity
{
	public GoGoSplash()
	{
		this.IsDeActivated = true;
	}

	public override void Activate()
	{
		Splash = new CCounter();
		base.Activate();
	}

	public override void DeActivate()
	{
		base.DeActivate();
	}

	/// <summary>
	/// ゴーゴースプラッシュの描画処理です。
	/// SkinCofigで本数を変更することができます。
	/// </summary>
	/// <returns></returns>
	public override int Draw()
	{
		if (Splash == null || OpenNijiiroRW.ConfigIni.SimpleMode) return base.Draw();
		Splash.Tick();
		if (Splash.IsEnded)
		{
			Splash.CurrentValue = 0;
			Splash.Stop();
		}
		if (Splash.IsTicked)
		{
			for (int i = 0; i < OpenNijiiroRW.Skin.Game_Effect_GoGoSplash_X.Length; i++)
			{
				if (i > OpenNijiiroRW.Skin.Game_Effect_GoGoSplash_Y.Length) break;
				// Yの配列がiよりも小さかったらそこでキャンセルする。
				if (OpenNijiiroRW.Skin.Game_Effect_GoGoSplash_Rotate && OpenNijiiroRW.Tx.Effects_GoGoSplash != null)
				{
					// Switch文を使いたかったが、定数じゃないから使えねぇ!!!!
					if (i == 0)
					{
						OpenNijiiroRW.Tx.Effects_GoGoSplash.Rotation = -0.2792526803190927f;
					}
					else if (i == 1)
					{
						OpenNijiiroRW.Tx.Effects_GoGoSplash.Rotation = -0.13962634015954636f;
					}
					else if (i == OpenNijiiroRW.Skin.Game_Effect_GoGoSplash_X.Length - 2)
					{
						OpenNijiiroRW.Tx.Effects_GoGoSplash.Rotation = 0.13962634015954636f;
					}
					else if (i == OpenNijiiroRW.Skin.Game_Effect_GoGoSplash_X.Length - 1)
					{
						OpenNijiiroRW.Tx.Effects_GoGoSplash.Rotation = 0.2792526803190927f;
					}
					else
					{
						OpenNijiiroRW.Tx.Effects_GoGoSplash.Rotation = 0.0f;
					}
				}
				OpenNijiiroRW.Tx.Effects_GoGoSplash?.t2D拡大率考慮下中心基準描画(OpenNijiiroRW.Skin.Game_Effect_GoGoSplash_X[i], OpenNijiiroRW.Skin.Game_Effect_GoGoSplash_Y[i], new Rectangle(OpenNijiiroRW.Skin.Game_Effect_GoGoSplash[0] * Splash.CurrentValue, 0, OpenNijiiroRW.Skin.Game_Effect_GoGoSplash[0], OpenNijiiroRW.Skin.Game_Effect_GoGoSplash[1]));
			}
		}
		return base.Draw();
	}

	public void StartSplash()
	{
		Splash = new CCounter(0, OpenNijiiroRW.Skin.Game_Effect_GoGoSplash[2] - 1, OpenNijiiroRW.Skin.Game_Effect_GoGoSplash_Timer, OpenNijiiroRW.Timer);
	}

	private CCounter Splash;
}
