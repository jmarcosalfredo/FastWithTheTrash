using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Configurações do Spawner")]
    public GameObject carPrefab; // O prefab "burro" do carro
    public float spawnRate = 1.0f; // Carros por segundo
    public bool spawnToLeft = false; // true = carros andam para a esquerda, false = para a direita
    private float nextSpawnTime = 0f;

    [Header("Tipos de Carro para Gerar")]
    public CarData[] carTypes; // Arraste todos os seus "arquivos de dados" de carro aqui!

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnCar();
            nextSpawnTime = Time.time + 1f / spawnRate;
        }
    }

    void SpawnCar()
    {
        if (carPrefab == null || carTypes.Length == 0)
        {
            Debug.LogError("Spawner não configurado! Verifique o prefab do carro e os tipos de carro.");
            return;
        }

        // 1. Escolhe um tipo de carro aleatoriamente da lista
        int randomIndex = Random.Range(0, carTypes.Length);
        CarData randomCarData = carTypes[randomIndex];

        // 2. Cria uma instância do prefab "burro"
        GameObject newCarObject = Instantiate(carPrefab, transform.position, transform.rotation);

        // 3. Pega o script CarController da nova instância
        CarController newCarController = newCarObject.GetComponent<CarController>();

        // 4. "Injeta" os dados do Scriptable Object no carro
        if (newCarController != null)
        {
            newCarController.Initialize(randomCarData, spawnToLeft);
        }
    }
}

