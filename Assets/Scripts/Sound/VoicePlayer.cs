using UnityEngine;
/// <summary>
/// Voice再生オブジェクト
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class VoicePlayer : MonoBehaviour
{
    #region public

    /// <summary>
    /// 再生中かどうか
    /// </summary>
    public bool IsPlaying
    {
        get { return audioSource.isPlaying; }
    }

    /// <summary>
    /// 音源 オーディオクリップ
    /// </summary>
    public AudioClip Clip
    {
        get { return audioSource == null ? null : audioSource.clip; }
    }

    #endregion

    #region private

    private AudioSource audioSource;
    private float volume;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }

    #endregion

    #region Method

    /// <summary>
    /// 初期化
    /// </summary>
    public void Init()
    {
        volume = 1f;
        transform.localPosition = Vector3.zero;
    }

    public void UpdateVolume()
    {
        if (IsPlaying == false) return;

        audioSource.volume = SoundManager.VoiceVolume * volume;
    }

    /// <summary>
    /// SEを再生します
    /// </summary>
    public void Play(AudioClip clip, bool isLoop, Vector3 worldPos)
    {
        if (clip == null)
        {
            Debug.LogError("SE AudioClip is Null.");
            return;
        }
        //Debug.Log("SE Play " + clip.name);

        audioSource.clip = clip;
        audioSource.loop = isLoop;
        transform.position = worldPos;//world pos

        audioSource.volume = SoundManager.VoiceVolume * volume;
        audioSource.time = 0f;
        audioSource.Play();
    }

    /// <summary>
    /// SEを停止します
    /// </summary>
    public void Stop()
    {
        audioSource.Stop();
        audioSource.clip = null;
    }

    /// <summary>
    /// SEを一時停止します
    /// </summary>
    public void Pause()
    {
        audioSource.Pause();
    }

    /// <summary>
    /// SEを（一時停止から）再生します
    /// </summary>
    public void Resume()
    {
        audioSource.Play();
    }

    /// <summary>
    /// リソース開放
    /// </summary>
    public void Release()
    {
        if (audioSource != null)
        {
            audioSource.clip = null;
        }
    }

    #endregion
}
