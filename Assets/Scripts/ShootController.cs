using Interfaces;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    // Velocidade do tiro
    [SerializeField] private float velocidade = 10f;

    // Dano que ele causa
    [SerializeField] private int danoCausado = 1;

    // Objeto de impacto do tiro
    [SerializeField] private GameObject impactoTiro;

    private Rigidbody2D _rigidbody2D;

    void Start()
    {
        this._rigidbody2D = GetComponent<Rigidbody2D>();

        this._rigidbody2D.velocity = new Vector2(0, this.velocidade);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destruindo se bater com o colisor
        if (other.CompareTag(Tags.Colisor))
        {
            Destroy(this.gameObject);
        }

        if (other.TryGetComponent(out ITomaDano tomaDano))
        {
            tomaDano.TomarDano(this.danoCausado);

            this.Destruir();
        }
    }

    private void Destruir()
    {
        Destroy(this.gameObject);

        if (this.impactoTiro)
        {
            Instantiate(this.impactoTiro, this.transform.position, Quaternion.identity);
        }
    }
}