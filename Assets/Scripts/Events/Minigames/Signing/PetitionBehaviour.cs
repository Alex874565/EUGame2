using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D), typeof(SelectableObjectSFX))]
public class PetitionBehaviour : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private GameObject sign;
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float stampedMoveSpeedMultiplier = 2f;

    public StampBehaviour Stamp { get; set; }
    
    public UnityAction OnPetitionSigned;
    
    private Canvas _canvas;
    private Rigidbody2D _rb;
    private Image _image;
    private Collider2D _collider;
    
    private bool _isStamped;
    private float _timeSinceStamped;
    
    private SelectableObjectSFX _objectSfx;
    
    private void Awake()
    {
        _objectSfx = GetComponent<SelectableObjectSFX>();
        _image = GetComponent<Image>();
        _rb = GetComponent<Rigidbody2D>();
        _canvas = GetComponentInParent<Canvas>();
        _collider = GetComponent<Collider2D>();
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
        if (_isStamped || !Stamp.CanStamp()) return;

        Stamp.Stamp();
        
        RectTransform parentRT = (RectTransform)transform;   // the UI object you clicked on

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRT,
                eventData.position,
                _canvas.worldCamera, // null if overlay canvas
                out var localPoint))
        {
            var go = Instantiate(sign, parentRT);
            var rt = (RectTransform)go.transform;

            rt.anchoredPosition = localPoint;
            rt.localRotation = Quaternion.identity;
            rt.localScale = Vector3.one;
            rt.SetAsLastSibling();
            Physics2D.SyncTransforms();
            if (_collider.OverlapPoint(go.transform.position))
            {
                if (!_isStamped)
                {
                    _objectSfx.PlaySelectSFX();
                    StartCoroutine(StampCoroutine());
                }
            }
            else
            {
                _objectSfx.PlayAppearSFX();
            }
        }
    }

    private IEnumerator StampCoroutine()
    {
        _isStamped = true;
        Time.timeScale = .5f;
        yield return new WaitForSecondsRealtime(.1f);
        Time.timeScale = 1f;
        _rb.linearVelocity *= stampedMoveSpeedMultiplier;
        OnPetitionSigned?.Invoke();
    }

    private void OnDestroy()
    {
        OnPetitionSigned = null;
    }
}