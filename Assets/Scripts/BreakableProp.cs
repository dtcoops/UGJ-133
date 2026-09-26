using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BreakableProp : MonoBehaviour
{
    [Header("Break Settings")]
    public float breakImpactThreshold = 3f;
    public int scoreValue = 50;
    public string breakableSurfaceTag = "Ground";

    [Header("Effects (optional)")]
    public GameObject breakEffectPrefab;
    public AudioClip breakSound;
    [Range(0f, 1f)] public float breakSoundVolume = 1f;

    bool isBroken;

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
        // Debug.Log($"{gameObject.name} collided with {collision.gameObject.name} as speed {impactSpeed}");
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

        Debug.Log($"Broke {gameObject.name}, worth {scoreValue} points!");
        Destroy(gameObject);
    }

    #region Helpers
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
