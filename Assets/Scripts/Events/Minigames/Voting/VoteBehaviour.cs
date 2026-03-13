using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

[RequireComponent(typeof(SelectableObjectSFX))]
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

    [Header("Camera Compensation")]
    [SerializeField] private float referenceOrthographicSize = 5f;

    private int _direction; // -1 for left, 1 for right
    private float _moveSpeed;

    private bool _isFalling;
    private float _fallSpeed;

    private Vector3 _originalScale;

    private SelectableObjectSFX _objectSfx;
    private Camera _uiCamera;

    private void Awake()
    {
        _objectSfx = GetComponent<SelectableObjectSFX>();
        _originalScale = image.transform.localScale;
        _isFalling = false;
        Rb.linearVelocity = Vector2.zero;

        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas != null && parentCanvas.worldCamera != null)
        {
            _uiCamera = parentCanvas.worldCamera;
        }
        else
        {
            _uiCamera = Camera.main;
        }
    }

    private void FixedUpdate()
    {
        ApplyVelocity();
    }

    private float GetCameraSpeedMultiplier()
    {
        if (_uiCamera == null)
            return 1f;

        if (!_uiCamera.orthographic)
            return 1f;

        return _uiCamera.orthographicSize / referenceOrthographicSize;
    }

    private void ApplyVelocity()
    {
        float multiplier = GetCameraSpeedMultiplier();

        if (_isFalling)
        {
            Rb.linearVelocity = _fallSpeed * multiplier * Vector2.down;
        }
        else
        {
            Rb.linearVelocity = _moveSpeed * multiplier * _direction * Vector2.right;
        }
    }

    public void StartMoving(bool spawnedLeft)
    {
        _moveSpeed = Random.Range(moveSpeedRange.x, moveSpeedRange.y);
        _direction = spawnedLeft ? 1 : -1;
        _isFalling = false;
        ApplyVelocity();
    }

    public void StartFalling()
    {
        _isFalling = true;
        _fallSpeed = Random.Range(fallSpeedRange.x, fallSpeedRange.y);
        ApplyVelocity();
    }

    private IEnumerator ClickVote()
    {
        if (!_isFalling)
        {
            _objectSfx.PlaySelectSFX();
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
        if (!_isFalling)
        {
            image.transform.localScale = _originalScale * hoverScaleMultiplier;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isFalling)
        {
            image.transform.localScale = _originalScale;
        }
    }

    #endregion
}