using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SquareSelected : MonoBehaviour, IPointerDownHandler, IDropHandler, IDragHandler
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.parent.GetComponent<MovePieceUI>().squareSelected(gameObject);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.parent.GetComponent<MovePieceUI>().onDragPiece();
    }

    public void OnDrop(PointerEventData eventData)
    {
        transform.parent.GetComponent<MovePieceUI>().squareSelected(gameObject);
    }
}
