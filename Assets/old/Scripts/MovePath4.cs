using UnityEngine;
using System.Collections;
using System.Threading;

[System.Serializable]
public class MovePath4 : MovePathGeneral
{
    [SerializeField]
    public float fadeInterval = 0.1f;
    public float animRatio =2.5f;


    [SerializeField]
    public Vector3 startPos;
    [SerializeField]
    public Vector3 finishPos;
    [SerializeField]
    public int w;

    [SerializeField]
    public int targetPoint;
    [SerializeField]
    public int targetPointsTotal;

    [SerializeField]
    public string animName;
    [SerializeField]
    public float maxSpeed;
    public float moveSpeed;
    [SerializeField]
    public bool loop;
    [SerializeField]
    public bool fire=false;
    [SerializeField]
    public bool forward;
    [SerializeField]
    public GameObject walkPath3;
    public GameObject walkPath;
    public GameObject walkPath2;
    public GameObject walkPath0;
    [SerializeField]
    public GameObject fireStart;
    [SerializeField]
    public GameObject fireStart1;
    [SerializeField]
    public GameObject fireStart2;
    float minStartTime;
    public GameObject countDown;
    public int[] choice = new int[100];
    Timer timekeeper;
    public int Bi = 1;
    public string[] animNames;
    public bool Stopman = true;
    int BeforePoint;
    int pace = 0;
    bool FireBegin = false;
    private Quaternion BeforeRotation;
    int targetMask;
    float cntDown2 = 5;
    bool Upstairs = false;
    GameObject UpstairsWalk = null;
    public RaycastHit hit;
    bool Run3Arrival = false;
    bool Upstairs3 = false;
    GameObject UpstairsWalk3, Block3 = null;
    private bool isRun4Broken;
    GameObject staticnumber;
    public float time = 0;
    bool write = false;
    bool set = false;


    [Header("Attribute")]
    //性别
    public bool gender;
    //年纪
    public  int age;
    //环境熟悉度
    public float similarity;
    //
    //public int season;
    //动态规划智能度的参数值（Deprecated）
    public float sensitiveFactor;
    private void generateAttribute(bool _gender)
    {
        gender = _gender;
        age = NPCAttributeController.GenerateAge();
        similarity = NPCAttributeController.GenerateSimilarity();
        sensitiveFactor = NPCAttributeController.GenerateSensitive();
        maxSpeed = NPCAttributeController.floatGenerateSpeed(gender, age);
        moveSpeed = maxSpeed * similarity;
    }



