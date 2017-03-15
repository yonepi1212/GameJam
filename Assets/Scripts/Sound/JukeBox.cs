using System;
using System.Collections.Generic;
using UnityEngine;
/* JukeBox.cs
	α2サウンドjukeBoxクラス
*/
[Serializable]
public class JukeBox : ScriptableObject
{
    #region Declaration
    [Serializable]
    public class JokeBoxInfo
    {
        public SoundKind SoundKind;

        public AudioClip SoundClip;
    }

    public List<JokeBoxInfo> JokeBoxInfoList;
    #endregion

    #region Public Method

    public AudioClip GetAudioClip(SoundKind soundKind)
    {
        var audioClip = JokeBoxInfoList.Find(soundEnum => soundEnum.SoundKind == soundKind).SoundClip;
        return audioClip;
    }

    #endregion

}
