using System;
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
    [Header("Wind")]
    public Text windSpeedText;
    public Text windDirectionText;
    public Transform direction;
    [Header("Sun")]
    public SpiralMeter sunMeter;
    public Text sunriseTimeText;
    public Text sunsetTimeText;
    public Text timeLeftText;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public void SetSunInfo(long sunriseTime, long sunsetTime, long lastUpdated)
    {
        sunriseTimeText.text = Utility.GetTimeFromUNIX(sunriseTime);
        sunsetTimeText.text = Utility.GetTimeFromUNIX(sunsetTime);

        long sunriseTicks = DateTime.Parse(sunriseTimeText.text).Ticks;
        long sunsetTicks = DateTime.Parse(sunsetTimeText.text).Ticks;
        long now = DateTime.Now.Ticks;
        long diff = sunsetTicks - now;
        if (diff > 0)
            timeLeftText.text = string.Format("Time Left\n{0} Hours", new DateTime(diff).ToString("HH:mm"));
        else Debug.LogError("Already Sunset");
        sunMeter.SetMeterValue(now / (float)sunsetTicks * 100);
    }

    public void SetWindInfo(double speed, int deg)
    {
        windSpeedText.text = string.Format("Speed: {0}m/s", speed);
        windDirectionText.text = string.Format("Direction: {0}°", deg % 90);
        foreach (Transform dir in direction) dir.GetComponent<Text>().color = Color.grey;
        direction.GetChild(deg / 90).GetComponent<Text>().color = Color.white;
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
