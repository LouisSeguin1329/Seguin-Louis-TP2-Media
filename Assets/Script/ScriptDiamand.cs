using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDiamand : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleDiamand;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        particleDiamand.Play();
    }

   


}

/*
 *
License pour particles:Licenced under the terms of the version 3.0 of the Creative Commons Attribution-Share Alike license. © 2005-2013 Julien Jorge <julien.jorge@stuff-o-matic.com>
https://creativecommons.org/licenses/by-sa/3.0/
*
*/
