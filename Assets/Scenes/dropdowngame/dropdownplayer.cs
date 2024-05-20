using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class dropdownplayer : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float jumpForce = 4f;
    public GameObject monsterPrefab1;
    public GameObject monsterPrefab2;
    public GameObject monsterPrefab3;
    public GameObject ObjectrPrefab1;
    public GameObject monsterPrefab4;
    public Text scoreText;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private float minX, maxX, minY, maxY;
    private float startTime;
    private float score;
    private List<GameObject> monsterClones = new List<GameObject>();
    private float horizontalInput = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        SetCameraBounds();
        startTime = Time.time;

        // 처음에 3초 지연 후에 랜덤한 시간 간격으로 몬스터 생성
        InvokeRepeating("SpawnRandomMonster", 3f, Random.Range(0.5f, 2.5f));
        InvokeRepeating("SpawnRandomObject", 10f, Random.Range(3f, 10f));
    }

    void Update()
    {
        MovePlayer();
        ClampPlayerPosition();
        UpdateScore();
    }

    void MovePlayer()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        Vector2 movement = new Vector2(horizontalInput, 0f);
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);

        if (Input.GetButtonDown("Jump"))
        {
            Jump(Vector2.right);
        }
    }

    void Jump(Vector2 direction)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce((Vector2.up + direction) * jumpForce, ForceMode2D.Impulse);
    }

    void ClampPlayerPosition()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    void UpdateScore()
    {
        score = Time.time - startTime;
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.Round(score);
        }
    }

    void SetCameraBounds()
    {
        Vector3 lowerLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 upperRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        minX = lowerLeft.x;
        maxX = upperRight.x;
        minY = lowerLeft.y;
        maxY = upperRight.y;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("충돌 감지: " + other.tag);

        // 몬스터와 충돌하면 게임 오버
        if (other.CompareTag("Monster"))
        {
            GameOver();
        }
    }

    void GameOver()
    {
        PlayerPrefs.SetFloat("LastScore", score);

        if (score > PlayerPrefs.GetFloat("HighScore", 0f))
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }

        PlayerPrefs.Save();

        DestroyMonsterClones();

        SceneManager.LoadScene("GameOverScene");
    }

    void SpawnRandomMonster()
    {
        GameObject monsterPrefab = GetRandomMonsterPrefab();
        float randomX = Random.Range(minX, maxX);
        GameObject newMonster = Instantiate(monsterPrefab, new Vector3(randomX, minY, 0f), Quaternion.identity);
        monsterClones.Add(newMonster);

        Invoke("DestroyMonster", 7f);
    }

    GameObject GetRandomMonsterPrefab()
    {
        if (score > 40)
        {
            int randomValue = Random.Range(0, 3);
            return randomValue == 0 ? monsterPrefab1 : (randomValue == 1 ? monsterPrefab2 : monsterPrefab3);
        }
        else if (score > 30)
        {
            return Random.Range(0, 3) == 0 ? monsterPrefab1 : (Random.Range(0, 3) == 1 ? monsterPrefab2 : monsterPrefab3);
        }
        else if (score > 15)
        {
            return Random.Range(0, 2) == 0 ? monsterPrefab1 : monsterPrefab2;
        }
        else
        {
            return Random.Range(0, 2) == 0 ? monsterPrefab1 : monsterPrefab2;
        }
    }

    void SpawnRandomObject()
    {
        float randomY = Random.Range(minY, maxY);
        GameObject newObjcet = Instantiate(ObjectrPrefab1, new Vector3(maxX, randomY, 0f), Quaternion.identity);
        monsterClones.Add(newObjcet);

        float speed = 5f;
        Vector3 direction = Vector3.left;
        newObjcet.transform.Translate(direction * speed * Time.deltaTime);

        Invoke("DestroyMonster", 7f);
    }

    void DestroyMonster()
    {
        if (monsterClones.Count > 0)
        {
            GameObject monsterToDestroy = monsterClones[0];
            monsterClones.RemoveAt(0);
            Destroy(monsterToDestroy);
        }
    }

    void DestroyMonsterClones()
    {
        foreach (var monsterClone in monsterClones)
        {
            if (monsterClone != null)
            {
                Destroy(monsterClone);
            }
        }
        monsterClones.Clear();
    }
}
