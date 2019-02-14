using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utility : MonoBehaviour
{
    public static Utility Instance;

    private void Awake()
    {
        if (!Instance)
            Instance = this;

        string[] cityNames = Enum.GetNames(typeof(CityNames));
        Array.Sort(cityNames);

        foreach (var item in cityNames)
            print(item);
    }

    public string GetCityID(CityNames city)
    {
        switch (city)
        {
            case CityNames.Chandigarh: return "1274746";
            case CityNames.Manali: return "1263968";
            case CityNames.Delhi: return "1273294";
            case CityNames.Mumbai: return "1275339";
            case CityNames.Bangalore: return "1277333";
            case CityNames.Hyderabad: return "1269843";
            case CityNames.Kolkata: return "1275004";
            case CityNames.Chennai: return "1264527";
            default: return "2172797";
        }
    }

}

public enum CityNames
{
    Chandigarh, Manali, Delhi, Mumbai, Bangalore, Hyderabad, Kolkata, Chennai
}