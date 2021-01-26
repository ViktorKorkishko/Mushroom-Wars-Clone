using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCanvasController : MonoBehaviour
{
    [SerializeField] private List<GameObject> _canvases;
    private bool isEnabled = true;

    public void SwitchNumbersVisability()
    {
        foreach (GameObject canvas in _canvases)
        {
            canvas.gameObject.SetActive(!isEnabled);
        }
        isEnabled = !isEnabled;
    }
}
