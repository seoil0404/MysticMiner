using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentInventorySelectedItemView : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private Item item;

    public Item Item
    {
        set
        {
            item = value;
            image.sprite = item.Sprite;
            nameText.text = item.Name;
            descriptionText.text = item.Description;
            Enable();
        }
    }

    private void Awake()
    {
        Refresh();
    }

    public void Refresh()
    {
        Disable();
    }

    public void Disable()
    {
        image.gameObject.SetActive(false);
    }

    public void Enable()
    {
        image.gameObject.SetActive(true);
    }
}
