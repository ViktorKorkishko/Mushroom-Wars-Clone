using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

[RequireComponent(typeof(ImageController), typeof(ISpawner))]
public abstract class Building : MonoBehaviour, IHaveStatus
{
    public virtual UnitStatus UnitStatus
    {
        get
        {
            return _buildingStatus;
        }
        protected set 
        {
            _buildingStatus = value;
        }
    }

    public virtual int CurrentUnitsIn
    {
        get
        {
            return Convert.ToInt32(_nominalUnitsIn);
        }
        set
        {
            _currentUnitsIn = value;
            onNumberOfUnitsInChanged?.Invoke(this);
            _nominalUnitsIn = _currentUnitsIn;
        }
    }

    protected float NominalUnitsIn
    {
        get
        {
            return _nominalUnitsIn;
        }
        set
        {
            _nominalUnitsIn = value;
            _currentUnitsIn = Convert.ToInt32(_nominalUnitsIn);
            onNumberOfUnitsInChanged?.Invoke(this);
        }
    }

    public delegate void OnNumberOfUnitsInChanged(Building building);
    public OnNumberOfUnitsInChanged onNumberOfUnitsInChanged;

    public delegate void OnSendUnits(Building targetBuilding, UnitStatus unitStatus);
    public OnSendUnits onSendUnits;

    public delegate void OnBuildingCaptured(Building building, UnitStatus newUnitStatus);
    public OnBuildingCaptured onBuildingCaptured;

    protected ISpawner _spawner;

    [Header("Building perameters")]
    [SerializeField] protected UnitStatus _buildingStatus;
    [SerializeField] protected int _currentUnitsIn;
    [SerializeField] private float _nominalUnitsIn;
    private ImageController _imageController;

    private void Awake()
    {
        _spawner = GetComponent<ISpawner>();
        _imageController = GetComponent<ImageController>();
    }

    public virtual void SendUnits(Building targetBuilding, float percentage)
    {
        onSendUnits?.Invoke(targetBuilding, UnitStatus);
    }

    protected virtual void HandleUnitEnter(GameObject go)
    {
        var unit = go.gameObject.GetComponent<Unit>();
        if (unit && unit.TargetPosition == transform.position)
        {
            if (unit.UnitStatus == UnitStatus)
            {
                CurrentUnitsIn++;
            }
            if(unit is Warrior && unit.UnitStatus != UnitStatus)
            {
                var warrior = unit as Warrior;
                NominalUnitsIn -= warrior.Damage;
                if (NominalUnitsIn <= 0)
                {
                    UnitStatus = unit.UnitStatus;
                    onBuildingCaptured?.Invoke(this, UnitStatus);
                    _imageController.ChangeColor(UnitStatus);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleUnitEnter(other.gameObject);
    }
}
