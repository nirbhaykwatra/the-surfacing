using System;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    
    [Header("Spawn Settings")]
    [field: SerializeField] private float SpawnInterval { get; set; }
    
    [Header("Spawn Settings")]
    [field: SerializeField] private float SpawnDelay { get; set; }

    [Header("Bubble Attributes")]
    [field: SerializeField]
    public float Lifespan { get; set; } = 1f;
    
    [Header("Bubble Attributes")]
    [field: SerializeField] public float Buoyancy { get; set; } = 1f;

    [Header("Bubble Attributes")]
    [field: SerializeField]
    public float BubbleScaleMultiplier { get; set; } = 1f;

    private float _timer;
    private bool _canSpawn;
    
    public BubbleSettings _ctx;

    private void Awake()
    {
        _timer = 0;
        _canSpawn = false;
        
        if (Lifespan == 0) Lifespan = _ctx.Lifespan;
        if (BubbleScaleMultiplier == 0) BubbleScaleMultiplier = _ctx.BubbleScaleMultiplier;
        if (Buoyancy == 0) Buoyancy = _ctx.Buoyancy;
    }

    private void Update()
    {
        _timer += Time.deltaTime;

        if (!_canSpawn)
        {
            if (_timer >= SpawnDelay)
            {
                _canSpawn = true;
            }
        }

        if (_timer >= SpawnInterval && _canSpawn)
        {
            GameObject bubbleObject = Instantiate(bubblePrefab, transform.position, Quaternion.identity);

            if (bubbleObject != null)
            {
                if (bubbleObject.TryGetComponent(out Bubble bubble))
                {
                    bubble.Lifespan = Lifespan;
                    bubble.Buoyancy = Buoyancy;
                    bubble.BubbleScaleMultiplier = BubbleScaleMultiplier;
                    bubble.Rise = true;
                    Debug.Log($"Spawning bubble with lifespan {bubble.Lifespan}");
                }
            }
            _timer = 0;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
