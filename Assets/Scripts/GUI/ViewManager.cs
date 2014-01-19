using UnityEngine;

public class ViewManager
{
	private const string GameScreenName = "GameScreen";
	private const string MainMenuName = "MainMenu";
	private const string WatchScreenName = "WatchScreen";
	
	public static void NewGame()
	{
		GameScreen.IsServer = true;
		Application.LoadLevel(GameScreenName);
	}
	
	public static void WatchGame()
	{
		GameScreen.IsServer = false;
		Application.LoadLevel(GameScreenName);
	}
	
	public static void BackToMenu()
	{
		Application.LoadLevel(MainMenuName); 
	}
}