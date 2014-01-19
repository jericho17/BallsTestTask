using System;
using System.Linq;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallManager
{
	private BallFactory _ballFactory;
	private int _newBallId = 1;
	
	public List<Ball> Balls = new List<Ball>();
		
	public BallManager (BallFactory ballFactory)
	{
		_ballFactory = ballFactory;
	}
	 	
	public int GetNewBallId()
	{
		return _newBallId++;
	}    
	
	public void AddBall(int id, BallType ballType, int xPos, int yPos)
	{
		Balls.Add (_ballFactory.GetBall(ballType, id, xPos, yPos));
	}
	
	public void RemoveBall(int id)
	{
		var ball = Balls.SingleOrDefault (x => x.Id == id);
		if (ball == null) return;
		
		ball.Destroy();
		Balls.Remove (ball);
	}
	
	public void MoveBalls()
	{
		foreach (var ball in Balls) 
		{
			ball.Move();
		}
	}
}