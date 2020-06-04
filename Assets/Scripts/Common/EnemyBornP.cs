using UnityEngine;
using System.Collections;

public class EnemyBornP : MonoBehaviour {
    [SerializeField]
    private Transform target;
    [SerializeField]
    private int createEnemyNumber = 3;
    [SerializeField]
    private bool isTriggerred = false;
	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        if (target == null)
            Debug.LogError("target is null");
	}
	
	// Update is called once per frame
	void Update () {
        if (Vector3ExtraTool.DistanceIgnoreYAxis(target.position, transform.position) < 5f && !isTriggerred)
        {
            isTriggerred = true;
            for (int index = 0; index < createEnemyNumber; index++)
            {
                EnemyPool.Instance.CreateEnemy(transform);
            }
        }
	}
}
