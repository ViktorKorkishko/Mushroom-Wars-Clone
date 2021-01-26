using System;
using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFlasher : MonoBehaviour, IFlasher
{
    private SpriteRenderer _spriteRenderer;
    private Color _defaultColor;
    private bool becomesTransparent = true;
    private bool isFlashing = false;
    
    [Range(0, 1f)]
    [SerializeField] private float fadingStep;
    
    [Range(0, 1f)] 
    [SerializeField] private float lowerFadingBound;
    
    [Range(0, 1f)] 
    [SerializeField] private float upperFadingBound;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;
    }

    public void StartFlashing()
    {
        isFlashing = true;
        StartCoroutine(FlashCo());
    }
    
    public void StopFlashing()
    {
        isFlashing = false;
        StopCoroutine(FlashCo());
    }

    public void ChangeDefaultColor()
    {
        _defaultColor = _spriteRenderer.color;
    }
    
    private IEnumerator FlashCo()
    {
        if (_spriteRenderer)
        {
            var tempColor = _spriteRenderer.color;
            while (isFlashing)
            {
                yield return new WaitForFixedUpdate();

                if (becomesTransparent)
                {
                    if (tempColor.a < lowerFadingBound)
                    {
                        tempColor.a = lowerFadingBound;
                        becomesTransparent = false;
                    }
                    else
                    {
                        tempColor.a = tempColor.a - fadingStep;
                    }

                    _spriteRenderer.color = tempColor;
                }

                if (!becomesTransparent)
                {
                    if (tempColor.a > upperFadingBound)
                    {
                        tempColor.a = upperFadingBound;
                        becomesTransparent = true;
                    }
                    else
                    {
                        tempColor.a = tempColor.a + fadingStep;
                    }

                    _spriteRenderer.color = tempColor;
                }
            }

            _spriteRenderer.color = _defaultColor;
        }
    }
}