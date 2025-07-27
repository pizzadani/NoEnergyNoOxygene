using UnityEngine;

public class TestMover : MonoBehaviour
{
    public RectTransform bar;
    public float speed = 200f;
    public float range = 400f;

    private float direction = 1f;
    private Vector3 startPos;

    void Start()
    {
        startPos = bar.localPosition;
    }

    void Update()
    {
        Vector3 pos = bar.localPosition;
        pos.x += speed * direction * Time.deltaTime;

        if (Mathf.Abs(pos.x - startPos.x) > range / 2)
        {
            direction *= -1;
        }

        bar.localPosition = pos;
    }
}
