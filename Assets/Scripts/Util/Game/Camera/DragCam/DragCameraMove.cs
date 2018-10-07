using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragCameraMove : MonoBehaviour {

    private Vector3 orginpos;
    public static bool Drag = false;

    void Update()
    {
        if(Drag == true)
        {
            if (!(Input.GetMouseButton(0)))
                return;

            float deltaX = Input.GetAxis("Mouse X") * 3f;
            float deltaY = Input.GetAxis("Mouse Y") * 3f;
            
            if (Input.GetMouseButtonDown(0))
            {
                orginpos = Input.mousePosition;
            }
            if (Input.GetMouseButton(0))
            {
                if (orginpos != Input.mousePosition)
                {
                    MoveForwards(deltaY);
                    MoveSide(deltaX);
                }
            }
        }
    }

    void MoveForwards(float aVal)
    {
        Vector3 fwd = transform.forward;
        fwd.y = 0;
        fwd.Normalize();
        transform.position -= aVal * fwd;
    }

    void MoveSide(float aVal)
    {
        Vector3 fwd = transform.right;
        fwd.y = 0;
        fwd.Normalize();
        transform.position -= aVal * fwd;
    }
}
