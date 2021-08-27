using UnityEngine;
using Random = UnityEngine.Random;

public class BossController : BaseInimigo
{
    [SerializeField] private BossState state = BossState.Modo1;

    [SerializeField] private float minX;

    [SerializeField] private float maxX;
    
    [Header("Prefabs")]
    // Prefab tiro 2
    [SerializeField] private GameObject prefabTiro1;
    
    // Prefab tiro
    [SerializeField] private GameObject prefabTiro2;
    
    [Header("Posições")]
    // Posições do tiro 1
    [SerializeField] private Transform[] posicaoTiro1;
    
    // Porição do tiro 2
    [SerializeField] private Transform posicaoTiro2;

    // Tempo entre os tiros dependendo do estado
    [SerializeField] private SerializableDictionary<BossState, float> tempoEsperaTiro;
    
    // Tempo pro próximo tiro 2 do estado 3
    private float _proximoTiro2;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        
        switch (this.state)
        {
            case BossState.Modo1:
                Modo1();
                break;
            case BossState.Modo2:
                Modo2();
                break;
            case BossState.Modo3:
                Mode3();
                break;
        }
    }

    private void Mode3()
    {
        this.Movimentar();
        
        if (this.PodeAtirar())
        {
            this.DispararTiro1();
            
            // Define tempo do próximo tiro
            this.ProximoTiro = this.tempoEsperaTiro[this.state];
        }

        if (this._proximoTiro2 <= 0)
        {
            this.DispararTiro2();
            
            var proximoTiro = this.tempoEsperaTiro[this.state];
            this._proximoTiro2 = Random.Range(proximoTiro - 1, proximoTiro + 1);
        }
        else
        {
            this._proximoTiro2 -= Time.deltaTime;
        }
    }

    private void Modo2()
    {
        this.Rigidbody2D.velocity = Vector2.zero;
        
        if (this.PodeAtirar())
        {
            this.DispararTiro2();
            
            // Define tempo do próximo tiro
            this.ProximoTiro = this.tempoEsperaTiro[this.state];
        }
    }

    private void Modo1()
    {
        this.Movimentar();

        if (this.PodeAtirar())
        {
            this.DispararTiro1();
            
            // Define tempo do próximo tiro
            this.ProximoTiro = this.tempoEsperaTiro[this.state];
        }
    }

    private void Movimentar()
    {
        if (this.transform.position.x >= this.maxX)
        {
            this.Rigidbody2D.velocity = Vector2.left * this.velocidade;
        }

        if (this.transform.position.x <= this.minX)
        {
            this.Rigidbody2D.velocity = Vector2.right * this.velocidade;
        }

        if (this.Rigidbody2D.velocity == Vector2.zero)
        {
            this.Rigidbody2D.velocity = new Vector2(this.RandomSign(), 0f) * this.velocidade;
        }
    }

    private void DispararTiro1()
    {
        var tiro1 = Instantiate(this.prefabTiro1, this.posicaoTiro1[0].position, Quaternion.identity);
        this.DirecionarTiro(tiro1);
        
        var tiro2 = Instantiate(this.prefabTiro1, this.posicaoTiro1[1].position, Quaternion.identity);
        this.DirecionarTiro(tiro2);
    }
    
    private void DispararTiro2()
    {
        var tiro = Instantiate(this.prefabTiro2, this.posicaoTiro2.position, Quaternion.identity);
        
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

    private int RandomSign()
    {
        return Random.value < 0.5f ? 1 : -1;
    }
}