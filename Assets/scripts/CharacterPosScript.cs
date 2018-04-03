using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPosScript : MonoBehaviour {

    private UniRx.Subject<UniRx.Tuple<float, float, float>> m_PosStream;

    // Use this for initialization
    void Start () {

        this.m_PosStream = ScriptReactor.Instance.getCharacterStream();
    }
	
	// Publish the character's position at every frame
	void Update () {

        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;

        this.m_PosStream.OnNext(UniRx.Tuple.Create(x, y, z));
	}
}
