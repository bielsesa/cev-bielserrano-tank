using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TankController : MonoBehaviour
{
    public float velocidadPosicion = 15.0f;
    public float velocidadRotacion = 50.0f;
    public float cadenciaDisparoSegundos = 15f;
    [Space]
    public float temporizador = 5f;
    [Space]
    public ParticleSystem cannonParticleSystem;

    private bool canPlay = true;
    private bool canShoot = true;
    private float projectileCooldownCurrent = 0;

    private GameObject endingMessageUI;
    private GameObject projectileCooldownUI;
    private TextMeshProUGUI projectileCooldownText;

    private void Start()
    {
        endingMessageUI = GameObject.Find("/UI/EndingMessage");
        endingMessageUI.SetActive(false);

        projectileCooldownUI = GameObject.Find("/UI/ProjectileCooldownTitle/ProjectileCooldown");
        projectileCooldownText = projectileCooldownUI.GetComponent<TextMeshProUGUI>();
        projectileCooldownText.text = "Can shoot";
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
        if (canPlay)
        {
            if (temporizador > 0)
            {
                temporizador -= Time.deltaTime;
            }
            else
            {
                canPlay = false;
                temporizador = 0;
                //TODO: Mostrar mensaje "Te has quedado sin gasolina"
                //      Actualizar UI?
                endingMessageUI.SetActive(true);
            }
        }

    }

    private void Rotar()
    {
        if (canPlay && Input.GetAxis("Horizontal") != 0)
        {
            Vector3 movimientoRotacion = new Vector3(0f, Math.Sign(Input.GetAxis("Horizontal")) * velocidadRotacion * Time.deltaTime, 0f);
            transform.Rotate(movimientoRotacion, Space.Self);
        }
    }

    private void Mover()
    {
        if (canPlay && Input.GetAxis("Vertical") != 0)
        {
            Vector3 movimientoPosicion = new Vector3(0f, 0f, Math.Sign(Input.GetAxis("Vertical")) * velocidadPosicion * Time.deltaTime);
            transform.Translate(movimientoPosicion, Space.Self);
        }
    }


    private void Disparar()
    {
        CompruebaCadencia();

        if (canPlay && canShoot && Input.GetKeyDown(KeyCode.Space))
        {
            cannonParticleSystem.Play();
            canShoot = false;
            projectileCooldownCurrent = cadenciaDisparoSegundos;
#if DEBUG
            Debug.Log("Shot! Now canShoot is false and projectileCooldownCurrent has been reset to maximum cooldown");
#endif
        }
    }
    private void CompruebaCadencia()
    {
        if (canPlay)
        {
            if (!canShoot && projectileCooldownCurrent <= 0f)
            {
                canShoot = true;
                projectileCooldownText.text = "Can shoot";

#if DEBUG
                Debug.Log("Can shoot again");
#endif
            }
            else if (!canShoot)
            {
                projectileCooldownCurrent -= Time.deltaTime;
                projectileCooldownText.text = ((int)projectileCooldownCurrent).ToString();
#if DEBUG
                Debug.Log("Projectile on cooldown...");
                Debug.Log($"Projectile cooldown value: {projectileCooldownCurrent}");
#endif
            }
        }
    }
}
