using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CompassView : Singleton<CompassView> {

    [SerializeField] private RectTransform _map;
    [SerializeField] private RectTransform _needle;
    [SerializeField] private RectTransform _landMarker;
    [SerializeField] private RectTransform _boatMarker;
    [SerializeField] private Text _landDist;
    [SerializeField] private Text _boatDist;

    private bool _zoomed = false;

    void OnEnable()
    {
        DayNightController.Instance.OnSunset += OnSunset;
    }

    void OnDisable()
    {
        if (DayNightController.Instance != null)
            DayNightController.Instance.OnSunset -= OnSunset;
    }

    public void OnSunset(int day)
    {
        if (_zoomed)
            Tap();
    }

    public void UpdateLandmarks(LandmarkController landmarks) {
        float north = landmarks.Rotation;
        _needle.eulerAngles = new Vector3(0.0f, 0.0f, north);

        _landMarker.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        _landMarker.localPosition = landmarks.IslandPos() * 100;
        _landMarker.GetComponent<Image>().color = new Color(1,1,1, landmarks.IslandFade());

        _landDist.text = ((int)landmarks.IslandDist()).ToString() + " M";

        _boatMarker.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        _boatMarker.localPosition = landmarks.BoatPos() * 100;
        _boatMarker.GetComponent<Image>().color = new Color(1,1,1, landmarks.BoatFade());

        _boatDist.text = ((int)landmarks.BoatDist()).ToString() + " M";
    }

    public void Tap()
    {
        UI.Handled = true;
        _zoomed = !_zoomed;

        if (_zoomed) {
            _map.localScale = new Vector3(1,1,1);
            _landDist.gameObject.SetActive(true);
            _boatDist.gameObject.SetActive(true);
        }
        else {
            _map.localScale = new Vector3(.5f,.5f,1);
            _landDist.gameObject.SetActive(false);
            _boatDist.gameObject.SetActive(false);
        }
    }
}

