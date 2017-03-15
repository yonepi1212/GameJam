using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// サウンドの管理クラス
/// </summary>
public class SoundManager : SingletonMonoBehaviour<SoundManager>
{

    #region const

    public JukeBox JukeBox;

    /// <summary>
    /// Resourcesフォルダ以下のSE格納パス
    /// プロジェクト毎に要変更
    /// </summary>
    public const string SE_ROOT_PATH = "Sound/SE/";

    /// <summary>
    /// Resourcesフォルダ以下のBGM格納パス
    /// プロジェクト毎に要変更
    /// </summary>
    public const string BGM_ROOT_PATH = "Sound/BGM/";

    /// <summary>
    /// Resourcesフォルダ以下のBGM格納パス
    /// プロジェクト毎に要変更
    /// </summary>
    public const string VOICE_ROOT_PATH = "Sound/Voice/";

    /// <summary>
    /// SEレイヤ数（同時際整数上限）
    /// </summary>
    public const int SE_LAYER_COUNT = 200;

    /// <summary>
    /// BGMレイヤ数（同時際整数上限）
    /// </summary>
    public const int BGM_LAYER_COUNT = 2;

    /// <summary>
    /// Voiceレイヤ数（同時際整数上限）
    /// </summary>
    public const int VOICE_LAYER_COUNT = 200;

    #endregion

    #region static 

    /// <summary>
    /// BGM音量 readonly
    /// </summary>
    public static float BGMVolume { get { return Instance.VolumeBGM; } }

    /// <summary>
    /// SE音量 readonly
    /// </summary>
    public static float SEVolume { get { return Instance.VolumeSE; } }

    /// <summary>
    /// Voice音量 readonly
    /// </summary>
    public static float VoiceVolume { get { return Instance.VolumeVoice; } }

    #endregion

    #region private

    /// <summary>
    /// SE音量[0~1]
    /// </summary>
    [Range(0f, 1f), SerializeField]
    float VolumeSE;

    /// <summary>
    /// BGM音量[0~1]
    /// </summary>
    [Range(0f, 1f), SerializeField]
    float VolumeBGM;

    /// <summary>
    /// Voice音量[0~1]
    /// </summary>
    [Range(0f, 1f), SerializeField]
    float VolumeVoice;

    /// <summary>
    /// SEのキャッシュを行うかどうか
    /// </summary>
    [SerializeField]
    bool useCacheSE;

    /// <summary>
    /// BGMのキャッシュを行うかどうか
    /// 基本ストリーミング再生するので不要
    /// </summary>
    [SerializeField]
    bool useCacheBGM;

    /// <summary>
    /// BGMのキャッシュを行うかどうか
    /// 基本ストリーミング再生するので不要
    /// </summary>
    [SerializeField]
    bool useCacheVoice;

    /// <summary>
    /// SE再生オブジェクト
    /// </summary>
    [HideInInspector, SerializeField]
    List<SEPlayer> seList;

    /// <summary>
    /// BGM再生オブジェクト
    /// </summary>
    [HideInInspector, SerializeField]
    List<BGMPlayer> bgmList;

    /// <summary>
    /// Vocie再生オブジェクト
    /// </summary>
    [HideInInspector, SerializeField]
    List<VoicePlayer> voiceList;

    /// <summary>
    /// 音源のキャッシュ.
    /// キーはパス SE/BGM共用
    /// </summary>
    Dictionary<string, AudioClip> Cache = new Dictionary<string, AudioClip>();

    /// <summary>
    /// 再生中BGMレイヤ. 非再生時-1
    /// </summary>
    private int bgmLayer = -1;

    #endregion

    #region public

    /// <summary>
    /// 音量を設定する
    /// </summary>
    public void SetVolume(float se, float bgm, float voice)
    {
        VolumeSE = se;
        VolumeBGM = bgm;
        VolumeVoice = voice;
    }

    /// <summary>
    /// キャッシュをクリア
    /// </summary>
    public void CacheClear()
    {
        Cache.Clear();
    }

    /// <summary>
    /// キャッシュに追加する.
    /// </summary>
    public void AddCache(string name, AudioClip clip)
    {
        if (useCacheSE == false || clip == null) return;
        if (Cache == null) Cache = new Dictionary<string, AudioClip>();
        if (Cache.ContainsKey(name)) return;

        Cache.Add(name, clip);
    }

