using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
	private Text _currentCoinText;

	[SerializeField]
	private Text _nextCoinText;

	[SerializeField]
	private Text _currentTapDamageText;


	// Use this for initialization
	void Start () {

		_levelUpButton.onClick.AddListener (LevelUp);

		// TODO:PlayerPrefsから読み込み

		CurrentStatus = new Status(){
			StageNumber = new IntReactiveProperty(1),
			TapDamage = new DoubleReactiveProperty(1),
			CurrentCoin =  new DoubleReactiveProperty(0),
			CurrentMyLevel = new IntReactiveProperty(1),
			NextLevelCoin = new DoubleReactiveProperty(1),
		};

		CurrentEnemy = GenerateEnemy(1);


		CurrentStatus.StageNumber.Subscribe (number => {
			_stageNumberText.text = String.Format("{0:F0}",number);
		});

		CurrentStatus.CurrentCoin.Subscribe (coin => {
			_currentCoinText.text = String.Format("{0:F1}",coin);
		});

		CurrentStatus.NextLevelCoin.Subscribe (coin => {
			_nextCoinText.text = String.Format("{0:F1}",coin);
		});

		CurrentStatus.TapDamage.Subscribe (damage => {
			
			_currentTapDamageText.text = String.Format("{0:F1}",damage);
		});

//		CurrentEnemy.CurrentHp.Subscribe (hp => {
//			var ratio = hp / CurrentEnemy.MaxHp.Value;
//			_stageEnemyHpSlider.value = ratio;
//		});

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OnTaped()
	{

		CurrentEnemy.CurrentHp.Value -= CurrentStatus.TapDamage.Value;

//		Debug.Log ("ザコHP:" + CurrentEnemy.CurrentHp.Value);
//		Debug.Log ("ザコMAXHP:" + CurrentEnemy.MaxHp.Value);
//
//		Debug.Log ("パワー:" + CurrentStatus.TapDamage.Value);

		if (CurrentEnemy.CurrentHp.Value < 0) {
			Debug.Log ("ザコ撃破");

			BreakEnemy ();

			// ステージ番号更新
			var nextStageNumber = CurrentStatus.StageNumber.Value + 1;
			CurrentStatus.StageNumber.Value = nextStageNumber;


			//CurrentStatus.CurrentMyLevel.Value = CurrentStatus.CurrentMyLevel.Value + 1;
			CurrentEnemy = GenerateEnemy (nextStageNumber);
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
			Debug.Log ("Level Up!!");

		} else {
			// 失敗
			Debug.Log ("No Level Up");

		}
	}

	private Enemy GenerateEnemy(int level)
	{
		Enemy enemy = new Enemy();
		enemy.MaxHp = new DoubleReactiveProperty(level * 10);
		enemy.CurrentHp = new DoubleReactiveProperty(level * 10);
		enemy.Coin = level;
		enemy.Type = (level % 10 == 0) ? Enemy.EnemyType.Boss : Enemy.EnemyType.Zako;

		enemy.CurrentHp.Subscribe (hp => {
			float ratio = (float)hp / (float)enemy.MaxHp.Value;
	
			//var hpString = hp + "/" + enemy.MaxHp.Value;
			var hpString = String.Format("{0:F1}/{1:F1}",hp,enemy.MaxHp.Value);

			_hpText.text = hpString;
			_stageEnemyHpSlider.value = ratio;
		});

		return enemy;
	}
}
