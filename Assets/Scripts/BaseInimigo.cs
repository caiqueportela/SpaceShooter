using System;
using Interfaces;
using UnityEngine;

public abstract class BaseInimigo : MonoBehaviour, ITomaDano
{
    // Velocidade de movimento
    [SerializeField] protected float velocidade = -3f;

    // Vida
    [SerializeField] private int vida = 1;

    // Objeto de explosão ao morrer
    [SerializeField] private GameObject explosao;

    // Velocidade do tiro
    [SerializeField] protected float velocidadeTiro = -10f;

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

    protected float GetVelocidadeTiro(bool normalized = false)
    {
        if (normalized)
        {
            return Mathf.Abs(this.velocidadeTiro);
        }

        return this.velocidadeTiro;
    }
}