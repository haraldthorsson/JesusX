using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerMove : MonoBehaviour
{
    [Header("General")] 
    public bool playerTwo;
    public playerMove otherPlayer;
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






    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb =   GetComponent<Rigidbody2D>();

        Unskip(playerPreset.intro);

        HP = playerPreset.MaxHP;
        HPSlider.maxValue = playerPreset.MaxHP;
        HPSlider.value = HP;
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
        Attack,
        AirAttack,

        HeavyAttack,
        SuperAttack,
        MegaAttack
    }

    AttackState attackState;
    private bool dead;





    // Update is called once per frame
    void Update()
    {


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

        else if (hit && !grounded)
        {
            Unskip(playerPreset.hitAir);
            hit = false;
        }

        //else if ((Input.GetKeyDown(KeyCode.E) && !grounded && !playerTwo) && !imobilized || (Input.GetKeyDown(KeyCode.RightControl) && !grounded && playerTwo) && !imobilized)
        //{
        //    Unskip(playerPreset.airAttack);
        //    Debug.Log("airhit");
        //}       


        else if (!grounded && rb.velocity.y >= 0) ChangeAnimation(playerPreset.Jump);

        else if (!grounded && rb.velocity.y < 0) ChangeAnimation(playerPreset.JumpDown);

        else if (hit)
        {
            Unskip(playerPreset.hit);
            hit = false;
        }


        // PUNCH
        else if ((Input.GetKeyDown(KeyCode.E) && !playerTwo) || (Input.GetKeyDown(KeyCode.Period) && playerTwo))
        {
            Unskip(playerPreset.Attack);
        }

        else if((Input.GetKeyDown(KeyCode.Alpha4) && !playerTwo) || (Input.GetKeyDown(KeyCode.Minus) && playerTwo))
        {
            Unskip(playerPreset.AttackUp);
        }
        
        else if((Input.GetKeyDown(KeyCode.R) && !playerTwo) || (Input.GetKeyDown(KeyCode.Comma) && playerTwo))
        {
            Unskip(playerPreset.AttackDown);
        }


        // KICK
        else if ((Input.GetKeyDown(KeyCode.F) && !playerTwo) || (Input.GetKeyDown(KeyCode.RightShift) && playerTwo))
        {
            Unskip(playerPreset.Kick);
        }

        else if ((Input.GetKeyDown(KeyCode.C) && !playerTwo) || (Input.GetKeyDown(KeyCode.RightControl) && playerTwo))
        {
            Unskip(playerPreset.KickDown);
        }


        // Block
        else if (Input.GetKeyDown(KeyCode.S) && !playerTwo || Input.GetKeyDown(KeyCode.DownArrow) && playerTwo)
        {
            Unskip(playerPreset.blockDown);
        }

        else if (Input.GetKey(KeyCode.S) && !playerTwo || Input.GetKey(KeyCode.DownArrow) && playerTwo)
        {
            ChangeAnimation(playerPreset.block);
            imobilized = true;
        }

        else if (Input.GetKeyUp(KeyCode.S) && !playerTwo || Input.GetKeyUp(KeyCode.DownArrow) && playerTwo)
        {
            Unskip(playerPreset.blockUp);
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
            case AttackState.Attack:
                damage = 1;

                break;
            case AttackState.AirAttack:
                damage = 2;

                break;
            case AttackState.HeavyAttack:
                break;
            case AttackState.SuperAttack:
                break;
            case AttackState.MegaAttack:
                break;
            default:
                break;
        }


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


    void AttackTimer(AnimationClip animClip)
    {
        if (attackTimer <= 0)
        {
            attackTimer = animClip.length;
        }
    }


    void Unskip(AnimationClip animClip)
    {
        ChangeAnimation(animClip);
        AttackTimer(animClip);

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
        otherPlayer.TakeDamage(damage * playerPreset.damage);
        Debug.Log("hitenemy");
    }


    // play hit animation.
    public void TakeDamage(float damageVal)
    {
        HP -= damageVal;
        HPSlider.value = HP;

        hit = true;

        if (HP <= 0) StartCoroutine( Die() );

    }


    // make corutine
    // time activate panel
    IEnumerator Die()
    {
        dead = true;

        yield return new WaitForSeconds(5);

        deathPanel.SetActive(true);
     
    }
}
