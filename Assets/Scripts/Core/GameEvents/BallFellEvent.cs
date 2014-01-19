using System;

public class BallFellEvent : GameEvent
{
	private int _ballId;
	
	public BallFellEvent (int id, GameCore core, int ballId) 
		: base(id, GameEventType.BallFell, core)
	{
		_ballId = ballId;
	}
	
	public override void Execute()
	{
		Core.BallManager.RemoveBall(_ballId);
	}
	
	public override string Serialize()
	{
		var type = (int)GameEventType.BallFell;
		var data = Id+";"+_ballId;
		
		return ParsingHelper.PackData(type, data);
	}
	public static GameEvent Deserialize(string serializedEvent, GameCore core)
	{
		var parts = serializedEvent.Substring(ParsingHelper.HeaderSize).Split (';');
		int id = int.Parse (parts [0]);
		int ballId = int.Parse (parts[1]);
		
		return new BallFellEvent(id, core, ballId);
	}
}