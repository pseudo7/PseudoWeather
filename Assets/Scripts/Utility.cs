using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Utility Instance;

    void Awake()
    {
        if (!Instance)
            Instance = this;
    }

    public string GetURL(CityName cityName, bool isMetricUnits)
    {
        return string.Format("{0}{1}&{2}&{3}", Constants.MAIN_URL, GetCityID(cityName), Constants.API_KEY, isMetricUnits ? Constants.WEATHER_METRIC_UNITS : Constants.WEATHER_IMPERIAL_UNITS);
    }

    public string[] CityNames
    {
        get
        {
            string[] cityNames = Enum.GetNames(typeof(CityName));
            Array.Sort(cityNames);
            return cityNames;
        }
    }

    string GetCityID(CityName city)
    {
        switch (city)
        {
            case CityName.Chandigarh: return "1274746";
            case CityName.Manali: return "1263968";
            case CityName.Delhi: return "1273294";
            case CityName.Mumbai: return "1275339";
            case CityName.Bangalore: return "1277333";
            case CityName.Hyderabad: return "1269843";
            case CityName.Kolkata: return "1275004";
            case CityName.Chennai: return "1264527";
            default: return "2172797";
        }
    }

    public static string GetTitleCase(string str)
    {
        return new System.Globalization.CultureInfo("en-US", false).TextInfo.ToTitleCase(str);
    }

}

public enum CityName
{
    Chandigarh, Manali, Delhi, Mumbai, Bangalore, Hyderabad, Kolkata, Chennai
}