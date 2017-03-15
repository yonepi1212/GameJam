using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// オブジェクトプール
/// Intantiateは処理コストが高いため、予めInstantiateしておいて
/// 必要になったら必要な分だけ、SetActive(true/false)で表示/非表示して
/// 使いまわす仕組みを提供します
/// </summary>
[System.Serializable]
public class ObjectPool<T> where T : Component
{
    #region Public Declaration

    /// <summary>
    /// InstantiateしたオブジェクトのParentオブジェクト
    /// </summary>
    public MonoBehaviour Root { get; private set; }

    /// <summary>
    /// コピー元となるプレハブ
    /// </summary>
    public T Prefab { get; private set; }

    /// <summary>
    /// コピー元となるプレハブのパス
    /// </summary>
    public string PrefabPath { get; private set; }

    /// <summary>
    /// Poolされたオブジェクトリスト
    /// </summary>
    public List<T> List = new List<T>();

    /// <summary>
    /// 初期化フラグ
    /// </summary>
    public bool IsInit { get; private set; }

    #endregion

    #region Private Declaration

    private int capacity;

    #endregion

    /// <summary>
    /// コンストラクタ（同期初期化）
    /// このコンストラクタを呼び出すと同期で初期化が開始される
    /// </summary>
    /// <param name="root_">親オブジェクト</param>
    /// <param name="path_">コピーするプレハブのパス</param>
    /// <param name="name_">コピーするプレハブの名前</param>
    /// <param name="capacity_">コピーする数</param>
    public ObjectPool(MonoBehaviour root_, string path_, string name_, int capacity_)
    {
        Root = root_;
        PrefabPath = path_ + name_;
        Prefab = default(T);
        IsInit = false;
        capacity = capacity_;

        PoolSync();
    }

    /// <summary>
    /// コンストラクタ（非同期初期化）
    /// このコンストラクタを呼び出すと自動的にPoolコルーチンが実行されて
    /// 非同期で初期化が開始する
    /// </summary>
    /// <param name="root_">親オブジェクト</param>
    /// <param name="path_">コピーするプレハブのパス</param>
    /// <param name="name_">コピーするプレハブの名前</param>
    /// <param name="capacity_">コピーする数</param>
    /// <param name="initRoutine_">プールコルーチン</param>
    public ObjectPool(MonoBehaviour root_, string path_, string name_, int capacity_, out Coroutine initRoutine_)
    {
        Root = root_;
        PrefabPath = path_ + name_;
        Prefab = default(T);
        IsInit = false;
        capacity = capacity_;

        initRoutine_ = root_.StartCoroutineSafe(PoolAsync());
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~ObjectPool()
    {
        Release();
    }

    #region Public Method

    /// <summary>
    /// 使用していないオブジェクトを走査して返します
    /// </summary>
    public T Spawn(Vector3 localPosition_ = default(Vector3))
    {
        return Spawn(null, localPosition_);
    }

    /// <summary>
    /// 使用していないオブジェクトを走査して返します
    /// </summary>
    public T Spawn(GameObject parent_)
    {
        return Spawn(parent_, Vector3.zero);
    }

    /// <summary>
    /// 使用していないオブジェクトを走査して返す
    /// </summary>
    public T Spawn(GameObject parent_, Vector3 localPosition_)
    {
        //Log.D();

        if (IsInit == false)
        {
            LogUtility.Log("まだ初期化が完了していません");
        }

        // 使用していないオブジェクトを探し、なければ新たに生成.
        T ret = List.Find(item => item && item.gameObject.activeSelf == false) ?? CreateInstance();

        if (parent_ != null)
        {
            ret.transform.SetParent(parent_.transform);
        }

        ret.transform.localPosition = localPosition_;
        ret.gameObject.SetActive(true);

        return ret;
    }

    #endregion

    #region Private Method

    /// <summary>
    /// プレハブのプールを同期で実行します
    /// </summary>
    private void PoolSync()
    {
        if (IsInit)
        {
            LogUtility.Log("すでに初期化済みです");
            return;
        }

        // 非同期ロード
        Prefab = Resources.Load<T>(PrefabPath);
        if (Prefab == null)
        {
            LogUtility.Log("Pool元のプレハブがNullです! " + PrefabPath);
            return;
        }

        // Pool
        for (int i = 0; i < capacity; ++i)
        {
            CreateInstance();
        }

        IsInit = true;
    }

    /// <summary>
    /// プレハブのプールを非同期で実行します
    /// </summary>
    private IEnumerator PoolAsync()
    {
        if (IsInit)
        {
            LogUtility.Log("すでに初期化済みです");
            yield break;
        }

        // 非同期ロード
        var req = Resources.LoadAsync<T>(PrefabPath);
        yield return req;

        Prefab = req.asset as T;
        if (Prefab == null)
        {
            LogUtility.Log("Pool元のプレハブがNullです! " + PrefabPath);
            yield break;
        }

        // Pool
        for (int i = 0; i < capacity; ++i)
        {
            CreateInstance();
        }

        IsInit = true;
    }

    /// <summary>
    /// 要素を生成
    /// </summary>
    private T CreateInstance()
    {
        var instance = Object.Instantiate(Prefab);
        if (instance != null)
        {
            if (Root != null)
            {
                instance.transform.SetParent(Root.transform);
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(Prefab.name);
            sb.Append("_");
            sb.Append(capacity.ToString());

            instance.name = sb.ToString();
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localScale = Vector3.one;
            instance.transform.localRotation = Quaternion.identity;
            instance.gameObject.SetActive(false);
            List.Add(instance);
        }
        else
        {
            LogUtility.Log("Spawn Add Fail!!" + PrefabPath);
        }

        if (IsInit) ++capacity;

        return instance;
    }

    /// <summary>
    /// 開放する
    /// </summary>
    public void Release()
    {
        Root = null;
        Prefab = null;
        if (List != null)
        {
            foreach (var obj in List)
            {
                if (obj != null && obj.gameObject != null)
                {
                    Object.Destroy(obj.gameObject);
                }
            }
        }
        List = null;
    }

    #endregion
}