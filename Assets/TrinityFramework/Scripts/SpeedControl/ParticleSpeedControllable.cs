using UnityEngine;
/* ParticleSpeedControllable.cs
    ParticleSystemのポーズ処理
*/
public class ParticleSpeedControllable : GameSpeedControllable
{
    #region Declration
    private ParticleSystem _particle;
    #endregion

    #region Private Method
    private ParticleSystem Particle
    {
        get { return _particle ?? (_particle = GetComponentInChildren<ParticleSystem>()); }
    }
    #endregion

    #region Public Method
    public override void ChangedSpeedNotification(float gameSpeed)
    {
        base.ChangedSpeedNotification(gameSpeed);

        if (Particle != null)
        {
            var main = Particle.main;
            main.simulationSpeed = gameSpeed;            
        }

    }
    #endregion

}
