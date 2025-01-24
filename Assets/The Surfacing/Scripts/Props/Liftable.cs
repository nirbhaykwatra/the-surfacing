using System.Collections;
using UnityEngine;

public class Liftable : MonoBehaviour
{
    public bool BubbleAttached { get; set; }
    
    private Rigidbody _rb;
    private BoxCollider _collider;
    private Bubble _bubble;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        BubbleAttached = false;
    }

    private void FixedUpdate()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (!BubbleAttached)
        {
            
        }
        else
        {
            
        }
    }

    public void AttachToBubble(Bubble bubble)
    {
        Debug.Log("Attaching to bubble");
        _bubble = bubble;
        BubbleAttached = true;
        _rb.isKinematic = true;
        _rb.useGravity = false;
        transform.parent = bubble.transform;
        StartCoroutine(Lift(bubble));
    }

    private IEnumerator Lift(Bubble bubble)
    {
        while (BubbleAttached)
        {
            Debug.Log("Lifting bubble");
            transform.position = bubble.transform.position;
            yield return null;
        }
        DetachFromBubble(bubble);
    }

    public void DetachFromBubble(Bubble bubble)
    {
        Debug.Log("Detaching from bubble");
        _bubble = null;
        BubbleAttached = false;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        transform.parent = null;
    }
}
