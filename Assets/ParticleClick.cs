using UnityEngine;

public class ParticleClick : MonoBehaviour
{
    //Get setup manage
    public Setup setupManager;

    //Mouse information variables
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
        //Get position of mouse on click
        mouseDownPos = Input.mousePosition;

        //Determine offset
        Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
        Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(mouseDownPos.x, mouseDownPos.y, screenPos.z));
        offset = transform.position - worldPos;
    }

    void OnMouseDrag()
    {
        Vector3 currentMousePos = Input.mousePosition;
        //Checks if the user is not already draggin element nad has moved since initial click +5 pixels
        if (!isDragging && Vector3.Distance(currentMousePos, mouseDownPos) > clickThreshold)
        {
            isDragging = true;
        }
        if (isDragging)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(transform.position);
            Vector3 worldPos = mainCamera.ScreenToWorldPoint(new Vector3(currentMousePos.x, currentMousePos.y, screenPos.z));

            //Don't allow for hi-hat and ride to be moved
            if (gameObject.name != "Hi-Hat Field" && gameObject.name != "Ride Field")
            {
                transform.position = worldPos + offset;
            }
            

            //Update the object position in the setup manage to keep up to date
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
                    Debug.Log("Default");
                    break;
            }
        }
    }

    void OnMouseUp()
    {
        //If we're not dragging and have released our click bring up the setup element menu
        if (!isDragging)
        {
            //Remove element labels
            setupManager.labels.enabled = false;
            
            //Get information on element
            ParticleSystemForceField fieldClicked = GetComponent<ParticleSystemForceField>();

            //Disable setup page default menu
            GameObject.Find("SetupPage").GetComponent<Canvas>().enabled = false;
            
            //Move the object to the side for the menu
            fieldClicked.gameObject.transform.position = new Vector3(5, 0, 0);

            //Run function to pause all other particles
            setupManager.pauseParticles(fieldClicked);

            //Bring up special menus for hats and ride, otherwise default menu
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
                //Bring up default menu
                GameObject.Find("Particle Editor").GetComponent<Canvas>().enabled = true;
                setupManager.particleLooping(fieldClicked);
            }
        }
        isDragging = false;
    }
}