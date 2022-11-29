using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject[] coinPrefabs;
    int[] coinRatio = new int[] {2, 2, 2, 2, 2, 1, 1, 1, 0, 0};

    public void Spawn_Coins(Vector3 tran){
        for(int i = 0; i < 8; i++){
            GameObject coins = Instantiate(coinPrefabs[coinRatio[Random.Range(0, 10)]], tran, Quaternion.identity);
            coins.transform.Rotate(0, Random.Range(-1, 180), 0, Space.Self);
            coins.GetComponent<Rigidbody>().AddRelativeForce(Random.onUnitSphere * 1.5f, ForceMode.Impulse);
        }
    }
}
