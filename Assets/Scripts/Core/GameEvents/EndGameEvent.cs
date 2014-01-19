using System;

public class EndGameEvent : GameEvent
{
	public EndGameEvent (int id, GameCore core) 
		: base(id, GameEventType.EndGame, core)
	{
	}
	
	public override void Execute()
	{
		Core.EndGame ();
	}
	
	public override string Serialize()
	{
		var type = (int)GameEventType.EndGame;
		var data = Id.ToString();
		
		return ParsingHelper.PackData(type, data);
	}
	public static GameEvent Deserialize(string serializedEvent, GameCore core)
	{
		var parts = serializedEvent.Substring(ParsingHelper.HeaderSize).Split (';');
		int id = int.Parse(parts [0]);
		
		return new EndGameEvent(id, core);
	}
}