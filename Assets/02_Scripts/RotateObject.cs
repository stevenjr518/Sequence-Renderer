using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{

    Transform trans;
    float timer;
    float x;
    float y;
    float z;
    Color color;
    Renderer render;
    void Start()
    {
        trans = transform;
        render = GetComponent<Renderer>();
        Change();
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 3)
        {
            Change();
            timer = 0;
        }
        trans.Rotate(x , y , z );
        render.material.color = Color.Lerp(render.material.color, color, Time.deltaTime);
    }

    private void Change()
    {
        x = (float)Random.Range(0, 100)/100;
        y = (float)Random.Range(0, 100)/100;
        z = (float)Random.Range(0, 100)/100;
        color = new Color(x, y, z, 1);
    }
}
