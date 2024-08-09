using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] GameObject[] balls;
    Rigidbody rgb;
    bool topPatladimi;
    [SerializeField] ParticleSystem dumanfx;

    void Start()
    {
        rgb = gameObject.GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bicaksapi") && !topPatladimi)
        {
            rgb.velocity = Vector3.zero;
            rgb.AddForce(0, Random.Range(1, 4), 0,ForceMode.Impulse);
            GameManager.instance.TopCarpti();
        }
        else if (other.CompareTag("bicakucu") && !topPatladimi)
        {
            topPatladimi = true;
            GameManager.instance.Go("topPatladi");
            GameManager.instance.BallFxPlay(other.transform);;
            rgb.isKinematic = true;
            rgb.constraints = RigidbodyConstraints.FreezePositionZ;

            balls[0].SetActive(false);
            balls[1].SetActive(true);

            transform.position = new Vector3(transform.position.x - 0.5f, other.gameObject.transform.position.y,0);
        }
    }
}
