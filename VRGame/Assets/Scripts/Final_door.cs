using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Final_door : MonoBehaviour
{
    public Text textbox;
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
            textbox.text = "You WIN !!!";
            textbox1.text = "Reloading..";
            Invoke("restart_game", 10.0f);
        }
    }

    void restart_game() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
