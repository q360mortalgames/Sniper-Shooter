using UnityEngine;
using System.Collections;

public class MeshBrushParent : MonoBehaviour{

    Component[] meshes;
    Component[] meshFilters;
    Matrix4x4 myTransform;

    Hashtable materialToMesh;

    MeshFilter filter;
    Renderer curRenderer;

    Material[] materials;

    MB_MeshCombineUtility.MeshInstance instance;
    MB_MeshCombineUtility.MeshInstance[] instances;

    ArrayList objects;
    ArrayList elements;

	public void FlagMeshesAsStatic ()
	{
        meshes = GetComponentsInChildren(typeof(Transform));
		foreach(Transform _t in meshes){
			_t.gameObject.isStatic = true;
		}
	}
	public void UnflagMeshesAsStatic ()
	{
        meshes = GetComponentsInChildren(typeof(Transform));
		foreach(Transform _t in meshes){
			_t.gameObject.isStatic = false;
		}
	}
    public void DeleteAllMeshes()
    {
        DestroyImmediate(gameObject);
    }

    public void CombinePaintedMeshes()
    {
        meshFilters = GetComponentsInChildren(typeof(MeshFilter));
        myTransform = transform.worldToLocalMatrix;
        materialToMesh = new Hashtable();

        for (long i = 0; i < meshFilters.Length; i++)
        {
            filter = (MeshFilter)meshFilters[i];
            curRenderer = meshFilters[i].GetComponent<Renderer>();
            instance = new MB_MeshCombineUtility.MeshInstance();
            instance.mesh = filter.sharedMesh;
            if (curRenderer != null && curRenderer.enabled && instance.mesh != null)
            {
                instance.transform = myTransform * filter.transform.localToWorldMatrix;

                materials = curRenderer.sharedMaterials;
                for (int m = 0; m < materials.Length; m++)
                {
                    instance.subMeshIndex = System.Math.Min(m, instance.mesh.subMeshCount - 1);

                    objects = (ArrayList)materialToMesh[materials[m]];
                    if (objects != null)
                    {
                        objects.Add(instance);
                    }
                    else
                    {
                        objects = new ArrayList();
                        objects.Add(instance);
                        materialToMesh.Add(materials[m], objects);
                    }
                }

                DestroyImmediate(curRenderer.gameObject);
            }
        }

        foreach (DictionaryEntry de in materialToMesh)
        {
            elements = (ArrayList)de.Value;
            instances = (MB_MeshCombineUtility.MeshInstance[])elements.ToArray(typeof(MB_MeshCombineUtility.MeshInstance));

            // We have a maximum of one material, so just attach the mesh to our own gameobject
            if (materialToMesh.Count == 1)
            {
                // Make sure we have a mesh filter & renderer
                if (GetComponent(typeof(MeshFilter)) == null)
                    gameObject.AddComponent(typeof(MeshFilter));
                if (GetComponent(typeof(MeshRenderer)) == false)
                    gameObject.AddComponent(typeof(MeshRenderer));

                filter = (MeshFilter)GetComponent(typeof(MeshFilter));
                filter.mesh = MB_MeshCombineUtility.Combine(instances, false);
                GetComponent<Renderer>().material = (Material)de.Key;
                GetComponent<Renderer>().enabled = true;
            }
            // We have multiple materials to take care of, build one mesh / gameobject for each material
            // and parent it to this object
            else
            {
                GameObject go = new GameObject("Combined mesh");
                go.transform.parent = transform;
                go.transform.localScale = Vector3.one;
                go.transform.localRotation = Quaternion.identity;
                go.transform.localPosition = Vector3.zero;
                go.AddComponent(typeof(MeshFilter));
                go.AddComponent<MeshRenderer>();
                go.GetComponent<Renderer>().material = (Material)de.Key;
				go.isStatic = true;
                filter = (MeshFilter)go.GetComponent(typeof(MeshFilter));
                filter.mesh = MB_MeshCombineUtility.Combine(instances, false);
            }
        }
		gameObject.isStatic = true;
    }
}

/*
 * 
 * Raphael Beck, 2014
 * 
*/
