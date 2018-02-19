using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UniRx;



public class EnemyImage : MonoBehaviour {

	public UnityAction OnBossTimeOut;


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

	[SerializeField]
	private Slider _bossTimerSlider;

	[SerializeField]
	private Text _bossTimerText;



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

		_bossTimerSlider.gameObject.SetActive (false);
	}

	public void SetRandomSpriteBoss(bool isShigeru)
	{
		int spriteIndex = (isShigeru) ? 0 : Random.Range (0, _bossSpriteList.Count);
		var sprite = _bossSpriteList[spriteIndex];
		_image.sprite = sprite;
		gameObject.transform.localScale = Vector3.one;
		_bossTimerSlider.gameObject.SetActive (true);
		StartBossTimer ();

	}

	public void SetDark(float value)
	{
		float alpha = 190 * value;

		_currentDarkColor = new Color (64f/255f,25f/255f,0f,alpha/255f);
		_darkMaskImage.color = _currentDarkColor;

		ShakeGameObject (gameObject);
	}

	private void StartBossTimer()
	{
		float elapsedTime = 0f;
		var bossTimeLimit = 10f;
		Observable.EveryUpdate ().TakeWhile (time => elapsedTime < bossTimeLimit).Subscribe (time => {
			elapsedTime += Time.deltaTime;
			_bossTimerSlider.value = (1-(elapsedTime/bossTimeLimit));
			_bossTimerText.text = string.Format("{0:F1} sec",bossTimeLimit - elapsedTime);
		},()=>{
			if(OnBossTimeOut != null)
			{
				OnBossTimeOut();
			}
		});
	}


	private void ShakeGameObject(GameObject target)
	{		
		target.transform.localScale = Vector3.one;
		LeanTween.scale (target, new Vector3 (1.05f, 1.05f, 1.0f), 0.16f).setEaseShake ();
	}


}
