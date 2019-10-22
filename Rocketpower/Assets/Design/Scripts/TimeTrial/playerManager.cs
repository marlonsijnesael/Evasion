using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class playerManager : MonoBehaviour
{
    public static playerManager _Instance;
    public Text checkpointText;
    public int checkpointsTotal;

    public GameObject[] checkpoints;
    public Dictionary<int, GameObject> checkPointsDict = new Dictionary<int, GameObject>();

    public Transform parentCheckpoint;

    [SerializeField] private GameObject Finish;

    public AudioClip hitEffect;
    private AudioSource audioSource;

    private void Awake()
    {
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
        ResetCheckpoints();
        audioSource = GetComponent<AudioSource>();
        //Finish = GameObject.FindGameObjectWithTag("Fin");
        //Debug.Log(Finish);
    }

    public void ResetCheckpoints()
    {
        checkPointsDict = new Dictionary<int, GameObject>();
        Finish.SetActive(false);

        for (int i = 0; i < parentCheckpoint.childCount; i++)
        {
            parentCheckpoint.GetChild(i).gameObject.SetActive(true);
        }

        for (int i = 0; i < checkpoints.Length; i++)
        {
            checkPointsDict.Add(i, checkpoints[i]);
            checkpointText.text = checkPointsDict.Count.ToString() + " /" +;
        }
    }

    public void ResetCheckpointPlace()
    {
        for (int i = 0; i < parentCheckpoint.childCount; i++)
        {
            parentCheckpoint.GetChild(i).gameObject.SetActive(true);
        }
    }

    public void UpdatecheckPoints(GameObject _checkpoint)
    {
        audioSource.clip = hitEffect;
        audioSource.Play();

        for (int i = 0; i <= checkpoints.Length; i++)
        {
            if (checkPointsDict.ContainsKey(i))
            {
                if (checkPointsDict[i] == _checkpoint)
                {
                    _checkpoint.SetActive(false);
                    checkPointsDict.Remove(i);
                    checkpointText.text = checkPointsDict.Count.ToString() + " / 3";
                }
                if (checkPointsDict.Count == 0) {
                    Finish.SetActive(true);
                }
            }
        }
    }
}