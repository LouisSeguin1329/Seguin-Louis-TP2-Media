using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ennemie : MonoBehaviour
{
    private Mouvement varScriptMouvement;
    [SerializeField] public GameObject monEnnemie;
    private Animator anim_Ennemie;
    [SerializeField] public GameObject monHero;

    public float delay = -4f;
    string laScene;




    void Start()
    {
        varScriptMouvement = monEnnemie.GetComponent<Mouvement>();
        anim_Ennemie = GetComponent<Animator>();
        laScene = laScene = SceneManager.GetActiveScene().name;
      
    }

    // Update is called once per frame
    void Update()
    {
        VerifieNbrObjet();
      
     
    }

    void VerifieNbrObjet(){

        if (varScriptMouvement.objetPris == 50 && laScene == "Jeu" ) //si objetPris = 50, activer l'animation EnnemiMeurt
        {

            anim_Ennemie.SetTrigger("EnnemiMeurt");
           
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.transform.tag == "Dague") //Si la dague entre en collsion avec un ennemi, jouer l'animation EnnemiPeutMourir et détruire le gameObject de l'ennemi
        {
            anim_Ennemie.SetTrigger("EnnemiPeutMourir");
             Destroy(gameObject, this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay);

      
        }
    }

  
}
