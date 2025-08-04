using FDK;

namespace OpenNijiiroRW;

internal class CActResultSongBar : CActivity
{
	// コンストラクタ

	public CActResultSongBar()
	{
		base.IsDeActivated = true;
	}


	// メソッド

	public void tアニメを完了させる()
	{
		this.ct登場用.CurrentValue = (int)this.ct登場用.EndValue;
	}


	// CActivity 実装

	public override void Activate()
	{

		var title = OpenNijiiroRW.TJA.TITLE.GetString("");

		using (var bmpSongTitle = pfMusicName.DrawText(title, OpenNijiiroRW.Skin.Result_MusicName_ForeColor, OpenNijiiroRW.Skin.Result_MusicName_BackColor, null, 30))
		{
			this.txMusicName = OpenNijiiroRW.tテクスチャの生成(bmpSongTitle, false);
			txMusicName.Scale.X = OpenNijiiroRW.GetSongNameXScaling(ref txMusicName, OpenNijiiroRW.Skin.Result_MusicName_MaxSize);
		}

		base.Activate();
	}
	public override void DeActivate()
	{
		if (this.ct登場用 != null)
		{
			this.ct登場用 = null;
		}

		OpenNijiiroRW.tテクスチャの解放(ref this.txMusicName);
		base.DeActivate();
	}
	public override void CreateManagedResource()
	{
		this.pfMusicName = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.Result_MusicName_FontSize);
		base.CreateManagedResource();
	}
	public override void ReleaseManagedResource()
	{
		OpenNijiiroRW.tDisposeSafely(ref this.pfMusicName);
		base.ReleaseManagedResource();
	}
	public override int Draw()
	{
		if (base.IsDeActivated)
		{
			return 0;
		}
		if (base.IsFirstDraw)
		{
			this.ct登場用 = new CCounter(0, 270, 4, OpenNijiiroRW.Timer);
			base.IsFirstDraw = false;
		}
		this.ct登場用.Tick();

		if (OpenNijiiroRW.Skin.Result_MusicName_ReferencePoint == CSkin.ReferencePoint.Center)
		{
			this.txMusicName.t2D描画(OpenNijiiroRW.Skin.Result_MusicName_X - ((this.txMusicName.szTextureSize.Width * txMusicName.Scale.X) / 2), OpenNijiiroRW.Skin.Result_MusicName_Y);
		}
		else if (OpenNijiiroRW.Skin.Result_MusicName_ReferencePoint == CSkin.ReferencePoint.Left)
		{
			this.txMusicName.t2D描画(OpenNijiiroRW.Skin.Result_MusicName_X, OpenNijiiroRW.Skin.Result_MusicName_Y);
		}
		else
		{
			this.txMusicName.t2D描画(OpenNijiiroRW.Skin.Result_MusicName_X - this.txMusicName.szTextureSize.Width * txMusicName.Scale.X, OpenNijiiroRW.Skin.Result_MusicName_Y);
		}

		if (!this.ct登場用.IsEnded)
		{
			return 0;
		}
		return 1;
	}


	// その他

	#region [ private ]
	//-----------------
	private CCounter ct登場用;

	private CTexture txMusicName;
	private CCachedFontRenderer pfMusicName;
	//-----------------
	#endregion
}
