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
		var ball = GetBallTemplate();
		ball.BallType = BallType.Small;
		ball.Score = 15;
		//Hack to repail boxcollider size :( its too BIG
		//var col = ball.Avatar.GetComponent<BoxCollider> ();
		//col.size = new Vector2 (0.3f,0.3f);
		AddSprite(ball, _textureContainer.GetBall32 ());
		return ball;
	}
	
	public Ball GetNormal()
	{
		var ball = GetBallTemplate ();
		ball.BallType = BallType.Normal;
		ball.Score = 10;
		AddSprite(ball, _textureContainer.GetBall64 ());	
		return ball;
	}
	
	public Ball GetBig()
	{
		var ball = GetBallTemplate();
		ball.BallType = BallType.Big;
		ball.Score = 5;
		AddSprite(ball, _textureContainer.GetBall96 ());
		return ball;
	}
	
	private Ball GetBallTemplate()
	{
		var ball = new Ball ();
		
		var avatar = new GameObject ("Ball");
		
		avatar.transform.position = new Vector3 (5, 15, 0);		
		ball.Avatar = avatar;
		
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