	using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DoraemonMovement : MonoBehaviour
{

    [SerializeField]
    float forceX;
    [SerializeField]
    float forceY;
    Rigidbody2D rigit;
    float positionX, positionY, camWidth;
    // Start is called before the first frame update
    void Start()
    {
        forceX = 400f;
        forceY = 300f;
        positionX = transform.position.x;
        positionY = transform.position.y;
        float gOWidth = gameObject.GetComponent<SpriteRenderer>().bounds.size.x / 2;
		camWidth = Camera.main.orthographicSize * Camera.main.aspect - gOWidth;
        positionX = transform.position.x;
        positionY = transform.position.y;
		rigit = gameObject.GetComponent<Rigidbody2D>();
		rigit.freezeRotation = true;
	}

    // Update is called once per frame
    void Update()
    {
		positionX = transform.position.x;
		if (positionX > camWidth)
        {
            transform.position = new Vector3(camWidth, transform.position.y, transform.position.z);


		}
		else if(positionX < -camWidth)
        {
            transform.position = new Vector3(-camWidth, transform.position.y, transform.position.z);


		}
		if (Input.GetKeyDown(KeyCode.RightArrow))
        {
			// GO di sang phai, tac dong luc theo phuong (1, 0 ,0)

			if (transform.localScale.x < 0)
			{
				transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
			}
			rigit.AddForce(forceX * Vector2.right);
        }
        else if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (transform.localScale.x > 0)
            {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
			rigit.AddForce(forceX * Vector2.left);

		} else if(Input.GetKeyDown(KeyCode.Space))
        {
			rigit.AddForce(forceY * Vector2.up);

		}

	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		Debug.Log("Bat dau va cham voi: " + collision.gameObject.name);	
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		Debug.Log("Van dang cham voi:" + collision.gameObject.name);

	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		Debug.Log("Ket thuc va cham voi:" + collision.gameObject.name);

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Debug.Log("da cham vao: " + collision.gameObject.name);
	}


}
