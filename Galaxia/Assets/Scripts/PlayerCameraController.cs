using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Tooltip("�Ѿư� ���� ������Ʈ(ĳ����)")]
    [SerializeField]
    private Transform targetTransform;
    private Camera usingCamera; 

    //Todo : isMoving�� �÷��̾�� �������� ������ ��.
    private bool isMoving; 
    private Vector3 previousTargetPosition; // isMoving �������� ����.

    //16:9 ȭ�� ���� ��������, OrthographicSize���� �����ش�.
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
        //ī�޶� �ʱ� ��ġ ����.
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
        ZoomCamera(isMoving); //�̵��� �� �ƿ�
        MoveScreenToCursor();
    }
    private void FixedUpdate()
    {

    }
    private void LateUpdate()
    {
        SetCameraOnPlayer();

        //�̵��Ѵٸ�-> isMoving Player���� �������� ����
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
    //�÷��̾� ���󰡱�
    private void SetCameraOnPlayer()
    {
        Vector3 offset = new Vector3(mousePositionForCameraOffset.x, mousePositionForCameraOffset.y, -10);
        usingCamera.transform.position = targetTransform.position + offset;
    }
    //�÷��̾ �����̰� ������ ���� ȭ����, ������ ������ ���� ȭ���� ������.
    void ZoomCamera(bool isZoomOut)
    {
        if (isZoomOut && usingCamera.orthographicSize < maxCameraSize) //������ �� ȭ�� ���
        {
            usingCamera.orthographicSize = Mathf.Lerp(usingCamera.orthographicSize, maxCameraSize, zoomSpeed * Time.deltaTime);
            if (Mathf.Abs(usingCamera.orthographicSize - maxCameraSize) < 0.05f) // MoveTowards ��� �������� �ʰ�, ��������� �ٷ� �� ����.
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

    //���콺 ��ġ�� ���� ī�޶� ��ġ ��ȭ
    private void MoveScreenToCursor()
    {
        //���콺 ��ġ�� ī�޶��� ��� ������ �ִ��� Ȯ��.
        Vector2 mousePosition = Input.mousePosition;
        float distanceToCenterX = mousePosition.x - (Screen.width / 2f);
        float distanceToCenterY = mousePosition.y - (Screen.height / 2f);
            
        //offset x�� ����
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

        //offset y�� ����
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
