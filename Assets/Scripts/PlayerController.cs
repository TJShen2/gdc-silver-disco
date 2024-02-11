using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{   
    [SerializeField] private Transform spawnpoint;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask lavaLayer;
    [SerializeField] private Rigidbody2D movingPlatform;

    public Vector2 InputVector { get; set; }

    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private Animator animator;

    Color spriteColor = new Color(255,255,255);

    #region Terrain
        private bool onGround;
        private bool onLava;
        public bool OnMovingPlatform { get; set; }
    #endregion

    private bool isDead;
    public bool IsDead {
        get { return isDead; }
        set { isDead = value; }
    }
    private bool isJumpImmune;
    private bool canJump;
    [SerializeField] private float jumpImmunityDelay;
    [SerializeField] private float jumpImmunityDuration;

    private float energy;
    public float Energy {
        get { return energy; }
        set { energy = value; } 
    }
    [SerializeField] private float startingEnergy;
    [SerializeField] private float jumpEnergyConsumption;

    public void Start()
    {
        // Get reference to the player's Rigidbody2D and animator
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        spriteColor = sr.sharedMaterial.color;

        // Set jump variables
        canJump = true;
        isJumpImmune = false;

        // Set energy
        energy = startingEnergy;
    }

    public void Update() {
        CheckTerrain();
    }

    private void CheckTerrain() {
        onGround = Physics2D.OverlapBox(transform.position, transform.localScale, 0f, groundLayer);
        onLava = Physics2D.OverlapBox(transform.position, transform.localScale, 0f, lavaLayer);

        if (!onGround && onLava) {
            StartCoroutine(DeathSequence());
        }
    }

    public void FixedUpdate()
    {
        // Update velocity
        if (OnMovingPlatform)
            rb2d.velocity = movingPlatform.velocity + InputVector * speed * Time.fixedDeltaTime;
        else
            rb2d.velocity = InputVector * speed * Time.fixedDeltaTime;
    }


    // Get input using Unity's "new" input system
    public void Move(InputAction.CallbackContext callbackContext)
    {
        if (isDead)
            return;
        
        InputVector = callbackContext.ReadValue<Vector2>().normalized;
    }
    private IEnumerator JumpSequence() {
        canJump = false;
        yield return new WaitForSeconds(jumpImmunityDelay);
        isJumpImmune = true;
        yield return new WaitForSeconds(jumpImmunityDuration);
        isJumpImmune = false;
        yield return new WaitForSeconds(jumpImmunityDelay);
        canJump = true;
    }
    public void Jump() {
        if (energy >= jumpEnergyConsumption && canJump) {
            energy -= jumpEnergyConsumption;
            StartCoroutine(JumpSequence());
        }
    }
    private IEnumerator DeathSequence() {
        isDead = true;
        InputVector = Vector2.zero;
        yield return StartCoroutine(FadeToDeath(1f));

        yield return new WaitForSeconds(1f);
        Respawn();
    }
    public void Die() {
        if (!isJumpImmune) {
            StartCoroutine(DeathSequence());
        }
    }

    private void Respawn() {
        // Stop death and jump coroutines since this is a new life
        StopAllCoroutines();

        // Set the death flag to be false
        // Since we are no longer dead -> respawning
        isDead = false;

        // Reset our position
        transform.position = spawnpoint.position;

        // Reset our color 
        sr.color = spriteColor;

        // Reset jump variables
        canJump = true;
        isJumpImmune = false;

        // Reset energy
        energy = startingEnergy;
    }

    private IEnumerator FadeToDeath(float duration) {
        float timeElapsed = 0f;

        while (timeElapsed < duration) {
            timeElapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(1, 0, timeElapsed / duration);
            sr.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, newAlpha);

            yield return null;
        }
    }
}
