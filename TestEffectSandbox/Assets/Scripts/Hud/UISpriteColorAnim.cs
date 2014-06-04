using UnityEngine;
using System.Collections;

public class UISpriteColorAnim : MonoBehaviour {

	public Color masterColor = new Color(225f / 255f, 200f / 255f, 150f / 255f, 1f);
	public Color targetColor = new Color(  1f / 255f,   1f /   1f, 150f / 255f, 1f);
	
//	[SerializeField]
	Color TempColor = new Color(0,0,0);
	
//	[SerializeField]
	UISprite sprite = null;
	
	[SerializeField]
	float animSpeed = 1.0f;

	public float timer = 0.0f;
//	public float timer_max = 0.0f;
	float addTime;

	// Use this for initialization
	void Start () {
	
		addTime = 1.0f;
		timer = 0.0f;

		sprite = this.GetComponent<UISprite>();
	}
	
	// Update is called once per frame
	void Update () {

		if (sprite != null){
			sprite.color = TempColor;
			sprite.MarkAsChanged();

			UpdateColorAnim();
		}
	}
	
	void UpdateColorAnim()
	{
		float rate;
		float r, g, b;

		timer += addTime;

		if (timer >= 255.0){
			timer = 255.0f;
			addTime = -animSpeed;
		}
		else if (timer <= 0.0f){
			timer = 0.0f;
			addTime = animSpeed;
		}
		rate = 255.0f - timer;

		r = ((255.0f - masterColor.r) * rate + (masterColor.r * 256.0f)) / 256.0f;
		g = ((255.0f - masterColor.g) * rate + (masterColor.g * 256.0f)) / 256.0f;
		b = ((255.0f - masterColor.b) * rate + (masterColor.b * 256.0f)) / 256.0f;
		
		TempColor.r = r / 255.0f;
		TempColor.g = g / 255.0f;
		TempColor.b = b / 255.0f;
	}
}
