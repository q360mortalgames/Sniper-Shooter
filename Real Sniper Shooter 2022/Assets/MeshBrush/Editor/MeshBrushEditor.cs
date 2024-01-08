using UnityEngine;
using UnityEditor;
using System.Collections;
[ExecuteInEditMode]
[CustomEditor(typeof(MeshBrush))] //Check out the docs for extending the editor in Unity. You can do lots of awesome stuff with this!
public class MeshBrushEditor : Editor
{
	//Let's declare the variables we are going to need in this script...
	private MeshBrush _mp; //This is the super private MeshPaint instance. It's the original script object we are editing(overriding) with this custom inspector script here.

    private bool help = false; //"Help" foldout boolean. I define it in here instead of in MeshPaint.cs because I want it to automatically close whenever the user deselects the GameObject, since it kinda takes away a lot of space in the inspector view.
    private bool canPaint = true; //This boolean is used alongside the delay variable whenever the user keeps the paint button pressed.

    private delegate void ActiveBrushMode(); //MeshBrush currently has 2 brush modes: Meshpaint mode and Vector sampling mode. I use a delegate to swiftly switch between these modes and keep the code clean at the same time (and of course keeping a door open for eventual future additional modes... who knows? ;)
    private ActiveBrushMode activeBrushMode;

    private enum BrushMode { //We still need an enumeration for the above mentioned modes though...
        MeshPaint, 
        Sample
    }
    private BrushMode brushMode = BrushMode.MeshPaint;

    private Collider thisCollider; //This gameobject's collider.
    private Transform thisTransform; //This gameobject's transform.
    private Transform paintedMeshTransform; //The painted mesh's transform.

    private GameObject paintedMesh; //Variable for the instantiated meshes.

    private int randomValueInArray;  //This is a variable that I use to generate a random index value of the various arrays I have in the multiple meshpaint function.

    private double Time2Die; //Insider joke, no one can understand except for you bro, if you ever read this ;)
    private double _t; //Time storage variable. 

    private KeyCode _paintKey; //Three variables for the customizable keyboard shortcuts.
    private KeyCode _incRadius;
    private KeyCode _decRadius;

    //The tooltips that show up in the inspector when we hover our mouse over certain GUI elements.
    private GUIContent toolTipColor, toolTipRadius, toolTipFreq, toolTipOffset, toolTipSlopeInfluence, toolTipSlopeFilter, toolTipInvSlope, toolTipManualRefVecS, toolTipRefVecSample, toolTipTangentY, toolTipInvertY, toolTipInset, toolTipNR, toolTipUniformly, toolTipUniformlyRange, //Tooltips for the various gui elements.
        toolTipWithinRange, toolTipRot, toolTipV4, toolTipReset, toolTipAddScale, toolTipFlagS, toolTipCombine, toolTipDelete;

    //And here comes the raycasting part...
    private Ray scRay; //This is the screen ray that shoots out of the scene view's camera when we press the paint button...
    private RaycastHit scHit; //And this is its raycasthit.
    private RaycastHit brHit; //This is the brush hit; it's used for multiple mesh painting.

    private float insetThreshold; //Threshold value used to calculate a random position inside the brush's area for each painted mesh (based on the amount of scattering defined by the user).

    private float slopeAngle = 0f; //The slope angle variable that gets recalculated for each time we paint. This is used by the slope filter.

	#region OnLoadScript(Awake)

