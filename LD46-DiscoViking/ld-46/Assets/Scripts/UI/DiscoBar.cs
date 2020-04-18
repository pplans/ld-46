using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscoBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxValhalla(int maxVahalla)
    {
        slider.maxValue = maxVahalla;
    }

    public void SetValhalla(int vahalla)
    {
        slider.value = vahalla;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
