using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
#if PP_V2_PRESENT
using UnityEngine.Rendering.PostProcessing;
#endif

namespace LowPolyUnderwaterPack
{
    /// <summary>
    /// Low Poly Underwater Pack script that applies camera, lighting, and scene effects when the camera is underwater.
    /// </summary>
    [RequireComponent(typeof(Camera))]
    public class UnderwaterEffect : MonoBehaviour
    {
        [Tooltip("Position offset for detecting whether the camera is above or under the water.")]
        [SerializeField] private Vector3 waterDetectionOffset = Vector3.zero;

        #region Effect Settings

#if PP_V2_PRESENT
        [Tooltip("Post processing profile for when the camera is above water.")]
        [SerializeField] private PostProcessProfile surfaceProfile = null;
        [Tooltip("Post processing profile for when the camera is underwater.")]
        [SerializeField] private PostProcessProfile underwaterProfile = null;
        [Tooltip("The color of the underwater fog.")]
#endif
        [SerializeField] private Color fogColor = Color.blue;
        [Tooltip("Toggle whether to change the tint color of the skybox to the color of the fog when underwater to better blend into the environment.")]
        [SerializeField] private bool modifySkyboxTint = false;

        [Tooltip("Water refraction effect material, used to create a refraction image effect.")]
        [SerializeField] private Material refractionMaterial = null;

        #endregion

        #region Hidden Fields

        [HideInInspector] public bool isUnderwater = false;
        [HideInInspector] public Vector3[] waterVerts;

        #endregion

        #region Private Fields

        private Material skybox;
        private Color skyColor;

        private WaterMesh water;
#if PP_V2_PRESENT
        private PostProcessVolume vol;
#endif
        private Camera cam;

        private string tintPropName;
        private Vector3 waterMeshPoint = Vector3.zero;

        private RaycastHit[] overHits, underHits;
        private int overHitsLength, underHitsLength;
        WaterMesh currentWater = null;
        Vector3 offsetPos;

        #region Boids Test Scene Variables
        
        private GameObject boidsRoom; 
        private bool inBoidsRoomScene;

        #endregion

        #endregion

        #region Unity Callbacks

        private void Awake() 
        {
#if PP_V2_PRESENT
            vol = GetComponent<PostProcessVolume>();
#endif
            cam = GetComponent<Camera>();
        }

        private void Start()
        {
            // Ensure camera generates a depth texture
            cam.depthTextureMode = DepthTextureMode.Depth;

            overHits = new RaycastHit[15];
            underHits = new RaycastHit[15];

            // Not in boids test room
            if (SceneManager.GetActiveScene().name != "Boids Test")
            {
                skybox = RenderSettings.skybox;
                if (RenderSettings.skybox.HasProperty("_Tint"))
                    tintPropName = "_Tint";
                if (RenderSettings.skybox.HasProperty("_SkyTint"))
                    tintPropName = "_SkyTint";
                
                if (modifySkyboxTint)
                    skyColor = skybox.GetColor(tintPropName);

                return;
            }
            
            // Only find the boids test room if in the boids test scene
            inBoidsRoomScene = true;
            boidsRoom = GameObject.Find("Boid Test Room");
        }

        private void OnValidate()
        {
            if (cam == null)
                cam = GetComponent<Camera>();

            // Change underwater fog and background color in real time
            RenderSettings.fogColor = fogColor;
            cam.backgroundColor = fogColor;
        }

