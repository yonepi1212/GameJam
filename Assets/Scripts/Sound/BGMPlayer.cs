using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// BGM再生を行う.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BGMPlayer : MonoBehaviour
{
    #region public

    /// <summary>
    /// BGMの状態
    /// </summary>
    public enum State : int
    {
        STOP = 0,
        FADEIN,
        FADEIN_WAIT,
        FADEOUT,
        FADEOUT_WAIT,
        PLAYING,
        PAUSE,
    }

    /// <summary>
    /// 音源 オーディオクリップ
    /// </summary>
    public AudioClip Clip
    {
        get { return audioSource == null ? null : audioSource.clip; }
    }

    /// <summary>
    /// 停止時に呼び出されるコールバック
    /// </summary>
    public Action OnStop { get; set; }

    /// <summary>
    /// 再生中かどうか
    /// </summary>
    public bool IsPlaying
    {
        get
        {
            return state == State.PLAYING
                || state == State.FADEIN
                || state == State.FADEOUT
                || state == State.FADEIN_WAIT
                || state == State.FADEOUT_WAIT;
        }
    }

    /// <summary>
    /// 一時停止中かどうか
    /// </summary>
    public bool IsPause
    {
        get
        {
            return state == State.PAUSE;
        }
    }

    #endregion

    #region private

    private State state;
    private AudioSource audioSource;
    private float volume;
    private float loopStart;
    private float loopEnd;
    private float fadeTime;
    private float startTime;
    private float pauseTime;
    private float delayTime;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = true;
    }

    void Start()
    {

    }

    #endregion

    #region Method

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        state = State.STOP;
        loopStart = 0f;
        loopEnd = 0f;
        fadeTime = 0f;
        startTime = 0f;
        delayTime = 0f;
        pauseTime = 0f;
        volume = 1f;
    }

    /// <summary>
    /// 更新. マネージャから呼ばれる想定
    /// </summary>
    public void UpdateVolume()
    {
        switch (state)
        {
            case State.STOP:
            case State.PAUSE:
                break;

            case State.PLAYING:
                // ループ処理
                if (audioSource.time >= loopEnd)
                {
                    audioSource.time = loopStart;
                    audioSource.Play();
                }

                audioSource.volume = SoundManager.BGMVolume * volume;
                break;

            case State.FADEIN_WAIT:
                if (Time.time - startTime >= delayTime)
                {
                    state = State.FADEIN;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
                break;
            case State.FADEOUT_WAIT:
                if (Time.time - startTime >= delayTime)
                {
                    state = State.FADEOUT;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                    }
                }
                break;

            case State.FADEIN:
                // ループ処理
                if (audioSource.time >= loopEnd)
                {
                    audioSource.time = loopStart;
                }

                // フェードとディレイがどちらも0なら即開始
                if (fadeTime <= 0f)
                {
                    audioSource.volume = SoundManager.BGMVolume * volume;
                    state = State.PLAYING;
                    return;
                }

                float t;
                t = (Time.time - startTime - fadeTime) / (fadeTime);
                t = Mathf.Clamp01(t);
                audioSource.volume = Mathf.Lerp(0f, SoundManager.SEVolume, t) * volume;

                if (t == 1f)
                {
                    state = State.PLAYING;
                }
                break;

            case State.FADEOUT:
                // ループ処理
                if (audioSource.time >= loopEnd)
                {
                    audioSource.time = loopStart;
                }

                // フェード時間が0なら即ストップ.
                if (fadeTime <= 0f)
                {
                    audioSource.volume = 0f;
                    audioSource.Stop();
                    state = State.STOP;
                    OnStop.Call();
                    return;
                }

                t = (Time.time - startTime - delayTime) / (fadeTime);
                t = Mathf.Clamp01(t);
                audioSource.volume = Mathf.Lerp(SoundManager.BGMVolume, 0f, t) * volume;

                if (t == 1f)
                {
                    audioSource.volume = 0f;
                    audioSource.Stop();
                    state = State.STOP;
                    OnStop.Call();
                }
                break;
        }
    }

    /// <summary>
    /// BGMの再生を開始
    /// </summary>
    public void Play(AudioClip clip, float fadeTime, float delayTime)
    {
        if (clip == null)
        {
            Debug.LogError("BGM AudioClip is Null.");
            return;
        }

        // 既に同じBGMを再生中
        if (IsPlaying == true && clip == audioSource.clip) { return; }

        //Debug.Log("BGM Play " + clip.name);
        this.fadeTime = fadeTime;
        this.delayTime = delayTime;
        this.startTime = Time.time;
        this.pauseTime = Time.time;

        if (fadeTime + delayTime <= 0f)
        {
            audioSource.volume = SoundManager.BGMVolume * volume;
            state = State.PLAYING;
        }
        else if (delayTime > 0f)
        {
            audioSource.volume = 0f;
            state = State.FADEIN_WAIT;
        }
        else
        {
            audioSource.volume = 0f;
            state = State.FADEIN;
        }

        // 頭から再生
        audioSource.time = 0f;
        audioSource.clip = clip;
        audioSource.Stop();

        // ループ設定 TODO 設定ファイルから読み込む
        this.loopStart = 0f;
        this.loopEnd = audioSource.clip.length;
    }

    /// <summary>
    /// BGMを停止します
    /// </summary>
    public void Stop(float fadeTime, float delayTime, Action onStop)
    {
        //Debug.Log("BGM Stop " + name);
        this.fadeTime = fadeTime;
        this.delayTime = delayTime;
        this.startTime = Time.time;
        this.OnStop = onStop;

        if (fadeTime + delayTime <= 0f)
        {
            audioSource.volume = 0f;
            audioSource.Stop();
            state = State.STOP;
        }
        else if (delayTime > 0f)
        {
            state = State.FADEOUT_WAIT;
        }
        else
        {
            state = State.FADEOUT;
        }
    }

    /// <summary>
    /// BGMを一時停止します
    /// </summary>
    public void Pause()
    {
        Debug.Log("BGM Pause " + name);
        audioSource.Pause();
        pauseTime = Time.time;
        state = State.PAUSE;
    }

    /// <summary>
    /// BGMを（一時停止から）再開します
    /// </summary>
    public void Resume()
    {
        Debug.Log("BGM Resume " + name);
        float elapsedTime = pauseTime - startTime;
        startTime = Time.time - elapsedTime;
        bool needWait = elapsedTime < delayTime;
        bool needFade = elapsedTime >= delayTime && elapsedTime < (delayTime + fadeTime);

        if (needWait)
        {
            audioSource.Stop();
            state = State.FADEIN_WAIT;
        }
        else if (needFade)
        {
            audioSource.Play();
            state = State.FADEIN;
        }
        else
        {
            audioSource.Play();
            state = State.PLAYING;
        }
    }

    #endregion
}
