using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private Image tutorialImageView;
    [SerializeField] private Sprite[] tutorialImages;

    private int index = 0;

    private void Awake()
    {
        Next();
    }

    public void Next()
    {
        if(index >= tutorialImages.Length)
        {
            OnEndTutorial();
            return;
        }

        tutorialImageView.sprite = tutorialImages[index];
        index++;
    }

    public void OnEndTutorial()
    {
        SceneController.Instance.LoadScene(SceneType.StartScene);
    }
}
