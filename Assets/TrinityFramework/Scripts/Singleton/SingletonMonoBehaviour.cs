using UnityEngine;
using System.Collections;
/// <summary>
/// テラシュールのシングルトン基底クラス
/// </summary>
/// <typeparam name="T"></typeparam>
public class SingletonMonoBehaviour<T> : MonoBehaviour, SingletonInterface where T : SingletonMonoBehaviour<T>
{
    protected static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null)
                {
                    GameObject go = new GameObject();
                    go.name = typeof(T).Name;
                    go.transform.localPosition = Vector3.zero;
                    go.transform.localRotation = Quaternion.identity;
                    go.transform.localScale = Vector3.one;
                    instance = go.AddComponent<T>();
                }
            }

            return instance;
        }
    }


    protected virtual void Awake()
    {
        CheckInstance();
    }

    protected virtual void OnDestroy()
    {
        if (instance != null)
        {
            Debug.LogWarning(typeof(T) + " instance is not null");
        }
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = (T)this;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }

        if (gameObject != null)
        {
            Debug.Log("CheckInstance " + name);
            Destroy(gameObject);
        }

        Destroy(this);
        return false;
    }

    public virtual void Release()
    {
        instance = null;
        if (gameObject != null)
        {
            Destroy(gameObject);
        }

        Destroy(this);
    }
}