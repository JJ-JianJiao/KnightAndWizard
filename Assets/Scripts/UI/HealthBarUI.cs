using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;
    public Transform barPoint;
    Image healthSlider;
    Transform UIbar;
    Transform cam;

    public bool alwaysVisible;

    public float visibleTime;

    CharacterStats currentState;

    private float timeLeft;

    private void Awake()
    {
        currentState = GetComponent<CharacterStats>();

        currentState.UpdateHealthBarOnAttack += UpdateHealthBar;
    }
    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace) {
                UIbar =  Instantiate(healthUIPrefab, canvas.transform).transform;
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.gameObject.SetActive(alwaysVisible);

            }
        }
    }

    private void UpdateHealthBar(int currentHealth, int MaxHealth)
    {
        if (currentHealth <= 0)
        {
            //TODO: need to fix here, dont know reason
            if(UIbar != null) UIbar.gameObject.SetActive(true);

            timeLeft = visibleTime;
            healthSlider.fillAmount = 0;
            Destroy(UIbar.gameObject, 0.5f);
        }
        else
        {
            UIbar.gameObject.SetActive(true);
            timeLeft = visibleTime;
            float sliderPercent = (float)currentHealth / MaxHealth;
            healthSlider.fillAmount = sliderPercent;
        }

    }

    private void LateUpdate()
    {
        if (UIbar != null) {
            UIbar.position = barPoint.position;
            UIbar.forward = -cam.forward;

            if (timeLeft <= 0 && !alwaysVisible)
                UIbar.gameObject.SetActive(false);
            else
                timeLeft -= Time.deltaTime;
        }
    }

}
