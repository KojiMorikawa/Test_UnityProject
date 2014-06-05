// This TempleteClass is based on Great Pioneers..
// terasur.blog.fc.2com/blog-entry-311.html
// naichilab.blogspot.jp/2013/05/unity.html
// warapuri.tumblr.com/post/28972633000/unity-50-tips

using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T :MonoBehaviour
{
	private static T _instance;
    public static T Instance
    {
		get {
            if (_instance == null)
            {
                _instance = (T)FindObjectOfType(typeof(T));

                if (_instance == null)
				{
					Debug.LogError( typeof(T) + "is not found!!");
				}
			}

            return _instance;
		}
	}
}

