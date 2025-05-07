using UnityEngine;

public class ParticleClick : MonoBehaviour
{
    public Setup setupManager;
    private Vector3 offset;
    private Vector3 mouseDownPos;
    private bool isDragging = false;
    private float clickThreshold = 5f;
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void OnMouseDown()
    {
        mouseDownPos = Input.mousePosition;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseDownPos.x, mouseDownPos.y, screenPos.z));
        offset = transform.position - worldPos;
    }

    void OnMouseDrag()
    {
        Vector3 currentMousePos = Input.mousePosition;
        if (!isDragging && Vector3.Distance(currentMousePos, mouseDownPos) > clickThreshold)
        {
            isDragging = true;
        }
        if (isDragging)
        {
            //setupManager.labels.enabled = false;
            Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(currentMousePos.x, currentMousePos.y, screenPos.z));

            if (gameObject.name != "Hi-Hat Field" && gameObject.name != "Ride Field")
            {
                transform.position = worldPos + offset;
            }
            

            switch (gameObject.name)
            {
                case "Bass Drum Field":
                    setupManager.bassPosition = transform.position;
                    break;
                case "Crash Field":
                    setupManager.crashPosition = transform.position;
                    break;
                case "Snare Drum Field":
                    setupManager.snarePosition = transform.position;
                    break;
                case "Tom 1 Field":
                    setupManager.tomPositions[0] = transform.position;
                    break;
                case "Tom 2 Field":
                    setupManager.tomPositions[1] = transform.position;
                    break;
                case "Tom 3 Field":
                    setupManager.tomPositions[2] = transform.position;
                    break;
                default:
                    Debug.Log("IDFK");
                    break;
            }
        }
    }

    void OnMouseUp()
    {
        if (!isDragging)
        {
            setupManager.labels.enabled = false;
            ParticleSystemForceField fieldClicked = GetComponent<ParticleSystemForceField>();
            Debug.Log(fieldClicked.gameObject.transform.position);
            GameObject.Find("SetupPage").GetComponent<Canvas>().enabled = false;
            
            //setupManager.colourPanel.SetActive(false);

            fieldClicked.gameObject.transform.position = new Vector3(5, 0, 0);

            setupManager.pauseParticles(fieldClicked);
            if (fieldClicked.gameObject.name == "Hi-Hat Field")
            {
                setupManager.hatsMenu();
            } 
            else if (fieldClicked.gameObject.name == "Ride Field")
            {
                setupManager.rideMenu();
            }
            else
            {
                GameObject.Find("Particle Editor").GetComponent<Canvas>().enabled = true;
                setupManager.particleLooping(fieldClicked);
            }
            
            
        }
        isDragging = false;
    }
}