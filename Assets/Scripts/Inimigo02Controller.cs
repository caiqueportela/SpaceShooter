using UnityEngine;

public class Inimigo02Controller : InimigoSimples
{
    [SerializeField] private float inicioMudarMovimentacaoY = 3f;

    private bool _mudouDirecao;

    protected new void Start()
    {
        base.Start();
    }

    protected new void Update()
    {
        base.Update();

        // Chegou na posição pra iniciar movimentos?
        if (this.transform.position.y < inicioMudarMovimentacaoY && !this._mudouDirecao)
        {
            // Verificando a direção de X a ser seguida
            var direcaoX = this.transform.position.x > 0 ? -1 : 1;

            // Definindo a nova velocidade/direção
            this.InimigoRigidbody2D.velocity = (new Vector2(direcaoX, -1)).normalized * this.GetVelocidade(true);

            this._mudouDirecao = true;
        }
    }

    protected override void DirecionarTiro(GameObject tiro)
    {
        // Encontrando o player na cena
        var player = FindObjectOfType<PlayerController>();

        if (!player)
        {
            return;
        }

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