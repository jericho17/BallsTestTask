using System;

public abstract class GameEvent
{
	private int _id;
	private GameEventType _type;
	protected GameCore Core;
	
	
	public int Id 
	{
		get 
		{ 
			return _id; 
		} 
	}
	
	public GameEventType GameEventType
	{
		get
		{
			return _type;
		}
	}
	
	
	public GameEvent (int id, GameEventType type, GameCore core)
	{
		_id = id;
		_type = type;
		Core = core;
	}
	
	
	public abstract void Execute();
	public abstract string Serialize();
}