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
    
    private void Awake()
    {
        
    }
    
    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Bubble>(out Bubble bubble))
        {
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<Bubble>(out Bubble bubble))
        {
            if (bubble.Rigidbody.isKinematic)
            {
                bubble.gameObject.transform.position += new Vector3(0f, Time.fixedDeltaTime * bubble.RiseSpeed, 0f);
                //bubble.Rigidbody.MovePosition(nextPosition);
                if (Mathf.Abs(bubble.gameObject.transform.position.y - transform.position.y) < 0.1f)
                {
                    Vector3 sidePosition = bubble.gameObject.transform.position + new Vector3(0f, 0f, Time.fixedDeltaTime * WaterCurrentStrength);
                    bubble.Rigidbody.MovePosition(sidePosition);
                }
            }
            else
            {
                if (Mathf.Abs(bubble.gameObject.transform.position.y - transform.position.y) < 0.1f)
                {
                    bubble.InCurrent = true;
                    bubble.Rigidbody.AddForce(new Vector3(0f, Buoyancy, WaterCurrentStrength), ForceMode.Force);
                
                }
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Bubble>(out Bubble bubble))
        {
            bubble.HasLeftCurrent = true;
            bubble.InCurrent = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, transform.lossyScale);
    }
}
