using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextureContainer
{
	private string bundleImagesUrl = @"file://C://Work/Fancy balls/trunk/FancyBalls/Assets/Bundles/ImageBundle.unity3d";
	private WWW _bundleLoader;
	
	private const SkinType DefaultSkinType = SkinType.Simple;
	
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
		case SkinType.Loaded:
		{
			CoroutineHelper.Start(LoadBundle());
			break;
		}
		case SkinType.Generated:
		{
			CoroutineHelper.Start(GenerateTextures());
			break;
		}
		default:
			break;
		}
	}
	
	private Sprite _background;
	private Sprite _ball32;
	private Sprite _ball64;
	private Sprite _ball96;
	
	public Sprite GetBackground(){ return _background; }
	public Sprite GetBall32()    { return _ball32;     }
	public Sprite GetBall64()    { return _ball64;     }
	public Sprite GetBall96()    { return _ball96;     }
	
	
	public  IEnumerator LoadBundle()	
	{ 	
		_bundleLoader = WWW.LoadFromCacheOrDownload (bundleImagesUrl,0);
		yield return _bundleLoader;
		_ball32 = _bundleLoader.assetBundle.Load("Ball32", typeof(Sprite)) as Sprite;
		_ball64 = _bundleLoader.assetBundle.Load("Ball64", typeof(Sprite)) as Sprite;
		_ball96 = _bundleLoader.assetBundle.Load("Ball96", typeof(Sprite)) as Sprite;
		UseTexturesFromBundle ();
	}
	
	private void UseTexturesFromResources()
	{
		_background = Resources.Load<Sprite> ("BackgroundImage");
		
		_ball32 = Resources.Load<Sprite> ("Ball1(32x32)");
		_ball64 = Resources.Load<Sprite> ("Ball1(64x64)");
		_ball96 = Resources.Load<Sprite> ("Ball1(96x96)");
	}
	
	private void UseTexturesFromBundle ()
	{
		
	}
	
	private IEnumerator GenerateTextures ()
	{
		_ball32 = TextureGenerator.GetBall64 ();
		_ball64 = TextureGenerator.GetBall128 ();
		_ball96 = TextureGenerator.GetBall256 ();
		return new List<int>().GetEnumerator();
	}
}