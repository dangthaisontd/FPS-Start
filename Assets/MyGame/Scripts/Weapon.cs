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
    public float bulletPrefabsTime = 3f;
    public int magazineSize = 35;
    [Header("Weapon")]
    public WeaponModel thisWeaponModel;
    public ShotingMode currentShotingMode = ShotingMode.Auto;
    [Header("MuzerFlash")]
    public ParticleSystem muzlerFlash;
    private Camera cam;
    private float bulletVelocity=100f;
    private bool isShoting;
    private int bulletsLef;
    private float distanceBullet=200f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {  
        cam = Camera.main;
        bulletsLef = magazineSize;
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
        if (isShoting&&bulletsLef>0)
        {
            FireWeapon();
        }
    }
    private void FireWeapon()
    {
        muzlerFlash.Play();
        Vector3 shotingDirection = CaculateDirectionAndSpred().normalized;
        GameObject bullet= Instantiate(bulletPrefabs,bulletsPawm.position, Quaternion.identity);
        bullet.transform.forward = shotingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shotingDirection*bulletVelocity,ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabsTime));
    }
    IEnumerator DestroyBulletAfterTime(GameObject bullet, float bulletTime)
    {
        yield return new WaitForSeconds(bulletTime);
        Destroy(bullet);
    }
    private Vector3 CaculateDirectionAndSpred()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(0.5f,0.5f,0));

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
    
}
