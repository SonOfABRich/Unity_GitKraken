using UnityEngine;
using TMPro;

public class PolyCounts : MonoBehaviour
{

    [SerializeField] public TextMeshProUGUI _FPSTracker;
    [SerializeField] public TextMeshProUGUI _PolyCounterTracker;
    [SerializeField] public TextMeshProUGUI _ObjectCountTracker;
    [SerializeField] public GameObject _PolyCountMenu;
    [SerializeField] private int _ScenePolyBudget = 1000000;
    [SerializeField] public TextMeshProUGUI _ScenePolyBudgetTracker;
    [SerializeField] public TextMeshProUGUI _ScenePolyBudgetRemainingTracker;
    [SerializeField] public TextMeshProUGUI _ScenePolyBudgetPercentRemainingTracker;


    public static int verts;
    public static int tris;
    public int numbObjs;
    [SerializeField] private bool enableMenuTracker = false;

    public float updateInterval = 0.5F;

    private float accum = 0; // FPS accumulated over the interval
    private int frames = 0; // Frames drawn over the interval
    private float timeleft; // Left time for current interval
    public float fps;

    // Use this for initialization
    void Start()
    {
        timeleft = updateInterval;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightBracket)) {
            if (enableMenuTracker == true)
            {
                enableMenuTracker = false;
            } else
            {
                enableMenuTracker = true;
            }
        }

        if (enableMenuTracker)
        {
            if (!_PolyCountMenu.activeInHierarchy) {
                _PolyCountMenu.SetActive(true);
            }
            timeleft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            ++frames;

            if (timeleft <= 0.0)
            {
                // display two fractional digits (f2 format)
                fps = accum / frames;
                string format = System.String.Format("{0:F2} FPS", fps);
                //  DebugConsole.Log(format,level);
                timeleft = updateInterval;
                accum = 0.0F;
                frames = 0;
                GetObjectStats();
            }

            _FPSTracker.text = fps.ToString();
            _ObjectCountTracker.text = numbObjs.ToString();
            _PolyCounterTracker.text = (verts/3).ToString();
            _ScenePolyBudgetTracker.text = _ScenePolyBudget.ToString();
            _ScenePolyBudgetRemainingTracker.text = (_ScenePolyBudget - (verts/3)).ToString();
            _ScenePolyBudgetPercentRemainingTracker.text = (((_ScenePolyBudget - (verts / 3)) /( _ScenePolyBudget / 100))).ToString();

            print("StartingPoint");
            print(numbObjs);
            print(fps);
            print(verts / 3); //tris
            print(tris / 2); //polygons
            print("EndingPoint");
        }
        else { _PolyCountMenu.SetActive(false); }
    }



    void GetObjectStats()
    {
        verts = 0;
        tris = 0;
        GameObject[] ob = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject obj in ob)
        {
            GetObjectStats(obj);
        }

        numbObjs = ob.Length;
    }

    void GetObjectStats(GameObject obj)
    {
        Component[] filters;
        filters = obj.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter f in filters)
        {
            tris += f.sharedMesh.triangles.Length / 3;
            verts += f.sharedMesh.vertexCount;
        }
    }
}
