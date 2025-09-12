using TMPro;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI resultText;

    private void Awake()
    {
        resultText.text = $"You have been taked {Ore.Score} of Ore!";
    }

    public void Resume()
    {
        SceneController.Instance.LoadScene(SceneType.StartScene);
    }
}
