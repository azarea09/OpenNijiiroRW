namespace OpenNijiiroRW
{
	public static class HBlackBackdrop
	{
		public static void Draw(int opacity = 255)
		{
			if (OpenNijiiroRW.Tx.Tile_Black != null)
			{
				OpenNijiiroRW.Tx.Tile_Black.Opacity = opacity;
				for (int i = 0; i <= FDK.RenderSurfaceSize.Width; i += OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Width)
				{
					for (int j = 0; j <= FDK.RenderSurfaceSize.Height; j += OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Height)
					{
						OpenNijiiroRW.Tx.Tile_Black.t2D描画(i, j);
					}
				}
				OpenNijiiroRW.Tx.Tile_Black.Opacity = 255;
			}
		}
	}
}
