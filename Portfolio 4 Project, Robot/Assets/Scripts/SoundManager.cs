using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static AudioClip shoot, dieR, dieM;
    static AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        shoot = Resources.Load<AudioClip>("SingleShotMachinegun");
        dieR = Resources.Load<AudioClip>("Squeal");
        dieM = Resources.Load<AudioClip>("Splat");

        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "SingleShotMachinegun":
                source.PlayOneShot(shoot);
                break;

            case "Squeal":
                source.PlayOneShot(dieR);
                break;
            case "Splat":
                source.PlayOneShot(dieM);
                break;
        }
    }
}
