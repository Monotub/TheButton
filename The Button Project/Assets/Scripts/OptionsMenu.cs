using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] Slider masterVol;
    [SerializeField] Slider musicVol;
    [SerializeField] Slider sfxVol;
    [SerializeField] TMP_Text masterText;
    [SerializeField] TMP_Text musicText;
    [SerializeField] TMP_Text sfxText;
    [SerializeField] TMP_Dropdown diffDropdown;



    private void OnEnable()
    {
        SetOptionValues();

    }

    private void Update()
    {
        masterText.text = (masterVol.value * 100f).ToString("n0") + " %";
        musicText.text = (musicVol.value * 100f).ToString("n0") + " %";
        sfxText.text = (sfxVol.value * 100f).ToString("n0") + " %";
    }

    private void SetOptionValues()
    {
        masterVol.value = PlayerPrefs.GetFloat("Master Volume");
        musicVol.value = PlayerPrefs.GetFloat("Music Volume");
        sfxVol.value = PlayerPrefs.GetFloat("SFX Volume");
        diffDropdown.value = PlayerPrefs.GetInt("Difficulty");
    }

    public void SaveSettings()
    {
        GameManager.Instance.masterVolumeValue = masterVol.value;
        GameManager.Instance.musicVolumeValue = musicVol.value;
        GameManager.Instance.sfxVolumeValue = sfxVol.value;
        GameManager.Instance.difficulty = diffDropdown.value;
        
        GameManager.Instance.SavePlayerPrefs();
        gameObject.SetActive(false);
    }

    public void ResetDefaultSettings()
    {
        GameManager.Instance.RestoreDefaultPrefs();
        SetOptionValues();
    }

    public void ShowOptionsMenu()
    {
        gameObject.SetActive(true);
    }

    public void CancelOptionsButton()
    {
        gameObject.SetActive(false);
    }

    public int GetDifficulty()
    {
        return diffDropdown.value;
    }
}

public enum Difficulty
{
    Casual, 
    Normal, 
    Hardcore
}