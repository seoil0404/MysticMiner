using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelectedItemView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI buttonText;

    private Item selectedItem = null;

    public Item SelectedItem
    {
        set
        {
            button.gameObject.SetActive(true);

            switch (value.Type)
            {
                case Item.ItemType.Consumable:
                    buttonText.text = "Consume";
                    break;
                case Item.ItemType.Equipment:
                    buttonText.text = "Equip";
                    break;
                default:
                    button.gameObject.SetActive(false);
                    break;
            }

            image.color = Color.white;
            image.sprite = value.Sprite;
            nameText.text = value.Name;
            descriptionText.text = value.Description;

            selectedItem = value;
        }
    }

    public void OnSelected()
    {
        if(selectedItem is ConsumableItem consumableItem)
        {
            consumableItem.Use();
            Inventory.DecreaseItem(consumableItem, 1);
        }
        else if(selectedItem is EquipmentItem equipmentItem)
        {
            EquipmentInventory.MainEquipment = equipmentItem;
        }

        InventoryView.Instance.Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        image.color = new Color(1, 1, 1, 0);
        image.sprite = null;
        nameText.text = "";
        descriptionText.text = "";
        button.gameObject.SetActive(false);
    }
}
