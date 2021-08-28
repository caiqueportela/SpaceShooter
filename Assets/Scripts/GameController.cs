using UnityEngine;

public class GameController : MonoBehaviour
{
    // Objetos inimigos
    [SerializeField] private GameObject[] inimigos;

    // Pontos do jogador
    [SerializeField] private int _pontos;

    // Level atual do jogador
    [SerializeField] private int _level = 1;

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

    // Level que o voss vai iniciar
    private int _levelBoss = 5;

    // Prefab da animação de entrada do boss
    [SerializeField] private GameObject prefabBossAnimacao;

    // Boss já foi criado?
    private bool _bossCriado;

    // Quantidade de inimigos vivos na tela
    private int _inimigosVivos;

    void Update()
    {
        if (this._level >= this._levelBoss)
        {
            this.GerarBoss();
            return;
        }

        GerarInimigos();
    }

    private void GerarBoss()
    {
        if (this._bossCriado) return;

        if (this._inimigosVivos > 0)
        {
            this._tempoInimigos = this.intervaloInimigos;
            return;
        }

        this._tempoInimigos -= Time.deltaTime;

        if (this._tempoInimigos > 0)
        {
            return;
        }

        var posicao = new Vector2(0f, -8f);

        var bossAnimacao = Instantiate(this.prefabBossAnimacao, posicao, Quaternion.identity);

        Destroy(bossAnimacao.gameObject, 8f);

        this._bossCriado = true;
    }

    public void GanharPontos(int pontos)
    {
        if (this._level >= 3)
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