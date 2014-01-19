using System;
using UnityEngine;

public class GameContainer
{
	public GameCore GameCore;
	public IGameController GameController;
	public Environment Environment;
	public EventFactory EventFactory;
	public TextureContainer TextureContainer;

	private bool _hasEnvironment;
	
	public GameContainer (GameCore core, IGameController controller, Environment environment, bool isServer)
	{
		GameCore = core;
		EventFactory = new EventFactory (GameCore);
		GameController = controller;
		Environment = environment;
		_hasEnvironment = isServer;
	}
	
	public void Update()
	{
		GameController.Update ();
		if (_hasEnvironment) 
		{
			Environment.Update ();
			HandleMouseClick ();
		}
	}
	
	//suppose, that should be the other way to get ball click
	void HandleMouseClick()
	{
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;
		
		if (Physics.Raycast (ray, out hit, 15)) {
			if (Input.GetMouseButtonDown(0))
				GameController.BallClick(hit.transform.gameObject.name);
		}
	}
	
	public void UseSimpleTextures()
	{
		TextureContainer.ChangeSkin (SkinType.Simple);
	}
	
	public void UseGeneratedTextures ()
	{
		TextureContainer.ChangeSkin (SkinType.Generated);
	}
}