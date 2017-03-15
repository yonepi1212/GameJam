using System;
using System.Collections.Generic;
using UnityEngine;
/* TransformValueMaster.cs
	キャラクター位置のデータクラス
*/
public class TransformValueMaster : ScriptableObject
{

    #region Declaration

    [Serializable]
    public class EnemyTransformInfo
    {
        public string Name;
        public List<TransformInfo> TransformList;
    }

    [Serializable]
    public class TransformInfo
    {
        public string Key;
        public Vector3 LocalPosition;
        public Quaternion LocalRotation;
        public Vector3 LocalScale;
    }

    public List<EnemyTransformInfo> EnemyTransformList;

    #endregion

    #region Public Method
    public EnemyTransformInfo GetEnemyTransform(string transformName)
    {
        var info = EnemyTransformList.Find(record => record.Name.Equals(transformName));
        var ret = new EnemyTransformInfo
        {
            Name = info.Name,
            TransformList = new List<TransformInfo>()
        };
        foreach (var transformInfo in info.TransformList)
        {
            var addInfo = new TransformInfo()
            {
                Key = transformInfo.Key,
                LocalPosition = transformInfo.LocalPosition,
                LocalRotation = transformInfo.LocalRotation,
                LocalScale = transformInfo.LocalScale
            };
            ret.TransformList.Add(addInfo);
        }
        return ret;
    }

    public void AddEnemyTransform(string transformName, string key, Transform valueTransform)
    {
        var info = EnemyTransformList.Find(record => record.Name.Equals(transformName));
        if (info != null)
        {
            bool update = false;
            foreach (var transformList in info.TransformList)
            {
                if (transformList.Key.Equals(key))
                {
                    // 更新
                    transformList.Key = key;
                    transformList.LocalPosition = valueTransform.localPosition;
                    transformList.LocalRotation = valueTransform.localRotation;
                    transformList.LocalScale = valueTransform.localScale;
                    update = true;
                }
            }
            if (!update)
            {
                info.TransformList.Add(new TransformInfo
                {
                    Key = key,
                    LocalPosition = valueTransform.localPosition,
                    LocalRotation = valueTransform.localRotation,
                    LocalScale = valueTransform.localScale
                });
            }
        }
        else
        {
            // 追加
            EnemyTransformInfo addInfo = new EnemyTransformInfo
            {
                Name = transformName,
                TransformList = new List<TransformInfo>()
            };
            addInfo.TransformList.Add(new TransformInfo
            {
                Key = key,
                LocalPosition = valueTransform.localPosition,
                LocalRotation = valueTransform.localRotation,
                LocalScale = valueTransform.localScale
            });
            EnemyTransformList.Add(addInfo);
        }
    }

    #endregion

    #region Private Method


    #endregion
}
