using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {

    public Transform target;
    private Animator playerAnim;
    private Transform playerBG;
    float speed = 0.1f;
    Vector3[] path;
    int targetIndex;

    private void Awake()
    {
        //transform.GetComponent<Renderer>().material.color = Color.red;
        playerAnim = transform.GetComponent<Animator>();
        playerBG = transform.Find("BG");
    }

    public void PlayerMove()
    {
        PlayAnim("Move");
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        targetIndex = 0;
        CheckHeading(currentWaypoint);
        while (true)
        {
            if(transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    PlayAnim("Idle");
                    yield break;
                }

                currentWaypoint = path[targetIndex];
                CheckHeading(currentWaypoint);
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed);
            yield return null;
        }
    }

    // 길 보이게
    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }

    private void CheckHeading(Vector3 currentWaypoint)
    {
        if((int)transform.position.x != (int)currentWaypoint.x && (int)transform.position.z != (int)currentWaypoint.z)
        {
            playerBG.rotation = Quaternion.Euler(0, (((int)transform.position.x < (int)currentWaypoint.x ? 90f : 270f) + (int)transform.position.z < (int)currentWaypoint.z ? 0f : 180f) / 2, 0);
        }
        else if((int)transform.position.x != (int)currentWaypoint.x)
        {
            playerBG.rotation = Quaternion.Euler(0, (int)transform.position.x < (int)currentWaypoint.x ? 90f : 270f, 0);
        }
        else if ((int)transform.position.z != (int)currentWaypoint.z)
        {
            playerBG.rotation = Quaternion.Euler(0, (int)transform.position.z < (int)currentWaypoint.z ? 0f : 180f, 0);
        }
    }
    private void PlayAnim(string anim)
    {
        playerAnim.Play(anim);
    }
}
