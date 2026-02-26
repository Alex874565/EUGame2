using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class PetitionBehaviour : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject stamp;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stampedMoveSpeedMultiplier = 2f;
    
    
    public UnityAction OnPetitionSigned;
    
    private Rigidbody2D _rb;
    private Image _image;
    
    private bool _isStamped;
    private float _timeSinceStamped;
    
    private void Awake()
    {
        _image = GetComponent<Image>();
        _rb = GetComponent<Rigidbody2D>();
        _isStamped = false;
    }
    
    public void Update()
    {
        if (_isStamped)
        {
            _timeSinceStamped += Time.deltaTime;
            _image.color = new Color(1f, 1f, 1f, Mathf.Lerp(1f, 0f, _timeSinceStamped / 2f));
            if (_timeSinceStamped > 2f)
            {
                Destroy(gameObject);
            }
        }
    }
    
    public void StartMoving(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * moveSpeed;
    }
    
    public void OnPointerClick(PointerEventData eventData)
    {
        Instantiate(stamp, eventData.position, Quaternion.identity, transform);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.CompareTag("Stamp") && !_isStamped)
        {
            _isStamped = true;
            _rb.linearVelocity *= stampedMoveSpeedMultiplier;
            OnPetitionSigned?.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnPetitionSigned = null;
    }
}