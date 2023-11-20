using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(CapsuleCollider2D), typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;

    private Rigidbody2D _rigidBody;

    private CapsuleCollider2D _capsuleCollider;

    private SpriteRenderer _spriteRenderer;

    private Collider2D[] _hitColliders;

    [HideInInspector] public Animator Animator;

    //���� ���� ����
    //================================================================================================================

    [Tooltip("���� �Ŀ�")]
    [SerializeField] private float _jumpHeight;

    [Tooltip("���� ���� Ƚ��")]
    [SerializeField] private int _jumpCount;

    private float _jumpForce;

    private int _currentJumpCount;


    //Ŭ����
    //================================================================================================================

    private PlayerStateMachine _stateMachine;


    void Start()
    {
        Init();
    }

    private void Init()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _capsuleCollider = GetComponent<CapsuleCollider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();

        _stateMachine = new PlayerStateMachine(this);

        _jumpCount = 1;
        _jumpForce = JumpHeightCalculator(_jumpHeight);
        ResetJumpCount();
    }


    private void Update()
    {
        _stateMachine.OnStateUpdate();
    }


    private void FixedUpdate()
    {
        _stateMachine.OnStateFixedUpdate();
    }


    public float Move()
    {
        Debug.Log("������ ����");
        float horizontal = Input.GetAxisRaw("Horizontal");

        if (0 < horizontal)
            _spriteRenderer.flipX = false;
        else if(0 > horizontal)
            _spriteRenderer.flipX = true;

        Vector2 movePos = new Vector3(horizontal * Time.deltaTime * _speed, 0);

        transform.Translate(movePos);

        return horizontal;
    }


    /// <summary>  �÷��̾� �Ʒ��� Ground�� ���� ��� true�� �ƴҰ�� false�� ��ȯ�ϴ� �Լ�  </summary>
    public bool GroundDetection()
    {
        Vector2 point = transform.position + new Vector3(0, -_capsuleCollider.size.y * 0.5f);
        Vector2 size = new Vector2(_capsuleCollider.size.x * 0.5f , _capsuleCollider.size.y * 0.2f);

        _hitColliders = Physics2D.OverlapBoxAll(point, size, 0);

        foreach (Collider2D hit in _hitColliders)
        {
            if (hit.transform != transform)
                return true;
        }

        return false;
    }


    /// <summary> ������ �����ϸ� true�� ��ȯ�ϸ� �����ϰ�, �Ұ����ϸ� false�� ��ȯ�ϴ� �Լ�  </summary>
    public bool JumpStart()
    {
        if (_currentJumpCount <= 0)
            return false;          

        _rigidBody.AddForce(transform.up * _jumpForce, ForceMode2D.Impulse);
        _currentJumpCount--;
        return true;
    }

    public void ResetJumpCount()
    {
        _currentJumpCount = _jumpCount;
    }


    /// <summary>���̸� �޾� �������ӵ��� ��ȯ�ϴ� �Լ�</summary>
    public float JumpHeightCalculator(float height) 
    {
        float jumpTime = Mathf.Sqrt(height / (0.5f * -Physics2D.gravity.y * _rigidBody.gravityScale)); 
        float jumpForce = -Physics2D.gravity.y * _rigidBody.gravityScale * jumpTime;

        return jumpForce; //������ ��ȯ�Ѵ�.
    }



    private void OnDrawGizmos()
    {
        if(TryGetComponent(out _capsuleCollider))
        {
            Gizmos.color = Color.red;
            Vector2 point = transform.position + new Vector3(0, -_capsuleCollider.size.y * 0.5f);
            Vector2 size = new Vector2(_capsuleCollider.size.x * 0.5f, _capsuleCollider.size.y * 0.2f);

            Gizmos.DrawWireCube(point, size);
        }
    }
}
