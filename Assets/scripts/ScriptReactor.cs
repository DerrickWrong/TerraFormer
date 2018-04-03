using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * This is a singleton class that allows other game object to 
 * listen to.
 *
 * The singleton wrapper contains different streams for 
 * bridging communication between each object. 
 * 
 */

public sealed class ScriptReactor : MonoBehaviour {

    private UniRx.Subject<UniRx.Tuple<float, float, float>> mainCharacterPosStream;

    private static ScriptReactor instance; 

    private ScriptReactor() {

        this.mainCharacterPosStream = new UniRx.Subject<UniRx.Tuple<float, float, float>>(); 
    }

    public static ScriptReactor Instance {

        get {

            if (instance == null) {

                instance = GameObject.FindObjectOfType(typeof(ScriptReactor)) as ScriptReactor;

                if (instance == null) {

                    GameObject go = new GameObject();
                    go.name = "ScriptReactorSingleton";
                    instance = go.AddComponent<ScriptReactor>();
                }
            }

            return instance;
        }
    }

    // Main Character Position Stream
    public UniRx.Subject<UniRx.Tuple<float, float, float>> getCharacterStream() {
        return this.mainCharacterPosStream;
    }


}
