using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float kekuatanLompatan = 5f;
	[SerializeField] private Animator animator;
	[SerializeField] private TrailRenderer tr;
	[SerializeField] private int JumlahLoncat = 2;
	[SerializeField] private float kekuatanDash = 8f;
	private bool bisaDash = true;
	private bool isDash;
	private float cooldownDash = 0.2f;
    private float moveX;
    private bool bisaLoncat = true;
	
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if(isDash)
		{
			return;
		}
		
        // gerak kiri kanan
        moveX = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveX * speed, rb.velocity.y);

        // hadap kiri kanan
        if(!Mathf.Approximately(0, moveX))
        {
            transform.rotation = moveX > 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
        }
		
		if(JumlahLoncat == 2)
		{
			bisaLoncat = true;
			
        }
		
		if(bisaLoncat)
			{
				if (Input.GetKeyDown(KeyCode.S))
				{
					if(JumlahLoncat == 1)
					{
						animator.ResetTrigger("isIdle");
						animator.ResetTrigger("isRun");
						animator.SetTrigger("isJump");
						
					}
					Jump();
				}
			}
		
		if(bisaDash)
		{
			if(Input.GetKeyDown(KeyCode.D))
			{
				StartCoroutine(Dash());
			}
		}
		// animasi
		if(moveX == 0)
		{
			animator.SetTrigger("isIdle");
			animator.ResetTrigger("isRun");
		}
		else if(moveX != 0)
		{
			animator.SetTrigger("isRun");
			animator.ResetTrigger("isIdle");
		}
		
		
        //lompat
		
		
		
		if(JumlahLoncat == 0)
		{
			bisaLoncat = false;
		}
    }
	
	void FixedUpdate()
	{
		if(isDash)
		{
			return;
		}
	}

    void OnCollisionEnter2D (Collision2D other)
    {
        if(other.collider.tag == "Terrain")
        {
            JumlahLoncat = 2;
			animator.ResetTrigger("isJump");
        }
    }
	
	void Jump()
	{
		rb.AddForce(new Vector2(0f, kekuatanLompatan), ForceMode2D.Impulse);
        JumlahLoncat -= 1;
		
	}
	
	IEnumerator Dash()
	{
		bisaDash = false;
		isDash= true;
		float gravitasiOriginal = rb.gravityScale;
		rb.gravityScale = 0f;
		rb.velocity = new Vector2(moveX * kekuatanDash, 0f);
		tr.emitting = true;
		yield return new WaitForSeconds(0.2f);
		tr.emitting = false;
		rb.gravityScale = gravitasiOriginal;
		isDash = false;
		yield return new WaitForSeconds(cooldownDash);
		bisaDash = true;
	}
}
