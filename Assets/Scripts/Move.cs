using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody))]
public class Move : MonoBehaviour
{
    Rigidbody rb;

    public float speed;
    public float sprintModifier;

    public float stamina, staminaRate, maxStamina;

    public bool isWalking, lockPos;

    public Slider staminaBar;

    public float rigidbodyVelocity;

    public float minVelocity, maxVelocity;

    public Transform cam;

    public static Vector3 transPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        if (PlayerManager.transitioning)
        {
            transform.position = transPos;
        }
    }

    private void FixedUpdate()
    {
        if (!PlayerManager.transitioning & !lockPos)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");

            //Booleans
            bool sprint = Input.GetKey(KeyCode.LeftShift) & stamina >= 0f || Input.GetKey(KeyCode.RightShift) & stamina >= 0f;
            bool isSprinting = sprint && y > 0;

            //Movement
            Vector3 direction = new Vector3(x, 0, y).normalized;
            direction.Normalize();

            float adjustedSpeed = speed;
            if (isSprinting)
            {
                adjustedSpeed *= sprintModifier;
            }
            else
            {
                adjustedSpeed = speed;
            }

            Vector3 targetVelocity = transform.TransformDirection(direction) * adjustedSpeed * Time.deltaTime;
            targetVelocity.y = rb.velocity.y;
            rb.velocity = targetVelocity;

            //Stamina bar (referenced from baldi)
            if (isSprinting)
            {
                if (stamina > 0f)
                {
                    stamina -= staminaRate * Time.deltaTime;
                }
                if (stamina < 0f & stamina > -5f)
                {
                    stamina = -5f;
                }
            }
            else if (stamina < maxStamina && !isWalking)
            {
                stamina += staminaRate * Time.deltaTime;
            }
            staminaBar.value = stamina / maxStamina * 100f;
            rigidbodyVelocity = rb.velocity.magnitude;
        }

        
    }
    private void Update()
    {
        if (rigidbodyVelocity > minVelocity && rigidbodyVelocity < maxVelocity)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }

        rb.isKinematic = lockPos;
    }
}
