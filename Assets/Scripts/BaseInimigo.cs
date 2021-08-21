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
    
    // Redenrizador da sprite
    protected SpriteRenderer SpriteRenderer;

    // Controlador do jogo
    private GameController _gameController;
    
    protected virtual void Start()
    {
        this._gameController = FindObjectOfType<GameController>();
        
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
            this._gameController.GanharPontos(this.valePontos);
            
            Destroy(this.gameObject);

            Instantiate(this.explosao, this.transform.position, Quaternion.identity);
        }
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