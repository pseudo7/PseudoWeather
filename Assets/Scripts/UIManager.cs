using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image weatherIcon;
    public Text weatherTitleText;
    public Text descriptionText;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public void SetIcon(Sprite iconSprite)
    {
        weatherIcon.sprite = iconSprite;
    }
    
    public void SetWeatherTitle(string title)
    {
        weatherTitleText.text = title;
    }

    public void SetDescription(string desc)
    {
        descriptionText.text = Utility.GetTitleCase(desc);
    }

}
