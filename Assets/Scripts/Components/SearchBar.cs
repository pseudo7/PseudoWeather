using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SearchBar : MonoBehaviour
{
    public GameObject resultItemPrefab;
    public GameObject resultPanel;
    public Transform resultParent;
    public InputField searchIF;

    void Start()
    {
        for (int i = 0; i < Utility.CityCount; i++)
        {
            var result = Instantiate(resultItemPrefab, resultParent).GetComponent<Button>();
            result.onClick.AddListener(() => SetResult(result.transform.GetChild(0).GetComponent<Text>().text));
        }
        DisableAllResults();
    }

    public void GetWeather()
    {
        if (string.IsNullOrEmpty(searchIF.text))
            Debug.LogError("ERROR");
        else if (Application.internetReachability == NetworkReachability.NotReachable)
            UIManager.Instance.ShowWarning();
        else
            WeatherManager.Instance.GetWeather(searchIF.text);
    }

    public void OnKeywordTyped()
    {
        UpdateResults(Utility.GetMatchCityNames(searchIF.text.Trim()));
    }

    public void OnSubmit()
    {
        string searchKeyword = searchIF.text.Trim();
        if (Utility.CityNames.Contains(searchKeyword))
        {
            WeatherManager.Instance.GetWeather(searchKeyword);
            resultPanel.SetActive(false);
        }
        else Debug.LogError("NO SUCH CITY");
    }

    public void UpdateResults(List<string> matchedNames)
    {
        DisableAllResults();
        for (int i = 0; i < matchedNames.Count; i++)
        {
            resultParent.GetChild(i).GetChild(0).GetComponent<Text>().text = matchedNames[i];
            resultParent.GetChild(i).gameObject.SetActive(true);
        }
    }

    void SetResult(string cityName)
    {
        searchIF.text = cityName;
        resultPanel.SetActive(false);
    }

    void DisableAllResults()
    {
        foreach (Transform child in resultParent) child.gameObject.SetActive(false);
    }
}
