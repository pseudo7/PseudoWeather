using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour
{
    public Transform mainLight;

    void Start()
    {
        StartCoroutine(GetWeather(Utility.Instance.GetURL(CityName.Chandigarh, true)));
        StartCoroutine(SunRotation());
    }

    IEnumerator SunRotation()
    {
        string[] hoursMin = DateTime.Now.ToString("HH:mm").Split(':');
        int hours = int.Parse(hoursMin[0]), mins = int.Parse(hoursMin[1]);
        const float degPerMinute = 360 / (24 * 60);

        while (gameObject.activeInHierarchy)
        {
            mainLight.rotation = Quaternion.Euler(270 + (hours * 60 + mins) * degPerMinute, 0, 0);
            yield return new WaitForSeconds(60);
        }
    }

    IEnumerator GetWeather(string requestURL)
    {
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
                UIManager.Instance.SetIcon(icon);
                UIManager.Instance.SetDescription(weatherMainData.weather[0].description);
                UIManager.Instance.SetWeatherTitle(weatherMainData.weather[0].main);
            }
        }
    }
}