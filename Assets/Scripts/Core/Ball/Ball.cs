using System;
using UnityEngine;

public class Ball
{
	public int Id { get; set;}
	public int Score { get; private set;}
	public BallType BallType { get; private set;}
	public GameObject Avatar { get; private set;}
	public bool Destroyed { get; set;}

	private float _speed;
	
	public Ball (BallType ballType, GameObject avatar, int score)
	{
		BallType = ballType;
		Avatar = avatar;
		Score = score;
		SetSpeed ();
		Destroyed = false;
	}

	private void SetSpeed()
	{
		switch (BallType) 
		{
		case BallType.Small:
			_speed = 0.7f;
			break;
		case BallType.Normal:
			_speed = 0.5f;
			break;
		case BallType.Big:
			_speed = 0.33f;
			break;
		case BallType.VeryBig:
			_speed = 0.25f;
			break;
		default:
			throw new ArgumentException();
		}
	}
	
	public void Move()
	{
		Avatar.transform.Translate (Vector3.down * Time.deltaTime*_speed);
	}
	
	public bool Check()
	{
		return Avatar.transform.position.y > -2;
	}
	
	public void Destroy()
	{
		GameObject.Destroy (Avatar);
	}
}