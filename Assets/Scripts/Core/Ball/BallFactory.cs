using System;
using UnityEngine;

public class BallFactory
{
	private TextureContainer _textureContainer;
	
	public BallFactory (TextureContainer textureContainer)
	{
		_textureContainer = textureContainer;
	}
	
	public Ball GetBall(BallType ballType, int ballId, int xPos, int yPos)
	{
		Ball ball;
		switch (ballType)
		{
		case BallType.Small:
			ball = GetSmall();
			break;
		case BallType.Normal:
			ball = GetNormal();
			break;
		case BallType.Big:
			ball = GetBig();
			break;
		default:
			throw new ArgumentException("BallType");
		}
		
		ball.Id = ballId;
		ball.Avatar.name = ballId.ToString();
		ball.Avatar.transform.position = new Vector3(xPos, yPos, 0);
		
		return ball;
	}
	
	public Ball GetSmall()
	{
		var ball = GetBallTemplate(BallType.Small, 200);
		AddSprite(ball, _textureContainer.GetBall32 ());
		return ball;
	}
	
	public Ball GetNormal()
	{
		var ball = GetBallTemplate (BallType.Normal, 100);
		AddSprite(ball, _textureContainer.GetBall64 ());	
		return ball;
	}
	
	public Ball GetBig()
	{
		var ball = GetBallTemplate(BallType.Big, 50);
		AddSprite(ball, _textureContainer.GetBall96 ());
		return ball;
	}

	public Ball GetVeryBig()
	{
		var ball = GetBallTemplate(BallType.Big, 25);
		AddSprite(ball, _textureContainer.GetBall128 ());
		return ball;
	}
	
	private Ball GetBallTemplate(BallType ballType, int score)
	{		
		var avatar = new GameObject ("Ball");
		var ball = new Ball (ballType, avatar, score);

		avatar.transform.position = new Vector3 (5, 15, 0);		

		return ball;
	}
	
	private void AddSprite(Ball ball, Sprite sprite)
	{
		var sr = ball.Avatar.AddComponent<SpriteRenderer> ();
		sr.sprite = sprite;
		sr.transform.localScale = new Vector3 (1,1,0);
		sr.sortingOrder = 1;	
		
		ball.Avatar.AddComponent<BoxCollider> ();
	}
}