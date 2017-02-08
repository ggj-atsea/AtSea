using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EquipmentView : Singleton<EquipmentView> {

    [SerializeField] private GridLayoutGroup _items;
    [SerializeField] private InventoryCard _card;

    // Populate UI

	void Start()
	{
		gameObject.SetActive (false);

        Inventory.OnItemAdded += (item) => Refresh();
        Inventory.OnItemRemoved += (item) => Refresh();

        Refresh();
	}

    private void Refresh() {
        foreach (GameObject c in _items.transform) {
            Destroy(c);
        }

        var equipment = Inventory.Instance.GetEquippedItems();
        if (equipment.Count > 0)
            gameObject.SetActive (true);

        for (int i = 0; i < equipment.Count; ++i) {
            var item = equipment[i];
            var card = Instantiate(_card, _items.transform, false);
//            var image = string.Equals(item.Key.Name, "Food") ? _food : _water;
//            card.transform.GetChild(0).GetComponent<Image>().sprite = image;
            card.GetComponent<Button>().onClick.AddListener(() => Equip(i));
            card.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    private void Equip(int slot) {
    }
}

