using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Color[] cores;
    [SerializeField] private Color32 corTitulo;
    [SerializeField] private Text txtTitulo;
    [SerializeField] private byte alpha;
    private bool _subir = false;
    private bool _descer = true;

    private void Awake()
    {
        var qtdGameManager = FindObjectsOfType<GameManager>().Length;
        if (qtdGameManager > 1)
        {
            Destroy(this.gameObject);
        }
        
        // Não serei destruido
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        if (!this.txtTitulo)
        {
            return;
        }
        
        this.GerarCor();
    }

    private void GerarCor()
    {
        var corN = Random.Range(0, this.cores.Length);
        this.corTitulo = this.cores[corN];
    }

    void Update()
    {
        if (!this.txtTitulo)
        {
            return;
        }
        
        if (this.alpha <= 0)
        {
            this.GerarCor();
            this._descer = false;
            this._subir = true;
        }
        
        if (this.alpha >= 255)
        {
            this._descer = true;
            this._subir = false;
        }
        
        if (this._subir)
        {
            this.alpha += 3;
        }
        
        if (this._descer)
        {
            this.alpha -= 3;
        }
        
        this.txtTitulo.color = new Color32(this.corTitulo.r, this.corTitulo.g, this.corTitulo.b, this.alpha);
    }
    
    IEnumerator PrimeiraCena()
    {
        yield return new WaitForSeconds(2f);
        
        SceneManager.LoadScene("Inicio");
    }
    
    public void Reiniciar()
    {
        StartCoroutine(PrimeiraCena());
    }

    public void IniciarJogo()
    {
        SceneManager.LoadScene("Jogo");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Inicio");
    }

    public void Sair()
    {
        Application.Quit();
    }
}