using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using Interfaces;

public class AIBehaviour : MonoBehaviour
{

    [Header("General settings")]
    [SerializeField] private List<Building> _alliedBuildings; // belongs to AI
    [SerializeField] private List<Spawner> _alliedSpawners;
    [SerializeField] private List<Building> _enemyBuildings; // belongs to Player
    [SerializeField] private List<Spawner> _enemySpawners;

    [Header("Counter attack settings")]
    [SerializeField] private int _maxUnitsForCounterAttack;
    [SerializeField] private float _unitPercentageForCounterAttack;
    [SerializeField] private List<Building> _buildingsForCounterAttack;

    [Header("Supporting settings")]
    [SerializeField] private int _maxUnitsForSupport;
    [SerializeField] private float _unitPercentageForSupport;
    [SerializeField] private List<Building> _buildingsForSupport;

    [Header("Attack Settings")]
    [SerializeField] private float _attackCoef;
    [SerializeField] private int _generalAllyPower; // belongs to AI
    [SerializeField] private int _generalEnemyPower; // belongs to Player
    [SerializeField] private int _maxBuildingsToAttack;
    [SerializeField] private float _unitPercentageForAttack;
    [SerializeField] private List<Building> _buildingsForAttack;
    private bool timeToAttack = false;

    private void Start()
    {
        foreach (var allyBuilding in _alliedBuildings)
        {
            allyBuilding.onBuildingCaptured += HandleOnBuildingCaptured;
            allyBuilding.onNumberOfUnitsInChanged += HandleOnNumberOfUnitsChanged;
        }

        foreach (var enemyBuilding in _enemyBuildings)
        {
            enemyBuilding.onSendUnits += HandleOnSendUnits;
            enemyBuilding.onBuildingCaptured += HandleOnBuildingCaptured;
            enemyBuilding.onNumberOfUnitsInChanged += HandleOnNumberOfUnitsChanged;
        }

        foreach (var upgradable in _enemySpawners)
        {
            upgradable.onSpawnerUpgrade += HandeOnSpawnerUpgrade;
        }

        StartCoroutine(AttackCo());
    }

    private void HandleOnBuildingCaptured(Building building, UnitStatus newUnitStatus)
    {
        if (newUnitStatus == UnitStatus.Ally) // now belongs to the Player
        {
            //AI Behaviour logic
            _enemyBuildings.Add(building);
            _alliedBuildings.Remove(building);

            //event reply as a AI
            ChooseBiggestBuildingsByUnitsIn(_maxUnitsForCounterAttack, out _buildingsForCounterAttack);
            foreach (var counterBuilding in _buildingsForCounterAttack)
            {
                counterBuilding.SendUnits(building, _unitPercentageForCounterAttack);
            }
        }

        if (newUnitStatus == UnitStatus.Enemy) // now belongs to an AI
        {
            //AI Behaviour logic
            _alliedBuildings.Add(building);
            _enemyBuildings.Remove(building);

            //event reply as a AI

        }
    }

    private void HandleOnSendUnits(Building targetBuilding, UnitStatus unitSender)
    {
        if (targetBuilding.UnitStatus == UnitStatus.Enemy && unitSender != UnitStatus.Enemy)
        {
            ChooseBiggestBuildingsByUnitsIn(_maxUnitsForSupport, out _buildingsForSupport);
            if (!_buildingsForSupport.Contains(targetBuilding))
            {
                foreach (var supportBuilding in _buildingsForSupport)
                {
                    supportBuilding.SendUnits(targetBuilding, _unitPercentageForSupport);
                }
            }
        }
    }

    private void HandeOnSpawnerUpgrade(Spawner spawner, UnitStatus spawnerStatus)
    {
        if (spawnerStatus == UnitStatus.Ally)
        {
            ChooseUpgradableWithLowestLevel().Upgrade();
        }
    }

    private IUpgradable ChooseUpgradableWithLowestLevel()
    {
        IUpgradable lowestLevelUpgradable;

        if (_alliedSpawners.Count != 0)
        {
            lowestLevelUpgradable = _alliedSpawners[0];
        }
        else
        {
            return default(IUpgradable);
        }

        foreach (var _upgradable in _alliedSpawners)
        {
            if (_upgradable.CurrentLevel < lowestLevelUpgradable.CurrentLevel)
            {
                lowestLevelUpgradable = _upgradable;
            }
        }

        return lowestLevelUpgradable;
    }

    private void ChooseBiggestBuildingsByUnitsIn(int amountOfBuildings, out List<Building> outList)
    {
        outList = new List<Building>();
        for (int i = 0; i < _alliedBuildings.Count; i++)
        {
            if (outList.Count == 0)
            {
                for (int j = 0; j < amountOfBuildings; j++)
                {
                    if (_alliedBuildings.Count - 1 >= j)
                    {
                        outList.Add(_alliedBuildings[j]);
                    }
                }
            }

            Building lowest = FindBuildingWithTheLowestUnitsInCount(outList);

            if (_alliedBuildings[i].CurrentUnitsIn > lowest.CurrentUnitsIn)
            {
                if (outList.Contains(lowest))
                {
                    outList.Remove(lowest);
                }
                outList.Add(_alliedBuildings[i]);
            }
        }
    }

    private Building FindBuildingWithTheLowestUnitsInCount(List<Building> buildings)
    {
        if (buildings.Count == 0)
        {
            return null;
        }

        Building buildingWithLowestUnitsInCount = buildings[0];

        for (int i = 1; i < buildings.Count; i++)
        {
            if (buildings[i].CurrentUnitsIn < buildingWithLowestUnitsInCount.CurrentUnitsIn)
            {
                buildingWithLowestUnitsInCount = buildings[i];
            }
        }

        return buildingWithLowestUnitsInCount;
    }

    private void HandleOnNumberOfUnitsChanged(Building building)
    {
        if (building.UnitStatus == UnitStatus.Ally)
        {
            _generalEnemyPower = 0;
            foreach (var _building in _enemyBuildings)
            {
                _generalEnemyPower += _building.CurrentUnitsIn;
            }
        }
        else if (building.UnitStatus == UnitStatus.Enemy)
        {
            _generalAllyPower = 0;
            foreach (var _building in _alliedBuildings)
            {
                _generalAllyPower += _building.CurrentUnitsIn;
            }

            if (_generalAllyPower / _attackCoef >= _generalEnemyPower)
            {
                timeToAttack = true;
            }
        }
    }

    private IEnumerator AttackCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (timeToAttack)
            {
                ChooseBiggestBuildingsByUnitsIn(_maxBuildingsToAttack, out _buildingsForAttack);
                Building closest = FindClosestEnemyBuilding(_buildingsForAttack);

                foreach (var building in _buildingsForAttack)
                {
                    building.SendUnits(closest, _unitPercentageForAttack);
                }

                if (_generalAllyPower / _attackCoef < _generalEnemyPower)
                {
                    timeToAttack = false;
                }
            }
        }
    }

    private Building FindClosestEnemyBuilding(List<Building> buildings)
    {
        Vector3 avaragePosition = Vector3.zero;
        foreach (var allyBuilding in buildings)
        {
            avaragePosition += allyBuilding.transform.position;
        }
        avaragePosition = avaragePosition / buildings.Count;

        Building closestEnemy = _enemyBuildings[0];

        foreach (var enemyBuilding in _enemyBuildings)
        {
            if (Vector3.Distance(avaragePosition, enemyBuilding.transform.position) < Vector3.Distance(avaragePosition, closestEnemy.transform.position))
            {
                closestEnemy = enemyBuilding;
            }
        }

        return closestEnemy;
    }
}
