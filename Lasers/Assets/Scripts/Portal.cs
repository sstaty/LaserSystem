using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Portal : OpticalElement {
    public Transform Target;

    private List<LaserBeamPair> laserBeamPairs = new List<LaserBeamPair>();

    private BoxCollider _boxCollider;

    private void Awake() {
        _boxCollider = GetComponent<BoxCollider>();
    }

    public override void RegisterLaserBeam(LaserBeam laserBeam) {
        LaserBeam outgoingLaserBeam = GameObject.Instantiate(laserBeam.Prefab, transform);
        laserBeamPairs.Add(new LaserBeamPair(laserBeam, outgoingLaserBeam));
    }
    public override void UnregisterLaserBeam(LaserBeam laserBeam) {
        var pair = GetPairFromIncomingBeam(laserBeam);

        if (pair.outgoing.OpticalElementThatTheBeamHit != null) {
            pair.outgoing.OpticalElementThatTheBeamHit.UnregisterLaserBeam(pair.outgoing);
        }

        laserBeamPairs.Remove(pair);
        GameObject.Destroy(pair.outgoing.gameObject);
    }
    public override void Propagate(LaserBeam laserBeam) {
        var pair = GetPairFromIncomingBeam(laserBeam);
        var localHitPosition = transform.InverseTransformPoint(pair.incoming.EndPosition);

        Vector3 targetPosition = Target.TransformPoint(localHitPosition);

        // Calculate the target direction
        Vector3 localDirection = transform.InverseTransformDirection(pair.incoming.Direction);
        Vector3 targetDirection = Target.TransformDirection(localDirection);


        // We add a small offset to the target position to avoid the beam to be stuck in the portal
        // This is dependant on size of collider
        targetPosition += targetDirection * _boxCollider.size.z;
        pair.outgoing.Propagate(targetPosition, targetDirection);
    }

    private LaserBeamPair GetPairFromIncomingBeam(LaserBeam laserBeam) => laserBeamPairs.Find(x => x.incoming == laserBeam);
}
