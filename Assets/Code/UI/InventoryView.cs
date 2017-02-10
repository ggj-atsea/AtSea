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
    [SerializeField] private Sprite _food;
    [SerializeField] private Sprite _water;

     private Dictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

     public Sprite GetSprite(string name)
     {
         var spriteName = name + "Icon";
         Debug.Log("Looking for icon " + spriteName);

         if (_sprites.ContainsKey(spriteName))
             return _sprites[spriteName];
         else
             return null;
     }

    // Populate UI

	void Start()
	{
        var sprites = Resources.LoadAll<Sprite>("icons");
        var names = new string[sprites.Length];
        foreach (var sprite in sprites) {
            Debug.Log("Loading sprite " + sprite.name);
            _sprites[sprite.name] = sprite;
        }

		gameObject.SetActive (false);

	}

    private void PopulatePlayerContents() {
        var items = Inventory.Items;

        if(items == null)
        {
            return;
        }

        foreach (var item in items) {
            var sprite = GetSprite(item.Key.Name);

            var card = Instantiate(_card, _playerContents.transform, false);
            card.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
            card.GetComponent<Button>().onClick.AddListener(() => {
                Inventory.RemoveItem(item.Key);
                ShowPlayerInventory();
            });
            card.transform.GetChild(2).GetComponent<Text>().text = item.Value.ToString();
        }
    }

    public void PopulatePackageContents() {
		foreach(var item in Inventory.ContainerItems)
		{
            var sprite = GetSprite(item.Value.Name);

            var card = Instantiate(_card, _packageContents.transform, false);
			Debug.Log ("Child 3 : " + card.transform.GetChild (0).name);
			card.transform.GetChild (0).GetComponent<Image>().sprite = sprite;
			card.GetComponent<Button> ().onClick.AddListener (() => {
				Inventory.RemoveContainerItem(item.Key);
				ShowPackageInventory();
			});
			card.transform.GetChild (2).GetComponent<Text> ().text = "1";
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

