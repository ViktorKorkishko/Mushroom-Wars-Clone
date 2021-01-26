using Enums;
using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Building))]
public abstract class Spawner : MonoBehaviour, ISpawner, IHaveStatus
{
    public UnitStatus UnitStatus
    {
        get
        {
            return _building.UnitStatus;
        }
    }

    public virtual float UnitProduceSpeed { get; }

    public virtual int CurrentLevel { get; protected set; }

    public virtual int DefaultLevel { get; protected set; }

    public virtual int MinLevel { get; protected set; }

    public virtual int MaxLevel { get; }

    public virtual int UpgradeCost { get; }

    protected Building _building;

    public delegate void OnUpgrade(Spawner spawner, UnitStatus spawnerStatus);
    public OnUpgrade onSpawnerUpgrade;

    public delegate void OnSpawn(Spawner spawner, Unit unit);
    public OnSpawn onUnitSpawn;

    public abstract void Spawn();

    public abstract void Upgrade();
}
