using System;
using System.Collections.Generic;
using System.Linq;

public class Bundles
{
	private static Dictionary<string, object> _assets = new Dictionary<string, object>();

	public static void Add(string name, object asset)
	{
		_assets.Add (name, asset);
	}

	public static T Load<T> (string name)
		where T : class
	{
		return _assets.ContainsKey (name) 
			? (T)_assets [name] 
			: null;
	}

	public static bool Initialized {get{return _assets.Any();}}
}
