using UnityEngine;

namespace Done
{
    [RequireComponent(typeof(Rigidbody))]
    public class Entity : MonoBehaviour
    {
        [Header("DNA")]
        public int geneCount = 1;
        public Vector3[] genes { get; set; }
        [Header("Movement")]
        public float forcePower = 1;
        public float beatTime = 0.5f;

        private int geneIndex;

        private float time = 0;

        private Rigidbody body;

        // ========================================================
        void Start()
        {
            if (this.genes == null)
            {
                Vector3[] startGenes = new Vector3[this.geneCount];

                for (int i = 0; i < this.geneCount; i++)
                {
                    startGenes[i] = (new Vector3(
                        Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f),
                        Random.Range(-1f, 1f)
                    )).normalized;
                }

                this.genes = startGenes;
            }

            this.body = GetComponent<Rigidbody>();
            this.geneIndex = 0;
            this.body.velocity = Vector3.zero;
        }

        // ========================================================
        private void FixedUpdate()
        {
            Vector3 forceDirection = this.genes[this.geneIndex];
            this.body.AddForce(forceDirection * this.forcePower);
        }

        // ========================================================
        void Update()
        {
            this.time += Time.deltaTime;
            if (this.time > this.beatTime)
            {
                this.time -= this.beatTime;
                if (this.geneIndex < (this.geneCount-1))
                {
                    this.geneIndex++;
                }
            }
        }
    }
}
