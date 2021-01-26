using Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : Unit
{
    public override UnitStatus UnitStatus
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

    public virtual float Damage
    {
        get
        {
            return _damage / GameSettings.DefenceCoefficient;
        }
        private set
        {
            _damage = value;
        }
    }

    [SerializeField] protected float _damage;
    [SerializeField] protected float _speed;

    private void Start()
    {
        if(TryGetComponent(out ImageController ic))
        {
            ic.ChangeColor(UnitStatus);
        }
    }

    private void FixedUpdate()
    {
        MoveTo(_targetPosition);
    }

    protected void MoveTo(Vector3 targetPosition)
    {
        if (targetPosition != null)
        {
            _rb.MovePosition(transform.position + (targetPosition - transform.position).normalized * _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.GetComponent<Building>() && other.gameObject.transform.position == _targetPosition)
        {
            onUnitDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
