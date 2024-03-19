using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFollowSegment : MonoBehaviour
{
    [SerializeField]private float pointSpacing = 0.5f;
    [SerializeField] private int pointLimit = 5;
    [SerializeField] private float moveSpeed = 2;
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] segments; 
    
    private List<Vector3> points = new List<Vector3>();
    private List<Vector2> forward = new List<Vector2>();

    private float playerDist;
    private float pointDist;

    private Vector2 tempPoint;

    private bool pointsExist;

    public Vector2 FollowPoint
    {
        get { return points[0]; }
    }

    public bool CloseToPlayer
    {
        get { return Vector2.Distance(transform.position,player.position) <= 2; }
    }

    private Vector3 prevPos;


    void Start()
    {
        CreateStartPoints();
    }

    public void CreateStartPoints()
    {
        if (pointsExist) return;
        for(int i=0;i<pointLimit;++i)
        {
            points.Add(transform.position);
        }

        pointsExist = true;
    }
    void Update()
    {
        DrawPoints();

        // MoveSegments();
        MoveSegment();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        UpdatePoints();
    }

    void MoveSegment()
    {
        transform.position += moveSpeed * Time.deltaTime * (points[0] - transform.position);
    }

    // void MoveSegments()
    // {
    //     // if (prevPos == transform.position) return;
    //     
    //     for (int i = 0; i < segments.Length; i++)
    //     {
    //         segments[i].position += (points[i] - segments[i].position) * moveSpeed * Time.deltaTime;
    //     }
    //
    //     prevPos = transform.position;
    // }

    void DrawPoints()
    {
        for(int i=0;i<pointLimit-1;++i)
        {
            Debug.DrawRay(points[i],points[i+1] - points[i], Color.green);
        }
        //print("p1: " + points[0] + "p2: " + points[points.Count-1]);
    }
    
    // Update the points when the player moves a certain distance away from the last point
    void UpdatePoints()
    {
        // 0 = newest, last = oldest
        playerDist = Vector3.Distance(points[points.Count-1], player.position);
        if (playerDist > pointSpacing)
        {
            points.Add(player.position);
            points.RemoveAt(0);
        }
    }
}
