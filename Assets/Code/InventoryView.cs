using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class InventoryView : Singleton<InventoryView> {

    [SerializeField] private GameObject _playerInventory;
    [SerializeField] private GameObject _packageInventory;

    [SerializeField] private GridLayoutGroup _playerContents;
    [SerializeField] private GridLayoutGroup _packageContents;
    
    [SerializeField] private InventoryCard _card;

    // Populate UI

    private void PopulatePlayerContents() {
        // TODO : get this data from somewhere
        var items = new List<string>();
        items.Add("foo");
        items.Add("bar");
        items.Add("ack");

        foreach (var item in items) {
            var card = Instantiate(_card, _playerContents.transform, false);
        }
    }

    private void PopulatePackageContents() {
        // TODO : get this data from somewhere
        var items = new List<string>();
        items.Add("foo");
        items.Add("bar");
        items.Add("ack");

        foreach (var item in items) {
            var card = Instantiate(_card, _packageContents.transform, false);
        }
    }

    // Show inventory dialog

    public void ShowPlayerInventory() {
        Reset();
        _packageInventory.SetActive(false);
        PopulatePlayerContents();
    }

    public void ShowPackageInventory() {
        Reset();
        _packageInventory.SetActive(true);
        PopulatePlayerContents();
        PopulatePackageContents();
    }

    private void Reset() {
        gameObject.SetActive(true);

        foreach (Transform child in _playerContents.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _packageContents.transform) {
            Destroy(child.gameObject);
        }
    }

    // Hide dialog

    public void Close()
    {
        gameObject.SetActive(false);
    }
}