    public void MyStart(int _w, int _i, string anim, bool _loop, bool _forward, bool gender)
    {
        fire = false;
        forward = _forward;
        generateAttribute(gender);
        walkPath = GameObject.Find("Run4");
        var _WalkPath = walkPath.GetComponent<WalkPath4>();
        w = _w;
        targetPointsTotal = _WalkPath.getPointsTotal(0) - 2;
        loop = _loop;
        animName = anim;
        pace = 0;
        if (fire == false)
        {
            targetPoint = _i;
            finishPos = _WalkPath.getNextPoint(w, _i);
        }  
    }
    void Start()
    {
        GetComponent<Animator>().CrossFade(animName, 0.1f, 0, Random.Range(0.0f, 1.0f));
        fire = false;
        targetMask = LayerMask.GetMask("target");
        UpstairsWalk = GameObject.Find("Run2");
        UpstairsWalk3 = GameObject.Find("Run0");
        Block3 = GameObject.Find("Run3");
        staticnumber = GameObject.Find("FPSController");
        walkPath2 = GameObject.Find("Run2");
        walkPath0 = GameObject.Find("Run0");
        walkPath3 = GameObject.Find("Run3");

    }
    void Update()
    {
        isRun4Broken = walkPath.GetComponent<WalkPath4>().isRunBroken;
        var _static = staticnumber.GetComponent<Control>();
        if (walkPath.GetComponent<WalkPath4>().StartRun == true)
        {
            fire = true;
            if (set == false)
            {
                time = Time.time;
                set = true;
            }
        }
        else
            fire = false;
        if (Upstairs == false && isRun4Broken==false)
        {         
            var _WalkPath = walkPath.GetComponent<WalkPath4>();
            if (Physics.Raycast(transform.position + new Vector3(0, 1.8f, 0), -transform.up, out hit, Mathf.Infinity, 1 << (LayerMask.NameToLayer("station"))))
            {

                finishPos.y = hit.point.y;
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            Vector3 targetPos = new Vector3(finishPos.x, transform.position.y, finishPos.z);
            Vector3 targetVector = targetPos - transform.position;
            agentbases(targetVector);
            bool x = _WalkPath.Run[targetPoint, 0];
            agentbase(x);

            if (Vector3.Distance(transform.position, finishPos) > 0.2f)
            {
                run();
            }
            else if (Vector3.Distance(transform.position, finishPos) <= 0.2f)
            {             
                if (fire == true && _WalkPath.Run[targetPoint, 0] == true)
                {
                    if (targetPoint == 35)
                    {
                        Upstairs = true;
                        _static.Bs5--;
                        _static.Bs2++;
                        //My implements for test
                        _static.cntTest++;
                        this.gameObject.AddComponent<MovePath2>();
                        this.gameObject.tag = "run2";
                        this.gameObject.GetComponent<MovePath2>().MyStart(0, 187, "run", false, false,gender);
                        this.gameObject.GetComponent<MovePath4>().enabled = false;
                    }
                    else if (targetPoint==34)
                        {
                        Upstairs = true;
                        _static.Bs5--;
                        _static.Bs2++;
                        //My implements for test
                        _static.cntTest++;
                        this.gameObject.AddComponent<MovePath2>();
                        this.gameObject.tag = "run2";
                        this.gameObject.GetComponent<MovePath2>().MyStart(0, 188, "run", false, false,gender);
                        this.gameObject.GetComponent<MovePath4>().enabled = false;
                    }
                    else if (targetPoint == 58)
                    {
                        Upstairs = true;
                        _static.Bs5--;
                        _static.Bs2++;
                        //My implements for test
                        _static.cntTest++;
                        this.gameObject.AddComponent<MovePath2>();
                        this.gameObject.tag = "run2";
                        this.gameObject.GetComponent<MovePath2>().MyStart(0, 188, "run", false, false,gender);
                        this.gameObject.GetComponent<MovePath4>().enabled = false;
                    }


                    else
                    {
                        int originalPoint = targetPoint;
                        targetPoint = _WalkPath.result[targetPoint, 2];
                        //3.26
                        _WalkPath.modifyPeopleToward(this, originalPoint, targetPoint);
                        finishPos = _WalkPath.getNextPoint(w, targetPoint);
                    }               
                }           
            }
        }
        else if (Upstairs == true)
        {
        }
        else if (isRun4Broken == true && Run3Arrival == false)
        {
            var _WalkPath = walkPath.GetComponent<WalkPath4>();
            RaycastHit hit;
            if (Physics.Raycast(transform.position + new Vector3(0, 1.8f, 0), -transform.up, out hit, Mathf.Infinity, 1 << (LayerMask.NameToLayer("station"))))
            {

                finishPos.y = hit.point.y;
                transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
            }
            Vector3 targetPos = new Vector3(finishPos.x, transform.position.y, finishPos.z);
            Vector3 targetVector = targetPos - transform.position;
            agentbases(targetVector);

            if (cntDown2 >= 0)
            {
                cntDown2 -= Time.deltaTime;
            }
            else
            {
                bool x = _WalkPath.Run[targetPoint, 0];
                agentbase(x);
            }

            if (Vector3.Distance(transform.position, finishPos) > 0.2f)
            {
                run();
            }
            else
                if (Vector3.Distance(transform.position, finishPos) <= 0.2f)
            {
                if (fire == true && _WalkPath.Run[targetPoint, 0] == true)
                {
                    if (targetPoint == 59)
                    {
                        _static.Bs5--;
                        _static.Bs4++;
                        Run3Arrival = true;
                        this.gameObject.AddComponent<MovePath3>();
                        this.gameObject.tag = "run3";
                        this.gameObject.GetComponent<MovePath3>().MyStart(0, 12, "run3", false, false,gender);
                        this.gameObject.GetComponent<MovePath4>().enabled = false;

                    }
                    else if (targetPoint == 60|| targetPoint == 61)
                    {
                        _static.Bs5--;
                        _static.Bs4++;
                        Run3Arrival = true;
                        this.gameObject.AddComponent<MovePath3>();
                        this.gameObject.tag = "run3";
                        this.gameObject.GetComponent<MovePath3>().MyStart(0, 15, "run3", false, false,gender);
                        this.gameObject.GetComponent<MovePath4>().enabled = false;
                    }
                    else
                    {
                        int originalPoint = targetPoint;
                        targetPoint = _WalkPath.result[targetPoint, 2];
                        //3.26
                        _WalkPath.modifyPeopleToward(this, originalPoint, targetPoint);
                        finishPos = _WalkPath.getNextPoint(w, targetPoint);
                    }
                }
            }
        }
    }
    public void BezierOpt(int Bnum, RaycastHit hit)
    {
        var _WalkPath = walkPath.GetComponent<WalkPath4>();
        Vector3 temp = _WalkPath.Bpath[Bnum, Bi];
        temp.y = hit.point.y;
        transform.position = Vector3.MoveTowards(transform.position, temp, Time.deltaTime * 1.0f * moveSpeed);
        if (Bi < _WalkPath.Bcounts - 3)
        {
            finishPos = _WalkPath.Bpath[Bnum, Bi + 1];
            finishPos.y = hit.point.y;
            Bi++;
        }
        else
        {
            temp = _WalkPath.Bpath[Bnum, _WalkPath.Bcounts - 2];
            temp.y = hit.point.y;
            transform.position = Vector3.MoveTowards(transform.position, temp, Time.deltaTime * 1.0f * moveSpeed);
            Bi = 1;
            targetPoint = _WalkPath.result[targetPoint, 2];
            targetPoint = _WalkPath.result[targetPoint, 2];
            targetPoint = _WalkPath.result[targetPoint, 2];
            finishPos = _WalkPath.getNextPoint(w, targetPoint);
        }
    }
    public void agentbase(bool x)
    {
        float runWalkthreshold = 2f;
        if (fire == true && x &&
        Physics.Raycast(this.transform.position + new Vector3(0, 1.8f, 0), (finishPos - this.transform.position), out hit, runWalkthreshold, targetMask))
        {
            if (hit.distance < 0.54f)
            {
                bool stand = false;

                if (hit.collider.gameObject.tag == "run0")
                {
                    var collision = hit.collider.gameObject.GetComponent<MovePath>();
                    if (collision.hit.collider != null)
                    {
                        if (collision.hit.collider.gameObject.ToString().Equals(this.gameObject.ToString()))
                        {
                            animName = "walk";
                            GetComponent<Animator>().CrossFade(animName, fadeInterval, 0);
                            moveSpeed = maxSpeed / 4f;
                            //男女动画不同
                            if (gender == false)
                            {
                                GetComponent<Animator>().speed = 2 * 0.45f / (1 * 1.34f);
                            }
                            else
                            {
                                GetComponent<Animator>().speed = 2 * 0.4f / (1 * 1.34f);
                            }
                            stand = true;
                        }
                    }
                }
                else if (hit.collider.gameObject.tag == "run1")
                {
                    var collision = hit.collider.gameObject.GetComponent<MovePath1>();
                    if (collision.hit.collider != null)
                    {
                        if (collision.hit.collider.gameObject.ToString().Equals(this.gameObject.ToString()))
                        {
                            animName = "walk";
                            GetComponent<Animator>().CrossFade(animName, fadeInterval, 0);
                            moveSpeed = maxSpeed / 4f;
                            //男女动画不同
                            if (gender == false)
                            {
                                GetComponent<Animator>().speed = 2 * 0.45f / (1 * 1.34f);
                            }
                            else
                            {
                                GetComponent<Animator>().speed = 2 * 0.4f / (1 * 1.34f);
                            }
                            stand = true;
                        }
                    }
                }
                else if (hit.collider.gameObject.tag == "run2")
                {
                    var collision = hit.collider.gameObject.GetComponent<MovePath2>();
                    if (collision.hit.collider != null)
                    {
                        if (collision.hit.collider.gameObject.ToString().Equals(this.gameObject.ToString()))
                        {
                            animName = "walk";
                            GetComponent<Animator>().CrossFade(animName, fadeInterval, 0);
                            moveSpeed = maxSpeed / 4f;
                            //男女动画不同
                            if (gender == false)
                            {
                                GetComponent<Animator>().speed = 2 * 0.45f / (1 * 1.34f);
                            }
                            else
                            {
                                GetComponent<Animator>().speed = 2 * 0.4f / (1 * 1.34f);
                            }
                            stand = true;
                        }
                    }
                }
                else if (hit.collider.gameObject.tag == "run3")
                {
                    var collision = hit.collider.gameObject.GetComponent<MovePath3>();
                    if (collision.hit.collider != null)
                    {
                        if (collision.hit.collider.gameObject.ToString().Equals(this.gameObject.ToString()))
                        {
                            animName = "walk";
                            GetComponent<Animator>().CrossFade(animName, fadeInterval, 0);
                            moveSpeed = maxSpeed / 4f;
                            //男女动画不同
                            if (gender == false)
                            {
                                GetComponent<Animator>().speed = 2 * 0.45f / (1 * 1.34f);
                            }
                            else
                            {
                                GetComponent<Animator>().speed = 2 * 0.4f / (1 * 1.34f);
                            }
                            stand = true;
                        }
                    }
                }
                else if (hit.collider.gameObject.tag == "run4")
                {
                    var collision = hit.collider.gameObject.GetComponent<MovePath4>();
                    if (collision.hit.collider != null)
                    {
                        if (collision.hit.collider.gameObject.ToString().Equals(this.gameObject.ToString()))
                        {
                            animName = "walk";
                            GetComponent<Animator>().CrossFade(animName, fadeInterval, 0);
                            moveSpeed = maxSpeed / 4f;
                            //男女动画不同
                            if (gender == false)
                            {
                                GetComponent<Animator>().speed = 2 * 0.45f / (1 * 1.34f);
                            }
                            else
                            {
                                GetComponent<Animator>().speed = 2 * 0.4f / (1 * 1.34f);
                            }
                            stand = true;
                        }
                    }

                }

                if (stand == false)
                {
                    if (animName != "listen")
                    {
                        animName = "listen";
                        GetComponent<Animator>().CrossFade(animName, fadeInterval, 0);
                    }
                    //Tests
                    moveSpeed = 0f;
                    //moveSpeed = maxSpeed / 4f;
                    GetComponent<Animator>().speed = 0.5f;
                }
            }
            else
             if (hit.distance >= 0.54f && hit.distance <= runWalkthreshold)
            {
                if (animName != "walk")
                {
                    animName = "walk";
                    GetComponent<Animator>().CrossFade(animName, fadeInterval, 0);
                }
                moveSpeed = ((hit.distance - 0.54f) / (3.8f - 0.54f)) * maxSpeed * similarity + 0.1f;
                float animSpeed = (moveSpeed < 1.0f) ? 1.0f : moveSpeed;
                //男女动画不同
                if (gender == false)
                {
                    GetComponent<Animator>().speed = 2 * 0.45f / (animSpeed * 1.34f);
                }
                else
                {
                    GetComponent<Animator>().speed = 2 * 0.4f / (animSpeed * 1.34f);
                }
            }
        }

        else if (fire == true && x &&
        !(Physics.Raycast(this.transform.position + new Vector3(0, 1.8f, 0), (finishPos - this.transform.position), out hit, runWalkthreshold, targetMask)) &&
        Physics.Raycast(this.transform.position + new Vector3(0, 1.8f, 0), (finishPos - this.transform.position), out hit, 3.8f, targetMask))
        {
            if (hit.distance >= runWalkthreshold && hit.distance <= 3.8f)
            {
                if (animName != "run")
                {
                    animName = "run";
                    GetComponent<Animator>().CrossFade(animName, fadeInterval, 0);
                }
                moveSpeed = ((hit.distance - 0.54f) / (3.8f - 0.54f)) * maxSpeed * similarity;
                float animSpeed = (moveSpeed < 1.5f) ? 1.5f : moveSpeed;
                //男女动画不同
                if (gender == false)
                {
                    GetComponent<Animator>().speed = 2 * 0.65f / (animSpeed * 0.8f);
                }
                else
                {
                    GetComponent<Animator>().speed = 2 * 0.6f / (animSpeed * 0.867f);
                }
            }
        }

        else if (fire == true && x &&
        !(Physics.Raycast(this.transform.position + new Vector3(0, 1.8f, 0), (finishPos - this.transform.position), out hit, runWalkthreshold, targetMask)) &&
        !(Physics.Raycast(this.transform.position + new Vector3(0, 1.8f, 0), (finishPos - this.transform.position), out hit, 3.8f, targetMask)))
        {
            if (animName != "run")
            {
                animName = "run";
                GetComponent<Animator>().CrossFade(animName, fadeInterval, 0);
            }
            moveSpeed = maxSpeed * similarity;
            float animSpeed = (moveSpeed < 1.5f) ? 1.5f : moveSpeed;
            //男女动画不同
            if (gender == false)
            {
                GetComponent<Animator>().speed = 2 * 0.65f / (animSpeed * 0.8f);
            }
            else
            {
                GetComponent<Animator>().speed = 2 * 0.6f / (animSpeed * 0.867f);
            }
        }
    }
    public void agentbases(Vector3 targetVector)
    {
        if (targetVector != Vector3.zero)
        {
            Quaternion look = Quaternion.identity;
            if (animName.Equals("walk"))
                look = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetVector), Time.deltaTime * 4f * moveSpeed);
            else
                if (animName.Equals("run"))
                look = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetVector), Time.deltaTime * 4f * moveSpeed);
            else
                 if (animName.Equals("listen"))
                look = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetVector), Time.deltaTime * 4f * 1f);
            transform.rotation = look;
        }
    }
    public void run()
    {
        if (transform.position != finishPos)
        {
            int size = 3;
            Vector3 onestep = new Vector3((finishPos.x - transform.position.x) / size, 0, (finishPos.z - transform.position.z) / size);
            for (int i = 1; i <= size; i++)
            {
                if (i != size)
                {
                    Vector3 temp;
                    float deltax = Random.Range(-4f, 4f);
                    float deltaz = Random.Range(-4f, 4f);
                    temp = transform.position + onestep + new Vector3(deltax, 0, deltaz);
                    transform.position = Vector3.MoveTowards(transform.position, temp, Time.deltaTime * 1.0f * moveSpeed / size);
                }
                else
                    transform.position = Vector3.MoveTowards(transform.position, finishPos, Time.deltaTime * 1.0f * moveSpeed);
            }
        }
    }
    
}
