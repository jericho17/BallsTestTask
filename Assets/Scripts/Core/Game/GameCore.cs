using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameCore
{
	public List<GameEvent> GameEvents = new List<GameEvent>();
	public List<GameEvent> ProcessedGameEvents = new List<GameEvent> ();
	public BallManager BallManager;
	public bool GameEnded;
	
	public int Score;
	public int LastEventId;
	private int _newEventId = 0;
	
	public GameCore (BallManager ballManager)
	{
		GameEnded = false;
		BallManager = ballManager;	
	}
	
	
	public void AddEvent(GameEvent gameEvent)
	{
		GameEvents.Add (gameEvent);
		LastEventId = gameEvent.Id;
	}
	
	public int GetNewEventId()
	{
		return _newEventId++;
	}
	
	public void Update()
	{
		BallManager.MoveBalls ();
		
		ProccesGameEvents ();
	}
	
	public void ProcessBallClick(int id)
	{
		var ball = BallManager.Balls.SingleOrDefault (x=>x.Id == id);
		if (ball != null) 
		{
			Score += ball.Score;
			BallManager.RemoveBall(id);
		}
	}
	
	public void EndGame()
	{
		GameEnded = true;
	}
	
	void ProccesGameEvents ()
	{
		if (GameEvents.Any ()) 
		{
			foreach (var gameEvent in GameEvents) 
			{
				gameEvent.Execute ();
				ProcessedGameEvents.Add(gameEvent);
			}
		}
		
		GameEvents.Clear ();
	}
}