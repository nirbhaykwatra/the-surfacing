using UnityEngine;

public class BubbleGun : MonoBehaviour
{
    [SerializeField] private GameObject _bubble;
    [field: SerializeField] private float BubbleShootForce { get; set; }
    
    private void Start()
    {
        
    }
    
    private void FixedUpdate()
    {
        
    }

    public void CreateBubble(Vector3 position)
    {
        GameObject bubble = Instantiate(_bubble, position, Quaternion.identity);
        
        if (bubble.TryGetComponent<Bubble>(out Bubble outBubble)) outBubble.PushBubble(position, BubbleShootForce);
    }
}
