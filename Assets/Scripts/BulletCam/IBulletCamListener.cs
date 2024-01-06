using UnityEngine;

/// <summary>
/// The Interface IBulletCamListener can be attached to classes that need to be
/// informed if we enter or leave the bullet cam.
/// </summary>
public interface IBulletCamListener
{
    /// <summary>
    /// Will be called if the bullet cam starts.
    /// We have access to the bullet and the target RaycastHit.
    /// </summary>
    /// <param name="targetBullet">target bullet</param>
    /// <param name="targetHit">target hit</param>
    public void OnBulletCamStart(Bullet targetBullet, RaycastHit targetHit);

    /// <summary>
    /// Will be called if the bullet cam ends (The bullet hits the enemy).
    /// We have access to the bullet and the target RaycastHit.
    /// </summary>
    /// <param name="targetBullet">target bullet</param>
    /// <param name="targetHit">target hit</param>
    public void OnBulletCamEnd(Bullet targetBullet, RaycastHit targetHit);
}
