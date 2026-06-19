using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Canon : MonoBehaviour
{
    public GameObject bullet;
    public ParticleSystem partical;
    public Canvas title_canvas, game_canvas, finish_canvas;
    bool end_flag = true;
    float time_d = 10f;
    System.DateTime last_dt;
    Rigidbody bullet_rb;

    void Start()
    {
        last_dt = System.DateTime.UtcNow;
        bullet_rb = bullet.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (IsFinished())
        {
            return;
        }
        System.DateTime dt = System.DateTime.UtcNow;
        System.TimeSpan ts = dt - last_dt;
        if (ts.TotalSeconds > time_d)
        {
            CreateCube();
            last_dt = dt;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
    }

    private void FixedUpdate()
    {
        if (IsFinished())
        {
            return;
        }
        float x = Input.GetAxis("Vertical");
        float y = Input.GetAxis("Horizontal");
        Vector3 v = transform.rotation.eulerAngles;
        v.z -= x;
        v.y += y;
        if (v.z < 0)
        {
            v.z = 0;
        }
        transform.rotation = Quaternion.Euler(v);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsFinished())
        {
            return;
        }
        if (other.tag == "Enemy")
        {
            partical.transform.position = other.transform.position;
            partical.Play();
            GameOver();
        }
    }
    void Initial()
    {
        end_flag = false;
    }
    public void ChangeTime()
    {
        time_d -= 0.1f;
        if (time_d < 1f)
        {
            time_d = 1f;
        }
    }
    public bool IsFinished()
    {
        Debug.Log(end_flag);
        return end_flag;
    }

    public void OnStartBtnClick()
    {
        title_canvas.enabled = false;
        game_canvas.enabled = true;
        Initial();
    }

    public void GameOver()
    {
        end_flag = true;
        finish_canvas.enabled = true;
    }

    void CreateCube()
    {
        float rr = Random.value * 50 + 50;
        float ra = Random.value * Mathf.PI * 2;
        float rx = Mathf.Sin(ra) * rr + 250;
        float rz = Mathf.Cos(ra) * rr + 250;
        float ry = Random.value * 90 + 10;

        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.tag = "Enemy";
        cube.transform.position = new Vector3(rx, ry, rz);
        cube.transform.localScale = new Vector3(10f, 10f, 10f);

        Rigidbody rb = cube.AddComponent<Rigidbody>();
        rb.useGravity = false;
        rb.isKinematic = false;
        Renderer rd = cube.GetComponent<Renderer>();
        rd.material.color = Color.red;

        MoveCube(rb);
    }

    void MoveCube(Rigidbody rb)
    {
        float tx = Random.value * 200f - 100f;
        float ty = Random.value * 600f - 300f;
        float tz = Random.value * 1000f - 500f;
        rb.AddTorque(new Vector3(tx, ty, tz));

        Vector3 p1 = transform.position;
        Vector3 p2 = rb.transform.position;
        Vector3 p3 = (p1 - p2);
        rb.AddForce(p3);
    }
    void Fire()
    {
        bullet.transform.position = transform.position;
        bullet_rb.linearVelocity = Vector3.zero;
        bullet_rb.angularVelocity = Vector3.zero;
        Vector3 v = transform.rotation.eulerAngles;
        float rd = (v.y + 90) * Mathf.Deg2Rad;
        float rd2 = (v.z + 30) * Mathf.Deg2Rad;
        float dx = Mathf.Sin(rd);
        float dz = Mathf.Cos(rd);
        float dy = Mathf.Sin(rd2);
        Vector3 nv = new Vector3(dx, dy, dz);
        nv *= 1000f;
        bullet_rb.useGravity = true;
        bullet_rb.AddForce(nv);
    }
}