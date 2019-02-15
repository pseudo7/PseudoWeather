using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpiralMeter : MonoBehaviour
{
    public float fillSpeed = 75f;

    Image meterImg;
    Text valueText;

    void Start()
    {
        meterImg = transform.GetChild(0).GetComponent<Image>();
        if (transform.childCount > 1)
            valueText = transform.GetChild(1).GetChild(0).GetComponent<Text>();
    }

    public void SetMeterValue(float val)
    {
        if (valueText)
            valueText.text = string.Format("{0}%", val.ToString("0#"));
        StartCoroutine(FillMeter(val / 100f));
    }

    IEnumerator FillMeter(float val)
    {
        var fillStep = 1f / fillSpeed;
        meterImg.fillAmount = 0;
        yield return new WaitForEndOfFrame();

        while (meterImg.fillAmount < val)
        {
            yield return new WaitForEndOfFrame();
            meterImg.fillAmount += fillStep;
        }
    }
}
