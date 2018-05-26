using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReticleController : MonoBehaviour {

    public static ReticleController singleton;
    public Camera main_cam;
    public GameObject truthBulletPrefab, spawnPoint, bulletContainer;
    public List<GameObject> truthBullets;
    private RectTransform thisTransform;
    private float moveFactor, topBoundary, bottomBoundary, leftBoundary, rightBoundary, bulletTimer;

    void Awake()
    {
        singleton = this;
    }

    void Start()
    {
        thisTransform = GetComponent<RectTransform>();
        moveFactor = 5.0f;
        bulletTimer = 5.0f;
        topBoundary = Screen.height * 0.5f;
        bottomBoundary = Screen.height * -0.5f;
        leftBoundary = Screen.width * -0.5f;
        rightBoundary = Screen.width * 0.5f;
    }



	void Update () {
        transform.SetAsLastSibling(); //ensures that its not overlapped by other UI elements
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) reticleMove(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")); //reticle move check
        if (Input.GetKeyDown(KeyCode.Space)) shootBullet();
    }

    private void shootBullet()
    {
        if (truthBullets.Count == 0)
        {
            GameObject bullet = Instantiate(truthBulletPrefab, spawnPoint.transform.position, Quaternion.identity) as GameObject;
            setBulletRotation(bullet);
            bullet.GetComponent<BulletController>().timer = bulletTimer;
            bullet.GetComponent<BulletController>().destination = main_cam.ScreenToWorldPoint(new Vector3(thisTransform.anchoredPosition.x, thisTransform.anchoredPosition.y, 0));
            bullet.SetActive(true);
        } else
        {
            GameObject b = truthBullets[0];
            b.transform.rotation = Quaternion.identity;
            setBulletRotation(b);
            b.transform.position = spawnPoint.transform.position;
            b.SetActive(true);
            b.GetComponent<BulletController>().timer = bulletTimer;
            b.GetComponent<BulletController>().destination = main_cam.ScreenToWorldPoint(new Vector3(thisTransform.anchoredPosition.x, thisTransform.anchoredPosition.y, 0));
            truthBullets.RemoveAt(0);
        }
    }

    private void setBulletRotation (GameObject bullet)
    {
         bullet.transform.Rotate(5, 44, 58);
    }

    private void reticleMove(float horizontalAxis, float verticalAxis) //reticle movement
    {
        Vector2 lastPos = thisTransform.anchoredPosition;
        if (horizontalAxis < -0.1) thisTransform.anchoredPosition = new Vector2(lastPos.x -= moveFactor, lastPos.y);
        if (horizontalAxis > 0.1) thisTransform.anchoredPosition = new Vector2(lastPos.x += moveFactor, lastPos.y);
        if (verticalAxis < -0.1) thisTransform.anchoredPosition = new Vector2(lastPos.x, lastPos.y -= moveFactor);
        if (verticalAxis > 0.1) thisTransform.anchoredPosition = new Vector2(lastPos.x, lastPos.y += moveFactor);
        checkBoundaries(thisTransform.anchoredPosition);
        RaycastHit hit;
        Ray ray = main_cam.ScreenPointToRay(new Vector3(thisTransform.anchoredPosition.x, thisTransform.anchoredPosition.y, 0));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            spawnPoint.transform.position = new Vector3(hit.point.x, hit.point.y, -100);
        }
    }

    private void checkBoundaries(Vector2 currPos) //boundary check to ensure that reticle doesn't go off screen
    {
        //four main sides
        if (currPos.x < leftBoundary) thisTransform.anchoredPosition = new Vector2(leftBoundary, currPos.y);
        if (currPos.x > rightBoundary) thisTransform.anchoredPosition = new Vector2(rightBoundary, currPos.y);
        if (currPos.y < bottomBoundary) thisTransform.anchoredPosition = new Vector2(currPos.x, bottomBoundary);
        if (currPos.y > topBoundary) thisTransform.anchoredPosition = new Vector2(currPos.x, topBoundary);

        //corner checks
        if (currPos.x < leftBoundary && currPos.y > topBoundary) thisTransform.anchoredPosition = new Vector2(leftBoundary, topBoundary);
        if (currPos.x < leftBoundary && currPos.y < bottomBoundary) thisTransform.anchoredPosition = new Vector2(leftBoundary, bottomBoundary);
        if (currPos.x > rightBoundary && currPos.y > topBoundary) thisTransform.anchoredPosition = new Vector2(rightBoundary, topBoundary);
        if (currPos.x > rightBoundary && currPos.y < bottomBoundary) thisTransform.anchoredPosition = new Vector2(rightBoundary, bottomBoundary); 
    }

} //end class