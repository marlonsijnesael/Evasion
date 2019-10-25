using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    public static playerManager _Instance;

    public Text checkpointText;

    private int checkpointIndex;
    private int checkpointNextIndex = 1;

    private int waypointsAtStart;
    private int waypointsCleared;

    [HideInInspector] public int indexToSwap;
    [HideInInspector] public bool isFinishActive = false;

    public Transform parentCheckpoint;
    public GameObject[] checkpoints;
    public SortedDictionary<int, GameObject> checkPointsDict = new SortedDictionary<int, GameObject>();

    private Vector3 checkpointHeight;
    private Vector3 finishHeight;

    [SerializeField] private GameObject Finish;
    [SerializeField] private Material orangeCheckpoint;
    [SerializeField] private Material blueCheckpoint;
    [SerializeField] private Material nonactiveFinish;
    [SerializeField] private Material activeFinish;

    public AudioClip hitEffect;
    private AudioSource audioSource;

    private void Awake()
    {
        checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");

        if (_Instance == null)
        {
            _Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {
        waypointsAtStart = checkPointsDict.Count;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        NextCheckpointSequence();
        //Debug.Log("Checkpoint Next Index = " + checkpointNextIndex);
        //Debug.Log("Waypoints Cleared = " + waypointsCleared);
    }

    public void ResetCheckpoints()
    {
        checkpoints = ShuffleArray(checkpoints);
        Finish.SetActive(false);
        isFinishActive = false;
        checkpointNextIndex = 1;
        checkpointIndex = 0;
        //waypointsCleared = 1;

        checkpointText.color = Color.white;
        checkpointText.text = 0 + " / " + checkpoints.Length;

        checkPointsDict = new SortedDictionary<int, GameObject>();

        for (int i = 0; i < parentCheckpoint.childCount; i++)
        {
            parentCheckpoint.GetChild(i).gameObject.SetActive(false);
        }

        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkPointsDict.Add(i, checkpoints[i]);
            //checkPointsDict[0].SetActive(true);
        }
    }

    public T[] ShuffleArray<T>(T[] array) {
        System.Random r = new System.Random();
        for(int i = array.Length; i > 0; i--) {
            int j = r.Next(i);
            T k = array[j];
            array[j] = array[i - 1];
            array[i - 1] = k;
        }
        return array;
    }

    public void UpdatecheckPoints(GameObject _checkpoint)
    {
        audioSource.clip = hitEffect;
        audioSource.volume = 0.2f;
        audioSource.Play();

        if (checkpointIndex == checkpoints.Length) {

        }
        else {
            checkpointIndex++;
            checkpointNextIndex++;
            checkpointText.text = checkpointIndex + " / " + checkpoints.Length;
        }

        for (int i = 0; i <= checkpoints.Length; i++)
        {
            if (checkPointsDict.ContainsKey(i)) {
                if (checkPointsDict[i] == _checkpoint) {
                    _checkpoint.SetActive(true);
                    checkPointsDict.Remove(i);
                    //float checkpointsCleared = waypointsCleared - checkPointsDict.Count;
                    //waypointsCleared++;
                }
            }
        }
    }

    public void NextCheckpointSequence() {
        if(checkpointIndex == checkpoints.Length) {
            isFinishActive = true;
            checkpointText.color = Color.green;

            Finish.gameObject.GetComponent<Renderer>().material = activeFinish;
            finishHeight = new Vector3(60, 120, 60);
            Finish.gameObject.transform.localScale = finishHeight;
        }
        else {
            checkpoints[checkpointIndex].SetActive(true);
            checkpoints[checkpointIndex].gameObject.GetComponent<Renderer>().material = blueCheckpoint;
            checkpoints[checkpointIndex].gameObject.GetComponent<Collider>().enabled = true;
            checkpointHeight = new Vector3(60, 80, 60);
            checkpoints[checkpointIndex].gameObject.transform.localScale = checkpointHeight;
        }

        if (checkpointNextIndex < checkpoints.Length) {
            checkpoints[checkpointNextIndex].SetActive(true);
            checkpoints[checkpointNextIndex].gameObject.GetComponent<Renderer>().material = orangeCheckpoint;
            checkpoints[checkpointNextIndex].gameObject.GetComponent<Collider>().enabled = false;
            checkpointHeight = new Vector3(60, 40, 60);
            checkpoints[checkpointNextIndex].gameObject.transform.localScale = checkpointHeight;
        }
        else if (checkpointIndex < checkpoints.Length){
            Finish.SetActive(true);
            Finish.gameObject.GetComponent<Renderer>().material = nonactiveFinish;
            finishHeight = new Vector3(60, 40, 60);
            Finish.gameObject.transform.localScale = finishHeight;
        }
    }

}