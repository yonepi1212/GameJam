using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Random = UnityEngine.Random;
using System;
using UniRx;

public class GameManager : MonoBehaviour {


	public class Status {
		public IntReactiveProperty StageNumber;
		public DoubleReactiveProperty TapDamage;
		public DoubleReactiveProperty CurrentCoin;
		public IntReactiveProperty CurrentMyLevel;
		public DoubleReactiveProperty NextLevelCoin;
	}

	public class Enemy
	{
		public DoubleReactiveProperty MaxHp;
		public DoubleReactiveProperty CurrentHp;
		public EnemyType Type;
		public enum EnemyType
		{
			Zako,
			Boss
		};
		public double Coin;
	}

	public Status CurrentStatus;

	public Enemy CurrentEnemy;

	public Enemy NextEnemy;

	[SerializeField]
	private Button _levelUpButton;

	[SerializeField]
	private Text _stageNumberText;

	[SerializeField]
	private Text _hpText;

	[SerializeField]
	private Slider _stageEnemyHpSlider;

	[SerializeField]
	private Text _currentLevelText;

	[SerializeField]
	private Text _currentCoinText;

	[SerializeField]
	private Text _nextCoinText;

	[SerializeField]
	private Text _currentTapDamageText;

	[SerializeField]
	private EnemyImage _enemyImage;

	[SerializeField]
	private Button _backButton;

	private float _bossHpGain = 6f;

