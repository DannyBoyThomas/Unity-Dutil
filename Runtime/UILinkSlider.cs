using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dutil;
using UnityEngine.UI;
[RequireComponent(typeof(Slider))]
public class UILinkSlider : MonoBehaviour
{
    public bool isMasterVolume = false;
    [ConditionalHideProperty("isMasterVolume")]
    public string variableName;
    Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
        if (isMasterVolume)
        {
            slider.value = GameSettings.MasterVolume;
        }
        else
        {
            slider.value = GameSettings.GetFloat(variableName, 0.5f);
        }
        slider.onValueChanged.AddListener(OnValueChange);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnValueChange(float val)
    {
        if (isMasterVolume)
        {
            GameSettings.MasterVolume = val;
        }
        else
        {
            GameSettings.SetFloat(variableName, val);
        }
    }
    void OnDestroy()
    {
        slider.onValueChanged.RemoveListener(OnValueChange);
    }
}