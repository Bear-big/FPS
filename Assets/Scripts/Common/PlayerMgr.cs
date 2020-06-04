using UnityEngine;
using System.Collections;
[RequireComponent(typeof(PlayerCtr))]
public class PlayerMgr : MonoBehaviour {
    private static PlayerMgr instance;
    public PlayerCtr currentPlayer;
    public static PlayerMgr Instance
    {
        get
        {
            return instance;
        }
    }
	// Use this for initialization
	void Awake () {
        instance = this;
        currentPlayer = GetComponent<PlayerCtr>();
	}
	
}
