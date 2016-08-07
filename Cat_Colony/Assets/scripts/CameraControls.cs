using UnityEngine;
using System.Collections;

/*
        ____
         _[]_/____\__n_
        |_____.--.__()_|
        |LI  //# \\    |
        |    \\__//    |
        |     '--'     |
    jgs '--------------'


Left click, activate/interact with buttons, cats, objects
Scroll, zoom in and out of world
Move cursor, move around the world
Middle mouse button, hold down and drag mouse left or right to rotate view
Right mouse button, hold and drag for precision movement
*/
// Currently working on: Move cursor to move around the world
// NOTE: Add boundries to the world so camera doesn't move outside of it
public class CameraControls : MonoBehaviour
{

    //Public fields
    //position
    public float smoothTime = 0.3f;
    public Vector3 moveVector;
    private Vector3 positionEnd;
    private Vector3 positionStart;
    private Vector3 posVelocity = Vector3.zero;
    private Vector3 positionTarget;

    //rotation
    public float rotationSpeed;
    public float rotationExitSpeed;
    private Quaternion rotationEnd;
    private Quaternion rotationStart;

    // Cursor position
    public float cameraSpeed = 0.1f;
    private Vector2 screenCenter;
    public float percentOfScreen = 0.75f;   // the thresh hold, where if the cursor passes it the camera will continuously move
                                            // percentage multiplied to center and then added for comparison
    private Vector4 threshHold;
    private Vector3 target;
    private Vector3 newPosition;
    private Vector3 cameraVelocity = Vector3.zero;
    private Vector2 prevMousePosition;
    private Vector2 mousePosition;

    // Y axis rotation
    private Quaternion rotationTarget;
    private Quaternion startRotation;
    private float yRotation;
    private bool rotating = false;

    // Use this for initialization
    void Start()
    {

        #region Scroll initialize

        // initialize the position variables
        positionStart = Camera.main.transform.localPosition;
        positionEnd = Camera.main.transform.localPosition;
        positionEnd += moveVector;

        // initialize the rotation variables
        rotationStart = transform.rotation;
        rotationEnd = transform.rotation;
        rotationEnd.x = 0;

        #endregion

        #region Move initialize
        // initialize mouse position
        mousePosition = Input.mousePosition;
        prevMousePosition = Input.mousePosition;

        // initialize the screen center
        screenCenter = new Vector2(Screen.width / 2, Screen.height);
        #endregion

        // initialize the starting rotation
        startRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {

        // update the mouse position
        mousePosition = Input.mousePosition;

        #region Middle Mouse rotate

        yRotation = Input.GetAxis("Mouse X") * 50.0f;
        startRotation = transform.rotation;

        // only works if a menu is not displaying
        if (!LoveShack.menuDisplaying)
        {

            if (Input.GetMouseButton(2))
            {
                if (mousePosition.x > prevMousePosition.x)
                {
                    rotationTarget = Quaternion.Euler(transform.eulerAngles.x, 50, transform.eulerAngles.z);
                }
                else if (mousePosition.x < prevMousePosition.x)
                {
                    rotationTarget = Quaternion.Euler(transform.eulerAngles.x, -50, transform.eulerAngles.z);
                }
                // if the middle button is being clicked player should not be able to use the other controls
                rotating = true;

                // if the mouse moves to the right rotate right
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationTarget, Time.deltaTime);
            }


            #endregion

            // TO DO: the end position and the target position need to be different. Clamp the camera zoom to the end and start
            // but the target needs to be relative
            #region Scroll Wheel Zoom
            // when the scroll is moving away from player zoom in
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && (int)transform.position.y > (int)positionEnd.y && !rotating)
            {
                positionTarget = transform.localPosition;
                positionTarget += moveVector;

                // change the position to the specified value by the designer
                transform.position = Vector3.SmoothDamp(transform.localPosition, positionTarget, ref posVelocity, smoothTime);

                // change the rotation to 0 to be parallel with the ground
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationEnd, rotationSpeed * Time.deltaTime);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0 && transform.position.y < positionStart.y && !rotating)
            {
                //revert position and rotation to original values
                transform.position = Vector3.SmoothDamp(transform.position, transform.position - moveVector, ref posVelocity, smoothTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotationStart, rotationExitSpeed * Time.deltaTime);
            }
            #endregion

            #region Move Mouse

            //TO DO: Revamp the moving so that the variable is the percent of screen that the player needs to hover over to move
            // the camera
            //BAM ALL OF THIS CRAP WORKS, TAKE THAT CODE! MY SLINGING ABILITIES ARE TOO GREAT FOR YOU!!!!!!111!
            // initialize the target position
            target = transform.position;

            // initialize the new position to the current position
            newPosition = transform.position;

            // create the thresh hold, Format: right, top, left, bottom
            threshHold = new Vector4(
                screenCenter.x + (screenCenter.x * percentOfScreen),
                Screen.height - (screenCenter.y - (screenCenter.y * percentOfScreen)),
                screenCenter.x - (screenCenter.x * percentOfScreen),
                screenCenter.y - (screenCenter.y * percentOfScreen)
                );

            if (!rotating)
            {
                // MOVE FORWARD
                if (mousePosition.y > threshHold.y)
                {
                    // add to the target z to move forward
                    target.z += cameraSpeed;
                }

                // MOVE BACKWARD
                else if (mousePosition.y < threshHold.w)
                {
                    // subtract from the target z to move backward
                    target.z -= cameraSpeed;
                }

                //MOVE RIGHT
                if (mousePosition.x > threshHold.x)
                {
                    // add to the target z to move forward
                    target.x += cameraSpeed;

                }

                //MOVE LEFT
                else if (mousePosition.x < threshHold.z)
                {
                    // add to the target z to move forward
                    target.x -= cameraSpeed;
                }
            }

            // smooth dap after the newPosition vector has been set and do it all at the same time
            newPosition = Vector3.SmoothDamp(transform.position, target, ref cameraVelocity, smoothTime);

            transform.position = newPosition;
            // update the previous mouse position after the mouse position has updated
            prevMousePosition = mousePosition;
            #endregion

            // set rotating to false at the end of the update
            rotating = false;
        }
    }
}
