using System;

public class GameContainer
{
	public GameCore GameCore;
	public IGameController GameController;
	public Environment Environment;
	public TextureContainer TextureContainer;
	public BallFactory BallFactory;
	public BallManager BallManager;
	public EventFactory EventFactory;
	
	public bool Initialized;
	
	public GameContainer ()
	{
		Initialized = false;
	}
	
	private void Initialize(bool isServer)
	{
		TextureContainer = new TextureContainer ();
		BallFactory = new BallFactory (TextureContainer);
		BallManager = new BallManager (BallFactory);
		
		GameCore = new GameCore (BallManager);//use ball manager
		EventFactory = new EventFactory (GameCore);
		var baseController = new GameController (GameCore, EventFactory);
		GameController = isServer
			? new ServerController (baseController) as IGameController
				: new ClientController (baseController) as IGameController;
		
		Environment = new Environment (GameController, EventFactory);
		
		Initialized = true;
	}
	
	public void StartServer()
	{
		Initialize (true);
	}
	
	public void StartClient()
	{
		Initialize (false);
	}
		
	public void UseSimpleTextures()
	{
		TextureContainer.ChangeSkin (SkinType.Simple);
	}
	
	public void UseGeneratedTextures ()
	{
		TextureContainer.ChangeSkin (SkinType.Generated);
	}
	
	#region Singletone
	
	private static object _locker = new Object();
	private static object _locker2 = new Object();
	
	private static GameContainer _instance;
	public static GameContainer Instance
	{
		get
		{
			lock(_locker)
			{
				if (_instance == null)
					lock (_locker2)
				{
					_instance = new GameContainer();
				}
			}
			
			return _instance;
		}
		
	}
	
	#endregion
}