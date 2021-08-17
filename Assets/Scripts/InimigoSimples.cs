using UnityEngine;

public class InimigoSimples : BaseInimigo
{
    private Rigidbody2D _rigidbody2D;
    
    protected void Start()
    {
        // Resgatando o rigidbody
        this._rigidbody2D = GetComponent<Rigidbody2D>();

        // Definindo velocidade
        this._rigidbody2D.velocity = new Vector2(0, this.velocidade);
    }
}