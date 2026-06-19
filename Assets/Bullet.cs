using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Bullet : MonoBehaviour
{
    public Canon canonScript;
    public GameObject canon;
    public TMP_Text score;
    public ParticleSystem particle;
    Rigidbody rb;
    int score_point = 0;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
    }
    void DestroyEnemy(Collision collision)
    {
        particle.Stop();
        particle.transform.position = collision.transform.position;
        particle.Play();
        GameObject.Destroy(collision.gameObject);
        ResetBullet();
        score_point += 100;
        score.text = "" + score_point;
        canonScript.ChangeTime();
    }
    public void ResetBullet()
    {
        rb.useGravity = false;
        Vector3 v = canon.transform.position;
        v.y = 5;
        transform.position = v;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (canonScript.IsFinished())
        {
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            DestroyEnemy(collision);
        }
    }
}