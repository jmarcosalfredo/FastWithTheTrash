using UnityEngine;

public class CarController : MonoBehaviour
{
    // Variáveis que serão configuradas pelo Spawner
    private float speed;
    private float trashSpawnChance;
    private int moveDirection = 1;

    // Referências de componentes
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform carVisualTransform; // Transform do objeto visual/animator do carro
    [SerializeField] private bool visualFacesRightAtY0 = false; // true se o sprite já olha para a direita com Y = 0
    public GameObject[] trashPrefabs; // Array para os 4 tipos de prefabs de lixo
    public Transform trashSpawnPoint; // Ponto de onde o lixo é "jogado"
    private BoxCollider2D boxCollider;

    void Awake()
    {
        // Pega o componente no início para não precisar fazer isso toda hora
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(); // Usando InChildren caso o sprite esteja em um objeto filho
        boxCollider = GetComponent<BoxCollider2D>();

        if (carVisualTransform == null && spriteRenderer != null)
        {
            carVisualTransform = spriteRenderer.transform;
        }
    }

    // Esta é a função mais importante! O Spawner vai chamá-la para configurar o carro.
    public void Initialize(CarData data, bool moveToLeft = false)
    {
        // Configura o visual
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = data.carSprite;
        }

        if (boxCollider != null)
        {
            boxCollider.size = data.boxColliderSize;
        }

        // Configura a velocidade (um valor aleatório entre o mínimo e o máximo)
        this.speed = Random.Range(data.minSpeed, data.maxSpeed);
        this.moveDirection = moveToLeft ? -1 : 1;
        UpdateVisualDirection();

        // Configura a chance de soltar lixo
        this.trashSpawnChance = data.trashSpawnChance;
    }

    void Update()
    {
        TrySpawnTrash();
        transform.Translate(Vector3.right * speed * moveDirection * Time.deltaTime);

        // Destrói o carro quando ele sai da tela (ajuste os valores para o tamanho da sua tela)
        if (transform.position.x > 20f || transform.position.x < -20f)
        {
            Destroy(gameObject);
        }
    }

    private void TrySpawnTrash()
    {
        float spawnProbabilityThisFrame = Mathf.Clamp01(this.trashSpawnChance * Time.deltaTime);
        if (trashPrefabs.Length > 0 && Random.value < spawnProbabilityThisFrame)
        {
            int randomIndex = Random.Range(0, trashPrefabs.Length);
            Instantiate(trashPrefabs[randomIndex], trashSpawnPoint.position, Quaternion.identity);
        }
    }

    private void UpdateVisualDirection()
    {
        if (carVisualTransform == null) return;

        Vector3 currentRotation = carVisualTransform.localEulerAngles;
        bool movingRight = moveDirection > 0;
        if (visualFacesRightAtY0)
        {
            currentRotation.y = movingRight ? 0f : 180f;
        }
        else
        {
            currentRotation.y = movingRight ? 180f : 0f;
        }
        carVisualTransform.localEulerAngles = currentRotation;
    }
}
