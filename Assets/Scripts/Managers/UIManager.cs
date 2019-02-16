using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    [Header("Misc")]
    public Transform masterScroll;
    public Image weatherIcon;
    public CanvasGroup canvasGroup;
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

    static ScreenOrientation currentOrientation;

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        currentOrientation = Screen.orientation;
        StartCoroutine(CheckForOrientation());
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public void SetSunInfo(long sunriseTime, long sunsetTime, long lastUpdated)
    {
        sunriseTimeText.text = Utility.GetTimeFromUNIX(sunriseTime);
        sunsetTimeText.text = Utility.GetTimeFromUNIX(sunsetTime);

        long sunriseTicks = DateTime.Parse(sunriseTimeText.text).Ticks;
        long sunsetTicks = DateTime.Parse(sunsetTimeText.text).Ticks;
        long now = DateTime.Now.Ticks;
        long diff = sunriseTicks - now;
        if (diff > 0)
            timeLeftText.text = string.Format("Time Left\n{0} Hours", new DateTime(diff).ToString("hh:mm"));
        else timeLeftText.text = "Already Sunset";
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

    public void ToggleScrollView(bool show)
    {
        StartCoroutine(ToggleScroll(show));
    }

    IEnumerator ToggleScroll(bool show)
    {
        if (show)
        {
            yield return new WaitForEndOfFrame();
            var targetScale = Vector3.one;
            while (masterScroll.localScale != targetScale)
            {
                masterScroll.localScale = Vector3.MoveTowards(masterScroll.localScale, targetScale, .05f);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            var targetScale = new Vector3(1, 0, 1);
            while (masterScroll.localScale != targetScale)
            {
                masterScroll.localScale = Vector3.MoveTowards(masterScroll.localScale, targetScale, .05f);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void RefreshScroll()
    {
        StartCoroutine(Refresh());
    }

    IEnumerator Refresh()
    {
        yield return StartCoroutine(ToggleScroll(false));
        StartCoroutine(ToggleScroll(true));
    }

    IEnumerator CheckForOrientation()
    {
        while (gameObject.activeInHierarchy)
        {
            if (Screen.orientation == ScreenOrientation.Landscape)
                yield return StartCoroutine(ToggleCanvasAlpha(false));
            else
                yield return StartCoroutine(ToggleCanvasAlpha(true));
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator ToggleCanvasAlpha(bool show)
    {
        var targetAplha = show ? 1f : 0f;
        if (show)
        {
            if (currentOrientation != ScreenOrientation.Portrait)
            {
                while (canvasGroup.alpha < targetAplha)
                {
                    canvasGroup.alpha += .075f;
                    yield return new WaitForEndOfFrame();
                }
                canvasGroup.blocksRaycasts = true;
            }
        }
        else
        {
            if (currentOrientation != ScreenOrientation.Landscape)
            {
                while (canvasGroup.alpha > targetAplha)
                {
                    canvasGroup.alpha -= .075f;
                    yield return new WaitForEndOfFrame();
                }
                canvasGroup.blocksRaycasts = false;
            }
        }
        currentOrientation = Screen.orientation;
    }
}