    private void Awake() {

        _mp = (MeshBrush)target; //Reference to the script we are overriding.

        activeBrushMode = BrushMode_MeshPaint; //Initialize the brushmode delegate. This is EXTREMELY important, since the call of a null delegate function could break everything spitting out horrible errors, if not even crash the program.

        thisTransform = _mp.transform;
        thisCollider = _mp.GetComponent<Collider>();

        if (_mp.GetComponentInChildren<MeshBrushParent>() != null) //This sets up a holder object for our painted meshes in case we don't already have one yet.
        {
            _mp.holderObj = _mp.GetComponentInChildren<MeshBrushParent>().transform;
        }
        else
        {
            GameObject parentObj = new GameObject("Painted meshes");
            parentObj.AddComponent<MeshBrushParent>();
            parentObj.transform.rotation = thisTransform.rotation;
            parentObj.transform.parent = thisTransform;
            parentObj.transform.localPosition = new Vector3(0, 0, 0);

            _mp.holderObj = parentObj.transform;
        }

        if (_mp.holderObj.Find("Brush") != null) //Create a brush object if we don't have one already. This is needed for multiple mesh painting.
        {
            _mp.brush = _mp.holderObj.Find("Brush").gameObject;
            _mp.brushTransform = _mp.brush.transform;
        }
        else
        {
            _mp.brush = new GameObject("Brush");
            _mp.brushTransform = _mp.brush.transform; //Initialize the brush's transform variable.
            _mp.brushTransform.position = thisTransform.position;
            _mp.brushTransform.parent = _mp.holderObj;
        }

        //The GUI elements of the custom inspector, with their corresponding tooltips:

        toolTipColor = new GUIContent("Color:","Color of the circle brush.");

		toolTipRadius = new GUIContent("Radius [m]:","Radius of the circle brush.");

        toolTipFreq = new GUIContent("Delay [s]:", "If you press and hold down the paint button, this value will define the delay (in seconds) between paint strokes; thus, the higher you set this value, the slower you'll be painting meshes.");

		toolTipOffset = new GUIContent("Offset amount [cm]:","Offsets all the painted meshes away from their underlying surface.\n\nThis is useful if your meshes are stuck inside your GameObject's geometry, or floating above it.\nGenerally, if you place your pivot points carefully, you won't need this.");

        toolTipSlopeInfluence = new GUIContent("Slope influence [%]:", "Defines how much influence slopes have on the rotation of the painted meshes.\n\nA value of 100% for example would adapt the painted meshes to be perfectly perpendicular to the surface beneath them, whereas a value of 0% would keep them upright at all times.");

        toolTipSlopeFilter = new GUIContent("Slope filter max. angle [�]:", "Avoids the placement of meshes on slopes and hills whose angles exceed this value.\nA low value of 20� for example would restrain the painting of meshes onto very flat surfaces, while the maximum value of 180� would deactivate the slope filter completely.");

        toolTipInvSlope = new GUIContent("Inverse slope filter", "Inverts the slope filter's functionality; low values of the filter would therefore focus the placement of meshes onto slopes instead of avoiding it.");

        toolTipManualRefVecS = new GUIContent("Manual reference vector sampling", "You can choose to manually sample a reference slope vector, whose direction will then be used by the slope filter instead of the world's Y-Up axis, to further help you paint meshes with the slope filter applied onto arbitrary geometry (like for instance painting onto huge round planet-meshes, concave topologies like caves etc...).\n\nTo sample one, enter the reference vector sampling mode by clicking the 'Sample reference vector' button below.");

        toolTipRefVecSample = new GUIContent("Sample reference vector", "Activates the reference vector sampling mode, which allows you to pick a normal vector of your mesh to use as a reference by the slope filter.\n\nPress "+_mp.paint+" to sample a vector.\nPress Esc to cancel the sampling and return to the regular mesh painting mode.\n(Deselecting and reselecting this object will also exit the sampling mode)");

        toolTipTangentY = new GUIContent("Y-Axis tangent to surface", "As you decrease the slope influence value, you can choose whether you want your painted meshes to be kept upright along the Y-Axis, or tangent to their underlying surface.");

        toolTipInvertY = new GUIContent("Invert Y-Axis","Flips all painted meshes on their Y-Axis.\nUseful if you are painting upside down and still want your painted meshes to be kept upright (but pointing downwards) on sloped ceiling surfaces for instance.");

        toolTipInset = new GUIContent("Scattering [%]:","Percentage of how much the meshes are scattered away from the center of the circle brush.\n\n(Default is 60%)");
        
        toolTipNR = new GUIContent("Nr. of meshes:","Maximum number of meshes you are going to paint inside the circle brush area at once.");

		toolTipUniformly = new GUIContent("Scale uniformly","Applies the scale uniformly along all three XYZ axes.");

		toolTipUniformlyRange = new GUIContent("Scale within this random range [Min/Max(XYZ)]:","Randomly scales the painted meshes between these two minimum/maximum scale values.\n\nX stands for the minimum scale and Y for the maximum scale applied.");

		toolTipWithinRange = new GUIContent("Scale within range","Randomly scales the meshes based on custom defined random range parameters.");

		toolTipRot = new GUIContent("Random Y rotation amount [%]:","Applies a random rotation around the local Y-axis of the painted meshes.");

		toolTipV4 = new GUIContent("[Min/Max Width (X/Y); Min/Max Height (Z/W)]","Randomly scales meshes based on custom defined random ranges.\n\nThe X and Y values stand for the minimum and maximum width (it picks a random value between them); " +
			"the Z and W values are for the minimum and maximum height.");

		toolTipReset = new GUIContent("Reset all randomizers","Resets all the randomize parameters back to zero.");

		toolTipAddScale = new GUIContent("Apply additive scale","Applies a constant, fixed amount of 'additive' scale after the meshes have been placed.");

		toolTipFlagS = new GUIContent("Flag all painted\nmeshes as static","Flags all the meshes you've painted so far as static in the editor.\nCheck out the Unity documentation about drawcall batching if you don't know what this is good for.");

        toolTipCombine = new GUIContent("Combine painted meshes", "Once you're done painting meshes, you can click here to combine them. This will combine all the meshes you've painted into one single mesh (one per material).\n\nVery useful for performance optimization.\nCannot be undone.");

		toolTipDelete = new GUIContent("Delete all painted meshes","Are you sure? This will delete all the meshes you've painted onto this GameObject's surface so far (except already combined meshes).");

        
    }
	#endregion

	#region MenuItem function
    [MenuItem("GameObject/Paint meshes on selected GameObject")] //Define a custom menu entry in the Unity toolbar above (this way we don't have to go through the add component menu every time).
    public static void AddMeshPaint() //This function gets called every time we click the above defined menu entry (since it is being defined exactly below the [MenuItem()] statement).
    {
		if (Selection.activeGameObject != null) //Check if there is a GameObject selected.
        {
			if(Selection.activeGameObject.GetComponent<Collider>() != null) //Check if the selected GameObject has a collider on it (without it, where would we paint our meshes on?) :-|
                Selection.activeGameObject.AddComponent<MeshBrush>();
			else {
				if(EditorUtility.DisplayDialog("GameObject has no collider component","The GameObject on which you want to paint meshes doesn't have a collider...\nOn top of what did you expect to paint meshes? :)\n\n" +
					"Do you want me to put a collider on it for you (it'll be a mesh collider)?","Yes please!","No thanks")){
					Selection.activeGameObject.AddComponent<MeshCollider>();
                    Selection.activeGameObject.AddComponent<MeshBrush>();
				}
				else return;
			}
        }
		else EditorUtility.DisplayDialog("No GameObject selected","No GameObject selected man... that's not cool bro D: what did you expect? To paint your meshes onto nothingness? :DDDDD","Uhmmm...");
    }
	#endregion

