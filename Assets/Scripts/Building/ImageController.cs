using Enums;
using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IHaveStatus), typeof(SpriteRenderer))]
public class ImageController : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private IHaveStatus _statusProvider;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _statusProvider = GetComponent<IHaveStatus>();
        _spriteRenderer.color  = GameSettings.GetColorByUnitStatus(_statusProvider.UnitStatus);
    }

    public void ChangeColor(UnitStatus unitStatus)
    {
        _spriteRenderer.color = GameSettings.GetColorByUnitStatus(unitStatus);
    }
}
