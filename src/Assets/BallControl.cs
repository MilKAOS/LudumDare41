using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 vel;
    private AudioSource audioSource;
    private AudioClip bipClip;

    // Use this for initialization
    void Start ()
    {
        rb2d = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        Invoke("GoBall", 2);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    void RestartGame()
    {
        ResetBall();
        Invoke("GoBall", 1);
    }

    void ResetBall()
    {
        vel = Vector2.zero;
        rb2d.velocity = vel;
        transform.position = Vector2.zero;
    }

    void GoBall()
    {
        int randQuartant = Random.Range(0, 4);
        int randAngleX = Random.Range(0, 51);
        int randAngleY = Random.Range(0, 51);
        int randScale = Random.Range(15, 30);

        Vector2 randomizedDirection = new Vector2();

        switch (randQuartant)
        {
            case 0:
                randomizedDirection = new Vector2(20 + randAngleX, 20 + randAngleY).normalized;
                rb2d.gameObject.transform.rotation = new Quaternion(0, -1, 0, 0);
                break;

            case 1:
                randomizedDirection = new Vector2(20 + randAngleX, - 20 - randAngleY).normalized;
                rb2d.gameObject.transform.rotation = new Quaternion(0, -1, 0, 0);
                break;

            case 2:
                randomizedDirection = new Vector2(- 20 - randAngleX, -20 - randAngleY).normalized;
                rb2d.gameObject.transform.rotation = new Quaternion(0, 0, 0, 1);
                break;

            case 3:
                randomizedDirection = new Vector2( -20 - randAngleX, 20 + randAngleY).normalized;
                rb2d.gameObject.transform.rotation = new Quaternion(0, 0, 0, 1);
                break;
        }

        randomizedDirection.Scale(new Vector2(randScale, randScale));
        rb2d.AddForce(randomizedDirection);        
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            bipClip = Resources.Load<AudioClip>(string.Format("{0}", GetPaddleCollisionSoundName()));
            audioSource.clip = bipClip;
            audioSource.Play();

            vel.x = rb2d.velocity.x;
            vel.y = (rb2d.velocity.y / 2.0f) + (coll.collider.attachedRigidbody.velocity.y / 3.0f);
            rb2d.velocity = vel;
        }
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            if (rb2d.velocity.x > 0)
                rb2d.gameObject.transform.rotation = new Quaternion(0, -1, 0, 0);
            else
                rb2d.gameObject.gameObject.transform.rotation = new Quaternion(0, 0, 0, 1);
        }
    }

    private string GetPaddleCollisionSoundName()
    {
        var rndSound = Random.Range(0, 4);
        switch (rndSound)
        {
            case 0:
                return "bip01";
            case 1:
                return "bip02";
            case 2:
                return "bip03";
            case 3:
                return "bip04";
            default:
                throw new System.ArgumentException("sound missing");
        }
    }
}
