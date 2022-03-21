using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : MonoBehaviour
{

    public SceneFader sceneFaderPrefab;


    private void OnMouseUp()
    {

        //if (Vector3.Distance(transform.position, GameManager.Instance.playerStates.transform.position) < 2) {
        //    Debug.Log("Please heal me, priest!");
        //    StartCoroutine(HealPlayer());
        //}
    }

    public void StartHeal() {
        StartCoroutine("HealPlayer");
    }

    IEnumerator HealPlayer()
    {
        AudioManager.Instance.PlaySfx("Recover");
        SceneFader fade = Instantiate(sceneFaderPrefab);

        yield return StartCoroutine(fade.FadeOut(2f));

        GameManager.Instance.playerStates.GetFullStates();
        LevelManager.Instance.ResetEnemies();

        //LevelManager.Instance.SaveData();

        yield return StartCoroutine(fade.FadeIn(2f));

        yield break;
    }


}
