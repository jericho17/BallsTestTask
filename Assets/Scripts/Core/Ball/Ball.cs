using System;
using UnityEngine;

public class Ball
{
	public int Id;
	public int Score;
	public BallType BallType;
	public GameObject Avatar;
	public bool Destroyed = false;
	public GameObject DestroySound;
	
	public Ball ()
	{
		//DestroySound = new GameObject ();
		
		//DestroySound.audio = Resources.Load<AudioClip> ("BallSound");
	}
	
	public void Move()
	{
		float speed = 0;
		
		switch (BallType) 
		{
		case BallType.Small:
			speed = 2;
			break;
		case BallType.Normal:
			speed = 1.5f;
			break;
		case BallType.Big:
			speed = 1;
			break;
		default:
			throw new ArgumentException();
		}
		
		Avatar.transform.Translate (Vector3.down * Time.deltaTime*speed/3);
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