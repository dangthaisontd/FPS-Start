using System;
using UnityEngine;
[AddComponentMenu("DangSon/BulletDamageEnemy")]
public class BulletDamageEnemy : MonoBehaviour
{
   
    public int bulletsDamage = 20;
    private void OnCollisionEnter(Collision objectHit)
    {
        if(objectHit != null)
        {
            if(objectHit.collider.CompareTag("Wall"))
            {
              //  Debug.Log("Va cham tuong");
              CreateBulletImpactEffect(objectHit);
              Destroy(gameObject);
            }
            if (objectHit.collider.CompareTag("Box"))
            {
                //  Debug.Log("Va cham tuong");
                CreateExplusionEffect(objectHit);
                Destroy(gameObject);
            }
        }
    }
    private void CreateExplusionEffect(Collision objectHit)
    {
        Destroy(objectHit.gameObject);
        Instantiate(GameReferences.Instance.explusionPrefabs, transform.position, Quaternion.identity);
        AudioSource audio =  objectHit.gameObject.GetComponent<AudioSource>();
        if (audio != null)
        {
         audio.Play();
        }
        Destroy(gameObject);
    }

    private void CreateBulletImpactEffect(Collision objectHit)
    {
        ContactPoint contact = objectHit.contacts[0];
        GameObject hole = Instantiate(GameReferences.Instance.fxBulletsPrefabs, contact.point, Quaternion.LookRotation(contact.normal));
        hole.transform.SetParent(objectHit.gameObject.transform);
    }
}
