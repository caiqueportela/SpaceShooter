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

    private void GerarInimigos()
    {
        this._tempoInimigos -= Time.deltaTime;

        if (this._tempoInimigos <= 0)
        {
            // Quanto maior o level, mais inimigos gerados
            for (int i = 0; i <= this._level; i++)
            {
                var inimigo = this.inimigos[0];

                var chance = Random.Range(0f, this._level);

                if (chance * 10 >= Random.Range(1f, 100f))
                {
                    inimigo = this.inimigos[1];
                }

                var posX = Random.Range(this.minXInimigo, this.maxXInimigo);
                var posY = Random.Range(this.minYInimigo, this.maxYInimigo);
                var position = new Vector3(posX, posY, this.transform.position.z);

                Instantiate(inimigo, position, Quaternion.identity);
            }

            this._tempoInimigos = this.intervaloInimigos;
        }
    }
}