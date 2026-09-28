using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BreakableProp : MonoBehaviour
{
    [Header("Break Settings")]
    public float breakImpactThreshold = 3f;
    public int scoreValue = 50;
    public int annoyanceValue = 50;
    public string breakableSurfaceTag = "Ground";

    [Header("Effects (optional)")]
    public GameObject breakEffectPrefab;
    public AudioClip breakSound;
    [Range(0f, 1f)] public float breakSoundVolume = 1f;

    bool isBroken;
    Vector3 startPosition;
    Quaternion startRotation;
    Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Start()
    {
        if (PropResetManager.Instance == null)
        {
            Debug.LogError($"{name}: PropResetManager.Instance is null in Start");
            return;
        }
        PropResetManager.Instance.Register(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        CheckForBreak(collision);      
    }

    void OnCollisionStay(Collision collision)
    {
        CheckForBreak(collision);     
    }

    void CheckForBreak(Collision collision)
    {
        if (isBroken) 
        {
            return;
        }
        if (!collision.gameObject.CompareTag(breakableSurfaceTag)) 
        {
            return;
        }

        float impactSpeed = collision.relativeVelocity.magnitude;
        if (impactSpeed >= breakImpactThreshold)
        {
             Break(); 
        }   
    }

    void Break()
    {   
        isBroken = true;

        PlayBreakVFX();
        PlayBreakSFX();

        ReportScore();
        ReportAnnoyance();
        
        gameObject.SetActive(false);
    }

    public void ResetProp()
    {
        isBroken = false;
        gameObject.SetActive(true); // This needs to happen before the below - modifications did not take effect while inactive.
        transform.position = startPosition;
        transform.rotation = startRotation;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    #region Helpers
    void ReportScore()
    {
        ScoreManager.Instance.AddScore(scoreValue);
    }

    void ReportAnnoyance()
    {
        AnnoyanceManager.Instance.AddAnnoyance(annoyanceValue);
    }

    void PlayBreakVFX()
    {
        if (breakEffectPrefab != null)
        {
            Instantiate(breakEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    void PlayBreakSFX()
    {
        if (breakSound != null)
        {
            AudioSource.PlayClipAtPoint(breakSound, transform.position, breakSoundVolume);
        }
    }

    #endregion
}
