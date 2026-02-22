using UnityEngine;
using System.Collections.Generic; // Necessário para usar Listas (o "inventário")

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float moveSpeed = 5f;

    [Header("Referências de Componentes")]
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    [Header("Sistema de Inventário")]
    private List<TrashType> inventory = new List<TrashType>();

    // --- MÉTODOS DO UNITY ---

    void Awake()
    {
        // Pega os componentes essenciais no início do jogo
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        // 1. Captura o input do jogador a cada frame
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 2. Atualiza a animação com base no input
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        // 3. Movimento por física 2D para respeitar colisores
        rb.linearVelocity = movement.normalized * moveSpeed;
    }

    // --- LÓGICA DE ANIMAÇÃO ---

    private void UpdateAnimation()
    {
       if (animator == null) return;

        // Lógica de animação super simples para 4 direções
        // 0=Idle, 1=Baixo, 2=Cima, 3=Esquerda, 4=Direita
        if (movement.y > 0) animator.SetInteger("Direction", 2);       // Cima
        else if (movement.y < 0) animator.SetInteger("Direction", 1);  // Baixo
        else if (movement.x < 0) animator.SetInteger("Direction", 3);  // Esquerda
        else if (movement.x > 0) animator.SetInteger("Direction", 4);  // Direita
        else animator.SetInteger("Direction", 0);  
    }

    // --- LÓGICA DE INTERAÇÃO E COLISÃO ---

    // Chamado quando o jogador entra em um Collider 2D marcado como "Trigger"
}
