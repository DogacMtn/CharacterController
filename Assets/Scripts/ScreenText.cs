using UnityEngine;

public class ScreenText : MonoBehaviour
{

    public Transform target;
    
    void Update()
    {
        transform.LookAt(target);
    }
}
