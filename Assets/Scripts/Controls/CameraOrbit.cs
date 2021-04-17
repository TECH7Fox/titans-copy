using System.Collections;
using UnityEngine;

public class CameraOrbit : MonoBehaviour 
{

    protected Transform _XForm_Camera;
    protected Transform _XForm_Parent;

    protected Vector3 _LocalRotation;
    protected float _CameraDistance = 10f;
    protected bool MouseButtonDown;

    public float MouseSensitivity = 4f;
    public float ScrollSensitvity = 2f;
    public float OrbitDampening = 10f;
    public float ScrollDampening = 6f;
    public float MinDistance = 75f;
    public float MaxDistance = 300f;
    public float ScrollPower = 100;

    public bool CameraDisabled = false;


    // Use this for initialization
    void Start() {
        this._XForm_Camera = this.transform;
        this._XForm_Parent = this.transform.parent;
    }


    void LateUpdate() {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            CameraDisabled = !CameraDisabled;

        if (Input.GetMouseButtonDown(2))
            MouseButtonDown = true;

        if (Input.GetMouseButtonUp(2))
            MouseButtonDown = false;

        if (!CameraDisabled)
        {
            //Rotation of the Camera based on Mouse Coordinates
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                if (MouseButtonDown)
                {
                    _LocalRotation.x += Input.GetAxis("Mouse X") * (_CameraDistance / ScrollPower) * MouseSensitivity;
                    _LocalRotation.y -= Input.GetAxis("Mouse Y") * (_CameraDistance / ScrollPower) * MouseSensitivity;
                }                
            }

            //Zooming Input from our Mouse Scroll Wheel
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float ScrollAmount = Input.GetAxis("Mouse ScrollWheel") * ScrollSensitvity;

                ScrollAmount *= (this._CameraDistance * 0.3f);

                this._CameraDistance += ScrollAmount * -1f;

                this._CameraDistance = Mathf.Clamp(this._CameraDistance, MinDistance, MaxDistance);
            }
        }

        //Actual Camera Rig Transformations
        Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
        this._XForm_Parent.rotation = Quaternion.Lerp(this._XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

        if ( this._XForm_Camera.localPosition.z != this._CameraDistance * -1f )
        {
            this._XForm_Camera.localPosition = new Vector3(0f, 0f, Mathf.Lerp(this._XForm_Camera.localPosition.z, this._CameraDistance * -1f, Time.deltaTime * ScrollDampening));
        }
    }
}