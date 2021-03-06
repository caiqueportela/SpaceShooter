using Interfaces;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, ITomaDano
{
    // Quantidade maxima de escudos que o player pode utilizar
    private const int MAX_ESCUDOS = 3;

    // Velocidade de movimento
    [SerializeField] private float velocidade = 5f;

    // Prefab tiro
    [SerializeField] private GameObject[] shoots;

    // Posicao de onde o tiro será criado
    [SerializeField] private Transform posicaoTiro;

    // Velocidade do tiro
    [SerializeField] private float velocidadeTiro = 10f;

    // Tempo entre cada tiro
    [SerializeField] private float tempoTiro = 1f;

    // Vida
    [SerializeField] private int vida = 3;

    // Objeto de explosão ao morrer
    [SerializeField] private GameObject explosao;

    [SerializeField] private float limitesX;
    [SerializeField] private float limitesY;

    [SerializeField] private int levelTiro = 1;

    // Prefab do escudo
    [SerializeField] private GameObject escudoPrefab;

    // Tempo entre poder criar um novo escudo
    [SerializeField] private float intervaloEscudo = 10f;

    // Tempo que dura o escudo
    [SerializeField] private float tempoEscudo = 5f;

    // Quantidade de escudos que o player já utilizou
    private int _escudosUtilizados;

    // Tempo até o proximo escudo
    private float _proximoEscudo;

    // Tempo até o termino do escudo
    private float _terminoEscudo;

    // Instancia do escudo criada
    private GameObject _escudo;

    private int _maximoLevelTiro = 3;

    // Tempo pro próximo tiro
    private float _proximoTiro;

    private Rigidbody2D _rigidbody2D;

    [SerializeField] private Text txtVida;
    [SerializeField] private Text txtEscudo;
    [SerializeField] private Text txtEscudoTempo;

    [SerializeField] private AudioClip tiroSom;
    [SerializeField] private AudioClip morteSom;
    [SerializeField] private AudioClip escudoSom;
    [SerializeField] private AudioClip escudoSaindoSom;

    void Start()
    {
        this._rigidbody2D = GetComponent<Rigidbody2D>();

        this.AtualizarVidaUI();
        this.AtualizarEscudoUI();
    }

    private void AtualizarVidaUI()
    {
        this.txtVida.text = $"{this.vida}";
    }

    private void AtualizarEscudoUI()
    {
        this.txtEscudo.text = $"{(MAX_ESCUDOS - this._escudosUtilizados)}";
        var tempo = this._proximoEscudo > 0 ? this._proximoEscudo : 0;
        this.txtEscudoTempo.text = $"{tempo.ToString("0")} s";
    }

    void Update()
    {
        this.Movimentar();

        this.Atirar();

        this.Escudo();
    }

    private void Escudo()
    {
        this.AtualizarEscudoUI();

        this._proximoEscudo -= Time.deltaTime;

        if (this._escudo)
        {
            this._terminoEscudo -= Time.deltaTime;
            this._escudo.transform.position = this.transform.position;

            if (this._terminoEscudo <= 0)
            {
                AudioSource.PlayClipAtPoint(this.escudoSaindoSom, Vector3.zero);
                Destroy(this._escudo.gameObject);
                this._escudo = null;
            }

            return;
        }

        if (this._proximoEscudo <= 0 && Input.GetButtonDown("Shield") && this._escudosUtilizados < MAX_ESCUDOS)
        {
            this._escudo = Instantiate(this.escudoPrefab, this.transform.position, Quaternion.identity);
            this._escudosUtilizados++;
            
            AudioSource.PlayClipAtPoint(this.escudoSom, Vector3.zero);

            this._terminoEscudo = this.tempoEscudo;
            this._proximoEscudo = this.intervaloEscudo;
        }
    }

    private void Atirar()
    {
        this._proximoTiro -= Time.deltaTime;

        if (this._proximoTiro <= 0 && Input.GetButton("Fire1"))
        {
            switch (this.levelTiro)
            {
                case 2:
                    this.CriarTiro(this.shoots[1], 0.3f, -0.3f);
                    this.CriarTiro(this.shoots[1], -0.3f, -0.3f);
                    break;
                case 3:
                    this.CriarTiro(this.shoots[0]);
                    this.CriarTiro(this.shoots[1], 0.3f, -0.3f);
                    this.CriarTiro(this.shoots[1], -0.3f, -0.3f);
                    break;
                default:
                    this.CriarTiro(this.shoots[0]);
                    break;
            }

            this._proximoTiro = this.tempoTiro;
        }
    }

    private void CriarTiro(GameObject objetoTiro, float x = 0, float y = 0)
    {
        var posicao = this.posicaoTiro.position;

        posicao.x += x;
        posicao.y += y;

        var tiro = Instantiate(objetoTiro, posicao, Quaternion.identity);

        tiro.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, this.velocidadeTiro);
        
        AudioSource.PlayClipAtPoint(this.tiroSom, Vector3.zero);
    }

    private void Movimentar()
    {
        // Resgatando movimento horizontal
        var horizontal = Input.GetAxis("Horizontal");
        // Resgatando movimento vertial
        var vertical = Input.GetAxis("Vertical");

        var velocidadeMovimento = (new Vector2(horizontal, vertical)).normalized * this.velocidade;

        this._rigidbody2D.velocity = velocidadeMovimento;

        // Limitando saida da tela
        var x = Mathf.Clamp(this.transform.position.x, -this.limitesX, this.limitesX);
        var y = Mathf.Clamp(this.transform.position.y, -this.limitesY, this.limitesY);
        this.transform.position = new Vector3(x, y, this.transform.position.z);
    }

    public void TomarDano(int dano)
    {
        this.vida -= dano;

        this.AtualizarVidaUI();

        this.Morrer();
    }

    private void Morrer()
    {
        if (this.vida <= 0)
        {
            AudioSource.PlayClipAtPoint(this.morteSom, Vector3.zero);
            
            Destroy(this.gameObject);

            Instantiate(this.explosao, this.transform.position, Quaternion.identity);

            var gameManager = FindObjectOfType<GameManager>();
            if (gameManager)
            {
                gameManager.Reiniciar();
            }
        }
    }

    public bool PodeTomarDano()
    {
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.PowerUp))
        {
            if (this._maximoLevelTiro > this.levelTiro)
            {
                this.levelTiro++;
            }

            Destroy(other.gameObject);
        }
    }
}