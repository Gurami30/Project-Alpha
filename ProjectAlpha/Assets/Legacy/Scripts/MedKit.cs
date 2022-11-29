using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    [Header("General")]
    public LayerMask playerMask;
    GameObject player;
    bool playerInSightRange;
    public GameObject hint;
    

    [Header("Animations")]
    private ParticleSystem helasParticles;

    void Awake(){
        player = GameObject.Find("Player");
        helasParticles = GameObject.Find("HealsParticles").GetComponent<ParticleSystem>();
    }
    
    void Update(){
        playerInSightRange = Physics.CheckSphere(transform.position, 30f, playerMask);
        if(playerInSightRange){
            hint.transform.LookAt(player.transform);
            hint.SetActive(true);
        }else{
            hint.SetActive(false);
        }

        this.transform.Rotate(Vector3.up * 15 * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col){
        helasParticles.Play();
        PlayerMovement.curHealth += 20;
        Destroy(this.gameObject);
    }
}
