using UnityEngine;
/*
prefabに関するutility
*/
public static class PrefabUtility
{

    // PrefabからGameObjectを読み込む
    public static GameObject LoadGameObject(string path_, string name_)
    {

        GameObject instance = Resources.Load(path_ + name_) as GameObject;
        GameObject gameObject = Object.Instantiate(instance, Vector3.zero, Quaternion.identity) as GameObject;
        gameObject.name = name_;

        return gameObject;
    }

    // PrefabからGameObjectを読み込む
    public static GameObject LoadGameObject(string path_, string name_, Vector3 vector_, Quaternion quaternion_)
    {
        GameObject instance = Resources.Load(path_ + name_) as GameObject;
        GameObject gameObject = Object.Instantiate(instance, vector_, quaternion_) as GameObject;
        gameObject.name = name_;

        return gameObject;
    }

    // PrefabからObjectを読み込む
    public static Object LoadObject(string path_, string name_)
    {
        Object instance = Resources.Load(path_ + name_);
        Object cloneObject = Object.Instantiate(instance, Vector3.zero, Quaternion.identity);

        return cloneObject;
    }

    // PrefabからObjectを読み込む
    public static Object LoadScriptableObject(string path_, string name_)
    {
        //Object cloneObject = ScriptableObject.CreateInstance(path_ + name_);
        Object cloneObject = Resources.Load(path_ + name_);
        return cloneObject;
    }


    // PrefabからTexture2Dを読み込む
    public static Texture2D LoadTexture2D(string path_, string name_)
    {
        return Resources.Load(path_ + name_) as Texture2D;
    }

    // PrefabからTextureを読み込む
    public static Texture LoadTexture(string path_, string name_)
    {
        return Resources.Load(path_ + name_) as Texture;
    }
}
