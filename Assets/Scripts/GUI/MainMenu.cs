using UnityEngine;

public class MainMenu : MonoBehaviour
{
	void Start()
	{
		Application.runInBackground = true;
	}
	
	void Update()
	{}
	
	void OnGUI()
	{
		var buttonSpan = 10;
		var buttonHeight = 30;
		
		var boxWidth = 200;
		var boxHeight = 120;
		var leftBoxSpan = (Screen.width - boxWidth) / 2;
		var topBoxSpan = (Screen.height - boxHeight) / 2;
		GUI.Box (new Rect (leftBoxSpan, topBoxSpan, boxWidth, boxHeight), "Game menu");
		
		
		var button1_leftSpan = leftBoxSpan + buttonSpan;
		var button1_topSpan = topBoxSpan + buttonSpan+20;
		var button1_height = buttonHeight;
		var button1_width = boxWidth - 2 * buttonSpan;
		if(GUI.Button(new Rect(button1_leftSpan,button1_topSpan,button1_width,button1_height), "Start new game")) 
		{
			ViewManager.NewGame();
		}
		
		
		var button2_leftSpan = leftBoxSpan + buttonSpan;
		var button2_topSpan = button1_topSpan+button1_height +10;
		var button2_height = buttonHeight;
		var button2_width = boxWidth - 2 * buttonSpan;
		if(GUI.Button(new Rect(button2_leftSpan,button2_topSpan,button2_width,button2_height), "Watch game")) 
		{
			ViewManager.WatchGame();
		}
	}
}