using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHP : MonoBehaviour
{
    GameManager gameManager;

    public int hp;
    public float damageEXP;
    public float triggerEXP;
    ItemShake damageAni;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        damageAni = GetComponent<ItemShake>();
    }

    public void Interact()
    {
        if (hp == 0)
        {
            hp--;
            gameManager.exp += triggerEXP;
            GetComponent<Animator>().SetTrigger("trigger");
        }
        else if (hp > 0)
        {
            hp--;
            gameManager.exp += damageEXP;
            if (damageAni != null)
                damageAni.isPlaying = true;
        }
    }
}
