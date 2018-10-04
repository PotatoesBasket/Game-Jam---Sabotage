using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableItem : MonoBehaviour
{
    GameObject player;
    SpriteRenderer sprite;
    BoxCollider2D itemCollider;
    Rigidbody2D body;
    static Vector3 offset = new Vector3(0f, 0.45f, 0f);
    public bool isAttached = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        sprite = GetComponent<SpriteRenderer>();
        itemCollider = GetComponent<BoxCollider2D>();
        body = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (isAttached)
        {
            transform.position = player.transform.position + offset;
        }
    }

    public void Attach()
    {
        isAttached = true;
        itemCollider.enabled = false;
        sprite.sortingOrder = 11;
    }

    public void Detach()
    {
        isAttached = false;
        itemCollider.enabled = true;
        sprite.sortingOrder = 4;
        transform.position -= offset;
        body.velocity = Vector3.zero;
    }
}