using UnityEngine;
using System.Collections;

public class MeshBrush : MonoBehaviour
{

	//Actual editor script is inside the Assets/Editor folder...
    //Here I just define some public variables that I need for the inspector gui. ツ

    [HideInInspector] public bool isActive = true;

    [HideInInspector] public GameObject brush; //The brush object. Never delete this!
    [HideInInspector] public Transform holderObj; //The holder object's transform.
    [HideInInspector] public Transform brushTransform; //The brush object's transform (used for multiple mesh painting).

    [HideInInspector] public string groupName = "<group name>";

    public GameObject[] setOfMeshesToPaint = new GameObject[1]; //This is the array of meshes to paint.

    //KeyCode variables for the customizable shortcuts.
    [HideInInspector]
    public KeyCode paint = KeyCode.P;
    [HideInInspector]
    public KeyCode increaseRadius = KeyCode.O;
    [HideInInspector]
    public KeyCode decreaseRadius = KeyCode.I;

	[HideInInspector]public float hRadius = 0.3f; //The radius of the helper handle. 

	[HideInInspector]public Color hColor = Color.white; //Sets the helper handle color.

	[HideInInspector]public int meshCount = 1; //Number of meshes to paint.
    [HideInInspector]public float delay = 0.25f; //Delay between paint strokes if you hold down your paint button.
	[HideInInspector]public float meshOffset = 0.0f; //A float variable for the vertical offset of the mesh we are going to paint. You probably won't ever need this if you place the pivot of your meshes nicely, but you never know.
    [HideInInspector]public float slopeInfluence = 100.0f; //Float value for how much the painted meshes are kept upright or not when painted on top of surfaces.
    
    [HideInInspector]public bool activeSlopeFilter = false; //Activate/deactivate the slope filter.

    [HideInInspector]public float maxSlopeFilterAngle = 30f; //Float value for the slope filter (use this to avoid having meshes painted on slopes or hills).
    [HideInInspector]public bool inverseSlopeFilter = false; //Invert the slope filter functionality with ease.
    [HideInInspector]public bool manualRefVecSampling = false; //Manually sample the reference slope vector.
    [HideInInspector]public bool showRefVecInSceneGUI = true; //Show/hide the reference gui vector in the scene view.
    
    
    [HideInInspector]public Vector3 slopeRefVec = Vector3.up; //The sampled reference slope vector.
    [HideInInspector]public Vector3 slopeRefVec_HandleLocation = Vector3.zero; //The point in space where we sampled our reference slope vector..

    
    [HideInInspector]public bool yAxisIsTangent = false; //Determines if the local Y-Axis of painted meshes should be tangent to its underlying surface or not (if it's not, regular global Vector3.up is used and the meshes will be kept upright).
    [HideInInspector]public bool invertY = false; //Inverts the Y-axis of the painted meshes (useful for upside-down painting without slope influence).
    [HideInInspector]public float inset = 60f; //Percentage of scattering.
	[HideInInspector]public bool autoStatic = true;
	[HideInInspector]public bool uniformScale = true;
	[HideInInspector]public bool constUniformScale = true;
	[HideInInspector]public bool rWithinRange = false; //Within range toggle bool.

    [HideInInspector]public bool b_CustomKeys = false; //Boolean for the customize keyboard shortcuts foldout.
    [HideInInspector]public bool b_Slopes = true; //Boolean for the slopes foldout.
	[HideInInspector]public bool b_Randomizers = true; //Boolean value for the randomize foldout menu in the inspector.
	[HideInInspector]public bool b_AdditiveScale = true; //Boolean for the 'Apply additive scale' foldout menu.
    [HideInInspector]public bool b_opt = true; //Boolean for the 'Optimization' foldout menu.

	[HideInInspector]public float rScaleW = 0.0f; //Random and constant scale multipliers.
	[HideInInspector]public float rScaleH = 0.0f;
	[HideInInspector]public float rScale = 0.0f;
	[HideInInspector]public Vector2 rUniformRange = Vector2.zero; //Variables for our customized random ranges.....
	[HideInInspector]public Vector4 rNonUniformRange = Vector4.zero;

	[HideInInspector]public float cScale = 0.0f; //Float variable for the uniform additive scale.
	[HideInInspector]public Vector3 cScaleXYZ = Vector3.zero; //Vector3 variable for the non-uniform additive scale.

	[HideInInspector]public float rRot = 0.0f; //Random rotation float value.


    public void ResetSlopeSettings()
    {
        slopeInfluence = 100f;
        maxSlopeFilterAngle = 30f;
        activeSlopeFilter = false;
        inverseSlopeFilter = false; 
        manualRefVecSampling = false; 
        showRefVecInSceneGUI = true;
    }


	public void ResetRandomizers()
	{
		rScale = 0;
		rScaleW = 0;
		rScaleH = 0;
		rRot = 0;
		rUniformRange = Vector2.zero;
		rNonUniformRange = Vector4.zero;
	}
}

/*
 * 
 * Raphael Beck, 2014
 * 
 */