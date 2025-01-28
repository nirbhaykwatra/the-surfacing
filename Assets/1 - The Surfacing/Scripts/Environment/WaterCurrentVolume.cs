using System;
using System.Collections;
using UnityEngine;

public class WaterCurrentVolume : MonoBehaviour
{
    public enum WaterCurrentAxis
    {
        Right,
        Left,
        Up,
    }

    [field: SerializeField] private float WaterCurrentStrength { get; set; } = 3;
    [field: SerializeField] private float Buoyancy { get; set; } = 0.2f;
    
    private BoxCollider _collider;
    public WaterCurrentAxis _axis;

    private void OnTriggerEnter(Collider other)
    {
        // TODO: Implement behaviour for intersecting water currents. 
        //  One way to do it is to add the current volume to a list in order of collision.
        //  Basically, the one the bubble hit first gets precedence, then when the bubble intersects with the
        //  second one, the velocity reduces and the first volume stops pushing the bubble, the second volume
        //  takes over pushing and so on. The list is cleared any time the bubble escapes any water current.
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Bubble bubble))
        {
            bubble.transform.SetParent(transform);
            if (bubble.Rigidbody.isKinematic)
            {
                if (Mathf.Abs(bubble.gameObject.transform.position.y - transform.position.y) > 0.1f)
                {
                    bubble.InCurrent = true;
                    bubble.gameObject.transform.localPosition += new Vector3(0f, Time.deltaTime * bubble.Buoyancy, 0f);
                }
                else
                {
                    bubble.gameObject.transform.localPosition += new Vector3(0f, 0f, Time.deltaTime * WaterCurrentStrength);
                }
            }
            else
            {
                
                if (Mathf.Abs(bubble.gameObject.transform.position.y - transform.position.y) < 0.5f)
                {
                    bubble.InCurrent = true;
                    bubble.Rigidbody.AddRelativeForce(new Vector3(0f, Buoyancy, 0f) + transform.forward * WaterCurrentStrength, ForceMode.Force);
                
                }
                else
                {
                    
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Bubble bubble))
        {
            // TODO: Figure out why the volume calls OnTriggerExit only when the character is standing on
            //  a bubble. Try Physics.OverlapSphere.
            
            // TODO: Currently, if two currents are intersecting, OnTriggerExit is called from both,
            //  meaning that even if the bubble is still overlapping (maybe use Physics.OverlapSphere) a current,
            //  it stops dead when it exits one of them and cannot be influenced by the next current.
            Debug.Log($"Exited water current volume");
            bubble.transform.SetParent(null);
            bubble.HasLeftCurrent = true;
            bubble.InCurrent = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        
    }
}
