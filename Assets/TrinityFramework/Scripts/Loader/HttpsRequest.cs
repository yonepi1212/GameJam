using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

/*  HttpsRequest

    証明書がない場合でも接続要求を行います。
*/
public class HttpsRequest : IDisposable
{
    #region Declaration
    // ダウンロード中のファイル識別名
    protected const string Temp = @"~";
    // Dispose したかどうか
    protected bool disposed = false;
    //非同期接続に使用するMemoryStream
    protected MemoryStream memoryStream;
    //受信したデータが一時的に入るバイト型配列
    protected byte[] memoryStreamBuffer;
    //応答データを受信するためのStream
    protected Stream stream;
    //保存するファイルネーム
    protected string fileName = string.Empty;
    //リクエスト監視フラグ
    protected bool isDone = false;
    //リクエスト監視フラグ
    public bool IsDone
    {
        get { return isDone; }
    }
    //トータルファイルサイズ
    protected long totalFileSize = 0;
    //ダウンロードしたサイズ
    protected long loadedFileSize = 0;
    #endregion

    #region Constracter & Destructor
    // デストラクター
    ~HttpsRequest()
    {
        Dispose(false);
    }
    #endregion

    #region IDisposable
    // IDisposable Dispose
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    #endregion

    #region Public Method
    //非同期接続を行う
    public void Begin(string url_, string filePath_)
    {
        //Log.D();
        if (!disposed)
        {
            fileName = filePath_;
            string filePath = Path.GetDirectoryName(filePath_);

            if (!File.Exists(filePath)) Directory.CreateDirectory(filePath);

            Func<object, X509Certificate, X509Chain, SslPolicyErrors, bool> serverCertificate = (sender, certificate, chain, sslPolicyErrors) =>
            {
                return true;
            };

            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(serverCertificate);
            HttpWebRequest request = WebRequest.Create(url_) as HttpWebRequest;

            request.BeginGetResponse(new AsyncCallback(ResponseCallback), request);
        }
    }
    //ダウンロード進捗を返す
    public float GetProgress()
    {
        return (loadedFileSize == 0 || totalFileSize == 0) ? 0 : (float)loadedFileSize / totalFileSize;
    }
    #endregion

    #region Private Method
    // Disposeの判定
    protected virtual void Dispose(bool disposing_)
    {
        if (!disposed)
        {
            // disposing が true の場合(Dispose() が実行された場合)は
            // マネージリソースも解放します。
            if (disposing_)
            {
                // マネージリソースの解放
                memoryStreamBuffer = null;

                if (memoryStream != null) memoryStream.Dispose();

                if (stream != null) stream.Dispose();
            }
            // アンマネージリソースの解放
            disposed = true;
        }
    }
    //非同期要求が終了した時に呼び出されるコールバックメソッド
    protected virtual void ResponseCallback(IAsyncResult ayncResult_)
    {
        //Log.D();
        if (!disposed)
        {
            HttpWebRequest request = ayncResult_.AsyncState as HttpWebRequest;
            HttpWebResponse response = request.EndGetResponse(ayncResult_) as HttpWebResponse;

            totalFileSize = response.ContentLength;

            memoryStream = new MemoryStream();
            memoryStreamBuffer = new byte[1024];

            stream = response.GetResponseStream();
            stream.BeginRead(memoryStreamBuffer, 0, memoryStreamBuffer.Length, new AsyncCallback(ReadCallback), stream);
        }
    }
    //非同期読み込み完了時に呼び出されるコールバックメソッド
    protected virtual void ReadCallback(IAsyncResult ayncResult_)
    {
        //Log.D();
        if (!disposed)
        {
            stream = ayncResult_.AsyncState as Stream;

            int streamReadSize = stream.EndRead(ayncResult_);

            loadedFileSize += streamReadSize;

            if (streamReadSize > 0)
            {
                memoryStream.Write(memoryStreamBuffer, 0, streamReadSize);
                //Debug.Log(memoryStream.Length + " bytes loaded");

                using (FileStream fileStream = new FileStream(fileName + Temp, FileMode.Create, FileAccess.Write))
                {
                    byte[] sourceData = memoryStream.ToArray();
                    fileStream.Write(sourceData, 0, sourceData.Length);
                }
                stream.BeginRead(memoryStreamBuffer, 0, memoryStreamBuffer.Length, new AsyncCallback(ReadCallback), stream);
            }
            else
            {
                File.Move(fileName + Temp, fileName);

                stream.Close();
                memoryStreamBuffer = null;
                memoryStream.Close();
                isDone = true;
            }
        }
    }
    #endregion
}