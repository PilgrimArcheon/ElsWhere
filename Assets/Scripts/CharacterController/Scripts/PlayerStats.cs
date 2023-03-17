using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public float health;
    public float energy;
    public int ammo;

    bool isDead;

    ThirdPersonController thirdPersonController;

    //UI
    public Slider healthSlider;
    public Slider energySlider;
    public Text ammoText;

    // Start is called before the first frame update
    void Start()
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        healthSlider = GameObject.Find("HealthSlider").GetComponent<Slider>();
        energySlider = GameObject.Find("EnergySlider").GetComponent<Slider>();
        ammoText = GameObject.Find("AmmoText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.value = health/100;
        energySlider.value = energy/100;

        ammoText.text = ammo.ToString("00");

        if(health > 100) health = 100;
        if(health < 0) health = 0;
        if(energy > 100) energy = 100;
        if(energy < 0) energy = 0;
    }

    public void TakeDamage(float damage)
    {
        if(health > 0 && !isDead)
        {
            health -= damage;
            //End Game
            if(health <= 0){
                isDead = true;
                thirdPersonController.Die();
            }
            else{
                thirdPersonController.TakeDamage();
            }
        }
    }

    public void DepleteEnergy(float value)
    {
        if(energy > 0)
            energy -= value;
    }
}
