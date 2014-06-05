using UnityEngine;
using System.Collections;

public class DebugGameBoot : MonoBehaviour {

    [SceneName]
    public string StartSceneName = "TestScene00";

	// Use this for initialization
	void Start () {
        Application.LoadLevel(StartSceneName);
	}
}
