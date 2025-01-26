using UnityEngine;
using UnityEditor.Events;
using UnityEngine.Events;

public class EndLevelTrigger : MonoBehaviour
{
    public UnityEvent GameEnd;

    private void OnTriggerEnter(Collider other)
    {
        if(TryGetComponent<PlayerController>(out PlayerController controller))
        {
            GameEnd.Invoke();
            Debug.Log("Player is here");
        }
    }
}
