using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Liftable : MonoBehaviour
{
    public bool BubbleAttached { get; set; }
    public Rigidbody Rigidbody { get; set; }
    public Bubble Bubble;
    public BoxCollider Collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<BoxCollider>();
        BubbleAttached = false;
    }

    private void FixedUpdate()
    {
        if (Bubble)
        {
            Rigidbody.useGravity = false;
            Rigidbody.Move(Bubble.gameObject.transform.position, Quaternion.identity);
        }
        else
        {
            Rigidbody.useGravity = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
