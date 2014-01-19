using UnityEngine;

public class GameScreen : MonoBehaviour
{
	private WWW www;
	private SpriteRenderer _spriteRenderer;
	
	private GameCore _game;
	private GameContainer _container;
	private Environment _environment;
	
	public static bool IsServer;
	
	void Start()
	{
		CheckIfTest ();
		_container = Configurator.Instance.Container;
		_game = _container.GameCore;
	}

	//If scene started, but configurator not initialized - 
	//so we're in test mode
	private void CheckIfTest()
	{
		if (Configurator.Instance.Initialized == true)
			return;

		Configurator.Instance.SetLocal ();
	}
	
	/*	//TODO: HAVE TO INIT BG WITH THIS STUFF
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
*/
	void OnApplicationQuit() 
	{
		Configurator.Instance.EndGame ();
	}
	
	void Update()
	{
		_container.Update ();
	}
	
	void OnGUI()
	{
		var info = _game.Score.ToString ();
		var style = new GUIStyle ();
		style.normal.textColor = Color.white;
		style.fontSize = SizeHelper.OfWindowHeight(5);
		
		GUI.Label (new Rect (SizeHelper.OfWindowWidth(5),SizeHelper.OfWindowHeight(81),200,50), "Score: " + info, style);
		GUI.Label (new Rect (SizeHelper.OfWindowWidth(5),SizeHelper.OfWindowHeight(88),200,50), "Seconds: " + ((int)Time.timeSinceLevelLoad).ToString(), style);
		GUI.Label (new Rect (SizeHelper.OfWindowWidth(5),SizeHelper.OfWindowHeight(95),200,50), "Ip address: " + IpHelper.Ip.ToString(), style);
		
		var button1_leftSpan = SizeHelper.OfWindowWidth(93);
		var button1_topSpan = SizeHelper.OfWindowHeight(95);
		
		var button1_height = SizeHelper.OfWindowHeight(5);
		var button1_width = SizeHelper.OfWindowWidth(7);
		if(GUI.Button(new Rect(button1_leftSpan,button1_topSpan,button1_width,button1_height), "Exit")) 
		{
			ViewManager.BackToMenu();
		}
	}
}