using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{

    public float xSpeed = 3.5f;
    public float zSpeed = 3.5f;
    public float zoomSpeed = 2f;

    public float minX = -30f;
    public float maxX = 40f;
    public float minY = 2f;
    public float maxY = 10f;
    public float minZ = -30f;
    public float maxZ = 40f;

    public float returnSpeed = 1f;
    public float returnSpeedMultiplier = 1.3f;
    public float currentReturnSpeed = 1f;
    public float distanceCheck = 0.5f;
    public float maxSpeed = 20f;

    public Vector3 newPosition;
    private bool returning = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!returning)
        {
            CheckInput();
        }
        else
        {
            float distance = Vector3.Distance(newPosition, transform.position);
            //Debug.Log(distance);
            if (distance > distanceCheck || distance < -distanceCheck)
            {
                Quaternion cameraRot = transform.rotation;
                if (currentReturnSpeed < maxSpeed)
                {
                    currentReturnSpeed *= returnSpeedMultiplier;
                }
                else
                {
                    currentReturnSpeed = maxSpeed;
                }
                transform.LookAt(newPosition);
                transform.Translate(Vector3.forward * currentReturnSpeed * Time.deltaTime);
                transform.rotation = cameraRot;
            }
            else
            {
                transform.position = newPosition;
                returning = false;
            }
        }
    }

    public void SetNewPosition(Vector3 position)
    {
        newPosition = position;
        returning = true;
    }

    private void CheckInput()
    {
        Vector3 newPosition = transform.position;
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += new Vector3(-xSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += new Vector3(xSpeed * Time.deltaTime, 0, 0);
        }
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += new Vector3(0, 0, zSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += new Vector3(0, 0, -zSpeed * Time.deltaTime);
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            transform.Translate(new Vector3(0, scroll * zoomSpeed, scroll * zoomSpeed));
        }

        CheckBoundries();
    }

    private void CheckBoundries()
    {
        Vector3 rightPosition;
        if (transform.position.x > maxX)
        {
            rightPosition = new Vector3(maxX, transform.position.y, transform.position.z);
            transform.position = rightPosition;
        }
        if (transform.position.x < minX)
        {
            rightPosition = new Vector3(minX, transform.position.y, transform.position.z);
            transform.position = rightPosition;
        }
        if (transform.position.y > maxY)
        {
            rightPosition = new Vector3(transform.position.x, maxY, transform.position.z);
            transform.position = rightPosition;
        }
        if (transform.position.y < minY)
        {
            rightPosition = new Vector3(transform.position.x, minY, transform.position.z);
            transform.position = rightPosition;
        }
        if (transform.position.z > maxZ)
        {
            rightPosition = new Vector3(transform.position.x, transform.position.y, maxZ);
            transform.position = rightPosition;
        }
        if (transform.position.z < minZ)
        {
            rightPosition = new Vector3(transform.position.x, transform.position.y, minZ);
            transform.position = rightPosition;
        }
    }
}
