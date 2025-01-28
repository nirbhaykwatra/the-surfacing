using UnityEngine;

[CreateAssetMenu(fileName = "BubbleSettings", menuName = "Scriptables/Bubble Settings")]
public class BubbleSettings : ScriptableObject
{
    [Header("Bubble Attributes")]
    [Tooltip("How fast a bubble attaches to a liftable object")]
    [field: SerializeField] public float AttachmentSpeed { get; set; } = 0.8f;

    [Header("Bubble Attributes")]
    [Tooltip("How far a bubble is when it attaches to a liftable object")]
    [field: SerializeField]
    public float AttachmentOffset { get; set; } = 0.05f;

    [Header("Bubble Attributes")]
    [field: SerializeField]
    public float TimeToOffset { get; set; } = 2f;

    [Header("Bubble Attributes")]
    [field: SerializeField]
    public float BubbleGrowthTime { get; set; } = 0.5f;

    [Header("Bubble Attributes")]
    [field: SerializeField]
    public float Lifespan { get; set; } = 3;

    [Header("Bubble Attributes")]
    [field: SerializeField]
    public float Buoyancy { get; set; } = 1f;

    [Header("Bubble Attributes")]
    [field: SerializeField]
    public float BubbleScaleMultiplier { get; set; } = 1f;

}
