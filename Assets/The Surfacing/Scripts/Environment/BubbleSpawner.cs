using System;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    
    [field: SerializeField] private float SpawnInterval { get; set; }

    [Header("Bubble Spawn Settings")]
    [field: SerializeField] private float BubbleLifespan { get; set; } = 10;

    [Header("Bubble Spawn Settings")]
    [field: SerializeField] private float BubbleRiseSpeed { get; set; } = 0.7f;

    private float timer;

    private void Awake()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= SpawnInterval)
        {
            GameObject bubbleObject = Instantiate(bubblePrefab, transform.position, Quaternion.identity);

            if (bubbleObject != null)
            {
                if (bubbleObject.TryGetComponent(out Bubble bubble))
                {
                    bubble.Lifespan = BubbleLifespan;
                    bubble.RiseSpeed = BubbleRiseSpeed;
                    bubble.Rise = true;
                }
            }
            timer = 0;
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 0.2f);
    }
}
