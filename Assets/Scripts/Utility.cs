using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Utility
{
    static List<string> cityNames;

    public static string GetURL(string cityName, bool isMetricUnits)
    {
        return string.Format("{0}{1}&{2}&{3}", Constants.MAIN_URL, GetCityID(cityName), Constants.API_KEY, isMetricUnits ? Constants.WEATHER_METRIC_UNITS : Constants.WEATHER_IMPERIAL_UNITS);
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

    public static int GetCityID(string cityName)
    {
        foreach (var info in CitiesInfo)
            if (info.name.Equals(cityName)) return info.id;
        return -1;
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
            var jsonData = File.ReadAllText(Path.Combine(Application.streamingAssetsPath, "CityNames.json"));
            Serializables.CityCollection collection = JsonUtility.FromJson<Serializables.CityCollection>(jsonData);
            return collection.cities;
        }
    }

    public static string GetTimeFromUNIX(long unixTimeStamp)
    {
        return new DateTime(TimeSpan.FromSeconds(unixTimeStamp).Ticks + TimeSpan.FromHours(5.5).Ticks).ToShortTimeString();
    }

    public static string GetTitleCase(string str)
    {
        return new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(str);
    }
}