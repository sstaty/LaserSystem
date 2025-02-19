using System;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBeam : MonoBehaviour
{
    public float width = 0.1f;

    public Vector3 StartPosition;
    public Vector3 EndPosition;
    public Vector3 HitNormal;
    public Vector3 Direction => (EndPosition - StartPosition).normalized;

    public LaserBeam Prefab;

    private const float _longestBeamDistance = 100f;

    private OpticalElement _opticalElementThatTheBeamHit;
    
    private LineRenderer _lineRenderer;

    public OpticalElement OpticalElementThatTheBeamHit { 
        get => _opticalElementThatTheBeamHit; 
        set {
            if (_opticalElementThatTheBeamHit == value) {
                return;
            }
            else {
                if (_opticalElementThatTheBeamHit != null) {
                    _opticalElementThatTheBeamHit.UnregisterLaserBeam(this);
                }

                _opticalElementThatTheBeamHit = value;

                if (_opticalElementThatTheBeamHit != null) {
                    _opticalElementThatTheBeamHit.RegisterLaserBeam(this);
                }
            }
        }
    }


    private void Awake() {                                                                                                               
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _lineRenderer.startWidth = width;
        _lineRenderer.endWidth = width;
    }

    public void Propagate(Vector3 startPosition, Vector3 direction) {
        Vector3 endPosition = startPosition + direction * _longestBeamDistance;
        Vector3 hitNormal = Vector3.zero;

        if (Physics.Raycast(startPosition, direction, out RaycastHit hit, _longestBeamDistance)) {
            endPosition = hit.point;
            hitNormal = hit.normal;

            if (hit.collider.TryGetComponent(out OpticalElement opticalElement)) {
                OpticalElementThatTheBeamHit = opticalElement;
            }
            else {
                OpticalElementThatTheBeamHit = null;
            }
        }
        else {
            OpticalElementThatTheBeamHit = null;
        }

        StartPosition = startPosition;
        EndPosition = endPosition;
        HitNormal = hitNormal;
        UpdateVisuals();

        if (OpticalElementThatTheBeamHit) {
            OpticalElementThatTheBeamHit.Propagate(this);
        }
    }

    void UpdateVisuals() {
        _lineRenderer.SetPosition(0, StartPosition);
        _lineRenderer.SetPosition(1, EndPosition);
    }

}
