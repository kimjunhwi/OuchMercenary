using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildAssetBundle : MonoBehaviour 
{

	[MenuItem("Bundles/Build AssetBundles")]
	static void BuildAllAssetBundles()
	{
		BuildPipeline.BuildAssetBundles ("Assets/AssetBundles", BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.Android);
	}

}
