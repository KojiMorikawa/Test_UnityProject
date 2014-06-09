using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
public class PTPTools
{
    [MenuItem("PTPTools/CreateZeroPosGameObjects")]
    static void Create()
    {
        GameObject obj = new GameObject("_GameObject");
        obj.transform.position = new Vector3(0,0,0);
    }

    [MenuItem("PTPTools/CreateDefaultObjects")]
    static void CreateDefualrObjects()
    {
        GameObject SearchObj;

        SearchObj = GameObject.Find("Manager");
        if (SearchObj == null)
        {
            GameObject obj = new GameObject("Manager");
            obj.transform.position = new Vector3(0,0,0);
        }

        SearchObj = GameObject.Find("SceneObjects");
        if (SearchObj == null)
        {
            GameObject obj = new GameObject("SceneObjects");
            obj.transform.position = new Vector3(0,0,0);
        }
    }
}
#endif 
