using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CombinedPlayerMonster : MonoBehaviour
{
    public float moveSpeed = 3.5f;
    public float jumpForce = 4f;

    public GameObject monsterPrefab1;
    public GameObject monsterPrefab2;
    public GameObject monsterPrefab3;

    public GameObject ObjectrPrefab1;

    public GameObject ObjectrPrefab2;
    public GameObject monsterPrefab4;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private float startTime;
    private float score;

    public Text scoreText;

    // 몬스터 클론을 저장할 리스트
    private List<GameObject> monsterClones = new List<GameObject>();
    float horizontalInput = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;

        Vector3 lowerLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 upperRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        minX = lowerLeft.x;
        maxX = upperRight.x;
        minY = lowerLeft.y;
        maxY = upperRight.y;

        startTime = Time.time;

        // 처음에 3초 지연 후에 랜덤한 시간 간격으로 몬스터 생성
        InvokeRepeating("SpawnRandomMonster", 3f, Random.Range(0.8f, 3.2f));

        InvokeRepeating("SpawnRandomObject", 7f, Random.Range(2f, 6f));
    }

    void Update()
    {

        //float verticalInput = Input.GetAxis("Vertical");
        Vector2 movement = new Vector2(horizontalInput, 0f);
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);

        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);

        score = Time.time - startTime;
        UpdateScoreText();
    }

    public void rbtn()
    {
        horizontalInput = +0.5f;
        float verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput);
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);

        Jump(Vector2.right);
    }
    public void lbtn()
    {

        horizontalInput = -0.5f;
        Jump(Vector2.left);
    }
    void Jump(Vector2 direction)
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce((Vector2.up + direction) * jumpForce, ForceMode2D.Impulse);
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
        // 게임 오버 처리 및 스코어 저장
        PlayerPrefs.SetFloat("LastScore", score);

        // 최고 점수 갱신
        if (score > PlayerPrefs.GetFloat("HighScore", 0f))
        {
            PlayerPrefs.SetFloat("HighScore", score);
        }


        // PlayerPrefs 변경 사항을 저장
        PlayerPrefs.Save();

        // 클론 삭제 함수 호출
        DestroyMonsterClones();

        // 게임 오버 시 다른 씬으로 전환
        SceneManager.LoadScene("GameOverScene");
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.Round(score);
        }
    }

    //몬스터 생성
    void SpawnRandomMonster()
    {
        // 무작위로 몬스터 프리팹을 선택
        GameObject monsterPrefab = Random.Range(0, 2) == 0 ? monsterPrefab1 : monsterPrefab2;

        // 무작위로 가로 위치 설정
        float randomX = Random.Range(minX, maxX);

        // 몬스터 생성 및 클론 참조 저장
        GameObject newMonster = Instantiate(monsterPrefab, new Vector3(randomX, minY, 0f), Quaternion.identity);
        monsterClones.Add(newMonster);

        // 7초 뒤에 몬스터를 삭제
        Invoke("DestroyMonster", 7f);

        if (score > 15) { SpawnMontserAbs(0); }
        if (score > 40) { SpawnMontserAbs(1); }
        if (score > 50){ SpawnMontserAbs(2); }
        if (score > 70) { SpawnMontserAbs(2); }
        if (score > 100) { SpawnMontserAbs(3); }
        if (score > 150) { SpawnMontserAbs(3); }
    }

    void SpawnMontserAbs(int level)
    {
        if (level == 1)
        {
            float randomspown_mo = Random.Range(0, 2);
            if (randomspown_mo > 0.8f)
            {
                // 무작위로 몬스터 프리팹을 선택
                GameObject monsterPrefab_2 = (Random.Range(0, 3) == 0) ? monsterPrefab1 : ((Random.Range(0, 3) == 1) ? monsterPrefab2 : monsterPrefab3);

                // 무작위로 가로 위치 설정
                float randomX_2 = Random.Range(minX, maxX);

                // 몬스터 생성 및 클론 참조 저장
                GameObject newMonster_2 = Instantiate(monsterPrefab_2, new Vector3(randomX_2, minY, 0f), Quaternion.identity);
                monsterClones.Add(newMonster_2);

                // 7초 뒤에 몬스터를 삭제
                Invoke("DestroyMonster", 7f);

            }
        }
        else if (level == 2)
        {
            float randomspown_mo2 = Random.Range(0, 2);
            if (randomspown_mo2 > 0.8f)
            {
                // 무작위로 몬스터 프리팹을 선택
                GameObject monsterPrefab_3;

                int randomValue = Random.Range(0, 3);
                if (randomValue == 0)
                {
                    monsterPrefab_3 = monsterPrefab1;
                }
                else if (randomValue == 1)
                {
                    monsterPrefab_3 = monsterPrefab2;
                }
                else
                {
                    monsterPrefab_3 = monsterPrefab3;
                }
                // 무작위로 가로 위치 설정
                float randomX_3 = Random.Range(minX, maxX);

                // 몬스터 생성 및 클론 참조 저장
                GameObject newMonster_3 = Instantiate(monsterPrefab_3, new Vector3(randomX_3, minY, 0f), Quaternion.identity);
                monsterClones.Add(newMonster_3);

                // 7초 뒤에 몬스터를 삭제
                Invoke("DestroyMonster", 7f);
            }
        }
        else if (level == 3)
        {
            float randomspown_mo2 = Random.Range(0, 2);
            if (randomspown_mo2 > 1.5f)
            {
                // 무작위로 몬스터 프리팹을 선택
                GameObject monsterPrefab_3;

                int randomValue = Random.Range(0, 3);
                if (randomValue == 0)
                {
                    monsterPrefab_3 = monsterPrefab1;
                }
                else if (randomValue == 1)
                {
                    monsterPrefab_3 = monsterPrefab2;
                }
                else
                {
                    monsterPrefab_3 = monsterPrefab3;
                }
                // 무작위로 가로 위치 설정
                float randomX_3 = Random.Range(minX, maxX);

                // 몬스터 생성 및 클론 참조 저장
                GameObject newMonster_3 = Instantiate(monsterPrefab_3, new Vector3(randomX_3, minY, 0f), Quaternion.identity);
                monsterClones.Add(newMonster_3);

                // 7초 뒤에 몬스터를 삭제
                Invoke("DestroyMonster", 7f);
            }
        }

        else
        {
            // 무작위로 몬스터 프리팹을 선택
            GameObject monsterPrefab = Random.Range(0, 2) == 0 ? monsterPrefab1 : monsterPrefab2;

            // 무작위로 가로 위치 설정
            float randomX = Random.Range(minX, maxX);

            // 몬스터 생성 및 클론 참조 저장
            GameObject newMonster = Instantiate(monsterPrefab, new Vector3(randomX, minY, 0f), Quaternion.identity);
            monsterClones.Add(newMonster);

            // 7초 뒤에 몬스터를 삭제
            Invoke("DestroyMonster", 7f);

        }
        }


    void SpawnObjectAbs(int level)
    {
        if (level == 1)
        {
            float randomspown_Obj = Random.Range(0, 2);
            if (randomspown_Obj > 0.8f)
            {
                // 무작위로 몬스터 프리팹을 선택
                float randomY1 = Random.Range(minY, maxY);

                // 몬스터 생성 및 클론 참조 저장
                GameObject newObjcet1 = Instantiate(ObjectrPrefab1, new Vector3(maxX, randomY1, 0f), Quaternion.identity);
                monsterClones.Add(newObjcet1);

                // 몬스터 이동 방향을 왼쪽으로 설정
                float speed_o1 = 5f; // 이동 속도 조절 필요
                Vector3 direction_o1 = Vector3.left;

                // MonoBehaviour를 상속받은 클래스에서 호출되는 업데이트 함수에서 호출           
                // 몬스터를 왼쪽으로 이동
                newObjcet1.transform.Translate(direction_o1 * speed_o1 * Time.deltaTime);

                // 7초 뒤에 몬스터를 삭제
                Invoke("DestroyMonster", 7f);
            }
        }
        else if (level == 2)
        {
            float randomspown_Obj = Random.Range(0, 2);
            if (randomspown_Obj > 0.8f)
            {
                // 무작위로 몬스터 프리팹을 선택
                float randomY1 = Random.Range(minY, maxY);

                // 몬스터 생성 및 클론 참조 저장
                GameObject newObjcet1 = Instantiate(ObjectrPrefab2, new Vector3(minX, randomY1, 0f), Quaternion.identity);
                monsterClones.Add(newObjcet1);

                // 몬스터 이동 방향을 왼쪽으로 설정
                float speed_o1 = 5f; // 이동 속도 조절 필요
                Vector3 direction_o1 = Vector3.right;

                // MonoBehaviour를 상속받은 클래스에서 호출되는 업데이트 함수에서 호출           
                // 몬스터를 왼쪽으로 이동
                newObjcet1.transform.Translate(direction_o1 * speed_o1 * Time.deltaTime);

                // 7초 뒤에 몬스터를 삭제
                Invoke("DestroyMonster", 7f);
            }
        }
        else if (level == 9)
        {
            // 오브젝트 생성 위치를 무작위로 지정
            float randomY = Random.Range(minY, maxY);

            // 몬스터 생성 및 클론 참조 저장
            GameObject newObjcet = Instantiate(ObjectrPrefab2, new Vector3(minX, randomY, 0f), Quaternion.identity);
            monsterClones.Add(newObjcet);

            // 몬스터 이동 방향을 왼쪽으로 설정
            float speed = 5f; // 이동 속도 조절 필요
            Vector3 direction = Vector3.right;

            // MonoBehaviour를 상속받은 클래스에서 호출되는 업데이트 함수에서 호출           
            // 몬스터를 오른쪽으로 이동
            newObjcet.transform.Translate(direction * speed * Time.deltaTime);

            // 7초 뒤에 몬스터를 삭제
            Invoke("DestroyMonster", 7f);

        }

        else
        {
            // 오브젝트 생성 위치를 무작위로 지정
            float randomY = Random.Range(minY, maxY);

            // 몬스터 생성 및 클론 참조 저장
            GameObject newObjcet = Instantiate(ObjectrPrefab1, new Vector3(maxX, randomY, 0f), Quaternion.identity);
            monsterClones.Add(newObjcet);

            // 몬스터 이동 방향을 왼쪽으로 설정
            float speed = 5f; // 이동 속도 조절 필요
            Vector3 direction = Vector3.left;

            // MonoBehaviour를 상속받은 클래스에서 호출되는 업데이트 함수에서 호출           
            // 몬스터를 왼쪽으로 이동
            newObjcet.transform.Translate(direction * speed * Time.deltaTime);

            // 7초 뒤에 몬스터를 삭제
            Invoke("DestroyMonster", 7f);

        }
    }


    void SpawnRandomObject()
        {
            // 오브젝트 생성 위치를 무작위로 지정
            float randomY = Random.Range(minY, maxY);

            // 몬스터 생성 및 클론 참조 저장
            GameObject newObjcet = Instantiate(ObjectrPrefab1, new Vector3(maxX, randomY, 0f), Quaternion.identity);
            monsterClones.Add(newObjcet);

            // 몬스터 이동 방향을 왼쪽으로 설정
            float speed = 5f; // 이동 속도 조절 필요
            Vector3 direction = Vector3.left;

            // MonoBehaviour를 상속받은 클래스에서 호출되는 업데이트 함수에서 호출           
            // 몬스터를 왼쪽으로 이동
            newObjcet.transform.Translate(direction * speed * Time.deltaTime);

            // 7초 뒤에 몬스터를 삭제
            Invoke("DestroyMonster", 7f);

        if (score > 10)
        {
            SpawnObjectAbs(9);
        }
        if (score > 30)
            {
            SpawnObjectAbs(1);
            }
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