	// Use this for initialization
	void Awake () {

		_levelUpButton.onClick.AddListener (LevelUp);
		_backButton.onClick.AddListener (() => {
			SceneManager.LoadScene("TitleScene");
		});

		// TODO:PlayerPrefsから読み込み

		int stageNum = 1;
		double tapDamage = 1d;
		double currentCoin = 0d;
		int currentMyLevel = 1;
		double nextLevelCoin = 1d;
		double lastCharacterHp = 10d;


		if(PlayerPrefs.HasKey("LastStageNumber"))
			stageNum = PlayerPrefs.GetInt ("LastStageNumber");

		if(PlayerPrefs.HasKey("LastMyLevel"))
			currentMyLevel = PlayerPrefs.GetInt ("LastMyLevel");

		if(PlayerPrefs.HasKey("LastMyCoin"))
			currentCoin = PlayerPrefs.GetFloat ("LastMyCoin");
		
		if(PlayerPrefs.HasKey("LastMyNextCoin"))
			nextLevelCoin = PlayerPrefs.GetFloat ("LastMyNextCoin");

		if(PlayerPrefs.HasKey("LastTapDamage"))
			tapDamage = PlayerPrefs.GetFloat ("LastTapDamage");

		if(PlayerPrefs.HasKey("LastCharacterHp"))
			lastCharacterHp = PlayerPrefs.GetFloat ("LastCharacterHp");



		CurrentStatus = new Status(){
			StageNumber = new IntReactiveProperty(stageNum),
			TapDamage = new DoubleReactiveProperty(tapDamage),
			CurrentCoin =  new DoubleReactiveProperty(currentCoin),
			CurrentMyLevel = new IntReactiveProperty(currentMyLevel),
			NextLevelCoin = new DoubleReactiveProperty(nextLevelCoin),
		};

		if (stageNum == 1) {
			CurrentEnemy = GenerateFirstEnemy (stageNum, lastCharacterHp);
		} else {
			CurrentEnemy = GenerateEnemy (stageNum, lastCharacterHp);
		}

		var isBoss = (stageNum % 10 == 0);
		if (isBoss) {
			_enemyImage.SetRandomSpriteBoss (false);
		} else {
			_enemyImage.SetRandomSpriteZako ();
		}

		CurrentStatus.StageNumber.Subscribe (number => {
			_stageNumberText.text = String.Format("{0:F0}",number);
			ShakeGameObject(_stageNumberText.gameObject);
			PlayerPrefs.SetInt ("LastStageNumber", number);
		});

		CurrentStatus.CurrentMyLevel.Subscribe(level =>
		{
			_currentLevelText.text = String.Format("{0:F0}", level);
				ShakeGameObject(_currentLevelText.gameObject);
			PlayerPrefs.SetInt ("LastMyLevel", level);

		});

		CurrentStatus.CurrentCoin.Subscribe (coin => {
			_currentCoinText.text = String.Format("{0:F1}",coin);
			ShakeGameObject(_currentCoinText.gameObject);
			PlayerPrefs.SetFloat ("LastMyCoin", (float)coin);

		});

		CurrentStatus.NextLevelCoin.Subscribe (coin => {
			_nextCoinText.text = String.Format("{0:F1}",coin);
			ShakeGameObject(_nextCoinText.gameObject);
			PlayerPrefs.SetFloat ("LastMyNextCoin",  (float)coin);

		});

		CurrentStatus.TapDamage.Subscribe (damage => {
			
			_currentTapDamageText.text = String.Format("{0:F1}",damage);
			ShakeGameObject(_currentTapDamageText.gameObject);
			PlayerPrefs.SetFloat ("LastTapDamage",  (float)damage);

		});
			
		UnityAction bossTimeOut = () => {
			Return10Stage();
		};
		_enemyImage.OnBossTimeOut = bossTimeOut;

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Return10Stage ();
		}
	}

	public void OnTaped()
	{

		CurrentEnemy.CurrentHp.Value -= CurrentStatus.TapDamage.Value;
		ShowDamage (CurrentStatus.TapDamage.Value);

		if (CurrentEnemy.CurrentHp.Value < 0) {

			BreakEnemy ();

			// ステージ番号更新
			var nextStageNumber = CurrentStatus.StageNumber.Value + 1;
			CurrentStatus.StageNumber.Value = nextStageNumber;


			var nextCharacterHp = 0d;
			if (CurrentEnemy.Type == Enemy.EnemyType.Boss) {
				nextCharacterHp = CurrentEnemy.MaxHp.Value / _bossHpGain;
				CurrentEnemy = GenerateEnemy (nextStageNumber,nextCharacterHp);
			} else {
				nextCharacterHp = CurrentEnemy.MaxHp.Value;
				CurrentEnemy = GenerateEnemy (nextStageNumber,nextCharacterHp);
			}

			PlayerPrefs.SetFloat ("LastCharacterHp", (float)nextCharacterHp);

		}

	}

	private void BreakEnemy()
	{
		CurrentStatus.CurrentCoin.Value += CurrentEnemy.Coin;

	}


	private void LevelUp()
	{
		if (CurrentStatus.CurrentCoin.Value >= CurrentStatus.NextLevelCoin.Value) {
			// レベルアップ
			CurrentStatus.TapDamage.Value *= 1.1f;
			CurrentStatus.CurrentCoin.Value -= CurrentStatus.NextLevelCoin.Value;

			CurrentStatus.NextLevelCoin.Value *= 1.1f;
			CurrentStatus.CurrentMyLevel.Value++;
			Debug.Log ("Level Up!!");

		} else {
			// 失敗
			Debug.Log ("No Level Up");

		}
	}

	private Enemy GenerateFirstEnemy(int level, double hp)
	{
		Enemy enemy = new Enemy();
		enemy.MaxHp = new DoubleReactiveProperty(hp);
		enemy.CurrentHp = new DoubleReactiveProperty(hp);
		enemy.Coin = level;
		enemy.Type = (level % 10 == 0) ? Enemy.EnemyType.Boss : Enemy.EnemyType.Zako;

		enemy.CurrentHp.Subscribe(currentHp =>
		{
			float ratio = (float)currentHp / (float)enemy.MaxHp.Value;
			var hpString = String.Format("{0:F1}/{1:F1}", currentHp, enemy.MaxHp.Value);

			_hpText.text = hpString;
			_stageEnemyHpSlider.value = ratio;
			
			_enemyImage.SetDark(1-ratio);
		});

		return enemy;
	}

	private Enemy GenerateEnemy(int level,double prevHp)
	{

		Enemy enemy = new Enemy();
		enemy.Type = (level % 10 == 0) ? Enemy.EnemyType.Boss : Enemy.EnemyType.Zako;

		var enemyHp = (enemy.Type == Enemy.EnemyType.Boss) ? new DoubleReactiveProperty(prevHp * 1.1f * _bossHpGain) : new DoubleReactiveProperty(prevHp * 1.1f);
		enemy.MaxHp = enemyHp;
		enemy.CurrentHp = new DoubleReactiveProperty(enemy.MaxHp.Value);
		enemy.Coin = level;

		enemy.CurrentHp.Subscribe (hp => {
			float ratio = (float)hp / (float)enemy.MaxHp.Value;
	
			//var hpString = hp + "/" + enemy.MaxHp.Value;
			var hpString = String.Format("{0:F1}/{1:F1}",hp,enemy.MaxHp.Value);

			_hpText.text = hpString;
			_stageEnemyHpSlider.value = ratio;

			_enemyImage.SetDark(1-ratio);
		});

		if (enemy.Type == Enemy.EnemyType.Zako) {
			_enemyImage.SetRandomSpriteZako ();
		} else {
			if (level == 10) {
				_enemyImage.SetRandomSpriteBoss (true);
			} else {
				_enemyImage.SetRandomSpriteBoss (false);
			}
		}

		return enemy;
	}

	private void Return10Stage()
	{
		if (CurrentEnemy.Type == Enemy.EnemyType.Boss) {

			// ステージ番号更新
			var nextStageNumber = CurrentStatus.StageNumber.Value - 9;
			CurrentStatus.StageNumber.Value = nextStageNumber;

			var nextCharacterHp = 0d;
			if (CurrentEnemy.Type == Enemy.EnemyType.Boss) {
				nextCharacterHp = (CurrentEnemy.MaxHp.Value / _bossHpGain) / 2.5937f;
				CurrentEnemy = GenerateEnemy (nextStageNumber,nextCharacterHp);
			} else {
				nextCharacterHp = CurrentEnemy.MaxHp.Value / 2.5937f;
				CurrentEnemy = GenerateEnemy (nextStageNumber,nextCharacterHp);
			}

			PlayerPrefs.SetFloat ("LastCharacterHp", (float)nextCharacterHp);
		}
	}

	private void ShakeGameObject(GameObject target)
	{		
		target.transform.localScale = Vector3.one;
		LeanTween.scale (target, new Vector3 (1.05f, 1.05f, 1.05f), 0.16f).setEaseShake ();
	}



	[SerializeField]
	private Font _myFont;
	[SerializeField]
	Transform _areaTopLeft;
	[SerializeField]
	Transform _areaBottomRight;
	[SerializeField]
	Canvas _canvasObject;

	private void ShowDamage(double damage)
	{
		// ダメージ表示オブジェクト生成
		GameObject damage1TextObject = new GameObject ();

		// Textをつける
		damage1TextObject.AddComponent<Text> ();

		// canvas下に(canvasはフィールド宣言)
		damage1TextObject.transform.SetParent (_canvasObject.transform, false);

		// 初期位置（一定範囲内ランダム）areaTopLeft,areaBottomRightはフィールド宣言
		damage1TextObject.transform.position = new Vector3 (
			Random.Range (_areaTopLeft.position.x, _areaBottomRight.position.x),
			Random.Range (_areaBottomRight.position.y, _areaTopLeft.position.y),
			_areaTopLeft.position.z);
		damage1TextObject.transform.localScale = new Vector3 (1f, 1.2f, 1f);


		// ダメージ数値整形
		var damageStr = String.Format("{0:F1}",damage);


		// テキスト編集
		Text damage1Text = damage1TextObject.GetComponent<Text> ();
		damage1Text.text = damageStr;
		damage1Text.font = _myFont;//fontはフィールドで。
		damage1Text.fontSize = 32;
		damage1Text.fontStyle = FontStyle.Bold;
		damage1Text.alignment = TextAnchor.MiddleCenter;
		damage1Text.horizontalOverflow = HorizontalWrapMode.Overflow;

		RectTransform damageTextRect = damage1TextObject.GetComponent<RectTransform> ();

		LeanTween.moveLocalY (damage1TextObject, 200.0f, 0.3f);
		LeanTween.color (damageTextRect, new Color (1, 1, 1, 0), 0.3f).setEaseOutElastic ();

		// 上に消えていく
		Observable.Timer(TimeSpan.FromSeconds(0.3f)).Subscribe(_=>{
			Destroy(damage1TextObject);
		}).AddTo(damage1TextObject);

		// 用済み
		//Destroy (damage1TextObject); 
	}
}
