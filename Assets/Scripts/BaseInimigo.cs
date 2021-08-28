using System;
using Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class BaseInimigo : MonoBehaviour, ITomaDano
{
    // Velocidade de movimento
    [SerializeField] protected float velocidade = -3f;

    // Vida
    [SerializeField] protected int vida = 1;

    // Objeto de explosão ao morrer
    [SerializeField] private GameObject explosao;

    // Velocidade do tiro
    [SerializeField] protected float velocidadeTiro = -10f;

    // Pontos que ele da quando morre
    [SerializeField] protected int valePontos = 1;

    // Prefab powerup
    [SerializeField] private GameObject powerUpPrefab;

    // Redenrizador da sprite
    private SpriteRenderer _spriteRenderer;

    // Controlador do jogo
    protected GameController GameController;

    // Tempo pro próximo tiro
    protected float ProximoTiro;

    protected Rigidbody2D Rigidbody2D;
    
    [SerializeField] protected AudioClip tiroSom;

    protected virtual void Start()
    {
        this.GameController = FindObjectOfType<GameController>();

        // Resgatando SpriteRenderer do sprite
        this._spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // Resgatando o rigidbody
        this.Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    protected virtual void Update()
    {
        // Se estiver visivel na tela
        if (IsVisible())
        {
            // Diminuindo tempo pro proximo tiro
            this.ProximoTiro -= Time.deltaTime;
        }
    }

    protected bool IsVisible()
    {
        return this._spriteRenderer.isVisible;
    }

    public virtual void TomarDano(int dano)
    {
        this.vida -= dano;

        this.Morrer();
    }

    protected virtual void Morrer()
    {
        if (this.vida <= 0)
        {
            this.GameController.GanharPontos(this.valePontos);

            Destroy(this.gameObject);

            Instantiate(this.explosao, this.transform.position, Quaternion.identity);

            CriarPowerUp();
        }
    }

    private void CriarPowerUp()
    {
        float chance = Random.Range(0f, 1f);

        if (chance <= .9f)
        {
            return;
        }

        var powerUp = Instantiate(this.powerUpPrefab, this.transform.position, Quaternion.identity);

        Destroy(powerUp.gameObject, 3f);

        var rb = powerUp.GetComponent<Rigidbody2D>();

        var direcao = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        rb.velocity = direcao;
    }

    protected float GetVelocidade(bool normalized = false)
    {
        if (normalized)
        {
            return Mathf.Abs(this.velocidade);
        }

        return this.velocidade;
    }

    protected float GetVelocidadeTiro(bool normalized = false)
    {
        if (normalized)
        {
            return Mathf.Abs(this.velocidadeTiro);
        }

        return this.velocidadeTiro;
    }

    public bool PodeTomarDano()
    {
        return this.IsVisible();
    }

    protected virtual void DirecionarTiro(GameObject tiro)
    {
        tiro.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, this.velocidadeTiro);
    }

    protected bool PodeAtirar()
    {
        return this.ProximoTiro <= 0;
    }

    private void OnDestroy()
    {
        if (!this.GameController)
        {
            return;
        }

        this.GameController.DiminuirInimigosVivos();
    }
}