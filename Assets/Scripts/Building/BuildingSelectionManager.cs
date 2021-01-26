using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildingSelectionManager : MonoBehaviour
{
    private Building _targetBuilding;
    private List<SelectableBuilding> _selectedObjects;

    [Header("Double Click settings")]
    [SerializeField] private float _doubleClickTime;

    private float _lastClickTime;
    private float _timeSinceLastClick;

    private void Awake()
    {

    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (Input.GetButtonDown("ChooseBuilding"))
        {
            _timeSinceLastClick = Time.time - _lastClickTime;
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero))
            {
                var selectedObject = hit.collider.gameObject.GetComponent<SelectableBuilding>();
                if (selectedObject)
                {
                    // upgrade with double mouse click
                    if (_timeSinceLastClick <= _doubleClickTime)
                    {
                        var upgradable = selectedObject.GetComponent<IUpgradable>();
                        if (upgradable != null)
                        {
                            upgradable.Upgrade();
                        }
                        selectedObject.OnDeselect();
                        _selectedObjects.Remove(selectedObject);
                    }
                    // choose if not a double mouse click
                    else if (!_selectedObjects.Contains(selectedObject))
                    {
                        _selectedObjects.Add(selectedObject);
                        selectedObject.OnSelect();
                    }
                    // deselect all if clicked not on a selectableObject
                    else
                    {
                        selectedObject.OnDeselect();
                        _selectedObjects.Remove(selectedObject);
                    }
                }
            }
            else
            {
                foreach (var selectedObject in _selectedObjects)
                {
                    selectedObject.OnDeselect();
                }
                _selectedObjects.Clear();
            }

            _lastClickTime = Time.time;
        }

        if (Input.GetButtonDown("Attack"))
        {
            RaycastHit2D hit;
            if (hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero))
            {
                var selectableObject = hit.collider.gameObject.GetComponent<SelectableBuilding>();
                if (selectableObject)
                {
                    _targetBuilding = hit.collider.gameObject.GetComponent<Building>();
                    if (_targetBuilding)
                    {
                        foreach (var selectedObject in _selectedObjects)
                        {
                            if (selectableObject == selectedObject)
                            {
                                continue;
                            }
                            selectedObject.GetComponent<Building>().SendUnits(_targetBuilding, GameSettings.UnitDelimitingCoef);
                            selectedObject.OnDeselect();
                        }
                        _selectedObjects.Clear();
                    }
                }
            }
        }
    }
}

