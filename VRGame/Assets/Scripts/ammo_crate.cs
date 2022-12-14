using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ammo_crate : MonoBehaviour
{
    public GameObject player;
    public Text textbox1;
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
            textbox1.text = "Got Ammo..";
            Invoke("refill_ammo", 0.5f);
        }
    }

    void refill_ammo() {
        player.GetComponent<Gun>().refill_ammo();
        textbox1.text = "";
    }
}
