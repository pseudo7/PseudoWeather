using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GetWeather(Utility.Instance.GetURL(CityName.Chandigarh, true)));
    }

    private IEnumerator GetWeather(string requestURL)
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