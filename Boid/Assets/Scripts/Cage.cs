using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Cage : MonoBehaviour {

	[SerializeField]
    GameObject _origin;

    [SerializeField]
    int _num;

    [SerializeField]
    float _cageSize;

    [SerializeField]
    float _groupDistance;
    
    [SerializeField]
    float _speedMin;
    
    [SerializeField]
    float _speedMax;


    List<Bird> _birds = new List<Bird>();
    

    // Use this for initialization
    void Start () {
        for (var i = 0; i < _num; i++) {
            var newChar = Instantiate(_origin,gameObject.transform);
            newChar.name = $"bird {i:d2}";

            newChar.transform.localPosition = RandomPosition(-_cageSize/2, _cageSize/2);
            var bird = newChar.GetComponent<Bird>();
            bird.setRotation(RandomRotate());
            bird.id = i;
            bird.speedMin = _speedMin;
            bird.speedMax = _speedMax;
            bird.sensitivity = Random.Range(0.5f,1.5f);
            bird.speed = Random.Range(_speedMin, _speedMax);
            bird.cageSize = _cageSize / 2;
            _birds.Add(bird);

        }

        _origin.SetActive(false);
    }

	Vector3 RandomPosition(float min, float max) {
        return new Vector3(
            Random.Range(min, max),
            Random.Range(min, max),
            Random.Range(min, max)
        );
    }

	Quaternion RandomRotate() {
        return Quaternion.Euler(
            Random.Range(0f, 360f),
            Random.Range(0f, 360f),
            0f
        );
    }
	
	// Update is called once per frame
	void Update () {

        foreach(var b in _birds) {
            b.otherBirds.Clear();
            foreach (var bb in _birds) {
                if (bb == b) { continue; }
                var distance = Vector3.Distance(
                    b.transform.localPosition,
                    bb.transform.localPosition);

                if (distance < _groupDistance * b.sensitivity * b.power) {
                    b.otherBirds.Add(bb);
                }
            }
        }

        foreach( var b in _birds) {
            b.DecideAngle();
        }
        
    }
}
