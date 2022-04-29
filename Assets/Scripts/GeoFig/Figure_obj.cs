using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure_obj : MonoBehaviour
{

    public Vector3 originPos;

    public List<GameObject> targetObjects;

    public bool done;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        originPos = transform.position;
    }

    private void OnMouseDrag()
    {
        Debug.Log("DRAGGIN " + gameObject.name);
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = originPos.z;
        transform.position = mousePos;
    }

    private void OnMouseUp()
    {
        Debug.Log("STOP and CHECK");
        foreach (GameObject item in targetObjects)
        {
            if (GeoFigManager.sharedInstance.completedTargets.Contains(item))
            {
                continue;
            }

            if (item.GetComponent<Collider2D>().OverlapPoint(transform.position))
            {
                Vector3 newPos = item.transform.position;
                newPos.z = transform.position.z;
                transform.position = newPos;
                done = true;
                GeoFigManager.sharedInstance.completedTargets.Add(item);

                GetComponent<Collider2D>().enabled = false;
                return;
            }
        }

        GeoFigManager.sharedInstance.mistakes++;
        transform.position = originPos;
    }
}
