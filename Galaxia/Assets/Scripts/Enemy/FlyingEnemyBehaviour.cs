using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyBehaviour : MonoBehaviour
{
    [Tooltip("���� ũ�� ���� ��ġ")]
    private Rigidbody2D enemyRB;
    private SpriteRenderer enemySprite;

    [Header("���� �ӵ� ����")]
    public float accelerationPower = 6.0f;
    public float breakingPower = 12.0f;
    public float maxSpeed = 1.0f;

    [Tooltip("���� ���� ��")]
    private float setDirectionInteval = 0.2f;

    [Tooltip("���� Ÿ������ ���� ���")]
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
