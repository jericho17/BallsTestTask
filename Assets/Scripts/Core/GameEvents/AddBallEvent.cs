using System;

public class AddBallEvent : GameEvent
{
	private int _ballId;
	private int _xPos;
	private int _yPos;
	private BallType _ballType;
	
	
	public AddBallEvent (int id, GameCore core, int ballId, BallType ballType, int xPos, int yPos)
		:base(id, GameEventType.AddBall, core)
	{
		_ballId = ballId;
		_ballType = ballType;
		_xPos = xPos;
		_yPos = yPos;
	}
	
	public override void Execute ()
	{
		Core.BallManager.AddBall (_ballId, _ballType, _xPos, _yPos);
	}
	
	
	public override string Serialize()
	{
		var type = (int)GameEventType.AddBall;
		var data = string.Join (";", new string[]
		                        {Id.ToString(),
			_ballId.ToString(),
			((int)_ballType).ToString(),
			_xPos.ToString(),
			_yPos.ToString()});
		
		return ParsingHelper.PackData(type, data);
	}
	public static GameEvent Deserialize(string serializedEvent, GameCore core)
	{
		var parts = serializedEvent.Substring(ParsingHelper.HeaderSize).Split (';');
		int id = int.Parse (parts [0]);
		int ballId = int.Parse (parts[1]);
		BallType ballType = (BallType)int.Parse (parts[2]);
		int xPos = int.Parse (parts[3]);
		int yPos = int.Parse (parts[4]);
		
		return new AddBallEvent(id, core, ballId, ballType, xPos, yPos);
	}
}

