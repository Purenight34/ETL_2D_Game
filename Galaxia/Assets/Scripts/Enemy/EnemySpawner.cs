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
    //각 영역의 크기를 카메라 기준으로 정하는 함수.
    private void SetAreaSize()
    {
        Debug.Log("SetAreaSize");

        float cameraY = 2f * Camera.main.orthographicSize;
        float cameraX = cameraY * Camera.main.orthographicSize;
        sizeCamera = new Vector2(cameraX, cameraY);
        sizeSummonArea = 3f * sizeCamera;
        sizeEnemySurvialArea = 5f * sizeCamera;
    }
    //sizeEnemySurvialArea의 영역 안에 있는 Enemy를 탐색
    private void DetectEnemyInSurvialArea()
    {
        Debug.Log("Detect Enemy...");

        Collider2D[] enemies = Physics2D.OverlapBoxAll(transform.position, sizeEnemySurvialArea, 0f, enemyLayer);
        enemyCounter = enemies.Length;

    }

    //Enemy 소환 조건 판단.
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

    public float colliderMovementStep = 1f; // Collider 이동 간격
    public float maxColliderMovementDistance = 10f; // Collider 이동 최대 거리

    //findingAreaSize(스폰지역) 안에서 exceptAreaSize(카메라 구역)만큼의 사이즈를 제외하고, findingColliderSize만큼의 공간(몬스터 스폰 가능 크기)이 비어있는지 찾는다. 
    private IEnumerator FindEmptyArea(Vector2 findingAreaSize, Vector2 exceptAreaSize, Vector2 findingColliderSize)
    {
        //원점 세팅 
        Vector2 FinderColliderPosition;
        Vector2 RandomPosition;

        while (true)
        {
            //랜덤으로 몬스터 스폰장소 고르기.
            while (true)
            {
                //범위 내 랜덤한 위치
                RandomPosition.x = Random.Range(-findingAreaSize.x / 2f, findingAreaSize.x / 2f);
                RandomPosition.y = Random.Range(-findingAreaSize.y / 2f, findingAreaSize.y / 2f);

                //제외 범위 내에 있다면 다시 실행
                if(Mathf.Abs(RandomPosition.x) < exceptAreaSize.x / 2f || Mathf.Abs(RandomPosition.y) < exceptAreaSize.y / 2f)
                {
                    Debug.Log("랜덤 포지션 값 재설정");
                }
                else
                {
                    break;
                }
            }
            FinderColliderPosition.x = transform.position.x + RandomPosition.x;
            FinderColliderPosition.y = transform.position.y + RandomPosition.y;

            //고른 장소가 비어있는지 확인.
            Collider2D checkCollider = Physics2D.OverlapBox(FinderColliderPosition, findingColliderSize, ~enemyLayer);
            if(checkCollider != null)
            {
                Debug.Log("뭐가 있다.");
                yield return 0;
            }
            else
            {
                SummonEnemy(FinderColliderPosition);
            }
            yield return null;
        }
    }

    //isSummon이 참이라면, Enemy를 소환한다.
    private void SummonEnemy(Vector2 summonPosition)
    {
        Debug.Log("Summon!");

        Instantiate(summonedEnemyPrefabs[0], summonPosition, Quaternion.identity);
    }
}
