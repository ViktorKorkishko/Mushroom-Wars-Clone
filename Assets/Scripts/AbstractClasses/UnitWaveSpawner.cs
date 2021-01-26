using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

public abstract class UnitWaveSpawner : Spawner, IWaveSpawner
{
    public override int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
        protected set
        {
            if (value > MaxLevel)
            {
                _currentLevel = MaxLevel;
            }
            if (value < MinLevel)
            {
                _currentLevel = MinLevel;
            }
            else
            {
                _currentLevel = value;
            }
        }
    }

    public override int DefaultLevel
    {
        get
        {
            return _defaultLevel;
        }
        protected set
        {
            if (value > MaxLevel)
            {
                _defaultLevel = MaxLevel;
            }
            else if (value < MinLevel)
            {
                _defaultLevel = MinLevel;
            }
            else
            {
                _defaultLevel = value;
            }
        }
    }

    public override int MinLevel
    {
        get
        {
            return _minLevel;
        }
        protected set
        {
            if (value >= 1)
            {
                _minLevel = value;
            }
            else
            {
                _minLevel = 1;
            }

        }
    }

    public override int MaxLevel
    {
        get
        {
            return _upgradeSystemList.Count;
        }
    }

    public virtual float UnitProducingSpeed
    {
        get
        {
            return _upgradeSystemList[CurrentLevel - 1].x;
        }
    }

    public override int UpgradeCost
    {
        get
        {
            return Convert.ToInt32(_upgradeSystemList[CurrentLevel - 1].y);
        }
    }

    public virtual Vector3 TargetPostion 
    {
        set
        {
            _targetPostion = value;
        }
        get
        {
            return _targetPostion;
        }
    }

    public override float UnitProduceSpeed
    {
        get
        {
            return _upgradeSystemList[CurrentLevel - 1].x;
        }
    }

    [SerializeField] private Vector3 _targetPostion;
    [SerializeField] protected GameObject _unitPrefab;
    [SerializeField] protected float _timeToSpawnBetweenWaves;
    [SerializeField] protected int _unitsToSpawnPerWave;
    [SerializeField] protected float _spawnDistance;
    [SerializeField] protected float _distanceBetweenUnitsInWaveCoef;

    [Header("Upgrade Settings")]
    [SerializeField] protected int _currentLevel;
    [SerializeField] protected int _defaultLevel;
    [SerializeField] protected int _minLevel;
    [SerializeField] protected int _maxLevel;

    [Header("Upgrade System")]
    //first element is the unit speed producing (x field)
    // second element is the cost for next level upgrade (y field)
    [SerializeField] protected List<Vector2> _upgradeSystemList;

    public override void Spawn()
    {
        SpawnWave(_targetPostion);
    }
    
    protected abstract void SpawnWave(Vector3 targetPostion);

    public override void Upgrade()
    {
        if (_building.CurrentUnitsIn >= _upgradeSystemList[CurrentLevel - 1].y && CurrentLevel != MaxLevel)
        {
            _building.CurrentUnitsIn -= Convert.ToInt32(_upgradeSystemList[CurrentLevel - 1].y);
            CurrentLevel++;
            onSpawnerUpgrade?.Invoke(this, UnitStatus);
        }
    }
}
