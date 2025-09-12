using TMPro;
using UnityEngine;

public class TimerManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    private readonly float StartTime = 60;
    private float timerTime;

    private void Awake()
    {
        timerTime = StartTime;
    }

    private void Update()
    {
        timerTime -= Time.deltaTime;
        timerText.text = timerTime.ToString();
        if(timerTime < 0 )
        {
            SceneController.Instance.LoadScene(SceneType.EndingScene);
            DestroyImmediate(gameObject);
        }
    }
}
