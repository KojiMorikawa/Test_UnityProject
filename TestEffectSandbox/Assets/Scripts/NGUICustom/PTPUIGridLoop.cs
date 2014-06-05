//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#pragma warning disable 414

/// <summary>
/// All children added to the game object with this script will be repositioned to be on a grid of specified dimensions.
/// If you want the cells to automatically set their scale based on the dimensions of their content, take a look at UITable.
/// </summary>

[AddComponentMenu("NGUI/Interaction/GridLoop")]
public class PTPUIGridLoop : UIWidgetContainer
{
	public delegate void OnReposition ();
	
	public enum Arrangement
	{
		Horizontal,
		Vertical,
	}
	
	/// <summary>
	/// Type of arrangement -- vertical or horizontal.
	/// </summary>
	
	public Arrangement arrangement = Arrangement.Horizontal;
	
	/// <summary>
	/// Maximum children per line.
	/// If the arrangement is horizontal, this denotes the number of columns.
	/// If the arrangement is vertical, this stands for the number of rows.
	/// </summary>
	
	//	public int maxPerLine = 0;
	
	/// <summary>
	/// The width of each of the cells.
	/// </summary>
	
//	public float cellWidth = 200f;
	public float padding = 0.0f;
	
	/// <summary>
	/// The height of each of the cells.
	/// </summary>
	
//	public float cellHeight = 200f;
	
	/// <summary>
	/// Whether the grid will smoothly animate its children into the correct place.
	/// </summary>
	
		public bool animateSmoothly = false;
	
	/// <summary>
	/// Whether the children will be sorted alphabetically prior to repositioning.
	/// </summary>
	
	//	public bool sorted = false;
	
	/// <summary>
	/// Whether to ignore the disabled children or to treat them as being present.
	/// </summary>
	
	public bool hideInactive = true;
	
	/// <summary>
	/// Callback triggered when the grid repositions its contents.
	/// </summary>
	
	public OnReposition onReposition;
	
	/// <summary>
	/// List item prefab
	/// </summary>
	
	public Object listItemPrefab;
	
	/// <summary>
	/// 
	/// </summary>
	
	public int margin = 6; 
	public Vector3 itemFirstPos;
	public bool reverseArrangement = false;
	public bool delayedUpdate = false;
	
	/// <summary>
	/// Reposition the children on the next Update().
	/// </summary>
	
	//	public bool repositionNow { set { if (value) { mReposition = true; enabled = true; } } }
	public bool repositionPrev { set { if (value) { mReposition = true; enabled = true; mNext = false;} } }
	public bool repositionNext { set { if (value) { mReposition = true; enabled = true; mNext = true; } } }
	
	bool mStarted = false;
	bool mReposition = false;
	bool mNext = false;
	UIPanel mPanel;
	UIScrollView mDrag;
	bool mInitDone = false;
	
	int mTopIndex = 0;
	int mKeepNum;
	int mAllItemNum = 30;//temp
	List<GameObject> mChildren = new List<GameObject>();

//	int IdCounter = 0;
	bool bDragDrop = false;

	//protected float mMarginSpace;
	protected float mMarginSpace_Under;
	protected float mMarginSpace_Over;
	
	//GameObject ScrollView;
	//UIScrollView ScrollView;
	
	public int   allItemNum  { set { mAllItemNum = value;  } get{ return mAllItemNum; } }
	//public float marginSpace { get { return mMarginSpace; } }
	public float marginSpace_Under{ get{ return mMarginSpace_Under; } }
	public float marginSpace_Over { get{ return mMarginSpace_Over;  } }
	
	public void SetupMenu( int itemNum, bool bdragDrop = false )
	{
		bDragDrop = bdragDrop;
		if (!mInitDone) Init( itemNum );
		mStarted = true;
		Reposition();
		enabled = false;
	}
	
	public void CleanupMenu()
	{
		foreach( GameObject obj in mChildren )
		{
			Destroy( obj );
		}
		mChildren.Clear();
		mInitDone = false;
	}
	
