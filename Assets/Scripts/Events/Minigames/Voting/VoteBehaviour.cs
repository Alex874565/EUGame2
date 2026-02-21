using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(Rigidbody2D), typeof(Image))]
public class VoteBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private Rigidbody2D _rb;
    private Image _image;

    [Header("Visual Settings")]
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    [SerializeField] private float clickScaleMultiplier = 1.4f;
    
    [Header("Movement Settings")]
    [SerializeField] private Vector2 moveSpeedRange = new Vector2(1f, 3f);
    [SerializeField] private Vector2 fallSpeedRange = new Vector2(1f, 3f);
    [SerializeField] private float fallDelay = 0.5f;



    private bool _shouldMove = false;
    private bool _movingRight;
    private float _moveSpeed;

    private bool _isFalling = false;
    private float _fallSpeed;

    private Vector3 _originalScale;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
        _image= GetComponent<Image>();
        _originalScale = _image.transform.localScale;
    }
    
    private void FixedUpdate()
    {
        if (_shouldMove)
        {
            float moveDirection = _movingRight ? 1f : -1f;
            _rb.MovePosition(_rb.position + Vector2.right * moveDirection * _moveSpeed * Time.fixedDeltaTime);
        }
        else
        {
            if (_isFalling)
            {
                _rb.MovePosition(_rb.position + Vector2.down * _fallSpeed * Time.fixedDeltaTime);
            }
        }
    }
    
    public void StartMoving(bool moveRight)
    {
        Debug.Log("StartMoving called");
        _moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        _movingRight = moveRight;
        _shouldMove = true;
        _isFalling = false;
    }
    
    public void StartFalling()
    {
        _fallSpeed = Random.Range(fallSpeedRange.x, fallSpeedRange.y);
        _shouldMove = false;
        _isFalling = true;
    }

    private IEnumerator ClickVote()
    {
        if(!_isFalling)
        {
            StartFalling();
            _image.transform.localScale = _originalScale * clickScaleMultiplier;
            Time.timeScale = 0.2f;
            yield return new WaitForSecondsRealtime(.05f);
            _image.transform.localScale = _originalScale;
            Time.timeScale = 1f;
        }
    }
    
    #region Interaction
    
    public void OnPointerClick(PointerEventData eventData)
    {
        StartCoroutine(ClickVote());
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!_isFalling)
        {
            _originalScale = transform.localScale;
            transform.localScale = _originalScale * hoverScaleMultiplier;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!_isFalling)
        {
            transform.localScale = _originalScale;
        }
    }
    
    #endregion
}