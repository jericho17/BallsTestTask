using UnityEngine;

public class SizeHelper
{
	public static int OfWindowWidth(int percent)
	{
		return Screen.width * percent / 100;
	}
	
	public static int OfWindowHeight(int percent)
	{
		return Screen.height * percent / 100;
	}
}