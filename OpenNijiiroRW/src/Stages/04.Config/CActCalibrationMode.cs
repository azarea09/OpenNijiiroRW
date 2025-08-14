using System.Drawing;
using FDK;

namespace OpenNijiiroRW;

internal class CActCalibrationMode : CActivity
{
	public CActCalibrationMode() { }

	public override void Activate()
	{
		//hitSound = TJAPlayer3.SoundManager.tCreateSound($@"Global{Path.DirectorySeparatorChar}HitSounds{Path.DirectorySeparatorChar}" + TJAPlayer3.Skin.hsHitSoundsInformations.don[0], ESoundGroup.SoundEffect);
		font = HPrivateFastFont.tInstantiateMainFont(OpenNijiiroRW.Skin.Config_Calibration_Font_Scale);
		base.Activate();
	}

	public override void DeActivate()
	{
		Stop();
		Offsets.Clear();
		font?.Dispose();
		offsettext?.Dispose();
		//hitSound?.tDispose();

		base.DeActivate();
	}

	public void Start()
	{
		CalibrateTick = new CCounter(0, 500, 1, OpenNijiiroRW.Timer);
		UpdateText();
	}

	public void Stop()
	{
		CalibrateTick = new CCounter();
		Offsets.Clear();
		LastOffset = 0;
		buttonIndex = 1;
	}

