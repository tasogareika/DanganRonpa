using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float timer;
    private float speed;
    public Vector3 destination;

    void Start()
    {
        speed = 100;
    }

	void Update () {
        if (gameObject.activeInHierarchy)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                float step = speed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, destination, step);
                /*Vector3 lastPos = transform.position;
                lastPos.z += 10;
                gameObject.transform.position = lastPos;*/
            } else
            {
                gameObject.SetActive(false);
                ReticleController.singleton.truthBullets.Add(this.gameObject);
                gameObject.transform.SetParent(ReticleController.singleton.bulletContainer.transform);
            }
        }
	}
}
