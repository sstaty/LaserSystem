using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBeam : MonoBehaviour
{
    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public Vector3 HitNormal;
    public Vector3 Direction => (EndPosition - StartPosition).normalized;

    public LaserBeam Prefab;

    private const float _longestBeamDistance = 100f;

    private Mirror _mirrorTheBeamHit;
    
    private LineRenderer _lineRenderer;

    public Mirror MirrorTheBeamHit { 
        get => _mirrorTheBeamHit; 
        set {
            if (_mirrorTheBeamHit == value) {
                return;
            }
            else {
                if (_mirrorTheBeamHit != null) {
                    _mirrorTheBeamHit.UnregisterLaserBeam(this);
                }

                _mirrorTheBeamHit = value;

                if (_mirrorTheBeamHit != null) {
                    _mirrorTheBeamHit.RegisterLaserBeam(this);
                }
            }
        }
    }


    private void Awake() {                                                                                                               
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
    }

    public void Propagate(Vector3 startPosition, Vector3 direction) {
        Vector3 endPosition = startPosition + direction * _longestBeamDistance;
        Vector3 hitNormal = Vector3.zero;

        if (Physics.Raycast(startPosition, direction, out RaycastHit hit, _longestBeamDistance)) {
            endPosition = hit.point;
            hitNormal = hit.normal;

            if (hit.collider.TryGetComponent(out Mirror mirror)) {
                MirrorTheBeamHit = mirror;
            }
            else {
                MirrorTheBeamHit = null;
            }
        }
        else {
            MirrorTheBeamHit = null;
        }

        StartPosition = startPosition;
        EndPosition = endPosition;
        HitNormal = hitNormal;
        UpdateVisuals();

        if (MirrorTheBeamHit) {
            MirrorTheBeamHit.Propagate(this);
        }
    }

    void UpdateVisuals() {
        _lineRenderer.SetPosition(0, StartPosition);
        _lineRenderer.SetPosition(1, EndPosition);
    }

}
