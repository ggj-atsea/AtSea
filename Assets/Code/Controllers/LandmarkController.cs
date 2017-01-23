using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandmarkController : Singleton<LandmarkController> {

    private float _kShippingLaneDistMin = 10.0f;
    private float _kShippingLaneDistMax = 150.0f;
    private float _kShowShippingLane = 8.0f;
    private float _kIslandDistMin = 10.0f;
    private float _kIslandDistMax = 150.0f;
    private float _kShowIsland = 8.0f;

    [SerializeField] private GameObject _shippingLaneObj;
    [SerializeField] private GameObject _islandObj;

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
        _shippingLane = Random.insideUnitCircle.normalized * Random.Range(_kShippingLaneDistMin + 10, _kShippingLaneDistMax);
        _island = Random.insideUnitCircle.normalized * Random.Range(_kIslandDistMin + 10, _kIslandDistMax);
        _north = new Vector2(100.0f, 0.0f);

        DayNightController.Instance.OnDawn += OnDawn;
    }

    public void OnDestroy() {
        if (DayNightController.HasInstance) {
            DayNightController.Instance.OnDawn -= OnDawn;
        }
    }

    public void MovePlayer(Vector3 motion) {
        var offset = new Vector2(motion.x, motion.z);
        _shippingLane -= offset;
        _island -= offset;
        _raft += offset;
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
        _rotation = 0.0f;

        UI.Instance.Compass.UpdateLandmarks(this);

        ShowShippingLane(_shippingLane.magnitude < _kShowShippingLane);
        ShowIsland(_island.magnitude < _kShowIsland);
    }

    public Vector2 BoatPos() {
        var result = _shippingLane.normalized;
        if (_shippingLane.magnitude < _kShippingLaneDistMin)
            result *= (_shippingLane.magnitude / _kShippingLaneDistMin);
        return result;
    }

    public float BoatFade() {
        return 1.0f - (_shippingLane.magnitude - _kShippingLaneDistMin) / (_kShippingLaneDistMax - _kShippingLaneDistMin);
    }

    public Vector2 IslandPos() {
        var result = _island.normalized;
        if (_island.magnitude < _kIslandDistMin)
            result *= (_island.magnitude / _kIslandDistMin);
        return result;
    }

    public float IslandFade() {
        return 1.0f - (_island.magnitude - _kIslandDistMin) / (_kIslandDistMax - _kIslandDistMin);
    }

    private void ShowShippingLane(bool show)
    {
        Vector2 pos = _shippingLane.normalized * 10;
        _shippingLaneObj.SetActive(show);
        _shippingLaneObj.transform.position = new Vector3(pos.x, 0, pos.y);
    }

    private void ShowIsland(bool show)
    {
        Vector2 pos = _island.normalized * 10;
        _islandObj.SetActive(show);
        _islandObj.transform.position = new Vector3(pos.x, 0, pos.y);
    }

    public bool NearShip() {
        return _shippingLaneObj.activeInHierarchy;
    }
    public bool NearIsland() {
        return _islandObj.activeInHierarchy;
    }
}

