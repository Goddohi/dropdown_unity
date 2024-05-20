using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverSceneController : MonoBehaviour
{
    public Text lastScoreText;
    public Text highScoreText;

    void Start()
    {
        // "LastScore" 값을 가져와 Text UI에 표시
        float lastScore = PlayerPrefs.GetFloat("LastScore", 0f);
        lastScoreText.text = "Last Score: " + Mathf.Round(lastScore);

        // "HighScore" 값을 가져와 Text UI에 표시
        float highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        highScoreText.text = "High Score: " + Mathf.Round(highScore);

        // 4초 뒤에 메인 화면으로 전환
        Invoke("GoToMainMenu", 4f);
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("Maintitle"); 
    }
}
