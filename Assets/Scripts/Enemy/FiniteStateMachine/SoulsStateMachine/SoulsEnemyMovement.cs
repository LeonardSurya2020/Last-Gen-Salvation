using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoulsEnemyMovement : MonoBehaviour
{
    public static SoulsEnemyMovement Instance;

    [Header("Movement Control")]
    public float speed;
    public Rigidbody2D rb;
    public bool isknocked;

    public GameObject enemyObject;

    [Header("Animation Control")]
    public float scaleX;
    public float scaleY;
    public float horiz;
    public float vert;
    public bool isAttack;

    [Header("Radius Control")]
    public float chaseRadius = 5f;

    [Header("Animatior Control")]
    public Animator animator;

    public Transform target;

    [Header("State Machine Control")]
    public SoulIdleState idleState = new SoulIdleState();
    public SoulChaseState chaseState = new SoulChaseState();
    private IEnemySoulState currentState;

    public EnemyUnit enemyUnit;
    [SerializeField] private Image healthFillBar;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        isAttack = false;
        //enemyUnit = GetComponentInParent<EnemyUnit>();
        if (enemyUnit != null)
        {
            Debug.Log("base speed soul = " + enemyUnit.baseSpeed);
            speed = enemyUnit.baseSpeed;
        }
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        TransitionToState(idleState);
        target = GameObject.FindWithTag("Player").transform;
    }

    public void TransitionToState(IEnemySoulState newState)
    {
        currentState?.ExitState(this);
        currentState = newState;
        currentState.EnterState(this);

    }

    private void Update()
    {
        if (target == null)
        {
            Debug.Log("Target sudah null.");
            target = null; // Pastikan target diset ke null jika hilang
            TransitionToState(idleState); // Pindah ke idle state
            return; // Hentikan proses lebih lanjut
        }

        currentState.UpdateState(this);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 gizmosPosition = enemyObject.transform.position;
        Gizmos.color = Color.green; // Warna untuk chaseRadius
        Gizmos.DrawWireSphere(gizmosPosition, chaseRadius);
    }


    public void ChasingPlayer()
    {
        if (isknocked == true) return;

        if (target == null)
        {
            Debug.Log("Target hilang, beralih ke idle state.");
            TransitionToState(idleState); // Pindah ke idle state
            return;
        }

        speed = enemyUnit.baseSpeed;
        Vector2 direction = (target.transform.position - transform.position);
        // Bulatkan nilai X dan Y ke -1, 0, atau 1
        float roundedX = Mathf.Round(direction.x);
        float roundedY = Mathf.Round(direction.y);

        // Update scaleX dan scaleY
        scaleX = (int)roundedX;
        if (scaleX > 1) scaleX = 1;
        else if (scaleX < -1) scaleX = -1;
        scaleY = (int)roundedY;
        if (scaleY > 1) scaleY = 1;
        else if (scaleY < -1) scaleY = -1;

        Vector2 moveDirection = direction.normalized;
        rb.velocity = moveDirection * speed;
        animator.SetFloat("MovementX", scaleX);
        animator.SetFloat("MovementY", scaleY);
        Flip(direction.x); // Menambahkan flip berdasarkan arah x
    }

    public void Flip(float direction)
    {
        Vector3 localscale = transform.localScale;
        Vector3 healthBarBGScale = healthFillBar.rectTransform.localScale;

        if (direction < 0)
        {
            // Membalikkan sprite untuk bergerak ke kiri
            localscale.x = -1f;
            healthBarBGScale.x = -1f;
        }
        else if (direction > 0)
        {
            // Membalikkan sprite untuk bergerak ke kanan
            localscale.x = 1f;
            healthBarBGScale.x = 1f;
        }

        // Terapkan perubahan skala pada musuh
        transform.localScale = localscale;

        healthFillBar.rectTransform.localScale = healthBarBGScale;
    }

    public void StopChasing()
    {
        animator.SetBool("IsRunning", false);
        rb.velocity = Vector2.zero;
    }


}
