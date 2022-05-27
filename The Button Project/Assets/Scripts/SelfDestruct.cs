using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float timer = 10f;

    private void Update()
    {
        if(timer <= 0)
        {
            Destroy(gameObject);
        }
        timer -= Time.deltaTime;
    }
}
