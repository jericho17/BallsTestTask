using System;

public class GameController : IGameController
{
	private GameCore _gameCore;
	private EventFactory _eventFactory;
	
	public GameCore GameCore {get{return _gameCore;}}
	
	public GameController (GameCore gameCore, EventFactory eventFactory)
	{
		_eventFactory = eventFactory;
		_gameCore = gameCore;
	}
	
	public void AddGameEvent(GameEvent gameEvent)
	{
		_gameCore.AddEvent (gameEvent);
	}
	
	public void AddGameEvent(string serializedEvent)
	{
		_gameCore.AddEvent (_eventFactory.DeserializeGameEvent(serializedEvent));
	}
	
	public void CreateNewGame()
	{
		throw new NotImplementedException ("HELL LOGIC LIVES THERE");	
	}
	
	public void EndGame(bool hasErrors)
	{
		if (hasErrors)
			Configurator.Instance.HasErrors = true;
		
		_gameCore.EndGame ();
		ViewManager.BackToMenu ();
	}
	
	public void BallClick(string ballName)
	{
		var ballId = int.Parse (ballName);
		AddGameEvent (_eventFactory.GetClickBallEvent (ballId));
	}
	
	public void Update()
	{
		_gameCore.Update ();
	}
}