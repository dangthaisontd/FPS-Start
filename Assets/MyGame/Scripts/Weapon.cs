using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
[AddComponentMenu("DangSon/Weapon")]
public class Weapon : MonoBehaviour
{
   public enum ShotingMode
    {
        Single,
        Burst,
        Auto
    }
    public enum WeaponModel
        {
        AK,
        Pistol
    }
    [Header("Bullets")]
    public Transform bulletsPawm;
    public GameObject bulletPrefabs;
    public float bulletVelocity = 100f;
    public float bulletPrefabsTime = 3f;
    public int magazineSize = 35;
    [Header("Fire Intensity")]
    [Range(0, 10f)]
    public float spreadIntensity;
    [Header("Range")]
    [Range(0f, 2f)]
    public float shootingdelay = 0.5f;
    [Header("Weapon")]
    public WeaponModel thisWeaponModel;
    public ShotingMode currentShotingMode = ShotingMode.Auto;
    [Header("MuzerFlash")]
    public ParticleSystem muzlerFlash;
    [Header("Audio Weapon")]
    public AudioClip shootClip;
    public AudioClip reloadClip;
    private Camera cam;
    private bool isShoting,readyToShoot;
    private int bulletsLef;
    private float distanceBullet=200f;
    private Animator anim;
    private int isShotingId;
    private int isReloadingCenterId;
    private int isReloadingRightId;
    private int isReloadingLeftId;
    private bool allowReset;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {  
        cam = Camera.main;
        bulletsLef = magazineSize;
        readyToShoot = true;
        allowReset = true;
        anim = GetComponent<Animator>();
        isShotingId = Animator.StringToHash("IsShooting");
        isReloadingCenterId = Animator.StringToHash("IsReloadingCenter");
        isReloadingLeftId = Animator.StringToHash("IsReloadingLeft");
        isReloadingRightId = Animator.StringToHash("IsReloadingRight");
    }

    // Update is called once per frame
    void Update()
    {
        if (currentShotingMode == ShotingMode.Auto)
        {
            isShoting = Input.GetKey(KeyCode.Mouse0);
        }
        else if((currentShotingMode == ShotingMode.Burst)||(currentShotingMode == ShotingMode.Single))
        {
            isShoting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        if (isShoting&&readyToShoot&&bulletsLef>0)
        {
            FireWeapon();
        }
        if (Input.GetKeyDown(KeyCode.R)&&bulletsLef<=magazineSize)
        {
            Reload();
        }
    }

    private void Reload()
    {
        int rand =UnityEngine.Random.Range(0, 3);
        switch(rand)
        {
            case 0: anim.SetTrigger(isReloadingLeftId);
                break;
            case 1:
                anim.SetTrigger(isReloadingCenterId);
                break;
            case 2:
                anim.SetTrigger(isReloadingRightId);
                break;
        }
        AudioManager.Instance.PlaysfxPlayer(reloadClip);
    }

    private void FireWeapon()
    {
        readyToShoot = false;
        muzlerFlash.Play();
        anim.SetTrigger(isShotingId);
        AudioManager.Instance.PlaysfxPlayer(shootClip);
        Vector3 shotingDirection = CaculateDirectionAndSpred().normalized;
        GameObject bullet= Instantiate(bulletPrefabs,bulletsPawm.position, Quaternion.identity);
        bullet.transform.forward = shotingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shotingDirection*bulletVelocity,ForceMode.Impulse);
        //bullet.GetComponent<Rigidbody>().linearVelocity = shotingDirection*bulletVelocity*Time.deltaTime
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabsTime));
        if(allowReset)
        {
            Invoke("ResetShot", shootingdelay);
            allowReset = false;
        }
        else
        {
            if(bulletsLef>0)
            {
                Invoke("FireWeapon", shootingdelay);
            }
        }
    }
    IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletTime)
    {
        yield return new WaitForSeconds(bulletTime);
        Destroy(bullet);
    }
    private Vector3 CaculateDirectionAndSpred()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f,0.5f,0));

        RaycastHit hit;
        Vector3 targetPoint=Vector3.zero;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(distanceBullet);
        }
        Vector3 direction = targetPoint - bulletsPawm.position;
        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        return direction+ new Vector3(0,y,z);
    }
    void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    
}
