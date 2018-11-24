using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CustomEditor(typeof(Control))] 
public class PeopleEditor : Editor {
    //地铁内点的数量
    public static int bias = 89;
    

    public override void OnInspectorGUI()
    {

        Control _control = target as Control;
        DrawDefaultInspector();



        List<Node> nodes = new List<Node>();
        for(int i=1;i<=WalkPath.pnum;i++)
        {
            if (i != 89 && i != 90 && i != 91 && i != 35 && i != 36 && i != 37)
                nodes.Add(new Node(0,i));
        }
        for (int i = 1; i <= WalkPath1.pnum; i++)
        {
            nodes.Add(new Node(1, i));
        }
        for (int i = 1; i <= WalkPath2.pnum; i++)
        {
            if(i<95||i>183)
                nodes.Add(new Node(2, i));
        }
        for (int i = 1; i <= WalkPath3.pnum; i++)
        {
            if(i!=33&&i!=34&&i!=80)
            nodes.Add(new Node(3, i));
        }
        for (int i = 1; i <= WalkPath4.pnum; i++)
        {
            if (i != 35 && i != 34 && i != 58 && i != 32 && i != 62 && i != 33)
                nodes.Add(new Node(4, i));
        }
        nodes = RandomSortList<Node>(nodes);


        if (GUILayout.Button("Populate!"))
        {
            //for(int i=0;i<nodes.Count;i++)
            //{
            //    Debug.Log(nodes[i].run + "-------" + nodes[i].point);
            //}
            GeneratePeopleFather();
            if (_control.maxNumber - bias>0)
            {
                for (int i = 0; i < _control.maxNumber - bias; i++)
                {
                    switch(nodes[0].run)
                    {
                        case 0:
                            _control.walkPath0.singleSpawn(nodes[0].point);
                            Node tmp0 = nodes[0];
                            nodes.RemoveAt(0);
                            nodes.Add(tmp0);
                            break;
                        case 1:
                            _control.walkPath1.singleSpawn(nodes[0].point);
                            Node tmp1 = nodes[0];
                            nodes.RemoveAt(0);
                            nodes.Add(tmp1);
                            break;
                        case 2:
                            _control.walkPath2.singleSpawn(nodes[0].point);
                            Node tmp2 = nodes[0];
                            nodes.RemoveAt(0);
                            nodes.Add(tmp2);
                            break;
                        case 3:
                            _control.walkPath3.singleSpawn(nodes[0].point);
                            Node tmp3 = nodes[0];
                            nodes.RemoveAt(0);
                            nodes.Add(tmp3);
                            break;
                        case 4:
                            _control.walkPath4.singleSpawn(nodes[0].point);
                            Node tmp4 = nodes[0];
                            nodes.RemoveAt(0);
                            nodes.Add(tmp4);
                            break;
                        default:
                            i--;
                            Debug.Log("Initial Error");
                            break;
                    }
                    Debug.Log("Initial 1");
                }
            }
            else
            {
                Debug.Log("Too less People");
            }
        }

        if (GUILayout.Button("Remove people"))
        {
            RemoveAllPeople();
        }

        if (GUILayout.Button("Count people"))
        {
            CountPeople();
        }

        EditorGUILayout.Space();

    }


