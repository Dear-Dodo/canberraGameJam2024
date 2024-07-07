using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image Fill;
    public Image OverFill;

    public void SetHealth(float health, float maxhealth)
    {
        Fill.fillAmount = Mathf.Clamp01(health / maxhealth);
        OverFill.fillAmount = Mathf.Clamp01((health - maxhealth) / maxhealth);
    }
}
