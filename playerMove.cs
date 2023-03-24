using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;
using System;

public class playerMove : MonoBehaviour
{
    [Header("General")] 
    public bool playerTwo;
    public playerMove otherPlayer;
    public PlayerPreset[] playerPresets;
    public PlayerPreset playerPreset;
    private Animator anim;
    private string currentState;
    private GameObject damageBox;

    private Rigidbody2D rb;

    float speed;
    float verticalSpeed;

    [Header("physics")]
    public bool grounded;
    public float radius;
    public LayerMask mask;
    public Transform feetPos;
    bool facingRight = true;

    public Transform Enemy;

    bool imobilized;


    [Header("UI")]
    private float HP;
    public Slider HPSlider;


    // ATTACKING BOOL
    // CHECK TIME AND DISABLES MOVEMENT

    private float attackTimer;
    bool attacking;
    
    private bool jumping;


    private float damage;

    private bool hit;

    public GameObject deathPanel;

    float HInput = 0;

    bool blocking = false;
    bool imortal = false;

    // controller
    public string port = "COM1";

    public static SerialPort serialport;

    public bool[] button = { false, false, false, false, false, false, false, false, false };

    public float timer;
    float time;


    string attackStr;
    string lastAttackStr;
    bool attackble;


    List<int[]> attacks = new List<int[]>
    {
        new int[] { 4, 5, 6 },       //[0]PUNCH
        new int[] { 1, 2, 3 },       //[1]PUCHUP
        new int[] { 7, 8, 9 },       //[2]PUNCHDOWN
        new int[] { 7, 5, 3 },       //[3]Kick
        new int[] { 1, 5, 9 },       //[4]kickDown
        new int[] { 1, 5, 3, 5, 7 }, //[5]ULT

    };


    int Touch = 0;
    int lastTouch = 0;
    bool valueChanged = false;



    List<int> history = new List<int>();





    // Start is called before the first frame update
    void Start()
    {
        if (!playerTwo)
        {
            playerPreset = playerPresets[PlayerPrefs.GetInt("p1ID")];
        }
        else if (playerTwo)
        {
            playerPreset = playerPresets[PlayerPrefs.GetInt("p2ID")];
        }

        anim = GetComponent<Animator>();
        rb =   GetComponent<Rigidbody2D>();

        //FIXA SNART
        Unskip(playerPreset.intro);
        StartCoroutine(Imortal(playerPreset.intro.length));

        HP = playerPreset.MaxHP;
        HPSlider.maxValue = playerPreset.MaxHP;
        HPSlider.value = HP;

        //CONTROLLER

        history.Add(0);
        history.Add(0);
        history.Add(0);
        history.Add(0);
        history.Add(0);

        serialport = new SerialPort(port, 9600, Parity.None, 8, StopBits.One);
        serialport.PortName = port;
        serialport.BaudRate = 9600;

        serialport.Open();

    }

    private void FixedUpdate()
    {
        // Gör senare
        // fysik här

        
        if ((grounded && attackTimer >= 0) || imobilized /*|| HInput == 0*/)
        {
            speed *= 0.9f;
            //speed = Mathf.Lerp(speed, 0, Time.deltaTime);
        }
        else if (!grounded)
        {
            speed = rb.velocity.x;
        }
        else if(HInput != 0)
        {
            speed = HInput * playerPreset.walkSpeed;
        }
        else
        {
            speed = 0;
        }

        if (!jumping)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
    }




    enum AttackState
    {
        LightAttack,
        HeavyAttack,
        KickAttack,
        UltimateAttack
    }




    AttackState attackState;
    private bool dead;





