using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    PlayerStats stats;
    float maxHealth;
    float damageValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        stats = FindObjectOfType<PlayerStats>();
        maxHealth = stats.health;
    }

    // Update is called once per frame
    void Update()
    {
        float health = stats.health;
        canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, damageValue, 20 * Time.deltaTime);
        if(health <= 50f)
            damageValue = 1f - (health/maxHealth);
        else
            damageValue = 0f;
    }
}
