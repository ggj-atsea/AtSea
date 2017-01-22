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

    public static void ShowPlayerInventory()
    {
        Instance.gameObject.SetActive(true);
        Instance.SetupPlayer();
    }

    public static void ShowPackageInventory()
    {
        Instance.gameObject.SetActive(true);
        Instance.SetupPackage();
    }

    #region Populate UI

    private void PopulatePlayerContents() {
        // TODO : get this data from somewhere
        var items = new List<string>();
        items.Add("foo");
        items.Add("bar");
        items.Add("ack");

        foreach (var item in items) {
            var card = Instantiate(_card);
            card.transform.SetParent(_playerContents.transform);
        }
    }

    private void PopulatePackageContents() {
        // TODO : get this data from somewhere
        var items = new List<string>();
        items.Add("foo");
        items.Add("bar");
        items.Add("ack");

        foreach (var item in items) {
            var card = Instantiate(_card);
            card.transform.SetParent(_playerContents.transform);
        }
    }

    #endregion

    #region Initialization Methods

    private void Reset() {
        foreach (Transform child in _playerContents.transform) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in _packageContents.transform) {
            Destroy(child.gameObject);
        }
    }

    private void SetupPlayer() {
        Reset();
        _packageInventory.SetActive(false);
        PopulatePlayerContents();
    }

    private void SetupPackage() {
        Reset();
        _packageInventory.SetActive(true);
        PopulatePlayerContents();
        PopulatePackageContents();
    }

    #endregion

    #region UI Callbacks

    public void Close()
    {
        gameObject.SetActive(false);
    }

    #endregion
}

