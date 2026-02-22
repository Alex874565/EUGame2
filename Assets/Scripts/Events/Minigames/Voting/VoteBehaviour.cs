using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class VoteBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [field: SerializeField] public Rigidbody2D Rb { get; private set; }
    [SerializeField] private Image image;

    [Header("Visual Settings")]
    [SerializeField] private float hoverScaleMultiplier = 1.2f;
    [SerializeField] private float clickScaleMultiplier = 1.4f;
    
    [Header("Movement Settings")]
    [SerializeField] private Vector2 moveSpeedRange = new Vector2(1f, 3f);
    [SerializeField] private Vector2 fallSpeedRange = new Vector2(1f, 3f);
    
    private int _direction; // -1 for left, 1 for right
    private float _moveSpeed;
    
    private bool _isFalling;
    private float _fallSpeed;

    private Vector3 _originalScale;

    private void Awake()
    {
        _originalScale = image.transform.localScale;
        _isFalling = false;
        Rb.linearVelocity = Vector2.zero;
    }
    
    public void StartMoving(bool spawnedLeft)
    {
        _moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        _direction = spawnedLeft ? 1 : -1;
        Rb.linearVelocity = _moveSpeed * _direction * Vector2.right;
    }
    
    public void StartFalling()
    {
        _isFalling = true;
        _fallSpeed = Random.Range(fallSpeedRange.x, fallSpeedRange.y);
        Rb.linearVelocity = _fallSpeed * Vector2.down;
    }

    private IEnumerator ClickVote()
    {
        if(!_isFalling)
        {
            StartFalling();
            image.transform.localScale = _originalScale * clickScaleMultiplier;
            Time.timeScale = 0.2f;
            yield return new WaitForSecondsRealtime(.05f);
            image.transform.localScale = _originalScale;
            if (!ServiceLocator.Instance.GameManager.IsPaused)
            {
                Time.timeScale = 1f;
            }
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
            image.transform.localScale = _originalScale * hoverScaleMultiplier;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if(!_isFalling)
        {
            image.transform.localScale = _originalScale;
        }
    }
    
    #endregion
}