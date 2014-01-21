using System;

public class ServerController : IGameController
{
	private IGameController _controller;
	private ServerTransport _transport;
	
	public GameCore GameCore { get {return _controller.GameCore;}}
	
	
	public ServerController (IGameController controller)
	{
		_controller = controller;

		var serverProtocol = new ServerProtocol (this);
		_transport = new ServerTransport (serverProtocol);
		
		_transport.StartListening ();
	}
	
	public void CreateNewGame ()
	{
		throw new NotImplementedException ();
	}
	
	public void EndGame(bool hasErrors)
	{
		_transport.StopListening ();
		_controller.EndGame(hasErrors);
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
		_controller.Update ();
	}
}