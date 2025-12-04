using System;
using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 dir;
    public gameManager gm;
    public GameObject destroyParticles;
    [SerializeField] private int speed = 10;
    [SerializeField] private int speedOfSpeedIncreasing = 5;
    [SerializeField] private int jumpForce = 10;
    [SerializeField] private int moveForce = 5;
    [SerializeField] private float gravity = 17;
    [SerializeField] private float distanceBetweenLines = 5;
    [SerializeField] private int health = 3;
    [SerializeField] private int currentLine = 1;
    [SerializeField] private bool isIndestructible = false;
    [SerializeField] private float indestructibleTimeAfterHit = 0.2f;
    [SerializeField] private bool isDead = false;
    [SerializeField] private float timeBetweenSpeedIncrease = 5.0f;

    private const int maxSpeed = 72;
    private Coroutine indestructibleCoroutine = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        StartCoroutine(IncreaseSpeed());
    }

    private void OnEnable()
    {
        InputManager.Instance.actions.Player.ChangeLine.performed += ChangeLine;
        InputManager.Instance.actions.Player.Jump.performed += Jump;
    }
    
    private void OnDisable()
    {
        InputManager.Instance.actions.Player.ChangeLine.performed -= ChangeLine;
        InputManager.Instance.actions.Player.Jump.performed -= Jump;
    }
    
    private void ChangeLine(InputAction.CallbackContext ctx)
    {
        if (ctx.control.name == "d" || ctx.control.name == "rightArrow")
        {
            if (currentLine < 2) currentLine++; // линий 3
        }
        
        if (ctx.control.name == "a" || ctx.control.name == "leftArrow")
        {
            if (currentLine > 0) currentLine--;
        }
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (controller.isGrounded) dir.y = jumpForce;
    }

    private void FixedUpdate()
    {
        dir.x = ((currentLine - 1) * distanceBetweenLines - transform.position.x) * moveForce;
        dir.z = speed;
        dir.y -= gravity * Time.fixedDeltaTime;
        if (dir.y < -10) dir.y = -10;
        controller.Move(dir * Time.fixedDeltaTime);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.gameObject.CompareTag("obstacle")) return;
        if (!isIndestructible)
        {
            int damage = hit.gameObject.GetComponent<Obstacle>().damage;
            health = Mathf.Max(health - damage, 0);
            gm.UpdateHealth(health);
            if (health == 0)
            {
                Die();
                gm.EndRun();
            }

            if (!isDead)
            {
                if (indestructibleCoroutine != null) StopCoroutine(indestructibleCoroutine);
                indestructibleCoroutine = StartCoroutine(SetIndestructible(indestructibleTimeAfterHit));
            }
        }

        hit.gameObject.GetComponent<Obstacle>().Die();
        Destroy(hit.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("heartBonus"))
        {
            if (health < 3) gm.UpdateHealth(++health);
        } else if (other.gameObject.CompareTag("indestructibleBonus"))
        {
            float duration = other.gameObject.GetComponent<IndestructibleBonus>().duration;
            gm.StartIndestructibleBonus(duration);
            if (indestructibleCoroutine != null) StopCoroutine(indestructibleCoroutine);
            indestructibleCoroutine = StartCoroutine(SetIndestructible(duration));
        }
        
        Destroy(other.gameObject);
    }

    private void Die()
    {
        isDead = true;
        Instantiate(destroyParticles, transform.position, Quaternion.identity);
        this.gameObject.SetActive(false);
    }

    private IEnumerator SetIndestructible(float time)
    {
        isIndestructible = true;
        yield return new WaitForSeconds(time);
        isIndestructible = false;
        indestructibleCoroutine = null;
    }

    private IEnumerator IncreaseSpeed()
    {
        yield return new WaitForSeconds(timeBetweenSpeedIncrease);

        speed += speedOfSpeedIncreasing;
        if (speed + speedOfSpeedIncreasing < maxSpeed)
            StartCoroutine(IncreaseSpeed());
    }
}
