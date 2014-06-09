using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestDebugDeviceInfo : MonoBehaviour {

#if ANDROID_DEBUG
    static Dictionary<string, string> m_dicSysInfo = null;

	// Use this for initialization
	void Start () {
    	GetSysInfo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    private void OnGUI()
    {
        GUILayout.Window(0, new Rect( 30, 30, 400, 350), DrawWindow, "Device Info Menu");
   
    }

    private void DrawWindow(int id)
    {
        int Lines = 0;

        GUILayout.Button(id.ToString());

        foreach (string key in m_dicSysInfo.Keys)
        {
            GUI.Label(new Rect(12, 40 + 16 * Lines, 380, 32), m_dicSysInfo[key]);
            Lines++;
       }
    }

    private void GetSysInfo()
    {
        if (m_dicSysInfo != null) return;

        m_dicSysInfo = new Dictionary<string, string>();

        m_dicSysInfo.Add("operatingSystem", SystemInfo.operatingSystem);

#if UNITY_IPHONE
        m_dicSysInfo.Add("iPhone.generation", iPhone.generation.ToString());
#endif
        m_dicSysInfo.Add("deviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);
        m_dicSysInfo.Add("deviceModel", SystemInfo.deviceModel);
        m_dicSysInfo.Add("deviceName", SystemInfo.deviceName);
        m_dicSysInfo.Add("graphicsDeviceName", SystemInfo.graphicsDeviceName);
        m_dicSysInfo.Add("graphicsDeviceVendor", SystemInfo.graphicsDeviceVendor);
        m_dicSysInfo.Add("processorType", SystemInfo.processorType);
//        m_dicSysInfo.Add("graphicsMemorySize", SystemInfo.graphicsMemorySize.ToString());
//        m_dicSysInfo.Add("systemMemorySize", SystemInfo.systemMemorySize.ToString());

        m_dicSysInfo.Add("ScreenWidth", string.Format("ScreenWidth  = {0}", Screen.width));
        m_dicSysInfo.Add("ScreenHeight", string.Format("ScreenHeight = {0}", Screen.height));

//        return m_dicSysInfo;
    }
#endif

}
