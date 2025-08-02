using FDK;

namespace OpenNijiiroRW;

internal class CAct演奏演奏情報 : CActivity {
	// Properties

	public double[] dbBPM = new double[5];
	public readonly int[] NowMeasure = new int[5];
	public double dbSCROLL;
	public int[] _chipCounts = new int[2];

	// コンストラクタ

	public CAct演奏演奏情報() {
		base.IsDeActivated = true;
	}


	// CActivity 実装

	public override void Activate() {
		for (int i = 0; i < 5; i++) {
			NowMeasure[i] = 0;
			this.dbBPM[i] = OpenNijiiroRW.TJA.BASEBPM;
		}
		this.dbSCROLL = 1.0;

		_chipCounts[0] = OpenNijiiroRW.TJA.listChip.Where(num => NotesManager.IsMissableNote(num)).Count();
		_chipCounts[1] = OpenNijiiroRW.TJA.listChip_Branch[2].Where(num => NotesManager.IsMissableNote(num)).Count();

		NotesTextN = string.Format("NoteN:         {0:####0}", OpenNijiiroRW.TJA.nノーツ数_Branch[0]);
		NotesTextE = string.Format("NoteE:         {0:####0}", OpenNijiiroRW.TJA.nノーツ数_Branch[1]);
		NotesTextM = string.Format("NoteM:         {0:####0}", OpenNijiiroRW.TJA.nノーツ数_Branch[2]);
		NotesTextC = string.Format("NoteC:         {0:####0}", OpenNijiiroRW.TJA.nノーツ数[3]);
		ScoreModeText = string.Format("SCOREMODE:     {0:####0}", OpenNijiiroRW.stageGameScreen.scoreMode[0]);
		ListChipText = string.Format("ListChip:      {0:####0}", _chipCounts[0]);
		ListChipMText = string.Format("ListChipM:     {0:####0}", _chipCounts[1]);

		base.Activate();
	}
	public override int Draw() {
		int dx = OpenNijiiroRW.actTextConsole.fontWidth;
		int dy = OpenNijiiroRW.actTextConsole.fontHeight;
		int x = OpenNijiiroRW.Skin.Resolution[0] - 8 - 34 * dx;
		int y = 404 * OpenNijiiroRW.Skin.Resolution[1] / 720;
		if (!base.IsDeActivated) {
			y += (13 - 1) * dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, string.Format("Song/G. Offset:{0:####0}/{1:####0} ms", OpenNijiiroRW.TJA.nBGMAdjust, OpenNijiiroRW.ConfigIni.nGlobalOffsetMs));
			y -= dy;
			int num = (OpenNijiiroRW.TJA.listChip.Count > 0) ? OpenNijiiroRW.TJA.listChip[OpenNijiiroRW.TJA.listChip.Count - 1].n発声時刻ms : 0;
			string str = "Time:          " + (OpenNijiiroRW.TJA.GameTimeToTjaTime(SoundManager.PlayTimer.NowTimeMs) / 1000.0).ToString("####0.00") + " / " + ((((double)num) / 1000.0)).ToString("####0.00");
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, str);
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, string.Format("Part:          {0:####0}/{1:####0}", NowMeasure[0], NowMeasure[1]));
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, string.Format("BPM:           {0:####0.0000}", this.dbBPM[0]));
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, string.Format("Frame:         {0:####0} fps", OpenNijiiroRW.FPS.NowFPS));
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, NotesTextN);
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, NotesTextE);
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, NotesTextM);
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, NotesTextC);
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, string.Format("SCROLL:        {0:####0.00}", this.dbSCROLL));
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, ScoreModeText);
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, ListChipText);
			y -= dy;
			OpenNijiiroRW.actTextConsole.Print(x, y, CTextConsole.EFontType.White, ListChipMText);

			//CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "Sound CPU :    {0:####0.00}%", CDTXMania.Sound管理.GetCPUusage() ) );
			//y -= dy;
			//CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "Sound Mixing:  {0:####0}", CDTXMania.Sound管理.GetMixingStreams() ) );
			//y -= dy;
			//CDTXMania.act文字コンソール.tPrint( x, y, C文字コンソール.Eフォント種別.白, string.Format( "Sound Streams: {0:####0}", CDTXMania.Sound管理.GetStreams() ) );
			//y -= dy;
		}
		return 0;
	}

	private string NotesTextN;
	private string NotesTextE;
	private string NotesTextM;
	private string NotesTextC;
	private string ScoreModeText;
	private string ListChipText;
	private string ListChipMText;
}