	#region OnInspectorGUI
    public override void OnInspectorGUI() //Works like OnGUI, except that it updates only the inspector view.
    {
        EditorGUILayout.Space();

        _mp.groupName = EditorGUILayout.TextField(_mp.groupName); //Useful textfield to name and organize your groups.
        help = EditorGUILayout.Foldout(help, "Help"); //Foldout menu for the help section, see below for further information

        //MAIN TOGGLE (this one can entirely turn on and off the meshbrush)
        EditorGUILayout.BeginHorizontal();
        _mp.isActive = EditorGUILayout.Toggle(_mp.isActive, GUILayout.Width(15f));
        EditorGUILayout.LabelField("Enabled", GUILayout.Width(150f), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();

        //The help foldout menu in the inspector.
        if (help)
        {
            EditorGUILayout.HelpBox("Paint meshes onto your GameObject's surface.\n_______\n\nKeyBoard shortcuts:\n\nPaint meshes: press or hold " + _mp.paint + "\nIncrease radius: press or hold " + _mp.increaseRadius + "\nDecrease radius: press or hold " + _mp.decreaseRadius + "\n_______\n\n" +
                "Assign one or more prefab objects to the 'Set of meshes to paint' array below and press 'P' while hovering your mouse above your GameObject to start painting meshes." +
                "\n\nMake sure that the local Y-axis of each prefab mesh is the one pointing away from the surface on which you are painting (to avoid weird rotation errors).\n\n" +
                "Check the documentation text file that comes with MeshBrush (or the YouTube tutorials) to find out more about the individual parameters (but most of them should be quite self explainatory, or supplied with a tooltip text label after hovering your mouse over them for a couple of seconds though).\n_______\n\n" +
                "You can press 'Flag/Unflag all painted meshes as static' to mark/unmark as static all the meshes you've painted so far.\nFlagging painted meshes as static will improve performance overhead thanks to Unity's built-in static batching functionality, " +
                "as long as the meshes obviously don't move (and as long as they share the same material).\nSo don't flag meshes as static if you have fancy looking animations on your prefab meshes (like, for instance, swaying animations for vegetation or similar properties that make the mesh move, rotate or scale in any way).\n_______\n\n" +
                "Once you're done painting, you can also combine your meshes with the 'Combine painted meshes button'. Check out the documentation for further information.\n\n" +
                "If you are painting grass or other kinds of small vegetation, I recommend using the '2-Sided Vegetation' shader that comes with the MeshBrush package. It's the " +
                "built-in Unity transparent cutout diffuse shader, just without backface culling, so that you get 2-sided materials.\nYou can obviously also use your own custom shaders if you want.\n_______\n\nFeel free to add multiple MeshBrush script instances to one GameObject for multiple mesh painting sets, with defineable group names and parameters for each of them;\n" +
                "MeshBrush will then randomly cycle through all of your MeshBrush instances and paint your meshes within the corresponding circle brush based on the corresponding parameters for that set.", MessageType.None);
        }
        EditorGUILayout.Space();//This leaves a little space between inspector properties... It makes the custom inspector a little lighter and less painful to look at. Believe me: you're gonna need this extra-air to breathe inside the inspector!

        DrawDefaultInspector(); //We need the default inspector being displayed in order to show the array of meshes to paint in the MeshBrush's inspector.
        //EditorGUILayout.Space();

        GUI.enabled = _mp.isActive;

        //The entire next block of code is dedicated to the inspector's GUI organization.

        EditorGUILayout.BeginHorizontal(); //Editor version of the GUILayout.BeginHorizontal().
        _mp.autoStatic = EditorGUILayout.Toggle(_mp.autoStatic, GUILayout.Width(15f)); 
        EditorGUILayout.LabelField("Automatically flag meshes as static", GUILayout.Width(210f), GUILayout.ExpandWidth(false));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        _mp.b_CustomKeys = EditorGUILayout.Foldout(_mp.b_CustomKeys, "Customize Keyboard Shortcuts");
        if (_mp.b_CustomKeys)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Paint");
            _mp.paint = (KeyCode)EditorGUILayout.EnumPopup(_mp.paint);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Increase radius");
            _mp.increaseRadius = (KeyCode)EditorGUILayout.EnumPopup(_mp.increaseRadius);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Decrease radius");
            _mp.decreaseRadius = (KeyCode)EditorGUILayout.EnumPopup(_mp.decreaseRadius);
            EditorGUILayout.EndHorizontal();
            //EditorGUILayout.Space();

            if (GUILayout.Button("Reset to default keys"))
            {
                _mp.paint = KeyCode.P;
                _mp.increaseRadius = KeyCode.O;
                _mp.decreaseRadius = KeyCode.I;
            }
            EditorGUILayout.Space();
        } EditorGUILayout.Space();

        _mp.paint = (_mp.paint == KeyCode.None) ? KeyCode.P : _mp.paint; //(Avoid having unassigned keys in MeshBrush; reset to the default value in case the user tries to set the button to "None")

        _mp.increaseRadius = (_mp.increaseRadius == KeyCode.None) ? KeyCode.O : _mp.increaseRadius;
        _mp.decreaseRadius = (_mp.decreaseRadius == KeyCode.None) ? KeyCode.I : _mp.decreaseRadius;

        _mp.hColor = EditorGUILayout.ColorField(toolTipColor, _mp.hColor); //Color picker for our circle brush.

        EditorGUILayout.BeginHorizontal();
        _mp.hRadius = EditorGUILayout.FloatField(toolTipRadius, _mp.hRadius, GUILayout.Width(175f), GUILayout.ExpandWidth(true));
        _mp.meshCount = EditorGUILayout.IntField(toolTipNR, Mathf.Clamp(_mp.meshCount, 1, 100), GUILayout.Width(175f), GUILayout.ExpandWidth(true)); //Clamp the meshcount so it never goes below 1 or above 100.
        EditorGUILayout.EndHorizontal();

        if (_mp.hRadius < 0.01f) _mp.hRadius = 0.01f; //Avoid negative or null radii.
        EditorGUILayout.Space();

        _mp.delay = EditorGUILayout.Slider(toolTipFreq, _mp.delay, 0.05f, 1.0f); //Slider for the delay between paint strokes.
        EditorGUILayout.Space(); EditorGUILayout.Space();

        EditorGUILayout.LabelField(toolTipOffset);
        _mp.meshOffset = EditorGUILayout.Slider(_mp.meshOffset, -50.0f, 50.0f); //Slider for the offset amount.

        EditorGUILayout.Space();

        if (_mp.meshCount <= 1)
            GUI.enabled = false;
        EditorGUILayout.LabelField(toolTipInset);
        _mp.inset = EditorGUILayout.Slider(_mp.inset, 0, 100.0f); //Slider for the scattering.
        EditorGUILayout.Space();

        GUI.enabled = _mp.isActive;

        EditorGUILayout.BeginHorizontal();
        {
            _mp.yAxisIsTangent = EditorGUILayout.Toggle(_mp.yAxisIsTangent, GUILayout.Width(15f)); 
            EditorGUILayout.LabelField(toolTipTangentY, GUILayout.Width(150f));
            _mp.invertY = EditorGUILayout.Toggle(_mp.invertY, GUILayout.Width(15f));
            EditorGUILayout.LabelField(toolTipInvertY, GUILayout.Width(150f));
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();

        _mp.b_Slopes = EditorGUILayout.Foldout(_mp.b_Slopes, "Slopes");
        if (_mp.b_Slopes)
        {
            EditorGUILayout.LabelField(toolTipSlopeInfluence);
            _mp.slopeInfluence = EditorGUILayout.Slider(_mp.slopeInfluence, 0f, 100f); //Slider for slope influence.
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            {
                _mp.activeSlopeFilter = EditorGUILayout.Toggle(_mp.activeSlopeFilter, GUILayout.Width(15f));
                EditorGUILayout.LabelField("Use slope filter");
            }
            EditorGUILayout.EndHorizontal();
            //EditorGUILayout.Space();

            if (_mp.activeSlopeFilter == false)
                GUI.enabled = false;

            EditorGUILayout.LabelField(toolTipSlopeFilter);
            _mp.maxSlopeFilterAngle = EditorGUILayout.Slider(_mp.maxSlopeFilterAngle, 1f, 180f);


            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            {
                _mp.inverseSlopeFilter = EditorGUILayout.Toggle(_mp.inverseSlopeFilter, GUILayout.Width(15f));
                EditorGUILayout.LabelField(toolTipInvSlope, GUILayout.Width(120f));
                _mp.manualRefVecSampling = EditorGUILayout.Toggle(_mp.manualRefVecSampling, GUILayout.Width(15f));
                EditorGUILayout.LabelField(toolTipManualRefVecS, GUILayout.Width(200f));
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (_mp.manualRefVecSampling == false)
                GUI.enabled = false;

            EditorGUILayout.BeginHorizontal();
            {
                _mp.showRefVecInSceneGUI = EditorGUILayout.Toggle(_mp.showRefVecInSceneGUI, GUILayout.Width(15f));
                EditorGUILayout.LabelField("Show sampled vector", GUILayout.Width(130f));

                if (GUILayout.Button(toolTipRefVecSample, GUILayout.Height(27f), GUILayout.Width(150f), GUILayout.ExpandWidth(true)))
                    brushMode = BrushMode.Sample;
            }
            EditorGUILayout.EndHorizontal();

            GUI.enabled = _mp.isActive;
            EditorGUILayout.Space();

            if (GUILayout.Button("Reset all slope settings", GUILayout.Height(27f), GUILayout.Width(150f), GUILayout.ExpandWidth(true)))
                _mp.ResetSlopeSettings();
        }
        //EditorGUILayout.Space();
        
        _mp.b_Randomizers = EditorGUILayout.Foldout(_mp.b_Randomizers, "Randomize"); //This makes the little awesome arrow for the foldout menu in the inspector view appear...

        if (_mp.b_Randomizers) //...and this below here makes it actually fold stuff in and out (the menu is closed if the arrow points to the right and thus rScale is false).
        {
            EditorGUILayout.BeginHorizontal();
            _mp.uniformScale = EditorGUILayout.Toggle("", _mp.uniformScale, GUILayout.Width(15f));
            EditorGUILayout.LabelField(toolTipUniformly, GUILayout.Width(100f));
            _mp.rWithinRange = EditorGUILayout.Toggle("", _mp.rWithinRange, GUILayout.Width(15f));
            EditorGUILayout.LabelField(toolTipWithinRange);
            EditorGUILayout.EndHorizontal();

            if (_mp.uniformScale == true)
            {
                if (_mp.rWithinRange == false)
                    _mp.rScale = EditorGUILayout.Slider("Random scale:", _mp.rScale, 0, 5f);
                else
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField(toolTipUniformlyRange);
                    _mp.rUniformRange = EditorGUILayout.Vector2Field("", _mp.rUniformRange);
                }
            }
            else
            {
                if (_mp.rWithinRange == false)
                {
                    EditorGUILayout.Space();

                    _mp.rScaleW = EditorGUILayout.Slider("Random width (X/Z)", _mp.rScaleW, 0, 3f);
                    _mp.rScaleH = EditorGUILayout.Slider("Random height (Y)", _mp.rScaleH, 0, 3f);
                }
                else
                {
                    EditorGUILayout.Space();

                    EditorGUILayout.LabelField("Randomly scale within these ranges:");
                    EditorGUILayout.LabelField(toolTipV4);
                    _mp.rNonUniformRange = EditorGUILayout.Vector4Field("", _mp.rNonUniformRange);
                    EditorGUILayout.Space();
                }
            }
            EditorGUILayout.Space();

            EditorGUILayout.LabelField(toolTipRot);
            _mp.rRot = EditorGUILayout.Slider(_mp.rRot, 0.0f, 100.0f); //Create the slider for the percentage of random rotation around the Y axis applied to our painted meshes.
            EditorGUILayout.Space();

            if (GUILayout.Button(toolTipReset, GUILayout.Height(27f), GUILayout.Width(150f), GUILayout.ExpandWidth(true)))
                _mp.ResetRandomizers();
        }

        _mp.b_AdditiveScale = EditorGUILayout.Foldout(_mp.b_AdditiveScale, toolTipAddScale); //Foldout for the additive scale.
        //EditorGUILayout.Space();

        if (_mp.b_AdditiveScale)
        {
            _mp.constUniformScale = EditorGUILayout.Toggle(toolTipUniformly, _mp.constUniformScale);
            if (_mp.constUniformScale == true)
                _mp.cScale = EditorGUILayout.FloatField("Add to scale", _mp.cScale);
            else {
                _mp.cScaleXYZ = EditorGUILayout.Vector3Field("Add to scale", _mp.cScaleXYZ);
            }
            if (_mp.cScale < -0.9f) _mp.cScale = -0.9f;
            if (_mp.cScaleXYZ.x < -0.9f) _mp.cScaleXYZ.x = -0.9f;
            if (_mp.cScaleXYZ.y < -0.9f) _mp.cScaleXYZ.y = -0.9f;
            if (_mp.cScaleXYZ.z < -0.9f) _mp.cScaleXYZ.z = -0.9f;
            EditorGUILayout.Space();

            if (GUILayout.Button("Reset additive scale", GUILayout.Height(27f), GUILayout.Width(150f), GUILayout.ExpandWidth(true)))
            {
                _mp.cScale = 0;
                _mp.cScaleXYZ = Vector3.zero;
            }
            EditorGUILayout.Space();
        }

        _mp.b_opt = EditorGUILayout.Foldout(_mp.b_opt, "Optimize");
        if (_mp.b_opt)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(toolTipFlagS, GUILayout.Height(50f), GUILayout.Width(150f), GUILayout.ExpandWidth(true)) && _mp.GetComponentInChildren<MeshBrushParent>() == true) //Create 2 buttons for quickly flagging/unflagging all painted meshes as static...
                _mp.GetComponentInChildren<MeshBrushParent>().FlagMeshesAsStatic();
            if (GUILayout.Button("Unflag all painted\nmeshes as static", GUILayout.Height(50f), GUILayout.Width(150f), GUILayout.ExpandWidth(true)) && _mp.GetComponentInChildren<MeshBrushParent>() == true)
                _mp.GetComponentInChildren<MeshBrushParent>().UnflagMeshesAsStatic();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(toolTipCombine, GUILayout.Height(50f), GUILayout.Width(150f), GUILayout.ExpandWidth(true))) //... one for combining them...
            {
                if (_mp.GetComponentInChildren<MeshBrushParent>() != null)
                {
                    _mp.GetComponentInChildren<MeshBrushParent>().CombinePaintedMeshes();
                    DestroyImmediate(_mp.GetComponentInChildren(typeof(MeshBrushParent)));
                }
            }

