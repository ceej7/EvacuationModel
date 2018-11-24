using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationController : MonoBehaviour {
    public GameObject start;
    public GameObject up;
    public GameObject down;
    public GameObject left;
    public GameObject right;
    public List<GameObject> dest;

    // Use this for initialization
    void Start () {
        naviTerminate();
    }
	
	// Update is called once per frame
	void Update () {
        if(dest.Count==0)
        {
            naviTerminate();
            return;
        }
        float alpha = calRotation(dest[0].transform.position.x - transform.position.x, dest[0].transform.position.z - transform.position.z, this.transform.rotation.eulerAngles.y);
        Debug.Log((dest[0].transform.position.x - transform.position.x) + "-------" + (dest[0].transform.position.z - transform.position.z) + "-------" + this.transform.rotation.eulerAngles.y);
        Debug.Log(alpha);
        if (alpha<=45&&alpha>=-45)
        {
            naviUp();
        }
        else if((alpha > 45 && alpha <= 135))
        {
            naviRight();
        }
        else if( (alpha < -45 && alpha >= -135))
        {
            naviLeft();
        }
        else
        {
            naviDown();
        }
    }

    public void naviTerminate()
    {
        //Debug.Log("naviTerminate");
        start.SetActive(false);
        up.SetActive(false);
        down.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
    }
    public void naviStart()
    {
        //Debug.Log("naviStart");
        start.SetActive(true);
        up.SetActive(false);
        down.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
    }
    public void naviUp()
    {
       // Debug.Log("naviUp");
        start.SetActive(false);
        up.SetActive(true);
        down.SetActive(false);
        left.SetActive(false);
        right.SetActive(false);
    }
    public void naviDown()
    {
        //Debug.Log("naviDown");
        start.SetActive(false);
        up.SetActive(false);
        down.SetActive(true);
        left.SetActive(false);
        right.SetActive(false);
    }
    public void naviLeft()
    {
        //Debug.Log("naviLeft");
        start.SetActive(false);
        up.SetActive(false);
        down.SetActive(false);
        left.SetActive(true);
        right.SetActive(false);
    }
    public void naviRight()
    {
       // Debug.Log("naviRight");
        start.SetActive(false);
        up.SetActive(false);
        down.SetActive(false);
        left.SetActive(false);
        right.SetActive(true);
    }

    public void newDest()
    {
        if (dest.Count == 0)
            return;
        dest.RemoveAt(0);
    }

    public float calRotation(float x,float z,float theta)
    {
        float len = Mathf.Sqrt(x * x + z * z);
        x = x / len;
        z = z / len;
        float _x=Mathf.Cos((90.0f  - theta) * Mathf.Deg2Rad);
        float _z = Mathf.Sin((90.0f  - theta) * Mathf.Deg2Rad);
        float alpha = Mathf.Acos(x * _x + z * _z)*Mathf.Rad2Deg;
        float tmp1 = Mathf.Cos(theta * Mathf.Deg2Rad) * x - Mathf.Sin(theta * Mathf.Deg2Rad) * z;
        if (tmp1>=0)
        {
            return alpha;
        }
        else
        {
            return -alpha;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="naviSeri")
        {
            newDest();
            Destroy(other.gameObject);
        }
    }
}
