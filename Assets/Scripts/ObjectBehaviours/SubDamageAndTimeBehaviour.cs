using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubDamageAndTimeBehaviour : MonoBehaviour
{
    public int MaxHealth = 6;
    
    public int Health = 6;

    [SerializeField] private float InvencibilityTime = 2f;

    [SerializeField] private float blinkInterval = 0.2f;

    [SerializeField] private float knockBackIntensity = 2f;

    private UITImeFrame timeFrame;

    private bool isInvincible = false;

    private SpriteRenderer[] renderers;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        renderers = GetComponentsInChildren<SpriteRenderer>();
        timeFrame = FindObjectOfType<UITImeFrame>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Obstacle")
        {
            var receivedDamage = other.gameObject.GetComponent<ObstacleDamageManager>().Damage;
            TakeDamage(other.transform.position, receivedDamage);
        }

        if (other.tag == "HealthItem")
        {
            GetHealth(1);
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        if (timeFrame.remainingTimeRatio <= 0)
        {
            StartCoroutine(FinishGame(Gamestate.SubOwn));
        }
    }

    private IEnumerator FinishGame(Gamestate endstate)
    {
        var server = FindObjectOfType<ServerManager>();
        string endString = "EndGame;";

        if (endstate == Gamestate.PhoneOwn)
        {
            GameOptions.gameState = Gamestate.PhoneOwn;
            endString += "phoneOwn";
        }
        else if (endstate == Gamestate.SubOwn)
        {
            GameOptions.gameState = Gamestate.SubOwn;
            endString += "subOwn";
        }

        if (server != null)
        {
            server.Send(endString);
        }

        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene("GameOverScene");
    }

    void TakeDamage(Vector2 sourcePosition, int receivedDamage = 1)
    {
        _audioManager?.Play("Damage");
        
        if (!isInvincible)
        {
            Health -= receivedDamage;
            isInvincible = true;
            StartCoroutine(InvencibilityBlink());
            KnockBack(sourcePosition);
        }

        if (Health < 0)
        {
            Health = 0;
        }

        if (Health == 0)
        {
            StartCoroutine(FinishGame(Gamestate.PhoneOwn));
        }
    }

    void GetHealth(int amount)
    {
        if (Health + amount <= MaxHealth)
        {
            Health += amount;
        }
    }

    void KnockBack(Vector2 fromPosition)
    {
        var newFromPosition = new Vector3(fromPosition.x, fromPosition.y, this.transform.position.z);
        var direction = this.transform.position - newFromPosition;
        direction.Normalize();
        this.transform.position += direction * knockBackIntensity;
    }

    IEnumerator InvencibilityBlink()
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