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

		var clientProtocol = new ClientProtocol (this);
		_transport = new ClientTransport (clientProtocol);
	}
	
	public void CreateNewGame ()
	{
		var t = new System.Threading.Thread (AskUpdate);
		t.Start ();
	}
	
	public void EndGame(bool hasErrors)
	{
		_controller.EndGame(hasErrors);
	}
	
	private void AskUpdate()
	{
		while (!GameCore.GameEnded) 
		{
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
		Debug.Log ("Update");
		_transport.Update ();
		_controller.Update ();
	}
}