using System;
public static class Transport
{
	public const int EventTypeLengh = 2;
	public const int PacketSizeLengh = 5;
	public const int HeaderSize = EventTypeLengh + PacketSizeLengh;

	public const int Port = 843;
	public const int PacketSize = 1024;
	public const string EndFlag = "#<EOF>";
	public const string UpdateFlag = "Update#";
	public const char Separator = '#';

	public static int UpdateFlagLength {get{return UpdateFlag.Length;}}
}