using UnityEngine;
using System.Collections;
using System.Linq;

public class BundleLoader : MonoBehaviour 
{
	private WWW _bundleLoader;

	void Start () 
	{
		StartCoroutine (LoadBundles ());
	}

	IEnumerator LoadBundles()
	{
		var bundleUrl = "http://54.201.135.182/BallTextures.unity3d";		
		_bundleLoader = new WWW (bundleUrl);

		yield return _bundleLoader;

		Bundles.Add("green32",_bundleLoader.assetBundle.Load("green32",typeof(Sprite)) as Sprite);
		Bundles.Add("green64",_bundleLoader.assetBundle.Load("green64",typeof(Sprite)) as Sprite);
		Bundles.Add("green96",_bundleLoader.assetBundle.Load("green96",typeof(Sprite)) as Sprite);
		Bundles.Add("green128",_bundleLoader.assetBundle.Load("green128",typeof(Sprite)) as Sprite);

		Bundles.Add("lightblue32",_bundleLoader.assetBundle.Load("lightblue32",typeof(Sprite)) as Sprite);
		Bundles.Add("lightblue64",_bundleLoader.assetBundle.Load("lightblue64",typeof(Sprite)) as Sprite);
		Bundles.Add("lightblue96",_bundleLoader.assetBundle.Load("lightblue96",typeof(Sprite)) as Sprite);
		Bundles.Add("lightblue128",_bundleLoader.assetBundle.Load("lightblue128",typeof(Sprite)) as Sprite);

		Bundles.Add("lightbrown32",_bundleLoader.assetBundle.Load("lightbrown32",typeof(Sprite)) as Sprite);
		Bundles.Add("lightbrown64",_bundleLoader.assetBundle.Load("lightbrown64",typeof(Sprite)) as Sprite);
		Bundles.Add("lightbrown96",_bundleLoader.assetBundle.Load("lightbrown96",typeof(Sprite)) as Sprite);
		Bundles.Add("lightbrown128",_bundleLoader.assetBundle.Load("lightbrown128",typeof(Sprite)) as Sprite);

		Bundles.Add("pink32",_bundleLoader.assetBundle.Load("pink32",typeof(Sprite)) as Sprite);
		Bundles.Add("pink64",_bundleLoader.assetBundle.Load("pink64",typeof(Sprite)) as Sprite);
		Bundles.Add("pink96",_bundleLoader.assetBundle.Load("pink96",typeof(Sprite)) as Sprite);
		Bundles.Add("pink128",_bundleLoader.assetBundle.Load("pink128",typeof(Sprite)) as Sprite);
	}
}
