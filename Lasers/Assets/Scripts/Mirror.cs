using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;


[RequireComponent(typeof(Collider))]
public class Mirror : OpticalElement
{
    private List<LaserBeamPair> laserBeamPairs = new List<LaserBeamPair>();

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
        Vector3 outgoingDirection = Vector3.Reflect(pair.incoming.Direction, pair.incoming.HitNormal);
        pair.outgoing.Propagate(pair.incoming.EndPosition, outgoingDirection);
    }

    private LaserBeamPair GetPairFromIncomingBeam(LaserBeam laserBeam) => laserBeamPairs.Find(x => x.incoming == laserBeam);
}