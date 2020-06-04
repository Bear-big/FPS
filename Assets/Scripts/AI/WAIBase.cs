using UnityEngine;
using System.Collections;


public class WAIBase : MonoBehaviour {

	public int _selfWayID;   //自己的寻路路线ID号

	public float moveSpeed = 1f; //移动速度
	public float rotateSpeed  = 20f;//旋转速度
    public CharacterController ccll;
    public Animator ani;
	void Awake(){
		OnAwake ();
	}

	void Start () {
		OnStart ();

	}
	//移动到目标点
	public void MoveToTargetWayPosition(Vector3 targetPosition)
	{
		Vector3 direction = targetPosition - transform.position;  //方向
		
		direction.y = 0;

		transform.rotation =  Quaternion.Slerp (transform.rotation,Quaternion.LookRotation (direction),Time.deltaTime * rotateSpeed);

		float speed = moveSpeed * Mathf.Clamp01 (Vector3.Dot(transform.forward.normalized,direction.normalized));

        
        ccll.SimpleMove(speed*direction.normalized);

	}

    void OnEnable() {
        AfterOnEnable();
		//transform.tr
    }
    void OnDisable()
    {
        AfterOnDisable();
    }
    protected virtual void AfterOnDisable() { }

	protected virtual void OnStart(){}
	protected virtual void OnAwake(){
	}
    protected virtual void AfterOnEnable() { }
}
