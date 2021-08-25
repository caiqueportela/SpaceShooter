using UnityEngine;

public class GameController : MonoBehaviour
{
    // Objetos inimigos
    [SerializeField] private GameObject[] inimigos;

    // Pontos do jogador
    private int _pontos;

    // Level atual do jogador
    private int _level = 1;

    // Tempo até a geração do próximo inimigo
    private float _tempoInimigos = 0f;

    // Intervalor entre a geração de inimigos
    [SerializeField] private float intervaloInimigos = 5f;

    // X mínimo para gerar inimigos
    [SerializeField] private float minXInimigo = -8.4f;

    // X máximo para gerar inimigos
    [SerializeField] private float maxXInimigo = 8.4f;

    // Y mínimo para gerar inimigos
    [SerializeField] private float minYInimigo = 5.8f;

    // Y máximo para gerar inimigos
    [SerializeField] private float maxYInimigo = 7f;

    // Quantidade de inimigos vivos na tela
    private int _inimigosVivos;

    void Update()
    {
        GerarInimigos();
    }

    public void GanharPontos(int pontos)
    {
        if (this._level >= 5)
        {
            pontos *= 2;
        }

        this._pontos += pontos;

        this.ChecarLevel();
    }

    private void ChecarLevel()
    {
        if (this._pontos >= (this._level * 5))
        {
            this._level++;
        }
    }

    public void DiminuirInimigosVivos()
    {
        this._inimigosVivos--;
    }

    private void GerarInimigos()
    {
        if (this._inimigosVivos > 0)
        {
            return;
        }

        this._tempoInimigos -= Time.deltaTime;

        if (this._tempoInimigos <= 0)
        {
            this._tempoInimigos = this.intervaloInimigos;
            
            // Quanto maior o level, mais inimigos gerados
            for (int i = 0, maximo = (this._level * 2); i <= maximo; i++)
            {
                var inimigo = this.inimigos[0];

                var chance = Random.Range(0f, this._level);

                if (chance * 10 >= Random.Range(1f, 100f))
                {
                    inimigo = this.inimigos[1];
                }

                var position = this.CalcularPosicao(inimigo.transform.localScale);

                Instantiate(inimigo, position, Quaternion.identity);
                
                this._inimigosVivos++;
            }
        }
    }

    private Vector2 CalcularPosicao(Vector3 tamanhoInimigo)
    {
        Collider2D hit;
        Vector2 position;
        var tentativas = 0;

        do
        {
            tentativas++;
            
            var posX = Random.Range(this.minXInimigo, this.maxXInimigo);
            var posY = Random.Range(this.minYInimigo, this.maxYInimigo);
            position = new Vector2(posX, posY);

            // Verificando se na posição gerada existe algum colisor
            hit = Physics2D.OverlapBox(position, tamanhoInimigo, 0f);
        } while (hit && tentativas <= 200);
        

        // Caso seja null, a posição está live
        return position;
    }
}