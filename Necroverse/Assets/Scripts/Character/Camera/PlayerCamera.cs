using UnityEngine;
using UnityEngine.Serialization;

public class PlayerCamera : MonoBehaviour
{
    /*
    [SerializeField] private Transform _orientation;
    [SerializeField] private Transform _playerCameraPosition;
    [SerializeField] private Transform _camera;
    */
    
    [Header("Cinemachine")]
    
    [SerializeField] private GameObject _cinemachineCameraTarget;
    [SerializeField] private Transform _orientation;
    [SerializeField] private float _sensX;
    [SerializeField] private float _sensY;
    [SerializeField] private float _topClamp = 70.0f;
    [SerializeField] private float _bottomClamp = -30.0f;
    [SerializeField] private float _cameraAngleOverride = 0.0f;
    [SerializeField] private bool _lockCameraPosition = false;
    
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;
    
    private float _mouseX;
    private float _mouseY;
    private float _rotationX;
    private float _rotationY;
    
    private Transform _holder;
   
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        _mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensX;
        _mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * _sensY;

        _cinemachineTargetYaw += _mouseX;
        _cinemachineTargetPitch -= _mouseY;
        
        _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
        _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, _bottomClamp, _topClamp);
        _cinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + _cameraAngleOverride,
            _cinemachineTargetYaw, 0.0f);
        _orientation.rotation =Quaternion.Euler(0.0f, _cinemachineTargetYaw, 0.0f);
    }
    
    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

}
