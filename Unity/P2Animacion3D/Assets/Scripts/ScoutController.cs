using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoutController : MonoBehaviour
{
    public Animator animator;

    private Transform scoutT;

    void Start()
    {
        // Referencia al componente Animator
        // animator = GetComponent<Animator>();
        scoutT = transform.GetChild(0);
    }

    void Update()
    {
        // Resetear las transiciones a un estado Idle si no se detectan inputs
        animator.SetBool("IsWalkingForward", false);
        animator.SetBool("IsWalkingBackwards", false);

        // Leer inputs del mando
        float horizontal = Input.GetAxis("Horizontal"); // Joystick izquierdo (X axis)
        float vertical = Input.GetAxis("Vertical"); // Joystick izquierdo (Y axis)
        bool blockButton = Input.GetButton("Fire3"); // Bot�n de bloqueo (generalmente bot�n "B" o "Circle")
        bool attackQuick = Input.GetButtonDown("Fire1"); // Bot�n de ataque r�pido (generalmente bot�n "X" o "Square")
        bool attackSlow = Input.GetButtonDown("Fire2"); // Bot�n de ataque lento (generalmente bot�n "A" o "Triangle")
        bool jumpButton = Input.GetButtonDown("Jump"); // Bot�n de salto (generalmente "Y" o "Triangle")

        // Transiciones
        if (-horizontal > 0) // Caminar hacia adelante
        {
            animator.SetBool("IsWalkingForward", true);
        }
        else if (-horizontal < 0) // Caminar hacia atr�s
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
        else if (vertical < -0.5f && attackQuick) // Ataque bajo r�pido
        {
            animator.SetTrigger("LowQuickAttack");
        }
        else if (vertical < -0.5f && attackSlow) // Ataque bajo lento
        {
            animator.SetTrigger("LowSlowAttack");
        }
        else if (attackQuick) // Ataque r�pido
        {
            animator.SetTrigger("QuickAttack");
        }
        else if (attackSlow) // Ataque lento
        {
            animator.SetTrigger("SlowAttack");
        }


        Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);

        //Ajustes movimiento
        scoutT.position = new Vector3(0, scoutT.position.y, scoutT.position.z);


        //if (animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Idle")
        //{
        //    Debug.Log("Esta idlee");
        //    if(azriT.position.y > 0.02 || azriT.position.y < -0.02)
        //    {
        //        azriT.position = new Vector3(azriT.position.x, 0, azriT.position.z);
        //    }
        //}
        //else
        //{
        //    //Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
        //}



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
