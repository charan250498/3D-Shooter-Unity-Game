using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public Animator animator;
    public GameObject[] targets = new GameObject [5];
    public GameObject player;

    // Enemy gun end and start points
    public GameObject end, start;
    public GameObject gun;
    public GameObject muzzleFlash;

    float gunShotTime = 0.1f;

    int target_index = 0;
    bool patrolling = true;
    float detection_angle = 30.0f;
    float lethal_distance = 4.0f;
    float detection_distance = 12.0f;
    bool player_detected = false;

    public float health = 100.0f;
    public bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.LookRotation(targets[0].transform.position - transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) {
            transform.position = new Vector3(transform.position.x, -0.8f, transform.position.z);
            return;
        }
        try{
        if (patrolling && !isDead) {
            if (Vector3.Distance(transform.position, targets[target_index].transform.position) < 1.0f) {
                target_index = (target_index + 1) % 4;
                //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targets[target_index].transform.position - transform.position), Time.deltaTime);
                transform.rotation = Quaternion.LookRotation(targets[target_index].transform.position - transform.position);
            }
        }
        
        //Debug.Log(Vector3.Angle(player.transform.position - transform.position, transform.forward));
        //Invoke("Reloading", 3.0f);

        // Detect Player code
        float angle_with_player = Vector3.Angle(player.transform.position - transform.position, transform.forward);
        float distance_with_player = Vector3.Distance(player.transform.position, transform.position);
        //Debug.Log(angle_with_player);
        if ((angle_with_player < Math.Abs(detection_angle)) && !isDead && (distance_with_player < detection_distance)){
            detected_player(distance_with_player);
        }
        } catch (Exception ex) {
        }
    }

    void detected_player(float distance_with_player){
        transform.rotation = Quaternion.LookRotation(player.transform.position - transform.position);
        animator.SetTrigger("player_detected");
        animator.SetTrigger("stop_fire");
        if (!player_detected) {
            patrolling = false;
            player_detected = true;
        }
        if ((distance_with_player < lethal_distance)) {
            animator.SetTrigger("lethal_distance_trigger");

            // Start shooting the player.
            RaycastHit hit;
            int layerMask = 1 << 8;
            if (gunShotTime >= 0.0f) {
                gunShotTime -= Time.deltaTime;
            }
            // Random vector makes the enemy hit the player with a 20% probability.
            if (gunShotTime <= 0) {
                // Delay between enemy shooting at the player.
                gunShotTime = 0.5f;

                // Player should be hit with only 20% probability.
                Vector3 randomVector = new Vector3(UnityEngine.Random.Range(0f, 0f), UnityEngine.Random.Range(-3f, 3f), UnityEngine.Random.Range(-0f, 0f));

                animator.SetTrigger("fire");
                animator.SetTrigger("stop_fire");

                // Adding muzzle Flash
                GameObject muzzleFlashObject = Instantiate(muzzleFlash, end.transform.position, end.transform.rotation);
                muzzleFlashObject.GetComponent<ParticleSystem>().Play();
                Destroy(muzzleFlashObject, 1.0f);

                if(Physics.Raycast(end.transform.position, ((end.transform.position - start.transform.position)+randomVector).normalized, out hit, 100.0f, layerMask)) {
                    player.GetComponent<Gun>().Being_shot();
                }
                else {
                    // Debug.Log("Enemey did not hit player");
                }
            }
            
        } else {
            animator.SetTrigger("nonlethal_distance_trigger");
        }
    }

    public void Being_shot(float damage) // getting hit from enemy
    {
        // The player shot the enemy. Reduce the enemy health by 20%.
        if (isDead == false){
            health = health - damage;

            if (!player_detected && (health > 1.0f)) {
                // Detect player if he shoots the enemy.
                float distance_with_player = Vector3.Distance(player.transform.position, transform.position);
                detected_player(distance_with_player);
            }
        }

        if ((health <= 0.0f) && !isDead){
            isDead = true;
            // Run death animation.
            animator.SetTrigger("die");

            // Detach the gun from the enemy.
            gun.transform.parent = null;
            gun.transform.position = new Vector3(gun.transform.position.x, 0.2f, gun.transform.position.z);
            transform.position = new Vector3(transform.position.x, -0.8f, transform.position.z);
        }
    }
}
