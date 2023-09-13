using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;

public class EnergyFlowController : MonoBehaviour
{
    private PlayerInput input;
    private Rigidbody2D rb;
    private HackingManager manager;

    [Header("Movement")]
    [SerializeField]
    private float verticalSpeed = 5f;
    [SerializeField]
    private float brakeFactor = 0.5f;
    [SerializeField]
    private float accelerationFactor = 1.5f;
    [SerializeField]
    private float horizontalSpeed = 3f;

    private Vector2 inputMovement;
    public bool canMove = false;

    [Header("Repel")]
    [SerializeField]
    private float repelForce = 500f;
    [SerializeField]
    private float repelTime = 0.15f;

    private List<EnergyPoint> energy = new();

    [Header("Audio")]
    [SerializeField]
    private AudioClip addPointSound;
    [SerializeField]
    private AudioClip damageSound;
    [SerializeField]
    private AudioClip destroySound;
    private AudioSource audioSource;

    void Start()
    {
        manager = GameObject.Find("Hacking Manager").GetComponent<HackingManager>();
        rb = GetComponent<Rigidbody2D>();
        InitInputActions();
        InitAudioSource();
    }

    private void InitInputActions()
    {
        input = PlayerManager.Instance.getPlayerInput();

        InputAction moveAction = input.actions[PlayerConstants.ActionMove];
        moveAction.performed += (ctx) => { inputMovement = ctx.ReadValue<Vector2>().normalized; };
        moveAction.canceled += (ctx) => { inputMovement = Vector2.zero; };
    }

    private void InitAudioSource()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (canMove)
        {
            if (!audioSource.isPlaying)
            {
                SoundManager.Instance.PlayAudioSource(audioSource, 60);
            }
            Move();
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }

    private void Move()
    {
        float verticalVelFactor = 1f;
        if (inputMovement.y < 0)
        {
            verticalVelFactor = brakeFactor;
        }
        else if (inputMovement.y > 0)
        {
            verticalVelFactor = accelerationFactor;
        }

        audioSource.pitch = verticalVelFactor;
        rb.velocity = new Vector2(inputMovement.x * horizontalSpeed, verticalVelFactor * verticalSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnergyPoint point = collision.GetComponent<EnergyPoint>();
        if (point && !energy.Contains(point))
        {
            AddEnergyPoint(point);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (canMove)
        {
            if (energy.Count > 0)
            {
                StartCoroutine(Repel(collision));
                RemoveEnergyPoint(energy[^1]);
            }
            else
            {
                audioSource.Stop();
                SoundManager.Instance.PlayEffectOneShot(destroySound);
                Stop();
                manager.Fail();
            }
        }
    }

    private void AddEnergyPoint(EnergyPoint point)
    {
        SoundManager.Instance.PlayEffectOneShot(addPointSound);
        if (energy.Count > 0)
        {
            point.SetTarget(gameObject, energy[^1].gameObject);
        }
        else
        {
            point.SetTarget(gameObject, gameObject);
        }
        energy.Add(point);
    }

    private void RemoveEnergyPoint(EnergyPoint point)
    {
        GameObject pointGo = point.gameObject;
        energy.Remove(point);
        Destroy(pointGo);
    }

    private IEnumerator Repel(Collision2D collision)
    {
        audioSource.Stop();
        SoundManager.Instance.PlayEffectOneShot(damageSound);
        canMove = false;
        rb.velocity = new Vector2(0, 0);

        Vector2 dir = collision.GetContact(0).point - new Vector2(transform.position.x, transform.position.y);
        dir = -dir.normalized;
        rb.AddForce(dir * repelForce);

        yield return new WaitForSeconds(repelTime);

        canMove = true;
    }

    public void Stop()
    {
        canMove = false;
        rb.velocity = new Vector2(0, 0);
    }

    public bool CanMove { get => canMove; set => canMove = value; }
    public List<EnergyPoint> Energy { get => energy; }
}
