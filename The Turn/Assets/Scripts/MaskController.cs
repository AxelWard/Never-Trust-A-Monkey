using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskController : MonoBehaviour
{
    [Range(0.0f, 100.0f)]
    public float showPercentage;
    
    private RectTransform mask;
    private float fullWidth;

    private void Start()
    {
        mask = GetComponent<RectTransform>();
        fullWidth = mask.rect.width;
    }

    private void Update()
    {
        mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullWidth * (showPercentage / 100f));
    }
}
