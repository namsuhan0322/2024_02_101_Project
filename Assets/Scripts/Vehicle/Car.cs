using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 구체적인 클래스 : 자동차
public class Car : Vehicle              // Vehicle 상속
{
    public override void Horn()
    {
        Debug.Log("자동차 경적");
    }
}
