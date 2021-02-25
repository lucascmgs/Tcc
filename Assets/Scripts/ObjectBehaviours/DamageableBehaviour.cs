using System;
using UnityEngine;

namespace DefaultNamespace.ObjectBehaviours
{
    public class DamageableBehaviour : MonoBehaviour
    {
        [SerializeField] private bool destructible = false;

        private Animator _anim;
    
        private Rigidbody2D _rb;

        private Collider2D _col;
        
        private AudioManager _audioManager;

        void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _anim = GetComponent<Animator>();
        
            _col = GetComponent<Collider2D>();
        
            _audioManager = FindObjectOfType<AudioManager>();
        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (destructible)
            {
                if (other.CompareTag("Bullet"))
                {
                    BreakThis();
                }
            }
        
            if (other.CompareTag("Player") || other.CompareTag("HealthItem"))
            {
                FindObjectOfType<PhoneComboManager>().IncrementRank();
            
                if (other.CompareTag("HealthItem"))
                {
                    Destroy(other.gameObject);
                }
            }
        }

        private void BreakThis()
        {
            this._col.enabled = false;
            this._rb.velocity = this._rb.velocity / 10;
            _audioManager.Play("BrokenObject");
            if (_anim != null)
            {
                _anim.SetBool("Broken", true);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}