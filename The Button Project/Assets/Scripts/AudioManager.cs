using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] AudioSource sfx;
    [SerializeField] AudioSource music;
    [Header("Audio Clips")]
    [SerializeField] AudioClip[] buttonClickClips;
    [SerializeField] AudioClip[] enemyDeathSounds; // 0: Button, 1: Shields


    private void Start()
    {
        StartCoroutine(StartFade(music, 3f, 0.75f));
    }

    private void Update()
    {

    }
    private void OnEnable()
    {
        Button.OnButtonClicked += PlayButtonClick;
        Enemy.OnShieldDeath += EnemyShieldDeath;
        Enemy.OnButtonDeath += EnemyButtonDeath;
    }

    private void OnDisable()
    {
        Button.OnButtonClicked -= PlayButtonClick;
        Enemy.OnShieldDeath -= EnemyShieldDeath;
        Enemy.OnButtonDeath -= EnemyButtonDeath;
    }

    private void EnemyButtonDeath()
    {
        sfx.PlayOneShot(enemyDeathSounds[0]);
    }

    private void EnemyShieldDeath()
    {
        sfx.pitch = UnityEngine.Random.Range(0.9f, 1.05f);
        sfx.PlayOneShot(enemyDeathSounds[1]);
    }

    private void PlayButtonClick()
    {
        AudioClip clip;
        int clipChoice = UnityEngine.Random.Range(0, buttonClickClips.Length);

        clip = buttonClickClips[clipChoice];

        sfx.PlayOneShot(clip);
    }
    public IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        audioSource.Play();
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
}
