using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class InspectManager : MonoBehaviour
{

    public float distance;
    public Transform playerSocket;

    private Vector3 _originalPos;
    private bool _onInspect;
    private GameObject _inspected;
    private Quaternion _inspectedDefaultRotation;
    public Volume globalVolume;
    private DepthOfField _depthOfField;
    
    public PlayerMovement playerScript;

    public MouseLook cameraScript;

    

    private void Update()
    {
        InspectorRayCast();
    }

    private void InspectorRayCast()
    {

        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, fwd, out hit, distance))
        {
            if (hit.transform.CompareTag("InspectableObject") && !_onInspect)  //Compare Tag
            {
                if (Input.GetKeyDown(KeyCode.E))
                {

                    _inspected = hit.transform.gameObject;
                    _originalPos = hit.transform.position;
                    cameraScript.onInspect = true;
                    
                    if (globalVolume.profile.TryGet(out _depthOfField))
                    {
                        _depthOfField.active = true;
                    }

                    _inspectedDefaultRotation = _inspected.transform.rotation;
                    StartCoroutine(PickupItem());
                }
            }
        }

        if (_onInspect)
        {
            _inspected.transform.position = Vector3.Lerp(_inspected.transform.position, playerSocket.position, 0.1f);
            playerSocket.Rotate(new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0) * (Time.deltaTime * 125f));
        }
        else if (_inspected != null)
        {
            _inspected.transform.rotation = Quaternion.Lerp(_inspected.transform.rotation, _inspectedDefaultRotation, 0.1f);
            _inspected.transform.SetParent(null);
            _inspected.transform.position = Vector3.Lerp(_inspected.transform.position, _originalPos, 0.1f);
        }

        if (Input.GetKeyDown(KeyCode.E) && _onInspect)
        {
            StartCoroutine(DropItem());
        }

        IEnumerator PickupItem()
        {
            playerScript.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _inspected.transform.SetParent(playerSocket);
            _onInspect = true;
        }

        IEnumerator DropItem()
        {
            _onInspect = false;
            cameraScript.onInspect = false;
            yield return new WaitForSeconds(0.1f);
            playerScript.enabled = true;
            
            
            if (globalVolume.profile.TryGet(out _depthOfField))
            {
                _depthOfField.active = false;
            }
            
        }
        
    }
    
}
