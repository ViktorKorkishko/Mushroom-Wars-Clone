using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

public class Village : Building
{
    [Header("Village settings")]
    [SerializeField] private float _maxUnitsCoef;

    public virtual UnitStatus UnitStatus
    {
        get
        {
            return _buildingStatus;
        }
    }

    public delegate void OnUnitCreation();
    public OnUnitCreation onUnitCreation;

    private void Start()
    {
        NominalUnitsIn = CurrentUnitsIn;
        StartCoroutine(CreateUnitCo());
    }

    public override void SendUnits(Building targetBuilding, float percentage)
    {
        if (_spawner is IWaveSpawner)
        {
            var waveSpawner = _spawner as IWaveSpawner;
            waveSpawner.TargetPostion = targetBuilding.transform.position;
        }
        _spawner.Spawn();
        onSendUnits?.Invoke(targetBuilding, UnitStatus);
    }

    private void CreateUnit()
    {
        if (_maxUnitsCoef * _spawner.CurrentLevel > NominalUnitsIn)
        {
            NominalUnitsIn += _spawner.UnitProduceSpeed;
        }
    }

    private IEnumerator CreateUnitCo()
    {
        CreateUnit();
        onUnitCreation?.Invoke();

        yield return new WaitForSeconds(1f);
        
        yield return StartCoroutine(CreateUnitCo());
    }
}