    // Update is called once per frame
    void Update()
    {
        // How controller will work

        // system to determine attacks / moves at the top

        // sets bools to true or false

        // clears them after the animation chain


        string[] input = serialport.ReadLine().Split(',');
        serialport.ReadTimeout = 25;




        Touch = int.Parse(input[0]);


        if (Touch != lastTouch)
        {
            valueChanged = true;
        }
        lastTouch = Touch;

        for (int i = 0; i < 9; i++)
        {
            if (Touch == i+1) button[i] = true; else button[i] = false;
        }

        for (int i = 0; i < 9; i++)
        {
            if (button[i] && valueChanged) { AddHistory(i+1); valueChanged = false; }
        }              



        for (int i = 0; i < attacks.Count; i++)
        {
            int attack = Check(attacks[i], i);
            if (attack >= 0)
            {
                Debug.Log(attack);
                attackStr = attack.ToString();

                if(attackStr != "-1" && lastAttackStr != attackStr)
                {
                    attackble = true;
                    lastAttackStr = attackStr;
                }
                // set bolean to true here 
                // and then to false at the end of update

            }
        }




        //bool up;
        //bool down;


        //if()
        //fixa mer general independent controls
        HInput = 0;
        if (!playerTwo)
        {
            if (Input.GetKey(KeyCode.A)) HInput = -1;
            else if (Input.GetKey(KeyCode.D)) HInput = 1;
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow)) HInput = -1;
            else if (Input.GetKey(KeyCode.RightArrow)) HInput = 1;
        }

        grounded = Physics2D.OverlapCircle(feetPos.position, radius, mask);

        //speed = Input.GetAxisRaw("Horizontal") * playerPreset.walkSpeed;
        


        if (((Input.GetKeyDown(KeyCode.W)  && grounded) && !playerTwo && !imobilized) || ((Input.GetKeyDown(KeyCode.UpArrow) && grounded) && playerTwo && !imobilized))
        {
            rb.velocity = new Vector2(speed * 2, playerPreset.jumpSpeed);
            jumping = true;
        }

        if(rb.velocity.y < 0)
        {
            jumping = false;
        }




        if(Enemy.transform.position.x - transform.position.x > 0 && !facingRight)
        {
            Flip();
        }
        else if (Enemy.transform.position.x - transform.position.x < 0 && facingRight)
        {
            Flip();
        }


       //if (Input.GetKeyDown(KeyCode.U))
       //{
       //    Flip();
       //}






        imobilized = false;


        // Animation
        //if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        //{
        //    ChangeAnimation(playerPreset.Jump.name);
        //    AttackTimer(playerPreset.Jump);
        //}

        //prioirty over attacks
        if (dead)
        {
            Unskip(playerPreset.death);
            imobilized = true;

        }

        // else if (hit && !grounded)
        // {
        //     Unskip(playerPreset.hitAir);
        //     hit = false;
        // }

        //else if ((Input.GetKeyDown(KeyCode.E) && !grounded && !playerTwo) && !imobilized || (Input.GetKeyDown(KeyCode.RightControl) && !grounded && playerTwo) && !imobilized)
        //{
        //    Unskip(playerPreset.airAttack);
        //    Debug.Log("airhit");
        //}       

        //JUMP
        else if (!grounded && rb.velocity.y >= 0) ChangeAnimation(playerPreset.Jump);

        else if (!grounded && rb.velocity.y < 0) ChangeAnimation(playerPreset.JumpDown);


        //HIT
        else if (hit && !imortal)
        {
            Unskip(playerPreset.hit, true);
            StartCoroutine(Imortal(playerPreset.hit.length));
            hit = false;
        }

        // PUNCH  #####################################################################################################################
        else if ((Input.GetKeyDown(KeyCode.G) && !playerTwo) || (attackStr == "5" && valueChanged && !playerTwo) || (Input.GetKeyDown(KeyCode.L) && playerTwo) || (attackStr == "5" && valueChanged && playerTwo))                         //#
        {                                                                                                                          
            Unskip(playerPreset.Ultimate);                                                                                         
            attackState = AttackState.UltimateAttack;                                                                              
        }                                                                                                                          
                                                                                                                                   
                                                                                                                                   
        else if ((Input.GetKeyDown(KeyCode.E) && !playerTwo) || (attackStr == "0" && valueChanged && !playerTwo) || (Input.GetKeyDown(KeyCode.Period) && playerTwo) || (attackStr == "0" && valueChanged && playerTwo))                    //#
        {                                                                                                                         
            Unskip(playerPreset.Attack);                                                                                          
            attackState = AttackState.HeavyAttack;                                                                                
        }                                                                                                                         
                                                                                                                                  
        else if ((Input.GetKeyDown(KeyCode.Alpha4) && !playerTwo) || (attackStr == "1" && valueChanged && !playerTwo) || (Input.GetKeyDown(KeyCode.Minus) && playerTwo) || (attackStr == "1" && valueChanged && playerTwo))                 //#
        {                                                                                                                         
            Unskip(playerPreset.AttackUp);                                                                                        
            attackState = AttackState.LightAttack;                                                                                
        }                                                                                                                         
                                                                                                                                  
        else if ((Input.GetKeyDown(KeyCode.R) && !playerTwo) || (attackStr == "2" && valueChanged && !playerTwo) || (Input.GetKeyDown(KeyCode.Comma) && playerTwo) || (attackStr == "2" && valueChanged && playerTwo))                     //#
        {                                                                                                                         
            Unskip(playerPreset.AttackDown);                                                                                      
            attackState = AttackState.LightAttack;                                                                                
        }                                                                                                                         
                                                                                                                                  
                                                                                                                                  
        // KICK                                                                                                                   
        else if ((Input.GetKeyDown(KeyCode.F) && !playerTwo) || (attackStr == "3" && valueChanged && !playerTwo) || (Input.GetKeyDown(KeyCode.RightShift) && playerTwo) || (attackStr == "3" && valueChanged && playerTwo))             //#
        {                                                                                                                          
            Unskip(playerPreset.Kick);                                                                                             
            attackState = AttackState.KickAttack;                                                                                  
        }                                                                                                                          
                                                                                                                                   
        else if ((Input.GetKeyDown(KeyCode.C) && !playerTwo) || (attackStr == "4" && valueChanged && !playerTwo) || (Input.GetKeyDown(KeyCode.RightControl) && playerTwo) || (attackStr == "4" && valueChanged && playerTwo))             //#
        {                                                                                                                      
            Unskip(playerPreset.KickDown);                                                                                     
            attackState = AttackState.KickAttack;                                                                              
        }                                                                                                                      
        // ############################################################################################################################

        // Block
        else if (Input.GetKeyDown(KeyCode.S) && !playerTwo || Input.GetKeyDown(KeyCode.DownArrow) && playerTwo)
        {
            Unskip(playerPreset.blockDown);
            blocking = true;
        }

        else if (Input.GetKey(KeyCode.S) && !playerTwo || Input.GetKey(KeyCode.DownArrow) && playerTwo)
        {
            ChangeAnimation(playerPreset.block);
            imobilized = true;
        }

        else if (Input.GetKeyUp(KeyCode.S) && !playerTwo || Input.GetKeyUp(KeyCode.DownArrow) && playerTwo)
        {
            Unskip(playerPreset.blockUp);
            blocking = false;
        }

        else if (speed > 0) 
        {
            if (facingRight)
            {
                ChangeAnimation(playerPreset.walkRight);
            }
            else
            {
                ChangeAnimation(playerPreset.walkLeft);
            }
        }

        else if (speed < 0)
        {
            if (facingRight)
            {
               ChangeAnimation(playerPreset.walkLeft);
            }
            else
            {
                ChangeAnimation(playerPreset.walkRight);
            }
        }

        else ChangeAnimation(playerPreset.idle);





        switch (attackState)
        {
            case AttackState.LightAttack:
                damage = playerPreset.lightDamage;
                break;
            
            case AttackState.HeavyAttack:
                damage = playerPreset.heavyDamage;
                break;
           
            case AttackState.KickAttack:
                damage = playerPreset.kickDamage;
                break;
            
            case AttackState.UltimateAttack:
                damage = playerPreset.ultimateDamage;
                break;
           
            default:
                break;
        }

        attackble = false;
        attackTimer -= Time.deltaTime;
    }


    void ChangeAnimation(AnimationClip animClip)
    {
        if (animClip.name == currentState) return;


        if(attackTimer <= 0)
        {
            anim.Play(animClip.name);

            currentState = animClip.name;
        }
        
    }
    void ChangeAnimation(AnimationClip animClip, bool bypass)
    {
        if (bypass)
        {
            if (animClip.name == currentState) return;

            anim.Play(animClip.name);

            currentState = animClip.name;
        }      
    }


    void AttackTimer(AnimationClip animClip, bool bypass)
    {
        if (attackTimer <= 0)
        {
            attackTimer = animClip.length /* * (1/animClip.apparentSpeed)*/;
        }
    }


    void Unskip(AnimationClip animClip)
    {
        ChangeAnimation(animClip);
        AttackTimer(animClip,false);

    }
    void Unskip(AnimationClip animClip, bool bypass)
    {
        ChangeAnimation(animClip, bypass);
        AttackTimer(animClip, true);

    }


    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale =  transform.localScale; 
        scale.x *= -1;
        transform.localScale = scale;
    }


    public void hitEnemy()
    {
        otherPlayer.TakeDamage(damage);
        Debug.Log("hitenemy");
    }


    public IEnumerator Imortal(float time)
    {
        imortal = true;
        yield return new WaitForSeconds(time);
        imortal = false;
    }

    


    // play hit animation.
    public void TakeDamage(float damageVal)
    {
        if (!blocking)
        {
            HP -= damageVal;
            HPSlider.value = HP;

            hit = true;

            // knockback

            transform.position -= new Vector3(2 * transform.localScale.x, 0, 0);

            if (HP <= 0) 
            { 
                StartCoroutine( Die() );
                otherPlayer.Win();
            } 
            
        }
        else
        {
            transform.position -= new Vector3(1 * transform.localScale.x, 0, 0);
        }

    }

    void Win()
    {
        Unskip(playerPreset.win, true);
        attackTimer = 5;
    }


    // make corutine
    // time activate panel
    IEnumerator Die()
    {
        dead = true;

        yield return new WaitForSeconds(5);

        deathPanel.SetActive(true);
     
    }



    // controller

    int Check(int[] attack, int attackId)
    {
        //Debug.Log("check");

        Array.Reverse(attack);

        for (int i = 0; i < attack.Length; i++)
        {
            if (!(history[4 - i] == attack[i]))
            {
                return -1;
            }
        }

        Clear();
        return attackId;
    }

    void AddHistory(int number)
    {
        history.RemoveAt(0);
        history.Add(number);



        //text.text = (history[0] + " " + history[1] + " " + history[2] + " " + history[3] + " " + history[4]);

    }

    void Clear()
    {
        history.Clear();
        history.Add(0);
        history.Add(0);
        history.Add(0);
        history.Add(0);
        history.Add(0);

        //text.text = (history[0] + " " + history[1] + " " + history[2] + " " + history[3] + " " + history[4]);
    }


    void OnApplicationQuit()
    {
        if (serialport != null)
            serialport.Close();
    }
}
