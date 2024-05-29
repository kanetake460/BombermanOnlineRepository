using System;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    /// <summary>
    /// �}�b�v�ݒ�N���X
    /// </summary>
    [Serializable]
    public class GridFieldMapSettings : MonoBehaviour
    {
        /// <summary>
        /// �u���b�N�N���X
        /// </summary>
        public class Block
        {
            // �u���b�N�̃O���b�h���W
            public Coord coord { get; }
            // �u���b�N�̎��
            public bool isSpace { get; set; }

            // �ǂ����邩�ǂ���( �ǂ����� : true )
            public bool fowardWall { get; set; } = false;
            public bool rightWall { get; set; } = false;
            public bool backWall { get; set; } = false;
            public bool leftWall { get; set; } = false;

            // �I�u�W�F�N�g�ݒ�
            public GameObject wallObjParent { get; set; }
            public GameObject planeObjParent { get; set; }
            public GameObject wallObj { get; set; }         // �ǃI�u�W�F�N�g
            public GameObject planeObj { get; set; }        // ���I�u�W�F�N�g
            public Renderer wallRenderer { get; set; }      // �ǃ����_��
            public Renderer planeRenderer { get; set; }     // �������_��

            /// <summary>
            /// �u���b�N�ɏ�������R���X�g���N�^
            /// </summary>
            /// <param name="x">x�O���b�h���W</param>
            /// <param name="z">z�O���b�h���W</param>
            /// <param name="isSpace">�ǂ��A��Ԃ�</param>
            public Block(int x, int z, bool isSpace = true)
            {
                coord = new Coord(x, z);
                this.isSpace = isSpace;
            }

            /// <summary>
            /// �^����Vector3���W�̌������ǂȂ̂��ǂ������ׂ܂�
            /// </summary>
            /// <param>Vector3�̌���</param>
            /// <returns>�ǂ��ǂ��Ȃ̂�</returns>
            public bool CheckWallDirection(Vector3 dir)
            {
                if (dir == Vector3.right) return rightWall;
                else if (dir == Vector3.left) return leftWall;
                else if (dir == Vector3.forward) return fowardWall;
                else if (dir == Vector3.back) return backWall;
                else return false;
            }

            /// <summary>
            /// �^�������W�̌������ǂȂ̂��ǂ������ׂ܂�
            /// </summary>
            /// <param name="x">��( -1 or 0 )</param>
            /// <param name="z">��( -1 or 0 )</param>
            /// <returns>�ǂ��ǂ��Ȃ̂�</returns>
            public bool CheckWall(int x, int z)
            {
                Vector3 checkDir = new Vector3(x, 0, z);
                return CheckWallDirection(checkDir);
            }


            /// <summary>
            /// �ǂ̃e�N�X�`����ύX���܂��B
            /// </summary>
            /// <param name="texture">�e�N�X�`��</param>
            public void SetWallMaterial(Texture texture) { wallRenderer.material.mainTexture = texture; }
            /// <summary>
            /// �ǂ̐F��ύX���܂��B
            /// </summary>
            /// <param name="color">�F</param>
            public void SetWallMaterial(Color color) { wallRenderer.material.color = color; }
            /// <summary>
            /// �ǂ̃e�N�X�`���ƐF��ύX���܂��B
            /// </summary>            
            /// <param name="texture">�e�N�X�`��</param>
            /// <param name="color">�F</param>
            public void SetWallMaterial(Texture texture, Color color) { wallRenderer.material.mainTexture = texture; wallRenderer.material.color = color; }


            /// <summary>
            /// ���̃e�N�X�`����ύX���܂��B
            /// </summary>
            /// <param name="texture">�e�N�X�`��</param>
            public void SetPlaneMaterial(Texture texture) { planeRenderer.material.mainTexture = texture; }
            /// <summary>
            /// ���̐F��ύX���܂��B
            /// </summary>
            /// <param name="color">�F</param>
            public void SetPlaneMaterial(Color color) { planeRenderer.material.color = color; }
            /// <summary>
            /// ���̃e�N�X�`���ƐF��ύX���܂��B
            /// </summary>            
            /// <param name="texture">�e�N�X�`��</param>
            /// <param name="color">�F</param>
            public void SetPlaneMaterial(Texture texture, Color color) { planeRenderer.material.mainTexture = texture; wallRenderer.material.color = color; }

        }
        // ===================================================================================================

        /// <summary>
        /// �p�����[�^�[
        /// </summary>
        [Header("GridField")]
        public Vector3 position;
        [Range(0, 30)] public int gridWidth;
        [Range(0, 30)] public int gridDepth;
        public float cellWidth;
        public float cellDepth;

        // �u���b�N��isSpace�����蓖�Ă�v���p�e�B
        public bool[] isSpaceProp;
        // �O���b�h�t�B�[���h
        public GridField gridField;

        public Block[,] blocks = new Block[100, 100];

        // �u���b�N�̈ꎟ���z��Q�b�^�[
        public Block[] Blocks
        {
            get
            {
                Block[] ret = new Block[gridField.TotalCell];
                int count = 0;

                for (int x = 0; x < gridWidth; x++)
                {
                    for (int z = 0; z < gridDepth; z++)
                    {
                        count++;
                        ret[count] = blocks[x, z];
                    }
                }

                return ret;
            }
        }

        // ===================================================================================================

        private void Awake()
        {
            gridField = new GridField(position, gridWidth, gridDepth, cellWidth, cellDepth);

            int count = 0;
            gridField.IterateOverGrid((UnityEngine.Events.UnityAction<Coord>)(coord =>
            {
                this.blocks[coord.x, coord.z] = new Block(coord.x, coord.z, isSpaceProp[count]);
                count++;
            }));
        }
        // ===================================================================================================





        /// <summary>
        /// �w�肵�����W��ǃu���b�N�ɐݒ肵�܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        public void SetWallBlock(int x, int z)
        {
            blocks[x, z].isSpace = false;
        }

        /// <summary>
        /// �w�肵���u���b�N��ǃu���b�N�ɐݒ肵�܂�
        /// </summary>
        /// <param name="block">�ǂɂ������u���b�N</param>
        public void SetWallBlock(Block block)
        {
            block.isSpace = false;
        }


        /// <summary>
        /// �w�肵�����W�̃u���b�N�A������ǂɐݒ肵�܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        /// <param name="dir">�ǂ��������</param>
        public void SetWallDirection(int x, int z, Vector3 dir)
        {
            if (dir == Vector3.forward) blocks[x, z].fowardWall = true;
            else if (dir == Vector3.right) blocks[x, z].rightWall = true;
            else if (dir == Vector3.back) blocks[x, z].backWall = true;
            else if (dir == Vector3.left) blocks[x, z].leftWall = true;
        }


        /// <summary>
        /// �w�肵���u���b�N�́A������ǂɐݒ肵�܂�
        /// </summary>
        /// <param name="block">�ݒ肵�����u���b�N</param>
        /// <param name="dir">�ǂ��������</param>
        public void SetWallDirection(Block block, Vector3 dir)
        {
            if (dir == Vector3.forward) block.fowardWall = true;
            else if (dir == Vector3.right) block.rightWall = true;
            else if (dir == Vector3.back) block.backWall = true;
            else if (dir == Vector3.left) block.leftWall = true;
        }


        /// <summary>
        /// �^�������W�̂��ׂĂ̌����̕ǂ�ݒ肵�܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        /// <param name="foward">�O��</param>
        /// <param name="right">�E��</param>
        /// <param name="back">���</param>
        /// <param name="left">����</param>
        public void SetWallsDirection(int x, int z, bool foward = true, bool right = true, bool back = true, bool left = true, bool isSpace = false)
        {
            blocks[x, z].fowardWall = foward;
            blocks[x, z].rightWall = right;
            blocks[x, z].backWall = back;
            blocks[x, z].leftWall = left;
            blocks[x, z].isSpace = isSpace;
        }


        /// <summary>
        /// ���������u���b�N�̂��ׂĂ̌����̕ǂ�ݒ肵�܂�
        /// �f�t�H���g�����ł͕ǂ�����܂�
        /// </summary>
        /// <param name="back">�u���b�N</param>
        /// <param name="foward">�O��</param>
        /// <param name="right">�E��</param>
        /// <param name="back">���</param>
        /// <param name="left">����</param>
        public void SetWallsDirection(Block block, bool foward = true, bool right = true, bool back = true, bool left = true, bool isSpace = false)
        {
            block.fowardWall = foward;
            block.rightWall = right;
            block.backWall = back;
            block.leftWall = left;
            block.isSpace = isSpace;
        }


        /// <summary>
        /// �w�肵�����W�̃u���b�N�A�����̕ǂ��Ȃ����܂�
        /// </summary>
        /// <param name="x">x�O���b�h���W</param>
        /// <param name="z">z�O���b�h���W</param>
        /// <param name="dir">�ǂ��������</param>
        public void BreakWall(int x, int z, Vector3 dir)
        {
            if (dir == Vector3.forward) blocks[x, z].fowardWall = false;
            else if (dir == Vector3.right) blocks[x, z].rightWall = false;
            else if (dir == Vector3.back) blocks[x, z].backWall = false;
            else if (dir == Vector3.left) blocks[x, z].leftWall = false;
        }


        /// <summary>
        /// �w�肵���u���b�N�A�����̕ǂ��Ȃ����܂�
        /// </summary>
        /// <param name="block">�u���b�N</param>
        /// <param name="dir">�ǂ��������</param>
        public void BreakWall(Block block, Vector3 dir)
        {
            if (dir == Vector3.forward) block.fowardWall = false;
            else if (dir == Vector3.right) block.rightWall = false;
            else if (dir == Vector3.back) block.backWall = false;
            else if (dir == Vector3.left) block.leftWall = false;
        }


        /// <summary>
        /// �����ɓ��Ă͂܂���W�Ƀu���b�N�𐶐����܂��B
        /// </summary>
        /// <param name="func">����</param>
        public void CreateWalls(Func<Coord, bool> func)
        {
            gridField.IterateOverGrid(coord =>
            {
                if (func(coord))
                {
                    SetWallsDirection(coord.x, coord.z);
                    SetWallBlock(coord.x, coord.z);
                }
            });
        }


        /// <summary>
        /// �����ɓ��Ă͂܂���W�̃u���b�N���X�g��Ԃ��܂�
        /// </summary>
        /// <param name="func">����</param>
        /// <returns>���Ă͂܂�u���b�N�̃��X�g</returns>
        public List<Block> WhereBlocks(Func<Coord, bool> func)
        {
            List<Block> ret = new List<Block>();
            gridField.IterateOverGrid((UnityEngine.Events.UnityAction<Coord>)(coord =>
            {
                if (func(coord))
                {
                    ret.Add((Block)this.blocks[coord.x, coord.z]);
                }
            }));
            return ret;
        }


        /// <summary>
        /// �}�b�v�̂��ׂẴu���b�N��ǂɐݒ肵�܂�
        /// </summary>
        public void CreateWallsAll() => CreateWalls(a => true);


        /// <summary>
        /// �O���b�h��ɕǂ𐶐����܂�
        /// </summary>
        public void CreateWallsOddGrid() => CreateWalls(coord => coord.x % 2 == 1 && coord.z % 2 == 1);
        public void CreateWallsEvenGrid() => CreateWalls(coord => coord.x % 2 == 0 && coord.z % 2 == 0);


        /// <summary>
        /// �}�b�v���͂ނ悤�ɕǂ�ݒ肵�܂�
        /// </summary>
        public void CreateWallsSurround() => CreateWalls(coord => coord.x == 0 ||
                                                                  coord.z == 0 ||
                                                                  coord.x == gridField.GridWidth - 1 ||
                                                                  coord.z == gridField.GridDepth - 1);


        /// <summary>
        /// �^�����O���b�h���W���}�b�v�Ȃ��Ȃ�false��Ԃ��܂�
        /// </summary>
        /// <param name="coord">���W</param>
        /// <returns>�O���b�h�̏�Ȃ�true</returns>
        public bool CheckMap(Coord coord)
        {
            return coord.x >= 0 &&
                    coord.z >= 0 &&
                    coord.x < gridField.GridWidth &&
                    coord.z < gridField.GridDepth;
        }


        /// <summary>
        /// �w�肵�����W����w��͈̔͂̂��ׂẴu���b�N��Ԃ��܂�
        /// </summary>
        /// <param name="coord">���S���W</param>
        /// <param name="areaX">X�̒���</param>
        /// <param name="areaZ">Z�̒���</param>
        public List<Block> AreaBlockList(Coord coord, int areaX, int areaZ)
        {
            // �I��͈͂̃u���b�N�̃��X�g
            List<Block> lAreaBlock = new List<Block>();

            // �����͈͂̃u���b�N�����X�g�ɒǉ�
            for (int x = -areaX; x < areaX; x++)
            {
                for (int z = -areaZ; z < areaZ; z++)
                {
                    if (!CheckMap(new Coord(coord.x + x, coord.z + z))) continue;
                    Block b = blocks[coord.x + x, coord.z + z];
                    lAreaBlock.Add(b);
                }
            }

            return lAreaBlock;
        }


        /// <summary>
        /// �w�肵�����W����w��͈̔͂�"�w�肵�����W�ȊO��"���ׂẴu���b�N��Ԃ��܂�
        /// </summary>
        /// <param name="coord">���S���W</param>
        /// <param name="exceptionCoordList">���O������W�̃��X�g</param>
        /// <param name="areaX"></param>
        /// <param name="areaZ"></param>
        public List<Block> CustomAreaBlockList(Coord coord, List<Coord> exceptionCoordList, int areaX, int areaZ)
        {
            // �I��͈͂̃u���b�N�̃��X�g
            List<Block> lAreaBlock = AreaBlockList(coord, areaX, areaZ);

            lAreaBlock.RemoveAll(b => exceptionCoordList.Contains(b.coord));

            return lAreaBlock;
        }


        /// <summary>
        /// �w�肵�����W����w��͈̔͂̃u���b�N��"�t���[����ɂ���"�u���b�N��Ԃ��܂�
        /// </summary>
        /// <param name="coord">���S���W</param>
        /// <param name="frameSize">�t���[���̃T�C�Y</param>
        /// <param name="areaX">X�̒���</param>
        /// <param name="areaZ">Z�̒���</param>
        /// <returns></returns>
        public List<Block> FrameAreaBlockList(Coord coord, int frameSize, int areaX, int areaZ)
        {
            // �I��͈͂̃u���b�N�̃��X�g
            List<Block> lAreaBlock = AreaBlockList(coord, areaX, areaZ);

            // �G���A����AframeSize�̒l�������̃G���A�̃u���b�N���폜
            for (int x = -areaX + frameSize; x < areaX - frameSize; x++)
            {
                for (int z = -areaZ + frameSize; z < areaZ - frameSize; z++)
                {
                    if (!CheckMap(new Coord(coord.x + x, coord.z + z))) continue;
                    Block removeBlock = blocks[coord.x + x, coord.z + z];
                    lAreaBlock.Remove(removeBlock);
                }
            }

            return lAreaBlock;
        }


        /// <summary>
        /// �w�肵�����W����w��͈̔͂̃u���b�N��"�t���[����ɂ���"�u���b�N��Ԃ��܂�
        /// </summary>
        /// <param name="coord">���S���W</param>
        /// <param name="frameSize">�t���[���̃T�C�Y</param>
        /// <param name="exceptionCoordList">���O������W�̃��X�g</param>
        /// <param name="areaX">X�̒���</param>
        /// <param name="areaZ">Z�̒���</param>
        public List<Block> CustomFrameAreaBlockList(Coord coord, int frameSize, List<Coord> exceptionCoordList, int areaX, int areaZ)
        {
            // �I��͈͂̃u���b�N�̃��X�g
            List<Block> lAreaBlock = FrameAreaBlockList(coord, frameSize, areaX, areaZ);

            lAreaBlock.RemoveAll(b => exceptionCoordList.Contains(b.coord));

            return lAreaBlock;
        }


        /// <summary>
        /// AStar�̓���ݒ肵�܂�
        /// </summary>
        /// <param name="start">�T���̍ŏ��̈ʒu</param>
        /// <param name="goal">�T���̃S�[���n�_</param>
        /// <param name="aStar">AStar</param>
        public void AStar(Vector3 start, Vector3 goal, GridFieldAStar aStar)
        {
            if (aStar == null)
            {
                aStar = new GridFieldAStar();
            }

            aStar.AStarPath(this, gridField.GridCoordinate(start), gridField.GridCoordinate(goal));

            foreach (Coord p in aStar.pathStack)
            {
                Debug.DrawLine(gridField[p.x, p.z], gridField[p.x, p.z] + Vector3.up, UnityEngine.Color.red, 10f);

            }
        }
    }
}