using TMPro;
using UnityEngine;

/* BattleHudDefine.cs
	バトルHUD用定義.
*/
public class BattleHudDefine
{
    #region Declaration
    // Decimal.
    private const float D = 255f;

    // カラー定義.
    public static class ColorDefine
    {
        // ダメージ.
        public static readonly Color Damage = new Color(212f / D, 21f / D, 91f / D);
        public static readonly Color DamageOutline = new Color(115f / D, 0f / D, 42f / D);
        // スコアアップ.
        public static readonly Color ScoreUp = new Color(48f / D, 149f / D, 220f / D);
        public static readonly Color ScoreUpOutline = new Color(13f / D, 91f / D, 145f / D);
        // スコアダウン.
        public static readonly Color ScoreDown = new Color(212f / D, 21f / D, 91f / D);
        public static readonly Color ScoreDownOutline = new Color(115f / D, 0f / D, 42f / D);
        // HP回復.
        public static readonly Color CureHp = new Color(128f / D, 255f / D, 0f / D);
        public static readonly Color CureHpOutline = new Color(61f / D, 121f / D, 0f / D);
        // アドエフェクト（クリティカル/急所/強打）.
        public static readonly Color AddDamage = new Color(246f / D, 139f / D, 0f / D);
        public static readonly Color AddDamageOutline = new Color(159f / D, 81f / D, 0f / D);
        // アドエフェクト（ミス）.
        public static readonly Color AddMiss = new Color(255f / D, 255f / D, 255f / D);
        public static readonly Color AddMissOutline = new Color(185f / D, 185f / D, 185f / D);
        // デフォルト（指定なし）.
        public static readonly Color Default = Color.white;
        public static readonly Color DefaultOutline = Color.black;
    }

    // フォント定義.
    public static class FontDefine
    {
        // 文字装飾.
        public static float OutlineThickness = 0.4f;
        public static FontStyles FontStyles = FontStyles.Bold;
    }
    #endregion
}
