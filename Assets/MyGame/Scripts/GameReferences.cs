using UnityEngine;
[AddComponentMenu("DangSon/GameReferences")]
public class GameReferences : MonoBehaviour
{
    private static GameReferences instance;
    public static GameReferences Instance
    {
        get => instance;
    }
    //
    [Header("Variable FX Prefabs")]
    public GameObject fxBulletsPrefabs;
    public GameObject explusionPrefabs;
    private void Awake()
    {
        if(instance!=null)
        {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this;
    }
}
