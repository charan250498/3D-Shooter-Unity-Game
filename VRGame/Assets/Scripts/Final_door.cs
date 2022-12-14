using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Final_door : MonoBehaviour
{
    public Text you_win;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
 
        if (other.tag == "Player")
        {
            you_win.text = "You WIN !!!";
        }
    }
}
