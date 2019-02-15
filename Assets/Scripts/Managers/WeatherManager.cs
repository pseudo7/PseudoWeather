using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    public Transform mainLight;

    static bool gettingWeather;

    void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    void Start()
    {
        StartCoroutine(SunRotation());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    IEnumerator SunRotation()
    {
        string[] hoursMin = DateTime.Now.ToString("HH:mm").Split(':');

        int hours = int.Parse(hoursMin[0]), mins = int.Parse(hoursMin[1]);
        const float DEG_PER_MINUTE = 360f / (24 * 60);
        var degToRotate = 270 + (hours * 60 + mins) * DEG_PER_MINUTE;

        while (gameObject.activeInHierarchy)
        {
            mainLight.rotation = Quaternion.Euler(degToRotate, 0, 0);
            yield return new WaitForSeconds(60);
        }
    }

    public void GetWeather(string cityName)
    {
        if (!Utility.CityNames.Contains(cityName))
        {
            Debug.LogError("Incorrect Name");
            return;
        }
        if (Application.internetReachability != NetworkReachability.NotReachable)
            if (!gettingWeather)
                StartCoroutine(StartWeatherCoroutine(Utility.GetURL(cityName, false)));
            else Debug.LogError("Patience");
        else Debug.LogError("NO INTERNET");
    }

    IEnumerator StartWeatherCoroutine(string requestURL)
    {
        gettingWeather = true;
        Debug.Log("URL: " + requestURL);
        using (UnityWebRequest weatherRequest = UnityWebRequest.Get(requestURL))
        {
            yield return weatherRequest.SendWebRequest();
            Serializables.WeatherMain weatherMainData = JsonUtility.FromJson<Serializables.WeatherMain>(weatherRequest.downloadHandler.text);
            Debug.Log(weatherMainData);
            using (UnityWebRequest iconRequest = UnityWebRequestTexture.GetTexture(Constants.WEATHER_ICON_URL + weatherMainData.weather[0].icon + ".png"))
            {
                yield return iconRequest.SendWebRequest();
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(iconRequest.downloadHandler.data);
                Sprite icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));

                UIManager.Instance.SetSunInfo(weatherMainData.sys.sunrise, weatherMainData.sys.sunset, weatherMainData.dt);
                UIManager.Instance.SetWindInfo(weatherMainData.wind.speed, weatherMainData.wind.deg);
                UIManager.Instance.SetComfort(weatherMainData.main.humidity, weatherMainData.main.pressure);
                UIManager.Instance.SetLocationAndTime(weatherMainData.name, Utility.GetTimeFromUNIX(weatherMainData.dt));
                UIManager.Instance.SetTemperature(weatherMainData.main.temp, weatherMainData.main.temp_max, weatherMainData.main.temp_min);
                UIManager.Instance.SetWeatherDetails(icon, weatherMainData.weather[0].main, weatherMainData.weather[0].description);
            }
        }
        UIManager.Instance.ToggleScrollView(true);
        gettingWeather = false;
    }
}