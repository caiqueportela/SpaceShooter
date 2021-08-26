using Interfaces;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    // Dano que ele causa
    [SerializeField] private int danoCausado = 1;

    // Objeto de impacto do tiro
    [SerializeField] private GameObject impactoTiro;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Destruindo se bater com o colisor
        if (other.CompareTag(Tags.Colisor))
        {
            Destroy(this.gameObject);
        }

        if (other.TryGetComponent(out ITomaDano tomaDano) && tomaDano.PodeTomarDano())
        {
            tomaDano.TomarDano(this.danoCausado);

            this.Destruir();
        }
        
        if (other.CompareTag(Tags.Escudo))
        {
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