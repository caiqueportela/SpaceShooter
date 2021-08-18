using System;
using UnityEngine;

public class Inimigo02Controller : InimigoSimples
{
    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();
    }

    protected override void DirecionarTiro(GameObject tiro)
    {
        // Encontrando o player na cena
        var player = FindObjectOfType<PlayerController>();

        // Calculando a direção
        var direcao = player.transform.position - tiro.transform.position;
        
        // Normalizando a velocidade da direção
        direcao.Normalize();

        // Direção do tiro = player
        tiro.GetComponent<Rigidbody2D>().velocity = direcao * this.GetVelocidadeTiro(true);
        
        //Calculando angulo
        var angulo = Mathf.Atan2(direcao.y, direcao.x) * Mathf.Rad2Deg;
        
        // Definindo o angulo
        tiro.transform.rotation = Quaternion.Euler(0f, 0f, angulo + 90);
    }
}