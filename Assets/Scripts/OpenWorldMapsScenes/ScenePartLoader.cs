using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePartLoader : MonoBehaviour
{
    bool shouldLoad;
    bool isLoaded;

    private void Start()
    {
        shouldLoad = false;
        if (SceneManager.sceneCount > 0) {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene scene = SceneManager.GetSceneAt(i);
                if (scene.name == gameObject.name)
                {
                    isLoaded = true;
                }
                else 
                    isLoaded = false;
            }
        }
    }

    private void Update()
    {
        TriggerCheck();
    }

    private void TriggerCheck()
    {
        if (shouldLoad)
        {
            LoadScene();
        }
        else {
            UnLoadScene();
        }
    }

    private void UnLoadScene()
    {
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }

    private void LoadScene()
    {
        if (!isLoaded) {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.tag.Equals("Player")) {
            shouldLoad = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            shouldLoad = false;
        }
    }
}
