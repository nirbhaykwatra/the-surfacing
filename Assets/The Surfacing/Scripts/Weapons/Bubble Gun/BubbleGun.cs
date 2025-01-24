using System.Collections;
using UnityEngine;

public class BubbleGun : MonoBehaviour
{
    [SerializeField] private GameObject _bubble;
    
    [field: SerializeField] public float BubbleSpawnDistance { get; set; }
    [field: SerializeField] public float BubbleTravelDistance { get; set; }
    [field: SerializeField] public float BubbleTravelTime { get; set; }
    
    private void Start()
    {
        
    }
    
    private void FixedUpdate()
    {
        
    }

    public void CreateBubble()
    {
        Vector3 spawnPosition = (transform.forward * BubbleSpawnDistance) + (transform.position + Vector3.up);
        Vector3 destination = (transform.forward * (BubbleSpawnDistance + BubbleTravelDistance)) + (transform.position + Vector3.up);
        
        GameObject bubble = Instantiate(_bubble, spawnPosition, Quaternion.identity);
        
        if (bubble.TryGetComponent(out Bubble bubbleComponent)) bubbleComponent.PushBubble(destination, Time.deltaTime * BubbleTravelTime);

        //StartCoroutine(MoveBubble(bubble, bubble.transform.position, destination, Time.deltaTime * BubbleTravelTime));
    }

    private IEnumerator MoveBubble(GameObject bubble, Vector3 spawnPosition, Vector3 destination, float time)
    {
        if (Vector3.Distance(bubble.transform.position, destination) > 0.05f)
        {
            bubble.transform.position = Vector3.Lerp(bubble.transform.position, destination, time);
            yield return null;
        }

        //if (bubble.TryGetComponent(out Bubble outBubble)) outBubble.Rise = true;
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
