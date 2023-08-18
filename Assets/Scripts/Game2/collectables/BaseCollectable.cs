using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class BaseCollectable : MonoBehaviour , Icollectable
{   
    [SerializeField] private ParticleSystem _collectableParticle;
    public void OnCollect()
    {
        Instantiate(_collectableParticle.gameObject , transform.position , quaternion.identity);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("player"))
        {
            OnCollect();
            Destroy(gameObject);
        }
    }
}
