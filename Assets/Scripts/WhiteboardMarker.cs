using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class WhiteboardMarker : MonoBehaviour
{
    [SerializeField]private Transform _tip;
    [SerializeField]private int _penSize = 5;

    private Renderer _renderer;
    private Color[] _colors;
    private float _tipHeight;
    
    private RaycastHit _touch;
    private Whiteboard _whiteBoard;
    private Vector2 _touchPos;
    private Vector2 _lastTouchPos;
    private bool _touchedLastFrame;
    private Quaternion _lastTouchRot;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = _tip.GetComponent<Renderer>();
        _colors = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray(); //Taking one value and repeating it over whatever we want the value to be.
        _tipHeight = _tip.localScale.y;

    }

    // Update is called once per frame
    void Update()
    {
        Draw();
    }

    private void Draw()
    {
        if(Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight)) //If we hit something, aka the white board, execute.
        {
            if(_touch.transform.CompareTag("Whiteboard"))
            {
                if(_whiteBoard == null)
                {
                    _whiteBoard = _touch.transform.GetComponent<Whiteboard>();
                }

                _touchPos = new Vector2(_touch.textureCoord.x, _touch.textureCoord.y);

                var x = (int)(_touchPos.x * _whiteBoard.textureSize.x - (_penSize / 2));
                var y = (int)(_touchPos.y * _whiteBoard.textureSize.y - (_penSize / 2));

                if(y < 0 || y > _whiteBoard.textureSize.y || x > _whiteBoard.textureSize.x) //Making sure we dont try and draw if we arent touching the board.
                {
                    return;
                }

                if(_touchedLastFrame) 
                {
                    _whiteBoard.texture.SetPixels(x,y, _penSize, _penSize, _colors);

                    for(float f = 0.01f; f < 1.00f; f += 0.01f) //If the line sucks, change this last value to an even smaller number. WARNING: WILL COST RESOURCES.
                    {
                        var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                        var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                         _whiteBoard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _colors);
                    }
                    transform.rotation = _lastTouchRot;
                    _whiteBoard.texture.Apply();
                }

                _lastTouchPos = new Vector2(x, y);
                _lastTouchRot = transform.rotation;
                _touchedLastFrame = true;
                return;
                
            }
        }

        _whiteBoard = null;
        _touchedLastFrame = false;
    }
}
