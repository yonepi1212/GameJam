/*  Define
    定義群
*/

using UnityEngine;

public static partial class Define
{

    public static class Window
    {
        public static float TransitionTime = 0.15f;
        public static float FocusTransitionTime = 0.6f;
    }
    #region Character
    // 改行
    public static string Newline = System.Environment.NewLine;
    // スラッシュ
    public static string Slash = @"/";
    // ハイフン
    public static string Hyphen = @"-";
    // 半角スペース
    public static string Space = @" ";
    #endregion

    #region Path
    // パス関連
    public static class Path
    {
        // PrefabのRootパス
        public static string Root = @"Prefab/";
        // Assetのパス
        public static string Asset = @"Assets/";
        // FileCacheのパス
        public static string FileCache = @"FileCache/";
        // Textureのパス
        public static string Texture = @"Texture/";
        // スキルが置かれているプレファブのディレクトリ
        public static string SkillPrefab = @"Prefab/Battle/Skill/";
		// ビルド出力用デフォルトパス(Android用)
        public static string DefaultAndroidBuildPath = "Build/Android/trinity.apk";
		// ビルド出力用デフォルトパス(iOS用)
        public static string DefaultIosBuildPath = "Build/iOS";
		// ビルド出力用デフォルトパス(WebGL用)
		public static string DefaultWebGLBuildPath = "Build/WebGL";
        // ビルド時に使用するBundleIdentifier
        public static string BundleIdentifier = "jp.co.griphone.trinity";
        // Csvルート
        public static string CsvRoot = @"Csv/";
        // StreamingAssetsのパス
        public static string StreamingAssets = @"StreamingAssets/";

        // プレハブ名をまとめたクラス
        public static class Prefab
        {
            // Managerのパス
            public static string Manager = Path.Root + @"Manager/";
            // Canvasのパス
            public static class Canvas
            {
                // Canvasのルートパス
                public static string Root = Path.Root + @"Canvas/";
                // MainCanvasのパス
                public static string Main = Canvas.Root + @"Main/";
                // Toolsのパス
                public static string Tools = Canvas.Root + @"Tools/";
                // Battleのパス
                public static string Battle = Canvas.Root + @"Battle/";
            }
            // Battleのパス
            public static class Battle
            {
                // Battleのルートパス
                public static string Root = Path.Root + @"Battle/";
                // スキルのパス
                public static string Skill = Battle.Root + @"Skill/";
                // スキルのパス
                public static string Effect = Battle.Root + @"Effect/";
            }
            // Commonのパス
            public static class Common
            {
                // Battleのルートパス
                public static string Root = Path.Root + @"Common/";
            }
            // Characterのパス
            public static class Character
            {
                // Battleのルートパス
                public static string Root = Path.Root + @"Character/";
            }
            // Metamorphoseのパス
            public static class Metamorphose
            {
                // Metamorphoseのパスのルートパス
                public static string Root = Path.Root + @"Metamorphose/";
            }

            // ローカルDBのパス
            public static class Db
            {
                public static string Root = @"Db/";
                public static string Battle = Root + @"Battle/";
            }
            // Cameraのパス
            public static string Camera = Path.Root + @"Camera/";
            // Movieのパス
            public static string Movie = Path.Root + @"Movie/";
            // Campのパス
            public static string Camp = Path.Root + @"Camp/";
        }
    }
    #endregion

    #region Name
    // 名前関連
    public static class Name
    {
        // EventSystem
        public static string EventSystem = @"EventSystem"; 
        // MainCanvas
        public static string MainCanvas = @"main_canvas"; 
        // CanvasTools
        public static string CanvasTools = @"CanvasTools";
        // AddOnManager
        public static string AddOnManager = @"AddOnManager";
        // CameraManager
        public static string CameraManager = @"CameraManager";
        // カメラの名前
        public static class Camera
        {
            // CameraManager
            public const string Main = @"main_camera";
            public const string CutIn = @"CutInCamera";
            public const string UI = @"ui_camera";
            public const string Focus = @"focus_camera";
        }
        // SoundManager
        public static string SoundManager = @"SoundManager";
        // TransitionManager
        public static string TransitionManager = @"TransitionManager";

        // csv関連
        public static class Csv
        {
            // クライアントテキスト
            public static string ClientText = @"ClientText";
        }

        // Sound関連
        public static class Sound
        {
            public static class BGM
            {
                public static string Home = @"Home";
                public static string Quest = @"Quest";
            }

            public static class SE
            {
                public static string ButtonPushed = @"Select02";
                public const string SkillFire = @"Skill01";
                public const string SkillThunder = @"Skill02";
                public const string SkillWater = @"Skill03";
                public const string SkillLight = @"Skill04";
                public const string SkillDark = @"Skill05";
                // 無属性は仮SE
                public const string SkillNone = @"Skill01";
                public const string SkillBuf = @"se_128_04";
                public const string SkillDebuf = @"se_38_03";
                public const string SkillHeal = @"se_battle_magic1_48_01";

            }
        }
    }
    #endregion

    #region API
    // API接続関連
    public static class Api
    {
        // テストサーバーURL
        public static string apiServerRootUrl = @"http://sb01.trinity.griphone.co.jp/";

