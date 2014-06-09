using UnityEngine;
//using UnityEditor;
using System.Collections;

public class AssetObject
{
    private static string excpetName = "Entity_";

#if PC_DEBUG_ASSET
    public static T Load<T> () where T : ScriptableObject
    {
        string FileName, AssetFilePath;

        string tmpName = typeof(T).Name;
        FileName = tmpName.Substring(excpetName.Length);

		AssetFilePath = "Assets/ExcelData/" + FileName + ".asset";
#if PC_DEBUG
        Debug.Log("--> Asset File Path : " + AssetFilePath);
#endif
		T AssetObj = (T)Resources.LoadAssetAtPath(AssetFilePath, typeof(T));
        if (AssetObj == null)
        {
#if PC_DEBUG
			Debug.Log("Error: Not Exist File or Make Object Failed.");
#endif
            return null;
		}
		else{
#if PC_DEBUG
			Debug.Log("AssetObject Loaded.");
#endif
    	}
        return AssetObj;
    }

#else

    public static T Load<T> () where T : ScriptableObject
    {
        string FileName, AssetFilePath;

        string tmpName = typeof(T).Name;
        //        FileName = tmpName.Substring(excpetName.Length - 1);
        FileName = tmpName.Substring(excpetName.Length);

        AssetFilePath = "ExcelData/" + FileName;
#if PC_DEBUG
        Debug.Log("--> Asset File Path : " + AssetFilePath);
#endif
        T AssetObj = (T)Resources.Load(AssetFilePath, typeof(T));
        if (AssetObj == null)
        {
#if PC_DEBUG
			Debug.Log("Error: Not Exist File or Make Object Failed.");
#endif
            return null;
		}
#if PC_DEBUG
		else{
			Debug.Log("AssetObject Loaded.");
    	}
#endif
        return AssetObj;
    }

#endif
}
