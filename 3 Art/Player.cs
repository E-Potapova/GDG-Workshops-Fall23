using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // SerializeField means we can view and edit this value through the Inspector.
    // speed is how fast our Player moves left and right.
    // 'float' is a type saying it is a floating-point number, AKA decimal
    [SerializeField] float speed = 5.0f;
    // jumpForce is how fast our Player moves up when jumping.
    [SerializeField] float jumpForce = 5.0f;
    // canJump is used to keep track is the Player is allowed to jump or not.
    // by default 'true' since the Player starts on the ground.
    // 'bool' is a type that can only be 'true' or 'false'.
    bool canJump = true;
    // this is where we will save the Rigidbody2D Component (has type 'Rigidbody2D').
    // need this to affect Player physics (like moving and jumping).
    // declaring rb2D so that we can use it throughout our Player class,
    // but setting it later in our Start() method
    Rigidbody2D rb2D;
    // declare and initialize a health amount for our Player.
    // I set it to an integer type because I want any damage source to
    // deal 1 damage to my Player, but you can design it how you'd like
    int health = 5;

    // Start is called when the Player is first created in the world
    void Start()
    {
        // get the Rigibody2D component and save it into our variable.
        // we don't specify type here since we did so in line 15.
        // 'gameObject' refers to the GameObject that the script is
        // attached to (so the Player GameObject)
        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // GetAxisRaw checks if the user is pressing Horizontal buttons as defined on the Horizontal Axis.
        // (check all Axes in Edit -> Project Settings -> Input Manager -> Axes)
        // returns -1 if pressing left, 0 if not pressing anything, and 1 if pressing right.
        float direction = Input.GetAxisRaw("Horizontal");
        // update the velocity of the Player based on the direction and speed.
        // the Y-value of the velocity doesn't change since moving left/right doesn't
        // affect how much we move up/down.
        // the '.' notation means we are accessing a property specifically of our rb2D variable
        rb2D.velocity = new Vector2(direction * speed, rb2D.velocity.y);

        // first check if we canJump; if it is set to false, we ignore the rest.
        // if it is true, check if the user pressed a Vertical up button (to jump).
        // we check if it is > 0 since 1 means the user pressed up, and -1 means they pressed down.
        if (canJump && Input.GetAxisRaw("Vertical") > 0)
        {
            // update the velocity of the Player based on our jumpForce, the Y-value.
            // the X-value doesn't change since jumping doesn't affect horizontal movement.
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpForce);
            // we don't want the Player to jump anytime we press the Jump button.
            // for example, the Player shouldn't jump again when already in the air.
            canJump = false;
        }
    }

    // built-in function provided by Unity.
    // this is called for us whenever the Player hits something that has a Collider component.
    // 'collision' is all information stored about the impact.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 'collision.gameObject' gives us the GameObject that the Player has collided with.
        // '.tag' gives us the Tag of the object, as seen in the Inspector below the object name.
        // we check what this tag is to check if we are colliding with a type of object.

        // check if we collided with a GameObject with Tag 'Ground'
        if (collision.gameObject.tag == "Ground") 
        {
            // inside here, we know the Player has touched Ground,
            // so we tell the Player that thay are allowed to jump again.
            canJump = true;
        }
    }

    // built-in function provided by Unity.
    // this is called for us whenever the Player hits something that has a Collider component,
    // but has 'Is Trigger' set to true.
    // 'collision' is all information stored about the trigger.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // what's the Tag of the GameObject we're colliding with?
        if (collision.gameObject.tag == "Finish")
        {
            // Debug.Log() shows our message in the Console tab, can be
            // found next to the Project tab
            Debug.Log("Goal reached!");
        }
        else if (collision.gameObject.tag == "Damage")
        {
            // is this is any Damage source, decrease our Player's health
            health--;
            Debug.Log("Health: " + health);
            // if the Player's health is below 0, we want the Player to die,
            // easiest way to do that is to delete or  get rid of the Player GameObject.
            // you can see Destroy() gets rid of the GameObject in the Hierarchy tab
            if (health < 1)
            {
                Destroy(gameObject);
                Debug.Log("You died :(");
            }
        }
    }
}
