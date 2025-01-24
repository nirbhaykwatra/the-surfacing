using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGun : MonoBehaviour
{
    [SerializeField] private GameObject _bubble;
    
    [field: SerializeField] public float BubbleSpawnDistance { get; set; }
    [field: SerializeField] public float BubbleTravelDistance { get; set; }
    [field: SerializeField] public float BubbleTravelTime { get; set; }

    [field: SerializeField] public int MaxBubbleInstances { get; set; } = 5;
    
    private List<Bubble> _bubbles;
    
    private void Awake()
    {
        _bubbles = new List<Bubble>();
    }
    
    private void Update()
    {
        
    }

    public void CreateBubble()
    {
        Vector3 spawnPosition = (transform.forward * BubbleSpawnDistance) + (transform.position + Vector3.up);
        Vector3 destination = (transform.forward * (BubbleSpawnDistance + BubbleTravelDistance)) + (transform.position + Vector3.up);
        
        GameObject bubble = Instantiate(_bubble, spawnPosition, Quaternion.identity);

        if (bubble.TryGetComponent(out Bubble bubbleComponent))
        {
            _bubbles.Add(bubbleComponent);
            bubbleComponent.PushBubble(destination, Time.deltaTime * BubbleTravelTime);
        }
        
    }
    
    private void OnDrawGizmosSelected()
    {
        Vector3 spawnPosition = (transform.forward * BubbleSpawnDistance) + (transform.position + Vector3.up);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(spawnPosition,0.2f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((transform.forward * (BubbleSpawnDistance + BubbleTravelDistance)) + (transform.position + Vector3.up), 0.3f);
    }
}
