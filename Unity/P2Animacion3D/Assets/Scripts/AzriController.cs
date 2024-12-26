using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AzriController : MonoBehaviour
{
    public Animator animator;

    private Transform azriT;

    void Start()
    {
        // Referencia al componente Animator
        // animator = GetComponent<Animator>();
        azriT = transform.GetChild(0);
    }

    void Update()
    {
        // Resetear las transiciones a un estado Idle si no se detectan inputs
        animator.SetBool("IsWalkingForward", false);
        animator.SetBool("IsWalkingBackwards", false);

        // Leer inputs del mando
        float horizontal = Input.GetAxis("Horizontal"); // Joystick izquierdo (X axis)
        float vertical = Input.GetAxis("Vertical"); // Joystick izquierdo (Y axis)
        bool blockButton = Input.GetButton("Fire3"); // Botón de bloqueo (generalmente botón "B" o "Circle")
        bool attackQuick = Input.GetButtonDown("Fire1"); // Botón de ataque rápido (generalmente botón "X" o "Square")
        bool attackSlow = Input.GetButtonDown("Fire2"); // Botón de ataque lento (generalmente botón "A" o "Triangle")
        bool jumpButton = Input.GetButtonDown("Jump"); // Botón de salto (generalmente "Y" o "Triangle")

        // Transiciones
        if (-horizontal > 0) // Caminar hacia adelante
        {
            animator.SetBool("IsWalkingForward", true);
        }
        else if (-horizontal < 0) // Caminar hacia atrás
        {
            animator.SetBool("IsWalkingBackwards", true);
        }
        else if (vertical < -0.5f && blockButton) // Esquivar ataque alto
        {
            animator.SetTrigger("DodgingHigh");
        }
        else if (vertical > 0.5f && blockButton) // Esquivar ataque bajo (salto en el lugar)
        {
            animator.SetTrigger("Jumping");
        }
        else if (vertical < -0.5f && attackQuick) // Ataque bajo rápido
        {
            animator.SetTrigger("LowQuickAttack");
        }
        else if (vertical < -0.5f && attackSlow) // Ataque bajo lento
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



        //Ajustes movimiento
        azriT.position = new Vector3(0, azriT.position.y, azriT.position.z);

        if(azriT.transform.position.y < -1)
        {
            azriT.position = new Vector3(azriT.position.x, -0.9f, azriT.position.z);
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
