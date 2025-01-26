using System.Collections.Generic;
using UnityEngine;

public class ResetCheckpoints : MonoBehaviour
{
    public CheckpointData CheckpointDataObject;
    private List<Checkpoint> _checkpoints;

    private void Awake()
    {
        _checkpoints = new List<Checkpoint>();
        foreach (Checkpoint checkpoint in FindObjectsByType<Checkpoint>(FindObjectsSortMode.None))
        {
            _checkpoints.Add(checkpoint);
        }
    }

    public void DoResetCheckpoints()
    {
        CheckpointDataObject.CheckpointID = 0;
    }

    public void IncreaseCheckpoint()
    {
        if (CheckpointDataObject.CheckpointID <= _checkpoints.Count - 1)
        {
            CheckpointDataObject.CheckpointID++;
        }
    }
    
    public void DecreaseCheckpoint()
    {
        if (CheckpointDataObject.CheckpointID > 0)
        {
            CheckpointDataObject.CheckpointID--;
        }
    }
}
