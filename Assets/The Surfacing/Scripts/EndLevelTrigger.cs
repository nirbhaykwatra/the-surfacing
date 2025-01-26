using UnityEngine;
using UnityEditor.Events;
using UnityEngine.Events;

public class EndLevelTrigger : MonoBehaviour
{
    public UnityEvent GameEnd;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponentInChildren<PlayerController>() != null)
        {
            GameEnd.Invoke();
            Debug.Log("Player is here");
        }
    }
}