	void Init ( int itemNum )
	{
		mInitDone = true;
		mPanel = NGUITools.FindInParents<UIPanel>(gameObject);
		mDrag = NGUITools.FindInParents<UIScrollView>(gameObject);
		
		mTopIndex = 0;
		mDrag.transform.localPosition = new Vector3( 0.0f, 0.0f, 0.0f );
		mDrag.panel.clipOffset = new Vector3( 0.0f, 0.0f, 0.0f );
		mAllItemNum = itemNum;
		
		if( listItemPrefab != null )
		{
			GameObject firstOne = (GameObject)Instantiate( listItemPrefab );
			firstOne.transform.parent = gameObject.transform;
			firstOne.AddComponent< PTPUIDragScrollViewLoop >();
			if( bDragDrop )
			{
				firstOne.AddComponent< UIDragDropItem >();
				firstOne.AddComponent< UIDragDropItem >().cloneOnDrag = true;
			}

//			firstOne.GetComponent< ScrollViewItemBase >().Id = IdCounter++;

			firstOne.transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
			firstOne.transform.localPosition = itemFirstPos; // new Vector3( -1000.0f, -1000.0f, 0.0f );
			//print( "first one = " + firstOne.transform.localPosition.ToString() );
			
			mChildren.Add( firstOne );

			PTPUIDragScrollViewLoop item = firstOne.GetComponent<PTPUIDragScrollViewLoop>();
			item.InitGrid( this );
			
			//firstOne.SendMessage( "SetItemNum" );
			int firstIndex = 0;
			firstOne.SendMessage( "SetupListItem", firstIndex );
			
			item.UpdateBounds();

//			cellWidth = (item.bounds.size.x + padding);
//			cellHeight = (item.bounds.size.y + padding);
			
			if( arrangement == Arrangement.Horizontal )
			{
				mKeepNum = (int)( mPanel.finalClipRegion.z / (item.bounds.size.x + padding) + margin );
				//mMarginSpace = item.bounds.size.x * margin * 0.5f;
			}
			else
			{
				mKeepNum = (int)( mPanel.finalClipRegion.w / (item.bounds.size.y + padding) + margin );
				//mMarginSpace = item.bounds.size.y * margin * 0.5f;
			}
			
			//mAllItemNum = 5;
			
			if( mKeepNum > mAllItemNum )
			{
				mKeepNum = mAllItemNum;
			}
		}
		else
		{
			//error
			print( "I need prefab!" );
			return;
		}
		
		/*
		for( int index = 1; index < mKeepNum; ++index )
		{
			Vector3 lastPos = GetLastPosition();

			GameObject obj = (GameObject)Instantiate( listItemPrefab );
			obj.transform.parent = gameObject.transform;

			obj.transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );

			LS4UIDragScrollViewLoop item = obj.GetComponent<LS4UIDragScrollViewLoop>();
			item.InitGrid( this );

			obj.SendMessage( "SetupListItem", index );

			item.UpdateBounds();

			Vector3 pos = new Vector3( 0.0f, 0.0f, 0.0f );
			if( reverseArrangement )
			{
				if( arrangement == Arrangement.Horizontal )
				{
					pos.x = lastPos.x - padding - (item.bounds.size.x * 0.5f) - item.bounds.center.x;
				}
				else
				{
					pos.y = lastPos.y + padding + (item.bounds.size.y * 0.5f) - item.bounds.center.y;
				}
			}
			else
			{
				if( arrangement == Arrangement.Horizontal )
				{
					pos.x = lastPos.x + padding + (item.bounds.size.x * 0.5f) - item.bounds.center.x;
				}
				else
				{
					pos.y = lastPos.y - padding - (item.bounds.size.y * 0.5f) - item.bounds.center.y;
				}
			}
			obj.transform.localPosition = pos;

			mChildren.Add( obj );
		}

		UpdateMargin();

		if( reverseArrangement )
		{
			mDrag.SetDragAmount( 1.0f, 1.0f, false );
			mDrag.SetDragAmount( 1.0f, 1.0f, true );
		}
		else
		{
			mDrag.SetDragAmount( 0.0f, 0.0f, false );
			mDrag.SetDragAmount( 0.0f, 0.0f, true );
		}
*/
		
		StartCoroutine( InitDelayed() );
	}
	
