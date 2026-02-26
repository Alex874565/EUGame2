using UnityEngine;

public class StampBehaviour : MonoBehaviour
{
    [SerializeField] private float cooldownTime = 1f;
    
    private Canvas _canvas;

    private bool _isOnCooldown;
    private float _cooldownTimer;

    private void Awake()
    {
        _canvas = GetComponentInParent<Canvas>();
        _isOnCooldown = false;
    }
    
    public void Update()
    {
        Camera cam = _canvas.worldCamera; // assign your camera-space canvas camera

        Vector3 mouse = Input.mousePosition;

        // IMPORTANT: set Z to distance from camera to the UI plane
        float z = Mathf.Abs(cam.transform.position.z - transform.position.z);
        mouse.z = z;
        
        transform.position = cam.ScreenToWorldPoint(mouse);
        
        if(_isOnCooldown)
        {
            _cooldownTimer += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Lerp(-90, 270, _cooldownTimer / cooldownTime));
            if (_cooldownTimer >= cooldownTime)
            {
                _isOnCooldown = false;
                _cooldownTimer = 0f;
            }
        }
    }

    public void Stamp()
    {
        _isOnCooldown = true;
    }
    
    public bool CanStamp()
    {
        return !_isOnCooldown;
    }
}