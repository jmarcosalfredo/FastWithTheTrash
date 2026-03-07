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
    public int maxInventorySize = 10;
    private List<TrashType> inventory = new List<TrashType>();

    [Header("Entrega")]
    public int pointsPerCorrectDelivery = 10;
    private TrashCan[] trashCans;

    // --- MÉTODOS DO UNITY ---

    void Awake()
    {
        // Pega os componentes essenciais no início do jogo
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();

        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Start()
    {
        // Cacheia as lixeiras da cena para checar alcance no apertar de tecla
        trashCans = FindObjectsByType<TrashCan>(FindObjectsSortMode.None);

        if (GameManager.instance != null)
        {
            GameManager.instance.UpdateInventoryUI(inventory.Count, maxInventorySize);
        }
    }

    void Update()
    {
        // 1. Captura o input do jogador a cada frame
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 2. Atualiza a animação com base no input
        UpdateAnimation();

        // Entrega apenas quando o jogador pressiona espaco
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryDeliverTrash();
        }
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

    // Chamado quando o jogador entra em um Collider 2D marcado como Trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Coleta de lixo
        TrashItem trash = collision.GetComponent<TrashItem>();
        if (trash != null)
        {
            if (inventory.Count < maxInventorySize)
            {
                inventory.Add(trash.trashType);
                Destroy(collision.gameObject);

                if (GameManager.instance != null)
                {
                    GameManager.instance.UpdateInventoryUI(inventory.Count, maxInventorySize);
                }

                Debug.Log("Lixo coletado: " + trash.trashType + " (" + inventory.Count + "/" + maxInventorySize + ")");
            }
            else
            {
                Debug.Log("Inventario cheio! Esvazie na lixeira primeiro.");
            }

            return;
        }

    }

    private void TryDeliverTrash()
    {
        if (inventory.Count == 0)
        {
            Debug.Log("Inventario vazio.");
            return;
        }

        TrashCan canInRange = GetNearestTrashCanInRange();
        if (canInRange == null)
        {
            Debug.Log("Nenhuma lixeira em alcance.");
            return;
        }

        int deliveredCount = 0;

        // Remove apenas os lixos aceitos pela lixeira encontrada em alcance
        for (int i = inventory.Count - 1; i >= 0; i--)
        {
            if (inventory[i] == canInRange.acceptedType)
            {
                inventory.RemoveAt(i);
                deliveredCount++;
            }
        }

        if (deliveredCount > 0)
        {
            int earnedPoints = deliveredCount * pointsPerCorrectDelivery;
            if (GameManager.instance != null)
            {
                GameManager.instance.AddScore(earnedPoints);
            }

            Debug.Log("Entregou " + deliveredCount + " item(ns) de " + canInRange.acceptedType + ". +" + earnedPoints + " pontos.");
        }
        else
        {
            Debug.Log("Nenhum item compativel com esta lixeira (" + canInRange.acceptedType + ").");
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.UpdateInventoryUI(inventory.Count, maxInventorySize);
        }
    }

    private TrashCan GetNearestTrashCanInRange()
    {
        if (trashCans == null || trashCans.Length == 0)
        {
            return null;
        }

        Vector2 playerPos = transform.position;
        TrashCan nearest = null;
        float nearestSqrDistance = float.MaxValue;

        foreach (TrashCan trashCan in trashCans)
        {
            if (trashCan == null) continue;
            if (!trashCan.IsPlayerInsideRange(playerPos)) continue;

            float sqrDistance = ((Vector2)trashCan.transform.position - playerPos).sqrMagnitude;
            if (sqrDistance < nearestSqrDistance)
            {
                nearestSqrDistance = sqrDistance;
                nearest = trashCan;
            }
        }

        return nearest;
    }
}
