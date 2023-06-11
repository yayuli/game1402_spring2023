using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    float lifeTime = 0.05f;
    private LineRenderer Line;
  
    
    // Start is called before the first frame update
    void Awake()
    {
        Line = GetComponent<LineRenderer>();
        
    }
    public void Init(Color c , Vector3 start, Vector3 end)
    {
        Line.SetPosition(0, start);
        Line.SetPosition(1, end);
        Line.startColor = c;
        Line.endColor = c;
        Invoke("DestroyMe", lifeTime );
    }
    private void DestroyMe ()
    {
        Destroy(this.gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
