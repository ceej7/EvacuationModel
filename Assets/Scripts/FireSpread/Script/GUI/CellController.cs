using UnityEngine;
using UnityEngine.UI;
public class CellController : MonoBehaviour
{
    #region Variables
    
    bool search = false;
    int grid = 8;
    private GameObject BurnCell;
    private GameObject[,] BurnCells=new GameObject [8,8];
    private Cell _selectedCell = null;
    private float timer = 0f;
    public Cell SelectedCell
    {
        get
        {
            return this._selectedCell;
        }
        set
        {
            this._selectedCell = value;
        }
    }
    private Cell _lastSelectedCell = null;
    private int _cellLayerMask = 0;

    /*[SerializeField]
    private GameObject _cellControlPanel = null;

    [SerializeField]
    private GameObject _igniteButton = null;
    [SerializeField]
    private Text _igniteText = null;

    [SerializeField]
    private Slider _cellMassSlider = null;
    [SerializeField]
    private Text _cellMassLabel = null;

    [SerializeField]
    private Slider _cellMoistureSlider = null;
    [SerializeField]
    private Text _cellMoistureLabel = null;*/

    //[SerializeField]
    //private Dropdown _cellMaterialDropdown = null;

    //private float _lastCellMassSliderValue = 0.0f;
    //private float _lastCellMoistureValue = 0.0f;

    private int _currentMaterialIndex = -1;
    Timer timekeeper;
    public float StartTime;
    GameObject size = null;
    int sizen;
    public GameObject Smoke;
    public GameObject Smoke1;
    public bool f0 = false, f1 = false, f2 = false, f3 = false, f4 = false, f5 = false, f6=false,f7=false;
    GameObject WalkObject = null;
    #endregion Variables
    #region Monobahviour Methods
    void Start()
    {
        size = this.gameObject;
        var _size = size.GetComponentInChildren<WorldGenerator>();
        sizen = _size.sizeX;
        WalkObject = GameObject.Find("FPSController");
        Smoke.GetComponent<ParticleSystem>().Pause();
        Smoke1.GetComponent<ParticleSystem>().Pause();
    }
    void Update()
    {
        var _walkPath = WalkObject.GetComponent<WalkPathP>();
        if (_walkPath.StartRun == true)
        {
            
            if (_walkPath.str.Contains("Left"))
            {
                Smoke.GetComponent<ParticleSystem>().Play();
                if (f6 == false)
                        if (this.ToString().Equals("FireSpread (CellController)"))
                        {
                            if (search==false)
                            {
                             for (int i = 0; i < grid; i++)
                                for (int j = 0; j < grid; j++)
                                    BurnCells[i, j] = transform.Find("World/Cell(" + i.ToString() + "," + j.ToString() + ")").gameObject;
                             search = true;
                            }
                            this.ProcessCells(WalkObject);
                        }    
                timer += 0.05f;
            }
            else
            {
                Smoke.GetComponent<ParticleSystem>().Pause();
            }
            if (_walkPath.str.Contains("Right"))
             {
                Smoke1.GetComponent<ParticleSystem>().Play();
                if (f4 == false)
                    if (this.ToString().Equals("FireSpread1 (CellController)"))
                    {
                        if (search == false)
                        {
                            for (int i = 0; i < grid; i++)
                                for (int j = 0; j < grid; j++)
                                    BurnCells[i, j] = transform.Find("World/Cell(" + i.ToString() + "," + j.ToString() + ")").gameObject;
                            search = true;
                        }
                        this.ProcessCells(WalkObject);
                    }
                timer += 0.05f;
            }
            else
            {
                Smoke1.GetComponent<ParticleSystem>().Pause();
            }

        }
    }
    #endregion Monobahaviour Methods
    #region Methods
    public void ProcessCellSelection()
    {
        if (BurnCell == null)
        {
            string CellName = null;
            GameObject CellGrass = null;
            int x = -1, y = -1;         
            while (BurnCell == null)
            {
                x = Random.Range(1, sizen); 
                y = Random.Range(1, sizen); 
                CellName = "World/Cell(" + x.ToString() + "," + y.ToString() + ")";
                if (transform.Find(CellName + "/Grass(Clone)")||transform.Find(CellName + "/Grasss(Clone)"))
                    CellGrass = transform.Find(CellName).gameObject;
                else if (transform.Find(CellName + "/Rocks(Clone)"))
                    CellGrass = null;
                if (CellGrass != null)
                {
                    BurnCell = transform.Find("World/Cell(" + x.ToString() + "," + y.ToString() + ")").gameObject;
                    this.SelectedCell = BurnCell.GetComponent<Cell>();
                    this.SelectedCell.SetValues(new System.Random());
                }
            }
        }
    }
    public void ProcessCells(GameObject WalkObject)
    {
        var _walkPath = WalkObject.GetComponent<WalkPathP>();
        for (int i = 0; i < grid; i++)
            for (int j = 0; j < grid; j++)
            {
                if (_walkPath.FireArea[i, j] > 0 && BurnCells[i,j].GetComponent<Cell>().IsBurning==false)
                    BurnCells[i, j].GetComponent<Cell>().Ignite();
                else if (_walkPath.FireArea[i, j] ==0 && BurnCells[i, j].GetComponent<Cell>().IsBurning == true)
                    BurnCells[i, j].GetComponent<Cell>().PutOffFire();
            }
    }

    /*private void ProcesParams()
    {
        if (this._selectedCell != null)
        {          
            float massSliderValue = this._selectedCell.materialMass / WorldGenerator.cellMass_max;
            this._cellMassSlider.value = massSliderValue;
            SetMassLabel(this._selectedCell.materialMass);
        }
        this._lastSelectedCell = this._selectedCell;
    }*/

    /*private void SetMassLabel(float mass)
    {
        if (this._cellMassLabel != null)
        {
            this._cellMassLabel.text = (Mathf.Floor(mass * 10.0f) / 10.0f).ToString() + "kg";
        }
    }*/

    public void Ignite()
    {
        if (this.SelectedCell != null)
        {
            this.SelectedCell.Ignite();
        }
    }

    private void OnIndexChanged(int index)
    {
        if (this._selectedCell != null)
        {
           //this.SelectedCell.SetMaterial(WorldGenerator._instance.materialPrefabs[index]);
            this.SelectedCell.SetValues(new System.Random());
            this._currentMaterialIndex = index;
        }
    }

    /*public void OnMassValueChanged(float value)
    {
        if (this._selectedCell != null)
        {
            float materialMass = this._cellMassSlider.value * WorldGenerator.cellMass_max;
            this._selectedCell.materialMass = materialMass;
            SetMassLabel(this._selectedCell.materialMass);
        }
    }*/

    #endregion Methods
}
