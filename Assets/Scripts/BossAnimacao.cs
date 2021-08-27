using UnityEngine;

public class BossAnimacao : MonoBehaviour
{
    [SerializeField] private GameObject bossPrefab;

    public void CriarBoss()
    {
        Instantiate(this.bossPrefab, this.transform.position, Quaternion.identity);
    }
    
}
