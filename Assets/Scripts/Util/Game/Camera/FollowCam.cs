using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

    public Transform target;
    public float dist = 10.0f;          // 카메라와의 일정거리
    public float height = 5.0f;         // 카메라 높이
    public float smoothRotate = 5.0f;   // 부드러운 회전 변수

    private Transform tr;               // 카메라 자신

    private void Start()
    {
        tr = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        // 부드러운 회전 위한
        float currYAngle = Mathf.LerpAngle(tr.eulerAngles.y, target.eulerAngles.y, smoothRotate * Time.deltaTime);

        // 오일러 타입을 쿼터니언으로 변환
        Quaternion rot = Quaternion.Euler(0, currYAngle, 0);

        // 카메라 위치를 타겟 회전각도만큼 회전 후 dist 만큼 띄우고, 높이 올리기
        tr.position = target.position - (rot * Vector3.forward * dist) + (Vector3.up * height);

        // 타겟 바라보게 하기
        tr.LookAt(target);
    }
}
