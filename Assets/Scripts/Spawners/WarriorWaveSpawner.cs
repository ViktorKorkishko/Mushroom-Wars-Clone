using System;
using System.Collections;
using System.Collections.Generic;
using Enums;
using Interfaces;
using UnityEngine;

public class WarriorWaveSpawner : UnitWaveSpawner, IUpgradable
{
    protected int UnitIndexInWave
    {
        get
        {
            return _unitIndexInWave;
        }
        set
        {
            if (value > _unitsToSpawnPerWave / 2 || value < _unitsToSpawnPerWave / -2)
            {
                _unitIndexInWave = 0;
            }
            else
            {
                _unitIndexInWave = value;
            }
        }
    }

    private int _unitIndexInWave;

    private void Awake()
    {
        CheckDefaultUpdateSettings();
        CurrentLevel = _defaultLevel;

        _building = GetComponent<Building>();
        _unitIndexInWave = -_unitsToSpawnPerWave / 2;
    }

    private void CheckDefaultUpdateSettings()
    {
        MinLevel = _minLevel;
        DefaultLevel = _defaultLevel;
        CurrentLevel = _currentLevel;
    }

    protected override void SpawnWave(Vector3 targetPostion)
    {
        TargetPostion = targetPostion;
        StartCoroutine(SpawnWaveCo());
    }

    //TODO : переписать так, чтобы начинались спавниться от середины к краям,
    // а не слева направо
    protected virtual Vector3 CalculateWarriorSpawnPositionInWave()
    {
        // unit position in wave formula for odd numberOfUnitsInWave
        if (_unitsToSpawnPerWave % 2 == 1)
        {
            return (TargetPostion - gameObject.transform.position).normalized * _spawnDistance +
                   new Vector3(0, _unitIndexInWave * 1.5f * _unitPrefab.transform.localScale.x * _distanceBetweenUnitsInWaveCoef, 0);
        }
        else // if _unitsToSpawnPerWave % 2 == 0
        {
            if (_unitIndexInWave == 0)
            {
                _unitIndexInWave++;
            }

            if (_unitIndexInWave < 0)
            {
                return (TargetPostion - gameObject.transform.position).normalized * _spawnDistance +
                       new Vector3(0, (_unitIndexInWave * 1.5f + 0.75f) * _unitPrefab.transform.localScale.x * _distanceBetweenUnitsInWaveCoef, 0);
            }
            else
            {
                return (TargetPostion - gameObject.transform.position).normalized * _spawnDistance +
                       new Vector3(0, (_unitIndexInWave * 1.5f - 0.75f) * _unitPrefab.transform.localScale.x * _distanceBetweenUnitsInWaveCoef, 0);
            }
        }
    }

    protected virtual void WarriorSetup()
    {
        var go = Instantiate(_unitPrefab, Vector3.zero, Quaternion.identity, transform);

        Unit unit = go.GetComponent<Unit>();
        unit.UnitStatus = UnitStatus;
        unit.SetTarget(TargetPostion);
        unit.transform.localPosition = CalculateWarriorSpawnPositionInWave();
        onUnitSpawn?.Invoke(this, unit);
    }

    private IEnumerator SpawnWaveCo()
    {
        float unitsToSpawn = _building.CurrentUnitsIn * GameSettings.UnitDelimitingCoef;

        while (unitsToSpawn > 0)
        {
            UnitIndexInWave = -_unitsToSpawnPerWave / 2;

            bool flag = false;
            float temp = _unitsToSpawnPerWave;

            if (_building.CurrentUnitsIn <= 0)
            {
                yield break;
            }

            if (_unitsToSpawnPerWave > unitsToSpawn)
            {
                _unitsToSpawnPerWave = Convert.ToInt32(unitsToSpawn);
                flag = true;
            }

            for (int i = 0; i < _unitsToSpawnPerWave; i++)
            {
                WarriorSetup();

                UnitIndexInWave++;
                _building.CurrentUnitsIn--;
                unitsToSpawn--;
            }

            if (flag)
            {
                flag = false;
                _unitsToSpawnPerWave = Convert.ToInt32(temp);
                break;
            }

            yield return new WaitForSeconds(_timeToSpawnBetweenWaves);
        }
    }
}
