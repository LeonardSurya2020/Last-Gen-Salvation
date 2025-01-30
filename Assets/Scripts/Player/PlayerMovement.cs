using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;

    [SerializeField]private Camera mainCamera;
    [Header("Movement Control")]
    public float speed;
    public Rigidbody2D rb;
    public float scaleX;
    public float scaleY;
    public float horiz;
    public float vert;
    public bool isKnocked;
    public Transform dropPoint;


    [Header("Attack Control")]
    public bool isAttacking;
    public bool canRecieveInput;
    public bool inputRecieved = true;
    public bool isRangeWeapon = false;
    public bool rangeFireButtonDown = false;

    [Header("Animatior Control")]
    public Animator animator;

    [Header("State Machine Control")]
    public IdleState idleState = new IdleState();
    public RunningState runningState = new RunningState();
    public AttackState attackState = new AttackState();
    
    private IPlayerState currentState;
    public PlayerUnit playerUnit;

    [SerializeField] private SpriteRenderer weaponSpriteRenderer;
    [SerializeField] private GameObject rangeWeaponSprite;

    [field: SerializeField]
    public UnityEvent <Vector2> OnPointerPositionChange { get; set; }

    [field: SerializeField]
    public UnityEvent OnFireButtonPressed { get; set; }

    [field: SerializeField]
    public UnityEvent OnFireButtonReleased { get; set; }

    private void Awake()
    {
        mainCamera = Camera.main;
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
        //GetPointerInput();
        currentState.UpdateState(this);
        if (isRangeWeapon)
        {
            GetRangeFireInput();
            GetPointerInput();
            if(Input.GetKeyDown(KeyCode.F))
            {
                if (playerUnit != null && playerUnit.weaponObject != null)
                {
                    Debug.Log("jatoh");
                    Instantiate(playerUnit.weaponObject, dropPoint.transform.position, Quaternion.identity);
                    playerUnit.RemoveRangeWeapon();
                    isRangeWeapon = false;
                }
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Attack();
                animator.SetFloat("ScaleX", scaleX);
                animator.SetFloat("ScaleY", scaleY);
                TransitionToState(attackState);

            }
        }


    }

    private void GetRangeFireInput()
    {
        if (Input.GetAxisRaw("Fire1") > 0)
        {
            if (!rangeFireButtonDown)
            {
                rangeFireButtonDown = true;
                OnFireButtonPressed?.Invoke();
            }

        }
        else
        {
            if(rangeFireButtonDown)
            {
                rangeFireButtonDown = false;
                OnFireButtonReleased?.Invoke();
            }
            
        }
    }

    // Mengambil Input Vector dari posisi mouse
    private void GetPointerInput()
    {
        Vector3 mousPos = Input.mousePosition;
        mousPos.z = mainCamera.nearClipPlane;
        var mouseInWorldSpace = mainCamera.ScreenToWorldPoint(mousPos);
        Debug.Log("mousepos = " +  mouseInWorldSpace); 
        OnPointerPositionChange?.Invoke(mouseInWorldSpace);
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

            if(movY > 0)
            {
                weaponSpriteRenderer.sortingOrder = 12;
            }
            else
            {
                weaponSpriteRenderer.sortingOrder = 10;
            }

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

            Vector3 rangeWeaponScale = rangeWeaponSprite.transform.localScale;
            rangeWeaponScale.x = 1f;
            rangeWeaponSprite.transform.localScale = rangeWeaponScale;


        }
        else if (horizontalInput < 0)
        {
            Vector3 localscale = transform.localScale;

            localscale.x = -1f;
            transform.localScale = localscale;

            Vector3 rangeWeaponScale = rangeWeaponSprite.transform.localScale;
            rangeWeaponScale.x = -1f;
            rangeWeaponSprite.transform.localScale = rangeWeaponScale;

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
