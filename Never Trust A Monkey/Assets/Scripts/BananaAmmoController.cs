using UnityEngine;

public class BananaAmmoController : MonoBehaviour
{
    public int spawnMin;
    public int spawnMax;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            GameObject nextBananas = Instantiate(gameObject);
            nextBananas.transform.position = new Vector3(Random.Range(spawnMin, spawnMax), 2.1f, Random.Range(spawnMin, spawnMax));
            nextBananas.GetComponent<AudioSource>().Play();

            other.GetComponent<PlayerController>().RefillAmmo();
            Destroy(gameObject);
        }
    }
}
