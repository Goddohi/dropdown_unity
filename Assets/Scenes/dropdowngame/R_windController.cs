using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_windController : MonoBehaviour
{
    public float moveSpeed = 5f; // 움직임 속도
    public float pushForce = 10f; // 밀기 힘
    public float followDuration = 1.5f; // 따라다닌 후 직진할 시간

    public Transform player; // 플레이어의 Transform
    private bool isFollowingPlayer = true;
    private float followTimer;

    void Update()
    {
        if (isFollowingPlayer)
        {
            FollowPlayer();
        }
        else
        {
            MoveForward();
        }
    }

    void FollowPlayer()
    {
        if (player != null)
        {
            // 플레이어의 위치를 따라 이동
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // 일정 시간 따라다닌 후 직진하도록 설정
            followTimer += Time.deltaTime;
            if (followTimer >= followDuration)
            {
                isFollowingPlayer = false;
            }
        }
    }

    void MoveForward()
    {
        // 플레이어를 따라다닌 후, 일정 시간 동안은 그대로 직진
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 플레이어에게 힘을 가하기
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDirection = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}
