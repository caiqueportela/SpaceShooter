using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class InimigoSimples : BaseInimigo
{
    // Prefab tiro
    [SerializeField] private GameObject shoot;

    // Posicao de onde o tiro será criado
    [SerializeField] private Transform posicaoTiro;
    
    // Tempo minimo entre cada tiro
    [SerializeField] protected float tempoMinimoTiro = 1f;

    // Tempo máximo entre cada tiro
    [SerializeField] protected float tempoMaximoTiro = 3f;

    protected override void Start()
    {
        base.Start();

        // Definindo velocidade
        this.Rigidbody2D.velocity = new Vector2(0, this.velocidade);
    }

    protected override void Update()
    {
        base.Update();

        // Se estiver visivel na tela
        if (IsVisible())
        {
            this.Atirar();
        }
    }

    private void Atirar()
    {
        // Se proximo tiro não liberado
        if (!this.PodeAtirar())
        {
            return;
        }

        // Cria o tiro
        var tiro = Instantiate(this.shoot, this.posicaoTiro.position, Quaternion.identity);
        
        AudioSource.PlayClipAtPoint(this.tiroSom, Vector3.zero);

        this.DirecionarTiro(tiro);

        // Define tempo do próximo tiro
        this.ProximoTiro = Random.Range(this.tempoMinimoTiro, this.tempoMaximoTiro);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destruindo se bater com o colisor
        if (other.CompareTag(Tags.Colisor))
        {
            Destroy(this.gameObject);
        }

        // Destruindo se bater no escudo
        if (other.CompareTag(Tags.Escudo))
        {
            // Tomando toda vida de dano
            this.TomarDano(this.vida);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Se colidiu com o player
        if (other.gameObject.CompareTag(Tags.Player))
        {
            // Tomando toda vida de dano
            this.TomarDano(this.vida);

            // Dando dano no player
            other.gameObject.GetComponent<PlayerController>().TomarDano(1);
        }

        // Se colidiu com o escudo
        if (other.gameObject.CompareTag(Tags.Escudo))
        {
            // Tomando toda vida de dano
            this.TomarDano(this.vida);
        }
    }
}