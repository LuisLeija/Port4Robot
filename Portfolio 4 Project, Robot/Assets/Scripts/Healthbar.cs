using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthBar;

    public PlayerMovement player;
   // public static Healthbar instance;

   // float maxHealth = 0;
    float currHealth;


    private void Awake()
    {
        //maxHealth = player.health;
        //instance = this;
    }

    private void Update()
    {
        healthBar.value = player.currHealth / player.health;
    }
}
