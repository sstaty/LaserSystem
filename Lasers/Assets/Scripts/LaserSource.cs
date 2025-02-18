using UnityEngine;


public class LaserSource : MonoBehaviour
{
    public Transform sourceTransform;
    public LaserBeam laserBeam;


    private void Update() {
        Vector3 startPosition = sourceTransform.position;
        Vector3 direction = sourceTransform.forward;

        laserBeam.Propagate(startPosition, direction);
    }

}
