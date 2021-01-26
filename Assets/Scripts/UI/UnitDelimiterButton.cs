using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Interfaces;

[RequireComponent(typeof(Button))]
public class UnitDelimiterButton : MonoBehaviour//, ISelectable
{
    public float DelimitingCoef
    {
        get
        {
            return _delimitingCoef;
        }
    }

    [Range(0, 1)]
    [SerializeField] private float _delimitingCoef;

    public delegate void DelimitionCoefIsSet(UnitDelimiterButton button);
    public DelimitionCoefIsSet delimitionCoefIsSet;

    private void Awake()
    {
    }

    public void SetUnitDelimitingCoef()
    {
        GameSettings.UnitDelimitingCoef = _delimitingCoef;
        delimitionCoefIsSet?.Invoke(this);
    }

    public void RegisterOnNumberOfUnitsChanged(DelimitionCoefIsSet delimitionCoefIsSet)
    {
        this.delimitionCoefIsSet = delimitionCoefIsSet;
    }
}
