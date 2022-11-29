using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    [Header("Enemy")]
    private float curHealth = 100f;
    public static bool gettingHurt = false;
    public GameObject healthBarUI;
    public Slider slider;

    [Header("Player")]
    GameObject player;

    Image htiMark;
    AudioSource hitSFX;

    void Awake(){
        player = GameObject.Find("Player");
        slider.value = curHealth / 100;
        htiMark = GameObject.Find("MainCanvas/HitMark").GetComponent<Image>();
        hitSFX = GameObject.Find("Sounds/Hit").GetComponent<AudioSource>();
    }

    void FixedUpdate(){
        htiMark.enabled = false;
    }

    void Update(){
        try{
            slider.value = curHealth / 100;
            healthBarUI.transform.LookAt(player.transform);
        }catch{
            Debug.Log("Killed");
        }
        if(gettingHurt){
            healthBarUI.SetActive(true);
        }if(!gettingHurt){
            StartCoroutine(Hide_Health());
        }
    }

    public void TakeDamage(float amount){
        curHealth -= amount;
        hitSFX.Play();
        htiMark.enabled = true;
        gettingHurt = true;
        if(curHealth <= 0){
            Vector3 lastPos = this.gameObject.transform.position;
            this.gameObject.GetComponent<CoinSpawner>().Spawn_Coins(lastPos);
            htiMark.enabled = false;
            Destroy(this.gameObject);
        }
    }

    IEnumerator Hide_Health()
    {
        if(healthBarUI.activeSelf){
            yield return new WaitForSeconds(2f);
            healthBarUI.SetActive(false);
        }
    }
}
