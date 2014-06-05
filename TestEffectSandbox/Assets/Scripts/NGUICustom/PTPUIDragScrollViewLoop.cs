using UnityEngine;
using System.Collections;

#pragma warning disable 414

public class PTPUIDragScrollViewLoop : UIDragScrollView {
	
	Bounds mBounds;
	bool mCalculatedBounds = false;
	
	private bool viewStatus = false;
	private PTPUIGridLoop grid;// = NGUITools.FindInParents<PTPUIGridLoop>(gameObject);
	
	public bool updateLocked = false;
	
	public Bounds bounds
	{
		get
		{
			if (!mCalculatedBounds)
			{
				mCalculatedBounds = true;
				mBounds = NGUIMath.CalculateRelativeWidgetBounds(mTrans, mTrans);
				//print( "Bounds.min:" + mBounds.min );
				//print( "Bounds.max:" + mBounds.max );
			}
			return mBounds;
		}
	}
	
	public void UpdateBounds()
	{
		mCalculatedBounds = false;
	}
	
	// Use this for initialization
	//	void Start() 
	//	{
	//	}
	
	// Update is called once per frame
	//	void Update () {
	//	}
	
	public void InitGrid( PTPUIGridLoop parentGrid )
	{
		grid = parentGrid;
	}
	
	void Update()
	{
		if( updateLocked )
		{
			print( "****** updateLocked!!!!" );
			return;
		}
		
		if( ( mScroll != null ) && ( grid != null ) )
		{
			//if( mScroll.currentMomentum.magnitude > 0.0001f )
			{
				Bounds transBounds = new Bounds();
				Vector3 transMin = mTrans.localPosition;
				Vector3 transMax = mTrans.localPosition;
				transMin.y -= (bounds.size.y * 0.5f);
				transMax.y += (bounds.size.y * 0.5f);
				transMax.x += bounds.size.x;
				transBounds.SetMinMax( transMin, transMax );
				//transBounds.SetMinMax( mTrans.localPosition, (mTrans.localPosition + bounds.size) );
				
				Bounds clipBounds = new Bounds();
				//	clipBounds.min = new Vector3( mScroll.panel.finalClipRegion.x - grid.marginSpace_Under, mScroll.panel.finalClipRegion.y - grid.marginSpace_Under, 0.0f );
				//	clipBounds.max = new Vector3( mScroll.panel.finalClipRegion.x + mScroll.panel.finalClipRegion.z + grid.marginSpace_Over, mScroll.panel.finalClipRegion.y + mScroll.panel.finalClipRegion.w + grid.marginSpace_Over, 0.0f );
				clipBounds.min = new Vector3( mScroll.panel.finalClipRegion.x - grid.marginSpace_Under, mScroll.panel.finalClipRegion.y - (mScroll.panel.finalClipRegion.w * 0.5f) - grid.marginSpace_Under, 0.0f );
				clipBounds.max = new Vector3( mScroll.panel.finalClipRegion.x + mScroll.panel.finalClipRegion.z + grid.marginSpace_Over, mScroll.panel.finalClipRegion.y + (mScroll.panel.finalClipRegion.w * 0.5f) + grid.marginSpace_Over, 0.0f );
				
				
				//print( "transBounds.min = " + transBounds.min + "    transBounds.max = " + transBounds.max );
				//print( "                         clipBounds.min = " + clipBounds.min  + "    clipBounds.max = " + clipBounds.max );
				//print( "transBounds.max.x = " + transBounds.max.x );
				//print( "clipBounds.min.x = " + clipBounds.min.x );
				//print( " bounds.size " + bounds.size );
				//print( mScroll.currentMomentum );
				
				//if( mScroll.currentMomentum.x > 0.0001f ) //right drag
				{
					if( transBounds.min.x > clipBounds.max.x )
					{
						//print( "StepBack  " + (transBounds.min.x - clipBounds.max.x) + "   " + (int)((transBounds.min.x - clipBounds.max.x) / bounds.size.x) );
						grid.StepDownOrRight();
					}
				}
				//else if( mScroll.currentMomentum.x < -0.0001f ) //left drag
				{
					if( transBounds.max.x < clipBounds.min.x )
					{
						//print( "StepForward   " + (clipBounds.min.x - transBounds.max.x) + "   " + (int)((clipBounds.min.x - transBounds.max.x) / bounds.size.x) );
						grid.StepUpOrLeft();
					}
				}
				//else if( mScroll.currentMomentum.y > 0.0001f )
				{
					if( transBounds.min.y > clipBounds.max.y )
					{
						//print( "StepForward   " + (transBounds.min.y + clipBounds.max.y) + "   " + (int)((transBounds.min.y + clipBounds.max.y) / bounds.size.y) );
						grid.StepUpOrLeft();
					}
				}
				//else if( mScroll.currentMomentum.y < -0.0001f )
				{
					if( transBounds.max.y < clipBounds.min.y )
					{
						//print( "StepBack   " + (clipBounds.min.y + transBounds.max.y) + "   " + (int)((clipBounds.min.y + transBounds.max.y) / bounds.size.y) );
						grid.StepDownOrRight();
					}
				}
			}
		}
		
		/*LineRenderer line = GetComponent<LineRenderer>();
		Vector3 temp = bounds.min;
		line.SetPosition( 0, temp );
		temp.x = bounds.min.x;
		temp.y = bounds.max.y;
		line.SetPosition( 1, temp );  
		temp = bounds.max;
		line.SetPosition( 2, temp );
		temp.x = bounds.max.x;
		temp.y = bounds.min.y;
		line.SetPosition( 3, temp );
		temp.x = bounds.min.x;
		temp.y = bounds.max.y;
		line.SetPosition( 4, temp );*/
	}
	
}
