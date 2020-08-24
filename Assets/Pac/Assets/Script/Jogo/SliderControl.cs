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
    public float[] defaultValues = new float[3] { 0,0,0,};
    
    // Start is called before the first frame update
    void GetDefaultValue()
    {
        defaultValues[0] = data.intervalToPrev;
        defaultValues[1] = data.maxDiffToStep;
        defaultValues[2] = data.maxDiffToResetStep;

    }
    void SetDefaultValue()
    {
        
        slider1.value = defaultValues[0];
        slider2.value = defaultValues[1];
        slider3.value = defaultValues[2];
        
        paramValue1.text = Math.Round(slider1.value, 3).ToString();
        paramValue2.text = Math.Round(slider2.value, 3).ToString();
        paramValue3.text = Math.Round(slider3.value, 3).ToString();
    }
    void AddListner()
    {
        slider1.onValueChanged.AddListener(delegate { ValueChangeSlider1(); });
        slider2.onValueChanged.AddListener(delegate { ValueChangeSlider2(); });
        slider3.onValueChanged.AddListener(delegate { ValueChangeSlider3(); });
    }

    void Start()
    {
        defaultValues = new float[3];
        GetDefaultValue();
        SetDefaultValue();
        AddListner();

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
    public void ResetToDefaultValues()
    {
        SetDefaultValue();
    }

   
}
