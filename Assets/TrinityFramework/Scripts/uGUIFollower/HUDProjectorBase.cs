using System;
using UnityEngine;

/* HUDProjector
    3D空間のオブジェクトをguiのように表示するコンポーネントです
    以下の機能を兼ね備えています
    1. カメラの方向を向く
    2. カメラの位置にかかわらず、同じ大きさに見える
    3. offsetが設定できる
    4. 画面外に出た場合、画面内に入る一番近い点に移動する
    5. 指定領域内しか移動できないようにする
    上記の実装はできていますが,エディタ上で可視化されないため使いにくいです.
    暇な時間に使いやすくしていきます
*/
public class HUDProjectorBase : MonoBehaviour
{
    #region Declaration
    /*
     カメラへの参照はプロジェクトによって異なるのでハードコーディングしています
     継承させて作っても良いのですが、エディタが作りにくくなるので微妙です
    */
    // 3D空間を見ているカメラ
    protected virtual Camera currentCamera
    {
        get { return Camera.current; }
    }
    // 常に手前に表示したいUI用等のカメラ
    protected virtual Camera uiCamera
    {
        get { return Camera.main; }
    }
    private Transform _transform;

    [Header("基本設定")]
    [SerializeField, Range(0, 10)]
    // カメラまでの距離
    private float _cameraRange = 1f;
    protected float cameraRange
    {
        get { return _cameraRange; }
    }
    // スクリーン座標でのオフセット
    // ex.ダメージテキスト上昇を表現できます
    public Vector2 ScreenOffset;
    // 3D空間でのオフセット
    // ex.HPゲージ等で頭の上に載せたい時に使います
    [SerializeField]
    private Vector3 _offset;
    protected Vector3 offset
    {
        get { return _offset; }
    }

    [Header("フォローするかしないか")]
    // フォローするかしないか
    [SerializeField]
    private bool _isFollow = false;
    protected bool isFollow
    {
        get { return _isFollow; }
    }
    // フォローするオブジェクトを保持するクラス
    [Serializable]
    protected class TargetGameObjectHolder
    {
        [SerializeField]
        private GameObject _targetObject;
        private Transform _transform;
        public Transform Transform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = _targetObject.transform;
                }
                return _transform;
            }
        }
    }
    [Header("フォローするオブジェクトを指定")]
    [SerializeField]
    protected TargetGameObjectHolder targetObject;

    // フォローしない場合は空間に固定するための座標を指定

    [Header("フォローしない場合は座標を指定")]
    public Vector3 _fixPosition;
    // 範囲指定するか

    [Header("表示領域を制限するか")]
    [SerializeField]
    private bool _isMovingRange = false;
    protected bool isMovingRange
    {
        get { return _isMovingRange; }
    }

    [Header("領域を制限する場合は動かすオブジェクトのサイズを指定")]
    [SerializeField, Range(0, 1000)]
    private float _height;
    [SerializeField, Range(0, 1000)]
    private float _width;

    [Header("表示領域のViewPortの座標")]
    [SerializeField]
    private Vector2 _leftBottomViewPortPoint = Vector2.zero;
    [SerializeField]
    private Vector2 _rightTopViewPortPoint = Vector2.one;

    public Transform Transform
    {
        get
        {
            if (_transform == null)
            {
                _transform = transform;
            }
            return _transform;
        }
    }
    #endregion

    #region MonoBehaviour
    protected virtual void Start()
    {
        _transform = transform;
        if (_cameraRange == 0f)
        {
            Debug.LogWarning("HUDProject's camera range is 0. This object is not visible.");

        }
    }

    protected virtual void Update()
    {
        // オブジェクトをフォローする場合はオブジェクトのポジションでUpdate
        if (_isFollow)
        {
            UpdatePosition(targetObject.Transform.position);
        }
        // オブジェクトをフォローしない場合は指定の座標でUpdate
        else
        {
            UpdatePosition(_fixPosition);
        }
    }
    #endregion

    #region Private Method
    // 位置を追従させる
    protected virtual void UpdatePosition(Vector3 position_)
    {
        // カメラに向きを合わせる
        Transform.rotation = uiCamera.transform.rotation;
        // 中央のポジションを取得
        var centerScreenPosition = currentCamera.WorldToScreenPoint(position_ + _offset);
        // 範囲指定がある場合は範囲内に収まるようにClampする
        if (_isMovingRange)
        {
            centerScreenPosition = ClampMovingRange(centerScreenPosition);
        }
        var worldPosition = uiCamera.ScreenToWorldPoint(new Vector3(centerScreenPosition.x + ScreenOffset.x, centerScreenPosition.y + ScreenOffset.y, _cameraRange));
        Transform.position = worldPosition;
    }
    // 座標を画面内に入れるように調整
    protected virtual Vector3 ClampMovingRange(Vector3 screenPosition_)
    {
        // 画面内に入れる
        var minX = Screen.width * _leftBottomViewPortPoint.x + _width;
        var maxX = Screen.width * _rightTopViewPortPoint.x - _width;
        var minY = Screen.height * _leftBottomViewPortPoint.y + _height;
        var maxY = Screen.height * _rightTopViewPortPoint.y - _height;
        var x = Mathf.Clamp(screenPosition_.x, minX, maxX);
        var y = Mathf.Clamp(screenPosition_.y, minY, maxY);
        return new Vector3(x, y, 0f);
    }
    #endregion
}