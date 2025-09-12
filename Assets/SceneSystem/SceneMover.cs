using UnityEngine;

public class SceneMover : MonoBehaviour
{
    [SerializeField] private SceneType sceneType;

    public void LoadScene()
    {
        SceneController.Instance.LoadScene(sceneType);
    }
}
