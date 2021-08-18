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
        // Se estiver visivel na tela
        if (IsVisible())
        {
            // Diminuindo tempo pro proximo tiro
            this._proximoTiro -= Time.deltaTime;

            this.Atirar();
        }
    }

    private void Atirar()
    {
        // Se proximo tiro não liberado
        if (this._proximoTiro >= 0)
        {
            return;
        }

        // Cria o tiro
        var tiro = Instantiate(this.shoot, this.posicaoTiro.position, Quaternion.identity);

        DirecionarTiro(tiro);

        // Define tempo do próximo tiro
        this._proximoTiro = Random.Range(this.tempoMinimoTiro, this.tempoMaximoTiro);
    }

    protected virtual void DirecionarTiro(GameObject tiro)
    {
        tiro.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, this.velocidadeTiro);
    }

    protected bool IsVisible()
    {
        return this._spriteRenderer.isVisible;
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