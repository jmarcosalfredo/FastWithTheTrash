using UnityEngine;

public class CarController : MonoBehaviour
{
    // Variáveis que serão configuradas pelo Spawner
    private float speed;
    private float trashSpawnChance;
    private int moveDirection = 1;

    // Referências de componentes
    private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform carVisualTransform;
    [SerializeField] private bool visualFacesRightAtY0 = false;
    public GameObject[] trashPrefabs;
    public Transform trashSpawnPoint;
    private BoxCollider2D boxCollider;

    void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();

        if (carVisualTransform == null && spriteRenderer != null)
        {
            carVisualTransform = spriteRenderer.transform;
        }
    }

    public void Initialize(CarData data, bool moveToLeft = false)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = data.carSprite;
        }

        if (boxCollider != null)
        {
            boxCollider.size = data.boxColliderSize;
        }

        this.speed = Random.Range(data.minSpeed, data.maxSpeed);
        this.moveDirection = moveToLeft ? -1 : 1;
        UpdateVisualDirection();

        this.trashSpawnChance = data.trashSpawnChance;
    }

    void Update()
    {
        TrySpawnTrash();
        transform.Translate(Vector3.right * speed * moveDirection * Time.deltaTime);

        if (transform.position.x > 26f || transform.position.x < -26f)
        {
            Destroy(gameObject);
        }
    }

    // Cobre o caso em que o BoxCollider2D do carro é sólido
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>() != null)
        {
            GameManager.instance?.GameOver("Você foi atropelado!");
        }
    }
    

    // Cobre o caso em que o BoxCollider2D do carro é Trigger
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null)
        {
            GameManager.instance?.GameOver("Você foi atropelado!");
        }
    }

    private void TrySpawnTrash()
    {
        float spawnProbabilityThisFrame = Mathf.Clamp01(this.trashSpawnChance * Time.deltaTime);
        if (trashPrefabs.Length > 0 && Random.value < spawnProbabilityThisFrame)
        {
            int randomIndex = Random.Range(0, trashPrefabs.Length);

            float randomX = Random.Range(-2f, 2f);
            float randomY = Random.Range(-2f, 2f);
            Vector3 spawnPosition = trashSpawnPoint.position + new Vector3(randomX, randomY, 0f);

            Instantiate(trashPrefabs[randomIndex], spawnPosition, Quaternion.identity);
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