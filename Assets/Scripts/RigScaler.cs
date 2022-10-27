using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RigScaler : MonoBehaviour
{
    public GameObject rig;
    public Text sliderText;
    public Slider heightSlider;

    void Update()
    {
        sliderText.text = "Current Height: " + heightSlider.value;
        rig.transform.position = new Vector3(0.0f, heightSlider.value, 0.0f);
    }
}
