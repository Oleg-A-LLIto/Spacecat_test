using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using GameEntities;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float EnginePower;
    [SerializeField] int AmmoCapacity;
    [SerializeField] float CooldownTime;
    [SerializeField] Camera mainCamera;
    [SerializeField] SpriteRenderer LaserBeam;
    bool thrustOn = false;
    public Player player { get; private set; }
    UITextRenderingBehaviour uiRenderer;

    void Awake()
    {
        float bulletSize = bulletPrefab.GetComponent<SpriteRenderer>().size.x / 2;
        float realSize = GetComponent<SpriteRenderer>().size.x / 4;
        player = new Player(UnityToNumerics.UtoN(transform.position), EnginePower, AmmoCapacity, CooldownTime, bulletSize, realSize);
        uiRenderer = FindObjectOfType<UITextRenderingBehaviour>();
    }  

    void Update()
    {
        if (player.destroy)
        {
            Destroy(gameObject);
        }
        transform.position = UnityToNumerics.NtoU(player.getPosition());
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        mousePos.z = 0;
        Vector3 direction = (mousePos - transform.position);
        direction.Normalize();
        player.direction = UnityToNumerics.UtoN(direction);
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        uiRenderer.fieldValue[0] = direction.x.ToString("0.00") + ", " + direction.y.ToString("0.00");
        if(thrustOn)
            player.Thrust(Time.deltaTime);
        uiRenderer.fieldValue[1] = player.Speed().ToString("0.00");
        uiRenderer.fieldValue[2] = player.ammo.ToString();
        uiRenderer.fieldValue[3] = player.cooldownProgress.ToString("0.0");
        uiRenderer.fieldValue[4] = transform.position.x.ToString("0.00") + ", " + transform.position.y.ToString("0.00");
        player.Reload(Time.deltaTime);
    }

    public void OnFire()
    {
        GameObject objBullet = Object.Instantiate(bulletPrefab);
        objBullet.GetComponent<BulletBehaviour>().bullet = player.Bullet();
    }

    public void OnLaser()
    {
        if(player.Laser())
            StartCoroutine(LaserAnimation());
    }

    IEnumerator LaserAnimation()
    {
        for (float i = 0; i < 0.12f; i += Time.deltaTime)
        {
            LaserBeam.color = new Color(1, 1, 1, 1 - Mathf.Abs(i - 0.06f) * (1 / 0.06f));
            yield return 0;
        }
        LaserBeam.color = new Color(0, 0, 0, 0);
    }

    public void OnToggleThrust()
    {
        thrustOn = !thrustOn;
    }
}
