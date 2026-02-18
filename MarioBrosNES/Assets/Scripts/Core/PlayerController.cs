using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.UI.Image;

[RequireComponent(typeof(Rigidbody2D))]
/// <summary>
/// Controla el movimiento del jugador: desplazamiento, carrera y salto.
/// </summary>
public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rg;
    private Collider2D _col;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _moveInput;


    [SerializeField] private Animator _animator;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _moveForce;
    [SerializeField] private float _maxVelocity;
    [SerializeField] private float _jumpCutMultiplier = 0.4f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayLenght;
    [SerializeField] private float _acceleration= 10f;
    [SerializeField] private float _deceleration= 10f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float lowJumpMultiplier = 2f;

    private bool _isRunning;
    private bool _jumpStarted;
    private bool _wasGrounded;


    /// <summary>
    /// Obtiene referencias a componentes necesarios (Rigidbody2D y Collider2D).
    /// </summary>
    private void Awake()
    {
        this._rg = GetComponent<Rigidbody2D>();
        this._col = GetComponent<Collider2D>();
    }

    /// <summary>
    /// Se suscribe al evento de salto del sistema de entrada en la inicialización.
    /// </summary>
    void Start()
    {
        InputManager.Actions.Player.Jump.performed += HandleJump;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    

    /// <summary>
    /// Lee la entrada de movimiento cada frame y la almacena para usarla en físicas.
    /// </summary>
    private void Update()
    {
        this._moveInput = InputManager.Actions.Player.Move.ReadValue<Vector2>();

        bool grounded = IsGrounded();
        // Detectamos el momento de despegar
        if (_jumpStarted && !grounded)
        {
            _animator.SetBool("isJumping", true);
        }

        // Detectamos aterrizaje REAL (venimos del aire)
        if (_jumpStarted && grounded && !_wasGrounded)
        {
            _jumpStarted = false;
            _animator.SetBool("isJumping", false);
        }

        _wasGrounded = grounded;
    }

    /// <summary>
    /// Ejecuta la lógica de movimiento y carrera en el bucle de físicas.
    /// </summary>
    private void FixedUpdate()
    {
        Move();
        Run();
        // Mejorar gravedad estilo Mario
        if (_rg.velocity.y < 0)
        {
            _rg.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (_rg.velocity.y > 0 && !InputManager.Actions.Player.Jump.IsPressed())
        {
            _rg.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }


        if (_moveInput.x > 0.1f)
           _spriteRenderer.flipX = false;
        else if (_moveInput.x < -0.1f)
           _spriteRenderer.flipX = true;
    }

    /// <summary>
    /// Ajusta la velocidad máxima según si el jugador está manteniendo la tecla de correr.
    /// </summary>
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

    /// <summary>
    /// Calcula y aplica la velocidad horizontal del jugador, y recorta el salto si se suelta la tecla.
    /// </summary>
    private void Move()
    {
        float targetSpeed = _moveInput.x * _maxVelocity;

        float accelRate = (_moveInput.x != 0) ? _acceleration : _deceleration;

        float newVelocityX = Mathf.MoveTowards(_rg.velocity.x, targetSpeed, accelRate * Time.fixedDeltaTime);

        _rg.velocity = new Vector2(newVelocityX, _rg.velocity.y);

        if (!InputManager.Actions.Player.Jump.IsPressed() && _rg.velocity.y > 0)
        {
            _rg.velocity = new Vector2(
                _rg.velocity.x,
                _rg.velocity.y * _jumpCutMultiplier
            );
            
        }
        AnimatorRun();
        AnimatorBrake();

    }

    private void AnimatorRun()
    {
        _animator.SetFloat("xVelocity", Math.Abs(_rg.velocity.x));
    }
    private void AnimatorBrake()
    {
        float velocityX = _rg.velocity.x;
        float inputX = _moveInput.x;

        bool braking =
            Mathf.Abs(velocityX) >= 0f &&
            Mathf.Sign(velocityX) != Mathf.Sign(inputX) &&
            inputX != 0;

        _animator.SetBool("isBraking", braking);
    }


    /// <summary>
    /// Realiza raycasts desde la base del collider para determinar si el jugador está en el suelo.
    /// </summary>
    private bool IsGrounded()
    {
        float halfHeight = _col.bounds.extents.y;
        float halfWidth = _col.bounds.extents.x;

        float rayOffset = halfWidth * 0.7f;
        float rayLength = 0.1f;

        Vector2 originCenter = new Vector2(transform.position.x, transform.position.y - halfHeight);
        Vector2 originLeft = new Vector2(transform.position.x - rayOffset, transform.position.y - halfHeight);
        Vector2 originRight = new Vector2(transform.position.x + rayOffset, transform.position.y - halfHeight);

        RaycastHit2D hitCenter = Physics2D.Raycast(originCenter, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(originLeft, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(originRight, Vector2.down, rayLength, groundLayer);


        return (hitCenter.collider != null || hitLeft.collider != null || hitRight.collider != null);
    }


    /// <summary>
    /// Maneja la entrada de salto; aplica fuerza si el jugador está en el suelo.
    /// </summary>
    /// <param name="context">Contexto del evento de entrada</param>
    public void HandleJump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            if (context.performed)
            {
                _rg.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
                _jumpStarted= true;
            }
        }
        
    }
    
    



}