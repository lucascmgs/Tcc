using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBulletsIndicator : MonoBehaviour
{
    [SerializeField] private GameObject Gun;

    [SerializeField] private float distanceBetweenIndicators = 1;

    private GunBehaviour _gunBehaviour;

    [SerializeField] private GameObject bulletIndicatorPrefab;

    private Stack<GameObject> bullets;

    private RectTransform _rectTransform;

    private Vector3 newAnchoredPosition;
    
    void Start()
    {
        _gunBehaviour = Gun.GetComponent<GunBehaviour>();
        _rectTransform = this.GetComponent<RectTransform>();
        newAnchoredPosition = _rectTransform.pivot;
        
        bullets = new Stack<GameObject>();
        
    }

    void Update()
    {
        var newCount = _gunBehaviour.maxBullets - _gunBehaviour.bulletCount;

        while (newCount != bullets.Count)
        {
            if (newCount > bullets.Count)
            {
           
                var newBulletIndicator = Instantiate(bulletIndicatorPrefab, this.transform);
            
            
                var newRectTrans = newBulletIndicator.GetComponent<RectTransform>();
                newRectTrans.pivot = _rectTransform.pivot;
                newRectTrans.anchoredPosition = newAnchoredPosition;

                newAnchoredPosition.x += distanceBetweenIndicators;
                bullets.Push(newBulletIndicator);
            }

            if (newCount < bullets.Count)
            {
                newAnchoredPosition.x -= distanceBetweenIndicators;
                var lastBullet = bullets.Pop();
                Destroy(lastBullet);
            }
        }
    }
}
