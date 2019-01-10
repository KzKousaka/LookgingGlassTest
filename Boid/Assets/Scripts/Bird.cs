using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {

	
    public int id;
	public float speed;
    public float cageSize;
    public float speedMax;
    public float speedMin;
    public float sensitivity;
    public Quaternion _nextRotate = Quaternion.identity;

    float _power = 1f;
    public float power {
        get {return _power;}
    }

    Bird _nearBird;
	public Bird nearBird {
        get { return _nearBird; }
        set { _nearBird = value; }
    }
    List<Bird> _otherBirds = new List<Bird>();
	public List<Bird> otherBirds {
        get { return _otherBirds; }
        set { _otherBirds = value; }
    }

    public void setRotation(Quaternion r) {
        _nextRotate = r;
        transform.localRotation = r;
    }

    float _accel = 0;


    public Vector3 direction {
        get { return transform.localRotation * Vector3.up; }
    }

    Quaternion _lastRotate = Quaternion.identity;


    public void DecideAngle() {

        Vector3 targetDirect;

        if (_otherBirds.Count > 0 && transform.localPosition.magnitude < 7f) {
            Vector3 d = Vector3.zero;
            Vector3 groupCenter = Vector3.zero;
            int near = 0;

            bool topFly = true;
            foreach(var b in _otherBirds) {
                groupCenter += b.transform.localPosition;
                d += b.direction;
                var dd = b.transform.localPosition - transform.localPosition;
                if(Vector3.Angle(direction,dd) < 90) {
                    topFly = false;
                }

                

                Debug.DrawLine(transform.position, b.transform.position, Color.white);
            }
            groupCenter /= _otherBirds.Count;
            var groupDirect = d / _otherBirds.Count;
            var centerDirect = -transform.localPosition;
            var groupCenterDirect = groupCenter - transform.localPosition;

            targetDirect = Vector3.Slerp(groupDirect, centerDirect, 0.2f);
            targetDirect = Vector3.Slerp(targetDirect, groupCenterDirect, 0.5f);
            targetDirect.Normalize();

            var r = Quaternion.FromToRotation(direction, targetDirect);
            _lastRotate = Quaternion.Lerp(
                transform.localRotation, transform.localRotation * r, 0.2f * sensitivity * _power);
            _nextRotate = _lastRotate;

            if ( topFly ) {
                _accel -= 0.1f;
            }else{
                _accel += 0.2f;
            }

            _accel = Mathf.Clamp(_accel, -1,1);
        }else{
            _accel -= 0.001f;
            var centerDirect = -transform.localPosition;
            if(Vector3.Angle(direction, centerDirect) > 60f) {
                // Vector3 d = direction;
                // var r = Quaternion.FromToRotation(direction, centerDirect);
                // _nextRotate = Quaternion.Slerp(transform.localRotation, transform.localRotation * r, 0.8f);
                _nextRotate *= Quaternion.Euler(Random.Range(2f,10f), 0f, Random.Range(2f,10f));
            }
        }
    }

    // Use this for initialization
    void Start () {
		
	}

	// Update is called once per frame
	void Update () {
        Step();
    }

	void Step() {
        transform.localRotation = _nextRotate;
		var p = transform.localPosition;
        speed += _accel;
        speed = Mathf.Clamp(speed + _accel, speedMin, speedMax);
        _power -= speed / 85f;
        _power = Mathf.Clamp(_power, 0, 1);
        var s = speed * _power * Time.deltaTime;
        _power += 0.05f - s;
        _power = Mathf.Clamp(_power, 0, 1);
        p += direction * s;
        transform.localPosition = p;
	}
}
