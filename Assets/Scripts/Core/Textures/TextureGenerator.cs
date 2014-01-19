using System;
using UnityEngine;

public class TextureGenerator
{
	public TextureGenerator ()
	{
	}
	
	public static Sprite GetBall32()
	{
		var size = 32;
		return GetSizedBall(size);
	}
	
	public static Sprite GetBall64()
	{
		var size = 64;
		return GetSizedBall(size);
	}
	
	public static Sprite GetBall128()
	{
		var size = 128;
		return GetSizedBall(size);
	}
	
	public static Sprite GetBall256()
	{
		var size = 256;
		return GetSizedBall(size);
	}
	
	private static Sprite GetSizedBall(int size)
	{
		var texture = new Texture2D (size, size, TextureFormat.ARGB32, false);
		var s = Sprite.Create(texture, new Rect(0,0,size,size), new Vector2(0,0));
		
		MakeTranspanent (texture);
		DrawCircle (texture, size/2, 4, Color.red);
		DrawCircle (texture, size/2-4, size/2-8, Color.black);
		return s;
	}
	
	static void MakeTranspanent (Texture2D texture)
	{
		var size = texture.width;
		
		for (int i = 0; i < size; i++) 
		{
			for (int j = 0; j < size; j++) 
			{
				texture.SetPixel(i,j,new Color());
			}
		}
	}
	
	private static void DrawCircle(Texture2D texture, int radius, int width, Color color)
	{
		var size = texture.width;
		
		for (int i = 0; i < size; i++) 
		{
			for (int j = 0; j < size; j++) 
			{
				if (IsInCircle(radius,radius-width,size,i,j))
					texture.SetPixel(i,j,color);
				
			}
			
		}
		
		texture.Apply ();
	}
	
	private static bool IsInCircle(int outerRadius, int innerRadius, int size, int x, int y)
	{
		var center = size / 2;
		var xPos = x - center;
		var yPos = y - center;
		var isInCircle = (xPos * xPos) + (yPos * yPos) <= outerRadius * outerRadius&&
			(xPos * xPos) + (yPos * yPos) >= innerRadius * innerRadius;
		return isInCircle;
	}
}