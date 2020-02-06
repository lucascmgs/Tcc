using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;

namespace DefaultNamespace
{
    public class UIHealthManager : MonoBehaviour
    {
        
        public GameObject Player;

        [SerializeField] 
        private Sprite[] sprites;
        
        private SubHealthManager playerHealthManager;

        private Image image;

        private void Start()
        {
            playerHealthManager = Player.GetComponent<SubHealthManager>();
            image = GetComponent<Image>();
        }

        private void Update()
        {
            int currentHealth = playerHealthManager.Health;
            image.sprite = sprites[currentHealth];
        }
    }
}