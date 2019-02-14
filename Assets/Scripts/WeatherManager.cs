using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeatherManager : MonoBehaviour
{
    string URL = "api.openweathermap.org/data/2.5/weather?id=1268782&APIKEY=1d5f9f9b27a4b4345f8fc69f597a7dbe&units=metric";

    void Start()
    {
        StartCoroutine(GetWeather(URL));
    }

    private IEnumerator GetWeather(string requestURL)
    {
        using (UnityWebRequest weatherRequest = UnityWebRequest.Get(requestURL))
        {
            yield return weatherRequest.SendWebRequest();
            Serializables.WeatherMain weatherMain = JsonUtility.FromJson<Serializables.WeatherMain>(weatherRequest.downloadHandler.text);
            Debug.Log(weatherMain);
        }
    }
}