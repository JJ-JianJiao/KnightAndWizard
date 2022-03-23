using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.SceneManagement;

public class MouseManager : Singleton<MouseManager>
{
    RaycastHit hitInfo;

    //mouse left click events on different object
    public event Action<Vector3> OnMouseClicked;
    public event Action<GameObject> OnEnemyClicked;

    public event Action ClearAllClickTarget;

    //cursor's texture2D
    public Texture2D point, doorway, attack, target, arrow;

    private GameObject mainDogKnight;
    private GameObject mainWizard;
    private MainMenu mainMenu;


    protected override void Awake()
    {
        base.Awake();
        if (SceneManager.GetActiveScene().name == "Main") {
            mainDogKnight = GameObject.FindGameObjectWithTag("MainDogKnight");
            mainWizard = GameObject.FindGameObjectWithTag("MaintWizard");
            mainMenu = FindObjectOfType<MainMenu>();
        }
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        //TODO: this part is for testing
        if (SceneManager.GetActiveScene().name == "Main" && mainDogKnight == null && mainWizard == null && mainMenu == null)
        {
            mainDogKnight = GameObject.FindGameObjectWithTag("MainDogKnight");
            mainWizard = GameObject.FindGameObjectWithTag("MaintWizard");
            mainMenu = FindObjectOfType<MainMenu>();
        }


        SetCursorTexture();
        if (!InteractWithUI())
            MouseControl();
        else
            return;
    }

    void SetCursorTexture() {

        //Camera.main - ccessing this property has a small CPU overhead, comparable to calling GameObject.GetComponent.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (InteractWithUI()) {
            Cursor.SetCursor(point, Vector2.zero, CursorMode.Auto);
            return;
        }

        if (Physics.Raycast(ray, out hitInfo)) {
            //TODO: Switch the cursor
            switch (hitInfo.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                case "Attackable":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;  
                case "Item":
                case "NPC":
                case "ChestContainer":
                    Cursor.SetCursor(point, Vector2.zero, CursorMode.Auto);
                    break;
                case "MainDogKnight":
                case "MaintWizard":
                    Cursor.SetCursor(point, Vector2.zero, CursorMode.Auto);
                    break;
                default:
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }

    }

    void MouseControl() {
        if (Input.GetMouseButtonDown(0) && hitInfo.collider != null) {

            if (hitInfo.collider.gameObject.CompareTag("Ground")) {
                OnMouseClicked?.Invoke(hitInfo.point);
                ClearAllClickTarget?.Invoke();

            }
            if (hitInfo.collider.gameObject.CompareTag("Enemy"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Attackable"))
            {
                OnEnemyClicked?.Invoke(hitInfo.collider.gameObject);
            }
            if (hitInfo.collider.gameObject.CompareTag("Portal"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("Item"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("ChestContainer"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }
            if (hitInfo.collider.gameObject.CompareTag("NPC"))
            {
                OnMouseClicked?.Invoke(hitInfo.point);
            }


            if (hitInfo.collider.gameObject.CompareTag("MainDogKnight")) {
                //mainDogKnight.SetActive(true);

                mainDogKnight.GetComponent<Animator>().SetTrigger("ShowOff");

                hitInfo.collider.transform.GetChild(3).gameObject.SetActive(true);
                mainWizard.transform.GetChild(2).gameObject.SetActive(false);
                mainWizard.GetComponent<Collider>().enabled = false;
                mainMenu.DisplayKnightCharacterBoard();
            }

            if (hitInfo.collider.gameObject.CompareTag("MaintWizard"))
            {
                //mainWizard.SetActive(true);
                mainWizard.GetComponent<Animator>().SetTrigger("ShowOff");


                hitInfo.collider.transform.GetChild(2).gameObject.SetActive(true);
                mainDogKnight.transform.GetChild(3).gameObject.SetActive(false);
                //mainDogKnight.SetActive(false);
                mainDogKnight.GetComponent<Collider>().enabled = false;
                mainMenu.DisplayWizardCharacterBoard();

            }

        }
    }


    bool InteractWithUI() {

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) {
            return true;
        }
        else
        {
            return false;
        }
        
    }

    //TODO: aaaaaaaaa add method next time


}
