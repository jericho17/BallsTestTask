using UnityEngine;

public class GameScreen : MonoBehaviour
{
	private WWW www;
	private SpriteRenderer _spriteRenderer;
	
	private GameCore _game;
	private IGameController _controller;
	private Environment _environment;
	
	public static bool IsServer;
	
	void Start()
	{
		if (IsServer)
			GameContainer.Instance.StartServer();	
		else
			GameContainer.Instance.StartClient();	
		
		_game = GameContainer.Instance.GameCore;
		_controller = GameContainer.Instance.GameController;
		_environment = GameContainer.Instance.Environment;
		
		//		GameContainer.Instance.TextureContainer.ChangeSkin (SkinType.Loaded); 
	}
	
	//TODO: HAVE TO INIT BG WITH THIS STUFF
	private void InitBackground()
	{
		var game = GameContainer.Instance;
		var background = game.TextureContainer.GetBackground ();
		
		var bg = new GameObject ("Background");
		var sr = bg.AddComponent<SpriteRenderer> ();
		sr.sprite = background;
		sr.transform.localScale = new Vector3 (2,2,0);
		bg.transform.position = new Vector3 (11f, 7f, 0);
	}
	
	
	void Update()
	{
		_controller.Update ();
		if (IsServer) 
		{
			
			_environment.Update ();
			HandleMouseClick ();
		}
	}
	
	//suppose, that should be the other way to get ball click
	void HandleMouseClick()
	{
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit, 10)) {
			if (Input.GetMouseButtonDown(0))
				_controller.BallClick(hit.transform.gameObject.name);
		}
	}
	
	void OnGUI()
	{
		var game = GameContainer.Instance.GameCore;
		
		var info = game.Score.ToString ();
		var style = new GUIStyle ();
		style.normal.textColor = Color.white;
		style.fontSize = SizeHelper.OfWindowHeight(5);
		
		GUI.Label (new Rect (SizeHelper.OfWindowWidth(5),SizeHelper.OfWindowHeight(81),200,50), "Score: " + info, style);
		GUI.Label (new Rect (SizeHelper.OfWindowWidth(5),SizeHelper.OfWindowHeight(88),200,50), "Seconds: " + ((int)Time.timeSinceLevelLoad).ToString(), style);
	}
}