using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AzriController : MonoBehaviour
{
    public Animator animator;

    private Transform azriT;

    private Vector2 movementInput; // Almacena los valores de movimiento

    void Start()
    {
        // Referencia al transform del modelo
        azriT = transform.GetChild(0);
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
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        if (ctx.performed || ctx.canceled)
        {
            // Leer y almacenar el valor de entrada
            movementInput = ctx.ReadValue<Vector2>();
        }
    }

    public void OnDodge(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (movementInput.y < -0.5f) // Esquivar ataque alto
            {
                animator.SetTrigger("DodgingHigh");
            }
            else if (movementInput.y > 0.5f) // Esquivar ataque bajo (salto)
            {
                animator.SetTrigger("Jumping");
            }
        }
    }

    public void OnAttackQuick(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
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
        if (ctx.performed)
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
        animator.SetTrigger("Die");
    }

    public void Win()
    {
        animator.SetTrigger("Win");
    }
}
