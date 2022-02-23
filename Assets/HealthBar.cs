using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

    public Slider _slider;
    public Gradient _gradient;
    public Image _fill;
    public void SetHealth(int health)
    {
        _slider.value = health;
        _fill.color = _gradient.Evaluate(_slider.normalizedValue);

    }

    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;
        _fill.color = _gradient.Evaluate(1f);
    }

}
