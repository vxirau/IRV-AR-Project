
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class targetcollider : MonoBehaviour
{
    float speed = 1f;
    float delta = 3f;
    public int decider=1;
	public Slider lifeBar;
    public bool hasLifeBar;
    public bool flag;
    public float currentHealth;
    public float totalHealth=12;
    public float initialpos;
    public AudioSource hitsound;
    public GameObject explosiontime;
    public ExampleSpooky spooky;
    public bool isArcade;
    public bool respawnCooldown;
    public int counter;

    public GameObject previousArrow;

    public GameObject[] arrows;
    public int index = 0;
    public bool isFirst=true;

    public IEnumerator destroy(GameObject missile)
    {
        yield return new WaitForSeconds(3f);
        Destroy(missile);
    }
    // Start is called before the first frame update
    void Start()
    {
        initialpos = transform.position.x;
    }
      void OnCollisionEnter(Collision collision)
    {
        hitsound.Play();
        if(collision.collider.tag=="tip")
        {
            collision.collider.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            collision.collider.gameObject.transform.parent = this.transform;
            currentHealth = currentHealth - 1;
            if(hasLifeBar){
                lifeBar.value = currentHealth;
            }
            if(!isFirst){
                Destroy(previousArrow);
            }
            isFirst=false;
            previousArrow = collision.collider.gameObject;
        }
        if (collision.collider.tag == "rocket")
        {
            collision.collider.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            collision.collider.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            collision.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
            collision.collider.gameObject.transform.GetChild(1).gameObject.SetActive(false);
            StartCoroutine(destroy(collision.collider.gameObject));
            currentHealth= currentHealth - 3;
            if(hasLifeBar){
                lifeBar.value = currentHealth;
            }
        }
        if(currentHealth<=0)
        {
            
            collision.collider.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>().Play();
            
            
        }
    }
    // Update is called once per frame
    void Update()
    {
       
        if (flag)
        {
            transform.LookAt(Camera.main.transform);

            if (currentHealth > 0)
            {
                float y = Mathf.PingPong(speed * Time.time, delta);
                // decider = decider * -1; 
                Vector3 pos = new Vector3(initialpos + Mathf.Sin(Time.time * 1f) * decider * y, y, transform.position.z);
                transform.position = pos;
            }
            else
            {
                if(!respawnCooldown){

                    var go = Instantiate(explosiontime, this.transform);
                    go.SetActive(true);
                    go.transform.parent = GameObject.FindGameObjectWithTag("Ball").transform;
                    //Destroy(this.gameObject);
                    respawnCooldown=true;
                    counter=0;
                }
            }

            if(respawnCooldown){
                counter++;
                if(counter == 10){
                    respawnCooldown = false;
                    counter=0;
                    if(isArcade){
                        spooky.newLevel();
                        Destroy(previousArrow);
                        totalHealth = totalHealth + 12;
                        currentHealth = totalHealth;
                        if(hasLifeBar){
                            lifeBar.maxValue = totalHealth;
                            lifeBar.value = currentHealth;
                        }
                    }
                }
            }
        }
    }
}
