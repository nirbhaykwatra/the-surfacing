using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int CheckpointID;
    public CheckpointData checkpointDataObject;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Character"))
        {
            checkpointDataObject.CheckpointID = CheckpointID;
        }
    }
}
