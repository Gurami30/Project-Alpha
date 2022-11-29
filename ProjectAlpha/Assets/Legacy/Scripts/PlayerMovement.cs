using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    [Header("UI")]
    public Slider healthSlider;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI coinText;


    [Header("General Info")]
    public Rigidbody playerrb;
    public Animator anim;
    [SerializeField]
    public AudioSource[] sounds;


    [Header("Movement Settings")]
    public LayerMask groundMask;
    public GameObject groundCheck;
    public static float moveSpeed = 70f;
    public float JumpForce = 300f;
    public static bool isGrounded = true;
    float horizontal;
    float vertical;
    Vector3 moveDirection;


    [Header("Characters Info")]
    public static float curHealth = 100;
    public static int coinCounter = 0;


    [Header("Other")]
    public float MouseSensitivity = 10f;


    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        //================ CHEKING IF PLAYER IS ON GROUNDED ===================
        isGrounded = Physics.CheckSphere(groundCheck.transform.position, 0.1f, groundMask);
        //===================================================================



        //============================ PLAYER'S HEALTH ========================
        healthText.text = curHealth.ToString();
        healthSlider.value = curHealth / 100;
        if(curHealth < 0){
            curHealth = 0;
        }
        if(curHealth > 100){
            curHealth = 100;
        }
        //=====================================================================

        

        //========================= KNOWLEDGE (COINS) =========================
        coinText.text = "KNOWLEDGE: " + coinCounter.ToString();
        //=====================================================================
        


        //========================== MOVEMENT =================================
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        MovePlayer();
        SpeedControl();
        if(isGrounded){
            playerrb.drag = 5;
        }else{
            playerrb.drag = 0;
        }
        if((Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d")) && moveSpeed != 120f){
            anim.SetBool("Moving", true);
        }else{
            anim.SetBool("Moving", false);
        }
        if(anim.GetBool("Moving")){
            sounds[0].pitch = 1.25f;
            sounds[0].enabled = true;
        }else if(anim.GetBool("Running")){
            sounds[0].pitch = 1.7f;
            sounds[0].enabled = true;
        }else{
            sounds[0].enabled = false;
        }
        //=====================================================================



        //============================ JUMP ===================================
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded){
            playerrb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
        }
        //=====================================================================



        //=============================== CROUCH ==============================
        if (Input.GetKey(KeyCode.LeftControl)){
            if(!Input.GetMouseButton(0)){
                anim.SetBool("Crouch", true);
            }else{
                anim.SetBool("Crouch", false);
            }
            moveSpeed = 40f;
            CapsuleCollider cc = this.gameObject.GetComponent<CapsuleCollider>();
            cc.height = 1f;
        }else{
            anim.SetBool("Crouch", false);
            CapsuleCollider cc = this.gameObject.GetComponent<CapsuleCollider>();
            cc.height = 2f;
        }
        //=====================================================================



        //================================ SPRINT =============================
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl) && isGrounded && (Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d"))){
            moveSpeed = 120f;
            anim.SetBool("Running", true);
        }else{
            anim.SetBool("Running", false);
        }
        //=====================================================================



        //========== RETURNING SPPED TO IT'S ORIGINAL STATE (SAFETY)===========
        if(!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl)){
            moveSpeed = 70f;
        }
        //=====================================================================
    }


    private void MovePlayer()
    {
        moveDirection = playerrb.transform.forward * vertical + playerrb.transform.right * horizontal;
        if(isGrounded){
            playerrb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            sounds[1].enabled = true;
        }else{
            playerrb.AddForce(moveDirection.normalized * moveSpeed * 0.2f, ForceMode.Force);
            sounds[1].enabled = false;
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(playerrb.velocity.x, 0f, playerrb.velocity.z);
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            playerrb.velocity = new Vector3(limitedVel.x, playerrb.velocity.y, limitedVel.z);
        }
    }
}
