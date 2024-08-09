using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

[Serializable]
public class Blocks
{
    public GameObject block;
    public GameObject bicakizi;
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] Ball ball;
    [SerializeField] CameraFollow cf;

    [Header("VALUEABLES")]
    bool bicakFirlat;
    bool oyunBittimi;
    float knifeYpztsn = 0.75f;
    int knifeindex;
    [SerializeField] int sure;
    int seriAtisSayisi;

    [SerializeField] GameObject[] knifes;
    public List<Blocks> blocks;
    [SerializeField] ParticleSystem ballfx;
    public AudioSource[] Sounds;
    public Sprite[] sprites; //musicon, musicoff, soundon, soundof;
    public Image[] buttons; //musicon, soundon;

    [Header("CANVAS_SETTINGS")]
    [SerializeField] GameObject firstpanel;
    [SerializeField] GameObject pausebtn;
    [SerializeField] GameObject pausepanel;
    [SerializeField] GameObject winpanel;
    [SerializeField] GameObject gopanel;
    [SerializeField] GameObject exitpanel;
    [SerializeField] TextMeshProUGUI suretxt;
    [SerializeField] TextMeshProUGUI seriAtisSayisitxt;
    [SerializeField] TextMeshProUGUI Leveltxt;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    void Start()
    {
        SahneIlkislemler();
    }
    void SahneIlkislemler()
    {
        Time.timeScale = 0;
        knifes[knifeindex].transform.position = new Vector3(2.5f, knifeYpztsn, 0);
        knifes[knifeindex].SetActive(true);
        StartCoroutine(GeriSayim());


        if (PlayerPrefs.GetInt("Sound") == 1 && PlayerPrefs.GetInt("Music") == 1)
        {
            for (int i = 0; i < Sounds.Length; i++)
            {
                Sounds[i].mute = false;
            }
            buttons[0].sprite = sprites[0];
            buttons[1].sprite = sprites[2];
        }
        else if (PlayerPrefs.GetInt("Sound") == 1 && PlayerPrefs.GetInt("Music") == 0)
        {
            Sounds[0].mute = true;
            Sounds[1].mute = false;
            Sounds[2].mute = false;
            Sounds[3].mute = false;
            Sounds[4].mute = false;
            Sounds[5].mute = false;
            Sounds[6].mute = false;
            Sounds[7].mute = false;

            buttons[0].sprite = sprites[1];
            buttons[1].sprite = sprites[2];
        }
        else if (PlayerPrefs.GetInt("Sound") == 0 && PlayerPrefs.GetInt("Music") == 1)
        {

            Sounds[0].mute = false;
            Sounds[1].mute = true;
            Sounds[2].mute = true;
            Sounds[3].mute = true;
            Sounds[4].mute = true;
            Sounds[5].mute = true;
            Sounds[6].mute = true;
            Sounds[7].mute = true;
            buttons[0].sprite = sprites[0];
            buttons[1].sprite = sprites[3];
        }
        else if (PlayerPrefs.GetInt("Sound") == 0 && PlayerPrefs.GetInt("Music") == 0)
        {
            for (int i = 0; i < Sounds.Length; i++)
            {
                Sounds[i].mute = true;
            }
            buttons[0].sprite = sprites[1];
            buttons[1].sprite = sprites[3];
        }
    }
    void Update()
    {
        if (Input.touchCount > 0 & Input.touchCount == 1 & bicakFirlat & !oyunBittimi & !UIObjesimi())
        {
            Touch tch = Input.GetTouch(0);
            if (tch.phase == TouchPhase.Began)
            {
                Leveltxt.gameObject.SetActive(false);
                Sounds[5].Play();
                knifes[knifeindex].GetComponent<Knife>().ilerle = true;
                knifeindex++;
                knifeYpztsn += 0.5f;

                if (knifeindex <= knifes.Length - 1)
                {
                    knifes[knifeindex].transform.position = new Vector3(2.5f, knifeYpztsn, 0);
                    knifes[knifeindex].SetActive(true);
                }
            }
        }
    }
    IEnumerator GeriSayim()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (sure != 0 & bicakFirlat)
            {
                sure--;
                suretxt.text = sure.ToString();
            }
            else if (sure == 0)
            {
                Go("SureBitti");
                yield break;
            }
        }
    }
    public void BicakSaplandi()
    {
        Sounds[4].Play();
        blocks[knifeindex - 1].bicakizi.SetActive(true);
        cf.hedefler[0] = blocks[knifeindex - 1].block.transform;

        seriAtisSayisi++;
        seriAtisSayisitxt.text = seriAtisSayisi.ToString();

        if (!seriAtisSayisitxt.isActiveAndEnabled)
        {
            seriAtisSayisitxt.gameObject.SetActive(true);
        }
    }
    public void TopCarpti()
    {
        Sounds[7].Play();
        seriAtisSayisi = 0;
        seriAtisSayisitxt.gameObject.SetActive(false);
    }
    public void BallFxPlay(Transform pztsn)
    {
        ballfx.transform.position = pztsn.position;
        ballfx.gameObject.SetActive(true);
        ballfx.Play();
    }
    public void Win()
    {
        Sounds[0].mute = true;
        oyunBittimi = true;
        seriAtisSayisitxt.gameObject.SetActive(false);
        suretxt.gameObject.SetActive(false);
        StopAllCoroutines();
        ball.gameObject.SetActive(false);

        Sounds[1].Play();
        winpanel.SetActive(true);

        PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);

        seriAtisSayisi = 0;
    }
    public void Go(string durum = "topPatladi")
    {
        Sounds[1].mute = true;
        Sounds[6].Play();
        oyunBittimi = true;
        seriAtisSayisi = 0;
        seriAtisSayisitxt.gameObject.SetActive(false);
        StopAllCoroutines();
        suretxt.gameObject.SetActive(false);
        knifes[knifeindex].SetActive(false);

        if (durum == "SureBitti")
        {
            ball.gameObject.SetActive(false);
        }

        Sounds[2].Play();
        gopanel.SetActive(true);
    }
    public void CanvasIslemleri(string durum)
    {
        switch (durum)
        {
            case "start":
                Time.timeScale = 1;
                Sounds[3].Play();
                firstpanel.SetActive(false);
                pausebtn.SetActive(true);
                bicakFirlat = true;
                suretxt.gameObject.SetActive(true);
                Leveltxt.gameObject.SetActive(true);
                suretxt.text = sure.ToString();
                Leveltxt.text = SceneManager.GetActiveScene().name;
                break;
            case "paused":
                Sounds[3].Play();
                bicakFirlat = false;
                pausebtn.SetActive(false);
                pausepanel.SetActive(true);
                break;
            case "esc":
                Sounds[3].Play();
                bicakFirlat = true;
                pausepanel.SetActive(false);
                pausebtn.SetActive(true);
                break;
            case "retry":
                Sounds[3].Play();
                pausepanel.SetActive(false);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case "exitpanel":
                Sounds[3].Play();
                exitpanel.SetActive(true);
                break;
            case "no":
                Sounds[3].Play();
                exitpanel.SetActive(false);
                break;
            case "yes":
                Sounds[3].Play();
                Application.Quit();
                break;
            case "nextlevel":
                Sounds[3].Play();
                SceneManager.LoadScene(PlayerPrefs.GetInt("Level"));
                break;
            case "music":
                if (PlayerPrefs.GetInt("Music") == 1)
                {
                    PlayerPrefs.SetInt("Music", 0);
                    Sounds[0].mute = true;
                    buttons[0].sprite = sprites[1];
                }

                else if (PlayerPrefs.GetInt("Music") == 0)
                {
                    PlayerPrefs.SetInt("Music", 1);
                    Sounds[0].mute = false;
                    buttons[0].sprite = sprites[0];
                }
                break;

            case "sound":
                if (PlayerPrefs.GetInt("Sound") == 1)
                {
                    PlayerPrefs.SetInt("Sound", 0);

                    Sounds[1].mute = true;
                    Sounds[2].mute = true;
                    Sounds[3].mute = true;
                    Sounds[4].mute = true;
                    Sounds[5].mute = true;
                    Sounds[6].mute = true;
                    Sounds[7].mute = true;
                    buttons[1].sprite = sprites[3];
                }

                else if (PlayerPrefs.GetInt("Sound") == 0)
                {
                    PlayerPrefs.SetInt("Sound", 1);

                    Sounds[1].mute = false;
                    Sounds[2].mute = false;
                    Sounds[3].mute = false;
                    Sounds[4].mute = false;
                    Sounds[5].mute = false;
                    Sounds[6].mute = false;
                    Sounds[7].mute = false;
                    buttons[1].sprite = sprites[2];
                }
                break;
        }
    }
    bool UIObjesimi()
    {
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
            {
                return true;
            }
        }
        return false;
    }
}
