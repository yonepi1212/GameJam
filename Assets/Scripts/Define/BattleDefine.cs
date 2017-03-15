using UnityEngine;

/* BattleDefine.cs
    バトル用のDefine
*/
public static partial class Define
{
    public static class Battle
    {
        public static class Path
        {
            public static string Root = @"Prefab/QuestBattle/";
            public static string Camera = Root + @"Camera/";
            public static string Background = Root + @"Background/";
            public static string BattleStart = Root + @"BattleStart/";
            public static string BattleEnd = Root + @"BattleEnd/";
            public static string MovePath = Root + @"MovePath/";
            public static string Judgement = Root + @"Judgement/";
        }

        // TODO:名前はCSV管理になる可能性大
        public static class Name
        {
            public static string QuestBattleBossEndMove = @"QuestBattleBossEndMove";
            public static string QuestBattleEndMove = @"QuestBattleEndMove";
            public static string DesinityEffect = @"destiny_effect";
            public static string Gaze = @"gaze";
            public static string Alpha2Background = @"bg_b_020";
            public static string StartUiAnimation = @"quest_battle_start_animation";
            public static string StartCameraWorkVs4 = @"battle_start_camerawork_vs4";
            public static string StartCameraWorkBoss = @"battle_start_camerawork_boss";
            public static string JudgementAnimation = @"quest_battle_judge_animation";
            public static string JudgementContinueAnimation = @"quest_battle_judge_continue_animation";
            public static string StartCutCameraSetting = @"StartCut->Camera";
            public static string StartCutGazeSetting = @"Camera_Target";
        }

        // アクションを処理する間隔
        public static float RepeatDelay = 0.25f;

        public static class Time
        {
            public static float AutoSelectStateAllyIconTime = 0.1f;
            public static float ReadyAttackTime = 0.8f;
            public static float ShowEnemyIconTime = 0.3f;
        }

        public static class Position
        {
            public static class Camera
            {
                public static class Neutral
                {
                    public static Vector3 Mob = new Vector3(2.909f, 2.41f, -6.405f);
                    public static Vector3 Boss = new Vector3(0f, 0.54f, -8.56f);
                }
            }
            public static class Gaze
            {
                public static class Neutral
                {
                    public static Vector3 Mob = new Vector3(-0.964f, 0.139f, 1.432f);
                    public static Vector3 Boss = new Vector3(0f, 2.354f, 3.592f);
                }
            }
        }
    }
}
