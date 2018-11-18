using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Threading;

public class Cell : MonoBehaviour 
{
	#region Variables
	public int			x;
	public int			y;

    public WorldGenerator _currentWorldGenerator = null;
    [Header("Cell Properties")]
	public CellMaterial materialType;
	[Range(0, 2000)]
	public float		currentTemperature;
	public float		materialMass;
	public float		waterMass;
	public float		cellSize;

	private bool		materialSet = false;
	// preset values
	[SerializeField]
	private ParticleSystem	fireParticles;

	private Color		temperatureColorA = new Color(0, 0, 1, 0.3f);
	private Color		temperatureColorB = new Color(1, 0.4f, 0.4f, 0.8f);
	private Color		temperatureColorC = new Color(0f, 0f, 0f, 0.5f);
	private Color		temperatureColorD = new Color(0f, 0f, 0f, 0.1f);

	[SerializeField]
	private MeshRenderer visualizerRenderer;
	private Material	visualizerMaterial;
	[SerializeField]
	private MeshRenderer selectRenderer;

	private GameObject _materialGO = null;

	//fire values;
	public  bool		_isBurning = false;
	public bool	    IsBurning { get { return this._isBurning; } }
	private float		_aquiredEnergy = 0.0f;
	private float		_storedEnergy = 0.0f;
	private float		initialMass;
    private string    CellName=null;
	private bool     _isSelected = false;
    GameObject WalkObject = null;
    public bool IsSelected
	{
		get
		{
			return this._isSelected;
		}
		set
		{
			this._isSelected = value;
			this.Selection(this._isSelected);
		}
	}

	#endregion

	#region Monobehaviour
	void Awake () 
	{
		this.IsSelected = false;
		//visualizerMaterial = visualizerRenderer.material;
		//selectRenderer.enabled = false;
	}

	void Start()
	{
        this.currentTemperature = this._currentWorldGenerator.globalTemperature;
        this._storedEnergy = 0.0f;
		this._aquiredEnergy = 0.0f;
        WalkObject = GameObject.Find("Walk");
    }

    void Update () 
	{
        //UpdateVisualizerColor();

        //this.ProcessCalculationSelf();
    }
	void LateUpdate()
	{
           /* this.ProcessCalculationOthers();
            if (this._aquiredEnergy > 0.0f)
            {
                this._storedEnergy += this._aquiredEnergy;
            }
            this._aquiredEnergy = 0.0f;*/
	}

	void UpdateVisualizerColor() 
	{
		if ( this._currentWorldGenerator.drawTemperatureGizmos&& materialSet && !materialType.isNonFlammable) 
		{
			if (!IsBurning)
				visualizerMaterial.color = Color.Lerp(temperatureColorA, temperatureColorB, Mathf.Clamp01(currentTemperature / materialType.ignitionTemperature));
			else if (materialMass > 0)
				visualizerMaterial.color = Color.Lerp(temperatureColorB, temperatureColorC, 1 - materialMass / initialMass);
			
			if (materialMass == 0)
				visualizerMaterial.color = temperatureColorD;
		}
		else {

		}
	}
	#endregion

	#region Methods
	public void Setup(int x, int y, float size) {
		this.x = x;
		this.y = y;
		cellSize = size;

		gameObject.name = "Cell(" + x + "," + y + ")";
	}

	public void SetMaterial(GameObject prefab) 
	{
		if(this._materialGO != null)
		{
			GameObject.Destroy(this._materialGO);
		}
		// spawn material prefab (visualization + values)
		this._materialGO = Instantiate(prefab);
		this._materialGO.transform.parent = transform;
		this._materialGO.transform.localPosition = new Vector3(0, 0.1f, 0);

		materialType = this._materialGO.GetComponent<CellMaterial>();
		materialSet = true;
	}

