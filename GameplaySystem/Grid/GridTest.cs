using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTest : MonoBehaviour
{
    [SerializeField] private GameObject gridObject;
    private GridSystem _gridSystem;

    private void Start()
    {
        _gridSystem = new GridSystem(5, 5, 1f, Vector3.zero);
        _gridSystem.InitializeGrid(typeof(int), false);
        _gridSystem.ShowDebug = true;
    }
}
