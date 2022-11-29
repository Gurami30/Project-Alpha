using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCombat : MonoBehaviour
{

    public Camera cam;

    [Header("SFXs")]
    public AudioSource shootSFX;
    public AudioSource reloadSFX;
    public ParticleSystem muzzleFlash;


    [Header("UI")]
    public GameObject crossHair;
    public TextMeshProUGUI ammoText;
    public Animator anim;


    [Header("Weapon")]
    public float damage = 5f;
    public float fireRate = 10f;
    float lastFired;


    [Header("Ammo")]
    public static int ammo = 150;
    int nashti = 0;
    public int curAmmo = 30;


    [Header("Objects")]
    Vector3 randPos;
    public GameObject ammoUpPrefab;
    public GameObject medKitPrefab;
    public GameObject bulletHolePrefab;

    void Update()
    {
        //AMO (UI)
        if(ammo < 0){
            ammo = 0;
        }
        ammoText.text = curAmmo.ToString() + "/" + ammo.ToString();



        //======================== BASIC SHOOTING =============================
        if(Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetKey(KeyCode.LeftControl) && curAmmo > 0 && !anim.GetBool("Reloading")){
            muzzleFlash.gameObject.SetActive(true);
            if (Time.time - lastFired > 1 / fireRate){
                anim.SetBool("Shooting", true);
                Shoot();
                lastFired = Time.time;
            }
        }else{
            anim.SetBool("Shooting", false);
        }
        //=====================================================================



        //============================ AIMING =================================
        if(Input.GetMouseButton(1) && !Input.GetMouseButton(0) && curAmmo > 0 && !anim.GetBool("Reloading")){
            anim.SetBool("Aiming", true);
            PlayerMovement.moveSpeed = 45f;
            crossHair.SetActive(false);
            Camera.main.fieldOfView = 45f;
        }else{
            anim.SetBool("Aiming", false);
            crossHair.SetActive(true);
            Camera.main.fieldOfView = 60f;
        }
        //=====================================================================



        //========================== ADS SHOOTING =============================
        if(Input.GetMouseButton(1) && Input.GetMouseButton(0) && curAmmo > 0 && ammo > 0 && !anim.GetBool("Reloading")){
            muzzleFlash.gameObject.SetActive(false);
            crossHair.SetActive(false);
            PlayerMovement.moveSpeed = 40f;
            Camera.main.fieldOfView = 45f;
            anim.SetBool("ADSshoot", true);
            anim.SetBool("Aiming", true);
            if (Time.time - lastFired > 1 / fireRate){
                anim.SetBool("ADSshoot", true);
                Shoot();
                lastFired = Time.time;
            }
        }else{
            anim.SetBool("ADSshoot", false);
        }
        //=====================================================================


        //CROUCH AIM
        /*if(Input.GetKey(KeyCode.LeftControl) && Input.GetMouseButton(1) && !Input.GetMouseButton(0) && curAmmo > 0 && !anim.GetBool("Reloading")){
            anim.SetBool("Aiming", true);
            anim.SetBool("Crouch", false);
            PlayerMovement.moveSpeed = 45f;
            crossHair.SetActive(false);
            Camera.main.fieldOfView = 45f;
        }else{
            anim.SetBool("Aiming", false);
            crossHair.SetActive(true);
            Camera.main.fieldOfView = 60f;
        }

        //CROUCH ADS SHOOTING
        if(Input.GetMouseButton(0) && Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl) && curAmmo > 0 && !anim.GetBool("Reloading")){
            muzzleFlash.gameObject.SetActive(false);
            crossHair.SetActive(false);
            PlayerMovement.moveSpeed = 40f;
            Camera.main.fieldOfView = 45f;
            anim.SetBool("ADSshoot", true);
            anim.SetBool("Aiming", true);
            if (Time.time - lastFired > 1 / fireRate){
                anim.SetBool("ADSshoot", true);
                Shoot();
                lastFired = Time.time;
            }
        }else{
            muzzleFlash.gameObject.SetActive(true);
            anim.SetBool("ADSshoot", false);
        }*/



        //======================= CROUCH SHOOTING =============================
        if(Input.GetMouseButton(0) && !Input.GetMouseButton(1) && Input.GetKey(KeyCode.LeftControl) && curAmmo > 0 && !anim.GetBool("Reloading")){
            if (Time.time - lastFired > 1 / fireRate){
                anim.SetBool("Shooting", false);
                anim.SetBool("CrouchShoot", true);
                anim.SetBool("Crouch", false);
                Shoot();
                lastFired = Time.time;
            }
        }else{
            anim.SetBool("CrouchShoot", false);
        }
        //=====================================================================



        //========================= RELOADING =================================
        if(curAmmo == 0 || (Input.GetKey("r") && curAmmo < 30 && ammo > 0)){
            shootSFX.Stop();
            anim.SetBool("Crouch", false);
            StartCoroutine(Reload());
        }
        //=====================================================================



        //========================= SPAWN AMMOS ===============================
        if(GameObject.Find("AmmoUp(Clone)") == null && ammo < 140){
            Spawn_Ammo();
        }
        if (Physics.Raycast(ammoUpPrefab.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hit)){
            if(hit.transform.tag == "NotWalkable") {
                Destroy(GameObject.Find("AmmoUp(Clone)"));
            }
        }
        //=====================================================================



        //========================= SPAWN MEDKIT ==============================
        if(GameObject.Find("MedKit(Clone)") == null && PlayerMovement.curHealth < 40f){
            Spawn_MedKit();
        }
        if (Physics.Raycast(medKitPrefab.transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hith)){
            if(hith.transform.tag == "NotWalkable") {
                Destroy(GameObject.Find("MedKit(Clone)"));
            }
        }
        //=====================================================================
    }


    void Shoot(){
        shootSFX.Play();
        muzzleFlash.Play();
        curAmmo--;
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 1500f)){
            Target target = hit.transform.GetComponent<Target>();
            if(hit.transform.tag != "Enemy") {
                Create_BulletHole(hit);
            }
            if(target != null){
                target.TakeDamage(damage);
                Target.gettingHurt = true;
            }else{
                Target.gettingHurt = false;
            }
            if(hit.rigidbody != null){
                hit.rigidbody.AddForce(-hit.normal * 30);
            }
        }
    }


    IEnumerator Reload(){
        anim.SetBool("Reloading", true);
        nashti = 30 - curAmmo;
        reloadSFX.Play();
        yield return new WaitForSeconds(2.333f);
        if(nashti <= ammo) {
            ammo = ammo - nashti;
            curAmmo += nashti;
            nashti = 0;
        }else{
            curAmmo = curAmmo + ammo;
            ammo = 0;
            nashti = 0;
        }
        anim.SetBool("Reloading", false);
    }


    void Spawn_Ammo(){
        randPos = new Vector3(Random.Range(7f, 170f), 0.4f, Random.Range(8f, 360f));
        Instantiate(ammoUpPrefab, randPos, Quaternion.identity);
    }
    void Spawn_MedKit(){
        randPos = new Vector3(Random.Range(7f, 170f), 0.4f, Random.Range(8f, 360f));
        Instantiate(medKitPrefab, randPos, Quaternion.identity);
    }
    void Create_BulletHole(RaycastHit hit){
        GameObject bulletHoleObj = Instantiate(bulletHolePrefab, hit.point, Quaternion.LookRotation(hit.normal));
        bulletHoleObj.transform.position += bulletHoleObj.transform.forward / 1000;
        Destroy(bulletHoleObj, 2f);
    }

}
