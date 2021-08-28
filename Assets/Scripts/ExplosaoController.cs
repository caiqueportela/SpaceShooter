using UnityEngine;

public class ExplosaoController : MonoBehaviour
{
    [SerializeField] private AudioClip som;

    private void Start()
    {
        if (this.som)
        {
            AudioSource.PlayClipAtPoint(this.som, Vector3.zero);
        }
    }

    public void Morrer()
    {
        Destroy(this.gameObject);
    }
}