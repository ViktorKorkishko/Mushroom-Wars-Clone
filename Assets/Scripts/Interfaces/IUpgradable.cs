using Enums;

namespace Interfaces
{
    public interface IUpgradable
    {
        int CurrentLevel { get; }
        int DefaultLevel { get; }
        int MinLevel { get; }
        int MaxLevel { get; }
        int UpgradeCost { get; }

        void Upgrade();
    }
}