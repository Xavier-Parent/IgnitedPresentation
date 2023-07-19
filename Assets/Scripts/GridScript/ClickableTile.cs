using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickableTile : MonoBehaviour {

    [SerializeField]
    private Material m_GridColor1;
    [SerializeField]
    private Material m_GridColor2;
    [SerializeField]
    private MeshRenderer m_Renderer;
	public int tileX;
	public int tileY;
	public TileMap map;


	void OnMouseUp() {
		Debug.Log ("Click!");

		if(EventSystem.current.IsPointerOverGameObject())
			return;
		Debug.Log("can move!");
		map.GeneratePathTo(tileX, tileY);
	}

    public void OffsetInit(bool m_Offset)
    {
        m_Renderer.material = m_Offset ? m_GridColor1 : m_GridColor2;
    }

    //Le highlight blanc de la souris sur une case est causé par ca!
    private void OnMouseEnter()
    {
        m_Renderer.material.color += new Color(0.5f, 0.5f, 0.5f);
    }
    private void OnMouseExit()
    {
        m_Renderer.material.color -= new Color(0.5f, 0.5f, 0.5f);
    }

}
