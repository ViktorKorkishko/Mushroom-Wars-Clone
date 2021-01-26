using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO : Refactor this hell
public class DelimitingButtonsController : MonoBehaviour
{
    [SerializeField] private List<UnitDelimiterButton> _buttons;
    private UnitDelimiterButton _lastClickedButton;

    private void Awake()
    {
        foreach (UnitDelimiterButton button in _buttons)
        {
            button.RegisterOnNumberOfUnitsChanged(HandleOnButtonClick);
            if (button.DelimitingCoef == GameSettings.UnitDelimitingCoef)
            {
                button.delimitionCoefIsSet?.Invoke(button);
            }
        }
    }

    private void HandleOnButtonClick(UnitDelimiterButton button)
    {
        if (button == _lastClickedButton)
        {
            return;
        }
        else
        {
            Outline outline = button.GetComponent<Outline>();
            if (outline)
            {   
                outline.enabled = true;
            }
            else
            {
                outline = button.gameObject.AddComponent<Outline>();
                outline.effectColor = Color.red;
                outline.effectDistance = new Vector2(10, 10);
            }
            if (_lastClickedButton)
            {
                _lastClickedButton.GetComponent<Outline>().enabled = false;
            }
            _lastClickedButton = button;
        }
    }
}
