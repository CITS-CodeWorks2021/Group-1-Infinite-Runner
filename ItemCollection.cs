using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ItemCollection : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public int ring = 0;
    [SerializeField] private Text coinText;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("hazard"))
        {
            if (ring > 0)
            {
                ring = 0;
                Damage();
                coinText.text = "ring:" + -ring;
            }
            else if (ring < 1)
            {
                Die();
            }
        }
    }
    private void Damage()
    {
        anim.SetTrigger("damage");
        rb.velocity = (new Vector2(-70, rb.velocity.y));
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    private void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        anim.SetTrigger("death");
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("ring"))
        {
            Destroy(collision.gameObject);
            ring++;
            coinText.text = "ring:" + ring;
        }
    }
}

