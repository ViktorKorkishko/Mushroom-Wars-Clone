using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberVisualizer : MonoBehaviour
{
    [SerializeField] private Building _building;
    [SerializeField] private Text _text;
 
    private void Awake()
    {
        _building.onNumberOfUnitsInChanged += HandleOnNumberOfUnitsChanged;
    }

    private void HandleOnNumberOfUnitsChanged(Building building)
    {
        _text.text = building.CurrentUnitsIn.ToString();
    }
}
