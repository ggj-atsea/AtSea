using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandmarkController : Singleton<LandmarkController> {

    private float _kShippingLaneDistMin = 10.0f;
    private float _kShippingLaneDistMax = 50.0f;
    private float _kIslandDistMin = 10.0f;
    private float _kIslandDistMax = 50.0f;

    private Vector2 _raft;
    private Vector2 _shippingLane;
    private Vector2 _island;
    private Vector2 _north;

    float _rotation = 0.0f;

    public float Rotation {
        get {
            return _rotation;
        }
    }

    public void Start() {
        _raft = new Vector2(0.0f, 0.0f);
        _shippingLane = Random.insideUnitCircle.normalized * Random.Range(_kShippingLaneDistMin, _kShippingLaneDistMax);
        _island = Random.insideUnitCircle.normalized * Random.Range(_kIslandDistMin, _kIslandDistMax);
        _north = new Vector2(100.0f, 0.0f);

        DayNightController.Instance.OnDawn += OnDawn;
    }

    public void OnDestroy() {
        if (DayNightController.HasInstance) {
            DayNightController.Instance.OnDawn -= OnDawn;
        }
    }

    private void OnDawn(int day) {
        Vector2 movement = Random.insideUnitCircle.normalized;
        _raft = _raft + movement;

        _rotation += Random.Range(-30.0f, 30.0f);
        if (_rotation > 180.0f) {
            _rotation -= 360.0f;
        }
        if (_rotation < -180.0f) {
            _rotation += 360.0f;
        }

        UI.Instance.Compass.UpdateLandmarks(this);
    }

    public Vector2 BoatPos() {
        return _shippingLane.normalized;
    }

    public float BoatFade() {
        return 1.0f - (_shippingLane.magnitude - _kShippingLaneDistMin) / (_kShippingLaneDistMax - _kShippingLaneDistMin);
    }

    public Vector2 IslandPos() {
        return _island.normalized;
    }

    public float IslandFade() {
        return 1.0f - (_island.magnitude - _kIslandDistMin) / (_kIslandDistMax - _kIslandDistMin);
    }
}

