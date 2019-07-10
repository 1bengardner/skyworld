using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextFitter : MonoBehaviour {

    public MinMax aspect;
    public MinMax fontSize;
    public MinMax lineSpacing;

    [System.Serializable]
    public struct MinMax
    {
        public float min;
        public float max;
    }
    Text text;
    float lastAspect;

    void Start()
    {
        text = GetComponent<Text>();
    }

    void Update()
    {
        if (lastAspect != Camera.main.aspect)
        {
            float t = (Camera.main.aspect - aspect.min) / (aspect.max - aspect.min);
            text.fontSize = (int)Mathf.Lerp(fontSize.min, fontSize.max, t);
            text.lineSpacing = Mathf.Lerp(lineSpacing.min, lineSpacing.max, t);
            lastAspect = Camera.main.aspect;
        }
	}
}
