using FMODUnity;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class Bubble : MonoBehaviour
{
    
    [Header("Bubble Attributes")]
    [field: SerializeField] public float Lifespan { get; set; }
    
    [Header("Bubble Attributes")]
    [field: SerializeField] public float Buoyancy { get; set; }

    [Header("Bubble Attributes")]
    [field: SerializeField]
    public float BubbleScaleMultiplier { get; set; } = 1f;
    
    [field: SerializeField] public bool Rise { get; set; }
    [field: SerializeField] public bool InCurrent { get; set; }

    [Header("Audio")]
    [field: SerializeField] public EventReference _bubblePop;
    
    public Rigidbody Rigidbody;
    public BoxCollider _collider;
    private bool _hasPopped;
    private bool _isAttached;
    private bool _hasTraveled;
    private bool _hasLeftCurrent;
    private float _lifeSpanTimer;
    private BubbleGun _bubbleGun;
    private Vector3 _previousPosition = new Vector3();
    private Vector3 _currentPosition = new Vector3();
    private Vector3 _kinematicVelocity = new Vector3();
    
    public BubbleSettings _ctx;
    public UnityEvent OnDestroyed;
    
    public bool HasLeftCurrent { get => _hasLeftCurrent; set => _hasLeftCurrent = value; }
    
    public Vector3 Velocity => _kinematicVelocity;

    private void OnEnable()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _bubbleGun = FindAnyObjectByType<BubbleGun>();
        if (Lifespan == 0) Lifespan = _ctx.Lifespan;
        if (BubbleScaleMultiplier == 0) BubbleScaleMultiplier = _ctx.BubbleScaleMultiplier;
        if (Buoyancy == 0) Buoyancy = _ctx.Buoyancy;
        transform.localScale *= BubbleScaleMultiplier;
        Rise = false;
        InCurrent = false;
        _hasPopped = false;
        _isAttached = false;
        _hasTraveled = false;
        _hasLeftCurrent = false;
    }

    private void FixedUpdate()
    {
        if (!InCurrent)
        {
            if (_hasLeftCurrent)
            {
                Rise = false;

                if (Rigidbody.isKinematic)
                {
                    Debug.Log($"_hasLeftCurrent: {_hasLeftCurrent}");
                    /*Vector3 targetPosition = transform.position + new Vector3(0f, 0f, 5f);
                    transform.position = Vector3.Lerp(transform.position, targetPosition, Time.fixedDeltaTime * 0.01f);

                    /*if (Vector3.Distance(transform.position, targetPosition) < 0.05f)
                    {
                        Rise = true;
                        _hasLeftCurrent = false;
                    }*/
                }
                else
                {
                    Rigidbody.linearVelocity = Vector3.Lerp(Rigidbody.linearVelocity, Vector3.zero, Time.fixedDeltaTime * _ctx.TimeToOffset);

                    if (Rigidbody.linearVelocity.magnitude < 0.05f)
                    {
                        Rise = true;
                        _hasLeftCurrent = false;
                    }
                }
            }
            
            if (!_hasPopped && Rise)
            {
                Vector3 nextPosition = transform.position + new Vector3(0f, Time.fixedDeltaTime * Buoyancy, 0f);

                if (Rigidbody.isKinematic)
                {
                    Rigidbody.MovePosition(nextPosition);
                }
                else
                {
                    Rigidbody.Move(nextPosition, Quaternion.identity);
                }
            }
        }
    }

    private void Update()
    {
        _currentPosition = transform.position;
        if (!_hasPopped)
        {
            if (InCurrent)
            {
                StopCoroutine(MoveBubble(transform.position, 0.1f));
            }

            if (_hasTraveled)
            {
                _lifeSpanTimer += Time.deltaTime;
                if (_lifeSpanTimer >= Lifespan)
                {
                    _hasPopped = true;
                }
            }

            if (Rigidbody.isKinematic)
            {
                _kinematicVelocity = (_currentPosition - _previousPosition) / Time.deltaTime;
            }
        }
        else
        {
            _lifeSpanTimer = 0;
            _isAttached = false;
            if (_bubbleGun != null)
            {
                if (_bubbleGun._bubbles.Contains(this))
                {
                    _bubbleGun._bubbles.Remove(this);
                }
            }
            OnDestroyed?.Invoke();
            Destroy(gameObject);
        }
    }

    private void LateUpdate()
    {
        _previousPosition = _currentPosition;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CharacterMovement3D character))
        {
            Rigidbody.isKinematic = true;
            return;
        }
        
        if (collision.gameObject.TryGetComponent(out Liftable liftable))
        {
            _collider.isTrigger = true;
            return;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Abrasive"))
        {
            _hasPopped = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Rigidbody.isKinematic = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Liftable liftable))
        {
            StartCoroutine(GrowBubble(transform.localScale.magnitude, liftable.transform.lossyScale.magnitude));
            liftable.Bubble = this;
        }
       
        if (other.gameObject.layer == LayerMask.NameToLayer("Abrasive"))
        {
            _hasPopped = true;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Current"))
        {
           
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Liftable liftable))
        {
            liftable.Bubble = null;
            _collider.isTrigger = false;
        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("Current"))
        {
            
        }
    }
    
    

    public void PushBubble(Vector3 destination, float time)
    {
        StartCoroutine(MoveBubble(destination, time));
    }

    private IEnumerator MoveBubble(Vector3 destination, float time)
    {
        while (Vector3.Distance(transform.position, destination) > _ctx.AttachmentOffset)
        {
            if (Rigidbody.isKinematic)
            {
                Rigidbody.MovePosition(Vector3.Lerp(transform.position, destination, time));
            }
            else
            {
                Rigidbody.Move(Vector3.Lerp(transform.position, destination, time), Quaternion.identity);
            }
            yield return null;
        }
        _hasTraveled = true;
        Rise = true;
        yield break;
    }

    private IEnumerator GrowBubble(float startingScale, float targetScale)
    {
        while (transform.lossyScale.magnitude < targetScale * 2)
        {
            transform.localScale += new Vector3(startingScale, startingScale, startingScale) * (_ctx.BubbleGrowthTime * Time.deltaTime);
            yield return null;
        }
    }

    public void BubblePop()
    {
        AudioManager.instance.PlayOneShot(_bubblePop, transform.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, _collider.size);
    }
}
