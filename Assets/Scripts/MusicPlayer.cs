using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] bool onlyOneMusic = false;

    // Start is called before the first frame update
    void Awake()
    {
        SetUpSingleton();
    }

    private void SetUpSingleton()
    {
        if (!onlyOneMusic) 
        { 
            return;     
        }
        else
        {
            // The GetType() Method will return the Type of the Object the script is currently in
            if (FindObjectsOfType(GetType()).Length > 1) // In here GetType() will return MusicPlayer
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
