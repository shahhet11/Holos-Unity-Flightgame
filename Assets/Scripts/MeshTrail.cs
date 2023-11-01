using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime = 2f;

    [Header("Mesh Related")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay = 3f; 
    public Transform positionToSpawn;

    [Header("Shader Related")]
    public Material mat;
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate = 0.05f;
    public bool isTrailActive;
    
    private SkinnedMeshRenderer[] meshRenderers;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey (KeyCode.Space) && !isTrailActive)
        {
            isTrailActive = true;
            StartCoroutine(ActiveTrail(activeTime));
        }
    }

    IEnumerator ActiveTrail (float timeAcitve)
    {
        while(timeAcitve > 0)
        {
            timeAcitve -= meshRefreshRate;

            if (meshRenderers == null)
                meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

            for (int i = 0; i < meshRenderers.Length; i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);
                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();
                Mesh mesh = new Mesh();
                meshRenderers[i].BakeMesh(mesh);
                mf.mesh = mesh;
                mr.material = mat;
                StartCoroutine(AnimateMaterialFloat(mr.material, 0.2f, shaderVarRate, shaderVarRefreshRate));
                Destroy(gObj, meshDestroyDelay);
                Debug.Log("AnimateMaterialFloat");
            }
            yield return new WaitForSeconds(meshRefreshRate);
        }

        isTrailActive = false;

    }

    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float refreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate > goal)
        {
            valueToAnimate -= rate;
            mat.SetFloat(shaderVarRef, valueToAnimate);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
