using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BubbleUI : MonoBehaviour
{
    public BubbleGun BubbleGun;
    public TextMeshProUGUI BubbleCounter;
    
    private Slider _cooldownBar;

    private void Awake()
    {
        _cooldownBar = GetComponentInChildren<Slider>();
        BubbleCounter.text = $"{BubbleGun.BubbleCount}";
        //gameObject.SetActive(false);
    }

    private void Update()
    {
        BubbleCounter.text = $"{BubbleGun.BubbleCount}";
        if (BubbleGun.CooldownTimer > 0 && BubbleGun.CooldownTimer <= BubbleGun.Cooldown)
        {
            _cooldownBar.value = BubbleGun.CooldownTimer / BubbleGun.Cooldown;

        }
        else
        {
            _cooldownBar.value = 0;
        }
    }
    
    
}
