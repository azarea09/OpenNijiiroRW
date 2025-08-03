using FDK;

namespace OpenNijiiroRW;

internal class CActFIFOBlack : CActivity {
	// メソッド

	public void tフェードアウト開始(int start = 0, int end = 100, int interval = 5) {
		this.mode = EFIFOMode.FadeOut;
		this.counter = new CCounter(start, end, interval, OpenNijiiroRW.Timer);
	}
	public void tフェードイン開始(int start = 0, int end = 100, int interval = 5) {
		this.mode = EFIFOMode.FadeIn;
		this.counter = new CCounter(start, end, interval, OpenNijiiroRW.Timer);
	}


	// CActivity 実装

	public override void DeActivate() {
		if (!base.IsDeActivated) {
			//CDTXMania.tテクスチャの解放( ref this.tx黒タイル64x64 );
			base.DeActivate();
		}
	}
	public override void CreateManagedResource() {
		//this.tx黒タイル64x64 = CDTXMania.tテクスチャの生成( CSkin.Path( @"Graphics\Tile black 64x64.png" ), false );
		base.CreateManagedResource();
	}
	public override int Draw() {
		if (base.IsDeActivated || (this.counter == null)) {
			return 0;
		}
		this.counter.Tick();
		// Size clientSize = CDTXMania.app.Window.ClientSize;	// #23510 2010.10.31 yyagi: delete as of no one use this any longer.
		if (OpenNijiiroRW.Tx.Tile_Black != null) {
			OpenNijiiroRW.Tx.Tile_Black.Opacity = (this.mode == EFIFOMode.FadeIn) ? (((100 - this.counter.CurrentValue) * 0xff) / 100) : ((this.counter.CurrentValue * 0xff) / 100);
			for (int i = 0; i <= (RenderSurfaceSize.Width / OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Width); i++)        // #23510 2010.10.31 yyagi: change "clientSize.Width" to "640" to fix FIFO drawing size
			{
				for (int j = 0; j <= (RenderSurfaceSize.Height / OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Height); j++)  // #23510 2010.10.31 yyagi: change "clientSize.Height" to "480" to fix FIFO drawing size
				{
					OpenNijiiroRW.Tx.Tile_Black.t2D描画(i * OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Width, j * OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Height);
				}
			}
		}
		if (this.counter.CurrentValue != this.counter.EndValue) {
			return 0;
		}
		return 1;
	}


	// その他

	#region [ private ]
	//-----------------
	private CCounter counter;
	private EFIFOMode mode;
	//private CTexture tx黒タイル64x64;
	//-----------------
	#endregion
}
