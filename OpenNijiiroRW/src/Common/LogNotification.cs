using System.Diagnostics;

namespace OpenNijiiroRW;

internal class LogNotification
{
	public static void PopError(string message)
	{
		OpenNijiiroRW.VisualLogManager?.PushCard(TraceEventType.Error, message);
		Trace.TraceError("<Runtime Error>: " + message);
	}

	public static void PopWarning(string message)
	{
		OpenNijiiroRW.VisualLogManager?.PushCard(TraceEventType.Warning, message);
		Trace.TraceWarning("<Runtime Warning>: " + message);
	}

	public static void PopSuccess(string message)
	{
		OpenNijiiroRW.VisualLogManager?.PushCard(TraceEventType.Verbose, message);
		Trace.TraceInformation("<Runtime Success>: " + message);
	}

	public static void PopInfo(string message)
	{
		OpenNijiiroRW.VisualLogManager?.PushCard(TraceEventType.Information, message);
		Trace.TraceInformation("<Runtime Info>: " + message);
	}
}
