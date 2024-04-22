using System.Collections;
using System.Collections.Generic;
using TakeshiLibrary;
using UnityEngine;
using UnityEngine.UIElements;

public class GridFieldMapObject
{
    public GridFieldMapSettings _mapSet;

    public GridFieldMapObject(GridFieldMapSettings mapSet)
    {
        _mapSet = mapSet;
    }


    /// <summary>
    /// �w�肵�����W�Ƀ}�b�v�̃I�u�W�F�N�g�𐶐����܂�
    /// </summary>
    /// <param name="coord"></param>
    /// <param name="scaleY">�ǂ̍���</param>
    public void InstanceMapObject(Coord coord, float scaleY)
    {
        // ���쐬
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        _mapSet.blocks[coord.x, coord.z].planeObj = plane;
        _mapSet.blocks[coord.x, coord.z].planeRenderer = plane.GetComponent<Renderer>();
        plane.name = new string(coord.x + "," + coord.z + "_Plane");
        plane.transform.position = _mapSet.gridField[coord.x, coord.z];
        plane.transform.localScale = new Vector3(_mapSet.gridField.CellWidth / 10, 1, _mapSet.gridField.CellDepth / 10);
        

        // �Ǎ쐬
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _mapSet.blocks[coord.x, coord.z].wallObj = cube;
        _mapSet.blocks[coord.x, coord.z].wallRenderer = cube.GetComponent<Renderer>();
        cube.name = new string(coord.x + "," + coord.z + "_Wall");
        cube.transform.position = _mapSet.gridField[coord.x, coord.z] + new Vector3(0, scaleY / 2, 0);
        cube.transform.localScale = new Vector3(_mapSet.gridField.CellWidth, scaleY, _mapSet.gridField.CellDepth);

    }


    /// <summary>
    /// ���ׂẴ}�b�v�̃I�u�W�F�N�g�𐶐����܂�
    /// </summary>
    /// <param>�ǂ̍���</param>
    public void InstanceMapObjects(float scaleY = 10) => _mapSet.gridField.IterateOverGrid(c => InstanceMapObject(c, scaleY));


    /// <summary>
    /// �w�肵���ǃI�u�W�F�N�g�̃��C���[�}�X�N��ݒ肵�܂�
    /// </summary>
    /// <param name="layerName">���C���[</param>
    public void SetLayerMapObject(Coord coord, string layerName) => _mapSet.blocks[coord.x, coord.z].wallObj.layer = LayerMask.NameToLayer(layerName);


    /// <summary>
    /// ���ׂĂ̕ǃI�u�W�F�N�g�̃��C���[�}�X�N��ݒ肵�܂�
    /// </summary>
    /// <param name="layerName">���C���[</param>
    public void SetLayerMapObjects(string layerName) => _mapSet.gridField.IterateOverGrid(c => SetLayerMapObject(c, layerName));


    /// <summary>
    /// �v���[���I�u�W�F�N�g�̐F��ς��܂�
    /// </summary>
    /// <param name="coord">�v���[���̍��W</param>
    /// <param name="color">�F</param>
    public void ChangePlaneColor(Coord coord, Color color) => _mapSet.blocks[coord.x, coord.z].planeRenderer.material.color = color;


    /// <summary>
    /// �ǃI�u�W�F�N�g�̐F��ς��܂�
    /// </summary>
    /// <param name="coord">�ǂ̍��W</param>
    /// <param name="color">�F</param>
    public void ChangeWallColor(Coord coord, Color color) => _mapSet.blocks[coord.x, coord.z].wallRenderer.material.color = color;


    /// <summary>
    /// ���ׂĂ̕ǃI�u�W�F�N�g�̐F��ς��܂�
    /// </summary>
    /// <param name="color">�F</param>
    public void ChangeAllWallColor(Color color) => _mapSet.gridField.IterateOverGrid(coord => ChangeWallColor(coord, color));


    /// <summary>
    /// �ǃI�u�W�F�N�g�̃e�N�X�`����ύX���܂�
    /// </summary>
    /// <param name="coord">�ǂ̍��W</param>
    /// <param name="texture">�e�N�X�`��</param>
    public void ChangeWallTexture(Coord coord, Texture texture) => _mapSet.blocks[coord.x, coord.z].SetWallMaterial(texture);


    /// <summary>
    /// ���ׂĂ̕ǃI�u�W�F�N�g�̃e�N�X�`����ύX���܂�
    /// </summary>
    /// <param name="texture">�e�N�X�`��</param>
    public void ChangeAllWallTexture(Texture texture) => _mapSet.gridField.IterateOverGrid(c => ChangeWallTexture(c, texture));


    /// <summary>
    /// �}�b�v�̕ǃI�u�W�F�N�g��SetActive
    /// </summary>
    public void SetActiveWallObject(Coord coord, bool isActive) => _mapSet.blocks[coord.x, coord.z].wallObj.SetActive(isActive);


    /// <summary>
    /// �w�肵�����W�̕ǃI�u�W�F�N�g�̃A�N�e�B�u�Ǘ�
    /// </summary>
    /// <param name="coord">���W</param>
    public void ActiveMapWallObject(Coord coord) => SetActiveWallObject(coord, !_mapSet.blocks[coord.x, coord.z].isSpace);


    /// <summary>
    /// ���ׂĂ̕ǃI�u�W�F�N�g�̃A�N�e�B�u�Ǘ�
    /// </summary>
    public void ActiveMapWallObjects() => _mapSet.gridField.IterateOverGrid(c => ActiveMapWallObject(c));
}