    public void GeneratePeopleFather()
    {
        Control _control = target as Control;

        if (_control.walkPath0.par != null)
            DestroyImmediate(_control.walkPath0.par);
        if (_control.walkPath0.par == null)
        {
            _control.walkPath0.par = new GameObject();
            _control.walkPath0.par.transform.parent = _control.walkPath0.gameObject.transform;
            _control.walkPath0.par.name = "people";
        }

        if (_control.walkPath1.par != null)
            DestroyImmediate(_control.walkPath1.par);
        if (_control.walkPath1.par == null)
        {
            _control.walkPath1.par = new GameObject();
            _control.walkPath1.par.transform.parent = _control.walkPath1.gameObject.transform;
            _control.walkPath1.par.name = "people";
        }

        if (_control.walkPath2.par != null)
            DestroyImmediate(_control.walkPath2.par);
        if (_control.walkPath2.par == null)
        {
            _control.walkPath2.par = new GameObject();
            _control.walkPath2.par.transform.parent = _control.walkPath2.gameObject.transform;
            _control.walkPath2.par.name = "people";
        }

        if (_control.walkPath3.par != null)
            DestroyImmediate(_control.walkPath3.par);
        if (_control.walkPath3.par == null)
        {
            _control.walkPath3.par = new GameObject();
            _control.walkPath3.par.transform.parent = _control.walkPath3.gameObject.transform;
            _control.walkPath3.par.name = "people";
        }

        if (_control.walkPath4.par != null)
            DestroyImmediate(_control.walkPath4.par);
        if (_control.walkPath4.par == null)
        {
            _control.walkPath4.par = new GameObject();
            _control.walkPath4.par.transform.parent = _control.walkPath4.gameObject.transform;
            _control.walkPath4.par.name = "people";
        }
    }

    public void RemoveAllPeople()
    {
        Control _control = target as Control;
        if (_control.walkPath0.par != null)
            DestroyImmediate(_control.walkPath0.par);
        if (_control.walkPath1.par != null)
            DestroyImmediate(_control.walkPath1.par);
        if (_control.walkPath2.par != null)
            DestroyImmediate(_control.walkPath2.par);
        if (_control.walkPath3.par != null)
            DestroyImmediate(_control.walkPath3.par);
        if (_control.walkPath4.par != null)
            DestroyImmediate(_control.walkPath4.par);
    }

    public List<T> RandomSortList<T>(List<T> a)
    {
        List<T> b=new List<T>();
        int countNum = a.Count;
        //使用while循环，保证将a中的全部元素转移到b中而不产生遗漏
        while (b.Count < countNum)
        {
            //随机将a中序号为index的元素作为b中的第一个元素放入b中
            int index = Random.Range(0, a.Count - 1);
            //检测是否重复，保险起见
            if (!b.Contains(a[index]))
            {
                //若b中还没有此元素，添加到b中
                b.Add(a[index]);
                //成功添加后，将此元素从a中移除，避免重复取值
                a.Remove(a[index]);
            }
        }

        return b;
    }


    public void CountPeople()
    {
        int cnt=0;
        int cnt0 = 0;
        int cnt1 = 0;
        int cnt2 = 0;
        int cnt3 = 0;
        int cnt4 = 0;
        GameObject[] obj;
        obj = FindObjectsOfType(typeof(GameObject)) as GameObject[]; //关键代码，获取所有gameobject元素给数组obj
        foreach (GameObject child in obj)    //遍历所有gameobject
        {
            //Debug.Log(child.gameObject.name);  //可以在unity控制台测试一下是否成功获取所有元素
           if(child.tag=="run0"|| child.tag == "run1"|| child.tag == "run2" || child.tag == "run3" || child.tag == "run4" )
            {
                cnt++;
            }
           if(child.tag=="run0")
            {
                cnt0++;
            }
            if (child.tag == "run1")
            {
                cnt1++;
            }
            if (child.tag == "run2")
            {
                cnt2++;
            }
            if (child.tag == "run3")
            {
                cnt3++;
            }
            if (child.tag == "run4")
            {
                cnt4++;
            }



        }
        float rate = (float)(cnt0 + cnt3 + cnt4) / (float)cnt2;
        float rate2 = (float)(cnt0 + cnt3 ) / (float)(cnt2+cnt4);
        Debug.Log("人数总量为：" + cnt + " run0:" + cnt0 + " run1:" + cnt1 + " run2:" + cnt2 + " run3:" + cnt3 + " run4:" + cnt4);
        Debug.Log("Dynamic rate:" + rate);
        Debug.Log("Dynamic rate2:" + rate2);
    }
}


public class Node
{
    public Node(int r,int p)
    {
        run = r;
        point = p;
    }

    public int run;
    public int point;
}



