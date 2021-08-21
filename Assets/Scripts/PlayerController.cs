using Interfaces;
using UnityEngine;

public class PlayerController : MonoBehaviour, ITomaDano
{
    // Velocidade de movimento
    [SerializeField] private float velocidade = 5f;

    // Prefab tiro
    [SerializeField] private GameObject shoot;

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

    // Tempo pro próximo tiro
    private float _proximoTiro;

    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        this._rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        this.Movimentar();

        this.Atirar();
    }

    private void Atirar()
    {
        this._proximoTiro -= Time.deltaTime;

        if (this._proximoTiro <= 0 && Input.GetButton("Fire1"))
        {
            var tiro = Instantiate(this.shoot, this.posicaoTiro.position, Quaternion.identity);

            tiro.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, this.velocidadeTiro);

            this._proximoTiro = this.tempoTiro;
        }
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

        this.Morrer();
    }

    private void Morrer()
    {
        if (this.vida <= 0)
        {
            Destroy(this.gameObject);

            Instantiate(this.explosao, this.transform.position, Quaternion.identity);
        }
    }

    public bool PodeTomarDano()
    {
        return true;
    }
}