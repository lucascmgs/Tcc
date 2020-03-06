using System;
using System.Collections;
using DefaultNamespace;
using Photon.Pun;
using UnityEngine;

public class LineBombBehaviour : MonoBehaviour
{
    [SerializeField] private LineDirection _explosionsDirection;

    [SerializeField] private float _timeUntilFirstExplosion = 4f;
    [SerializeField] private float _initialBlinkTime = 0.5f;
    [SerializeField] private int _numberOfExplosions = 5;
    [SerializeField] private float _timeBetweenExplosions = 0.5f;
    [SerializeField] private float _distanceBetweenExplosions = 1f;
    [SerializeField] private Sprite _onSprite;
    [SerializeField] private Sprite _offSprite;
    [SerializeField] private GameObject _explosionPrefab;

    private AudioManager _audioManager;

    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();
    }

    private void OnBecameVisible()
    {
        if (PhotonNetwork.IsMasterClient || !PhotonNetwork.IsConnected) 
        {
            StartCoroutine(BombBlink());
        }
    }

    private void FlipSprite()
    {
        if (_spriteRenderer.sprite == _onSprite)
        {
            _spriteRenderer.sprite = _offSprite;
        }
        else
        {
            _spriteRenderer.sprite = _onSprite;
        }
    }

    private void DisableComponents()
    {
        _spriteRenderer.enabled = false;
        _collider2D.enabled = false;
        _rigidbody2D.simulated = false;
    }

    private IEnumerator BombBlink()
    {
        float timeUntilExplosion = _timeUntilFirstExplosion;
        float durationToWait = _initialBlinkTime;
        float decrement = 0.1f;

        int numberOfBlinks;
        while (timeUntilExplosion > 0f)
        {
            if (durationToWait - decrement > 0.1f)
            {
                durationToWait -= decrement;
            }

            timeUntilExplosion -= durationToWait;
            
            _audioManager.Play("Beep");
            FlipSprite();
            yield return new WaitForSeconds(durationToWait/2);
            FlipSprite();
            

            yield return new WaitForSeconds(durationToWait/2);
        }

        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        DisableComponents();
        int explosionsCount = 0;
        var thisPos = transform.position;
        
        while (explosionsCount < _numberOfExplosions)
        {
            _audioManager.Play("Explosion");
            var newPositivePosition = thisPos;
            var newNegativePosition = thisPos;
            if (_explosionsDirection == LineDirection.Horizontal)
            {
                newPositivePosition.x = thisPos.x + _distanceBetweenExplosions * (explosionsCount + 1);
                newNegativePosition.x = thisPos.x - _distanceBetweenExplosions * (explosionsCount + 1);
            }

            if (_explosionsDirection == LineDirection.Vertical)
            {
                newPositivePosition.y = thisPos.y + _distanceBetweenExplosions * (explosionsCount + 1);
                newNegativePosition.y = thisPos.y - _distanceBetweenExplosions * (explosionsCount + 1);
            }

            Instantiate(_explosionPrefab, newPositivePosition, Quaternion.identity);
            Instantiate(_explosionPrefab, newNegativePosition, Quaternion.identity);
            
            explosionsCount++;
            yield return new WaitForSeconds(_timeBetweenExplosions);
        }
        Destroy(this.gameObject);
    }
}

[Serializable]
enum LineDirection
{
    Horizontal, Vertical
}
