using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    TMP_Text levelText;
    TMP_Text healthDetail;
    Image healthSlider;
    Image expSlider;

    private void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<TMP_Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        healthDetail = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TMP_Text>();
    }

    private void Update()
    {
        levelText.text = "Level " + GameManager.Instance.playerStates.characterData.currentLevel.ToString("00");
        healthDetail.text = GameManager.Instance.playerStates.CurrentHealth.ToString("000") + "/" + GameManager.Instance.playerStates.MaxHealth.ToString("000");
        UpdateHealt();
        ExpUpdate();
    }

    void UpdateHealt() {
        healthSlider.fillAmount = (float)GameManager.Instance.playerStates.CurrentHealth / GameManager.Instance.playerStates.MaxHealth;
    }

    void ExpUpdate() {
        expSlider.fillAmount = (float)GameManager.Instance.playerStates.characterData.currentExp / GameManager.Instance.playerStates.characterData.baseExp;
    }
}
