using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyBehavior : MonoBehaviour
{
    [Tooltip("���� ���͸� �������� �ʱ� ���� ���� ������ ���̾ �����Ѵ�.")]
    public LayerMask EnemyLayer;

    [Tooltip("���� ũ�� ���� ��ġ")]
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySprite;

    [Header("���� �ӵ� ����")]
    public float jumpPower = 6.0f;
    public float speed = 1.0f;

    [Tooltip("���� ���� ��")]
    private float setDirectionInteval = 1.0f;
    private float detectJumpDistance = 1.5f;
    private float detectMoveXDistance = 0.75f;

    [Tooltip("���� Ÿ������ ���� ���")]
    private Transform targetTransform;
    private string targetTag = "Player";
    private string wallTag = "Wall";
    private bool canMoveX;

    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        StartCoroutine(SetDirection());
    }

    void Update()
    {
        DetectMoveXTest();
        if (canMoveX)
        {
            Move();
        }
        else
        {
            Vector2 velocity = enemyRB.velocity;
            velocity.x = 0;
            enemyRB.velocity = velocity;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(targetTag))
        {
            //Todo : �÷��̾�� �ε����� �� �Ͼ ��.
            Debug.Log("�÷��̾�� �ε���");
        }
    }
    //Ÿ���� ���� ������
    IEnumerator SetDirection()
    {
        while (true)
        {
            GameObject go = GameObject.FindGameObjectWithTag(targetTag);
            if (go != null)
            {
                targetTransform = go.transform;
                DetectFrontObject();
            }
            else
            {
                Debug.Log("No target...");
            }
            yield return new WaitForSeconds(setDirectionInteval);
        }
    }

    //������ �̵�
    private void Move()
    {
        Vector2 velocity = enemyRB.velocity;
        if (targetTransform.position.x > gameObject.transform.position.x)
        {
            enemySprite.flipX = false;
            velocity.x = speed;
        }
        else
        { 
            enemySprite.flipX = true;
            velocity.x = -speed;
        }
        enemyRB.velocity = velocity;
    }

    private void Jump()
    {
        enemyRB.AddForce(jumpPower * Vector2.up, ForceMode2D.Impulse);
    }

    //���� �տ� �ִ� ��ü Ȯ��
    private void DetectFrontObject() 
    {
        //��ü ���⿡ ���� ���� ���� ����
        Vector2 rayDirection = enemySprite.flipX ? Vector2.left : Vector2.right;
        //enemyLayer�� �����ϰ� �����Ѵ�.
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, rayDirection, detectJumpDistance, ~EnemyLayer);
        if (hit.collider != null)
        {
            //���̸� ����
            if (hit.collider.CompareTag(wallTag))
            {
                Jump();
            }
            //�÷��̾�� ����
            if (hit.collider.CompareTag(targetTag))
            {
                //Todo, ������ ����(�ݶ��̴� �浹 �� Attack �� �� 1)
                //Attack();
            }
        }
    }

    //�¿�� ������ �� �ִ� �������� Ȯ���ϴ� �Լ�
    private void DetectMoveXTest()
    {
        Vector2 rayDirection = enemySprite.flipX ? Vector2.left : Vector2.right;
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, rayDirection, detectMoveXDistance, ~EnemyLayer);
        if (hit.collider != null)
        {
            canMoveX = false;
        }
        else
        {
            canMoveX = true;
        }
    }
}
