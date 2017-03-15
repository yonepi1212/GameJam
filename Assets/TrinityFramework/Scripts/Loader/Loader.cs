using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/*  Loader

    同一URLの多重ダウンロードを行いません。
    接続先のURLが既にKeyに登録されていていた場合、
    イベントに紐づけてダウンロード完了時にアクションを実行します。

    ex.

    loader.Texture2DWithUrl(url_m, (result) =>
    {
        Image.sprite = result;
    }, (progress) =>
    {
        Debug.Log("Image:" + progress);
    });

    string url_s = @"https://arcane-hollows-2992.herokuapp.com/trinity/src/img/nimue_s.png";
    string url_m = @"https://arcane-hollows-2992.herokuapp.com/trinity/src/img/nimue_m.png";
    string url_l = @"https://arcane-hollows-2992.herokuapp.com/trinity/src/img/nimue_l.png";
*/
public class Loader : SingletonMonoBehaviour<Loader>
{
    #region Declaration
    protected FileCache<Texture2D> cache = new FileCache<Texture2D>();
    #endregion

    #region MonoBehaviour
    // OnDestroy
    protected override void OnDestroy()
    {
        cache.Release();
        base.OnDestroy();
    }
    #endregion

    #region Public Method
    // ダウンロードしたテクスチャーをSpriteで返す
    public virtual void SpriteWithUrl(string url_, System.Action<Sprite> complete_, System.Action<float> progress_ = null)
    {
        System.Action<Texture2D> action = (result) =>
        {
            if (complete_ != null)
            {
                Sprite image = Sprite.Create(result, new Rect(0, 0, result.width, result.height), Vector2.zero);
                complete_(image);
            }
        };

        Texture2DWithUrl(url_, action);
    }
    // ダウンロードしたテクスチャーをSpriteで返す
    public void Texture2DWithUrl(string url_, System.Action<Texture2D> complete_, System.Action<float> progress_ = null)
    {
        if (complete_ != null)
        {
            string key = cache.KeyForURL(url_);
            string path = cache.PathForKey(key);

            if (progress_ != null)
            {
                cache.RegisterProgress(key, progress_);
            }

            if (cache.Exists(path))
            {
                if (cache.RegisterExecutes(key, complete_))
                {
                    Execute(key, path);
                }
            }
            else
            {
                if (cache.RegisterExecutes(key, complete_))
                {
                    StartCoroutine(ConnectHttps(url_, key, path));
                }
            }
        }
    }
    #endregion

    #region Private Method
    // Httpsでダウンロードする
    protected virtual IEnumerator ConnectHttps(string url_, string key_, string path_)
    {
        using (HttpsRequest httpsRequest = new HttpsRequest())
        {
            httpsRequest.Begin(url_, path_);

            float progress = 0;

            while (!httpsRequest.IsDone)
            {
                yield return null;

                if (cache.ContainsProgress(key_))
                {
                    if (progress != httpsRequest.GetProgress())
                    {
                        progress = httpsRequest.GetProgress();
                        cache.ExecuteProgress(key_, progress);
                    }  
                }
            }

            if (cache.ContainsProgress(key_)) cache.RemoveProgress(key_);

            Execute(key_, path_);
        }
    }
    // ダウンロードしたテクスチャを返す
    protected virtual void Execute(string key_, string path_)
    {
        if (cache.ContainsExecutes(key_))
        {
            using (FileStream fileStream = new FileStream(path_, FileMode.Open, FileAccess.Read))
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                byte[] readBinary = binaryReader.ReadBytes((int)binaryReader.BaseStream.Length);
                binaryReader.Close();

                int pos = 16; // 16バイトから開始

                int width = 0;
                for (int i = 0; i < 4; i++)
                {
                    width = width * 256 + readBinary[pos++];
                }

                int height = 0;
                for (int i = 0; i < 4; i++)
                {
                    height = height * 256 + readBinary[pos++];
                }

                Texture2D texture = new Texture2D(width, height);
                texture.LoadImage(readBinary);

                cache.Execute(key_, texture);
            }
        }
    }
    #endregion
}