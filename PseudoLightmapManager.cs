using UnityEngine;
using System.IO;
using UnityEngine.Events;
using System.Collections.Generic;

/* NOTES:

   ALSO: My Favourite Piece Of Code Yet - Omptimized To Prefection With Really Good Code Overall!

   HOW TO USE: To Change Lightmap You Need To Set 'LightmapIndex' To Whatever The Index Of The Lightmap.
   As Soon As You Set It, The Lightmap Will Update (Unless You Enable: 'updateEveryFrame').
   This Obviously Saves A Lot Of Performance, As The Lightmap Switching Does Not Change All The Lightmaps
   In The Scene Every Single Frame.

   STN: The Script Is Incredibly Basic Yet Really Modular, You Can Put Pretty Much As
   Much Lightmaps As You Want And Change Between Them In A Single Line Of Code.
   MEANING: It Can Be Used Globally And Very Comfortably So!

   STN: Everything That The Script Needs ALREADY HERE Meaning - 
   If You Import It In To A New Project, It Works Perfectly Fine
   Considering It Doesn't Rely On Other Scripts.
*/

public class PseudoLightmapManager : MonoBehaviour
{
    public static PseudoLightmapManager Instance {get; private set;}

    #region Variables
    [Header("Lightmap Options:"), SerializeField] private int lightmapIndex; 
    [Space]
    [SerializeField] private bool updateEveryFrame = false;
    [SerializeField] private bool updateOnStart = true, enableMessagesOnConsole = false;
    
    [Space] public UnityEvent onLightmapIndexChanged;

    [Header("Lightmaps:")]
    public PseudoLightmapInfo[] PseudoDynamicLightmaps;

    public int LightmapIndex 
    {
        get { return lightmapIndex; } 

        set 
        {
            lightmapIndex = value;

            if (!updateEveryFrame) 
            {
                updateLightmap(); // Update Lightmap On Value Changed.
                onLightmapIndexChanged.Invoke(); 
            }
        }
    }
    #endregion

    #region Callback Methods
    void Awake()
    {
        if (Instance)
        {
            Debug.LogWarning($"Two Pseudo Dynamic Lightmap Managers Have Been Found In The Same Scene - Terminating This GameObject: ({transform.name})");
            Destroy(gameObject);
        }

        else
            Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitializeLMS();
        if (updateOnStart) updateLightmap();
    }

    // Update is called once per frame
    void Update() {
        if (updateEveryFrame) LightmapIndex = lightmapIndex; // <-- For Updating Every Frame. 
    }
    #endregion


    #region PDLM Specific Functions
    public void updateLightmap() 
    {
        if (lightmapIndex > PseudoDynamicLightmaps.Length - 1 | lightmapIndex < 0) 
        {
            if (enableMessagesOnConsole) 
                Debug.LogWarning($"Index Isn't In Lightmap Array's Range (AKA: Not In Range Of: 0 - {PseudoDynamicLightmaps.Length - 1})");

            return;
        }

        if (enableMessagesOnConsole) Debug.Log($"Lightmap Changed! (Index: {lightmapIndex})");
        LightmapSettings.lightmaps = PseudoDynamicLightmaps[lightmapIndex].DATA;
    }

    private void InitializeLMS()
    { 
        foreach (PseudoLightmapInfo pdlm in PseudoDynamicLightmaps)
        {
            if (pdlm.GetTexturesFromDirectory)
                pdlm.AssignTexturesFromDirectory(pdlm.directory);

            List<LightmapData> lmd = new List<LightmapData>();

            for (int i = 0; i < pdlm.DIR.Length; i++)
            {
                LightmapData mlm = new LightmapData();

                mlm.lightmapColor = pdlm.CLR[i];
                mlm.lightmapDir   = pdlm.DIR[i];

                lmd.Add(mlm);
            }

            pdlm.DATA = lmd.ToArray();
        }
    }
}
#endregion

#region PDLightmap Type
[System.Serializable]
public class PseudoLightmapInfo
{
    public Texture2D[]    CLR, DIR;
    public LightmapData[]     DATA; // <-- Doesn't Show In Inspector So HideInInspector Is Useless.
    
    [Space]
    public bool GetTexturesFromDirectory = false;
    public string directory = "Assets/";
    // End Of Variables \\



    // Get All Lightmap Types From Directory.
    //
    // Function Is Declared Here.
    public void AssignTexturesFromDirectory(string dir)
    {
        DirectoryInfo d = new DirectoryInfo(dir);
        
        List<Texture2D> directionalLightmaps = new List<Texture2D>();
        List<Texture2D> coloredLightmaps     = new List<Texture2D>();

        // Get Colored Lightmaps.
        foreach (var file in d.GetFiles("*.*"))
        {
            // Skip .meta files
            if (file.Extension == ".meta")
                continue; // Skip to the next iteration

            string fileName = file.ToString();

            // Read The File Data
            byte[] fileData = File.ReadAllBytes(file.FullName);
            Texture2D texture = new Texture2D(2, 2); // Create a new Texture2D (placeholder size)

            // Load The File Into The Texture2D
            if (texture.LoadImage(fileData))
            {
                if (fileName.Contains("_dir"))
                    directionalLightmaps.Add(texture);

                else if (fileName.Contains("_light"))
                    coloredLightmaps.Add(texture);
            }

            else
                Debug.LogError($"Failed to load texture from file: {fileName}, Directory: {dir}" );
        }
        
        // Assignment:
        DIR = directionalLightmaps.ToArray();
        CLR =     coloredLightmaps.ToArray();
    }
    #endregion
}
