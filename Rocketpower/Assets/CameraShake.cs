using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    public Transform camTransform;

    // How long the object should shake for.
    public float duration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.7f;
    public float decreaseFactor = 1.0f;
    public GameObject player;
    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        yield return new WaitForSeconds(0);

        float elapsed = 0.0f;
        float magnitude = 0.5f;

        while (elapsed < duration)
        {

            elapsed += Time.deltaTime;

            float calPerc = elapsed / duration;
            float zigzag = 1.0f - Mathf.Clamp(4.0f * calPerc - 3.0f, 0.0f, 1.0f);



            // camera position near about player transform position

            float FX = Random.Range(-1.0f, 1.0f);
            float x = Random.Range(player.transform.position.x, player.transform.position.x + FX);

            float FY = Random.Range(-1.0f, 1.0f);
            float y = Random.Range(player.transform.position.y, player.transform.position.y + FY);

            x += magnitude + zigzag;
            y += magnitude + zigzag;

            Vector3 pos = transform.position;
            pos.x += x;
            pos.y += y;
            transform.position = pos;

            yield return null;
        }

        // Camera.main.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, Camera.main.transform.position.z);
    }
}
//////////