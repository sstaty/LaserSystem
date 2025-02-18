using UnityEngine;

public abstract class OpticalElement : MonoBehaviour
{
    public abstract void RegisterLaserBeam(LaserBeam laserBeam);

    public abstract void UnregisterLaserBeam(LaserBeam laserBeam);

    public abstract void Propagate(LaserBeam laserBeam);
}
