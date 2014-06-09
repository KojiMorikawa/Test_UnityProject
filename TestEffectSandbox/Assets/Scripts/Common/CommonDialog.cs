using UnityEngine;
using System.Collections;

public class CommonDialog : MonoBehaviour {

	public enum CommonDialogButtonType
	{
		OK,
		OKCancel,

		Count
	}

	public enum DialogPriority
	{
		Common = 1000,
		System = 4000
	}

	public enum DialogResult
	{
		None,
		OK,
		Cancel
	}




	private int			Id;

	private TweenScale	ShowAnim;

	private DialogResult Result;

	protected DelegateButtonPressed ButtonDelegate;
	public delegate void DelegateButtonPressed( int id, DialogResult result );


	void Awake () {

		ShowAnim = gameObject.transform.FindChild( "Frame" ).GetComponent< TweenScale >();
		gameObject.SetActive( false );
	}

	// Use this for initialization
	void Start()
	{
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SetLabel( string path, string text )
	{
		Transform labelTransform = gameObject.transform.FindChild( "Frame/" + path );
		if( labelTransform != null )
		{
			UILabel label = labelTransform.gameObject.GetComponent< UILabel >();
			if( label != null )
			{
				label.text = text;
			}
		}
	}

	private void SetDelegate( string buttonName, EventDelegate.Callback callback )
	{
		Transform buttonTransform = gameObject.transform.FindChild( "Frame/" + buttonName );
		if( buttonTransform != null )
		{
			UIButton button = buttonTransform.gameObject.GetComponent< UIButton >();
			if( button != null )
			{
				EventDelegate.Add( button.onClick, callback );
			}
		}
	}

	public void Show( int id, string text, string okButton, string cancelButton, DelegateButtonPressed buttonDelegate, DialogPriority priority )
	{
		GetComponent< UIPanel >().depth = ( int )priority + id;
		
		transform.position = new Vector3( 0, 0, 0 );
		transform.localScale = new Vector3( 1, 1, 1 );

		GetComponent< UIWidget >().depth = 100 + id;

		ShowAnim.Play( true );
		gameObject.SetActive( true );

		Id = id;

		ButtonDelegate = buttonDelegate;

		SetLabel ( "Label_Text", text );
		SetLabel( "Button_OK/Label", okButton );
		SetLabel( "Button_Cancel/Label", cancelButton );

		SetDelegate( "Button_OK", OnButtonOKPressed );
		SetDelegate( "Button_Cancel", OnButtonCancelPressed );
	}

	public void Hide()
	{
		EventDelegate.Add( ShowAnim.onFinished, DisableDialog );
		ShowAnim.Play( false );
	}

	private void DisableDialog()
	{
		if( ButtonDelegate != null )
		{
			ButtonDelegate( Id, Result );
		}
		Destroy( gameObject );
	}

	private void OnButtonOKPressed()
	{
		Result = DialogResult.OK;
		Hide();
	}

	private void OnButtonCancelPressed()
	{
		Result = DialogResult.Cancel;
		Hide();
	}

}
