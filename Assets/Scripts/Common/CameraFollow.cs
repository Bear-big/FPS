using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {
    [SerializeField]
    private Transform target;

    /// <summary>
    /// 水平距离
    /// </summary>
    [SerializeField]
    private float m_Distance = 5f;
    /// <summary>
    /// 竖直高度
    /// </summary>
    [SerializeField]
    private float m_Height = 2f;

    [SerializeField]
    private float m_DistanceDamping = 15f;
    [SerializeField]
    private float m_HeightDamping = 20f;

    private Vector3 defaultP;
	void Start () {
        defaultP = target.position - transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        if (target == null) return;
        //transform.position = target.position - defaultP; //第一人称
        FollowLogic();
	}

    void FollowLogic()
    {
        Vector3 targetP = target.position;
        Vector3 currentP = transform.position;

        float tweenHorizontal = Mathf.Lerp(currentP.z, targetP.z - m_Distance, m_DistanceDamping * Time.deltaTime);
        float tweenVertical = Mathf.Lerp(currentP.y, targetP.y + m_Height, m_HeightDamping * Time.deltaTime);

        transform.position = new Vector3(targetP.x, tweenVertical, tweenHorizontal);

        transform.LookAt(target);
    }
}
