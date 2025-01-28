using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ValidateCheckpoint : MonoBehaviour
{
    public List<Vector3> SpawnLocations = new List<Vector3>();
    public CheckpointData CheckpointDataObject;
    
    private Dictionary<int, Vector3> checkpoints = new Dictionary<int, Vector3>();

    private void Awake()
    {
        switch (CheckpointDataObject.CheckpointID)
        {
            case 0:
                transform.position = SpawnLocations[0];
                break;
            case 1:
                transform.position = SpawnLocations[1];
                break;
            case 2:
                transform.position = SpawnLocations[2];
                break;
            case 3:
                transform.position = SpawnLocations[3];
                break;
            case 4:
                transform.position = SpawnLocations[4];
                break;
            default:
                transform.position = SpawnLocations[0];
                break;
        }
    }
}