	// uses a seeded random instance for deterministic generation
	public void SetValues(System.Random rand) 
	{
		float tempMass = (rand.Next((int)WorldGenerator.cellMass_min, (int)WorldGenerator.cellMass_max) * 100) / 100f;
		materialMass = Mathf.Lerp(tempMass, (WorldGenerator.cellMass_min + WorldGenerator.cellMass_max) / 2, 1 - /*WorldGenerator.Instance.cellMassRandomization*/this._currentWorldGenerator.cellMassRandomization);
		initialMass = materialMass;
		this.waterMass = this.materialMass * this.materialType.moisture;
	}

	public void SetTemperature(float temp) {
		currentTemperature = temp;
	}

	public void Ignite() 
	{
        if (this.materialMass > 0.0f && !materialType.isNonFlammable && !this._isBurning)
		{
			this.waterMass = 0.0f;
			this._isBurning = true;
			currentTemperature = materialType.ignitionTemperature;
			this.fireParticles.Play();
            this.tag= "Fire";
		}
	}
	public void PutOffFire()
	{
		this._isBurning = false;
		this.fireParticles.Stop();
		materialType.SwitchMesh();
	}

	private void ProcessCalculationSelf()
	{
		float generatedEnergy = 0.0f;

        if (this._isBurning)
        {
            //how much of mass was burned
            float burnedMaterialMass = this.materialType.burnSpeed * Time.deltaTime * 0.5f;
            if (this.materialMass > burnedMaterialMass)
            {
                this.materialMass -= burnedMaterialMass;
            }
            else
            {
                burnedMaterialMass = this.materialMass;
                this.materialMass = 0.0f;
            }

            //energy is in GJ - giga jules
            //how much energy was generated in burning process                       K        M         G
            generatedEnergy += burnedMaterialMass * this.materialType.fuelEnergy * 1000.0f * 1000.0f * 10.0f;
        }
            generatedEnergy += this._storedEnergy;
            this._storedEnergy = 0.0f;
            int energyTransferRadius = WorldGenerator.energyTransferRadius;
            int myX = this.x;
            int myY = this.y;
            float myTemperature = this.currentTemperature;
            float myEmissivity = this.materialType.emissivity;
            float stefan_boltzman_coefficient = WorldGenerator.stefan_boltzman_coefficient;
            List<List<Cell>> tmpCells = this._currentWorldGenerator.Cells;
            int sizeX = this._currentWorldGenerator.sizeX;
            int sizeY = this._currentWorldGenerator.sizeY;
            int coefficientTableSize = energyTransferRadius * 2 + 1;
            float[][] positionCoefitients = new float[coefficientTableSize][];
            for (int i = 0; i < coefficientTableSize; ++i)
            {
                positionCoefitients[i] = new float[coefficientTableSize];
            }
            int distance = 0;
            //distribute energy basing on coeficients
            for (int indexY = -energyTransferRadius; indexY <= energyTransferRadius; ++indexY)
            {
                for (int indexX = -energyTransferRadius; indexX <= energyTransferRadius; ++indexX)
                {
                    distance = Mathf.Abs(indexX) + Mathf.Abs(indexY);
                    int positionX = myX + indexX;
                    int positionY = myY + indexY;
                    if (distance <= energyTransferRadius
                        &&(positionX >= 0 && positionX < sizeX)
                        &&(positionY >= 0 && positionY < sizeY))
                        tmpCells[positionY][positionX].AquireEnergy(Random.Range(500f, 5000f));
                }
            }


            //energy radiated into the oblivion

            float temperatureDiffrence = Mathf.Abs(myTemperature - this._currentWorldGenerator.globalTemperature);

            float oblivionEnergy = myEmissivity * stefan_boltzman_coefficient * Mathf.Pow(temperatureDiffrence, 3.0f) * 1.0f;

            if (generatedEnergy > oblivionEnergy)
            {
                generatedEnergy -= oblivionEnergy;
            }

            if (this.materialMass <= 0.0f)
            {
                generatedEnergy = 0.0f;
                this.currentTemperature = this._currentWorldGenerator.globalTemperature;
            }

            if (generatedEnergy > 0.0f)
            {
                this._aquiredEnergy += generatedEnergy;
            }

            if (this.materialMass <= 0.0f && IsBurning)
            {
                PutOffFire();
            }
	}


