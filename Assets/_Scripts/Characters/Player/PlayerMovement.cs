using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpHeight;


    [SerializeField] private PlayerController _controller;

    [SerializeField] private bool _isGrounded = true;
    [SerializeField] private bool facingRight = true;

    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private AnimationStateHandler _animatiorHandler;

    [Header("Do not touch")]
    [SerializeField] private float movement;
    void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _playerRb = GetComponent<Rigidbody2D>();
        _animatiorHandler = GetComponent<AnimationStateHandler>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (_controller.IsAlive)
        {
            movement = Input.GetAxis("Horizontal");

            FlipSidesHandler();

            if (Input.GetKey(KeyCode.Space) && _isGrounded)
            {
                PlayerJump();
                _isGrounded = false;
            }

            if (Mathf.Abs(movement) > 0f)
            {
                _animatiorHandler.SetFloat("Run", 1);
            }
            else _animatiorHandler.SetFloat("Run", 0);

            if (Input.GetKey(KeyCode.Z))
            {
                _animatiorHandler.SetTrigger("Attack_1");
                //_animation.PlayAnimation("Player_Attack_1");
            }
        }
    }

    private void FixedUpdate()
    {
        if (_controller.IsAlive)
        {
            transform.position += new Vector3(movement, 0.0f, 0.0f) * Time.fixedDeltaTime * _moveSpeed;
        }
    }

    private void FlipSidesHandler()
    {
        if (movement < 0f && facingRight)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
            facingRight = false;
        }
        else if (movement > 0f && !facingRight)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
            facingRight = true;
        }
    }

    private void PlayerJump()
    {
        _animatiorHandler.SetBool("Jump", true);
        _playerRb.AddForce(new Vector2(0f, _jumpHeight), ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            _animatiorHandler.SetBool("Jump", false);
            _isGrounded = true;
        }
    }
}
