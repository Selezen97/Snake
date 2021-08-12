using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeController : MonoBehaviour
{
    public List<Transform> Tails;
    [Range(0,3)] public float BonesDistance;
    public GameObject BonePrefab;
    [Range(0, 4)] public float Speed;
    private Transform _transform;
    public UnityEvent OnEat;
    public GameObject lose,win;
    public GameObject ApplePrefab;
    public Text _score;

    private void Awake()
    {
        for (int i = 0; i < 25; i++)
        {
            Instantiate(ApplePrefab, new Vector3(UnityEngine.Random.Range(-18, 18), 0.4f, UnityEngine.Random.Range(2, 38) ) , Quaternion.identity);
        }
    }

    private void Start()
    {
        _transform = GetComponent<Transform>();
    }

    private void FixedUpdate()
    {
        MoveSnake(_transform.position + _transform.forward*Speed);
        _transform.Rotate(0,Input.GetAxis("Horizontal")*4,0);
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }

    private void MoveSnake(Vector3 newPosition)
    {
        float sqrDistance = BonesDistance * BonesDistance;
        Vector3 previousPosition = _transform.position;
        foreach (var bone in Tails)
        {
            if ((bone.position - previousPosition).sqrMagnitude>sqrDistance)
            {
                var temp = bone.position;
                bone.position = previousPosition;
                previousPosition = temp;
            }
            else
            {
                break;
            }
        }
        _transform.position = newPosition;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Food")
        {
            Destroy(collision.gameObject);
            var bone = Instantiate(BonePrefab);
            if (Tails.Count > 5)
            bone.tag = "Enemy";
            Tails.Add(bone.transform);
            _score.text= (int.Parse(_score.text)+1).ToString();
            if (int.Parse(_score.text) == 25)
            {
                win.SetActive(true);
                Speed = 0;
                Invoke("GoMenu", 2.0f);
            }
            if (OnEat != null)
                OnEat.Invoke();
        }
        else
        if (collision.gameObject.tag == "Wall")
        {
            lose.SetActive(true);
            Speed = 0;
            Invoke("GoMenu", 2.0f);
        }
        else if (Tails.Count > 5 && collision.gameObject.tag == "Enemy")
        {
            lose.SetActive(true);
            Speed = 0;
            Invoke("GoMenu", 2.0f);
        }
    }

    private void GoMenu()
    {
        SceneManager.LoadScene(0);
    }
}