	private IEnumerator InitDelayed()
	{
		List<GameObject> children = new List<GameObject>();
		for( int index = 1; index < mKeepNum; ++index )
		{
			GameObject obj = (GameObject)Instantiate( listItemPrefab );
			obj.AddComponent< PTPUIDragScrollViewLoop >();
			if( bDragDrop )
			{
				obj.AddComponent< UIDragDropItem >();
				obj.AddComponent< UIDragDropItem >().cloneOnDrag = true;
			}
			obj.transform.parent = gameObject.transform;
			obj.transform.localScale = new Vector3( 1.0f, 1.0f, 1.0f );
//			obj.GetComponent< ScrollViewItemBase >().Id = IdCounter++;
			
			PTPUIDragScrollViewLoop item = obj.GetComponent<PTPUIDragScrollViewLoop>();
			item.InitGrid( this );
			
			obj.SendMessage( "SetupListItem", index );
			
			children.Add( obj );
		}
		
		// Wait for next frame
		if( delayedUpdate )
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
		}
		
		// Following code will be executed in the next frame
		//		mChildren[0].transform.localPosition = itemFirstPos;
		mChildren[0].GetComponent<PTPUIDragScrollViewLoop>().UpdateBounds();
		
		while( children.Count > 0 )
		{
			Vector3 lastPos = GetLastPosition();
			
			GameObject obj = children[0];
			children.RemoveAt( 0 );
			
			PTPUIDragScrollViewLoop item = obj.GetComponent<PTPUIDragScrollViewLoop>();
			item.UpdateBounds();
			
			Vector3 pos = new Vector3( 0.0f, 0.0f, 0.0f );
			if( reverseArrangement )
			{
				if( arrangement == Arrangement.Horizontal )
				{
					pos.x = lastPos.x - padding - (item.bounds.size.x * 0.5f) - item.bounds.center.x;
				}
				else
				{
					pos.y = lastPos.y + padding + (item.bounds.size.y * 0.5f) - item.bounds.center.y;
				}
			}
			else
			{
				if( arrangement == Arrangement.Horizontal )
				{
					pos.x = lastPos.x + padding + (item.bounds.size.x * 0.5f) - item.bounds.center.x;
				}
				else
				{
					pos.y = lastPos.y - padding - (item.bounds.size.y * 0.5f) - item.bounds.center.y;
				}
			}
			obj.transform.localPosition = pos;
			//print( "pos " + pos.ToString()  );
			
			mChildren.Add( obj );
		}
		
		UpdateMargin();
		