	private void ProcessCalculationOthers()
	{
		float massSpecificHeatCoefitient = (this.materialMass * this.materialType.specificHeat + this.waterMass * CellMaterial.specificHeat_water);
		float massOnlyHeatCoefficient = this.materialMass * this.materialType.specificHeat;

		if (this.currentTemperature < CellMaterial.vaporizationTemperature && this.materialMass > 0.0f)
		{
			//possible grow of temperature below vaporization temperature
			float deltaTemperature = this._aquiredEnergy / massSpecificHeatCoefitient;

			if (this.currentTemperature + deltaTemperature < CellMaterial.vaporizationTemperature)
			{
				this.currentTemperature += deltaTemperature;
				this._aquiredEnergy = 0.0f;
				return;
			}
			else
			{

				//get to vaporization temperature
				float missingTemperature = CellMaterial.vaporizationTemperature - this.currentTemperature;

				float energyRequiredToGetVaporizationTemperature = missingTemperature * massSpecificHeatCoefitient;

				this.currentTemperature = CellMaterial.vaporizationTemperature;

				this._aquiredEnergy -= energyRequiredToGetVaporizationTemperature;

			}
		}

		if (this.currentTemperature == CellMaterial.vaporizationTemperature && this.waterMass > 0.0f && this._aquiredEnergy > 0.0f)
		{
			//possible mass of vaporized water
			float deltaWaterMassVaporization = this._aquiredEnergy * CellMaterial.specificHeat_water;

			if (this.waterMass > deltaWaterMassVaporization)
			{
				//less energy to fully vaporize
				this.waterMass -= deltaWaterMassVaporization;
				this._aquiredEnergy = 0.0f;
				return;
			}
			else
			{
				//more energy to fully vaporize
				float energyNeededToVaporizeRemainingWater = this.waterMass * CellMaterial.specificHeat_water;
				this.waterMass = 0.0f;
				this._aquiredEnergy -= energyNeededToVaporizeRemainingWater;

			}
		}

		if (this.currentTemperature >= CellMaterial.vaporizationTemperature && this.waterMass == 0.0f && this._aquiredEnergy > 0.0f)
		{
			//possible grow in temperature
			float deltaTemperature = this._aquiredEnergy / massOnlyHeatCoefficient;

			if (this.currentTemperature < this.materialType.ignitionTemperature)
			{

				if (deltaTemperature + this.currentTemperature < this.materialType.ignitionTemperature)
				{
					//less energy than needed to ignite

					this.currentTemperature += deltaTemperature;
					this._aquiredEnergy = 0.0f;
					return;
				}
				else
				{

					//more energy than needed to ignite
					float deltaTemperatureMissingToIgnite = this.materialType.ignitionTemperature - this.currentTemperature;
					float energyToIgnite = deltaTemperatureMissingToIgnite * massOnlyHeatCoefficient;

					this.currentTemperature = this.materialType.ignitionTemperature;
					this._aquiredEnergy -= energyToIgnite;

				}
			}else{

				if(deltaTemperature + this.currentTemperature < CellMaterial.maxCellTemperature)
				{
					this.currentTemperature += deltaTemperature;
					this._aquiredEnergy = 0.0f;
				}else{

					float deltaTemperatureMissingToMax = CellMaterial.maxCellTemperature - this.currentTemperature;
					float energyToMaxTemperature = deltaTemperatureMissingToMax * massOnlyHeatCoefficient;

					this.currentTemperature = CellMaterial.maxCellTemperature;
					this._aquiredEnergy -= energyToMaxTemperature;
				}
				
			}
		}

		if(this.currentTemperature >= this.materialType.ignitionTemperature)
		{
			Ignite();
		}
	}

	public void AquireEnergy(float energy)
	{
		this._aquiredEnergy += energy;
	}

	public void PrintStatus()
	{
        //Debug.LogFormat("Pos: {0} {1} Temp: {2} Mass: {3} WaterMass: {4}",this.x,this.y,currentTemperature,this.materialMass,this.waterMass);
    }

	public void Selection(bool selected) 
	{
		//selectRenderer.enabled = selected;
	}
	#endregion
}
