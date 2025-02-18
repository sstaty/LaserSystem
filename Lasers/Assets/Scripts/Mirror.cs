using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


public class Mirror : MonoBehaviour
{
    public List<LaserBeamPair> laserBeamPairs = new List<LaserBeamPair>();

    public void RegisterLaserBeam(LaserBeam laserBeam) {
        LaserBeam outgoingLaserBeam = GameObject.Instantiate(laserBeam.Prefab, transform);
        laserBeamPairs.Add(new LaserBeamPair(laserBeam, outgoingLaserBeam));
    }
    public void UnregisterLaserBeam(LaserBeam laserBeam) {
        var pair = GetPairFromIncomingBeam(laserBeam);

        if (pair.outgoing.MirrorTheBeamHit != null) {
            pair.outgoing.MirrorTheBeamHit.UnregisterLaserBeam(pair.outgoing);
        }

        laserBeamPairs.Remove(pair);
        GameObject.Destroy(pair.outgoing.gameObject);
    }

    public void Propagate(LaserBeam laserBeam) {
        var pair = GetPairFromIncomingBeam(laserBeam);
        Vector3 outgoingDirection = Vector3.Reflect(pair.incoming.Direction, pair.incoming.HitNormal);
        pair.outgoing.Propagate(pair.incoming.EndPosition, outgoingDirection);
    }

    private LaserBeamPair GetPairFromIncomingBeam(LaserBeam laserBeam) => laserBeamPairs.Find(x => x.incoming == laserBeam);
}


public class LaserBeamPair {
    public LaserBeam incoming;
    public LaserBeam outgoing;

    public LaserBeamPair(LaserBeam incoming, LaserBeam outgoing) {
        this.incoming = incoming;
        this.outgoing = outgoing;
    }
}
