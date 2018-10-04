using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;
    public float exp = 0;
    public int level = 1;
    public bool inCutscene = true;

    public Text levelText;
    public Text expText;

    public Vector3 currentCameraPos;
    public Transform[] cameraPositions;
    public BoxCollider2D[] cameraTriggers;
    public BoxCollider2D[] interactionTriggers;

    //Interactions
    public bool i_plushies1 = false;
    public bool i_plushies2 = false;
    public bool i_figures1 = false;
    public bool i_figures2 = false;
    public bool i_figures3 = false;
    public bool i_keyboard = false;

    //Events
    public bool e_room1door = false;

    private void Awake()
    {
        KeepGameManager();
    }

    private void Update()
    {
        levelText.text = "LV: " + level.ToString();
        expText.text = "EXP: " + exp.ToString();
    }

    private void KeepGameManager() //Keeps the GameManager across scenes.
    {
        if (gameManager == null)
            gameManager = this;
        else if (gameManager != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
}