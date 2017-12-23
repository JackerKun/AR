using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
*Create By Keefor On 12/20/2017
*/

public class CircleSlider : MonoBehaviour {

    Image circleImage;
    Text valueText;
    private float maxvalue = 100;
    private float _value = 0;
    private float _percent;

	void Awake () {
        circleImage = this.transform.Find("circle").GetComponent<Image>();
        valueText = this.transform.Find("value").GetComponent<Text>();
	}

    public float Max {
        set { maxvalue = value; }
    }

    public float Value {
        set {            
            _value = value;
            _percent = _value / maxvalue;
            valueText.text = string.Format("{0:00.00}%", _percent);
            circleImage.fillAmount = _percent;
            circleImage.color = GetWarnColor(_percent);
        }
        get { return _value; }
    }

    public Color GetWarnColor(float percent)
    {
        float r = Mathf.Clamp01((percent - 1f / 3f) * 3f);
        float g = Mathf.Max(0, (1f - 3f * percent));
        float b = 1f - Mathf.Max(0, (percent - 2f / 3f) * 3f);
        return new Color(r, g, b);
    }

	
}
