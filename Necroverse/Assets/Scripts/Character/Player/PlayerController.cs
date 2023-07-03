using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    //fields
    [SerializeField] private int _movementSpeed;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _playerHeight;
    [SerializeField] private float _groundDrag;
    
    private float _horizontalInput;
    private float _verticalInput;
    private Vector3 _moveDirection;
    private bool _isGrounded;

    //events
    private UnityEvent _shoot = new UnityEvent();
    private UnityEvent<HealtInfo> _hurt = new UnityEvent<HealtInfo>();
    private UnityEvent _die = new UnityEvent();
    void Start()
    {
        _rb.freezeRotation = true;
    }
    
    void Update()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * .5f + .2f, _groundLayer);
        GetInput();
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
        if (Input.GetMouseButtonDown(0)) Shoot();
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

    private void Shoot()
    {
        _shoot.Invoke();    
    }

    private void Hurt(int damage)
    {
        HealtInfo healtInfo = new HealtInfo();
        healtInfo.damage = damage;
        healtInfo.currentHealth = 10;
        _hurt.Invoke(healtInfo);
    }
}
