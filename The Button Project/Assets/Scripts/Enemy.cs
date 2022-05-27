using System;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1;
    [SerializeField] ParticleSystem deathByShieldVFX;
    [SerializeField] ParticleSystem deathByButtonVFX;


    Transform target;
    NavMeshAgent agent;
    Animator anim;
    float startingYpos;
    float deathDelay = 1f;


    public static event Action<GameObject> OnEnemyDied;
    public static event Action OnEnemyBounce;
    public static event Action OnShieldDeath;
    public static event Action OnButtonDeath;


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        startingYpos = transform.position.y;
        target = FindObjectOfType<Button>().transform;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        agent.SetDestination(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Shield"))
            DieAtButton();
        else
            DieToShield();
    }

    private void DieAtButton()
    {
        GameManager.Instance.LowerHealth(1);
        deathByButtonVFX.Play();
        OnButtonDeath?.Invoke();

        DeathStuff();
    }


    private void DieToShield()
    {
        deathByShieldVFX.Play();
        GameManager.Instance.IncreaseScore(100);
        OnShieldDeath?.Invoke();

        DeathStuff();
    }

    private void DeathStuff()
    {
        OnEnemyDied.Invoke(gameObject);
        GetComponent<Collider>().enabled = false;
        agent.enabled = false;
        anim.SetBool("isRunning", false);
        anim.StopPlayback();
        Destroy(gameObject, deathDelay);
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void EnemyBounce()
    {
        //OnEnemyBounce?.Invoke();
    }
}
