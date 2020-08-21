using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class SliderControl : MonoBehaviour
{
    public Slider slider1, slider2, slider3;
    public TextMeshProUGUI paramValue1, paramValue2, paramValue3;
    public DataReceive data;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(data.intervalToPrev);
        slider1.value = data.intervalToPrev;
        slider2.value = data.maxDiffToStep;
        slider3.value = data.maxDiffToResetStep;
        paramValue1.text = Math.Round(slider1.value, 3).ToString();
        paramValue2.text = Math.Round(slider2.value, 3).ToString();
        paramValue3.text = Math.Round(slider3.value, 3).ToString();
        slider1.onValueChanged.AddListener(delegate { ValueChangeSlider1(); });
        slider2.onValueChanged.AddListener(delegate { ValueChangeSlider2(); });
        slider3.onValueChanged.AddListener(delegate { ValueChangeSlider3(); });
    }

    public void ValueChangeSlider1()
    {
        paramValue1.text = Math.Round(slider1.value, 3).ToString();
        data.intervalToPrev = slider1.value;
        
    }
    public void ValueChangeSlider2()
    {
        
        paramValue2.text = Math.Round(slider2.value, 3).ToString();
        data.maxDiffToStep = slider2.value;

    }
    public void ValueChangeSlider3()
    {
        
        paramValue3.text = Math.Round(slider3.value, 3).ToString();
        data.maxDiffToResetStep = slider3.value;
    }
    
}
