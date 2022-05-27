using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveOnDeath : MonoBehaviour
{
    Shader dissolveShader;
    SkinnedMeshRenderer rend;
    bool canDissolve = false;
    float t = 0.0f;
    float speed = 1f;
    Color dissolveLineColor = new Color(0, 1, 0);


    private void OnEnable()
    {
        Enemy.onEnemyDied += onEnemyDeath;
    }
    private void OnDisable()
    {
        Enemy.onEnemyDied -= onEnemyDeath;
    }

    private void Awake()
    {
        dissolveShader = Shader.Find("Ultimate 10+ Shaders/Dissolve");
        rend = GetComponentInChildren<SkinnedMeshRenderer>();
    }

    private void Update()
    {
        Dissolve();
    }

    private void Dissolve()
    {
        if (canDissolve)
        {
            Material[] mats = rend.materials;

            foreach (var material in mats)
            {
                material.shader = dissolveShader;
                material.SetColor("_EdgeColor", dissolveLineColor);
                material.SetFloat("_Cutoff", Mathf.Lerp(0, 1, t * speed));
            }

            t += Time.deltaTime;

            //rend.materials = mats;
        }
    }

    void onEnemyDeath(GameObject enemy)
    {
        if(enemy == gameObject)
            canDissolve = true;
    }
}
