using System;
using UnityEngine;
using DG.Tweening;

public class ExtraLifePowerUp : MonoBehaviour
{
    [SerializeField] ParticleSystem visuals;

    public static event Action<float> OnPowerupAquired;

    void Start()
    {
        StartPowerupAnimations();
    }

    private void StartPowerupAnimations()
    {
        visuals.transform.DOMoveY(visuals.transform.position.y + 0.5f, 1f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Shield")
        {
            OnPowerupAquired?.Invoke(0f);
            GameManager.Instance.AddHealth(1);
            Destroy(gameObject);
        }
    }
}
