///
/// Scripts creados por Biel Serrano Sánchez para el curso B-TS3DV1OA2223. 
/// Ejercicio 03 del módulo de Desarrollo de entornos multidispositivo interactivos y videojuegos.
/// Fecha: 16-10-2022
///

using System;
using TMPro;
using UnityEngine;

public class TankController : MonoBehaviour
{
    [Header("Velocidades")]
    public float velocidadPosicion = 15.0f;
    public float velocidadRotacion = 50.0f;
    [Space]
    [Header("Temporizadores")]
    public float cadenciaDisparoSegundos = 15f;
    public float temporizador = 5f;
    [Space]
    public ParticleSystem cannonParticleSystem;
    [Space]
    [Header("UI")]
    public GameObject mensajeFinalObjetoUI;
    public GameObject cadenciaDisparoObjetoUI;
    public GameObject temporizadorObjetoUI;

    private bool puedeJugar = true;
    private bool puedeDisparar = true;
    private float cadenciaDisparoActual = 0;


    private TextMeshProUGUI cadenciaDisparoText;
    private TextMeshProUGUI temporizadorText;

    private void Start()
    {
        mensajeFinalObjetoUI.SetActive(false);

        cadenciaDisparoText = cadenciaDisparoObjetoUI.GetComponent<TextMeshProUGUI>();
        cadenciaDisparoText.text = "Can shoot";

        temporizadorText = temporizadorObjetoUI.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        Temporizador();
        Rotar();
        Mover();
        Disparar();
    }

    private void Temporizador()
    {
        if (puedeJugar)
        {
            if (temporizador > 0)
            {
                temporizador -= Time.deltaTime;
                ActualizaTemporizadorUI(temporizador);
            }
            else
            {
                puedeJugar = false;
                temporizador = 0;
                mensajeFinalObjetoUI.SetActive(true);
                GameObject.Find("UI/ProjectileCooldownPanel").SetActive(false);
            }
        }
    }

    void ActualizaTemporizadorUI(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        if (minutes == -1)
        {
            minutes = 0;
        }
        if (seconds == -1)
        {
            seconds = 0;
        }

        temporizadorText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void Rotar()
    {
        if (puedeJugar && Input.GetAxis("Horizontal") != 0)
        {
            Vector3 movimientoRotacion = new(0f, Math.Sign(Input.GetAxis("Horizontal")) * velocidadRotacion * Time.deltaTime, 0f);
            transform.Rotate(movimientoRotacion, Space.Self);
        }
    }

    private void Mover()
    {
        if (puedeJugar && Input.GetAxis("Vertical") != 0)
        {
            Vector3 movimientoPosicion = new(0f, 0f, Math.Sign(Input.GetAxis("Vertical")) * velocidadPosicion * Time.deltaTime);
            transform.Translate(movimientoPosicion, Space.Self);
        }
    }

    private void Disparar()
    {
        CompruebaCadencia();

        if (puedeJugar && puedeDisparar && Input.GetKeyDown(KeyCode.Space))
        {
            cannonParticleSystem.Play();
            puedeDisparar = false;
            cadenciaDisparoActual = cadenciaDisparoSegundos;
#if DEBUG
            Debug.Log("¡Disparo!");
#endif
        }
    }

    private void CompruebaCadencia()
    {
        if (puedeJugar)
        {
            if (!puedeDisparar && cadenciaDisparoActual <= 0f)
            {
                puedeDisparar = true;
                cadenciaDisparoText.text = "Can shoot";

#if DEBUG
                Debug.Log("Se puede volver a disparar.");
#endif
            }
            else if (!puedeDisparar)
            {
                cadenciaDisparoActual -= Time.deltaTime;
                cadenciaDisparoText.text = ((int)cadenciaDisparoActual).ToString();
            }
        }
    }
}
