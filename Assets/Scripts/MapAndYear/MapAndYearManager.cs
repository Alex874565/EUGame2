using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MapAndYearManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI yearText;
    [SerializeField] private SpriteRenderer map;
    [SerializeField] private List<Sprite> mapImages;
    
    private int _year;
    
    private void Start()
    {
        _year = GetYear();
        
        if (yearText != null)
        {
            yearText.text = $"{_year}";
        }

        if (map != null)
        {
            if (_year <= 2003)
            {
                map.sprite = mapImages[0];
            }
            else if (_year <= 2007)
            {
                map.sprite = mapImages[1];
            }else if (_year <= 2012)
            {
                map.sprite = mapImages[2];
            }else if (_year <= 2019)
            {
                map.sprite = mapImages[3];
            }
        }
    }

    private int GetYear()
    {
        return ServiceLocator.Instance.GameManager.WaveIndex + 2001;
    }
}