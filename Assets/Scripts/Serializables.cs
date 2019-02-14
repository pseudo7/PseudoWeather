using System;

public class Serializables
{
    [Serializable]
    public class Coordinates
    {
        public double lon;
        public double lat;

        public override string ToString()
        {
            return string.Format("Longitude: {0}, Latitude:{1}", lon, lat);
        }
    }

    [Serializable]
    public class Weather
    {
        public int id;
        public string main;
        public string description;
        public string icon;

        public override string ToString()
        {
            return string.Format("ID: {0}, Main:{1}, Desc: {2}, Icon:{3}", id, main, description, icon);
        }
    }

    [Serializable]
    public class Main
    {
        public double temp;
        public int pressure;
        public int humidity;
        public double temp_min;
        public double temp_max;

        public override string ToString()
        {
            return string.Format("Temperature: {0}, Pressure: {1}, Humidity: {2}, Max Temp: {3}, Min Temp: {4}", temp, pressure, humidity, temp_max, temp_min);
        }
    }

    [Serializable]
    public class Wind
    {
        public double speed;
        public int deg;

        public override string ToString()
        {
            return string.Format("Speed: {0}, Degrees: {1}", speed, deg);
        }
    }

    [Serializable]
    public class Clouds
    {
        public int all;

        public override string ToString()
        {
            return string.Format("Cloud All: {0}", all);
        }
    }

    [Serializable]
    public class System
    {
        public int type;
        public int id;
        public double message;
        public string country;
        public int sunrise;
        public int sunset;

        public override string ToString()
        {
            return string.Format("Type: {0}, ID:{1}, Message: {2}, Country: {3}, Sunrise: {4}, Sunset: {5}", type, id, message, country, sunrise, sunset);
        }
    }

    public class WeatherMain
    {
        public Coordinates coord;
        public Weather[] weather;
        public Main main;
        public int visibility;
        public Wind wind;
        public Clouds clouds;
        public int dt;
        public System sys;
        public int id;
        public string name;
        public int cod;

        public override string ToString()
        {
            return string.Format("Coordinates: {0}\n Weather: {1}\n WeatherMain: {2}\n Visibility: {3}\n Wind: {4}\n Cloud: {5}\n DT: {6}\n System: {7}\n Main ID: {8}\n Name: {9}\n COD: {10}",
                coord, weather[0], main, visibility, wind, clouds, dt, sys, id, name, cod);
        }
    }
}
