using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensitivityController : MonoBehaviour
{
    public static float sensitivity;

    private Slider senseSlider;

    private void Start()
    {
        senseSlider = gameObject.GetComponent<Slider>();
        sensitivity = PlayerPrefs.GetFloat("Sensitivity", 2f);

        if(senseSlider != null)
        {
            senseSlider.value = sensitivity;
        }
    }

    private void Update()
    {
        if(senseSlider != null)
        {
            sensitivity = senseSlider.value;
            PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        }
    }
}
