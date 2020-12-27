using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootCountText : MonoBehaviour
{
    public AnimationCurve scaleCurve;

    private CanvasGroup cg;

    private Text topText, bottomText;

    public string TopText { 
        get 
        { 
            return topText.text;
        } 
        set 
        {
            topText.text = value; 
        } 
    }
    public string BottomText
    {
        get
        {
            return bottomText.text;
        }
        set
        {
            bottomText.text = value;
        }
    }


    void Awake()
    {
        cg = GetComponent<CanvasGroup>();
        topText = transform.Find("TopText").GetComponent<Text>();
        bottomText = transform.Find("BottomText").GetComponent<Text>();
        transform.localScale = Vector3.zero;
    }

    public void Flash()
    {
        cg.alpha = 1;
        transform.localScale = Vector3.zero;
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        for (int i = 0; i <= 60; i++)
        {
            transform.localScale = Vector3.one * scaleCurve.Evaluate((float)i / 50);
            if (i >= 40)
            {
                cg.alpha = (float)(60 - i) / 20;
            }
            yield return null;
        }
        yield break;
    }
}
