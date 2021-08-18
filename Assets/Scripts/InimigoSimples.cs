using UnityEngine;

public class InimigoSimples : BaseInimigo
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

    private Rigidbody2D _rigidbody2D;

    protected void Start()
    {
        // Resgatando o rigidbody
        this._rigidbody2D = GetComponent<Rigidbody2D>();

        // Definindo velocidade
        this._rigidbody2D.velocity = new Vector2(0, this.velocidade);

        // Resgatando SpriteRenderer do sprite
        this._spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    protected void Update()
    {
        this.Atirar();
    }

    private void Atirar()
    {
        if (!this._spriteRenderer.isVisible)
        {
            return;
        }
        
        // Diminuindo tempo pro proximo tiro
        this._proximoTiro -= Time.deltaTime;

        // Se proximo tiro liberado e estiver visivel na tela
        if (this._proximoTiro < 0)
        {
            // Cria o tiro
            Instantiate(this.shoot, this.posicaoTiro.position, Quaternion.identity);

            // Define tempo do próximo tiro
            this._proximoTiro = Random.Range(this.tempoMinimoTiro, this.tempoMaximoTiro);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destruindo se bater com o colisor
        if (other.CompareTag(Tags.Colisor))
        {
            Destroy(this.gameObject);
        }
    }
}