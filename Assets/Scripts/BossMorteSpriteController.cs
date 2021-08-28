using UnityEngine;
using UnityEngine.SceneManagement;

public class BossMorteSpriteController : MonoBehaviour
{
    // Objeto de explosão
    [SerializeField] private GameObject explosao;

    public void CriarExplosao()
    {
        var x = this.transform.position.x + Random.Range(0, 1);
        var y = this.transform.position.y + Random.Range(0, 1);
        var position = new Vector2(x, y);

        Instantiate(this.explosao, position, Quaternion.identity);
    }

    public void Reiniciar()
    {
        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager)
        {
            gameManager.Reiniciar();
        }
    }
}