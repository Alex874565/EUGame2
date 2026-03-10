using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Collider2D), typeof(Image), typeof(SelectableObjectSFX))]
public class UrnBehaviour : MonoBehaviour
{
    public VotingMinigameController Minigame { get; set; }
    
    [Header("Visual Settings")]
    [SerializeField] private float voteRegisterMultiplier = 1.2f;
    
    private Image _urnImage;
    private Vector3 _originalScale;
    
    private SelectableObjectSFX _objectSfx;
    
    private void Start()
    {
        _objectSfx = GetComponent<SelectableObjectSFX>();
        _urnImage = GetComponent<Image>();
        _originalScale = _urnImage.transform.localScale;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<VoteBehaviour>() != null)
        {
            _objectSfx.PlaySelectSFX();
            StartCoroutine(RegisterVote(other.gameObject));
        }
    }
    
    private IEnumerator RegisterVote(GameObject vote)
    {
        _urnImage.transform.localScale = _originalScale * voteRegisterMultiplier;
        Minigame.RegisterVote(vote);
        yield return new WaitForSeconds(0.2f);
        _urnImage.transform.localScale = _originalScale;
    }
}