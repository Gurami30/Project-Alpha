using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shotgun : MonoBehaviour
{
    int ammo = 90;
    int nashti = 0;
    int curAmmo = 30;

    //Gun stats
    float damage = 20f;
    float timeBetweenShooting = 1f;
    float spread = 0.06f;
    float range = 100f;
    float reloadTime = 2f;
    float timeBetweenShots = 0f;
    float magazineSize = 30f;
    float bulletsPerTap = 6f;
    float bulletsShot;
    bool allowButtonHold = false;

    //bools 
    bool shooting, readyToShoot;
    bool reloading;

    //Reference
    private Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    //Graphics
    public ParticleSystem muzzleFlash;
    public GameObject bulletHoleGraphic;
    private TextMeshProUGUI ammoText;
    [SerializeField]
    public Image[] ammoOnGunImage;

    private void Awake()
    {
        fpsCam = GameObject.Find("CameraHolder/Main Camera").GetComponent<Camera>();
        ammoText = GameObject.Find("MainCanvas/Ammo").GetComponent<TextMeshProUGUI>();
        readyToShoot = true;
    }

    void Update()
    {
        MyInput();
        //AMO (UI)
        if(ammo < 0){
            ammo = 0;
        }
        ammoText.text = curAmmo.ToString() + "/" + ammo.ToString();
        for(int i = 4; i >= curAmmo / 6; i--){
            ammoOnGunImage[i].enabled = false;
        }

        if ((curAmmo == 0) || (Input.GetKeyDown(KeyCode.R) && curAmmo < magazineSize && reloading)){
            Reload();
        }
    }
    private void MyInput()
    {
        if (allowButtonHold){
            shooting = Input.GetKey(KeyCode.Mouse0);
        }else{
            shooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (Input.GetKeyDown(KeyCode.R) && curAmmo < magazineSize && ammo > 0 && !reloading){
            Reload();
        }

        //Shoot
        if (readyToShoot && shooting && !reloading && curAmmo > 0){
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }
    private void Shoot()
    {
        readyToShoot = false;

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range))
        {
            if (rayHit.collider.CompareTag("Enemy")){
                rayHit.collider.GetComponent<Target>().TakeDamage(damage);
            }else{
                Create_BulletHole(rayHit);
            }
        }

        muzzleFlash.Play();

        curAmmo--;
        bulletsShot--;

        Invoke("ResetShot", timeBetweenShooting);

        if(bulletsShot > 0 && curAmmo > 0){
            Invoke("Shoot", timeBetweenShots);
        }
    }
    private void ResetShot()
    {
        readyToShoot = true;
    }
    private void Reload(){
        reloading = true;
        // <--- Reload Animation Here
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        nashti = (int)magazineSize - curAmmo;
        if(nashti <= ammo) {
            ammo = ammo - nashti;
            curAmmo += nashti;
            nashti = 0;
        }else{
            curAmmo = curAmmo + ammo;
            ammo = 0;
            nashti = 0;
        }
        for(int i = 0; i < magazineSize / 6; i++){
            ammoOnGunImage[i].enabled = true;
        }
        reloading = false;
    }
    void Create_BulletHole(RaycastHit hit){
        GameObject bulletHoleObj = Instantiate(bulletHoleGraphic, hit.point, Quaternion.LookRotation(hit.normal));
        bulletHoleObj.transform.position += bulletHoleObj.transform.forward / 1000;
        Destroy(bulletHoleObj, 2f);
    }
}
