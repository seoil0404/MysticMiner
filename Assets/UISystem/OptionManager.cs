using UnityEngine;

public class OptionManager : MonoBehaviour
{
    [SerializeField] private OptionView optionView;

    private OptionView currentOptionView = null;

    public void OpenOption()
    {
        if (currentOptionView != null) return;

        currentOptionView = Instantiate(optionView);
    }
}
