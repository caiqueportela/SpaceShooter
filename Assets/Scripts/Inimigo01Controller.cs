using UnityEngine;

public class Inimigo01Controller : InimigoSimples
{
    // Prefab tiro
    [SerializeField] private GameObject shoot;

    // Posicao de onde o tiro será criado
    [SerializeField] private Transform posicaoTiro;

    // Tempo minimo entre cada tiro
    [SerializeField] private float tempoMinimoTiro = 1f;

    // Tempo máximo entre cada tiro
    [SerializeField] private float tempoMaximoTiro = 3f;

    // Tempo pro próximo tiro
    private float _proximoTiro;

    private SpriteRenderer _spriteRenderer;

    private new void Start()
    {
        base.Start();

        this._spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        Atirar();
    }

    private void Atirar()
    {
        this._proximoTiro -= Time.deltaTime;

        if (this._proximoTiro < 0 && this._spriteRenderer.isVisible)
        {
            Instantiate(this.shoot, this.posicaoTiro.position, Quaternion.identity);

            this._proximoTiro = Random.Range(this.tempoMinimoTiro, this.tempoMaximoTiro);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destruindo se bater com o colisor
        if (other.CompareTag("Colisor"))
        {
            Destroy(this.gameObject);
        }
    }
}