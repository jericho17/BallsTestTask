using UnityEngine;

public class ViewManager
{
	private const string GameScreenName = "GameScreen";
	private const string MainMenuName = "MainMenu";
	
	public static void LocalGame()
	{
		Configurator.Instance.SetLocal();
		Application.LoadLevel(GameScreenName);
	}
	
	public static void StreamingGame()
	{
		Configurator.Instance.SetStreaming();
		Application.LoadLevel(GameScreenName);
	}
	
	public static void WatchGame()
	{
		Configurator.Instance.SetWatching();
		Application.LoadLevel(GameScreenName);
	}
	
	public static void BackToMenu()
	{
		Configurator.Instance.EndGame ();
		Application.LoadLevel(MainMenuName); 
	}
}