using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hud : MonoBehaviour {

    public GameObject world;
    public GameObject player;

    public Slider staminaBar;
    public Slider energyBar;
    public Slider healthBar;

    void Start() {
        
    }

    void Update() {
        staminaBar.value = Mathf.Lerp(staminaBar.value, player.GetComponent<player_logic>().stamina, 0.1f);
        energyBar.value = Mathf.Lerp(energyBar.value, player.GetComponent<player_logic>().energy, 0.1f);
        healthBar.value = Mathf.Lerp(healthBar.value, player.GetComponent<player_logic>().health, 0.1f);
    }
}
