using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CompassView : Singleton<CompassView> {

    [SerializeField] private RectTransform _needle;
    [SerializeField] private RectTransform _landMarker;
    [SerializeField] private RectTransform _boatMarker;

    public void UpdateLandmarks(LandmarkController landmarks) {
        float north = landmarks.Rotation;
        _needle.eulerAngles = new Vector3(0.0f, 0.0f, north);

        _landMarker.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        _landMarker.localPosition = landmarks.IslandPos() * 100;
        _landMarker.GetComponent<Image>().color = new Color(1,1,1, landmarks.IslandFade());

        _boatMarker.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
        _boatMarker.localPosition = landmarks.BoatPos() * 100;
        _boatMarker.GetComponent<Image>().color = new Color(1,1,1, landmarks.BoatFade());
    }
}

