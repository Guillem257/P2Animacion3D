using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AzriController : MonoBehaviour
{
    public Animator animator;

    private Transform azriT;

    public static AzriController instance;

    private Vector2 movementInput; // Almacena los valores de movimiento

    public bool isAttacking = false;
    public bool isBlocking = false;
    public bool dead = false;

    void Start()
    {
        // Referencia al transform del modelo
        azriT = transform.GetChild(0);

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

        // Verificar el movimiento en función del input
        if (movementInput.x > 0.5f) // Caminar hacia adelante
        {
            animator.SetBool("IsWalkingBackwards", true);
        }
        else if (movementInput.x < -0.5f) // Caminar hacia atrás
        {
            animator.SetBool("IsWalkingForward", true);
            
        }

        // Ajustar posición de movimiento en el eje X
        azriT.position = new Vector3(0, azriT.position.y, azriT.position.z);


        UpdateStates();

    }


    private void UpdateStates()
    {
        // Verificar si el personaje está atacando
        isAttacking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack");

        // Verificar si el personaje está bloqueando (dodge o jump)
        isBlocking = animator.GetCurrentAnimatorStateInfo(0).IsTag("Dodge") ||
                     animator.GetCurrentAnimatorStateInfo(0).IsTag("Jump");


        if (ScoutController.instance.dead)
        {
            Win();
        }


    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed || ctx.canceled)
        {
            // Leer y almacenar el valor de entrada
            movementInput = ctx.ReadValue<Vector2>();
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isBlocking)
        {
            animator.SetTrigger("Jumping");
        }
    }

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isBlocking)
        {
            if (movementInput.y < -0.5f) // Esquivar ataque alto
            {
                animator.SetTrigger("DodgingHigh");
            }
            else if (movementInput.y > 0.5f) // Esquivar ataque bajo (salto)
            {
                animator.SetTrigger("Jumping");
            }
            else
            {
                animator.SetTrigger("DodgingHigh");
            }
        }
    }

    public void OnAttackQuick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isAttacking)
        {
            if (movementInput.y < -0.5f) // Ataque bajo rápido
            {
                animator.SetTrigger("LowQuickAttack");
            }
            else // Ataque rápido normal
            {
                animator.SetTrigger("QuickAttack");
            }
        }
    }

    public void OnAttackSlow(InputAction.CallbackContext ctx)
    {
        if (ctx.performed && !isAttacking)
        {
            if (movementInput.y < -0.5f) // Ataque bajo lento
            {
                animator.SetTrigger("LowSlowAttack");
            }
            else // Ataque lento normal
            {
                animator.SetTrigger("SlowAttack");
            }
        }
    }

    public void Die()
    {
        dead = true;
        animator.SetTrigger("Die");
    }

    public void Win()
    {
        animator.SetTrigger("Win");
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("alhooo");

        if(other.CompareTag("ColisionScout") && ScoutController.instance.isAttacking)
        {
            Die();
        }
    }
}
