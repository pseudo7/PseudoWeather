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
    public Button switchButton;
    public GameObject warningPanel;
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
    }

    public void SetSunInfo(long sunriseTime, long sunsetTime, long lastUpdated)
    {
        sunriseTimeText.text = Utility.GetTimeFromUNIX(sunriseTime);
        sunsetTimeText.text = Utility.GetTimeFromUNIX(sunsetTime);

        long sunriseTicks = DateTime.Parse(sunriseTimeText.text).Ticks;
        long sunsetTicks = DateTime.Parse(sunsetTimeText.text).Ticks;
        long nowTicks = DateTime.Now.Ticks;

        int sunriseMinutes = DateTime.Parse(sunriseTimeText.text).Minute;
        int sunsetMinutes = DateTime.Parse(sunsetTimeText.text).Minute;
        int nowMinutes = DateTime.Now.Minute;

        if (nowTicks > sunsetTicks)
        {
            timeLeftText.text = string.Format("Sunrise In\n{0} Hours", new DateTime(sunriseTicks - (nowTicks - new TimeSpan(24, 0, 0).Ticks)).ToString("HH:mm"));
            sunMeter.SetMeterValue(100);
        }
        else if (nowTicks < sunsetTicks)
        {
            timeLeftText.text = string.Format("Sunset In\n{0} Hours", new DateTime(sunsetTicks - nowTicks).ToString("HH:mm"));
            sunMeter.SetMeterValue((nowTicks - sunriseTicks) / (float)(sunsetTicks - sunriseTicks) * 100);
        }
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
        currentTemp.text = string.Format("{0}°{1}", current, Utility.TempSymbol);
        maxTemp.text = string.Format("{0}°{1}/", max, Utility.TempSymbol);
        minTemp.text = string.Format("{0}°{1}", min, Utility.TempSymbol);
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

    public void ShowWarning()
    {
        warningPanel.ScaleGameObject(true);
    }

    public void HideWarning()
    {
        warningPanel.ScaleGameObject(false);
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
            switchButton.interactable = true;
        }
        else
        {
            var targetScale = new Vector3(1, 0, 1);
            while (masterScroll.localScale != targetScale)
            {
                masterScroll.localScale = Vector3.MoveTowards(masterScroll.localScale, targetScale, .05f);
                yield return new WaitForEndOfFrame();
            }
            switchButton.interactable = false;
        }
    }

    public void ScaleGO(GameObject gameObject, bool show)
    {
        StartCoroutine(Toggle(gameObject, show));
    }

    IEnumerator Toggle(GameObject gameObject, bool show)
    {
        if (show)
        {
            yield return new WaitForEndOfFrame();
            var targetScale = Vector3.one;
            while (gameObject.transform.localScale != targetScale)
            {
                gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, targetScale, .1f);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            var targetScale = Vector3.zero;
            while (gameObject.transform.localScale != targetScale)
            {
                gameObject.transform.localScale = Vector3.MoveTowards(gameObject.transform.localScale, targetScale, .1f);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void SwitchCF()
    {
        StartCoroutine(Switch());
    }

    IEnumerator Switch()
    {
        yield return StartCoroutine(ToggleScroll(false));
        Utility.IsFahrenheit = !Utility.IsFahrenheit;
        WeatherManager.Instance.GetWeather();
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
                FindObjectOfType<GyroCamera>().ResetCamera();
            }
        }
        currentOrientation = Screen.orientation;
    }
}
