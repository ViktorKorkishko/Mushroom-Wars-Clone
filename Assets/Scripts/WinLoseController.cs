using Enums;
using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinLoseController : MonoBehaviour
{
    [SerializeField] private List<Spawner> _spawners;
    [SerializeField] private List<Unit> _units;

    public delegate void OnGameEnd(UnitStatus unitStatus);
    public OnGameEnd onGameEnd;

    private void Awake()
    {
        foreach (var spawner in _spawners)
        {
            spawner.onUnitSpawn += HandleOnUnitSpawn;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckWinning();
        }
    }

    private void HandleOnUnitSpawn(Spawner spawner, Unit unit)
    {
        _units.Add(unit);
        unit.onUnitDeath += HandleOnUnitDeath;
    }

    private void HandleOnUnitDeath(Unit unit)
    {
        unit.onUnitDeath -= HandleOnUnitDeath;
        _units.Remove(unit);
        CheckWinning();
    }

    private void CheckWinning()
    {
        if (_spawners.TrueForAll(AreAllSpawnersHaveTheSameUnitStatus) && 
            _units.TrueForAll(AreAllUnitsHaveTheSameUnitStatus))
        {
            onGameEnd?.Invoke(_spawners[0].UnitStatus);
            Time.timeScale = 0;
        }
    }

    private bool AreAllSpawnersHaveTheSameUnitStatus(IHaveStatus haveStatus)
    {
        if (_spawners.Count == 0)
        {
            return false;
        }
        if (_spawners[0].UnitStatus == haveStatus.UnitStatus)
        {
            return true;
        }
        return false;
    }

    private bool AreAllUnitsHaveTheSameUnitStatus(IHaveStatus haveStatus)
    {
        if (_units.Count == 0)
        {
            return true;
        }
        else
        { 
            if (_units[0].UnitStatus == haveStatus.UnitStatus)
            {
                return true;
            }
            return false;
        }
    }
}
