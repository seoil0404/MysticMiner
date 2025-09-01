using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemView : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemCount;

    private Item item;

    public Item Item
    {
        get => item;
        set
        {
            item = value;
            Sprite = item.Sprite;

            if(item is IStackableItem stackable)
                Count = stackable.Count;
        }
    }

    public Sprite Sprite
    {
        set
        {
            itemImage.sprite = value;
        }
    }

    public int Count
    {
        set
        {
            if(value <= 1)
                itemCount.gameObject.SetActive(false);
            else
            {
                itemCount.gameObject.SetActive(true);
                itemCount.text = value.ToString();
            }
        }
    }

    private void Awake()
    {
        itemCount.gameObject.SetActive(false);
    }

    public void OnSelected()
    {
        InventoryView.Instance.SelectedItemView.SelectedItem = Item;
    }
}
