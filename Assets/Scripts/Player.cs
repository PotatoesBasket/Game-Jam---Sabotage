using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Handles player movement and updates animation state accordingly.

    GameManager gameManager;
    Rigidbody2D player;
    SpriteRenderer sprite;
    Animator animator;
    AnimatorClipInfo[] clip;

    AudioSource meow;

    readonly float speed = 90f;
    readonly float zoomSpeed = 180f;
    float cutsceneTimer = 0f;

    bool zoomieMode = false;
    bool allowMovement = true;
    bool allowAction = true;

    bool canPickUpItem = false;
    GameObject grabbableItem = null;
    GameObject currentItem = null;
    public Transform itemPos;

    enum Interactions
    {
        None,
        Start,
        BedOff,
        BedOn,
        DeskOn,
        DeskOff,
        Plushies1,
        Plushies2
    }
    Interactions availableInteraction = Interactions.Start;

    private void Awake()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        player = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!gameManager.inCutscene)
        {
            Movement();
            Actions();
        }
        else
        {
            Cutscene();
        }
    }

    private void Movement()
    {
        if (allowMovement)
        {
            float currentSpeed;

            if (zoomieMode)
                currentSpeed = zoomSpeed;
            else
                currentSpeed = speed;

            Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            player.velocity = movement * currentSpeed * Time.deltaTime;

            if (Input.GetAxis("Horizontal") > 0) //move right
            {
                sprite.flipX = false;
                SetMoveAnimation();
            }

            if (Input.GetAxis("Horizontal") < 0) //move left
            {
                sprite.flipX = true;
                SetMoveAnimation();
            }

            if (Input.GetAxis("Vertical") > 0 || Input.GetAxis("Vertical") < 0) //move vertically
            {
                SetMoveAnimation();
            }

            if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) //stopped
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isZooming", false);
            }
        }
        else
        {
            player.velocity = Vector2.zero;
        }

        if (Input.GetButton("Fire3")) //hold left shift to run
        {
            zoomieMode = true;
        }
        else
        {
            zoomieMode = false;
        }
    }

    private void Actions()
    {
        if (availableInteraction == Interactions.Start) //press button to start
        {
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetButtonDown("Jump"))
            {
                animator.SetTrigger("Start");
                gameManager.inCutscene = false;
                availableInteraction = Interactions.None;
            }
        }

        if (Input.GetButtonDown("Jump")) //space to interact
        {
            if (availableInteraction == Interactions.None && canPickUpItem && currentItem == null)
            {
                currentItem = grabbableItem;
                currentItem.GetComponent<GrabbableItem>().Attach();
            }
            else if (currentItem != null)
            {
                currentItem.GetComponent<GrabbableItem>().Detach();
                currentItem = null;
            }
            else if (availableInteraction != Interactions.None && availableInteraction != Interactions.Start)
            {
                animator.SetTrigger(availableInteraction.ToString());
                animator.SetBool("isWalking", false);
                animator.SetBool("isZooming", false);
                player.velocity = Vector2.zero;
                gameManager.inCutscene = true;

                switch (availableInteraction)
                {
                    case Interactions.BedOff:
                        if (sprite.flipX == false)
                        {
                            player.transform.position = new Vector2(-0.5f, -3f);
                        }
                        break;

                    case Interactions.BedOn:
                        if (sprite.flipX == true)
                        {
                            player.transform.position = new Vector2(-3.5f, -1f);
                            sprite.flipX = true;
                        }
                        break;

                    case Interactions.DeskOn:
                        if (sprite.flipX == false)
                        {
                            player.transform.position = new Vector2(-0.8f, 0.85f);
                        }
                        break;

                    case Interactions.DeskOff:
                        if (sprite.flipX == true)
                        {
                            player.transform.position = new Vector2(-4f, 0f);
                        }
                        break;

                    case Interactions.Plushies1:
                        gameManager.i_plushies1 = true;
                        gameManager.interactionTriggers[(int)availableInteraction - 2].GetComponentInParent<ItemHP>().Interact();
                        break;

                    case Interactions.Plushies2:
                        break;
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.X) && allowAction) //x to meow
        {
            meow.Play();
        }
    }

    private void Cutscene()
    {
        clip = animator.GetCurrentAnimatorClipInfo(0);

        if (cutsceneTimer <= clip.Length)
        {
            cutsceneTimer += Time.deltaTime;
            player.velocity = Vector2.zero;
        }
        else
        {
            gameManager.inCutscene = false;
            allowMovement = true;
            allowAction = true;
            cutsceneTimer = 0f;
        }
    }

    private void SetMoveAnimation()
    {
        animator.SetBool("isWalking", true);

        if (zoomieMode)
            animator.SetBool("isZooming", true);
        else
            animator.SetBool("isZooming", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractionTrigger")
        {
            int offset = 2;

            for (int i = 0; i < gameManager.interactionTriggers.Length; i++)
            {
                if (collision.gameObject == gameManager.interactionTriggers[i].gameObject)
                {
                    availableInteraction = (Interactions)i + offset;
                    break;
                }
            }
        }

        if (collision.gameObject.tag == "Item")
        {
            canPickUpItem = true;
            grabbableItem = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "InteractionTrigger")
        {
            availableInteraction = Interactions.None;
        }

        if (collision.gameObject.tag == "Item")
        {
            canPickUpItem = false;
            grabbableItem = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "CameraTrigger")
        {
            for (int i = 0; i < gameManager.cameraTriggers.Length; i++)
            {
                if (collision.GetComponent<BoxCollider2D>() == gameManager.cameraTriggers[i])
                {
                    if (sprite.flipX == false)
                    {
                        gameManager.currentCameraPos = gameManager.cameraPositions[i + 1].position;
                        gameManager.GetComponent<CameraMovement>().UpdatePosition();
                    }
                    else
                    {

                    }

                    break;
                }
            }
        }
    }
}