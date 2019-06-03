using UnityEngine;

namespace Done
{
    public class Population : MonoBehaviour
    {
        [Header("UI")]
        public StatusWindow statusWindow;

        [Header("Entity prefab")]
        public GameObject entityPrefab;

        [Header("Population")]
        public int populationSize = 8;
        public float generationTime = 10f;

        [Header("Map points")]
        public Transform startPoint;
        public Transform targetPoint;

        private Entity[] population;

        private float time = 0;

        private int currentGeneration;

        // ========================================================
        void Start()
        {
            this.population = new Entity[this.populationSize];

            for (int i = 0; i < this.populationSize; i++)
            {
                this.population[i] = this.CreateEntity();
            }
            this.currentGeneration = 1;
            this.statusWindow.SetGenerationNumber(this.currentGeneration);
        }

        // ========================================================
        void Update()
        {
            this.time += Time.deltaTime;

            if (this.time > this.generationTime)
            {
                this.currentGeneration++;
                this.statusWindow.SetGenerationNumber(this.currentGeneration);
                this.time -= this.generationTime;

                // Generate new population
                Entity[] newPopulation = new Entity[this.populationSize];
                Entity[] parents = new Entity[2];

                for(int i = 0; i < this.populationSize; i++)
                {
                    Entity newborn = this.CreateEntity();
                    int parentCount = 0;

                    int safety = 0;
                    while (parentCount < 2 && safety < 10000)
                    {
                        safety++;
                        int index = Random.Range(0, this.populationSize);
                        Entity candidate = this.population[index];
                        float dice = Random.Range(0f, 1f);

                        // Calculate fitness
                        if (this.CalculateEntityFitness(candidate) > dice)
                        {
                            parents[parentCount] = candidate;
                            parentCount++;
                        }
                    }

                    if (parents.Length == 2)
                    {
                        // Crossover
                        Vector3[] newGenes = this.Crossover(parents[0].genes, parents[1].genes);
                        // Mutate
                        newGenes = this.Mutate(newGenes, 0.05f);

                        newborn.genes = newGenes;
                    }
                    else
                    {
                        Debug.LogError("No parents found. The Entity will be random.");
                    }

                    newPopulation[i] = newborn;
                }

                // Clear old population
                foreach (Entity e in this.population)
                {
                    Destroy(e.gameObject);
                }

                // Hail to the new population!
                this.population = newPopulation;
            }
        }

        // ========================================================
        Entity CreateEntity()
        {
            GameObject e = Instantiate(this.entityPrefab, this.startPoint.position, Quaternion.identity);

            e.transform.SetParent(this.transform);

            return e.GetComponent<Entity>();
        }

        // ========================================================
        float CalculateEntityFitness(Entity e)
        {
            float maxDistance = Vector3.Distance(this.startPoint.position, this.targetPoint.position);
            float entityDistance = Vector3.Distance(e.transform.position, this.targetPoint.position);

            float percent = Mathf.Clamp01(entityDistance / maxDistance);

            return Mathf.Pow(1 - percent, 2);
        }

        // ========================================================
        Vector3[] Mutate(Vector3[] e, float chance)
        {
            for (int i = 0; i < e.Length; i++)
            {
                float rnd = Random.Range(0f, 1f);
                if (rnd < chance)
                {
                    e[i].Set(
                        Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f)
                    );
                    e[i].Normalize();
                }
            }

            return e;
        }

        // ========================================================
        Vector3[] Crossover(Vector3[] a, Vector3[] b)
        {
            Vector3[] offspring = new Vector3[a.Length];

            for (int i = 0; i < a.Length; i++)
            {
                if (i % 2 == 0)
                {
                    offspring[i] = a[i];
                }
                else
                {
                    offspring[i] = b[i];
                }
            }

            return offspring;
        }
    }
}
