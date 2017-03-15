//using System;
//using System.IO;
using UnityEngine;
using UnityEngine.Profiling;


public class DebugManager : SingletonMonoBehaviour<DebugManager>
{
    #region Declaration
    private int _count;
    private int _currentCount;
    private int _totalFrames;
    private int _worstFps;
    private int _totalFps;
    private int _fpsSampingCount;
    private int _fps;
    private int _nextTime;
    [HideInInspector]
    private string _text;

    private bool _showFlg;

    //ログテキスト書き出し用変数
    //private string _memLogText;
    //private int _worstFrame;
    #endregion

    #region Monobehaviour
    override protected void Awake()
    {
        _worstFps = 999;
        _nextTime = 1;
        _fps = 0;

        _showFlg = false;
        base.Awake();
    }

    void Update() {

        if (Input.GetKeyDown(KeyCode.D))
        {
            _showFlg = !_showFlg;
        }

        _text = "";
        SetFps();
        SetMemStatus();
    }

    void OnGUI()
    {
        if (_showFlg)
        {
            GUI.TextField(new Rect(10, 10, 260, 70), _text);
        }
    }
    #endregion

    #region Private Method

    private void SetFps()
    {
        _count++;
        _totalFrames++;

        _text += "Frame : " + _totalFrames + "\n";


        if (Time.time >= _nextTime)
        {
            _fps = _count;

            _currentCount = _count;
            _totalFps += _count;
            _count = 0;
            _nextTime += 1;

            _fpsSampingCount++;

            if (_currentCount < _worstFps && _fpsSampingCount > 1)
            {
                _worstFps = _currentCount;
            }
        }

        if (_fps == 0)
        {
            _text += "FPS(Now / Ave / Worst) : - ";
        }
        else
        {
            _text += "FPS(Now / Ave / Worst) : " + _fps;


            if (_fpsSampingCount > 2)
            {
                float averageFps = (Mathf.Round(((float)_totalFps / (float)_fpsSampingCount) * 10 ) / 10);

                _text += " / " + _worstFps;
                _text += " / " + averageFps + "\n";
            }
            else
            {
                _text += " / -";
                _text += " / - \n";
            }
        }
    }


    private void SetMemStatus()
    {
        uint monoUsedSize = Profiler.GetMonoUsedSize();
        uint monoHeapSize = Profiler.GetMonoHeapSize();
        uint totalAllocatedMemory = Profiler.GetTotalAllocatedMemory(); // == Profiler.usedHeapSize
        uint totalReservedMemory = Profiler.GetTotalReservedMemory();
        string memText = string.Format(
            "MemMono : {0} / {1} MB({2:f1}%)\n" +
            "MemTotal : {3} / {4} MB({5:f1}%)\n",
            (monoUsedSize / 1024).ToString("N0"),
            (monoHeapSize / 1024).ToString("N0"),
            100.0 * monoUsedSize / monoHeapSize,
            (totalAllocatedMemory / 1024).ToString("N0"),
            (totalReservedMemory / 1024).ToString("N0"),
            100.0 * totalAllocatedMemory / totalReservedMemory
        );
        _text += memText;

        // 
        //_memLogText = string.Format(
        //    "{0},{1},{2:f1},{3},{4},{5:f1}",
        //    (_monoUsed / 1024).ToString(),
        //    (_monoSize / 1024).ToString(),
        //    100.0 * _monoUsed / _monoSize,
        //    (_totalUsed / 1024).ToString(),
        //    (_totalSize / 1024).ToString(),
        //    100.0 * _totalUsed / _totalSize
        //);
    }
    #endregion

    
    /*
        private void OnApplicationQuit()
        {
            #if UNITY_EDITOR
                this.logSave();
            #endif
        }

        public void logSave()
        {
    #if UNITY_EDITOR
        float _averageFPS = _totalFPS / _fpsSampringCount;

        string log;
        System.DateTime datetime = System.DateTime.Now;
        EffectManager Eff = GetComponent<EffectManager>();

        log = datetime.ToString("yyyy/MM/dd,HH:mm:ss"); // date time
        log += "," + PhaseStr;                          // pahse
        log += "," + _averageFPS.ToString();            // FPS Average
        log += "," + _worstFPS.ToString();              // FPS Worst
        log += "," + _worstFrame.ToString();            // FPS Worst Frame
        //log += "," + Eff.EffectObj.name;                // EffectName
        log += "," + Eff.EffCounter.ToString();         // EffectCount
        log += "," + (Eff.EffCounter * 2).ToString();   // EffectTris
        log += "," + (Eff.EffCounter * 4).ToString();   // EffectVarts
        log += "," + _memLogText;                       // memory

        //StreamWriter sw = new StreamWriter(Application.dataPath + "/Performance.txt", true); //true=追記 false=上書き
        StreamWriter sw = new StreamWriter(Application.dataPath + "/Log/Performance.txt", true); //true=追記 false=上書き

        sw.WriteLine(log);
        sw.Flush();
        sw.Close();
    #endif
        }
    }
    */

}