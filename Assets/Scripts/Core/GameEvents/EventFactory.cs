using System;
public class EventFactory
{
	private GameCore _core;
	
	
	public EventFactory (GameCore core)
	{
		_core = core;
	}
	
	
	public GameEvent GetAddBallEvent(BallType ballType, int xPos, int yPos)
	{
		var addBallEvent = new AddBallEvent (_core.GetNewEventId(),
		                                     _core, 
		                                     _core.BallManager.GetNewBallId(),
		                                     ballType,
		                                     xPos,
		                                     yPos);
		return addBallEvent;
	}
	
	public GameEvent GetBallFellEvent(int ballId)
	{
		var ballFellEvent = new BallFellEvent (_core.GetNewEventId(),
		                                       _core,
		                                       ballId);
		return ballFellEvent;
	}
	
	public GameEvent GetClickBallEvent(int ballId)
	{
		var clickBallId = new ClickBallEvent (_core.GetNewEventId(),
		                                      _core,
		                                      ballId);
		return clickBallId;
	}
	
	public GameEvent GetEndGameEvent()
	{
		var endGameEvent = new EndGameEvent (_core.GetNewEventId(), _core);
		return endGameEvent;
	}
	
	
	public GameEvent DeserializeGameEvent(string serializedEvent)
	{
		GameEvent gameEvent;
		
		var eventType = ParsingHelper.GetEventType (serializedEvent, 0);
		switch (eventType) 
		{
		case GameEventType.AddBall:
			gameEvent = AddBallEvent.Deserialize(serializedEvent, _core);
			break;
		case GameEventType.BallFell:
			gameEvent = BallFellEvent.Deserialize(serializedEvent, _core);
			break;
		case GameEventType.ClickBall:
			gameEvent = ClickBallEvent.Deserialize(serializedEvent, _core);
			break;
		case GameEventType.EndGame:
			gameEvent = EndGameEvent.Deserialize(serializedEvent, _core);
			break;
		default:
			throw new ArgumentOutOfRangeException("GameEventType");
		}
		
		return gameEvent;
	}
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
	
}