	public int Update()
	{
		if (IsDeActivated || CalibrateTick.IsStoped)
			return 1;

		CalibrateTick.Tick();

		bool decide = OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.Decide) ||
					  OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.RRed) ||
					  OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.LRed) ||
					  OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Return);

		if (CalibrateTick.IsEnded)
		{
			OpenNijiiroRW.Skin.calibrationTick.tPlay();
			CalibrateTick.Start(0, 500, 1, OpenNijiiroRW.Timer);
		}

		if (OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.LeftChange) ||
			OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.LBlue) ||
			OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.LeftArrow))
		{
			buttonIndex = Math.Max(buttonIndex - 1, 0);
			OpenNijiiroRW.Skin.soundChangeSFX.tPlay();
		}
		else if (OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.RightChange) ||
				   OpenNijiiroRW.Pad.bPressed(EInstrumentPad.Drums, EPad.RBlue) ||
				   OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.RightArrow))
		{
			buttonIndex = Math.Min(buttonIndex + 1, 2);
			OpenNijiiroRW.Skin.soundChangeSFX.tPlay();
		}
		else if (buttonIndex == 0 && decide) // Cancel
		{
			OpenNijiiroRW.Skin.soundCancelSFX.tPlay();
			Stop();
		}
		else if (buttonIndex == 1 && decide) // Hit!
		{
			//hitSound?.PlayStart();
			AddOffset();
			UpdateText();
		}
		else if (buttonIndex == 2 && decide) // Save
		{
			OpenNijiiroRW.ConfigIni.nGlobalOffsetMs = GetMedianOffset();
			OpenNijiiroRW.stageConfig.actList.iGlobalOffsetMs.n現在の値 = GetMedianOffset();
			OpenNijiiroRW.Skin.soundDecideSFX.tPlay();
			Stop();

			return 0;
		}
		else if (OpenNijiiroRW.ConfigIni.KeyAssign.KeyIsPressed(OpenNijiiroRW.ConfigIni.KeyAssign.System.Cancel) ||
				   OpenNijiiroRW.InputManager.Keyboard.KeyPressed((int)SlimDXKeys.Key.Escape))
		{
			OpenNijiiroRW.Skin.soundCancelSFX.tPlay();
			Stop();

			return 0;
		}

		return 0;
	}

	public override int Draw()
	{
		if (IsDeActivated || CalibrateTick.IsStoped)
			return 1;

		if (OpenNijiiroRW.Tx.Tile_Black != null)
		{
			OpenNijiiroRW.Tx.Tile_Black.Opacity = 128;
			for (int i = 0; i <= RenderSurfaceSize.Width; i += OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Width)
			{
				for (int j = 0; j <= RenderSurfaceSize.Height; j += OpenNijiiroRW.Tx.Tile_Black.szTextureSize.Height)
				{
					OpenNijiiroRW.Tx.Tile_Black.t2D描画(i, j);
				}
			}
			OpenNijiiroRW.Tx.Tile_Black.Opacity = 255;
		}

		OpenNijiiroRW.Tx.CalibrateBG?.t2D描画(OpenNijiiroRW.Skin.Config_Calibration_Highlights[buttonIndex].X,
			OpenNijiiroRW.Skin.Config_Calibration_Highlights[buttonIndex].Y,
			OpenNijiiroRW.Skin.Config_Calibration_Highlights[buttonIndex]);
		OpenNijiiroRW.Tx.CalibrateFG?.t2D描画(0, 0);

		//OpenNijiiroRW.Tx.Lane_Background_Main?.t2D描画(OpenNijiiroRW.Skin.Game_Lane_X[0], OpenNijiiroRW.Skin.Game_Lane_Y[0]);
		//OpenNijiiroRW.Tx.Lane_Background_Sub?.t2D描画(OpenNijiiroRW.Skin.Game_Lane_Sub_X[0], OpenNijiiroRW.Skin.Game_Lane_Sub_Y[0]);
		//OpenNijiiroRW.Tx.Taiko_Frame[2]?.t2D描画(OpenNijiiroRW.Skin.Game_Taiko_Frame_X[0], OpenNijiiroRW.Skin.Game_Taiko_Frame_Y[0]);

		OpenNijiiroRW.Tx.Notes[0]?.t2D描画(OpenNijiiroRW.Skin.nScrollFieldX[0], OpenNijiiroRW.Skin.nScrollFieldY[0], new RectangleF(0, 0, OpenNijiiroRW.Skin.Game_Notes_Size[0], OpenNijiiroRW.Skin.Game_Notes_Size[1]));

		for (int x = OpenNijiiroRW.Skin.nScrollFieldX[0]; x < RenderSurfaceSize.Width + 500; x += 500)
		{
			OpenNijiiroRW.Tx.Bar?.t2D描画(
				(x - CalibrateTick.CurrentValue) + ((OpenNijiiroRW.Skin.Game_Notes_Size[0] - OpenNijiiroRW.Tx.Bar.szTextureSize.Width) / 2),
				OpenNijiiroRW.Skin.nScrollFieldY[0],
				new Rectangle(0, 0, OpenNijiiroRW.Tx.Bar.szTextureSize.Width, OpenNijiiroRW.Skin.Game_Notes_Size[1])
			);
			OpenNijiiroRW.Tx.Notes[0]?.t2D描画(
				(x - CalibrateTick.CurrentValue),
				OpenNijiiroRW.Skin.nScrollFieldY[0],
				new Rectangle(OpenNijiiroRW.Skin.Game_Notes_Size[0], OpenNijiiroRW.Skin.Game_Notes_Size[1], OpenNijiiroRW.Skin.Game_Notes_Size[0], OpenNijiiroRW.Skin.Game_Notes_Size[1])
			);
		}

		if (OpenNijiiroRW.P1IsBlue())
			OpenNijiiroRW.Tx.Taiko_Background[1]?.t2D描画(OpenNijiiroRW.Skin.Game_Taiko_Background_X[0], OpenNijiiroRW.Skin.Game_Taiko_Background_Y[0]);
		else
			OpenNijiiroRW.Tx.Taiko_Background[0]?.t2D描画(OpenNijiiroRW.Skin.Game_Taiko_Background_X[0], OpenNijiiroRW.Skin.Game_Taiko_Background_Y[0]);

		#region Calibration Info

		offsettext?.t2D描画(OpenNijiiroRW.Skin.Config_Calibration_OffsetText[0] - offsettext.szTextureSize.Width, OpenNijiiroRW.Skin.Config_Calibration_OffsetText[1]);

		int xInfo = OpenNijiiroRW.Skin.Config_Calibration_InfoText[0];
		int yInfo = OpenNijiiroRW.Skin.Config_Calibration_InfoText[1];
		yInfo = OpenNijiiroRW.actTextConsole.Print(xInfo, yInfo, CTextConsole.EFontType.Cyan,
			"MEDIAN OFFSET : " + GetMedianOffset() + "ms\n").y;
		yInfo = OpenNijiiroRW.actTextConsole.Print(xInfo, yInfo, CTextConsole.EFontType.White,
			"MIN OFFSET    : " + GetLowestOffset() + "ms\n" +
			"MAX OFFSET    : " + GetHighestOffset() + "ms\n" +
			"LAST OFFSET   : " + LastOffset + "ms\n" +
			"OFFSET COUNT  : " + (Offsets != null ? Offsets.Count : 0) + "\n").y;
		OpenNijiiroRW.actTextConsole.Print(xInfo, yInfo, CTextConsole.EFontType.White,
			"CURRENT OFFSET: " + CurrentOffset() + "ms");

		#endregion

		return 0;
	}

	public void AddOffset() { Offsets.Add(CurrentOffset()); LastOffset = CurrentOffset(); }

	public int GetMedianOffset()
	{
		if (Offsets != null)
			if (Offsets.Count > 0)
			{
				Offsets.Sort();
				return Offsets[Offsets.Count / 2];
			}
		return 0;
	}
	public int GetLowestOffset()
	{
		if (Offsets != null)
			return Offsets.Count > 0 ? Offsets.Min() : 0;
		return 0;
	}
	public int GetHighestOffset()
	{
		if (Offsets != null)
			return Offsets.Count > 0 ? Offsets.Max() : 0;
		return 0;
	}
	public int CurrentOffset()
	{
		return -(CalibrateTick.CurrentValue > 250 ? CalibrateTick.CurrentValue - 500 : CalibrateTick.CurrentValue);
	}

	private void UpdateText()
	{
		offsettext?.Dispose();
		offsettext = new CTexture(font.DrawText(CLangManager.LangInstance.GetString("SETTINGS_GAME_CALIBRATION_OFFSET", GetMedianOffset().ToString()), Color.White, Color.Black, null, 32));
	}

	public bool IsStarted { get { return CalibrateTick.IsStarted; } }
	#region Private
	private CCounter CalibrateTick = new CCounter();
	private List<int> Offsets = new List<int>();
	private int LastOffset = 0;
	private CCachedFontRenderer font;
	private CTexture offsettext;

	//private CSound hitSound;

	private int buttonIndex = 1;
	private Rectangle[] BGs = new Rectangle[3]
	{
		new Rectangle(371, 724, 371, 209),
		new Rectangle(774, 724, 371, 209),
		new Rectangle(1179, 724, 371, 209)
	};
	#endregion
}
