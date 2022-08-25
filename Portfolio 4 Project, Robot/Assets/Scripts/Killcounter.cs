using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Killcounter : MonoBehaviour
{
    public Text counter;
    int kills;

    // Update is called once per frame
    void Update()
    {
        ShowKills();
        if (kills == 50)
        {
            SceneManager.LoadScene("WinScreen");
            print("weener");
        }
    }

    public void ShowKills()
    {
        counter.text = kills.ToString();
    }
    public void AddKill()
    {
        kills++;
    }
}
