using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerCtr : MonoBehaviour {
    [SerializeField]
    private ETCJoystick stick;
    //private EasyJoystick easyJoy = null;
    //[SerializeField]
    private float damping = 20f;
    [SerializeField]
    private CharacterController ccll = null;
    [SerializeField]
    private float moveSpeed = 3f;
    [SerializeField]
    private Animator ani = null;

    bool isAttack = false;
    [SerializeField]
    private float canAttackDis = 1f;

    [SerializeField]
    [Range(0f,360f)]
    private float canAttackFOV = 80f;

    public Button Btn1;
    public Button Btn2;
    public Button Btn3;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //float x = Input.GetAxis("Horizontal");
        //float y = Input.GetAxis("Vertical");
        float x = stick.axisX.axisValue;

        float y = stick.axisY.axisValue;

        Vector3 direction = Vector3.zero;
        if (x != 0f || y != 0f)
        {
            direction = new Vector3(x, 0, y);
            direction = direction.normalized;
            Quaternion wantedRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, wantedRotation, damping * Time.deltaTime);
            float speed = Vector3.Dot(transform.forward.normalized, direction);
            speed = Mathf.Clamp01(speed);
            direction = direction * speed;
            ani.SetBool("ToRun", true);
            ani.SetInteger("CurrState", 0);
            if (isAttack)
            {
                isAttack = false;
                ani.SetInteger("ToPhyAttack", 0);
            }
        }
        else
        {
            if (isAttack) return;
           // ani.SetInteger("CurrState", 2);
            ani.SetBool("ToIdleFight", true);
            ani.SetBool("ToRun", false);
           
        }
        ccll.SimpleMove(direction*moveSpeed);
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && isAttack)
        {
            isAttack = false;
            ani.SetInteger("ToPhyAttack", 0);
        }


        Btn1.onClick.AddListener(
            delegate ()
            {
                isAttack = true;
                ani.SetBool("ToIdleFight", false);
                ani.SetInteger("ToPhyAttack", 1);
                ani.SetInteger("CurrState", 1);

                ani.SetBool("ToRun", false);
            }
            );
       
        


    }

    public void Btn3Onclick()
    {
        isAttack = true;
        ani.SetBool("ToIdleFight", false);
        ani.SetTrigger("Skill");
        ani.SetBool("ToRun", false);
    }

    public void Btn2Onclick()
    {
        isAttack = true;
        ani.SetBool("ToIdleFight", false);
        ani.SetInteger("ToPhyAttack", 2);
        ani.SetBool("ToRun", false);
    }

    public void DoAttack()
    {
        isAttack = true;
        ani.SetBool("ToIdleFight", false);
        ani.SetInteger("ToPhyAttack", Random.Range(1, 3));
        ani.SetBool("ToRun", false);
        for (int index = 0; index < EnemyPool.Instance.enemyList.Count; index++)
        {
            GameObject enemy = EnemyPool.Instance.enemyList[index];
            if (Vector3ExtraTool.DistanceIgnoreYAxis(enemy.transform.position, transform.position) > canAttackDis)
            {
                continue;
            }
            float dot = Vector3.Dot(transform.forward, (enemy.transform.position - transform.position).normalized);
            if (dot <= Mathf.Cos(Mathf.PI * canAttackFOV / 2 / 180)) //不在攻击区域
                continue;
            Debug.Log("DoHurt");
            enemy.SendMessage("DoHurt",Random.Range(5,20),SendMessageOptions.DontRequireReceiver);
            
        }
    }

}
