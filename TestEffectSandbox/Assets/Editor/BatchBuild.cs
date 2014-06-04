using UnityEngine;
using UnityEditor;
using System.Collections;

public class BatchBuild : MonoBehaviour {

	[UnityEditor.MenuItem("Tools/Build Project AllScene Android")]
	public static void BuildProjectAllSceneAndroid() {
		EditorUserBuildSettings.SwitchActiveBuildTarget( BuildTarget.Android );
		string[] allScene = new string[EditorBuildSettings.scenes.Length];
		int i = 0;
		foreach( EditorBuildSettingsScene scene in EditorBuildSettings.scenes ){
			allScene[i] = scene.path;
			i++;
		}	
		PlayerSettings.bundleIdentifier = "jp.co.inis.golf";
		PlayerSettings.statusBarHidden = true;
		BuildPipeline.BuildPlayer( allScene,
		                          "Test_Sandbox.apk",
		                          BuildTarget.Android,
		                          BuildOptions.None
		                          );
	}

	[UnityEditor.MenuItem("Tools/Build Project AllScene iOS")]
	public static void BuildProjectAllSceneiOS()
	{
		EditorUserBuildSettings.SwitchActiveBuildTarget( BuildTarget.iPhone );
		string[] allScene = new string[EditorBuildSettings.scenes.Length];
		int i = 0;
		foreach( EditorBuildSettingsScene scene in EditorBuildSettings.scenes ){
			allScene[i] = scene.path;
			i++;
		}
		
		BuildOptions opt = BuildOptions.SymlinkLibraries |
			BuildOptions.AllowDebugging |
				BuildOptions.ConnectWithProfiler |
				BuildOptions.Development;
		
		//BUILD for Device
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
		PlayerSettings.bundleIdentifier = "jp.co.inis.golf";
		PlayerSettings.statusBarHidden = true;
		string errorMsg_Device = BuildPipeline.BuildPlayer( 
		                                                   allScene,
		                                                   "_Test_Sandbox",
		                                                   BuildTarget.iPhone,
		                                                   opt
		                                                   );
		
		if (string.IsNullOrEmpty(errorMsg_Device)){

			Debug.Log(" --->> ");
		}
		else{
			//エラー処理適当に		
			Debug.Log(" --->> ");
		}

		//BUILD for Simulator
		PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
		//あとはDeviceビルドと同様に。
	}


}


