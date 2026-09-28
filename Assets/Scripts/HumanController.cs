using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HumanController : MonoBehaviour
{
    [Header("Wander Settings")]
    public float wanderRadius = 8f;
    public float wanderInterval = 4f;

    [Header("Chase Settings")]
    public float catchDistance = 1.5f;
    public float chaseRepathInterval = 0.2f;
    public Transform catTarget;

    [Header("Audio")]
    public AudioClip catchSound;
    [Range(0f, 1f)] public float catchSoundVolume = 1f;

    private NavMeshAgent agent;
    private float wanderTimer;
    float chaseRepathTimer;
    bool wasChasing;

    public Transform catSpawnPoint;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        bool isChasing = AnnoyanceManager.Instance.IsChasing;

        if (isChasing)
        {
            Chase();
        }
        else
        {
            Wander();    
        }
        
        bool isChasingNow = AnnoyanceManager.Instance.IsChasing;

        if (wasChasing && !isChasingNow)
        {
            OnChaseEnded();
        }

        wasChasing = isChasing;
        
    }

    void Wander()
    {
        wanderTimer -= Time.deltaTime;

        if (wanderTimer <= 0f)
        {
            PickNewWanderDestination();
            wanderTimer = wanderInterval;
        }
    }

    void PickNewWanderDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        Vector3 randomWanderTarget = randomDirection + transform.position;

        // Find valid position CLOSEST to randomDirection. 
        if (NavMesh.SamplePosition(randomWanderTarget, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void Chase()
    {
        chaseRepathTimer -= Time.deltaTime;

        if (chaseRepathTimer <= 0f)
        {
            agent.SetDestination(catTarget.position);
            chaseRepathTimer = chaseRepathInterval;
        }

        CheckForCatch();
    }

    void CheckForCatch()
    {
        float distance = Vector3.Distance(transform.position, catTarget.position);

        if (distance <= catchDistance)
        {
            CatchCat();
        }
    }

    void CatchCat()
    {   
        PlayCatchSound();
        LivesManager.Instance.LoseLife();

        AnnoyanceManager.Instance.ForceEndChase();

        if (LivesManager.Instance.CurrentLives > 0)
        {
            RepositionCat();
        } 
    }

    void RepositionCat()
    {
        catTarget.position = catSpawnPoint.position;
    }

    void OnChaseEnded()
    {
        PropResetManager.Instance.ResetAllProps();
    }

    void PlayCatchSound()
    {
        if (catchSound != null)
        {
            AudioSource.PlayClipAtPoint(catchSound, transform.position, catchSoundVolume);
        }
    }
}
