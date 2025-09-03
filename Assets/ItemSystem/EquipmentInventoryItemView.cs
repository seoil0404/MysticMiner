using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventoryItemView : MonoBehaviour
{
    [SerializeField] private Image itemImage;

    private Item item;

    public Item Item
    {
        get => item;
        set
        {
            if (value == null) return;
            item = value;
            Sprite = item.Sprite;
        }
    }

    public Sprite Sprite
    {
        set
        {
            itemImage.sprite = value;
        }
    }

    public void OnSelected()
    {
        if (Item == null) return;
        EquipmentInventoryView.Instance.SelectedItemView.SelectedItem = Item;
    }
}
