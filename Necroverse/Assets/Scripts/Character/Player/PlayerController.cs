using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IController
{
    //fields
    [SerializeField] private Transform _orientation;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _groundDrag;
    [SerializeField] private EntityModel _playerStats; 
    [SerializeField] private Transform _gunPosition;

    private float _reloadTime;
    private float _movementSpeed;
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private bool _isGrounded;
    private bool _dead;
    private bool _isRealoading;
    private bool _isShooting;
    private ushort _ammoReserve;
    private ushort _magazineSize;
    private Ray _ray;
    private RaycastHit _hit;
    private Camera _camera;
    private Transform _t;
    private Vector3 _destination;
    

    //views
    [SerializeField] private UIView _uiView;
    private IView _animationView;


    //events
    private UnityEvent<bool> _shoot = new UnityEvent<bool>();
    private UnityEvent<HealtInfo> _hurt = new UnityEvent<HealtInfo>();
    private UnityEvent _die = new UnityEvent();
    private UnityEvent _reload = new UnityEvent();
    private UnityEvent _fire = new UnityEvent();


    void Start()
    {
        _magazineSize = 19;
        _ammoReserve = 19;
        _movementSpeed = _playerStats.MovementSpeed;
        _animationView = GetComponent<IView>();
        _rb.freezeRotation = true;
        _uiView.MaxHealth = _playerStats.MaxHealth;
        _camera = Camera.main;
        _t = this.transform;
        
        _hurt.AddListener(_playerStats.OnHurt);
        _shoot.AddListener(_animationView.OnShoot);
        _shoot.AddListener(_uiView.OnShoot);
        _fire.AddListener(_uiView.OnFire);
        _fire.AddListener(OnFire);
        _die.AddListener(_animationView.OnDead);
        _die.AddListener(_uiView.OnDead);
        _reload.AddListener(_uiView.OnReload);
        _reload.AddListener(_animationView.OnReload);
        _reload.AddListener(OnReload);
    }
    
    void Update()
    {
        if (_dead) return;
        GetInput();
        _isGrounded = Physics.Raycast(_t.position, Vector3.down, _playerHeight * .5f + .2f, _groundLayer);
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
        if (Time.time < _reloadTime + 2) return;
        if (Input.GetMouseButtonUp(0) || _ammoReserve == 0)
        {
            _isShooting = false;
            Shoot();
            StopCoroutine(Fire());
        }
        else if (Input.GetMouseButtonDown(0))
        {
            _isShooting = true;
            Shoot();
            StartCoroutine(Fire());
        }
        if(Input.GetKeyDown(KeyCode.R)) Reload();
    }
    
    #region Movement
    private void MovePlayer()
    {
        _moveDirection = _orientation.forward * _verticalInput + _orientation.right * _horizontalInput;
        //_rb.AddForce( , ForceMode.Force);
        _rb.velocity += _moveDirection.normalized * (_movementSpeed * 10);
    }

    private void LimitSpeed()
    {
        var velocity = _rb.velocity;
        Vector3 flatVel = new Vector3(velocity.x, 0f, velocity.z);

        if (flatVel.magnitude > _movementSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * _movementSpeed;
            _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
        }
    }
    #endregion

    #region  Shooting
    private IEnumerator Fire()
    {
        while(_ammoReserve != 0 && _isShooting)
        {
            _fire.Invoke();
            yield return new WaitForSeconds(.1f);
        }
    }
    
    private void OnFire()
    {
        _ray = _camera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(_ray, out _hit)) 
            _destination = _hit.point;
        else
        {
            _destination = _ray.GetPoint(100);
        }
        BulletSpawner.Instance.GetBullet(_gunPosition.position, Quaternion.identity, _playerStats.AttackPower, _destination);
        _ammoReserve--;
    }

    
    private void Shoot()
    {
        _shoot.Invoke(_isShooting);
        _movementSpeed = _isShooting ? _movementSpeed * .5f : _playerStats.MovementSpeed;
    }

    private void Reload()
    {
        _reload.Invoke();
    }
    
    private void OnReload()
    {
        _isShooting = false;
        Shoot();
        _reloadTime = Time.time;
        _ammoReserve = _magazineSize;
    }
    public void Hurt(int damage)
    {
        HealtInfo healtInfo = new HealtInfo();
        healtInfo.damage = damage;
        healtInfo.currentHealth = 10;
        _hurt.Invoke(healtInfo);
    }
    #endregion
    

    public void Dead()
    {
        _dead = true;
    }
}