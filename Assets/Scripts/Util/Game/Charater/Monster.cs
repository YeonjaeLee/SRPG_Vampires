using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    private Animator monsterAnim;
    private Transform monsterBG;
    float speed = 0.06f;

    private void Awake()
    {
        monsterAnim = transform.GetComponent<Animator>();
        monsterBG = transform.Find("BG");
    }
}