        // バトルAPIのアクションリスト
        public static class Battle
        {
            public static string ACTION = @"battle/action";
            public static string PERIODIC = @"battle/periodic";
            public static string CHANGE_POSITION_ACTION = @"battle/change_position_action";
            public static string CHANGE_POSITION_PERIODIC = @"battle/change_position_periodic";
            public static string UNIQUE_SKILL_ACTION = @"battle/unique_skill_action";
            public static string UNIQUE_SKILL_PERIODIC = @"battle/unique_skill_periodic";
        }

        // ホーム画面APIのアクションリスト
        public static class Home
        {
            // 例
            public static string INDEX = @"home/index";
        }

        public static string SelectedServerUrl
        {
            get
            {
                return apiServerRootUrl;
            }
        }
    }
    #endregion

    #region Scene
    // シーン名
    public static class Scene
    {
        // Title
        public static string Title = @"TitleScene";
        // Movie
        public static string Movie = @"MovieScene";
        // Camp
        public static string Camp = @"CampScene";
        // Battle
        public static string Battle = @"BattleScene";
    }
    #endregion

    #region StateName
    public static class StateName
    {
        public static string Chant = @"Chant";
        public static string Metamorphose = @"Metamorphose";
        public static string Activate = @"Activate";
        public static string Complete = @"Complete";
        public static string Attack = @"Attack";
        public static string Step = @"Step";
        public static string Run = @"Run";
        public static string Proximity = @"Proximity";
    }
    #endregion

    #region SkillName
    // スキル名リスト(TODO:DB or Message Packで管理する可能性大)
    public static class SkillName
    {
        public static string Fire = @"Fire";
    }
    #endregion

    #region PlayerPrefs

    public static class UserData
    {
        public static string EpisodeStageUnLockIndex = @"EpisodeStageUnLockIndex";
    }

    #endregion

    #region Animation Curve

    public static class AnimationCurves
    {
        private static AnimationCurve _countUpCurve;
        public static AnimationCurve CountUpCurve
        {
            get
            {
                if (_countUpCurve == null)
                {
                    AnimationCurve curve = new AnimationCurve();
                    float firstScaleTime = 0.15f;
                    float secondScaleTime = 0.2f;
                    float thirdScaleTime = 0.2f;
                    curve.AddKey(new Keyframe(0f, 1f));
                    curve.AddKey(new Keyframe(firstScaleTime, 1.8f));
                    curve.AddKey(new Keyframe(firstScaleTime + secondScaleTime, 0.8f));
                    curve.AddKey(new Keyframe(firstScaleTime + secondScaleTime + thirdScaleTime, 1f));
                    _countUpCurve = curve;
                }
                return _countUpCurve;
            }
        }

        private static AnimationCurve _countDownCurve;
        public static AnimationCurve CountDownCurve
        {
            get
            {
                if (_countDownCurve == null)
                {
                    AnimationCurve curve = new AnimationCurve();
                    float firstScaleTime = 0.15f;
                    float secondScaleTime = 0.2f;
                    float thirdScaleTime = 0.2f;
                    curve.AddKey(new Keyframe(0f, 1f));
                    curve.AddKey(new Keyframe(firstScaleTime, 0.6f));
                    curve.AddKey(new Keyframe(firstScaleTime + secondScaleTime, 1.2f));
                    curve.AddKey(new Keyframe(firstScaleTime + secondScaleTime + thirdScaleTime, 1f));
                    _countDownCurve = curve;
                }
                return _countDownCurve;
            }
        }

        // 直線
        //(0, 0, 0, 1) (1, 1, 1, 0) 
        private static AnimationCurve _linearCurve;
        public static AnimationCurve LinearCurve
        {
            get
            {
                if (_linearCurve == null)
                {
                    Keyframe keyFrameStart = new Keyframe(0f, 0f, 0f, 1f);
                    Keyframe keyFrameEnd = new Keyframe(1f, 1f, 1f, 0f);
                    _linearCurve = new AnimationCurve(keyFrameStart, keyFrameEnd);
                }
                return _linearCurve;
            }
        }

        // スムーズ　視点終点近辺で緩やかにカーブ
        //(0, 0, 0, 0) (1, 1, 0, 0)
        private static AnimationCurve _smoothCurve;
        public static AnimationCurve SmoothCurve
        {
            get
            {
                if (_smoothCurve == null)
                {
                    Keyframe keyFrameStart = new Keyframe(0f, 0f, 0f, 0f);
                    Keyframe keyFrameEnd = new Keyframe(1f, 1f, 0f, 0f);
                    _smoothCurve = new AnimationCurve(keyFrameStart, keyFrameEnd);
                }
                return _smoothCurve;
            }
        }

        // 終点を保持し続ける（動かない）
        //(0, 1) (1, 1) 
        private static AnimationCurve _noneCurve;
        public static AnimationCurve NoneCurve
        {
            get
            {
                if (_noneCurve == null)
                {
                    Keyframe keyFrameStart = new Keyframe(0f, 1f);
                    Keyframe keyFrameEnd = new Keyframe(1f, 1f);
                    _noneCurve = new AnimationCurve(keyFrameStart, keyFrameEnd);
                }
                return _noneCurve;
            }
        }
    }

    #endregion

    #region Button

    public static class Button
    {
        public static float AnimationTime = 0.30f;
    }

    #endregion
    }
