using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 10f;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;

    public bool CameraEnabled = false;

    public GameObject field;
    private GameObject[,,] ar = new GameObject[3, 3, 3];
    private FieldScript fieldScript; 


    // Use this for initialization
    void Start()
    {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;

        fieldScript = field.GetComponent<FieldScript>(); 
    }


    void Update()
    {
        RaycastHit hit;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        fieldScript.StartHidingBoxes();

        if (Physics.Raycast(ray, out hit))
        {
            Transform objectHit = hit.transform;

            BoxScript boxScript = objectHit.gameObject.GetComponent<BoxScript>();

            boxScript.StartGlowing(); 
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Camera.main.orthographic)
            {
                Camera.main.orthographic = false;
            }
            else
            {
                Camera.main.orthographic = true;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            CameraEnabled = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            CameraEnabled = false;
        }

        if (CameraEnabled)
        {

            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                _LocalRotation.y += Input.GetAxis("Mouse Y") * -MouseSensitivity;

                //Clamp the y Rotation to horizon and not flipping over at the top
                //if (_LocalRotation.y < 0f)
                //    _LocalRotation.y = 0f;
                //else if (_LocalRotation.y > 90f)
                //    _LocalRotation.y = 90f;
            }
        }

        //Zooming Input from our Mouse Scroll Wheel
        if (Input.GetAxis("Mouse ScrollWheel") != 0f)
        {
            if (!Camera.main.orthographic)
            {
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

                ScrollAmount *= (this._CameraDistance * 2f);

                this._CameraDistance += ScrollAmount * -1f;

                this._CameraDistance = Mathf.Clamp(this._CameraDistance, 1.5f, 100f);
            } 
            else
            {
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

                Camera.main.orthographicSize += ScrollAmount * -1f;
                Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, 1f, 100f);
            }
        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        if (this._XForm_Camera.localPosition.z != this._CameraDistance * -1f)
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    }
}
