using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _playerCameraPosition;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _player;
    
    private float _mouseX;
    private float _mouseY;
    private float _rotationX;
    private float _rotationY;
    
    private Transform _holder;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _holder = this.transform;
        _holder.position = _playerCameraPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        _mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensX;
        _mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensY;

        _rotationY += _mouseX;
        _rotationX -= _mouseY;
        
        _rotationX = Mathf.Clamp(_rotationX , -20f, 45f);
        _holder.rotation = Quaternion.Euler(_rotationX, _rotationY, 0);
        _player.rotation = Quaternion.Euler(0, _rotationY, 0);
        _orientation.rotation = Quaternion.Euler(0, _rotationY, 0);
        _holder.position = _playerCameraPosition.position;
    }
}
