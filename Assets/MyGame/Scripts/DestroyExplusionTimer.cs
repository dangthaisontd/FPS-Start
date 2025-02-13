using UnityEngine;
[AddComponentMenu("DangSon/DestroyExplusionTimer")]
public class DestroyExplusionTimer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("DestroyTime", 1f);
    }

    // Update is called once per frame
    void DestroyTime()
    {
        Destroy(gameObject);
    }
}
