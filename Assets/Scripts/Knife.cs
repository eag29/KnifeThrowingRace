using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    [Header("VALUEABLES")]
    [SerializeField] bool firstKnife;
    public bool ilerle;
    bool hedefeGelindi;
    [SerializeField] float donusHizi;


    void Update()
    {
        if (firstKnife)
        {
            return;
        }
        if (!ilerle)
        {
            transform.Rotate(donusHizi * Time.deltaTime * 90, 0, 0, Space.World);
        }
        else
        {
            if (!hedefeGelindi)
            {
                transform.Translate(30 * Time.deltaTime * Vector3.left, Space.World);
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (firstKnife)
        {
            return;
        }
        if (other.CompareTag("varis"))
        {
            transform.SetPositionAndRotation(other.transform.position, other.transform.rotation);
            hedefeGelindi = true;
            GameManager.instance.BicakSaplandi();
        }
        else if (other.CompareTag("final"))
        {
            transform.SetPositionAndRotation(other.transform.position, other.transform.rotation);
            hedefeGelindi = true;
            GameManager.instance.BicakSaplandi();
            GameManager.instance.Win();
        }
    }
}
