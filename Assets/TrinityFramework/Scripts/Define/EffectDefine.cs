/*
 * Title  EffectDefine
 * Date   2017/1/17
 * Description
 * EffectManagerで利用する設定
 * 
 * Sample Sandbox/SampleEffect
 */

public class EffectDefine
{
    #region Declration
    public enum EffectKind
    {
        [EnumExtension.Alias("eff_bullet_01")]
        Bullet01,
        [EnumExtension.Alias("eff_damage_01")]
        Damage01,
        [EnumExtension.Alias("trail_fire")]
        TrailFire,
        [EnumExtension.Alias("trail_thunder")]
        TrailThunder,
        [EnumExtension.Alias("trail_water")]
        TrailWater,
        [EnumExtension.Alias("trail_light")]
        TrailLight,
        [EnumExtension.Alias("trail_dark")]
        TrailDark,
        [EnumExtension.Alias("trail_default")]
        TrailDefault,
        [EnumExtension.Alias("explosion_fire")]
        ExplosionFire,
        [EnumExtension.Alias("explosion_thunder")]
        ExplosionThunder,
        [EnumExtension.Alias("explosion_ice")]
        ExplosionIce,
        [EnumExtension.Alias("explosion_dark")]
        ExplosionDark,
        [EnumExtension.Alias("explosion_light")]
        ExplosionLight,
        [EnumExtension.Alias("Fx_heal")]
        ExplosionHeal,
        [EnumExtension.Alias("Fx_buffup")]
        ExplosionBuff,
        [EnumExtension.Alias("Fx_buffdown")]
        ExplosionDebuff,
        [EnumExtension.Alias("Fx_death")]
        Death,
        [EnumExtension.Alias("Fx_death_small")]
        DeathSmall,

    }
    public string EffectLoadPath = "Prefab/QuestBattle/Effect/";
#endregion
}


