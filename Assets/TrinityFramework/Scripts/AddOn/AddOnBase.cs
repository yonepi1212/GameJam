using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

/*  AddOnBase
    AddOnの基底クラス
*/
public class AddOnBase : MonoBehaviour ,AddOnInterface
{
    #region Declaration
    // パス
    protected struct Path
    {
        public const string Prefab = @"Prefab/";
        public const string Texture = @"Texture/";
    }
    // シーンが加算されたかの判定
    protected bool isAdded = false;
    // Unloadしたときに呼ばれるイベント
    private event System.Action _unloadNotification = delegate () { };
    // シーンが加算されたかの判定
    public bool IsAdded
    {
        set { isAdded = value; }
    }
    // Unloadしたときに呼ばれるイベント
    public event System.Action UnloadNotification
    {
        add
        {
            if (_unloadNotification != null)
            {
                lock (_unloadNotification) _unloadNotification += value;
            }
        }
        remove
        {
            if (_unloadNotification != null)
            {
                lock (_unloadNotification) _unloadNotification -= value;
            }
        }
    }
    // 単体テストで必要だけど重複を防止したいオブジェクト等のリスト
    [SerializeField]
    protected List<GameObject> duplicateBan;
    #endregion

    #region EventSystem
    // イベントシステム
    [SerializeField]
    private EventSystem _eventSystem;
    // イベントシステムを探す
    protected virtual EventSystem FindEventSystem()
    {
        if (_eventSystem == null)
        {
            GameObject g = GameObject.Find(Define.Name.EventSystem);

            if (g == null)
            {
                g = new GameObject();
                g.name = Define.Name.EventSystem;
                _eventSystem = g.AddComponent<EventSystem>();
                g.AddComponent<StandaloneInputModule>();
            }
        }

        return _eventSystem;
    }
    #endregion

    #region MonoBehaviour
    // Start:重複を防止したいオブジェクトがある場合やイベントシステムが必要な場合は呼んでください
    protected virtual void Start()
    {
        if (isAdded)
        {
            duplicateBan.ForEach(g => { Destroy(g); });
        }
        FindEventSystem();
    }
    // OnDestroy
    protected virtual void OnDestroy()
    {
        if (_unloadNotification != null)
        {
            _unloadNotification();
        }

        _unloadNotification = null;
        _eventSystem = null;
        duplicateBan = null;
    }
    #endregion

    #region method
    // ゲームオブジェクトを読み込む
    protected GameObject LoadGameObject(string name_)
    {
        GameObject instance = Resources.Load(Path.Prefab + name_) as GameObject;
        GameObject result = Instantiate(instance);
        result.name = instance.name;

        return result;
    }
    // テクスチャを読み込む
    protected Texture2D LoadTexture(string name_)
    {
        return Resources.Load(Path.Texture + name_) as Texture2D;
    }
    // スプライト群を読み込む
    protected Sprite[] LoadSprite(string name_)
    {
        return Resources.LoadAll<Sprite>(Path.Texture + name_);
    }
    // スプライト群から単一のスプライトを名前で取得する
    protected Sprite FindSingleSprite(Sprite[] sprite_, string name_)
    {
        return System.Array.Find(sprite_, s => s.name.Equals(name_));
    }
    #endregion
}
