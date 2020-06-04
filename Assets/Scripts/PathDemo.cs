using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDemo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //项目Asset目录
        Debug.Log(Application.dataPath);
        //持久化路径，可读可写路径
        Debug.Log(Application.persistentDataPath);
        //项目目录下的Streaming Assets目录，只读，并且直接打包进项目，不进行任何压缩
        Debug.Log(Application.streamingAssetsPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
