using FDK;
using Silk.NET.Maths;
using Rectangle = System.Drawing.Rectangle;

namespace OpenNijiiroRW;

class PuchiChara : CActivity
{
	public PuchiChara()
	{
		base.IsDeActivated = true;
	}

	public override void Activate()
	{
		Counter = new CCounter(0, OpenNijiiroRW.Skin.Game_PuchiChara[2] - 1, OpenNijiiroRW.Skin.Game_PuchiChara_Timer * 0.5f, OpenNijiiroRW.Timer);
		SineCounter = new CCounter(0, 360, OpenNijiiroRW.Skin.Game_PuchiChara_SineTimer, SoundManager.PlayTimer);
		SineCounterIdle = new CCounter(1, 360, (float)OpenNijiiroRW.Skin.Game_PuchiChara_SineTimer * 2f, OpenNijiiroRW.Timer);
		this.inGame = false;
		base.Activate();
	}
	public override void DeActivate()
	{
		Counter = null;
		SineCounter = null;
		SineCounterIdle = null;
		base.DeActivate();
	}

	public static int tGetPuchiCharaIndexByName(int p)
	{
		var _pc = OpenNijiiroRW.SaveFileInstances[p].data.PuchiChara;
		var _pcs = OpenNijiiroRW.Skin.Puchicharas_Name;
		int puriChar = 0;
		if (_pcs.Contains(_pc))
			puriChar = _pcs.ToList().IndexOf(_pc);

		return puriChar;
	}

	public void ChangeBPM(double secPerBeat)
	{
		Counter = new CCounter(0, OpenNijiiroRW.Skin.Game_PuchiChara[2] - 1, (int)(OpenNijiiroRW.Skin.Game_PuchiChara_Timer * secPerBeat / OpenNijiiroRW.Skin.Game_PuchiChara[2]), OpenNijiiroRW.Timer);
		SineCounter = new CCounter(1, 360, OpenNijiiroRW.Skin.Game_PuchiChara_SineTimer * secPerBeat / 180, SoundManager.PlayTimer);
		this.inGame = true;
	}

	public void IdleAnimation()
	{
		this.inGame = false;
	}

	/// <summary>
	/// Draws Puchi Chara (small character) on the screen. Note: this is not an override.
	/// </summary>
	/// <param name="x">X coordinate (center)</param>
	/// <param name="y">Y coordinate (center)</param>
	/// <param name="alpha">Opacity (0-255)</param>
	/// <returns></returns>
	public int On進行描画(int x, int y, bool isGrowing, int alpha = 255, bool isBalloon = false, int player = 0, float scale = 1.0f)
	{
		if (!OpenNijiiroRW.ConfigIni.ShowPuchiChara) return base.Draw();
		if (Counter == null || SineCounter == null || OpenNijiiroRW.Tx.Puchichara == null) return base.Draw();
		Counter.TickLoop();
		SineCounter.TickLoopDB();
		SineCounterIdle.TickLoop();

		int p = OpenNijiiroRW.GetActualPlayer(player);

		if (inGame)
			sineY = (double)SineCounter.CurrentValue;
		else
			sineY = (double)SineCounterIdle.CurrentValue;

		sineY = Math.Sin(sineY * (Math.PI / 180)) * (OpenNijiiroRW.Skin.Game_PuchiChara_Sine * (isBalloon ? OpenNijiiroRW.Skin.Game_PuchiChara_Scale[1] : OpenNijiiroRW.Skin.Game_PuchiChara_Scale[0]));

		int puriChar = PuchiChara.tGetPuchiCharaIndexByName(p);
		var chara = OpenNijiiroRW.Tx.Puchichara[puriChar].tx;

		if (chara != null)
		{
			float puchiScale = OpenNijiiroRW.Skin.Resolution[1] / 720.0f;

			chara.Scale = new Vector3D<float>((isBalloon ? OpenNijiiroRW.Skin.Game_PuchiChara_Scale[1] * puchiScale : OpenNijiiroRW.Skin.Game_PuchiChara_Scale[0] * puchiScale));
			chara.Scale.X *= scale;
			chara.Scale.Y *= scale;
			chara.Opacity = alpha;

			/* Todo :
			 **
			 ** - Yellow light color filter when isGrowing is true
			 */

			int adjustedX = x - 32;
			int adjustedY = y - 32;

			chara.t2D拡大率考慮中央基準描画(adjustedX, adjustedY + (int)sineY, new Rectangle((Counter.CurrentValue % OpenNijiiroRW.Skin.Game_PuchiChara[2]) * OpenNijiiroRW.Skin.Game_PuchiChara[0], 0, OpenNijiiroRW.Skin.Game_PuchiChara[0], OpenNijiiroRW.Skin.Game_PuchiChara[1]));
		}

		return base.Draw();
	}

	public void DrawPuchichara(int index, int x, int y, float scale = 1.0f, int alpha = 255, bool useSine = true)
	{
		DrawPuchichara(index, x, y, Counter.CurrentValue % OpenNijiiroRW.Skin.Game_PuchiChara[2], scale, alpha, useSine);
	}
	public void DrawPuchichara(int index, int x, int y, int sprite, float scale = 1.0f, int alpha = 255, bool useSine = true)
	{
		if (OpenNijiiroRW.Tx.Puchichara.Length <= index || index < 0) return;
		if (OpenNijiiroRW.Tx.Puchichara[index].tx == null) return;

		CTexture puchi = OpenNijiiroRW.Tx.Puchichara[index].tx;

		puchi.Scale = new(1);
		puchi.Scale.X *= scale;
		puchi.Scale.Y *= scale;
		puchi.Opacity = alpha;

		sineY = (double)SineCounterIdle.CurrentValue;
		sineY = (Math.Sin(sineY * (Math.PI / 180)) * (OpenNijiiroRW.Skin.Game_PuchiChara_Sine * OpenNijiiroRW.Skin.Game_PuchiChara_Scale[0]));

		puchi.t2D拡大率考慮中央基準描画(x, y + (useSine ? (int)sineY : 0),
			new((puchi.szTextureSize.Width / OpenNijiiroRW.Skin.Game_PuchiChara[2]) * sprite, 0,
			puchi.szTextureSize.Width / OpenNijiiroRW.Skin.Game_PuchiChara[2], puchi.szTextureSize.Height));

		puchi.Scale = new(1);
		puchi.Opacity = 255;
	}

	public double sineY;

	public CCounter Counter;
	private CCounter SineCounter;
	private CCounter SineCounterIdle;
	private bool inGame;
}
