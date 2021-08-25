using Interfaces;
using UnityEngine;

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
    [SerializeField] private int valePontos = 1;
    
    // Prefab powerup
    [SerializeField] private GameObject powerUpPrefab;
    
    // Redenrizador da sprite
    protected SpriteRenderer SpriteRenderer;

    // Controlador do jogo
    protected GameController GameController;
    
    protected virtual void Start()
    {
        this.GameController = FindObjectOfType<GameController>();
        
        // Resgatando SpriteRenderer do sprite
        this.SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    protected bool IsVisible()
    {
        return this.SpriteRenderer.isVisible;
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
            this.GameController.GanharPontos(this.valePontos);
            this.GameController.DiminuirInimigosVivos();
            
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
}