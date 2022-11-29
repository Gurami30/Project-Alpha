using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spawner : MonoBehaviour
{

    [Header("Graphics")]
    public GameObject canvas;
    public Image powerUp;
    public TextMeshProUGUI countDownText;
    private ParticleSystem helasParticles;
    [SerializeField]
    public Sprite[] image;


    // Player and Stuff
    public LayerMask playerMask;
    GameObject player;
    float timer = 10f;
    bool heal, ammo;
    bool activated = true;

    public PowerUpState state;
    public enum PowerUpState
    {
        heal,
        ammo
    }

    void Awake(){

        // Hiding Counter Text
        countDownText.gameObject.SetActive(false);

        // Finding Items
        player = GameObject.Find("Player");
        helasParticles = GameObject.Find("HealsParticles").GetComponent<ParticleSystem>();

        // Telling What to Display
        if(state == PowerUpState.heal){
            powerUp.sprite = image[0];
        }
        if(state == PowerUpState.ammo){
            powerUp.sprite = image[1];
        }
    }

    void Update(){

        // Image Looking at Player
        canvas.transform.LookAt(player.transform);


        // Starting Countdown Timer
        if(!activated){
            countDownText.gameObject.SetActive(true);
            countDownText.text = Mathf.Round(timer).ToString();
            timer -= Time.deltaTime;
        }

        // Resetting The Spawner
        if(timer <= 0){
            powerUp.gameObject.SetActive(true);
            countDownText.gameObject.SetActive(false);
            timer = 10;
            activated = true;
        }
    }


    private void OnTriggerEnter(Collider other){
        if(state == PowerUpState.heal){
            HealUp();
        }
        if(state == PowerUpState.ammo){
            AmmoUp();
        }
    }


    private void HealUp(){
        powerUp.gameObject.SetActive(false);
        activated = false;
        helasParticles.Play();
        PlayerMovement.curHealth += 20;
    }


    private void AmmoUp(){
        powerUp.gameObject.SetActive(false);
        activated = false;
        PlayerCombat.ammo += 45;
    }
}
