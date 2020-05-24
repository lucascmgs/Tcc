using System;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class UIComboLevel : MonoBehaviour
    {
        
        [NonSerialized] public int ComboLevel = 0;
        [SerializeField] private Sprite[] sprites;
        [SerializeField] private Animator animator;
        [SerializeField] private const int MaxComboLevel = 3;
        private Image thisImage;

        private void Start()
        {
            var clientManager = FindObjectOfType<ClientManager>();

            if (clientManager != null)
            {
                clientManager.levelEvent.AddListener(ChangeComboLevel);
            }
            
            thisImage = GetComponent<Image>();
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }
        }

        public void ChangeComboLevel(int newComboLevel)
        {
            ComboLevel = newComboLevel;
            
            if (ComboLevel == MaxComboLevel)
            {
                animator.SetBool("playState", true);
                animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            }
            else
            {
                animator.cullingMode = AnimatorCullingMode.CullCompletely;
                animator.SetBool("playState", false);

                thisImage.sprite = sprites[ComboLevel];
            }
        }
        
        
    }
}