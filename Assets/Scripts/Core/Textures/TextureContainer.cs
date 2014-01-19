using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureContainer
{
	private const SkinType DefaultSkinType = SkinType.Simple;

	private Sprite _background;
	private Sprite _greenBall32;
	private Sprite _greenBall64;
	private Sprite _greenBall96;
	private Sprite _greenBall128;
	private Sprite _lightblueBall32;
	private Sprite _lightblueBall64;
	private Sprite _lightblueBall96;
	private Sprite _lightblueBall128;
	private Sprite _lightbrownBall32;
	private Sprite _lightbrownBall64;
	private Sprite _lightbrownBall96;
	private Sprite _lightbrownBall128;
	private Sprite _pinkBall32;
	private Sprite _pinkBall64;
	private Sprite _pinkBall96;
	private Sprite _pinkBall128;
	
	
	public Sprite GetBackground(){ return _background; }
	public Sprite GetBall32()    { return GetRandom32();}
	public Sprite GetBall64()    { return GetRandom64();}
	public Sprite GetBall96()    { return GetRandom96();}
	public Sprite GetBall128()    { return GetRandom128();}

	
	public TextureContainer ()
	{
		ChangeSkin (DefaultSkinType);
	}
	
	public void ChangeSkin( SkinType skinType)
	{
		switch (skinType) 
		{
		case SkinType.Simple:
		{
			UseTexturesFromResources();
			break;
		}
		case SkinType.Generated:
		{
			GenerateTextures();
			break;
		}
		default:
			throw new ArgumentOutOfRangeException();
		}
	}


	private void UseTexturesFromResources()
	{
		_background = Resources.Load<Sprite> ("BackgroundImage");

		_greenBall32  = Resources.Load<Sprite> ("green32");
		_greenBall64  = Resources.Load<Sprite> ("green64");
		_greenBall96  = Resources.Load<Sprite> ("green96");
		_greenBall128 = Resources.Load<Sprite> ("green128");

		_lightblueBall32  = Resources.Load<Sprite> ("lightblue32");
		_lightblueBall64  = Resources.Load<Sprite> ("lightblue64");
		_lightblueBall96  = Resources.Load<Sprite> ("lightblue96");
		_lightblueBall128 = Resources.Load<Sprite> ("lightblue128");

		_lightbrownBall32  = Resources.Load<Sprite> ("lightbrown32");
		_lightbrownBall64  = Resources.Load<Sprite> ("lightbrown64");
		_lightbrownBall96  = Resources.Load<Sprite> ("lightbrown96");
		_lightbrownBall128 = Resources.Load<Sprite> ("lightbrown128");

		_pinkBall32  = Resources.Load<Sprite> ("pink32");
		_pinkBall64  = Resources.Load<Sprite> ("pink64");
		_pinkBall96  = Resources.Load<Sprite> ("pink96");
		_pinkBall128 = Resources.Load<Sprite> ("pink128");
	}
			
	private IEnumerator GenerateTextures ()
	{
		_background = Resources.Load<Sprite> ("BackgroundImage");
		
		_greenBall32  = TextureGenerator.GetBall32 (Color.green);
		_greenBall64  = TextureGenerator.GetBall64 (Color.green);
		_greenBall96  = TextureGenerator.GetBall96 (Color.green);
		_greenBall128 = TextureGenerator.GetBall128 (Color.green);
		
		_lightblueBall32  = TextureGenerator.GetBall32 (Color.blue);
		_lightblueBall64  = TextureGenerator.GetBall64 (Color.blue);
		_lightblueBall96  = TextureGenerator.GetBall96 (Color.blue);
		_lightblueBall128 = TextureGenerator.GetBall128 (Color.blue);
		
		_lightbrownBall32  = TextureGenerator.GetBall32 (Color.magenta);
		_lightbrownBall64  = TextureGenerator.GetBall64 (Color.magenta);
		_lightbrownBall96  = TextureGenerator.GetBall96 (Color.magenta);
		_lightbrownBall128 = TextureGenerator.GetBall128 (Color.magenta);
		
		_pinkBall32  = TextureGenerator.GetBall32 (Color.cyan);
		_pinkBall64  = TextureGenerator.GetBall64 (Color.cyan);
		_pinkBall96  = TextureGenerator.GetBall96 (Color.cyan);
		_pinkBall128 = TextureGenerator.GetBall128 (Color.cyan);

		return new List<int>().GetEnumerator();
	}

	private Sprite GetRandom32()
	{
		var num = UnityEngine.Random.Range (0, 3);
		switch (num) 
		{
		case 0:
			return _greenBall32;
		case 1:
			return _lightblueBall32;
		case 2:
			return _lightbrownBall32;
		case 3:
			return _pinkBall32;
		default:
			throw new ArgumentOutOfRangeException ();
		}
	}
	
	private Sprite GetRandom64()
	{
		var num = UnityEngine.Random.Range (0, 3);
		switch (num) 
		{
		case 0:
			return _greenBall64;
		case 1:
			return _lightblueBall64;
		case 2:
			return _lightbrownBall64;
		case 3:
			return _pinkBall64;
		default:
			throw new ArgumentOutOfRangeException ();
		}
	}
	
	private Sprite GetRandom96()
	{
		var num = UnityEngine.Random.Range (0, 3);
		switch (num) 
		{
		case 0:
			return _greenBall96;
		case 1:
			return _lightblueBall96;
		case 2:
			return _lightbrownBall96;
		case 3:
			return _pinkBall96;
		default:
			throw new ArgumentOutOfRangeException ();
		}
	}
	
	private Sprite GetRandom128()
	{
		var num = UnityEngine.Random.Range (0, 3);
		switch (num) 
		{
		case 0:
			return _greenBall128;
		case 1:
			return _lightblueBall128;
		case 2:
			return _lightbrownBall128;
		case 3:
			return _pinkBall128;
		default:
			throw new ArgumentOutOfRangeException ();
		}
	}
}