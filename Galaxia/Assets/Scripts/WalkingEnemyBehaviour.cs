using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemyBehavior : MonoBehaviour
{
    [Tooltip("같은 몬스터를 감지하지 않기 위해 몬스터 별도의 레이어를 설정한다.")]
    public LayerMask EnemyLayer;

    [Tooltip("몬스터 크기 관련 수치")]
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySprite;

    [Header("몬스터 속도 관련")]
    public float jumpPower = 6.0f;
    public float speed = 1.0f;

    [Tooltip("감지 관련 값")]
    private float setDirectionInteval = 1.0f;
    private float detectJumpDistance = 1.5f;
    private float detectMoveXDistance = 0.75f;

    [Tooltip("쫓을 타겟으로 잡을 대상")]
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
            //Todo : 플레이어와 부딪혔을 때 일어날 일.
            Debug.Log("플레이어와 부딪힘");
        }
    }
    //타겟을 향한 움직임
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

    //몬스터의 이동
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

    //몬스터 앞에 있는 물체 확인
    private void DetectFrontObject() 
    {
        //물체 방향에 따른 감지 방향 수정
        Vector2 rayDirection = enemySprite.flipX ? Vector2.left : Vector2.right;
        //enemyLayer을 제외하고 감지한다.
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, rayDirection, detectJumpDistance, ~EnemyLayer);
        if (hit.collider != null)
        {
            //벽이면 점프
            if (hit.collider.CompareTag(wallTag))
            {
                Jump();
            }
            //플레이어면 공격
            if (hit.collider.CompareTag(targetTag))
            {
                //Todo, 몬스터의 공격(콜라이더 충돌 과 Attack 중 택 1)
                //Attack();
            }
        }
    }

    //좌우로 움직일 수 있는 상태인지 확인하는 함수
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
