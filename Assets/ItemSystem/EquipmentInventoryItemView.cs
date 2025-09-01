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
        InventoryView.Instance.SelectedItemView.SelectedItem = Item;
    }
}
