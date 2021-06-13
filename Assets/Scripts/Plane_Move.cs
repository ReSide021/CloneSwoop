using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Plane_Move : MonoBehaviour
{
    [SerializeField] private GameObject _plane;
    [SerializeField] private float _speed = 0.5f;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _moduleForce = 1;
    [SerializeField] private GameObject _gameOverUi;
    [SerializeField] private Text _textScore;
    [SerializeField] private GameObject _propeller;
    [SerializeField] private Slider _slider;
    [SerializeField] private Image _fill;
    private int score = 0;
    private float radius;
    private float angle = 0f;
    private Vector3 defaultPosition;
    private float fuelSize = 100f;
    private float currentFuel;
    private GameObject fillGameObject;
    // Start is called before the first frame update
    void Start()
    {
        defaultPosition = _plane.transform.position;
        radius = Mathf.Sqrt(Mathf.Pow(_plane.transform.position.x, 2) + Mathf.Pow(_plane.transform.position.y-22, 2) + Mathf.Pow(_plane.transform.position.z, 2));
        currentFuel = fuelSize;
        fillGameObject = GameObject.Find("Fill Area");
    }

    private void FixedUpdate()
    {
        if (currentFuel <= 0)
        {
            fillGameObject.SetActive(false);
        }
        _slider.value = currentFuel;
        var x = Mathf.Cos(angle * _speed) * radius;
        var z = Mathf.Sin(angle * _speed) * radius;
        transform.position = new Vector3(x, _plane.transform.position.y, z);
        angle = angle + Time.deltaTime * _speed;
        _propeller.transform.Rotate(Vector3.right, Time.deltaTime * 1000f, Space.Self);
        _fill.fillAmount = _slider.value;
        if (angle > 360f)
        {
            angle = 0f;
        }

        if (_plane.transform.position.y > 36f)
        {
            _rigidbody.velocity = Vector3.down;
        }
        else 
        {
            if (_plane.transform.position.y <= 29f)
            {
                if (currentFuel > 0)
                {
                    if (Input.GetMouseButton(0))
                    {
                        currentFuel -= 8 * Time.deltaTime;
                        _rigidbody.AddForce(Vector3.up * _moduleForce);
                    }
                }
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
        {
            //_plane.transform.position = defaultPosition;
            Time.timeScale = 0f;
            _gameOverUi.transform.GetChild(0).transform.GetChild(3).transform.GetChild(0).GetComponent<Text>().text = score.ToString();
            _gameOverUi.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Cloud") {
            //Time.timeScale = 0f;
            other.gameObject.SetActive(false);
            currentFuel -= 20;
        }

        if (other.gameObject.tag == "Star")
        {
            //Time.timeScale = 0f;
            score += 2;
            other.gameObject.SetActive(false);
            _textScore.text = score.ToString();
        }
    }
}
