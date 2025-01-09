using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutController : MonoBehaviour
{
    public Animator animator;

    private Transform scoutT;
    public float velocidadMovimiento = 1;

    public static ScoutController instance;

    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool dead = false;


    public AudioSource audioSource;

    public AudioClip jump, kick, punch, victory, walk;
    private float contadorWalk = 0;
    private bool winSound = false;

    void Start()
    {
        // Referencia al transform del modelo
        scoutT = transform;

        if(instance == null)
        {
            instance = this;
        }
    }

    void Update()
    {
        // Resetear las transiciones a un estado Idle si no se detectan inputs
        animator.SetBool("IsWalkingForward", false);
        animator.SetBool("IsWalkingBackwards", false);

        // Leer inputs del teclado
        bool moveForward = Input.GetKey(KeyCode.A); 
        bool moveBackward = Input.GetKey(KeyCode.D); 
        bool moveUp = Input.GetKey(KeyCode.W);
        bool moveDown = Input.GetKey(KeyCode.S); 
        bool blockButton = Input.GetKey(KeyCode.L); // Bloquear
        bool attackQuick = Input.GetKeyDown(KeyCode.J); // Ataque rápido
        bool attackSlow = Input.GetKeyDown(KeyCode.K); // Ataque lento

        contadorWalk += Time.deltaTime;
        // Transiciones de animación y movimiento
        if (moveBackward) // Caminar hacia adelante
        {
            animator.SetBool("IsWalkingForward", true);
            transform.Translate(0, 0, velocidadMovimiento * Time.deltaTime);
            if (contadorWalk > 0.5f)
            {
                audioSource.PlayOneShot(walk);
                contadorWalk = 0;
            }
        }
        else if (moveForward) // Caminar hacia atrás
        {
            animator.SetBool("IsWalkingBackwards", true);
            transform.Translate(0, 0, -velocidadMovimiento * Time.deltaTime);
            if (contadorWalk > 0.5f)
            {
                audioSource.PlayOneShot(walk);
                contadorWalk = 0;
            }
        }
        else if (blockButton && moveDown) // Esquivar ataque alto
        {
            animator.SetTrigger("DodgingHigh");
        }
        else if (blockButton && moveUp) // Esquivar ataque bajo (salto en el lugar)
        {
            animator.SetTrigger("Jumping");
            audioSource.PlayOneShot(jump);
        }
        else if (moveDown && attackQuick) // Ataque bajo rápido
        {
            animator.SetTrigger("LowQuickAttack");
        }
        else if (moveDown && attackSlow) // Ataque bajo lento
        {
            animator.SetTrigger("LowSlowAttack");
        }
        else if (attackQuick) // Ataque rápido
        {
            animator.SetTrigger("QuickAttack");
        }
        else if (attackSlow) // Ataque lento
        {
            animator.SetTrigger("SlowAttack");
        }

        // Ajustes de movimiento
        scoutT.position = new Vector3(0, scoutT.position.y, scoutT.position.z);


        UpdateStates();
    }

    private void UpdateStates()
    {
        // Verificar si el personaje está atacando
        isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        // Verificar si el personaje está bloqueando (dodge o jump)
        isBlocking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge") ||
                     animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump");


        if (AzriController.instance.dead)
        {
            Win();
        }


    }

    public void Die()
    {
        dead = true;
        animator.SetTrigger("Die");
    }

    public void Win()
    {
        if (!winSound)
        {
            audioSource.clip = victory;
            audioSource.Play();
            winSound = true;
        }
        
        animator.SetTrigger("Win");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ColisionAzri") && AzriController.instance.isAttacking)
        {
            audioSource.PlayOneShot(kick);
            Die();
        }
    }
}
