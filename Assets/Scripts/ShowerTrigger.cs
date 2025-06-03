using UnityEngine;

public class ShowerTrigger : MonoBehaviour
{
    public GameObject foamEffect;
    public ParticleSystem showerEffect;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Baby"))
        {
            if (showerEffect != null && !showerEffect.isPlaying)
            {
                showerEffect.Play();
                Debug.Log("[SHOWER] Air menyala.");
            }

            if (foamEffect != null && foamEffect.activeSelf)
            {
                foamEffect.SetActive(false);
                Debug.Log("[SHOWER] Busa hilang.");
            }
        }
    }
}
