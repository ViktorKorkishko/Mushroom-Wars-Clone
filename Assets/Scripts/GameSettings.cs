using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSettings
{
    private static Dictionary<UnitStatus, Color> _colorDictionary = new Dictionary<UnitStatus, Color>()
    {
        { UnitStatus.Ally, Color.blue },
        { UnitStatus.Enemy, Color.red },
        { UnitStatus.Neutral, Color.grey }
    };

    private static float _defenceCoefficient = 1.2f;
    private static float _unitDelimitingCoef = 0.25f;

    public static float DefenceCoefficient
    {
        get
        {
            return _defenceCoefficient;
        }
    }

    public static float UnitDelimitingCoef
    {
        get
        {
            return _unitDelimitingCoef;
        }
        set
        {
            if (value > 1)
            {
                _unitDelimitingCoef = 1;
            }
            if (value < 0.25)
            {
                _unitDelimitingCoef = 0.25f;
            }
            else
            {
                _unitDelimitingCoef = value;
            }
        }
    }

    public static Color GetColorByUnitStatus(UnitStatus unitStatus)
    {
        Color color;
        _colorDictionary.TryGetValue(unitStatus, out color);
        return color;
    }
}
