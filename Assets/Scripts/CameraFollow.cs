using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("VALUEABLES")]
    public List<Transform> hedefler;
    [SerializeField] float OffsetY;
    float hareketYumusakliði = 0.8f;
    float minzoom = 80f;
    float maxzoom = 70f;
    float zoomlimit = 10f;
    Vector3 gecerlihiz;
    [SerializeField] Camera cm;


    void LateUpdate()
    {
        HareketEt();
        Zoom();
    }

    void Zoom()
    {
        if (MesafeHesapla() > 5)
        {
            float newZoom = Mathf.Lerp(maxzoom, minzoom, MesafeHesapla() / zoomlimit);

            cm.fieldOfView = Mathf.Lerp(cm.fieldOfView, newZoom, Time.deltaTime * 1.5f);
        }
        else
        {
            cm.fieldOfView = Mathf.Lerp(cm.fieldOfView, maxzoom, Time.deltaTime * 1.5f);
        }
    }

    float MesafeHesapla()
    {
        var bounds = new Bounds(hedefler[0].position, Vector3.zero);
        for (int i = 0; i < hedefler.Count; i++)
        {
            bounds.Encapsulate(hedefler[i].position);
        }
        return bounds.size.y;
    }

    void HareketEt()
    {
        Vector3 newpxtsn = new Vector3(transform.position.x, hedefler[0].position.y + OffsetY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, newpxtsn, ref gecerlihiz, hareketYumusakliði);
    }
}
