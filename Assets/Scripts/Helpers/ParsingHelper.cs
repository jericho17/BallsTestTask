using System;
using System.Text;


public static class ParsingHelper
{
	public const int DataLengthNumberSymbols = 5;
	public const int EventTypeNumberSymbols = 2;
	public static int HeaderSize{get{return DataLengthNumberSymbols + EventTypeNumberSymbols;}}
	
	public static string ip;
	
	public static int GetLength(string message, int startIndex)
	{
		var lengthString = message.Substring (startIndex, DataLengthNumberSymbols);
		var length = int.Parse (lengthString);
		return length;
	}
	
	public static GameEventType GetEventType(string message, int startIndex)
	{
		var eventTypeString = message.Substring (startIndex, EventTypeNumberSymbols);
		var eventTypeNumber = int.Parse (eventTypeString);
		return (GameEventType)eventTypeNumber;
	}
	
	/// <summary>
	/// Message format:
	/// 2 symbols - DataTypeNumber
	/// 5 symbols - DataLength
	/// other - data
	/// </summary>
	/// <returns>The data.</returns>
	/// <param name="dataType">Data type.</param>
	/// <param name="data">Data.</param>
	public static string PackData(int dataType, string data)
	{
		if (dataType > 99)
			throw new ArgumentOutOfRangeException("dataType");

		var sb = new StringBuilder ();
		sb.Append (string.Format ("{0:00}", dataType));				
		sb.Append (string.Format ("{0:00000}",data.Length));		
		sb.Append (data);
		
		return sb.ToString ();
	}
}