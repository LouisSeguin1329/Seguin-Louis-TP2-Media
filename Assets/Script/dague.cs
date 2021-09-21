using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dague : MonoBehaviour
{
    public Vector2 speed = new Vector2(20, 20); //vitesse de la dague

     public Vector2 direction = new Vector2(1, 0); //direction de la dague

     public Vector2 mouvement;


     [SerializeField] public GameObject monHero;
     [SerializeField] public GameObject Dague;
     [SerializeField] public GameObject monEnnemie;


    [SerializeField] private ParticleSystem particleDague;


    private void FixedUpdate()
    {
        GetComponent<Rigidbody2D>().velocity = mouvement;
    }
    
    void OnCollisionEnter2D(Collision2D laCollision)
    {
        if (laCollision.transform.tag == "Vide" ||laCollision.transform.tag == "Plancher") //détruire la dague si elle entre en collision avec le plancher ou le vide
        {
            Destroy(gameObject);
        }
        if(laCollision.transform.tag == "Ennemie") //Détruire la dague si elle entre en collsion avec un ennemi
        {
            Destroy(gameObject);
        }
        if(laCollision.transform.tag == "Hero") //Si le hero entre en collsion avec la dague, jouer le particle de la dague et faire bouger la dague
        {
            particleDague.Play();
            mouvement = new Vector2(speed.x * direction.x, speed.y * direction.y);
        }
    }
}
