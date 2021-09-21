using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Mouvement : MonoBehaviour
{
    //-----Vitesse de bouger et sauter----------//
    [SerializeField] private float vitesseBouger = 3f;
    [SerializeField] private float vitesseSaute = 6.5f;

    private bool peutBougerPerso = true;

    //-----Hero component----------------------//
    public Animator animationHero;
    private Rigidbody2D rigidBHero;
    private Collider2D colliderHero;

    bool heroBouge;

    public bool directionHero;
    public bool directionHeroG;


    //-----Variable Texte Game Over et Victoire-------//

    [SerializeField] public GameObject Hero;


    //----------------------------------------------------//
    public float objetPris = 0f;

    [SerializeField] public GameObject laPlateforme;
    [SerializeField] public GameObject laPlateforme2;
    [SerializeField] public GameObject laPlateforme3;

    private bool finDePartie = false;
     public bool victoiree;
     public bool defaitee;


    private GameManager scriptGameManager;
    [SerializeField] public GameObject monEnnemie;
    [SerializeField] public GameObject monGestionnaire;
    //--------------Audios-----------------------------------//

    [SerializeField] private AudioClip audioJump = null;
    [SerializeField] private AudioClip audioCollect = null;
    [SerializeField] private AudioClip audioMeurt = null;
    [SerializeField] private AudioClip audioGagne = null;

    private AudioSource heroAudioSource;

    string laScene;












    void Start()
    {

        animationHero = GetComponent<Animator>();
        rigidBHero = GetComponent<Rigidbody2D>();
        colliderHero = GetComponent<Collider2D>();

        heroAudioSource = GetComponent<AudioSource>();
       
        scriptGameManager = monGestionnaire.GetComponent<GameManager>(); //Variable pour aller chercher des variables dans le script GameManager
       
        laScene = SceneManager.GetActiveScene().name;
    }
    void Update()
    {

        Deplacement();
        FlipHero();
        Saute();

        NombreObjetPris();
    }


    //---------Bouger Hero de gauche à droite---------------------------------------------------//
    void Deplacement() {

        if (peutBougerPerso != false) 
        {

            float bougerHorizontal = Input.GetAxis("Horizontal"); //La variable vas chercher le Input Horizontal dans Unity qui équivaut au flèches directionelles
            float deplacementX = bougerHorizontal * vitesseBouger; // Initier la var deplacementX à la variable bougerHorizontal (Flèches) * la var vitesseBouger qui est la vitesse de déplacement du Héro
            rigidBHero.velocity = new Vector2(deplacementX, rigidBHero.velocity.y); //modifié la velocité du rigidbody du héro avec un Vector2(DeplacementX et la velocité du rigidbody)


            heroBouge = rigidBHero.velocity.x != 0;

            animationHero.SetBool("PeutMarcher", heroBouge); // pour que le hero ait l'animation quand il marche
        }
    }


    //-------------Flip du Hero-------------------------------------------------------------//
    void FlipHero() {

        bool heroBouge = rigidBHero.velocity.x != 0;



        if (heroBouge)
        {
            //Arrondir le 1 ou -1, pour ne pas avoir les 0.25, 0.5, etc..
            //le scale du hero passe a 3 lorsqu'il regarde a droite et -3 lorsqu'il regarde à gauche

            transform.localScale = new Vector2(Mathf.Sign(rigidBHero.velocity.x) * 3, 3);



        }


    }

    //---------------Pouvoir sauter avec la barre espace------------------------------------//
    void Saute() {

        if (peutBougerPerso != false) {


            int quelNiveau = LayerMask.GetMask("Sol"); //s'assurer que le hero touche le sol, pour ne pas qu'il puisse sauter dans les aires

            if (!colliderHero.IsTouchingLayers(quelNiveau))
            {
                return;
            }

            if (Input.GetButtonDown("Jump")) //si la barre espace est enfoncé, modifier la velocité du hero pour qu'il puisse sauter
            {

                rigidBHero.velocity = new Vector2(0f, vitesseSaute);
                animationHero.SetTrigger("PeutSauter"); // jouer l'animation du saut

                heroAudioSource.PlayOneShot(audioJump); //jouer l'audio du saut

            }
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Si le hero est en Trigger avec un objet qui a le tag ObjetsAPrendre, detruire l'objet, ajouter 10 au objetPris(point), jouer l'audio, afficher le pointage actuelle
        if (collision.transform.tag == "ObjetsAPrendre")
        {
            Destroy(collision.gameObject);
            objetPris += 10f;
            heroAudioSource.PlayOneShot(audioCollect);
            GameObject.Find("Pointage").GetComponent<TextMeshProUGUI>().text = objetPris.ToString();
        }


    }



    //-----------Collision entre les différents objets/ennemies--------------------------------//
    void OnCollisionEnter2D(Collision2D laCollision) {


        if (laCollision.transform.tag == "Ennemie" && !finDePartie) //Si le hero rentre en collision avec un ennemie -> game over
        {

            GameOver();
        }

        if (laCollision.transform.tag == "Vide" && !finDePartie) // si le hero tombe dans le vide ou dépasse les limites -> game over
        {

            GameOver();
        }

        if (laCollision.transform.tag == "PlateformeBouge")     //Si le hero est sur une plateforme qui bouge, lui attribuer les meme propriété transform que la plateforme
        {
            gameObject.transform.parent = laPlateforme.transform;
        }
        if (laCollision.transform.tag == "PlateformeBouge2")
        {
            gameObject.transform.parent = laPlateforme2.transform;

        }
        if (laCollision.transform.tag == "PlateformeBouge3")
        {
            gameObject.transform.parent = laPlateforme3.transform;
        }

        if (laCollision.transform.tag == "Plancher" && !peutBougerPerso) // utile si le hero entre en collision avec un ennemi pendant qu'il est dans les aires, 
                                                                         //pour qu'il retombe sur le plancher et qu'une fois mort sur le planché il ne puisse plus bouger, même si un ennemie passe dessu
        {
            Destroy(gameObject.GetComponent<Rigidbody2D>());
            Destroy(gameObject.GetComponent<Collider2D>());
        }


    }


    //----------Sortir le Hero du gameobject de la plateforme------------------------------//
    void OnCollisionExit2D(Collision2D laCollision) {
        
        //Enlever au hero les propriété qui sont identique à celles de la plateforme

        if (laCollision.transform.tag == "PlateformeBouge")
        {
            gameObject.transform.parent = null;
        }
        if (laCollision.transform.tag == "PlateformeBouge2"){
            gameObject.transform.parent = null;
    }
        if (laCollision.transform.tag == "PlateformeBouge3")
        {
            gameObject.transform.parent = null;
        }

    }

//------------Nombre d'objet ramassé---------------------------------------------//
void NombreObjetPris(){

        //Selon le niveau, le nombre d'objets à prendre est différents

        if(laScene == "Jeu")
        {
       
            if (objetPris >= 50 && scriptGameManager.tempsRestant != 0 && !finDePartie)
                    {
                        Gagner();
                    }
                    else if (objetPris < 50 && scriptGameManager.tempsRestant == 0 && !finDePartie)
                    {
                        GameOver();
                    }
        }
       
        if (laScene == "Niveau2" || laScene == "Niveau3")
        {
           
            if (objetPris >= 100 && scriptGameManager.tempsRestant != 0 && !finDePartie)
            { 
              
                Gagner();
            }
            else if (objetPris < 100 && scriptGameManager.tempsRestant == 0 && !finDePartie)
            {
              
                GameOver();
            }
        }

    }

   

     //---------------Fin de la partie/Défaite------------------------------//
    void GameOver(){

       finDePartie = true; //variable pour dire que la partie est fini
         defaitee = true;  //variable pour dire que c'est une défaite
       
        heroAudioSource.PlayOneShot(audioMeurt); //jouer l'audio de défaite
        animationHero.Play("Meurt"); //jouer l'animation de défaite

        peutBougerPerso = false; // dire que le hero ne doit plus bouger
       
    }

   

     //-------------------Fin de la partie/Victoire---------------------------------//
    void Gagner(){

        finDePartie = true;
        victoiree = true;
        heroAudioSource.PlayOneShot(audioGagne);
        animationHero.Play("Gagne");

        peutBougerPerso = false;

    }



}


/*---------------------------------------------AUDIOS----------------------------------------------------//
 * Ambiance: https://freesound.org/people/LittleRobotSoundFactory/sounds/323929/   License: https://creativecommons.org/licenses/by/3.0/    Créateur: LittleRobotSoundFactory
 * 
 * Ambiance Accueil: https://freesound.org/people/LittleRobotSoundFactory/sounds/323884/   License: https://creativecommons.org/licenses/by/3.0/  Créateur: LittleRobotSoundFactory
 * 
 * Ambiance Fin: https://freesound.org/people/LittleRobotSoundFactory/sounds/323893/    License:  https://creativecommons.org/licenses/by/3.0/  Créateur: LittleRobotSoundFactory
 * 
 * Saut: https://freesound.org/people/cabled_mess/sounds/350901/        License: https://creativecommons.org/publicdomain/zero/1.0/         Créateur: cabled_mess
 * 
 * Collect: https://freesound.org/people/Leszek_Szary/sounds/171580/    License: https://creativecommons.org/publicdomain/zero/1.0/         Créateur: Leszek_Szary
 * 
 * Mort: https://freesound.org/people/myfox14/sounds/382310/       License: https://creativecommons.org/publicdomain/zero/1.0/              Créateur: myfox14
 * 
 * Gagne: https://freesound.org/people/mickleness/sounds/269198/    License: https://creativecommons.org/publicdomain/zero/1.0/             Créateur: 
 */