    /// <summary>
    /// 音源データを先にロードしてキャッシュします
    /// シーン遷移などのタイミングで呼び出す想定
    /// </summary>
    public void PreLoad(string[] paths)
    {
        if (paths == null) return;
        foreach (var path in paths)
        {
            // TODO : AssetBundle対応
            AudioClip clip = Resources.Load<AudioClip>(path);
            AddCache(path, clip);
        }
    }

    /// <summary>
    /// 音源データを先にロードし、キャッシュします (非同期ver)
    /// シーン遷移などのタイミングで呼び出す想定
    /// </summary>
    public IEnumerator PreLoadAsync(string[] paths)
    {
        if (paths == null) yield break;
        foreach (var path in paths)
        {
            // TODO : AssetBundle対応
            ResourceRequest request = Resources.LoadAsync<AudioClip>(path);
            while (request.isDone == false)
            {
                yield return null;
            }
            AddCache(path, request.asset as AudioClip);
        }
    }

    #endregion

    #region MonoBehaviour

    protected override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this);

        InitJukeBox();

        // デフォルトのボリュームを設定
        SetVolume(1f, 1f, 1f);

        // SEプレイヤーを初期化
        InitSE();

        // BGMプレイヤーを初期化
        InitBGM();

        // Voiceプレイヤーを初期化
        InitVoice();

        // キャッシュ初期化
        Cache = new Dictionary<string, AudioClip>();
    }

    void Update()
    {
        // SE更新
        if (seList != null)
        {
            foreach (var se in seList)
            {
                se.UpdateVolume();
            }
        }

        // BGM更新
        if (bgmList != null)
        {
            foreach (var bgm in bgmList)
            {
                bgm.UpdateVolume();
            }
        }

        // Voice更新
        if (voiceList != null)
        {
            foreach (var voice in voiceList)
            {
                voice.UpdateVolume();
            }
        }
    }

    void OnEnable()
    {
        // サウンドマネージャを非アクティブ状態から戻した時に再開する処理を記述

    }

    #endregion ================================================================================

    #region private Method

    private void InitJukeBox()
    {
        if (JukeBox == null)
        {
            JukeBox = Resources.Load("JukeBox") as JukeBox;
        }
    }


    private AudioClip GetSound(string rootPath, string soundName, bool useCache)
    {
        AudioClip clip;
        string path = rootPath + soundName;

        Cache.TryGetValue(path, out clip);
        if (clip == null)
        {
            // TODO AssetBundle対応
            clip = Resources.Load<AudioClip>(path);
            if (useCache)
            {
                AddCache(path, clip);
            }
        }
        // 読み込みに失敗.
        if (clip == null)
        {
            Debug.LogWarning("AudioClip Not Found!! : " + path);
        }
        return clip;
    }

    private AudioClip GetSound(SoundKind soundKind, bool useCache)
    {
        AudioClip clip;
        Cache.TryGetValue(soundKind.ToString(), out clip);

        if (clip == null)
        {
            // TODO AssetBundle対応
            clip = JukeBox.GetAudioClip(soundKind);
            if (useCache)
            {
                AddCache(soundKind.ToString(), clip);
            }
        }
        // 読み込みに失敗.
        if (clip == null)
        {
            Debug.LogWarning("AudioClip Not Found!! : " + soundKind.ToString());
        }
        return clip;
    }
    #endregion

    #region SE

    /// <summary>
    /// SE再生オブジェクトの初期化
    /// </summary>
    private void InitSE()
    {
        if (seList == null)
        {
            seList = new List<SEPlayer>();
        }
        seList.RemoveAll(x => x == null);

        for (int i = 0; i < SE_LAYER_COUNT; i++)
        {
            if (seList.Count <= i || seList[i] == null)
            {
                GameObject go = new GameObject("SE_" + (i + 1).ToString());
                SEPlayer se = go.AddComponent<SEPlayer>();
                go.transform.SetParent(transform);
                seList.Add(se);
            }

            seList[i].Init();
        }
    }

    /// <summary>
    /// SEレイヤを取得する
    /// </summary>
    /// <param name="layer">SEのレイヤを指定します. 指定しない場合、使用していないレイヤを返します</param>
    /// <returns>SEレイヤ</returns>
    private int GetSELayer(int layer = -1)
    {
        // レイヤが指定されている場合は指定レイヤを返す
        if (layer >= 0 && layer < SE_LAYER_COUNT) return layer;

        // 指定されていない場合は未使用レイヤを返す
        for (int i = 0; i < SE_LAYER_COUNT; i++)
        {
            if (seList[i].IsPlaying) continue;
            return i;
        }

        return -1;
    }

    /// <summary>
    /// 全てSE停止.
    /// </summary>
    public void StopSE()
    {
        foreach (var se in seList)
        {
            se.Stop();
        }
    }

    /// <summary>
    /// 特定のSEを停止させる
    /// </summary>
    public void StopSE(int layer)
    {
        if (layer < 0 || layer >= SE_LAYER_COUNT) return;
        seList[layer].Stop();
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    public int PlaySE(string seName)
    {
        return PlaySE(seName, -1, false, Vector3.zero);
    }

    public int PlaySE(SoundKind soundKind)
    {
        return PlaySE(soundKind, -1, false, Vector3.zero);
    }
    /// <summary>
    /// SEを再生する
    /// </summary>
    public int PlaySE(string seName, int layer)
    {
        return PlaySE(seName, layer, false, Vector3.zero);
    }

    public int PlaySE(SoundKind soundKind, int layer)
    {
        return PlaySE(soundKind, layer, false, Vector3.zero);
    }

    /// <summary>
    /// SEを再生する
    /// </summary>
    public int PlaySE(string seName, int layer, bool isLoop)
    {
        return PlaySE(seName, layer, isLoop, Vector3.zero);
    }

    public int PlaySE(SoundKind soundKind, int layer, bool isLoop)
    {
        return PlaySE(soundKind, layer, isLoop, Vector3.zero);
    }

    /// <summary>
    /// SEを再生する
    /// 3Dサウンド用にPositionを設定
    /// </summary>
    public int PlaySE(string seName, int layer, bool isLoop, Vector3 worldPos)
    {
        int layerNo = GetSELayer(layer);
        if (layerNo == -1)
        {
            Debug.LogWarning("All SELayer is Busy!!");
            return -1;
        }

        AudioClip clip = GetSound(SE_ROOT_PATH, seName, useCacheSE);
        if (clip == null)
        {
            return -1;;
        }

        // SE再生
        seList[layerNo].Play(clip, isLoop, worldPos);
        return layerNo;
    }

    public int PlaySE(SoundKind soundKind, int layer, bool isLoop, Vector3 worldPos)
    {
        int layerNo = GetSELayer(layer);
        if (layerNo == -1)
        {
            Debug.LogWarning("All SELayer is Busy!!");
            return -1;
        }
        AudioClip clip = GetSound(soundKind, useCacheSE);
        if (clip == null)
        {
            return -1;
        }
        // SE再生
        seList[layerNo].Play(clip, isLoop, worldPos);
        return layerNo;
    }



    #endregion

    #region BGM

    /// <summary>
    /// BGM再生オブジェクトの初期化
    /// </summary>
    private void InitBGM()
    {
        if (bgmList == null)
        {
            bgmList = new List<BGMPlayer>();
        }
        bgmList.RemoveAll(x => x == null);

        for (int i = 0; i < BGM_LAYER_COUNT; i++)
        {
            if (bgmList.Count <= i || bgmList[i] == null)
            {
                GameObject go = new GameObject("BGM_" + (i + 1).ToString());
                BGMPlayer bgm = go.AddComponent<BGMPlayer>();
                go.transform.SetParent(transform);
                bgm.Init();
                bgmList.Add(bgm);
            }

            bgmList[i].Init();
        }
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    public void PlayBGM(string bgmName, float fadeTime = 2f, Action onStop = null)
    {

        AudioClip clip = GetSound(BGM_ROOT_PATH, bgmName, useCacheBGM);
        // 読み込みに失敗.
        if (clip == null)
        {
            return;
        }

        PlayBGM(clip, fadeTime, onStop);
    }

    public void PlayBGM(SoundKind soundKind, float fadeTime = 2f, Action onStop = null)
    {

        AudioClip clip = GetSound(soundKind, useCacheBGM);
        // 読み込みに失敗.
        if (clip == null)
        {
            return;
        }
        PlayBGM(clip, fadeTime, onStop);
    }


    private void PlayBGM(AudioClip clip, float fadeTime, Action onStop = null)
    {
        // 同じBGMを再生中なら何もしない.
        if (bgmLayer >= 0
            && bgmList[bgmLayer].IsPlaying
            && bgmList[bgmLayer].Clip.name == clip.name)
        {
            return;
        }
        // 再生中BGM停止
        if (bgmLayer != -1)
        {
            bgmList[bgmLayer].Stop(fadeTime / 2, 0f, onStop);
        }

        // 再生開始
        for (int i = 0; i < bgmList.Count; i++)
        {
            if (bgmList[i].IsPlaying == false)
            {
                bgmLayer = i;
                bgmList[i].Play(clip, fadeTime / 2, fadeTime / 2);
                break;
            }
        }
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    public void StopBGM(float fadeTime = 1f, float delayTime = 0f, Action onStop = null)
    {
        bgmLayer = -1;
        BGMPlayer player = null;
        foreach (var bgm in bgmList)
        {
            if (bgm.IsPlaying)
            {
                bgm.Stop(fadeTime, 0f, null);
                player = bgm;
            }
        }

        if (player != null)
        {
            player.OnStop = onStop;
        }
        else
        {
            if (onStop != null)
            {
                onStop();
            }
        }
    }

    /// <summary>
    /// BGM一時停止
    /// </summary>
    public void PauseBGM()
    {
        foreach (var bgm in bgmList)
        {
            if (bgm.IsPlaying)
            {
                bgm.Pause();
            }
        }
    }

    /// <summary>
    /// BGM再開
    /// </summary>
    public void ResumeBGM()
    {
        foreach (var bgm in bgmList)
        {
            if (bgm.IsPause)
            {
                bgm.Resume();
            }
        }
    }

    #endregion

    #region Voice

    private void InitVoice()
    {
        if (voiceList == null)
        {
            voiceList = new List<VoicePlayer>();
        }
        voiceList.RemoveAll(x => x == null);

        for (int i = 0; i < VOICE_LAYER_COUNT; i++)
        {
            if (voiceList.Count <= i || voiceList[i] == null)
            {
                GameObject go = new GameObject("Voice_" + (i + 1).ToString());
                VoicePlayer voice = go.AddComponent<VoicePlayer>();
                go.transform.SetParent(transform);
                voiceList.Add(voice);
            }

            voiceList[i].Init();
        }
    }

    private int GetVoiceLayer(int layer = -1)
    {
        // レイヤが指定されている場合は指定レイヤを返す
        if (layer >= 0 && layer < VOICE_LAYER_COUNT) return layer;

        // 指定されていない場合は未使用レイヤを返す
        for (int i = 0; i < VOICE_LAYER_COUNT; i++)
        {
            if (voiceList[i].IsPlaying) continue;
            return i;
        }

        return -1;
    }

    public void StopVoice()
    {
        foreach (var voice in voiceList)
        {
            voice.Stop();
        }
    }

    public void StopVoice(int layer)
    {
        if (layer < 0 || layer >= VOICE_LAYER_COUNT) return;
        voiceList[layer].Stop();
    }

    public int PlayVoice(string voiceName)
    {
        return PlayVoice(voiceName, -1, false, Vector3.zero);
    }

    public int PlayVoice(SoundKind soundKind)
    {
        return PlayVoice(soundKind, -1, false, Vector3.zero);
    }

    public int PlayVoice(string seName, int layer)
    {
        return PlaySE(seName, layer, false, Vector3.zero);
    }

    public int PlayVoice(SoundKind soundKind, int layer)
    {
        return PlayVoice(soundKind, layer, false, Vector3.zero);
    }

    public int PlayVoice(string voiceName, int layer, bool isLoop)
    {
        return PlayVoice(voiceName, layer, isLoop, Vector3.zero);
    }

    public int PlayVoice(SoundKind soundKind, int layer, bool isLoop)
    {
        return PlayVoice(soundKind, layer, isLoop, Vector3.zero);
    }

    public int PlayVoice(string voiceName, int layer, bool isLoop, Vector3 worldPos)
    {
        int layerNo = GetSELayer(layer);
        if (layerNo == -1)
        {
            Debug.LogWarning("All VoiceLayer is Busy!!");
            return -1;
        }

        AudioClip clip = GetSound(VOICE_ROOT_PATH, voiceName, useCacheVoice);
        if (clip == null)
        {
            return -1; ;
        }

        // Voice再生
        voiceList[layerNo].Play(clip, isLoop, worldPos);
        return layerNo;
    }

    public int PlayVoice(SoundKind soundKind, int layer, bool isLoop, Vector3 worldPos)
    {
        int layerNo = GetSELayer(layer);
        if (layerNo == -1)
        {
            Debug.LogWarning("All VoiceLayer is Busy!!");
            return -1;
        }
        AudioClip clip = GetSound(soundKind, useCacheVoice);
        if (clip == null)
        {
            return -1;
        }
        // Voice再生
        voiceList[layerNo].Play(clip, isLoop, worldPos);
        return layerNo;
    }

    #endregion

}