		// Wait for next frame
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		
		// Reset scroll position instead of calling ResetPosition()
		if( reverseArrangement )
		{
			mDrag.SetDragAmount( 1.0f, 1.0f, false );
			mDrag.SetDragAmount( 1.0f, 1.0f, true );
		}
		else
		{
			mDrag.SetDragAmount( 0.0f, 0.0f, false );
			mDrag.SetDragAmount( 0.0f, 0.0f, true );
		}
	}
	
	Vector3 GetTopPosition()
	{
		Transform temp = mChildren[0].transform;
		PTPUIDragScrollViewLoop item = mChildren[0].GetComponent<PTPUIDragScrollViewLoop>();
		Vector3 tempVec3 = temp.localPosition;
		
		if( reverseArrangement )
		{
			tempVec3.x += (item.bounds.size.x * 0.5f);
			tempVec3.y -= (item.bounds.size.y * 0.5f);
		}
		else
		{
			tempVec3.x -= (item.bounds.size.x * 0.5f);
			tempVec3.y += (item.bounds.size.y * 0.5f);
		}
		tempVec3.x += item.bounds.center.x;
		tempVec3.y += item.bounds.center.y;
		return tempVec3;
		//return temp.localPosition - ( item.bounds.size * 0.5f );
	}
	
	Vector3 GetLastPosition()
	{
		Transform temp = mChildren[ mChildren.Count-1 ].transform;
		PTPUIDragScrollViewLoop item = mChildren[ mChildren.Count-1 ].GetComponent<PTPUIDragScrollViewLoop>();
		Vector3 tempVec3 = temp.localPosition;
		
		if( reverseArrangement )
		{
			tempVec3.x -= (item.bounds.size.x * 0.5f);
			tempVec3.y += (item.bounds.size.y * 0.5f);
		}
		else
		{
			tempVec3.x += (item.bounds.size.x * 0.5f);
			tempVec3.y -= (item.bounds.size.y * 0.5f);
		}
		tempVec3.x += item.bounds.center.x;
		tempVec3.y += item.bounds.center.y;
		return tempVec3;
	}
	
	public void StepUpOrLeft()
	{
		if( reverseArrangement )
		{
			StartCoroutine( StepBack() );
		}
		else
		{
			StartCoroutine( StepForward() );
		}
	}
	
	public void StepDownOrRight()
	{
		if( reverseArrangement )
		{
			StartCoroutine( StepForward() );
		}
		else
		{
			StartCoroutine( StepBack() );
		}
	}
	
	private IEnumerator StepForward()
	{
		if( (mTopIndex + mKeepNum) >= mAllItemNum ) // list end
		{
			//print( "List end" );
			yield break;
		}
		
		Vector3 lastPos = GetLastPosition();
		
		mTopIndex++;
		
		//print( "Top Index = " + mTopIndex );
		
		GameObject obj = mChildren[0];
		mChildren.Remove( obj );
		mChildren.Add( obj );
		
		obj.SendMessage( "SetupListItem", mTopIndex + mKeepNum -1 );
		
		PTPUIDragScrollViewLoop item = obj.GetComponent<PTPUIDragScrollViewLoop>();
		item.updateLocked = true;
		
		// Wait for next frame
		if( delayedUpdate )
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
		}
		
		// Following code will be executed in the next frame
		item.updateLocked = false;
		item.UpdateBounds();
		
		Vector3 pos = new Vector3( 0.0f, 0.0f, 0.0f );
		if( reverseArrangement )
		{
			if( arrangement == Arrangement.Horizontal )
			{
				pos.x = lastPos.x - (padding + (item.bounds.size.x * 0.5f)) - item.bounds.center.x;
			}
			else
			{
				pos.y = lastPos.y + (padding + (item.bounds.size.y * 0.5f)) - item.bounds.center.y;
			}
		}
		else
		{
			if( arrangement == Arrangement.Horizontal )
			{
				pos.x = lastPos.x + (padding + (item.bounds.size.x * 0.5f)) - item.bounds.center.x;
			}
			else
			{
				pos.y = lastPos.y - (padding + (item.bounds.size.y * 0.5f)) - item.bounds.center.y;
			}
		}
		obj.transform.localPosition = pos;
		
		//ScrollView.RecalculateBounds();
		
		UpdateMargin();
	}
	
	private IEnumerator StepBack()
	{
		if( mTopIndex <= 0 ) // list top
		{
			//print( "List top" );
			yield break;
		}
		
		Vector3 topPos = GetTopPosition();
		
		mTopIndex--;
		
		//print( "Top Index = " + mTopIndex );
		
		GameObject obj = mChildren[ mChildren.Count-1 ];
		mChildren.Remove( obj );
		mChildren.Insert( 0, obj );
		
		obj.SendMessage( "SetupListItem", mTopIndex );
		
		PTPUIDragScrollViewLoop item = obj.GetComponent<PTPUIDragScrollViewLoop>();
		item.updateLocked = true;
		
		// Wait for next frame
		if( delayedUpdate )
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
		}
		
		// Following code will be executed in the next frame
		item.updateLocked = false;
		item.UpdateBounds();
		
		Vector3 pos = new Vector3( 0.0f, 0.0f, 0.0f );
		if( reverseArrangement )
		{
			if( arrangement == Arrangement.Horizontal )
			{
				pos.x = topPos.x + (padding + (item.bounds.size.x*0.5f)) - item.bounds.center.x;
			}
			else
			{
				pos.y = topPos.y - (padding + (item.bounds.size.y*0.5f)) - item.bounds.center.y;
			}
		}
		else
		{
			if( arrangement == Arrangement.Horizontal )
			{
				pos.x = topPos.x - (padding + (item.bounds.size.x*0.5f)) - item.bounds.center.x;
			}
			else
			{
				pos.y = topPos.y + (padding + (item.bounds.size.y*0.5f)) - item.bounds.center.y;
			}
		}
		obj.transform.localPosition = pos;
		
		//ScrollView.RecalculateBounds();
		
		UpdateMargin();
	}
	
	void UpdateMargin()
	{
		int index;
		PTPUIDragScrollViewLoop item;
		
		mMarginSpace_Under = 0.0f;
		mMarginSpace_Over  = 0.0f;
		for( index = 0; index < ( margin/2 ); ++index )
		{
			if( index < mChildren.Count )
			{
				item = mChildren[index].GetComponent<PTPUIDragScrollViewLoop>();
				if( arrangement == Arrangement.Horizontal )
				{
					mMarginSpace_Under += item.bounds.size.x;
				}
				else
				{
					mMarginSpace_Under += item.bounds.size.y;
				}
			}
			
			if( (mChildren.Count - index - 1) >= 0 )
			{
				item = mChildren[mChildren.Count - index - 1].GetComponent<PTPUIDragScrollViewLoop>();
				if( arrangement == Arrangement.Horizontal )
				{
					mMarginSpace_Over += item.bounds.size.x;
				}
				else
				{
					mMarginSpace_Over += item.bounds.size.y;
				}
			}
		}
		
		print( "UpdateMargin  Under = " + marginSpace_Under + "    Over = " + marginSpace_Over );
	}
	
	void Start ()
	{
		/*		ScrollView = gameObject.transform.parent.gameObject.GetComponent<UIScrollView>();
		if (!mInitDone) Init();
		mStarted = true;*/
		bool smooth = animateSmoothly;
		animateSmoothly = false;
		//Reposition();
		animateSmoothly = smooth;
//		enabled = false;
	}
	
	void Update ()
	{
		if (mReposition) Reposition();
		enabled = false;
	}
	
	static public int SortByName (Transform a, Transform b) { return string.Compare(a.name, b.name); }
	
	/// <summary>
	/// Recalculate the position of all elements within the grid, sorting them alphabetically if necessary.
	/// </summary>
	
	[ContextMenu("Execute")]
	public void Reposition ()
	{
		if (Application.isPlaying && !mStarted)
		{
			mReposition = true;
			return;
		}
		
		//if (!mInitDone) Init();
		
		mReposition = false;
		Transform myTrans = transform;
		
		/*
		int x = 0;
		int y = 0;
		//if (sorted)
		if( false )
		{
			List<Transform> list = new List<Transform>();

			for (int i = 0; i < myTrans.childCount; ++i)
			{
				Transform t = myTrans.GetChild(i);
				if (t && (!hideInactive || NGUITools.GetActive(t.gameObject))) list.Add(t);
			}
			list.Sort(SortByName);

			for (int i = 0, imax = list.Count; i < imax; ++i)
			{
				Transform t = list[i];

				if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;

				float depth = t.localPosition.z;
				Vector3 pos = (arrangement == Arrangement.Horizontal) ?
					new Vector3(cellWidth * x, -cellHeight * y, depth) :
					new Vector3(cellWidth * y, -cellHeight * x, depth);

				if (animateSmoothly && Application.isPlaying)
				{
					SpringPosition.Begin(t.gameObject, pos, 15f);
				}
				else t.localPosition = pos;
		
				if (++x >= maxPerLine && maxPerLine > 0)
				{
					x = 0;
					++y;
				}
			}
/*		}
		else
		{
			/*for (int i = 0; i < myTrans.childCount; ++i)
			{
				Transform t = myTrans.GetChild(i);

				if (!NGUITools.GetActive(t.gameObject) && hideInactive) continue;

				float depth = t.localPosition.z;
				Vector3 pos = (arrangement == Arrangement.Horizontal) ?
					new Vector3(cellWidth * x, -cellHeight * y, depth) :
					new Vector3(cellWidth * y, -cellHeight * x, depth);

				if (animateSmoothly && Application.isPlaying)
				{
					SpringPosition.Begin(t.gameObject, pos, 15f);
				}
				else t.localPosition = pos;

				if (++x >= maxPerLine && maxPerLine > 0)
				{
					x = 0;
					++y;
				}
			}
		}
*/
		
		if (mDrag != null)
		{
			mDrag.UpdateScrollbars(true);
			mDrag.RestrictWithinBounds(true);
		}
		else if (mPanel != null)
		{
			mPanel.ConstrainTargetToBounds(myTrans, true);
		}
		
		if (onReposition != null)
			onReposition();
	}
}
