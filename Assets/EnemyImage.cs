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
	private List<Sprite> _bossSpriteList;

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
		gameObject.transform.localScale = Vector3.one;
	}

	public void SetRandomSpriteBoss(bool isShigeru)
	{
		int spriteIndex = (isShigeru) ? 0 : Random.Range (0, _bossSpriteList.Count);
		var sprite = _bossSpriteList[spriteIndex];
		_image.sprite = sprite;
		gameObject.transform.localScale = Vector3.one;
	}

	public void SetDark(float value)
	{
		float alpha = 190 * value;

		_currentDarkColor = new Color (100f/255f,48f/255f,0f,alpha/255f);
		_darkMaskImage.color = _currentDarkColor;

		ShakeGameObject (gameObject);
	}

	private void ShakeGameObject(GameObject target)
	{		
		target.transform.localScale = Vector3.one;
		LeanTween.scale (target, new Vector3 (1.05f, 1.05f, 1.0f), 0.16f).setEaseShake ();
	}
}
