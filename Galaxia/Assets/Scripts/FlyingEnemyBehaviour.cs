using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyBehaviour : MonoBehaviour
{
    [Tooltip("몬스터 크기 관련 수치")]
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySprite;

    [Header("몬스터 속도 관련")]
    public float accelerationPower = 6.0f;
    public float breakingPower = 12.0f;
    public float maxSpeed = 1.0f;

    [Tooltip("감지 관련 값")]
    private float setDirectionInteval = 0.2f;

    [Tooltip("쫓을 타겟으로 잡을 대상")]
    private Transform targetTransform;
    private string targetTag = "Player";

    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();
    }

    IEnumerator SetDirection()
    {
        while (true)
        {
            GameObject go = GameObject.FindGameObjectWithTag("targetTag");
            if (go != null)
            {
                targetTransform = go.transform;
            }
            else
            {
                Debug.Log("No targetTag...");
            }
            yield return new WaitForSeconds(setDirectionInteval);
        }
    }

}
