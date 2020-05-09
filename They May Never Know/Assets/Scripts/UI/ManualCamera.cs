using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCamera : MonoBehaviour
{
    public float scrollMax = 20f;
    public float scrollMin = -160f;
    public float MouseScroll;

    public Man target = null;
    public float followSize;
    
    float oldScroll;
    float smoothTime = 0.3f;
    Vector3 velocity = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        oldScroll = MouseScroll;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            float MoveX = Input.GetAxisRaw("Horizontal");
            float MoveY = Input.GetAxisRaw("Vertical");
            MouseScroll += Input.GetAxis("Mouse ScrollWheel")*4;
            if (MouseScroll > scrollMax)
                MouseScroll = scrollMax;
            if (MouseScroll < scrollMin)
                MouseScroll = scrollMin;
       
            Camera.main.orthographicSize -= (MouseScroll - oldScroll);
            if (Camera.main.orthographicSize < 1.1f)
            {
                Camera.main.orthographicSize = 1.1f;
                MouseScroll = oldScroll;
            }
            transform.position = new Vector3(transform.position.x + ((Mathf.Log10(Camera.main.orthographicSize))/2 *  MoveX), transform.position.y + (Mathf.Log10(Camera.main.orthographicSize)/2 * MoveY),-20);
             //transform.position = transform.forward * MouseScroll;
             oldScroll = MouseScroll;
        } else
        {
            Vector3 targetPosition = target.transform.TransformPoint(new Vector3(0, 1, -10));
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
        
    }

    public void setTarget(Man man)
    {
        if(man == null)
        {
            target = null;
            oldScroll = -followSize;
            MouseScroll = -followSize;
        }
        else
        {
            target = man;
            StartCoroutine(setTargetAfterTime());
        }
        
    }

    IEnumerator setTargetAfterTime()
    {
        yield return new WaitForEndOfFrame();
        MouseScroll = -followSize;
        Camera.main.orthographicSize = followSize;
    }

}
