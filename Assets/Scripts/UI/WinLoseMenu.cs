using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using UnityEngine.UI;

public class WinLoseMenu : MonoBehaviour
{
    [SerializeField] private WinLoseController _winLoseController;
    [SerializeField] private GameObject _winLoseHolder;
    [SerializeField] private Text _text;

    [SerializeField] private string _winText;
    [SerializeField] private string _loseText;

    private void Awake()
    {
        _winLoseController.onGameEnd += HandleOnGameEnd;
        _winLoseHolder.SetActive(false);
    }

    private void HandleOnGameEnd(UnitStatus unitStatus)
    {
        if (unitStatus == UnitStatus.Ally)
        {
            _text.text = _winText;
        }
        if(unitStatus == UnitStatus.Enemy)
        {
            _text.text = _loseText;
        }

        _winLoseHolder.SetActive(true);
    }
}
