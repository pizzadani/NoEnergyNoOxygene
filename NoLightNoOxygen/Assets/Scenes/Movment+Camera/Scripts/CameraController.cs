using UnityEngine;

public class CameraController: MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0f, 10f, 0f); // direkt von oben

    void Update()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = Quaternion.Euler(90f, 0f, 0f); // senkrechte Draufsicht
        }
    }
}