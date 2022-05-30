using System;
using UnityEngine;
using DG.Tweening;

public class ShieldPowerUp : MonoBehaviour
{
    [SerializeField] Transform shieldGraphic;
    [SerializeField] float duration;

    Vector3 rotationAxes = new Vector3(0,359,0);

    public static event Action<float> OnPowerupAquired;


    void Start()
    {
        StartPowerupAnimations();
    }

    private void StartPowerupAnimations()
    {
        shieldGraphic.DOMoveY(shieldGraphic.transform.position.y + 0.5f, 1f)
                    .SetEase(Ease.InOutSine)
                    .SetLoops(-1, LoopType.Yoyo);
        shieldGraphic.DORotate(rotationAxes, 2f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Shield")
        {
            OnPowerupAquired?.Invoke(duration);
            Destroy(gameObject);
        }
    }
}
