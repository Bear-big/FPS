using UnityEngine;
using System.Collections;


public class AIRole : WAIBase {

    [SerializeField]
    private Vector3 walkSize; //漫游区域
	[SerializeField]
	[Header("允许的误差距离")]
	private float _errorDis = 0.03f;  

	[SerializeField]
	private Transform _attackPlayer;  //要攻击的目标
	[SerializeField]
	private float _followDis = 5f;  //跟随距离
	[SerializeField]
	private float _attackDis = 3f;//攻击距离
    [SerializeField]
    private float _thinkTime = 4f; //思考时间
    private float currentTime = 0f;

    [SerializeField]
    private float HP = 200f; //血量
    private Vector3 targetPosition;  //目标位置
    [SerializeField]
    private Vector3 defaultPosition; //出生位置
    private float defaultSpeed;  //初始移动速度  给DeBuff用

    public delegate void KillerCall();
    public static KillerCall KillerCallEvent;//被杀死时调用

    bool isDie = false;
	protected override void OnStart ()
	{
		base.OnStart ();
	}
    
    //
    protected override void AfterOnEnable()
    {
        base.AfterOnEnable();
        Init();
        StartCoroutine(MoveLogic());
        
    }
	void Init()
	{
       // Mathf.Floor
        HP = 200f + Mathf.Ceil(Random.Range(-50f,50f));
        isDie = false;
        currentTime = 0f;
        ani.SetBool("ToDie", false);
        ani.SetBool("ToRun", false);
        ani.SetBool("ToIdleNormal", false);
        ani.SetBool("ToHurt", false);
        ani.SetBool("ToIdleFight", false);
        ani.SetInteger("CurrState", 0);
        ani.SetInteger("ToPhyAttack", 0);
        
        defaultPosition = transform.position;//出生位置
        defaultSpeed = moveSpeed;  //初始速度
        targetPosition = defaultPosition + RandomPoint();
        if (_attackPlayer == null)
            _attackPlayer = GameObject.FindGameObjectWithTag("Player").transform;
	}
    //
    Vector3 RandomPoint()
    {
        return new Vector3(Random.Range(-1f, 1f)*walkSize.x/2, 0, Random.Range(-1f, 1f)*walkSize.z/2);
    }
	IEnumerator MoveLogic()
	{
		while (true) {	

		    //抵达跟随区域
			if (Vector3ExtraTool.DistanceIgnoreYAxis (transform.position, _attackPlayer.position) <= _followDis) {
				yield return StartCoroutine (FollowLogic());
			}
			if (Vector3ExtraTool.DistanceIgnoreYAxis (transform.position, targetPosition) <=  _errorDis) {
                ani.SetBool("ToRun", false);
                targetPosition = RandomPoint() + defaultPosition;
                yield return StartCoroutine(ThinkLogic());
			}
            ani.SetBool("ToRun", true);
            MoveToTargetWayPosition(targetPosition); // 移动到目标点
			yield return new WaitForEndOfFrame ();
		}
	}

    IEnumerator ThinkLogic()
    {
        while (true)
        {
           //抵达跟随区域
			if (Vector3ExtraTool.DistanceIgnoreYAxis (transform.position, _attackPlayer.position) <= _followDis) {
                currentTime = 0f;
                ani.SetBool("ToIdleFight", false);
                yield break;
			}
            ani.SetBool("ToIdleFight", true);

            currentTime += Time.deltaTime;
            if (currentTime >= _thinkTime)
            {
                currentTime = 0f;
               ani.SetBool("ToIdleFight", false);
                yield break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

	IEnumerator FollowLogic()
	{
		while (true) {
            if (Vector3ExtraTool.DistanceIgnoreYAxis(transform.position, _attackPlayer.position) > _followDis)
            {
                yield break;
            }
            //抵达攻击区域
            if (Vector3ExtraTool.DistanceIgnoreYAxis(transform.position, _attackPlayer.position) <= _attackDis)
            {
                ani.SetBool("ToRun", false);
                ani.SetInteger("ToPhyAttack", Random.Range(1, 3));
                yield return StartCoroutine(AttackLogic());
            }
            ani.SetBool("ToRun", true);
			MoveToTargetWayPosition (_attackPlayer.position); // 移动到目标点
			yield return new WaitForEndOfFrame ();
		}
	}

    IEnumerator AttackLogic()
    {
        while (true)
        {
            //超出攻击区域
            if (Vector3ExtraTool.DistanceIgnoreYAxis(transform.position, _attackPlayer.position) > _attackDis)
            {
                ani.SetInteger("ToPhyAttack",0);
                yield break;
            }
            //Debug.Log(ani.GetCurrentAnimatorStateInfo(0).normalizedTime);
            //if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            //    ani.SetInteger("ToPhyAttack", Random.Range(1, 4));
            yield return new WaitForEndOfFrame();
        }
    }
    void Update()
    {
        ccll.SimpleMove(Vector3.zero);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(defaultPosition,walkSize);
        Gizmos.DrawIcon(targetPosition,"Icon");
    }

    void DoHurt(int hurt = 5)
    {
        if (isDie) return;
        HP -= hurt;
        if (HP <= 0f)
        {
            if (KillerCallEvent != null)
                KillerCallEvent();
           // ani.SetBool("ToDie", true);
            ani.CrossFade("Die",0.5f);

            ani.SetBool("ToIdleNormal", false);
            ani.SetBool("ToHurt", false);
            ani.SetBool("ToIdleFight", false);
            ani.SetInteger("ToPhyAttack", 0);
            moveSpeed = defaultSpeed;
           // ani.SetInteger("CurrState",5);
            isDie = true ;
            Invoke("RecycleEnemy",2f);
            return;
        }
        //减速效果 
        moveSpeed = defaultSpeed / 2f;
    }


    protected override void AfterOnDisable()
    {
        base.AfterOnDisable();
        StopAllCoroutines();
    }

    void RecycleEnemy()
    {
        EnemyPool.Instance.RecycleEnemy(this.gameObject);
    }



}
