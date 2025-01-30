using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BubbleGun : MonoBehaviour
{
    [SerializeField] private GameObject _bubble;
    
    [Header("Spawn Settings")]
    [field: SerializeField] public float SpawnDistance { get; set; }
    
    [Header("Spawn Settings")]
    [field: SerializeField] public float OffsetDistance { get; set; }
    
    [field: SerializeField] private float Cooldown { get; set; } = 1f;
    [field: SerializeField] public int MaxBubbleInstances { get; set; } = 5;
    
    [Tooltip("If false, bubbles cannot be created until all player-created bubbles have popped.")]
    [field: SerializeField] public bool ConsecutiveBubbleSpawning { get; set; }
    
    [Header("Bubble Attributes")]
    [field: SerializeField] public float Lifespan { get; set; }
    
    [Header("Bubble Attributes")]
    [field: SerializeField] public float Buoyancy { get; set; }
    
    [Header("Bubble Attributes")]
    [field: SerializeField] public float BubbleScaleMultiplier { get; set; }

    [Header("Audio")]
    [field: SerializeField] public EventReference _gunShoot;
    
    public List<Bubble> _bubbles;
    private bool _canShoot;
    private float _shootTimer;
    
    public BubbleSettings _ctx;
    
    private void Awake()
    {
        _bubbles = new List<Bubble>();
        
        if (Lifespan == 0) Lifespan = _ctx.Lifespan;
        if (BubbleScaleMultiplier == 0) BubbleScaleMultiplier = _ctx.BubbleScaleMultiplier;
        if (Buoyancy == 0) Buoyancy = _ctx.Buoyancy;
        
        _canShoot = true;
        _shootTimer = 0;
    }
    
    private void Update()
    {
        if (MaxBubbleInstances != 0)
        {
            if (_bubbles.Count - 1 >= MaxBubbleInstances)
            {
                if (_bubbles[0] != null)
                {
                    Destroy(_bubbles[0].gameObject);
                    _bubbles.RemoveAt(0);
                }
            }
        }
        else
        {
            return;
        }
    }

    public void CreateBubble()
    {
        if (MaxBubbleInstances != 0)
        {
            if (ConsecutiveBubbleSpawning)
            {
                if (_bubbles.Count >= MaxBubbleInstances) return;
            }
            else
            {
                if (_bubbles.Count >= MaxBubbleInstances) StartCoroutine(WaitForBubbles());
            }
            if (!_canShoot) return;
            
            SpawnBubble();
        }
        else
        {
            SpawnBubble();
        }
    }

    private void SpawnBubble()
    {
        Vector3 spawnPosition = (transform.forward * SpawnDistance) + (transform.position + Vector3.up);
        Vector3 destination = (transform.forward * (SpawnDistance + OffsetDistance)) + (transform.position + Vector3.up);

        AudioManager.instance.PlayOneShot(_gunShoot, transform.position);
        
        GameObject bubble = Instantiate(_bubble, spawnPosition, Quaternion.identity);

        if (bubble.TryGetComponent(out Bubble bubbleComponent))
        {
            bubbleComponent.BubbleScaleMultiplier = BubbleScaleMultiplier;
            bubbleComponent.Lifespan = Lifespan;
            bubbleComponent.Buoyancy = Buoyancy;
            _bubbles.Add(bubbleComponent);
            bubbleComponent.PushBubble(destination, Time.deltaTime * _ctx.TimeToOffset);
        }
    }

    private IEnumerator WaitForBubbles()
    {
        _canShoot = false;
        yield return new WaitForSeconds(Cooldown);
        _canShoot = true;
    }
    
    private void OnDrawGizmosSelected()
    {
        Vector3 spawnPosition = (transform.forward * SpawnDistance) + (transform.position + Vector3.up);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(spawnPosition, (transform.forward * (SpawnDistance + OffsetDistance)) + (transform.position + Vector3.up));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPosition,0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((transform.forward * (SpawnDistance + OffsetDistance)) + (transform.position + Vector3.up), 0.3f);
    }
}
