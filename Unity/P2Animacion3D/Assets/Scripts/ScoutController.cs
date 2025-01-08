using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutController : MonoBehaviour
{
    public Animator animator;

    private Transform scoutT;
    public float velocidadMovimiento = 1;

    void Start()
    {
        // Referencia al transform del modelo
        scoutT = transform.GetChild(0);
    }

    void Update()
    {
        // Resetear las transiciones a un estado Idle si no se detectan inputs
        animator.SetBool("IsWalkingForward", false);
        animator.SetBool("IsWalkingBackwards", false);

        // Leer inputs del teclado
        bool moveForward = Input.GetKey(KeyCode.A); // Caminar hacia adelante
        bool moveBackward = Input.GetKey(KeyCode.D); // Caminar hacia atrás
        bool moveUp = Input.GetKey(KeyCode.W); // Moverse a la izquierda
        bool moveDown = Input.GetKey(KeyCode.S); // Moverse a la derecha
        bool blockButton = Input.GetKey(KeyCode.L); // Bloquear
        bool attackQuick = Input.GetKeyDown(KeyCode.J); // Ataque rápido
        bool attackSlow = Input.GetKeyDown(KeyCode.K); // Ataque lento

        // Transiciones de animación y movimiento
        if (moveForward) // Caminar hacia adelante
        {
            animator.SetBool("IsWalkingForward", true);
            transform.Translate(0, 0, velocidadMovimiento * Time.deltaTime);
        }
        else if (moveBackward) // Caminar hacia atrás
        {
            animator.SetBool("IsWalkingBackwards", true);
            transform.Translate(0, 0, -velocidadMovimiento * Time.deltaTime);
        }
        else if (blockButton && moveDown) // Esquivar ataque alto
        {
            animator.SetTrigger("DodgingHigh");
        }
        else if (blockButton && moveUp) // Esquivar ataque bajo (salto en el lugar)
        {
            animator.SetTrigger("Jumping");
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
