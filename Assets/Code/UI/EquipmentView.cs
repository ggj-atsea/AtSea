using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EquipmentView : Singleton<EquipmentView> {

    [SerializeField] private GridLayoutGroup _items;
    [SerializeField] private InventoryCard _card;
    [SerializeField] private Image[] _slots;

    // Populate UI

	void Start()
	{
		gameObject.SetActive (false);

        Inventory.OnItemAdded += (item) => Refresh();
        Inventory.OnItemRemoved += (item) => Refresh();

        Refresh();
	}

    private void Refresh() {
        foreach (Transform c in _items.transform) {
            Destroy(c.gameObject);
        }

        var equipment = Inventory.Instance.GetEquippedItems();
        if (equipment.Count > 0)
            gameObject.SetActive (true);

        for (int i = 0; i < equipment.Count; ++i) {
            int index = i;
            Debug.Log("Adding an item " + i);
            var item = equipment[i];
            var card = Instantiate(_card, _items.transform, false);
            var sprite = InventoryView.Instance.GetSprite(item.Name);
            card.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            card.GetComponent<Button>().onClick.AddListener(() => Equip(index, item.Name));
            card.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    private void Equip(int i, string item) {
        UI.Handled = true;

        for (int x = 0; x < _slots.Length; ++x) {
            _slots[x].enabled = (i == x);
            _slots[x].color = new Color(0,1,0,1);

            PlayerController.Instance.UseItem(item);
        }
    }
}

