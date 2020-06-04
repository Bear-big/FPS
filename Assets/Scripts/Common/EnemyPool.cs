using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyPool : MonoBehaviour {
    private static EnemyPool instance;
    public static EnemyPool Instance {
        get {
            if (instance == null)
            {
                GameObject go =  new GameObject("EnemyPool");
                instance = go.AddComponent<EnemyPool>();
            }
            return instance;
        }
    }
    [SerializeField]
    private GameObject spawnEnemyPrefab = null;
    [SerializeField]
    private float spawnNumber = 1;
    [SerializeField]
    private List<GameObject> spawnPool = new List<GameObject>();
    
    public List<GameObject> enemyList = new List<GameObject>();
	void Awake () {
        if(instance == null)
           instance = this;
        DontDestroyOnLoad(gameObject);
        if (spawnEnemyPrefab == null)
        {
            #if UNITY_EDITOR
                Debug.LogError("spawnEnemyPrefab is Null");
            #endif

            return;
        }
        Init();
	}
    //初始化操作
    void Init()
    {
        for (int index = 0; index < spawnNumber; index++)
        {
            GameObject enemy =  GameObject.Instantiate(spawnEnemyPrefab);
            AddToPool(enemy);

        }
    }

    //加入缓存池
    void AddToPool(GameObject enemy)
    {
        spawnPool.Add(enemy);
        enemy.transform.parent = transform;
        enemy.SetActive(false);
        ResetTransform(enemy.transform);
    }
    void RemoveFromPool(GameObject enemy)
    {
        spawnPool.Remove(enemy);
        enemy.SetActive(true);
    }
    //创建敌人
    public GameObject CreateEnemy(Transform born = null)
    {
        GameObject enemy = null;
        if (spawnPool.Count == 0)
        {
            enemy = NewEnemy();
            AddToPool(enemy);
        }
        int index = Random.Range(0,spawnPool.Count);
        enemy = spawnPool[index];
        if (born != null){
            enemy.transform.parent = born;
            enemy.transform.position = Vector3.zero;
            //Vector3 bornP = born.position;
            //bornP.y = 0;
            enemy.transform.position = born.position + new Vector3(Random.Range(-5f, 5f), 0, Random.Range(-5f, 5f));
            //ResetTransform(enemy.transform);
        }
        RemoveFromPool(enemy);
       
        enemyList.Add(enemy);
        return enemy;
    }

    void ResetTransform(Transform t)
    {
        t.localScale = Vector3.one;
        t.localPosition = Vector3.zero;
    }

    //回收敌人
    public void RecycleEnemy(GameObject enemy)
    {
        if (!enemyList.Contains(enemy))
        {
            #if UNITY_EDITOR
                Debug.LogError("Wrong");
            #endif
            return;
        }
        enemyList.Remove(enemy);
        AddToPool(enemy);
    }

    //创建敌人游戏对象
    GameObject NewEnemy()
    {
         GameObject enemy =  GameObject.Instantiate(spawnEnemyPrefab);
         enemy.transform.parent = transform;
         enemy.transform.localScale = Vector3.one;
         enemy.transform.localPosition = Vector3.zero;
         return enemy;
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown("c"))
    //    {
    //        enemyList.Add(CreateEnemy(GameObject.Find("EnemyBornP").transform));
    //    }
    //    if (Input.GetKeyDown("r"))
    //    {
    //        int i = Random.Range(0, enemyList.Count);
    //        Debug.Log(i);
    //        RecycleEnemy(enemyList[i]);
    //        enemyList.Remove(enemyList[i]);
    //    }
    //}
    public void ClearAll()
    {
        enemyList.Clear();
        spawnPool.Clear();

    }
}
