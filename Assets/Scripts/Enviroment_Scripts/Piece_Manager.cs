using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece_Manager : MonoBehaviour
{
    class Piece
    {
        public GameObject go;
        public Vector3 position;

        public Piece(GameObject g, Vector3 p)
        {
            go = g;
            position = p;
        }
    }

    private List<Piece> pieces = new List<Piece>();
    public bool loaded = false;
    // Start is called before the first frame update
    void Start()
    {
        loaded = false;
        foreach (Transform child in gameObject.transform)
        {
            if(child.TryGetComponent<Despawn_Piece>(out Despawn_Piece temp))
            {
                Piece piece = new Piece(temp.gameObject, new Vector3(0, 0, 0));
                //Debug.Log(temp.gameObject.name);
                piece.position = temp.gameObject.transform.localPosition;
                //Debug.Log(temp.gameObject.transform.position);

                pieces.Add(piece);
            }
        }
        loaded = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (loaded && checkPieces())
        {
            SimplePool.Despawn(gameObject);
        }
    }
    /// <summary>
    /// resets all pieces
    /// </summary>
    public void init()
    {
        loaded = false;
        foreach(Piece piece in pieces)
        {            
            piece.go.transform.SetParent(this.gameObject.transform);
            piece.go.transform.localPosition = piece.position;
            piece.go.transform.localScale = new Vector3(1,1,1);
            piece.go.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            piece.go.SetActive(true);
            piece.go.GetComponent<Despawn_Piece>().init();
        }
        loaded = true;
    }
    /// <summary>
    /// Checks to see if all the pieces are deactivated
    /// </summary>
    /// <returns>If all the pieces are deactivated</returns>
    private bool checkPieces()
    {
        bool done = true;
        foreach (Piece piece in pieces)
        {
            if(piece.go.activeSelf == true)
            {
                done = false;
            }
        }
        return done;
    }
}
