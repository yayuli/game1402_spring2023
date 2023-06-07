using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    //this is going to be the base class for both our PlayerController, and our eventual AIController. This is going to be shared properties between our PlayerController and our AI
    // Start is called before the first frame update
    protected Animator animator;
    //some things all units have: health, how much damage they do, their current team, a viewing angle, a rigid body, where they begin
    [SerializeField]
    int fullHealth = 100;
    [SerializeField]
    int team; //the team our unit belongs to
    [SerializeField]
    int health; //the current health value of our unit 
    [SerializeField]
    int damage = 10;
    private const float RAYCAST_LENGTH = 0.3f;
    protected Rigidbody rb;
    private Color myColor;
    [SerializeField]
    Laser laserPrefab;

    private Eye[] eyes = new Eye[2];
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        eyes = GetComponentsInChildren<Eye>();
        myColor = GameManager.Instance.teams[team];
        transform.Find("Teddy_Body").GetComponent<SkinnedMeshRenderer>().material.color = myColor;
    }
    public int Team
    {
        get
        {
            return team;
        }
    }
    protected virtual void OnHit(Unit attacker )
    {
        Debug.Log("Ouchie");
        health -= attacker.damage;
        if(health<=0)
        {

        }
    }
    protected Vector3 GetEyesPosition()
    {
        return (eyes[0].transform.position + eyes[1].transform.position) / 2.0f;
    }
    protected void ShootAt(RaycastHit hit)
    {
        Unit unit = hit .transform .GetComponent<Unit>();
        if(unit != null )
        {
            if (unit.team !=Team)
            {
                unit.OnHit(this);
                ShowLasers(hit.point);
            }
            
        }
    }
    protected void ShowLasers (Vector3 targetPosition)
    {
        foreach(Eye eye in eyes)
        {
            Laser laser = Instantiate(laserPrefab) as Laser;
            laser.Init(Color.red, eye.transform.position, targetPosition);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected bool IsGrounded() //we want to figure out if our character is on the ground or not
    {
        Vector3 origin = transform.position;//this is where our character begins
        origin.y += RAYCAST_LENGTH * 0.5f;
        LayerMask mask = LayerMask.GetMask("Terrain");
        return Physics.Raycast(origin, Vector3.down, RAYCAST_LENGTH, mask);
    }
}
