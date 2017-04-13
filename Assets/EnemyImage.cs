using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EnemyImage : MonoBehaviour {

	[SerializeField]
	private Image _image;

	[SerializeField]
	private Image _darkMaskImage;

	[SerializeField]
	private List<Sprite> _zakoSpriteList;

	[SerializeField]
	private Color _currentDarkColor = Color.white;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetRandomSpriteZako()
	{
		var sprite = _zakoSpriteList[Random.Range(0,_zakoSpriteList.Count)];
		_image.sprite = sprite;
	}

	public void SetDark(float value)
	{
		float alpha = 190 * value;

		_currentDarkColor = new Color (100f/255f,48f/255f,0f,alpha/255f);
		_darkMaskImage.color = _currentDarkColor;
	}
}
