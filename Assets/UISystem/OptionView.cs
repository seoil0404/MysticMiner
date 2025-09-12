using UnityEngine;

public class OptionView : MonoBehaviour
{
    private void Awake()
    {
        Time.timeScale = 0f;
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        Destroy(gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
