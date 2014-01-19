using System;
public class Configurator
{
	private GameContainer _container;
	public GameContainer Container
	{
		get
		{
			return _container;
		}
	}
	
	public bool HasErrors = false;	
	public bool Initialized = false;

	public void SetLocal()
	{
		SetGameInternal (GameType.Local, true);
	}
	
	public void SetStreaming()
	{
		SetGameInternal (GameType.Streaming, true);
	}
	
	public void SetWatching()
	{
		SetGameInternal (GameType.Watching, false);
	}
	
	private void SetGameInternal(GameType type, bool hasEnvironment)
	{
		var TextureContainer = new TextureContainer ();
		var BallFactory = new BallFactory (TextureContainer);
		var BallManager = new BallManager (BallFactory);
		var GameCore = new GameCore (BallManager);
		var EventFactory = new EventFactory (GameCore);
		var GameController = MakeController (type, GameCore, EventFactory);
		var Environment = new Environment (GameController, EventFactory);
		
		_container = new GameContainer (GameCore, GameController, Environment, hasEnvironment);
		HasErrors = false;
		Initialized = true;
	}
	
	private static IGameController MakeController(GameType type, GameCore core, EventFactory eventFactory)
	{
		IGameController controller;
		var baseController = new GameController(core, eventFactory);
		switch (type) 
		{
		case GameType.Local:
			controller = baseController;
			break;
		case GameType.Streaming:		
			controller = new ServerController(baseController);
			break;
		case GameType.Watching:
			controller = new ClientController(baseController);
			break;
		default:
			throw new ArgumentOutOfRangeException("GameType");
		}
		
		return controller;
	}
	
	public void EndGame()
	{
		if (_container.GameCore.GameEnded)
			return;
		
		_container.GameController.EndGame (false);
	}
	
	#region Singletone

	private static object _locker = new Object();
	private static object _locker2 = new Object();
	
	private static Configurator _instance;
	public static Configurator Instance
	{
		get
		{
			lock(_locker)
			{
				if (_instance == null)
					lock (_locker2)
				{
					_instance = new Configurator();
				}
			}
			
			return _instance;
		}
		
	}
	
	#endregion
	
	#region GameTypes
	
	private enum GameType
	{
		Local,
		Streaming,
		Watching
	}
	
	#endregion
}