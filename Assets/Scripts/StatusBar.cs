using UnityEngine;
using System.Collections;

public class StatusBar : MonoBehaviour {

    private float maxHeight, currentHeight;

	// Use this for initialization
	void Start () {
        maxHeight = GetComponent<RectTransform>().localScale.y;
	}

    public void UpdateBar (float _value, float _maxValue )
    {
        float ratio = maxHeight / _maxValue;

        currentHeight = _value * ratio;

        print("CURRENT: " + currentHeight + " - MAX: " + maxHeight);
        if (currentHeight >= maxHeight) currentHeight = maxHeight;

        GetComponent<RectTransform>().localScale = new Vector2(GetComponent<RectTransform>().localScale.x, currentHeight);


       

    }
}
