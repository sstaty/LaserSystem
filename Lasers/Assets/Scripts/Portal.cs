using System.Collections.Generic;
using UnityEngine;

public class Portal : OpticalElement {
    public Transform Target;

    private List<LaserBeamPair> laserBeamPairs = new List<LaserBeamPair>();


    public override void RegisterLaserBeam(LaserBeam laserBeam) {
        LaserBeam outgoingLaserBeam = GameObject.Instantiate(laserBeam.Prefab, transform);
        laserBeamPairs.Add(new LaserBeamPair(laserBeam, outgoingLaserBeam));
    }
    public override void UnregisterLaserBeam(LaserBeam laserBeam) {
        var pair = GetPairFromIncomingBeam(laserBeam);

        if (pair.outgoing.MirrorTheBeamHit != null) {
            pair.outgoing.MirrorTheBeamHit.UnregisterLaserBeam(pair.outgoing);
        }

        laserBeamPairs.Remove(pair);
        GameObject.Destroy(pair.outgoing.gameObject);
    }
    public override void Propagate(LaserBeam laserBeam) {
        var pair = GetPairFromIncomingBeam(laserBeam);
        pair.outgoing.Propagate(Target.position, Target.forward);
    }

    private LaserBeamPair GetPairFromIncomingBeam(LaserBeam laserBeam) => laserBeamPairs.Find(x => x.incoming == laserBeam);
}
