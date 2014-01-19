using System;

public class Environment
{
	private IGameController _gameController;
	private EventFactory _eventFactory;
	private Random _rnd;
	private int _balls;
	private int _ballProbability;
	
	private int _lastRequestNum;
	
	public Environment (IGameController gameController, EventFactory eventFactory)
	{
		_gameController = gameController;
		_eventFactory = eventFactory;
		_rnd = new Random (DateTime.Now.Millisecond);
		_ballProbability = 12;
	}
	
	public void Update()
	{
		if (Processed ())
			return;
		
		CheckBalls ();
		TryAddBalls ();
	}
	
	private void CheckBalls()
	{
		foreach (var ball in _gameController.GameCore.BallManager.Balls) 
		{
			if (ball.Avatar.transform.position.y<-1)
				_gameController.AddGameEvent(_eventFactory.GetBallFellEvent(ball.Id));
		}
	}
	
	private void TryAddBalls()
	{
		if (GotChance ()) 
		{
			AddBall ();
			_balls++;
			
			if(_balls%20==19)
			{
				_ballProbability++;			
			}
		}
	}
	
	//We will process update only every 100ms for ease core load
	private bool Processed()
	{
		var requestNum = (int)(UnityEngine.Time.timeSinceLevelLoad * 10);
		if (_lastRequestNum == requestNum)
			return true;
		
		_lastRequestNum = requestNum;
		return false;
	}
	
	private bool GotChance()
	{
		var number = _rnd.Next (0, 100);
		return number <= _ballProbability;
	}
	
	public void AddBall()
	{
		var addBallEvent = 
			_eventFactory.GetAddBallEvent (GetBallType(), _rnd.Next(2,20), 14);
		_gameController.AddGameEvent (addBallEvent);
	}
	
	private BallType GetBallType()
	{
		var number = _rnd.Next (0, 3);
		return (BallType)number;
	}
}