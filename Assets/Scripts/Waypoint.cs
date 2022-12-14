using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    public GameObject left;
    [SerializeField]
    public GameObject right;
    [SerializeField]
    public GameObject next;
    public bool isEnd = false;

    void Start()
    {
        Color c = Color.black;
        if (this.tag == "Left")
        {
            c = Color.red;
        }
        if (this.tag == "Right")
        {
            c = Color.blue;
        }
        if (this.tag == "Center")
        {
            c = Color.green;
        }
        MeshRenderer renderer = this.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            //renderer.material.color = c;
        }

    }
    void Update()
    {
    }
    public Transform getLeftMove()
    {
        return this.left == null ? null : this.left.transform;

    }
    public Transform getRightMove()
    {
        return this.right == null ? null : this.right.transform;
    }

}
