using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class DialogManager : SingletonMonoBehaviour< DialogManager >
{

	public enum DialogType
	{
		CommonDialog_OK,
		CommonDialog_OKCancel,

		Count
	}

	private string[] DialogNames = new string[]
	{
//		"CommonDialog",
		"CommonOKDialog",
		"CommonOKCancelDialog"
	};

	enum DialogState
	{
		Show,
		Finished
	}


	public struct DialogStatus
	{
		public int								Id{ get; set; }
//		private DialogState						_state;
//		public DialogState						ShowState{ get{ return _state; } set{ _state = value; } }
		private CommonDialog.DialogResult	_result;
		public CommonDialog.DialogResult	Result{ get{ return _result; } set{ _result = value; } }

		public DialogStatus( int id )
		{
			Id = id;
//			_state = DialogState.Show;
			_result = CommonDialog.DialogResult.None;
		}

	}

	private const string 			UIRootName = "UI Root";

	private GameObject				DialogPanel;

	private List< DialogStatus >	DialogStatusList;
	private int						IdCounter;
	private DialogStatus			UpdateDialogStatus = new DialogStatus();

	CommonDialog.DelegateButtonPressed	ButtonDelegate;


	void Awake()
	{
		if (this != Instance)
		{
			Destroy(this);
			return;
		}
        DontDestroyOnLoad( gameObject );
		
		DialogStatusList = new List< DialogStatus >();

	}

	// Use this for initialization
	void Start () {
	
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FindUIRootCameraAndCreateDialogPanel()
	{
		if( DialogPanel == null )
		{
//			UIRoot root = NGUITools.FindInParents< UIRoot >( gameObject );
			GameObject root = GameObject.Find( UIRootName );
			if( root != null )
			{
				UICamera uiCamera = root.transform.GetComponentInChildren< UICamera >();
				if( uiCamera != null )
				{
					GameObject dialogPanel = new GameObject();
					dialogPanel.name = "DialogPanel";

					dialogPanel.AddComponent< UIPanel >();
					dialogPanel.transform.parent = uiCamera.transform;

					dialogPanel.transform.localPosition = Vector3.zero;
					dialogPanel.transform.localScale = new Vector3( 1, 1, 1 );
					dialogPanel.layer = uiCamera.gameObject.layer;

					DialogPanel = dialogPanel;
					DontDestroyOnLoad( dialogPanel.gameObject );
				}
			}
			else
			{
				Debug.Log( ">> you didn't find uiroot!" );
			}
		}
	}

	CommonDialog.DialogResult GetDialogResult( int id )
	{
		foreach( DialogStatus status in DialogStatusList )
		{
			if( status.Id == id )
			{
				if( status.Result != CommonDialog.DialogResult.None )
				{
					return status.Result;
				}
				break;
			}
		}
		return CommonDialog.DialogResult.None;
	}

	CommonDialog CreateCommonDialogInstance( string dialogName )
	{
		UnityEngine.Object loadedObject = Resources.Load( "Prefabs/HUD/" + dialogName );
		if( loadedObject == null )
		{
			Debug.Assert( false, ">> there is no prefab : Prefabs/HUD/" + dialogName );
		}
		else
		{
			GameObject dialogObject = ( GameObject )Instantiate( loadedObject );
			Debug.Log( ">> create dialogObject : " + dialogObject );

			FindUIRootCameraAndCreateDialogPanel();

			dialogObject.transform.parent = DialogPanel.transform;
			dialogObject.AddComponent< CommonDialog >();
			return dialogObject.GetComponent< CommonDialog >();
		}
		return null;
	}

	public int ShowDialog( DialogType type, string text, string okButton = "OK", string cancelButton = "Cancel",
	                      CommonDialog.DelegateButtonPressed buttonDelegate = null, CommonDialog.DialogPriority priority = CommonDialog.DialogPriority.Common )
	{
		CommonDialog dialog = CreateCommonDialogInstance( DialogNames[ ( int )type ] );
		if( dialog != null )
		{
			DialogStatus status = new DialogStatus( IdCounter++ );
			ButtonDelegate = buttonDelegate;

			dialog.Show( status.Id, text, okButton, cancelButton, OnButtonPressed, priority );

			DialogStatusList.Add( status );
			return status.Id;
		}
		return -1;
	}

	void OnButtonPressed( int id, CommonDialog.DialogResult result )
	{
		Debug.Log( ">> OnDialogButtonPressed id:" + id + " button:" + result );

 		if( ButtonDelegate != null )
		{
			ButtonDelegate( id, result );
		}

		for( int i = 0; i < DialogStatusList.Count; ++i )
		{
			if( DialogStatusList[ i ].Id == id )
			{
				UpdateDialogStatus.Id = id;
				UpdateDialogStatus.Result = result;
				DialogStatusList[ i ] = UpdateDialogStatus;
			}
		}
	}

}
