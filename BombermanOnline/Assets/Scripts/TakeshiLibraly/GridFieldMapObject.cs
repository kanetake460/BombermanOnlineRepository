using System;
using TakeshiLibrary;
using UnityEngine;

public class GridFieldMapObject
{
    public GridFieldMapSettings _mapSet;

    public GridFieldMapObject(GridFieldMapSettings mapSet)
    {
        _mapSet = mapSet;
    }


    /// <summary>
    /// 指定した座標にマップのオブジェクトを生成します
    /// </summary>
    /// <param name="coord"></param>
    /// <param name="scaleY">壁の高さ</param>
    public void InstanceMapObject(Coord coord, float scaleY)
    {
        // 床作成
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        _mapSet.blocks[coord.x, coord.z].planeObj = plane;
        _mapSet.blocks[coord.x, coord.z].planeRenderer = plane.GetComponent<Renderer>();
        plane.name = new string(coord.x + "," + coord.z + "_Plane");
        plane.transform.position = _mapSet.gridField[coord.x, coord.z];
        plane.transform.localScale = new Vector3(_mapSet.gridField.CellWidth / 10, 1, _mapSet.gridField.CellDepth / 10);
        plane.transform.parent = _mapSet.transform;

        // 壁作成
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _mapSet.blocks[coord.x, coord.z].wallObj = cube;
        _mapSet.blocks[coord.x, coord.z].wallRenderer = cube.GetComponent<Renderer>();
        cube.name = new string(coord.x + "," + coord.z + "_Wall");
        cube.transform.position = _mapSet.gridField[coord.x, coord.z] + new Vector3(0, scaleY / 2, 0);
        cube.transform.localScale = new Vector3(_mapSet.gridField.CellWidth, scaleY, _mapSet.gridField.CellDepth);
        cube.transform.parent = _mapSet.transform;
    }


    /// <summary>
    /// すべてのマップのオブジェクトを生成します
    /// </summary>
    /// <param>壁の高さ</param>
    public void InstanceMapObjects(float scaleY = 10) => _mapSet.gridField.IterateOverGrid(c => InstanceMapObject(c, scaleY));


    /// <summary>
    /// 指定した壁オブジェクトのレイヤーマスクを設定します
    /// </summary>
    /// <param name="layerName">レイヤー</param>
    public void SetLayerMapObject(Coord coord, string layerName) => _mapSet.blocks[coord.x, coord.z].wallObj.layer = LayerMask.NameToLayer(layerName);


    /// <summary>
    /// すべての壁オブジェクトのレイヤーマスクを設定します
    /// </summary>
    /// <param name="layerName">レイヤー</param>
    public void SetLayerMapObjects(string layerName) => _mapSet.gridField.IterateOverGrid(c => SetLayerMapObject(c, layerName));


    /// <summary>
    /// プレーンオブジェクトの色を変えます
    /// </summary>
    /// <param name="coord">プレーンの座標</param>
    /// <param name="color">色</param>
    public void ChangePlaneColor(Coord coord, Color color) => _mapSet.blocks[coord.x, coord.z].planeRenderer.material.color = color;


    /// <summary>
    /// 壁オブジェクトの色を変えます
    /// </summary>
    /// <param name="coord">壁の座標</param>
    /// <param name="color">色</param>
    public void ChangeWallColor(Coord coord, Color color) => _mapSet.blocks[coord.x, coord.z].wallRenderer.material.color = color;


    /// <summary>
    /// すべての壁オブジェクトの色を変えます
    /// </summary>
    /// <param name="color">色</param>
    public void ChangeAllWallColor(Color color) => _mapSet.gridField.IterateOverGrid(coord => ChangeWallColor(coord, color));


    /// <summary>
    /// 壁オブジェクトのテクスチャを変更します
    /// </summary>
    /// <param name="coord">壁の座標</param>
    /// <param name="texture">テクスチャ</param>
    public void ChangeWallTexture(Coord coord, Texture texture) => _mapSet.blocks[coord.x, coord.z].SetWallMaterial(texture);


    /// <summary>
    /// すべての壁オブジェクトのテクスチャを変更します
    /// </summary>
    /// <param name="texture">テクスチャ</param>
    public void ChangeAllWallTexture(Texture texture) => _mapSet.gridField.IterateOverGrid(c => ChangeWallTexture(c, texture));


    /// <summary>
    /// マップの壁オブジェクトのSetActive
    /// </summary>
    public void SetActiveWallObject(Coord coord, bool isActive) => _mapSet.blocks[coord.x, coord.z].wallObj.SetActive(isActive);


    /// <summary>
    /// 指定した座標の壁オブジェクトのアクティブ管理
    /// </summary>
    /// <param name="coord">座標</param>
    public void ActiveMapWallObject(Coord coord) => SetActiveWallObject(coord, !_mapSet.blocks[coord.x, coord.z].isSpace);


    /// <summary>
    /// すべての壁オブジェクトのアクティブ管理
    /// </summary>
    public void ActiveMapWallObjects() => _mapSet.gridField.IterateOverGrid(c => ActiveMapWallObject(c));


    public void AddComponentWallObject<T>(Coord coord) where T : Component
    {
        if (_mapSet.blocks[coord.x, coord.z].wallObj != null)
        {
            _mapSet.blocks[coord.x, coord.z].wallObj.AddComponent<T>();
        }
        else
        {
            Debug.LogError("指定された壁オブジェクトがありません");
        }
    }

    public void AddComponentAllWallObjects<T>() where T : Component => _mapSet.gridField.IterateOverGrid(c => AddComponentWallObject<T>(c));
}
