using UnityEngine;
using System;

public class MainMenu : MonoBehaviour
{
	private enum MenuState
	{
		Main,
		Single,
		Watching,
		Error
	}
	
	private MenuState _state;
	private string _ip = string.Empty;
	
	void Start()
	{
		Application.runInBackground = true;
		
		_state = Configurator.Instance.HasErrors
			? MenuState.Error
				: MenuState.Main;
	}
	
	void OnApplicationQuit() 
	{
		Configurator.Instance.EndGame ();
	}
	
	void OnGUI()
	{
		switch (_state) 
		{
		case MenuState.Main:
			ShowMain();
			break;
		case MenuState.Single:
			ShowSingle();
			break;
		case MenuState.Watching:
			ShowWatching();
			break;
		case MenuState.Error:
			ShowError();
			break;
		default:
			throw new ArgumentException("MenuState");
		}
		
	}
	
	void ShowMain()
	{	
		var buttonSpan = 10;
		var buttonHeight = 30;
		
		var boxWidth = 200;
		var boxHeight = 150;
		var leftBoxSpan = (Screen.width - boxWidth) / 2;
		var topBoxSpan = (Screen.height - boxHeight) / 2;
		GUI.Box (new Rect (leftBoxSpan, topBoxSpan, boxWidth, boxHeight), 
		         "Game menu");
		
		
		var button1_leftSpan = leftBoxSpan + buttonSpan;
		var button1_topSpan = topBoxSpan + buttonSpan+20;
		var button1_height = buttonHeight;
		var button1_width = boxWidth - 2 * buttonSpan;
		if(GUI.Button(new Rect(button1_leftSpan,button1_topSpan,button1_width,button1_height), 
		              "Single player")) 
		{
			_state = MenuState.Single;
		}
		
		
		var button2_leftSpan = leftBoxSpan + buttonSpan;
		var button2_topSpan = button1_topSpan+button1_height +10;
		var button2_height = buttonHeight;
		var button2_width = boxWidth - 2 * buttonSpan;
		if(GUI.Button(new Rect(button2_leftSpan,button2_topSpan,button2_width,button2_height), 
		              "Watch game")) 
		{
			_state = MenuState.Watching;
		}
		
		var button3_leftSpan = leftBoxSpan + buttonSpan;
		var button3_topSpan = button2_topSpan+button2_height +10;
		var button3_height = buttonHeight;
		var button3_width = boxWidth - 2 * buttonSpan;
		if(GUI.Button(new Rect(button3_leftSpan,button3_topSpan,button3_width,button3_height), 
		              "Quit")) 
		{
			Application.Quit();
		}
	}
	void ShowSingle()
	{
		var buttonSpan = 10;
		var buttonHeight = 30;
		
		var boxWidth = 200;
		var boxHeight = 150;
		var leftBoxSpan = (Screen.width - boxWidth) / 2;
		var topBoxSpan = (Screen.height - boxHeight) / 2;
		GUI.Box (new Rect (leftBoxSpan, topBoxSpan, boxWidth, boxHeight), 
		         "Single game");
		
		
		var button1_leftSpan = leftBoxSpan + buttonSpan;
		var button1_topSpan = topBoxSpan + buttonSpan+20;
		var button1_height = buttonHeight;
		var button1_width = boxWidth - 2 * buttonSpan;
		if(GUI.Button(new Rect(button1_leftSpan,button1_topSpan,button1_width,button1_height), 
		              "Local")) 
		{
			
			ViewManager.LocalGame();
		}
		
		
		var button2_leftSpan = leftBoxSpan + buttonSpan;
		var button2_topSpan = button1_topSpan+button1_height +10;
		var button2_height = buttonHeight;
		var button2_width = boxWidth - 2 * buttonSpan;
		if(GUI.Button(new Rect(button2_leftSpan,button2_topSpan,button2_width,button2_height), 
		              "Streaming")) 
		{
			ViewManager.StreamingGame();
		}
		
		var button3_leftSpan = leftBoxSpan + buttonSpan;
		var button3_topSpan = button2_topSpan+button2_height +10;
		var button3_height = buttonHeight;
		var button3_width = boxWidth - 2 * buttonSpan;
		if(GUI.Button(new Rect(button3_leftSpan,button3_topSpan,button3_width,button3_height), 
		              "Back")) 
		{
			_state = MenuState.Main;
		}
	}
	void ShowWatching()
	{
		var boxWidth = 200;
		var boxHeight = 150;
		var leftBoxSpan = (Screen.width - boxWidth) / 2;
		var topBoxSpan = (Screen.height - boxHeight) / 2;
		var style = new GUIStyle ();
		style.normal.textColor = Color.white;
		style.fontSize = SizeHelper.OfWindowHeight(5);
		GUI.Box (new Rect (leftBoxSpan, topBoxSpan, boxWidth, boxHeight), 
		         "Watch game");
		
		var buttonSpan = 10;
		var buttonHeight = 30;
		
		var button1_leftSpan = leftBoxSpan + buttonSpan;
		var button1_topSpan = topBoxSpan + buttonSpan+20;
		var button1_height = buttonHeight;
		var button1_width = boxWidth - 2 * buttonSpan;
		_ip = GUI.TextField (new Rect (button1_leftSpan, button1_topSpan, button1_width, button1_height), 
		                     _ip);
		
		
		var button2_leftSpan = leftBoxSpan + buttonSpan;
		var button2_topSpan = button1_topSpan+button1_height +10;
		var button2_height = buttonHeight;
		var button2_width = boxWidth - 2 * buttonSpan;
		if(GUI.Button(new Rect(button2_leftSpan,button2_topSpan,button2_width,button2_height), 
		              "OK")) 
		{
			if (IpHelper.CheckValid(_ip))
			{
				IpHelper.SetServerIp(_ip);
				ViewManager.WatchGame();
			}
			
		}
		
		var button3_leftSpan = leftBoxSpan + buttonSpan;
		var button3_topSpan = button2_topSpan+button2_height +10;
		var button3_height = buttonHeight;
		var button3_width = boxWidth - 2 * buttonSpan;
		if(GUI.Button(new Rect(button3_leftSpan,button3_topSpan,button3_width,button3_height), 
		              "Back")) 
		{
			_state = MenuState.Main;
		}
	}
	
	void ShowError()
	{
		var boxWidth = 200;
		var boxHeight = 70;
		var leftBoxSpan = (Screen.width - boxWidth) / 2;
		var topBoxSpan = (Screen.height - boxHeight) / 2;
		var style = new GUIStyle ();
		style.normal.textColor = Color.white;
		style.fontSize = SizeHelper.OfWindowHeight(5);
		GUI.Box (new Rect (leftBoxSpan, topBoxSpan, boxWidth, boxHeight), "Cannot connect to server.");
		
		var buttonSpanTop = 10;
		var buttonSpanBot = 65;
		var buttonHeight = 30;
		
		var button1_leftSpan = leftBoxSpan + buttonSpanBot;
		var button1_topSpan = topBoxSpan + buttonSpanTop+20;
		var button1_height = buttonHeight;
		var button1_width = boxWidth - 2 * buttonSpanBot;
		if(GUI.Button(new Rect(button1_leftSpan,button1_topSpan,button1_width,button1_height), "Exit")) 
		{
			_state = MenuState.Watching;
		}
	}
	
}