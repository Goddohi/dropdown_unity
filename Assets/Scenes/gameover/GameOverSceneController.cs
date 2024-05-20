using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverSceneController : MonoBehaviour
{
    public Text lastScoreText;
    public Text highScoreText;

    void Start()
    {
        // "LastScore" ���� ������ Text UI�� ǥ��
        float lastScore = PlayerPrefs.GetFloat("LastScore", 0f);
        lastScoreText.text = "Last Score: " + Mathf.Round(lastScore);

        // "HighScore" ���� ������ Text UI�� ǥ��
        float highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        highScoreText.text = "High Score: " + Mathf.Round(highScore);

        // 4�� �ڿ� ���� ȭ������ ��ȯ
        Invoke("GoToMainMenu", 4f);
    }

    void GoToMainMenu()
    {
        SceneManager.LoadScene("Maintitle"); 
    }
}
