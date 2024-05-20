using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class R_windController : MonoBehaviour
{
    public float moveSpeed = 5f; // ������ �ӵ�
    public float pushForce = 10f; // �б� ��
    public float followDuration = 1.5f; // ����ٴ� �� ������ �ð�

    public Transform player; // �÷��̾��� Transform
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
            // �÷��̾��� ��ġ�� ���� �̵�
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // ���� �ð� ����ٴ� �� �����ϵ��� ����
            followTimer += Time.deltaTime;
            if (followTimer >= followDuration)
            {
                isFollowingPlayer = false;
            }
        }
    }

    void MoveForward()
    {
        // �÷��̾ ����ٴ� ��, ���� �ð� ������ �״�� ����
        transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // �÷��̾�� ���� ���ϱ�
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDirection = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDirection * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}
