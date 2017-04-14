using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

	// Use this for initialization
	void Awake () {

		_levelUpButton.onClick.AddListener (LevelUp);

		// TODO:PlayerPrefsから読み込み

		CurrentStatus = new Status(){
			StageNumber = new IntReactiveProperty(1),
			TapDamage = new DoubleReactiveProperty(1),
			CurrentCoin =  new DoubleReactiveProperty(0),
			CurrentMyLevel = new IntReactiveProperty(1),
			NextLevelCoin = new DoubleReactiveProperty(1),
		};

		CurrentEnemy = GenerateFirstEnemy(1,10);

		CurrentStatus.StageNumber.Subscribe (number => {
			_stageNumberText.text = String.Format("{0:F0}",number);
			ShakeGameObject(_stageNumberText.gameObject);
		});

		CurrentStatus.CurrentMyLevel.Subscribe(level =>
		{
			_currentLevelText.text = String.Format("{0:F0}", level);
				ShakeGameObject(_currentLevelText.gameObject);

		});

		CurrentStatus.CurrentCoin.Subscribe (coin => {
			_currentCoinText.text = String.Format("{0:F1}",coin);
			ShakeGameObject(_currentCoinText.gameObject);
		});

		CurrentStatus.NextLevelCoin.Subscribe (coin => {
			_nextCoinText.text = String.Format("{0:F1}",coin);
			ShakeGameObject(_nextCoinText.gameObject);
		});

		CurrentStatus.TapDamage.Subscribe (damage => {
			
			_currentTapDamageText.text = String.Format("{0:F1}",damage);
			ShakeGameObject(_currentTapDamageText.gameObject);

		});

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTaped()
	{

		CurrentEnemy.CurrentHp.Value -= CurrentStatus.TapDamage.Value;
		ShowDamage (CurrentStatus.TapDamage.Value);

//		Debug.Log ("ザコHP:" + CurrentEnemy.CurrentHp.Value);
//		Debug.Log ("ザコMAXHP:" + CurrentEnemy.MaxHp.Value);
//
//		Debug.Log ("パワー:" + CurrentStatus.TapDamage.Value);

		if (CurrentEnemy.CurrentHp.Value < 0) {

			BreakEnemy ();

			// ステージ番号更新
			var nextStageNumber = CurrentStatus.StageNumber.Value + 1;
			CurrentStatus.StageNumber.Value = nextStageNumber;


			//CurrentStatus.CurrentMyLevel.Value = CurrentStatus.CurrentMyLevel.Value + 1;
			CurrentEnemy = GenerateEnemy (nextStageNumber,CurrentEnemy.MaxHp.Value);
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
		enemy.MaxHp = new DoubleReactiveProperty(prevHp * 1.1f);
		enemy.CurrentHp = new DoubleReactiveProperty(enemy.MaxHp.Value);
		enemy.Coin = level;
		enemy.Type = (level % 10 == 0) ? Enemy.EnemyType.Boss : Enemy.EnemyType.Zako;

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

		// ダメージ数値整形
		var damageStr = String.Format("{0:F1}",damage);


		// テキスト編集
		Text damage1Text = damage1TextObject.GetComponent<Text> ();
		damage1Text.text = damageStr;
		damage1Text.font = _myFont;//fontはフィールドで。
		damage1Text.fontSize = 32;
		damage1Text.alignment = TextAnchor.MiddleCenter;
		damage1Text.horizontalOverflow = HorizontalWrapMode.Overflow;

		LeanTween.moveLocalY (damage1TextObject, 320.0f, 0.3f);

		// 上に消えていく
//		iTween.MoveBy (damage1TextObject, iTween.Hash ("y", 1f, "time", 0.5f));
//		iTween.ValueTo (gameObject, iTween.Hash ("from", 1, "to", 0, "time", 0.5f,
//			"onupdate", "ValueChange"));

		//yield return new WaitForSeconds (0.55f);
		Observable.Timer(TimeSpan.FromSeconds(0.3f)).Subscribe(_=>{
			Destroy(damage1TextObject);
		}).AddTo(damage1TextObject);

		// 用済み
		//Destroy (damage1TextObject); 
	}
}
