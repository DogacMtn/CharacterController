using UnityEngine;
using UnityEngine.Video;

public class MouseLook : MonoBehaviour
{

    public float interactDistance; //15f
    public KeyCode interactKey = KeyCode.E;
    public KeyCode endVideo = KeyCode.Q;
    public GameObject text;
    

    public float mouseSensitivity = 200f;

    public Transform playerBody;

    private float _xRotation;
    private Camera _mainCamera;
    public VideoPlayer video;
    private bool _isPlayed;
    public AudioSource backgroundMusic;
    public bool onInspect;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _mainCamera = Camera.main;
    }

    

    // Update is called once per frame
    void Update()
    {
        if(!onInspect)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        
        
            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -50f, 70f);
        
        
        
            transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        
        }
        
        ScreenRayCast();
        

    }
    
    public void ScreenRayCast()
    {
        
        RaycastHit hit;
        Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out hit, interactDistance) && hit.collider.CompareTag("videoscreen"))
        {
            float distance = Vector3.Distance(transform.position, video.transform.position);

            if (distance <= interactDistance)
            {
            
            
            
                if (Input.GetKeyDown(endVideo))
                {
                    video.Stop();
                    video.time = 0;
                }
            
                if (Input.GetKeyDown(interactKey))
                {
                    if (_isPlayed == false)
                    {
                        _isPlayed = true;
                        video.Play();
                        backgroundMusic.Stop();
                        backgroundMusic.time = 0;
                    }
                    else
                    {
                        _isPlayed = false;
                        video.Pause();
                        backgroundMusic.Play();
                    }
            
                }
            

                if(video.isPlaying)
                {
                    text.SetActive(false);
                
                }
                else
                {
                    text.SetActive(true);
                }
            }
            else
            {
                text.SetActive(false);
            }   
        }
        
        
        
    }
    
}
