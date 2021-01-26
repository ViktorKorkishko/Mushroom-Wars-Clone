using Enums;
using Interfaces;
using UnityEngine;

public abstract class Unit : MonoBehaviour, IHaveStatus
{
    public virtual UnitStatus UnitStatus
    {
        get
        {
            return _unitStatus;
        }
        set
        {
            _unitStatus = value;
        }
    }

    public virtual Vector3 TargetPosition
    {
        get
        {
            return _targetPosition;
        }
    }
    
    protected UnitStatus _unitStatus;
    
    protected Vector3 _targetPosition;

    protected Rigidbody2D _rb;

    public delegate void OnUnitDeath(Unit unit);
    public OnUnitDeath onUnitDeath;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetTarget(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
}
