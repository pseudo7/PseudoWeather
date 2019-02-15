using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public Image weatherIcon;
    [Header("Temperature")]
    public Text currentTemp;
    public Text maxTemp;
    public Text minTemp;
    [Header("Description")]
    public Text weatherTitleText;
    public Text descriptionText;
    [Header("Temperature")]
    public Text locationText;
    public Text lastUpdateText;
    [Header("Comfort")]
    public SpiralMeter humidityMeter;
    public LinearSlider pressureSlider;
    public Text humidityText;
    public Text pressureText;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public void SetComfort(float humidity, int pressure)
    {
        humidityMeter.SetMeterValue(humidity);
        pressureSlider.SetSliderValue(pressure);
        humidityText.text = string.Format("Humidity: {0}/100", humidity);
        pressureText.text = string.Format("Pressure: {0} hPa", pressure);
    }

    public void SetLocationAndTime(string location, string time)
    {
        locationText.text = location;
        lastUpdateText.text = string.Format("Last Updated: {0}", time);
    }

    public void SetTemperature(double current, double max, double min)
    {
        currentTemp.text = string.Format("{0}°", current);
        maxTemp.text = string.Format("{0}°/", max);
        minTemp.text = string.Format("{0}°", min);
    }

    public void SetWeatherDetails(Sprite iconSprite, string title, string desc)
    {
        weatherIcon.sprite = iconSprite;
        weatherTitleText.text = title;
        descriptionText.text = Utility.GetTitleCase(desc);
    }
}
