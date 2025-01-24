using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Liftable : MonoBehaviour
{
    public bool BubbleAttached { get; set; }
    
    private Rigidbody _rb;
    public Rigidbody LiftableRb { get { return _rb; } set { _rb = value; } }
    public Bubble _bubble;
    public BoxCollider _boxCollider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _boxCollider = GetComponent<BoxCollider>();
        BubbleAttached = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_bubble == null)
        {
            BubbleAttached = false;
            _rb.isKinematic = false;
            _rb.useGravity = true;
            transform.parent = null;
            _boxCollider.isTrigger = false;
        }
    }
}
