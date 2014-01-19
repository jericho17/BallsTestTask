using System;
using UnityEngine;
public class ClientController : IGameController
{
	private IGameController _controller;
	private ClientTransport _transport;
	
	public GameCore GameCore {	get {return _controller.GameCore;}	}
	
	public ClientController (IGameController controller)
	{
		_controller = controller;
		_transport = new ClientTransport (this);
		
	}
	
	public void CreateNewGame ()
	{
		var t = new System.Threading.Thread (AskUpdate);
		t.Start ();
	}
	
	private void AskUpdate()
	{
		while (!GameCore.GameEnded) 
		{
			Debug.Log("asking update");
			_transport.Update();
			System.Threading.Thread.Sleep(100);
		}
	}
	
	public void AddGameEvent (GameEvent gameEvent)
	{
		_controller.AddGameEvent (gameEvent);
	}
	
	public void AddGameEvent (string serializedEvent)
	{
		_controller.AddGameEvent (serializedEvent);
	}
	
	public void BallClick (string ballName)
	{
		_controller.BallClick (ballName);
	}
	
	public void Update ()
	{
		_transport.Update ();
		_controller.Update ();
	}
}