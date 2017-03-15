using UniRx;

/* GameSpeedManager.cs
  ゲーム全体のスピードを管理するクラス
*/
public class GameSpeedManager : SingletonMonoBehaviour<GameSpeedManager>
{
    #region Declration
    // 実際のゲームスピード
    private ReactiveProperty<float> _gameSpeed = new FloatReactiveProperty() { Value = 1 };
    // 設定したゲームスピード
    private float _settingSpeed = 1f;
    public float SettingSpeed
    {
        get
        {
            return _settingSpeed;
        }
    }
    // 等倍モード
    private bool _normalMode;
    public float Value
    {
        get
        {
            return _gameSpeed.Value;
        }
        set
        {
            if (!_normalMode)
            {
                _gameSpeed.Value = value;
            }
            _settingSpeed = value;
        }
    }
    #endregion

    #region Monobehavior
    protected virtual void Start()
    {
        DontDestroyOnLoad(gameObject);
        _gameSpeed.Value = 1f;
    }
    #endregion

    #region Public Method
    // スピードコントロールするオブジェクトを追加
    public void AddObserve(GameSpeedControllable gameSpeedControlollable)
    {
        _gameSpeed.Subscribe(gameSpeedControlollable.ChangedSpeedNotification).AddTo(gameSpeedControlollable.gameObject);
    }
    // 等倍で再生
    public void SetNormalSpeed()
    {
        _normalMode = true;
        _gameSpeed.Value = 1f;
    }

    // 設定したスピードで再生
    public void SetSettingSpeed()
    {
        _normalMode = false;
        _gameSpeed.Value = _settingSpeed;
    }
    #endregion
}
