using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public static class Utility
{
    public static Serializables.CityInfo[] cityInfos;

    static List<string> cityNames;

    public static bool IsFahrenheit { get; set; }

    public static string TempSymbol { get { return IsFahrenheit ? "F" : "C"; } }

    public static string GetURL(string cityName)
    {
        return string.Format("{0}{1}&{2}&{3}", Constants.MAIN_URL, GetCityID(cityName), Constants.API_KEY, IsFahrenheit ? Constants.WEATHER_IMPERIAL_UNITS : Constants.WEATHER_METRIC_UNITS);
    }

    public static List<string> CityNames
    {
        get
        {
            if (cityNames == null)
            {
                cityNames = new List<string>();
                foreach (var cityInfo in CitiesInfo) cityNames.Add(cityInfo.name);
                cityNames.Sort();
            }
            return cityNames;
        }
    }

    public static int CityCount
    {
        get
        {
            return CityNames.Count;
        }
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        System.Random rng = new System.Random();
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static int GetCityID(string cityName)
    {
        foreach (var info in CitiesInfo)
            if (info.name.Equals(cityName)) return info.id;
        return -1;
    }

    public static void ScaleGameObject(this GameObject gameObj, bool show)
    {
        UIManager.Instance.ScaleGO(gameObj, show);
    }

    public static List<string> GetMatchCityNames(string keyword)
    {
        List<string> matches = new List<string>();

        foreach (var cityName in CityNames) if (cityName.ToLower().StartsWith(keyword.ToLower())) matches.Add(cityName);

        return matches;
    }

    public static Serializables.CityInfo[] CitiesInfo
    {
        get
        {
            if (cityInfos == null)
            {
                var jsonData = Resources.Load<TextAsset>(Constants.CITIES_FILE_NAME);
                Serializables.CityCollection collection = JsonUtility.FromJson<Serializables.CityCollection>(jsonData.text);
                cityInfos = collection.cities;
            }
            return cityInfos;
        }
    }

    public static string GetTimeFromUNIX(long unixTimeStamp)
    {
        return new DateTime(TimeSpan.FromSeconds(unixTimeStamp).Ticks + TimeSpan.FromHours(5.5).Ticks).ToString("hh:mm tt");
    }

    public static string GetTitleCase(string str)
    {
        return new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(str);
    }
}