        private void LateUpdate()
        {
            // Special logic if we are in the boids test room
            if (inBoidsRoomScene)
            {
                // If in the boids test scene but no boids room exists, do nothing and return
                if (boidsRoom == null)
                    return;

                // Camera is underwater if it is inside the walls of the test room
                ApplyUnderwaterEffects(boidsRoom.GetComponent<MeshCollider>().bounds.Contains(transform.position));

                return;
            }

            // Determining if object is over/under water or not and getting a reference to the water if it is
            overHitsLength = Physics.RaycastNonAlloc(transform.position + (Vector3.up * 10), -Vector3.up, overHits, float.MaxValue);
            underHitsLength = Physics.RaycastNonAlloc(transform.position - (Vector3.up * 10), Vector3.up, underHits, float.MaxValue);

            currentWater = null;

            for (int i = 0; i < overHitsLength || i < underHitsLength; i++)
            {
                // Check if over water
                if (i < overHitsLength)
                {
                    if(overHits[i].transform.TryGetComponent(out WaterMesh wm))
                    {
                        currentWater = wm;
                        break;
                    }           
                }

                // Check if under water
                if (i < underHitsLength)
                {
                    if(underHits[i].transform.TryGetComponent(out WaterMesh wm))
                    {
                        currentWater = wm;
                        break;
                    }
                }
            }

            // Assign the current water reference to "water" variable. Will assign null if no water above or under the camera. 
            water = currentWater; 

            // Water detection position offsetted for a optimally smooth transition between above and below water
            offsetPos = transform.position + waterDetectionOffset;
            if (water != null) waterMeshPoint = water.GetWaterPoint(offsetPos);

            // If the point on the water is above our position, we are underwater and should be applying underwater effects
            ApplyUnderwaterEffects((waterMeshPoint != Vector3.zero) ? waterMeshPoint.y > offsetPos.y : false);
        }

        // THIS WILL NOT WORK IN URP
        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            // Apply refraction material if underwater
            if (isUnderwater && refractionMaterial != null)
            {
                Graphics.Blit(source, destination, refractionMaterial);
                return;
            }

            Graphics.Blit(source, destination);
        }

        private void OnDrawGizmos()
        {
            // Draw green sphere gizmo where our offsetted water detection point is
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(transform.position + waterDetectionOffset, .25f);
        }

        private void OnApplicationQuit()
        {
            // Reset underwater effects when quitting or going back into edit mode
            ApplyUnderwaterEffects(false);
        }

        #endregion

        private void ApplyUnderwaterEffects(bool underwater)
        {
            // Underwater effects
            isUnderwater = underwater;
            RenderSettings.fog = underwater;
            RenderSettings.skybox.SetColor("_Tint", ((!underwater && modifySkyboxTint) ? skyColor : fogColor));
#if PP_V2_PRESENT
            vol.profile = ((underwater) ? underwaterProfile : surfaceProfile);
#endif
        }
    }

    /// <summary>
    /// Low Poly Underwater Pack custom editor which creates a custom inspector for UnderwaterEffect to organize properties and improve user experience.
    /// </summary>
#if UNITY_EDITOR
    [CustomEditor(typeof(UnderwaterEffect), true), CanEditMultipleObjects, System.Serializable]
    public class UnderwaterEffect_Editor : Editor
    {
        private SerializedProperty waterDetectionOffset, surfaceProfile, underwaterProfile, fogColor, modifySkyboxTint, refractionMaterial;

        private bool effectFoldout = true;

        private void OnEnable()
        {
            #region Seriealized Property Initialization

            waterDetectionOffset = serializedObject.FindProperty("waterDetectionOffset");
            
            surfaceProfile = serializedObject.FindProperty("surfaceProfile");
            underwaterProfile = serializedObject.FindProperty("underwaterProfile");
            fogColor = serializedObject.FindProperty("fogColor");
            modifySkyboxTint = serializedObject.FindProperty("modifySkyboxTint");

            refractionMaterial = serializedObject.FindProperty("refractionMaterial");

            #endregion
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Grayed out script property
            GUI.enabled = false;
            EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((UnderwaterEffect)target), typeof(UnderwaterEffect), false);
            GUI.enabled = true;

            EditorGUILayout.PropertyField(waterDetectionOffset);

            EditorGUILayout.Space(10);

            #region Effect Settings

            effectFoldout = GUIHelper.Foldout(effectFoldout, "Effect Settings");

            if (effectFoldout)
            {
                EditorGUI.indentLevel++;

#if PP_V2_PRESENT
                EditorGUILayout.PropertyField(surfaceProfile);
                EditorGUILayout.PropertyField(underwaterProfile);
#endif
                EditorGUILayout.PropertyField(fogColor);
                EditorGUILayout.PropertyField(modifySkyboxTint);

                EditorGUILayout.Space(10);

                EditorGUILayout.PropertyField(refractionMaterial);

                EditorGUI.indentLevel--;
            }

            #endregion

            serializedObject.ApplyModifiedProperties();
            
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
#endif
}