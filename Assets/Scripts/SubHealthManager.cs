using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SubHealthManager : MonoBehaviour
{
    public int Health = 6;

    [SerializeField] private float InvencibilityTime = 2f;

    [SerializeField] private float blinkInterval = 0.2f;

    [SerializeField] private float knockBackIntensity = 2f;
    
    
    private bool isInvincible = false;

    private SpriteRenderer[] renderers;

    private void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {
            var receivedDamage = other.gameObject.GetComponent<ObstacleDamageManager>().Damage;
            TakeDamage( other.transform.position, receivedDamage);
        }
    }

    void TakeDamage(Vector2 sourcePosition, int receivedDamage = 1)
    {
        if (!isInvincible)
        {
            Health -= receivedDamage;
            isInvincible = true;
            StartCoroutine(Blink());
            KnockBack(sourcePosition);
        }
        
        if (Health < 0)
        {
            Health = 0;
        }
    }

    void KnockBack(Vector2 fromPosition)
    {
        var newFromPosition = new Vector3(fromPosition.x, fromPosition.y, this.transform.position.z);
        var direction = this.transform.position - newFromPosition;
        direction.Normalize();
        
        Debug.Log(direction);
        
        this.transform.position += direction * knockBackIntensity;
    }

    IEnumerator Blink()
    {
        float blinkDuration = InvencibilityTime;

        var currentTime = Time.timeSinceLevelLoad;
        float durationToReduce = 0f;
        bool isEnabled = false;
        
        
        while (blinkDuration > 0f)
        {
            durationToReduce = Time.timeSinceLevelLoad - currentTime;
            currentTime = Time.timeSinceLevelLoad;
            blinkDuration -= durationToReduce;

            EnableRenderers(isEnabled);

            isEnabled = !isEnabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        isInvincible = false;
        EnableRenderers(true);
    }

    void EnableRenderers(bool isEnabled)
    {
        foreach (var r in renderers)
        {
            r.enabled = isEnabled;
        }
    }
}
