﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValhallaBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxValhalla(int val)
    {
        slider.maxValue = val;
    }

    public void SetValhalla(int val)
    {
        slider.value = val;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
