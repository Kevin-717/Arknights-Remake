using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 2;
    public float nextWaypointDistance;
    public bool reachedEndOfPath;
    [HideInInspector]
    public Vector3 nextTargetPosition;
    public Seeker seeker;
    Path path;
    int currentWaypoint = 0;
    Vector3 targetLastPosition;
    void Start()
    {
        // seeker = GetComponent<Seeker>();
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }
    public void UpdatePath(Vector3 targetPosition)
    {
        // if (Vector3.Distance(targetPosition, targetLastPosition) > 0.1f)
        // {
            targetLastPosition = targetPosition;
            seeker.StartPath(transform.position, targetPosition, OnPathComplete);
        // }
    }
    public void NextTarget()
    {
        if (path == null)
        {
            return;
        }
        reachedEndOfPath = false;

        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;
        if (!reachedEndOfPath)
        {
            nextTargetPosition = path.vectorPath[currentWaypoint];
            Vector3 dir = (nextTargetPosition - transform.position).normalized;
            Vector3 velocity = dir * speed * speedFactor;
            transform.position += velocity * Time.deltaTime;
            if(path.vectorPath[currentWaypoint].x > transform.position.x){
                transform.eulerAngles = new Vector3(-30,0,0);
            }else{
                transform.eulerAngles = new Vector3(30,180,0);
                
            }
            MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetFloat("_angle",path.vectorPath[currentWaypoint].x <= transform.position.x ? -30f : 60f);
            GetComponent<MeshRenderer>().SetPropertyBlock(propertyBlock);
        }
        else
            path = null;
    }
}