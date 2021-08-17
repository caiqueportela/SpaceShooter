using UnityEngine;

public class ExplosaoController : MonoBehaviour
{
    public void Morrer()
    {
        Destroy(this.gameObject);
    }
}