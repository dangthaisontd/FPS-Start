using System.Collections;
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
    private bool allowReset;
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {  
        cam = Camera.main;
        bulletsLef = magazineSize;
        readyToShoot = true;
        allowReset = true;
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
    }
    private void FireWeapon()
    {
        readyToShoot = false;
        muzlerFlash.Play();
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
        return direction;
    }
    void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    
}
