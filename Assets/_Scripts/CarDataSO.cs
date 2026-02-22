using UnityEngine;

// A anotação [CreateAssetMenu] é a mágica que nos permite criar "arquivos de dados" no editor do Unity.
[CreateAssetMenu(fileName = "New Car Data", menuName = "Game Data/Car Data")]
public class CarData : ScriptableObject
{
    [Header("Identificação")]
    public string carName; // Ex: "Fusca Azul", "Táxi Amarelo"

    [Header("Atributos Visuais")]
    public Sprite carSprite; // O visual do carro
    public Vector2 boxColliderSize = new Vector2(1.5f, 0.8f); // Tamanho padrão do BoxCollider2D

    [Header("Atributos de Comportamento")]
    public float minSpeed = 8f;
    public float maxSpeed = 12f; // Usar um range deixa o tráfego mais natural
    public bool IsHorizontal = true; // Para saber se o carro se move horizontalmente ou verticalmente

    [Header("Lógica do Lixo")]
    [Range(0, 1)] // Cria um slider no Inspector, de 0 a 100%
    public float trashSpawnChance = 0.1f; // 10% de chance de soltar lixo
}
