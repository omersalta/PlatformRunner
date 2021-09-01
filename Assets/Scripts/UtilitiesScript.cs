using System;
using UnityEngine;
using System.Collections.Generic;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static readonly Lazy<T> LazyInstance = new Lazy<T>(CreateSingleton);
    
    public static T Instance => LazyInstance.Value;

    private static T CreateSingleton()
    {
        var ownerObject = new GameObject($"{typeof(T).Name} (singleton)");
        var instance = ownerObject.AddComponent<T>();
        DontDestroyOnLoad(ownerObject);
        return instance;
    }
}

public class GameObjectUtil {

    private static ObjectPool pool = null;
    private static Dictionary<RecycleGameObject, ObjectPool> pools = new Dictionary<RecycleGameObject, ObjectPool> ();

    public static GameObject Instantiate(GameObject prefab, Vector3 localPos){
        GameObject instance = null;

        var recycledScript = prefab.GetComponent<RecycleGameObject> ();
        if (recycledScript != null) {
            var pool = GetObjectPool (recycledScript);
            instance = pool.NextObject (localPos).gameObject;
        } else {

            instance = GameObject.Instantiate (prefab);
            instance.transform.localPosition = localPos;
        }
        return instance;
    }

    public static void Destroy(GameObject gameObject){

        var recyleGameObject = gameObject.GetComponent<RecycleGameObject> ();

        if (recyleGameObject != null) {
            recyleGameObject.Shutdown ();
        } else {
            GameObject.Destroy (gameObject);
        }
    }

    public static void ResetPools() {
        foreach (var curentPool in pools) {
            curentPool.Value.ResetPool();
        }
        pools = new Dictionary<RecycleGameObject, ObjectPool> ();
    }
    
    private static ObjectPool GetObjectPool(RecycleGameObject reference){

        
        if (pools.ContainsKey (reference)) {
            pool = pools [reference];
        } else {
            var poolContainer = new GameObject(reference.gameObject.name + "ObjectPool");
            pool = poolContainer.AddComponent<ObjectPool>();
            pool.prefab = reference;
            pools.Add (reference, pool);
        }

        return pool;
    }

}