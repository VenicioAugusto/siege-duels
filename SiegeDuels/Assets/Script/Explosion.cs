using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour {

    [SerializeField] private float delay = 3f;
    [SerializeField] private float explosionRadius = 1f;
    [SerializeField] private float force = 700f;   

    [SerializeField] private GameObject explosionEffect;

    float countdown;
    bool hasExploded = false;


    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;
    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if(countdown <= 0f && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }

        
    }

    void Explode()
    {
        Vector3 bombposition = transform.position;
        Quaternion bombrotation = transform.rotation;

        Instantiate(explosionEffect, bombposition, bombrotation);
        Debug.Log("Boom");

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);



        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.transform.gameObject.layer == 9)
            {
                nearbyObject.gameObject.SetActive(false);
            }

            if (nearbyObject.transform.gameObject.layer == 6)
            {
                //nearbyObject.gameObject.SetActive(false);
            }

            //float radius = nearbyObject.GetComponent<SphereCollider>().radius;
            //float distObjects = Vector3.Distance(bombposition, nearbyObject.transform.position);

            //float death = radius + distObjects;

            //if (death < explosionRadius)
            //{
            //nearbyObject.gameObject.SetActive(false);
            //}
        }        

        Destroy(gameObject);
    }
}
