using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UI : Singleton<UI> {

    // Text fields
    [SerializeField] private UIText _day;
    [SerializeField] private UIText _subtitle;
    [SerializeField] private UIText _gameOver;

    [SerializeField] private GameObject _blackout;
    [SerializeField] private GameObject _splash;

    [SerializeField] private InventoryView _inventory;
    [SerializeField] private CompassView _compass;

    public InventoryView Inventory { get { return _inventory; } }
    public CompassView Compass { get { return _compass; } }

    public static bool Handled { get; set; }

    void Start()
    {
        _splash.SetActive(true);
    }

    public void Update() {
        Handled = false;
    }

    public void SetSubtitle(string message) {
        _subtitle.SetText(message, 3.0f, 1.0f);
    }

    public void SetDay(int day) {
        _day.SetText("Day " + (day+1), 3.0f, 1.0f);
    }

    public void GameOver(string message) {
        _blackout.SetActive(true);
        _gameOver._text.enabled = true;
        _gameOver.SetText(message, 30.0f, 1.0f);
    }

    public void ShowInventory() {
        UI.Handled = true;
        _inventory.ShowPlayerInventory();
    }

    // Helper class for handling text state
    [System.Serializable]
    private class UIText {
        public Text _text;
        private Coroutine co;

        public void SetText(string message, float start, float duration) {
            if (co != null) {
                _text.StopCoroutine(co);
            }

            _text.enabled = true;
            _text.color = Color.white;
            _text.text = message;
            co = _text.StartCoroutine(FadeText(start, duration));
        }

        private IEnumerator FadeText(float start, float duration) {
            yield return new WaitForSeconds(start);

            float elapsed = 0.0f;
            while(elapsed < duration) {
                elapsed += Time.deltaTime;
                var a = 1.0f - (elapsed / duration);
                _text.color = new Color(1,1,1, a);
                yield return null;
            }

            _text.enabled = false;
        }
    }

    public void FadeOut() {
        _gameOver._text.enabled = false;
        _blackout.SetActive(true);
		LeanTween.alpha(_blackout.GetComponent<RectTransform>(), 1.0f, 0.6f);
    }

    public void FadeIn() {
		LeanTween.alpha(_blackout.GetComponent<RectTransform>(), 0.0f, 0.6f).setOnComplete(
            () => { _blackout.SetActive(false); }
        );
    }

    public void HideSplash() {
        LeanTween.alpha(_splash.GetComponent<RectTransform>(), 0.0f, 1.0f).setOnComplete(
            () => { _splash.SetActive(false); }
        );
    }
}

