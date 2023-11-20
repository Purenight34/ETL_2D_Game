using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Tooltip("쫓아갈 게임 오브젝트(캐릭터)")]
    [SerializeField]
    private Transform targetTransform;
    private Camera usingCamera; 

    //Todo : isMoving을 플레이어에서 가져오면 수정할 것.
    private bool isMoving; 
    private Vector3 previousTargetPosition; // isMoving 가져오면 삭제.

    //16:9 화면 비율 기준으로, OrthographicSize값을 정해준다.
    private float minCameraSize = 4.5f;
    private float maxCameraSize = 9.0f;
    private float zoomSpeed = 3.0f;

    private Vector2 mousePositionForCameraOffset;
    private float mapMaxSizeX;
    private float mapMaxSizeY;

    private void Awake()
    {
        usingCamera = GetComponent<Camera>();
    }

    void Start()
    {
        //카메라 초기 위치 설정.
        if (targetTransform != null)
        {
            Vector3 initalCamPos = new Vector3(targetTransform.position.x, targetTransform.position.y, transform.position.z);
            gameObject.transform.position = initalCamPos;
        }
        else
        {
            Debug.Log("No Target");
        }
    }

    void Update()
    {
        ZoomCamera(isMoving); //이동시 줌 아웃
        MoveScreenToCursor();
    }
    private void FixedUpdate()
    {

    }
    private void LateUpdate()
    {
        SetCameraOnPlayer();

        //이동한다면-> isMoving Player에서 가져오면 삭제
        if (previousTargetPosition != targetTransform.position)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        previousTargetPosition = targetTransform.position;

    }
    //플레이어 따라가기
    private void SetCameraOnPlayer()
    {
        Vector3 offset = new Vector3(mousePositionForCameraOffset.x, mousePositionForCameraOffset.y, -10);
        usingCamera.transform.position = targetTransform.position + offset;
    }
    //플레이어가 움직이고 있으면 넓은 화면을, 가만히 있으면 작은 화면을 보여줌.
    void ZoomCamera(bool isZoomOut)
    {
        if (isZoomOut && usingCamera.orthographicSize < maxCameraSize) //움직일 때 화면 축소
        {
            usingCamera.orthographicSize = Mathf.Lerp(usingCamera.orthographicSize, maxCameraSize, zoomSpeed * Time.deltaTime);
            if (Mathf.Abs(usingCamera.orthographicSize - maxCameraSize) < 0.05f) // MoveTowards 계속 실행하지 않게, 가까워지면 바로 값 대입.
            {
                usingCamera.orthographicSize = maxCameraSize;
            }
        }
        else if (!isZoomOut && usingCamera.orthographicSize > minCameraSize)
        {
            usingCamera.orthographicSize = Mathf.Lerp(usingCamera.orthographicSize, minCameraSize, zoomSpeed * Time.deltaTime);
            if (Mathf.Abs(usingCamera.orthographicSize - minCameraSize) < 0.05f)
            {
                usingCamera.orthographicSize = minCameraSize;
            }
        }
    }

    //마우스 위치에 따른 카메라 위치 변화
    private void MoveScreenToCursor()
    {
        //마우스 위치를 카메라의 어느 비율에 있는지 확인.
        Vector2 mousePosition = Input.mousePosition;
        float distanceToCenterX = mousePosition.x - (Screen.width / 2f);
        float distanceToCenterY = mousePosition.y - (Screen.height / 2f);
            
        //offset x값 설정
        if (Mathf.Abs(distanceToCenterX) > Screen.width / 4f) 
        {
            if(distanceToCenterX > 0)
            {
                mousePositionForCameraOffset.x = ((mousePosition.x / Screen.width) - (3f / 4f)) * usingCamera.orthographicSize * usingCamera.aspect;
            }
            else
            {
                mousePositionForCameraOffset.x = -((1f / 4f) - (mousePosition.x / Screen.width)) * usingCamera.orthographicSize * usingCamera.aspect;
            }
        }
        else
        {
            mousePositionForCameraOffset.x = 0;
        }

        //offset y값 설정
        if (Mathf.Abs(distanceToCenterY) > Screen.height / 4f)
        {
            if(distanceToCenterY > 0)
            {
                mousePositionForCameraOffset.y = ((mousePosition.y / Screen.height) - (3f / 4f)) * usingCamera.orthographicSize;
            }
            else
            {
                mousePositionForCameraOffset.y = -((1f / 4f) - (mousePosition.y / Screen.height)) * usingCamera.orthographicSize;
            }
        }
        else
        {
            mousePositionForCameraOffset.y = 0;
        }
    }
}
