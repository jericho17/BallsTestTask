using System;
using UnityEngine;
using System.Collections;

//Class to do the coroutine in core
public class CoroutineHelper
{
	private static MonoBehaviour _executer;
	
	static CoroutineHelper ()
	{
		var obj = new GameObject ();
		var executer = obj.AddComponent<CoroutineHelperBehaviour> ();
		_executer = executer;

	}
	
	public static void Start (IEnumerator method)
	{
		try {
			_executer.StartCoroutine (method);		
				} catch (Exception ex) {
			Debug.Log(ex.ToString());
				}

	}
}

public class CoroutineHelperBehaviour : MonoBehaviour 
{
	void OnDestroy() {
		print("Script was destroyed");
	}
}