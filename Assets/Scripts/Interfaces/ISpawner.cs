using Enums;
using UnityEngine;

namespace Interfaces
{
    public interface ISpawner : IUpgradable
    {
        float UnitProduceSpeed { get; }
        void Spawn();
    }

    public interface IWaveSpawner : ISpawner
    {
        Vector3 TargetPostion { set; }
    }
}