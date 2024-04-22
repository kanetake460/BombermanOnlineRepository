using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TakeshiLibrary;
using UnityEngine;
using UnityEngine.UIElements;

public class GameMap : MonoBehaviour
{
    [SerializeField] GridFieldMapSettings _mapSet;
    private GridFieldMapObject _mapObj;

    private List<GridFieldMapSettings.Block> constantBlockList = new List<GridFieldMapSettings.Block>();

    [SerializeField] Coord[] m_startCoords;
    [SerializeField] GameObject m_player;
    [SerializeField] Texture m_wallTexture;
    [SerializeField] Texture m_stoneTexture;

    private void Awake()
    {
        _mapObj = new GridFieldMapObject(_mapSet);
    }
    void Start()
    { 
        _mapObj.InstanceMapObjects();
        _mapObj.ChangeAllWallTexture(m_wallTexture);
        constantBlockList = _mapSet.WhereBlocks(c => _mapSet.blocks[c.x, c.z].isSpace == true);

        constantBlockList.RemoveAll(b => m_startCoords.Contains(b.coord));
        constantBlockList.ForEach(b => b.isSpace = false);
        constantBlockList.ForEach(b => b.wallRenderer.material.mainTexture = m_stoneTexture);
        _mapObj.ActiveMapWallObjects();

    }


    void Update()
    {


    }
}
