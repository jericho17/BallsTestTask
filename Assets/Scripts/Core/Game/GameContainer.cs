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
	
	private void Initialize()
	{
		TextureContainer = new TextureContainer ();
		BallFactory = new BallFactory (TextureContainer);
		BallManager = new BallManager (BallFactory);		
		GameCore = new GameCore (BallManager);
		EventFactory = new EventFactory (GameCore);
		GameController = GetGameController (SelectedGameType);		
		Environment = new Environment (GameController, EventFactory);
		
		Initialized = true;
	}

	private IGameController GetGameController(GameType gameType)
	{
		var baseController = new GameController (GameCore, EventFactory);

		if (gameType == GameType.Single)
			return baseController;
		else
			return gameType == GameType.Server
				? new ServerController (baseController) as IGameController
				: new ClientController (baseController) as IGameController;
	}

	public void StartSingle()
	{
		SelectedGameType = GameType.Single;
		Initialize ();
	}
	
	public void StartServer()
	{
		SelectedGameType = GameType.Server;
		Initialize ();
	}
	
	public void StartClient()
	{
		SelectedGameType = GameType.Client;
		Initialize ();
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

	private static object syncRoot = new Object();

	public const GameType DefaultGameType = GameType.Single;
	public static GameType SelectedGameType = DefaultGameType;

	private static GameContainer _instance;
	public static GameContainer Instance
	{
		get
		{
			if (_instance == null)
				lock (syncRoot) 
				{
					_instance = new GameContainer ();
				}
			return _instance;
		}		
	}

	#endregion
}