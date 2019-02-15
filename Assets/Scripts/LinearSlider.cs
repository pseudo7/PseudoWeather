using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinearSlider : MonoBehaviour
{
    public float fillSpeed = 7;

    Slider pressureSlider;

    void Start()
    {
        pressureSlider = GetComponent<Slider>();
    }

    public void SetSliderValue(float val)
    {
        StartCoroutine(FillSlider(val));
    }

    IEnumerator FillSlider(float val)
    {
        var fillStep = fillSpeed;
        pressureSlider.value = 0;
        yield return new WaitForEndOfFrame();

        while (pressureSlider.value < val)
        {
            yield return new WaitForEndOfFrame();
            pressureSlider.value += fillStep;
        }
    }
}
