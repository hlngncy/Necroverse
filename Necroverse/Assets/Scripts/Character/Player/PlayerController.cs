using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IController
{
    //fields
    [SerializeField] private Transform _orientation;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _groundDrag;
    [SerializeField] private EntityModel _playerStats;
    [SerializeField] private Animator _animator;

    private float _movementSpeed;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private bool _isGrounded;
    private bool _dead;

    //views
    [SerializeField] private UIView _uiView;
    private IView _animationView;


    //events
    private UnityEvent<bool> _shoot = new UnityEvent<bool>();
    private UnityEvent<HealtInfo> _hurt = new UnityEvent<HealtInfo>();
    private UnityEvent _die = new UnityEvent();
    private UnityEvent _reload = new UnityEvent();
    

    void Start()
    {
        _movementSpeed = _playerStats.MovementSpeed;
        _animationView = GetComponent<IView>();
        _rb.freezeRotation = true;
        _hurt.AddListener(_playerStats.OnHurt);
        _shoot.AddListener(_animationView.OnShoot);
        _shoot.AddListener(_uiView.OnShoot);
        _die.AddListener(_animationView.OnDead);
        _die.AddListener(_uiView.OnDead);
    }
    
    void Update()
    {
        if (_dead) return;
        GetInput();
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * .5f + .2f, _groundLayer);
        if (_isGrounded) _rb.drag = _groundDrag;
        else _rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        LimitSpeed();
    }
    

    // Update is called once per frame
    private void GetInput()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetMouseButtonDown(0)) Shoot(true);
        if (Input.GetMouseButtonUp(0)) Shoot(false);
    }
    
    #region Movement
    private void MovePlayer()
    {
        _moveDirection = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        _rb.AddForce(_moveDirection.normalized * (_movementSpeed * 10) , ForceMode.Force);
        
    }

    private void LimitSpeed()
    {
        Vector3 flatVel = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

        if (flatVel.magnitude > _movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _movementSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }
    #endregion

    private void Shoot(bool isShooting)
    {
        _shoot.Invoke(isShooting);
        _movementSpeed = isShooting ? _movementSpeed * .5f : _playerStats.MovementSpeed;
    }

    public void Hurt(int damage)
    {
        HealtInfo healtInfo = new HealtInfo();
        healtInfo.damage = damage;
        healtInfo.currentHealth = 10;
        _hurt.Invoke(healtInfo);
    }
    
    

    public void Dead()
    {
        _dead = true;
    }
}