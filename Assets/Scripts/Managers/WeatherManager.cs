using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    public Transform mainLight;
    public Skybox skybox;
    public Material nightSkyMat;
    public Material daySkyMat;

    static bool gettingWeather;
    static string lastCitySearched = "Chandigarh";

    void Awake()
    {
        if (!Instance)
            Instance = this;
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.ExponentialSquared;
        Handheld.SetActivityIndicatorStyle(AndroidActivityIndicatorStyle.Small);
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
        degToRotate %= 360;

        if ((degToRotate >= 210 || degToRotate <= -30))
            SetNightSky(true);
        else SetNightSky(false);

        while (gameObject.activeInHierarchy)
        {
            mainLight.rotation = Quaternion.Euler(degToRotate, 0, 0);
            yield return new WaitForSeconds(300);
        }
    }

    public void GetWeather(string cityName)
    {
        if (!Utility.CityNames.Contains(cityName))
        {
            Debug.LogError("Incorrect Name");
            return;
        }
        else lastCitySearched = cityName;
        if (Application.internetReachability != NetworkReachability.NotReachable)
            if (!gettingWeather)
                StartCoroutine(StartWeatherCoroutine(Utility.GetURL(cityName)));
            else Debug.LogError("Patience");
        else UIManager.Instance.ShowWarning();
    }

    public void GetWeather()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable)
            if (!gettingWeather)
                StartCoroutine(StartWeatherCoroutine(Utility.GetURL(lastCitySearched)));
            else Debug.LogError("Patience");
        else UIManager.Instance.ShowWarning();
    }

    public void SetNightSky(bool enable)
    {
        skybox.material = enable ? nightSkyMat : daySkyMat;
    }

    IEnumerator StartWeatherCoroutine(string requestURL)
    {
        gettingWeather = true;
        Handheld.StartActivityIndicator();

        using (UnityWebRequest weatherRequest = UnityWebRequest.Get(requestURL))
        {
            yield return weatherRequest.SendWebRequest();

            if (weatherRequest.isNetworkError)
            {
                Debug.LogError(weatherRequest.error);
                Handheld.StopActivityIndicator();
                gettingWeather = false;
                yield break;
            }

            Serializables.WeatherMain weatherMainData = JsonUtility.FromJson<Serializables.WeatherMain>(weatherRequest.downloadHandler.text);
            Debug.Log(weatherMainData);
            using (UnityWebRequest iconRequest = UnityWebRequestTexture.GetTexture(Constants.WEATHER_ICON_URL + weatherMainData.weather[0].icon + ".png"))
            {
                yield return iconRequest.SendWebRequest();
                Texture2D texture = new Texture2D(0, 0);
                texture.LoadImage(iconRequest.downloadHandler.data);
                Sprite icon = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(.5f, .5f));

                UpdateEnvironment(GetEnvironmentType(weatherMainData.weather[0].id));

                UIManager.Instance.SetSunInfo(weatherMainData.sys.sunrise, weatherMainData.sys.sunset, weatherMainData.dt);
                UIManager.Instance.SetWindInfo(weatherMainData.wind.speed, weatherMainData.wind.deg);
                UIManager.Instance.SetComfort(weatherMainData.main.humidity, weatherMainData.main.pressure);
                UIManager.Instance.SetLocationAndTime(weatherMainData.name, Utility.GetTimeFromUNIX(weatherMainData.dt));
                UIManager.Instance.SetTemperature(weatherMainData.main.temp, weatherMainData.main.temp_max, weatherMainData.main.temp_min);
                UIManager.Instance.SetWeatherDetails(icon, weatherMainData.weather[0].main, weatherMainData.weather[0].description);
            }
        }
        Handheld.StopActivityIndicator();
        UIManager.Instance.ToggleScrollView(true);
        gettingWeather = false;
    }

    void UpdateEnvironment(EnvironmentType environmentType)
    {
        Debug.Log(environmentType);
        CloudManager.Instance.ClearClouds();
        MistManager.Instance.ClearMist();
        switch (environmentType)
        {
            case EnvironmentType.Thunderstrom:
                CloudManager.Instance.ShowClouds(CloudType.Thunder);
                break;

            case EnvironmentType.Drizzle:
                CloudManager.Instance.ShowClouds(CloudType.Medium);
                break;

            case EnvironmentType.Rain:
                CloudManager.Instance.ShowClouds(CloudType.Heavy);
                break;

            case EnvironmentType.Snow:
                CloudManager.Instance.ShowClouds(CloudType.Heavy);
                MistManager.Instance.ShowMist(MistType.Medium);
                break;

            case EnvironmentType.Atmosphere:
                MistManager.Instance.ShowMist(MistType.Heavy);
                break;

            case EnvironmentType.Clear:
                break;

            case EnvironmentType.LightClouds:
                CloudManager.Instance.ShowClouds(CloudType.Light);
                break;

            case EnvironmentType.ScatteredClouds:
                CloudManager.Instance.ShowClouds(CloudType.Medium);
                break;

            case EnvironmentType.OvercastClouds:
                CloudManager.Instance.ShowClouds(CloudType.Heavy);
                break;
        }
    }

    EnvironmentType GetEnvironmentType(int code)
    {
        Debug.Log("Weather Code: " + code);
        switch (code)
        {
            case 800: return EnvironmentType.Clear;
            case 801: return EnvironmentType.LightClouds;
            case 802: return EnvironmentType.ScatteredClouds;
            case 803: return EnvironmentType.ScatteredClouds;
            case 804: return EnvironmentType.OvercastClouds;
        }

        switch (code / 100)
        {
            case 2: return EnvironmentType.Thunderstrom;
            case 3: return EnvironmentType.Drizzle;
            case 5: return EnvironmentType.Rain;
            case 6: return EnvironmentType.Snow;
            case 7: return EnvironmentType.Atmosphere;
            default: return EnvironmentType.Clear;
        }
    }
}

public enum EnvironmentType
{
    Thunderstrom, Drizzle, Rain, Snow, Atmosphere, Clear, LightClouds, ScatteredClouds, OvercastClouds
}