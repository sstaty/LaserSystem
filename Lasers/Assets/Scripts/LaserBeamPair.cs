using UnityEngine;

public class LaserBeamPair {
    public LaserBeam incoming;
    public LaserBeam outgoing;

    public LaserBeamPair(LaserBeam incoming, LaserBeam outgoing) {
        this.incoming = incoming;
        this.outgoing = outgoing;
    }
}

