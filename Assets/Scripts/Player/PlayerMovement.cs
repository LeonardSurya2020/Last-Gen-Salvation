using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [Header("Movement Control")]
    public float speed;
    public Rigidbody2D rb;
    public float scaleX;
    public float scaleY;
    public float horiz;
    public float vert;
    public bool isKnocked;


    [Header("Attack Control")]
    public bool isAttacking;
    public bool canRecieveInput;
    public bool inputRecieved = true;

    [Header("Animatior Control")]
    public Animator animator;

    [Header("State Machine Control")]
    public IdleState idleState = new IdleState();
    public RunningState runningState = new RunningState();
    public AttackState attackState = new AttackState();
    
    private IPlayerState currentState;
    public PlayerUnit playerUnit;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        isKnocked = false;
        playerUnit = GetComponentInParent<PlayerUnit>();
        if (playerUnit != null )
        {
            speed = playerUnit.baseSpeed;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        TransitionToState(idleState);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
            animator.SetFloat("ScaleX", scaleX);
            animator.SetFloat("ScaleY", scaleY);
            TransitionToState(attackState);
            
        }


    }

    private void FixedUpdate()
    {
        horiz = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");
    }

    public void TransitionToState(IPlayerState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);
        
    }

    public void Running(float horizontalInput, float verticalInput)
    {
        if (isKnocked == true) return;
        if (!isAttacking)
        {
            Vector2 movement = new Vector2(horizontalInput, verticalInput).normalized * speed;
            float movX = movement.x;
            float movY = movement.y;
            scaleX = horizontalInput;
            scaleY = verticalInput;
            animator.SetFloat("MovementX", movX);
            animator.SetFloat("MovementY", movY);

            if (Mathf.Abs(horizontalInput) > 0 || Mathf.Abs(verticalInput) > 0)
            {

                rb.velocity = movement;
            }
            else
            {
                rb.velocity = new Vector2(0, 0);
            }

            Flip(horizontalInput, verticalInput);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    public void Flip(float horizontalInput, float verticalInput)
    {
        if (horizontalInput > 0)
        {
            Vector3 localscale = transform.localScale;
            localscale.x = 1f;
            transform.localScale = localscale;

        }
        else if (horizontalInput < 0)
        {
            Vector3 localscale = transform.localScale;
            localscale.x = -1f;
            transform.localScale = localscale;
        }

    }


    public void Attack()
    {

            if (canRecieveInput)
            {
                canRecieveInput = false;
                inputRecieved = true;
            }
            else
            {
                return;
            }
        

    }

    public void InputManager()
    {
        if(!canRecieveInput)
        {
            canRecieveInput= true;
        }
        else
        {
            canRecieveInput = false;
        }
    }
    public void MovementStop()
    {
        StartCoroutine(MovementStopWhileAttack());
    }
    public IEnumerator MovementStopWhileAttack()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.5f);
        isAttacking = false;
    }
}
