using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //-------------------Variable Hero + Scene-------------------------------//
    static public string nomJoueur;             
    [SerializeField] public GameObject monHero;             
    string laScene;


    private  Mouvement scriptHeroMouvement;     


    //-----------------Variable Fin------------------------------------------//
    [SerializeField] public Text theEndText;
    [SerializeField] public Text LePointage;

    [SerializeField] public TextMeshProUGUI txtDeFin;

    //----------------------Variable Timer------------------------------------//
    [SerializeField] public Text LeTempsEcoulerr;
    [SerializeField] public Text theTimer;
    [SerializeField] public GameObject theTimerGO;
    [SerializeField] public float tempsRestant = 30.0f;
    public float tempsJeu;


    void Start()
    {
       
        //--------------------------------------------------------------//
        laScene = SceneManager.GetActiveScene().name;   //Variable qui va chercher le nom de la scene actuelle
         scriptHeroMouvement = monHero.GetComponent<Mouvement>();  //Variable pour aller chercher des variables dans le script Mouvement
      

        //---------------------Cette condition sert à initier la variable nomJoueur au nom du joueur seulement dans les scenes mentionné----------------------------------//
        if (laScene == "Jeu" || laScene == "Fin" || laScene == "InterScene" || laScene == "Niveau2" || laScene == "InterScene2" || laScene == "Niveau3")
        {
            GameObject.Find("NomJoueur").GetComponent<TextMeshProUGUI>().text = nomJoueur;
        }
       
          
       //-----------Cette condition initie la variable tempsRestant au temps restant de la scene----------------------------//
        if(laScene == "Jeu")
        {
            tempsRestant = 30f;
        }
        if (laScene == "Niveau2")
        {
            tempsRestant = 50f;
        }
        if(laScene == "Niveau3")
        {
            tempsRestant = 60f;
        }


     

    }

    public void Update()
    {
   

        if (laScene == "Jeu" || laScene == "Niveau2" || laScene == "Niveau3")
        {
        
            Sijegagne();
            DebutTimer();

            //---------------------Si LeTemps Ecouler deviens plus haut que 30, le mettre a 30 -----------------------------------------//
            tempsJeu = Mathf.Round(Time.timeSinceLevelLoad);
            if (laScene == "Jeu")
            {
            if (tempsJeu >= 30)
             {
                tempsJeu = 30f;
             }
            }
            //---------------------Si LeTemps Ecouler deviens plus haut que 50, le mettre a 50 -----------------------------------------//
            if (laScene == "Niveau2")
            {
                if (tempsJeu >= 50)
                {
                    tempsJeu = 50f;
                }
            }
            //---------------------Si LeTemps Ecouler deviens plus haut que 60, le mettre a 60 -----------------------------------------//
            if (laScene == "Niveau3")
            {
                if (tempsJeu >= 60)
                {
                    tempsJeu = 60;
                }
            }

        }
    

    }

    public void DebutPartie()
    {

        nomJoueur = GameObject.Find("NomDuJoueur").GetComponent<TMP_InputField>().text; 


        //---------------------S'assurer que le joueur à entré un nom-----------------------------------------//
        if (nomJoueur.Length == 0)
        {
           
            GameObject.Find("Placeholder").GetComponent<TextMeshProUGUI>().text = "Inscrivez votre nom";
        }
        else
        {
            SceneManager.LoadScene("Jeu");
        }
    }

    void DebutTimer()
    {
        //-------------------------Le Timer-------------------------------------//
       
        tempsRestant -= Time.deltaTime; //Le temps qu'il reste avant la fin
        theTimer.text = Mathf.Round(tempsRestant).ToString(); //Temps affiché à la fin du niveau
       
        if (tempsRestant <= 0)
        {
            tempsRestant = 0f;
        }
        if (tempsRestant <= 10)
        {
            theTimer.text = "0:0" + Mathf.Floor(tempsRestant); // Exemple: 0:05
        }
        else if (tempsRestant >= 10)
        {
            theTimer.text = "0:" + Mathf.Floor(tempsRestant); // Exemple: 0:10
        }
    }

    void Sijegagne()
    {
       
       
        //-------------------Méthode pour détecter si la partie est gagné ou perdu-------------------------------------------//
        if (scriptHeroMouvement.victoiree == true)
        {
            gagner();
        }

        else if (scriptHeroMouvement.defaitee == true)
        {
            defaite();
        }
    }

    void defaite()
    {
        
        theEndText.text = "DÉFAITE!";  //Titre de fin

        txtDeFin.text = "Tu as perdu"; // Message de fin
 
        LePointage.text = (scriptHeroMouvement.objetPris).ToString(); //var pour afficher le pointage

        LeTempsEcoulerr.text = "00:" + tempsJeu;
       
        if (Mathf.Round(tempsJeu) < 10)
        {
            LeTempsEcoulerr.text = "0:0" + Mathf.Round(tempsJeu);
        }
        else if (Mathf.Round(tempsJeu) >= 10)
        {
            LeTempsEcoulerr.text = "0:" + Mathf.Round(tempsJeu);
        }

    }
  
    void gagner()
    {

        theEndText.text = "VICTOIRE!"; //Titre de fin

        txtDeFin.text = "Tu as réussi tous les niveaux !!!"; //Message de fin

        LePointage.text = (scriptHeroMouvement.objetPris).ToString(); //var pour afficher le pointage
        LeTempsEcoulerr.text = "00:" + tempsJeu;

        if (Mathf.Round(tempsJeu) < 10)
        {
            LeTempsEcoulerr.text = "0:0" + Mathf.Round(tempsJeu);
        }
        else if (Mathf.Round(tempsJeu) >= 10)
        {
            LeTempsEcoulerr.text = "0:" + Mathf.Round(tempsJeu);
        }

    }

    public void restartGame()
    {
        //------Si la scene est Fin, le bouton Recommencer mène à la scene Accueil-----------//
        if (laScene == "Fin")
        {
            SceneManager.LoadScene("Accueil");
            
        }
    }

    public void niveauSuivant()
    {
       //----------Si la scene est InterScene, le bouton niveau suivant mène à la scene Niveau2----------------------------------//
        if(laScene == "InterScene")
        {
           
            SceneManager.LoadScene("Niveau2");
        }

        //----------Si la scene est InterScene2, le bouton niveau suivant mène à la scene Niveau3----------------------------------//

        if (laScene == "InterScene2")
        {
            SceneManager.LoadScene("Niveau3");
        }
    }

    //-------J'ai dû faire une deuxième scène InterScene car ma variable qui détectait dans quelle scène on est ne marchais pas--------------------//
   
 
}
