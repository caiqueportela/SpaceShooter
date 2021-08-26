﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

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
    
    protected Rigidbody2D InimigoRigidbody2D;

    protected override void Start()
    {
        base.Start();
        
        // Resgatando o rigidbody
        this.InimigoRigidbody2D = GetComponent<Rigidbody2D>();

        // Definindo velocidade
        this.InimigoRigidbody2D.velocity = new Vector2(0, this.velocidade);
    }

    protected virtual void Update()
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
        if (this._proximoTiro > 0)
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