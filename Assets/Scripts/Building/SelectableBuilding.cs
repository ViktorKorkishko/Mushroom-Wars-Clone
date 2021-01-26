using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof(Building))]
public class SelectableBuilding : MonoBehaviour, ISelectable, IHaveStatus
{
    public bool IsSelected { get; private set; }
    public UnitStatus UnitStatus
    {
        get
        {
            return _building.UnitStatus;
        }
    }
    [SerializeField] protected IFlasher _flasher;

    private Building _building;
    private void Awake()
    {
        _building = GetComponent<Building>();
        if (TryGetComponent(out IFlasher flasher))
        {
            _flasher = flasher;
        }
    }

    public void OnSelect()
    {
        if (UnitStatus == UnitStatus.Ally)
        { 
            IsSelected = true;
            if (_flasher != null)
            {
                if (_flasher is SpriteFlasher)
                {
                    SpriteFlasher _sf = _flasher as SpriteFlasher;
                    _sf.ChangeDefaultColor();
                }
                _flasher.StartFlashing();
            }
        }
    }

    public void OnDeselect()
    {
        IsSelected = false;
        if (_flasher != null)
        {
            _flasher.StopFlashing();
        }
    }
}