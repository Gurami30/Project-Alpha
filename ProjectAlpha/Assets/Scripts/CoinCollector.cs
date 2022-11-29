using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public int pickUpRange = 2;
    bool playerIsInRange = false;
    public LayerMask whatIsPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerIsInRange = Physics.CheckSphere(transform.position, pickUpRange, whatIsPlayer);
        if(playerIsInRange){
            transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("Player/Main Camera").transform.position, 5.0f * Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected(){
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, pickUpRange);
    }

    void OnTriggerEnter(Collider col){
        if(col.gameObject.tag == "Player"){
            if(this.gameObject.name == "GoldCoin(Clone)"){
                PlayerMovement.coinCounter += 5;
            }
            if(this.gameObject.name == "SilverCoin(Clone)"){
                PlayerMovement.coinCounter += 2;
            }
            if(this.gameObject.name == "BronzeCoin(Clone)"){
                PlayerMovement.coinCounter += 1;
            }
            Destroy(this.gameObject);
        }
    }
}
