/* BattleDefine.cs
    ホーム用のDefine
*/
using UnityEngine;

public static partial class Define
{
    public static class Home
    {
        public static class Path
        {
            public static string Root = @"Prefab/Home/";
            public static string Background = Root + @"Background/";
        }

        // TODO:名前はCSV管理になる可能性大
        public static class Name
        {
            public static string Alpha2Background = @"bg_h_010";
            public static string ForcusBackground = @"focus_background";
        }
        private static Transform _world;
        public static Transform World
        {
            get
            {
                if (_world == null)
                {
                    var g = new GameObject { name = "home_world" };
                    Object.DontDestroyOnLoad(g);
                    _world = g.transform;
                }

                return _world;
            }
        }
    }


}
