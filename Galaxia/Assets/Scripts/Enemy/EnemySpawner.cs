using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

//Todo
public class EnemySpawner : MonoBehaviour
{
    public LayerMask enemyLayer;

    private bool isSummon = false;
    private int enemyCounter;
    [SerializeField]
    private int maxEnemiesCounter = 50;
    public GameObject[] summonedEnemyPrefabs;
    public Vector2 emptyDectorPosition;

    public Vector2 sizeCamera;
    public Vector2 sizeSummonArea;
    public Vector2 sizeEnemySurvialArea;

    private void Start()
    {
        SetAreaSize();
        StartCoroutine(FindEmptyArea(sizeSummonArea, sizeCamera, new Vector2(2f, 2f)));
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            DetectEnemyInSurvialArea();
            CheckSummonCondition();
            if (isSummon)
            {
                SummonEnemy(transform.position);
            }
        }
    }
    //�� ������ ũ�⸦ ī�޶� �������� ���ϴ� �Լ�.
    private void SetAreaSize()
    {
        Debug.Log("SetAreaSize");

        float cameraY = 2f * Camera.main.orthographicSize;
        float cameraX = cameraY * Camera.main.orthographicSize;
        sizeCamera = new Vector2(cameraX, cameraY);
        sizeSummonArea = 3f * sizeCamera;
        sizeEnemySurvialArea = 5f * sizeCamera;
    }
    //sizeEnemySurvialArea�� ���� �ȿ� �ִ� Enemy�� Ž��
    private void DetectEnemyInSurvialArea()
    {
        Debug.Log("Detect Enemy...");

        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position, sizeEnemySurvialArea, 0f, enemyLayer);
        enemyCounter = enemies.Length;

    }

    //Enemy ��ȯ ���� �Ǵ�.
    private void CheckSummonCondition()
    {
        Debug.Log("Check Condition...");

        if (enemyCounter >= maxEnemiesCounter)
        {
            isSummon = false;
        }
        else
        {
            isSummon = true;
        }
    }

    public float colliderMovementStep = 1f; // Collider �̵� ����
    public float maxColliderMovementDistance = 10f; // Collider �̵� �ִ� �Ÿ�

    //findingAreaSize(��������) �ȿ��� exceptAreaSize(ī�޶� ����)��ŭ�� ����� �����ϰ�, findingColliderSize��ŭ�� ����(���� ���� ���� ũ��)�� ����ִ��� ã�´�. 
    private IEnumerator FindEmptyArea(Vector2 findingAreaSize, Vector2 exceptAreaSize, Vector2 findingColliderSize)
    {
        //���� ���� 
        Vector2 FinderColliderPosition;
        Vector2 RandomPosition;

        while (true)
        {
            //�������� ���� ������� ����.
            while (true)
            {
                //���� �� ������ ��ġ
                RandomPosition.x = Random.Range(-findingAreaSize.x / 2f, findingAreaSize.x / 2f);
                RandomPosition.y = Random.Range(-findingAreaSize.y / 2f, findingAreaSize.y / 2f);

                //���� ���� ���� �ִٸ� �ٽ� ����
                if(Mathf.Abs(RandomPosition.x) < exceptAreaSize.x / 2f || Mathf.Abs(RandomPosition.y) < exceptAreaSize.y / 2f)
                {
                    Debug.Log("���� ������ �� �缳��");
                }
                else
                {
                    break;
                }
            }
            FinderColliderPosition.x = transform.position.x + RandomPosition.x;
            FinderColliderPosition.y = transform.position.y + RandomPosition.y;

            //�� ��Ұ� ����ִ��� Ȯ��.
            Collider2D checkCollider = Physics2D.OverlapBox(FinderColliderPosition, findingColliderSize, ~enemyLayer);
            if(checkCollider != null)
            {
                Debug.Log("���� �ִ�.");
                yield return 0;
            }
            else
            {
                SummonEnemy(FinderColliderPosition);
            }
            yield return null;
        }
    }

    //isSummon�� ���̶��, Enemy�� ��ȯ�Ѵ�.
    private void SummonEnemy(Vector2 summonPosition)
    {
        Debug.Log("Summon!");

        Instantiate(summonedEnemyPrefabs[0], summonPosition, Quaternion.identity);
    }
}
