using System;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

public class ClickBallEvent : GameEvent
{
	private int _ballId;
	
	public ClickBallEvent (int id, GameCore core, int ballId)
		: base(id, GameEventType.ClickBall, core)
	{
		_ballId = ballId;
	}
	
	public override void Execute ()
	{
		Core.ProcessBallClick (_ballId);
	}
	
	public override string Serialize()
	{
		var type = (int)GameEventType.ClickBall;
		var data = Id+";"+_ballId;
		
		return ParsingHelper.PackData(type, data);
	}
	public static GameEvent Deserialize(string serializedEvent, GameCore core)
	{
		var parts = serializedEvent.Substring(ParsingHelper.HeaderSize).Split (';');
		int id = int.Parse (parts [0]);
		int ballId = int.Parse (parts[1]);
		
		return new ClickBallEvent(id, core, ballId);
	}
}