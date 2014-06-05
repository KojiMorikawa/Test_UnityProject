// ============================================================================
// I performed reference of documents of rodostw(Twitter name.)
// http://qiita.com/rodostw/items/39183e62ed2a1f52f690
// ============================================================================
//#if !UNITY_EDITOR
#define DEBUG_LOG_OVERWRAP
//#endif
using UnityEngine;
using System;

#if DEBUG_LOG_OVERWRAP
public static class Debug
{
    static public void Break(){
        if( IsEnable() )    UnityEngine.Debug.Break();
    }

    static public void Log( object message ){
        if( IsEnable() ){
            UnityEngine.Debug.Log( message );
        }
    }
    static public void Log( object message, UnityEngine.Object context ){
        if( IsEnable() ) {
            UnityEngine.Debug.Log( message, context );
        }
    }

    static bool IsEnable(){ return UnityEngine.Debug.isDebugBuild; }

    public static void LogWarning( object message ){}
	public static void LogWarning( object message, UnityEngine.Object context ){}
    public static void LogError( object message ){}
	public static void LogError( object message, UnityEngine.Object context ){}
    public static void DrawLine(Vector3 start, Vector3 end){}
    public static void DrawLine(Vector3 start, Vector3 end, Color color){}
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration){}
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest){}
    public static void DrawRay(Vector3 start, Vector3 dir){}
    public static void DrawRay(Vector3 start, Vector3 dir, Color color){}
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration){}
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest){}



	public static void Assert( bool condition )
	{
		if ( !condition )
		{
			throw new Exception();
		}
	}
	
	public static void Assert( bool condition, string message )
	{
		if ( !condition )
		{
			throw new Exception( message );
		}
	}

	public static void Assert( bool condition, Func< string > getMessage )
	{
		if (! condition )
		{
			throw new Exception( getMessage() );
		}
	}
}
#endif
