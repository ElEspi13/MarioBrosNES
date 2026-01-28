using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rg;
    private Collider2D _col;
    private Vector2 _moveInput;

    [SerializeField] private float _jumpForce;
    [SerializeField] private float _moveForce;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _jumpCutMultiplier = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLenght;
    [SerializeField] private float _acceleration= 10f;
    [SerializeField] private float _deceleration= 10f;
    private bool _isJumping;
    private bool _isRunning;


    private void Awake()
    {
        this._rg = GetComponent<Rigidbody2D>();
        this._col = GetComponent<Collider2D>();
    }

    void Start()
    {
        InputManager.Actions.Player.Jump.performed += HandleJump;
    }

    

    private void Update()
    {
        this._moveInput = InputManager.Actions.Player.Move.ReadValue<Vector2>();

    }

    private void FixedUpdate()
    {
        Move();
        Run();
    }

    private void Run()
    {
        if (InputManager.Actions.Player.Run.IsPressed())
        {
            _maxVelocity = 8f;
        }
        else
        {
            _maxVelocity = 3f;
        }
    }

    private void Move()
    {
        float targetSpeed = _moveInput.x * _maxVelocity;

        float accelRate = (_moveInput.x != 0) ? _acceleration : _deceleration;

        float newVelocityX = Mathf.MoveTowards(_rg.velocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);

        _rg.velocity = new Vector2(newVelocityX, _rg.velocity.y);

        if (_isJumping &&
            !InputManager.Actions.Player.Jump.IsPressed() &&
            _rg.velocity.y > 0)
        {
            _rg.velocity = new Vector2(
                _rg.velocity.x,
                _rg.velocity.y * _jumpCutMultiplier
            );

            _isJumping = false;
        }
    }

    private bool IsGrounded()
    {

        float halfHeight = _col.bounds.extents.y;
        float halfWidth = _col.bounds.extents.x;

        float rayOffset = halfWidth * 0.7f;
        float rayLength = 0.1f + 0.05f;

        Vector2 originCenter = new Vector2(transform.position.x, transform.position.y - halfHeight);
        Vector2 originLeft = new Vector2(transform.position.x - rayOffset, transform.position.y - halfHeight);
        Vector2 originRight = new Vector2(transform.position.x + rayOffset, transform.position.y - halfHeight);

        RaycastHit2D hitCenter = Physics2D.Raycast(originCenter, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(originRight, Vector2.down, rayLength, groundLayer);


        return (hitCenter.collider != null || hitLeft.collider != null || hitRight.collider != null);
    }


    public void HandleJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            if (context.performed)
            {
                _rg.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                _isJumping = true;
            }
        }
        
    }
    
    



}