            if (GUILayout.Button(toolTipDelete, GUILayout.Height(50f), GUILayout.Width(150f), GUILayout.ExpandWidth(true)) && _mp.GetComponentInChildren<MeshBrushParent>() == true) //...and one to delete all the meshes we painted on this GameObject so far.
                Undo.DestroyObjectImmediate(_mp.GetComponentInChildren(typeof(MeshBrushParent)).gameObject);
            EditorGUILayout.EndHorizontal(); //EditorGUILayout.Space();
        }

        EditorGUILayout.Space();

        if (GUI.changed) //Repaint the scene view whenever the inspector's gui is changed in some way. 
            SceneView.RepaintAll(); //This avoids weird disturbing snaps of the reference slope vector and the circle brush gui handle inside the scene view when we return to it after changing some settings in the inspector.
    }
	#endregion

	#region Scene GUI
	void OnSceneGUI() //http://docs.unity3d.com/Documentation/ScriptReference/Editor.OnSceneGUI.html
	{
        if (_mp.isActive) { //Only enable the meshbrush when the user sets the specific instance to enabled (through the toggle in the inspector).

            Handles.color = _mp.hColor;

            Time2Die = EditorApplication.timeSinceStartup;

            canPaint = (_t > Time2Die) ? false : true;

            activeBrushMode(); //Call the delegate method.

            switch (brushMode) //Assign the various brushmode methods to the delegate based on the current value of the brushmode enum. This is very comfortable, because now I can just change the enum's value to swap the brushmodes with ease.
            {
                case BrushMode.MeshPaint:
                    activeBrushMode = BrushMode_MeshPaint;
                    break;
                case BrushMode.Sample:
                    activeBrushMode = BrushMode_SampleReferenceVector;
                    break;

                default:
                    activeBrushMode = BrushMode_MeshPaint;
                    break;
            }

            switch (Event.current.type) //Increase/decrease the radius with the keyboard buttons I and O
            {
                case EventType.KeyDown:
                    if (Event.current.keyCode == _mp.increaseRadius)
                        _mp.hRadius += 0.05f;
                    else if (Event.current.keyCode == _mp.decreaseRadius && _mp.hRadius > 0)
                        _mp.hRadius -= 0.05f;
                    break;
            }

            //Draw the custom sampled reference slope vector in the scene view (given that the user wants it to appear and he is actually using the slope filter at all)...
            if (_mp.showRefVecInSceneGUI == true && _mp.manualRefVecSampling == true && _mp.activeSlopeFilter == true)
                Handles.ArrowCap(0, _mp.slopeRefVec_HandleLocation, Quaternion.LookRotation(_mp.slopeRefVec), 0.9f);
        }
    }
    #endregion
    
    #region Brush mode functions
    void BrushMode_MeshPaint() //This method represents the MeshPaint mode for the brush. This is the default brushmode.
    {
        if (Selection.gameObjects.Length == 1 && Selection.activeGameObject.transform == thisTransform) //Only cast rays if we have our object selected (for the sake of performance).
        {
            scRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition); //Shoot the ray through the 2D mouse position on the scene view window.

            if (thisCollider.Raycast(scRay, out scHit, Mathf.Infinity))
            {
                SceneView.RepaintAll(); //Constantly update scene view at this point (to avoid the circle handle jumping around as we click in and out of the scene view).
                Handles.DrawWireDisc(scHit.point, scHit.normal, _mp.hRadius); //Thanks to the RepaintAll() function above, the circle handle that we draw here gets updated at all times inside our scene view.
                if (canPaint) //If a paint stroke is possible (depends on the delay defined in the inspector), call the paint function when the user presses the paint button. 
                {
                    if (Event.current.type == EventType.KeyDown && Event.current.keyCode == _mp.paint)
                    {
                        _t = Time2Die + _mp.delay;
                        Paint();
                    }
                }
            }
        }
    }

    void BrushMode_SampleReferenceVector() //This one represents the vector sampling mode. This brushmode allows the user to sample a custom defined slope reference vector used by the slope filter. Check out the tutorial to find out what this does in case you're confused.
    {
        if (Selection.gameObjects.Length == 1 && Selection.activeGameObject.transform == thisTransform) 
        {
            scRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition); 

            if (thisCollider.Raycast(scRay, out scHit, Mathf.Infinity))
            {
                SceneView.RepaintAll();
                Handles.ArrowCap(0, scHit.point, Quaternion.LookRotation(scHit.normal), 0.9f); //Draw a GUI handle arrow to represent the vector to sample. 

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == _mp.paint) //Sample the reference vector for the slope filter and store it in the slopeRefVec variable.
                {
                    _mp.slopeRefVec = scHit.normal.normalized;
                    _mp.slopeRefVec_HandleLocation = scHit.point;
                    brushMode = BrushMode.MeshPaint; //Jump back to the meshpaint mode automatically.
                }

                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape) //Here I give the user the possibility to cancel sampling mode by pressing the escape button.
                    brushMode = BrushMode.MeshPaint;
            }
        }
    }
    #endregion

    void Paint() //The actual paint function.
    {
        if (_mp.setOfMeshesToPaint.Length == 0) //Display an error dialog box if we are trying to paint 'nothing' onto our GameObject.
        {
            EditorUtility.DisplayDialog("No meshes to paint...", "Please add at least one prefab mesh to the array of meshes to paint.", "Okay");
            return;
        }
        else
        {
            for (int i = _mp.setOfMeshesToPaint.Length - 1 ; i >= 0 ; i--) //Check if every single field of the array has a GameObject assigned (this is necessary to avoid a disturbing error printed in the console).
            {
                if (_mp.setOfMeshesToPaint[i] == null)
                {
                    EditorUtility.DisplayDialog("Warning!", "One or more fields in the array of meshes to paint is empty. Please assign something to all fields before painting.", "Okay");
                    return;
                }
            }

            if (_mp.GetComponentInChildren<MeshBrushParent>() == false)      // Check if we already have an empty Mesh "holder" GameObject in our GameObject's hierarchy; in case we don't, create one!
            {
                GameObject createdHolder = new GameObject("Painted meshes"); // This creates and sets up the above mentioned holder object.
                createdHolder.AddComponent<MeshBrushParent>();
                createdHolder.transform.rotation = thisTransform.rotation;
                createdHolder.transform.parent = thisTransform;
                createdHolder.transform.localPosition = new Vector3(0, 0, 0);

                _mp.holderObj = createdHolder.transform;
            }

            //  Call the correct meshpaint method. This is where the actual paint-stuff happens :)
            if (_mp.meshCount == 1)
                PaintSingleMesh();
            else if (_mp.meshCount > 1)
                PaintMultipleMeshes();
        }
    }

    #region Single Meshpaint
    void PaintSingleMesh() //  Single meshpaint function.
    {
        randomValueInArray = Random.Range(0, _mp.setOfMeshesToPaint.Length);    // Calculate a random value inside the array of meshes to paint.

        //  Calculate the angle between the world's upvector (or a manually sampled reference vector) and the normal vector of our hit.
        slopeAngle = _mp.activeSlopeFilter ? Vector3.Angle(scHit.normal, _mp.manualRefVecSampling ? _mp.slopeRefVec : Vector3.up) : _mp.inverseSlopeFilter ? 180f : 0f;

        //  Here I'm applying the slope filter based on the angle value obtained above...
        if ((_mp.inverseSlopeFilter == true) ? (slopeAngle > _mp.maxSlopeFilterAngle) : (slopeAngle < _mp.maxSlopeFilterAngle))
        {
            paintedMesh = Instantiate(_mp.setOfMeshesToPaint[randomValueInArray], scHit.point, Quaternion.LookRotation(scHit.normal)) as GameObject; //This is the creation of the mesh. Here it gets instantiated, placed and rotated correctly at the location of our brush's center.

            paintedMeshTransform = paintedMesh.transform;

            //  Align the painted mesh's up vector to the corresponding direction (defined by the user).
            if (_mp.yAxisIsTangent)
                paintedMeshTransform.up = Vector3.Lerp(paintedMeshTransform.up, paintedMeshTransform.forward, _mp.slopeInfluence * 0.01f);
            else
                paintedMeshTransform.up = Vector3.Lerp(Vector3.up, paintedMeshTransform.forward, _mp.slopeInfluence * 0.01f);

            paintedMeshTransform.parent = _mp.holderObj;        //  Set the instantiated object as a parent of the "Painted meshes" holder gameobject.
            paintedMesh.name = _mp.setOfMeshesToPaint[0].name;  //  Set the painted object's name to the one of the asset you assigned as the mesh to paint. 

            if (_mp.autoStatic) paintedMesh.isStatic = true;    //  Automatically flag the painted mesh as static, if the user wishes to do so.

            //  The various states of the toggles:
            if (!_mp.rWithinRange && !_mp.uniformScale)
            {
                if (_mp.rScaleW > 0 || _mp.rScaleH > 0)
                    ApplyRandomScale(paintedMesh, _mp.rScaleW, _mp.rScaleH);
            }
            else if (!_mp.rWithinRange && _mp.uniformScale)
            {
                if (_mp.rScale > 0)
                    ApplyRandomScale(paintedMesh, _mp.rScale);
            }
            else if (_mp.rWithinRange && !_mp.uniformScale)
            {
                if (_mp.rNonUniformRange != Vector4.zero)
                    ApplyRandomScale(paintedMesh, _mp.rNonUniformRange);
            }
            else
            {
                if (_mp.rUniformRange != Vector2.zero)
                    ApplyRandomScale(paintedMesh, _mp.rUniformRange);
            }

            //Constant, additive scale (adds up to the total scale after everything else:
            if (!_mp.constUniformScale)
            {
                if (_mp.cScaleXYZ != Vector3.zero)
                    AddConstantScale(paintedMesh, _mp.cScaleXYZ.x, _mp.cScaleXYZ.y, _mp.cScaleXYZ.z);
            }
            else
            {
                if (_mp.cScale != 0)
                    AddConstantScale(paintedMesh, _mp.cScale);
            }

            if (_mp.rRot > 0)
                ApplyRandomRotation(paintedMesh, _mp.rRot);
            if (_mp.meshOffset != 0)
                ApplyMeshOffset(paintedMesh, _mp.meshOffset, scHit.normal);

            Undo.RegisterCreatedObjectUndo(paintedMesh, paintedMesh.name); //Allow the "undo" operation for the creation of meshes.
        }
    }
    #endregion

    #region Multiple Meshpaint
    void PaintMultipleMeshes() //Multiple meshpaint function.
    {
        insetThreshold = (_mp.hRadius * 0.01f * _mp.inset);

            //  For the creation of multiple meshes at once we need a temporary brush gameobject, which will wander around our circle brush's area to shoot rays and adapt the meshes.
        if (_mp.holderObj.Find("Brush") == false) { //In case we don't have one yet (or the user deleted it), create one.
            _mp.brush = new GameObject("Brush");
            _mp.brushTransform = _mp.brush.transform; //Initialize the brush's transform variable.
            _mp.brushTransform.position = thisTransform.position;
            _mp.brushTransform.parent = _mp.holderObj;
        }

        for (int i = _mp.meshCount ; i > 0 ; i--) 
        {
            randomValueInArray = Random.Range(0, _mp.setOfMeshesToPaint.Length); //Pick a random value inside the array of meshes to paint.

                //  Position the brush object slightly away from our raycasthit and rotate it correctly.
            _mp.brushTransform.position = scHit.point + (scHit.normal * 0.5f);
            _mp.brushTransform.rotation = Quaternion.LookRotation(scHit.normal); _mp.brushTransform.up = _mp.brushTransform.forward;

                //  Afterwards, translate it inside the brush's circle area based on the scattering percentage defined by the user.
            _mp.brushTransform.Translate(Random.Range(-Random.insideUnitCircle.x * insetThreshold, Random.insideUnitCircle.x * insetThreshold), 0, Random.Range(-Random.insideUnitCircle.y * insetThreshold, Random.insideUnitCircle.y * insetThreshold), Space.Self);

                //  Perform the final raycast from the brush object's location to our gameobject's surface. 
                //  I'm giving this a limit of 2.5m to avoid meshes being painted behind hills and walls when the brush's radius value is high.
            if (thisCollider.Raycast(new Ray(_mp.brushTransform.position, -scHit.normal), out brHit, 2.5f))
            {
                //Calculate the slope angle.
                slopeAngle = _mp.activeSlopeFilter ? Vector3.Angle(brHit.normal, _mp.manualRefVecSampling ? _mp.slopeRefVec : Vector3.up) : _mp.inverseSlopeFilter ? 180f : 0f; //Calculate the angle between the world's upvector (or a manually sampled reference vector) and the normal vector of our hit.

                //And if all conditions are met, paint our meshes according to the user's parameters.
                if (_mp.inverseSlopeFilter == true ? slopeAngle > _mp.maxSlopeFilterAngle : slopeAngle < _mp.maxSlopeFilterAngle)
                {
                    paintedMesh = Instantiate(_mp.setOfMeshesToPaint[randomValueInArray], brHit.point, Quaternion.LookRotation(brHit.normal)) as GameObject;

                    paintedMeshTransform = paintedMesh.transform;

                    if (_mp.yAxisIsTangent)
                        paintedMeshTransform.up = Vector3.Lerp(paintedMeshTransform.up, paintedMeshTransform.forward, _mp.slopeInfluence * 0.01f);
                    else
                        paintedMeshTransform.up = Vector3.Lerp(Vector3.up, paintedMeshTransform.forward, _mp.slopeInfluence * 0.01f);

                    paintedMeshTransform.parent = _mp.holderObj; //Afterwards we set the instantiated object as a parent of the holder GameObject.
                    paintedMesh.name = _mp.setOfMeshesToPaint[randomValueInArray].name;	//Set the painted object's name to the one of the asset you assigned as the "mesh to paint". 

                    if (_mp.autoStatic == true) paintedMesh.isStatic = true;

                    if (!_mp.rWithinRange && !_mp.uniformScale)
                    {
                        if (_mp.rScaleW > 0 || _mp.rScaleH > 0)
                            ApplyRandomScale(paintedMesh, _mp.rScaleW, _mp.rScaleH);
                    }
                    else if (!_mp.rWithinRange && _mp.uniformScale)
                    {
                        if (_mp.rScale > 0)
                            ApplyRandomScale(paintedMesh, _mp.rScale);
                    }
                    else if (_mp.rWithinRange && !_mp.uniformScale)
                    {
                        if (_mp.rNonUniformRange != Vector4.zero)
                            ApplyRandomScale(paintedMesh, _mp.rNonUniformRange);
                    }
                    else
                    {
                        if (_mp.rUniformRange != Vector2.zero)
                            ApplyRandomScale(paintedMesh, _mp.rUniformRange);
                    }

                    //Constant, additive scale (adds up to the total scale after everything else:
                    if (!_mp.constUniformScale)
                    {
                        if (_mp.cScaleXYZ != Vector3.zero)
                            AddConstantScale(paintedMesh, _mp.cScaleXYZ.x, _mp.cScaleXYZ.y, _mp.cScaleXYZ.z);
                    }
                    else
                    {
                        if (_mp.cScale != 0)
                            AddConstantScale(paintedMesh, _mp.cScale);
                    }

                    //The next two if-statements apply the random rotation and the vertical offset to the mesh.
                    if (_mp.rRot > 0)
                        ApplyRandomRotation(paintedMesh, _mp.rRot);
                    if (_mp.meshOffset != 0)
                        ApplyMeshOffset(paintedMesh, _mp.meshOffset, scHit.normal);

                    Undo.RegisterCreatedObjectUndo(paintedMesh, paintedMesh.name); //Allow the "undo" operation for the creation of meshes.
                }
            }
        } 
    }
    #endregion


    #region Other functions

    float rW, rH;
    void ApplyRandomScale(GameObject sMesh, float W, float H) //Apply some random scale (non-uniformly) to the freshly painted object.
    {
        rW = W*Random.value + 0.15f;
        rH = H*Random.value + 0.15f;
        sMesh.transform.localScale = new Vector3(rW,rH,rW);
    }
    float r;
    void ApplyRandomScale(GameObject sMesh, float U) //Here I overload the ApplyRandomScale function for the uniform random scale.
    {
        r = U * Random.value + 0.15f;
        sMesh.transform.localScale = new Vector3(r,r,r);
    }

    float s;
    void ApplyRandomScale(GameObject sMesh, Vector2 range) //Overload for the customized uniform random scale.
    {
        s = Random.Range(range.x,range.y);
        sMesh.transform.localScale = new Vector3(s,s,s);
	}

    
	void ApplyRandomScale(GameObject sMesh, Vector4 ranges) //Non-uniform custom random range scale.
	{
		rW = Random.Range(ranges.x,ranges.y);
		rH = Random.Range(ranges.z,ranges.w);
		sMesh.transform.localScale = new Vector3(rW,rH,rW);
	}
	
	void AddConstantScale(GameObject sMesh, float X, float Y, float Z) { //Same procedure for the constant scale methods.
		sMesh.transform.localScale += new Vector3(X,Y,Z);
	}
	
	void AddConstantScale(GameObject sMesh, float S) {
        sMesh.transform.localScale += new Vector3(S, S, S);
	}

    void ApplyRandomRotation(GameObject rMesh, float rot) //Apply some random rotation (around local Y axis) to the freshly painted mesh.
    { 
        float randomRotation = Random.Range(0f, 3.60f * rot);
        rMesh.transform.Rotate(new Vector3(0, randomRotation, 0));
	}

	void ApplyMeshOffset(GameObject oMesh, float offset, Vector3 direction) { //Apply the offset
        oMesh.transform.Translate((direction.normalized * offset * 0.01f), Space.World);  //We divide offset by 100 since we want to use centimeters as our offset unit (because 1cm = 0.01m)
    }

    #endregion

}

/*
 * 
 * Raphael Beck, 2014
 * 
 */
