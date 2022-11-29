using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIAnimations : MonoBehaviour
{
    protected float timer;

    void Update(){
        timer += Time.deltaTime;
        Play_AnimationHeal();
    }

    public void Play_AnimationHeal(){
        this.gameObject.transform.position += new Vector3(0, 0.5f, 0);
        if (timer >= 0.1f){
            Color c = this.gameObject.GetComponent<Image>().color;
            c.a -= 0.1f;
            this.gameObject.GetComponent<Image>().color = c;
            if(c.a < 0){
                Destroy(this.gameObject);
            }
            timer = 0f;
        }
    }
}
