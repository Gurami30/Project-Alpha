using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUp : MonoBehaviour
{
    public LayerMask playerMask;
    GameObject player;
    bool playerInSightRange;
    public GameObject hint;
    public GameObject ammosPrefab;

    void Awake(){
        player = GameObject.Find("Player");
    }
    
    void Update()
    {
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
        Display_Ammos();
        PlayerCombat.ammo += 45;
        Destroy(this.gameObject);
    }

    void Display_Ammos(){
        //float[] randomPosition = new float[] {Random.Range(Screen.width * 0.25f, 0), Random.Range(Screen.width - Screen.width * 0.25f, Screen.width)};
        GameObject heals = Instantiate(ammosPrefab, ammosPrefab.transform.position, Quaternion.identity);
        heals.transform.SetParent(GameObject.Find("MainCanvas/Ammo").transform);
        GameObject.Find("MainCanvas/Ammo/AmmosAnim(Clone)").GetComponent<UIAnimations>().Play_AnimationHeal();
    }
}
