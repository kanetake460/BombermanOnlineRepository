using System;
using UnityEngine;
using UnityEngine.Events;

namespace TakeshiLibrary
{
    /*=====�O���b�h�t�B�[���h���쐬����֐�=====*/
    // Vector3�̃N���X���Q�l�ɍ쐬���܂���
    // C:\Users\kanet\AppData\Local\Temp\MetadataAsSource\b33e6428b1fe4c03a5b0b222eb1e9f0b\DecompilationMetadataAsSourceFileProvider\4496430b4e32462b86d5e9f4984747a4\Vector3.cs


    public class GridField
    {
        //======�ϐ�===========================================================================================================================
        private Vector3 _position;
        private int _gridWidth;
        private int _gridDepth;
        private float _cellWidth;
        private float _cellDepth;


        public Vector3 Position => _position;
        public int GridWidth => _gridWidth;
        public int GridDepth => _gridDepth;
        public float CellWidth => _cellWidth;
        public float CellDepth => _cellDepth;

        public Vector3 this[int x, int z]
        {
            get
            {
                if (!CheckOnGridCoord(new Coord(x, z)))
                    throw new IndexOutOfRangeException();

                return _position + new Vector3(x * _cellWidth, 0, z * _cellDepth)
                    - new Vector3((float)(_gridWidth - 1) / 2 * _cellWidth, 0, (float)(_gridDepth - 1) / 2 * _cellDepth);    // x��z��10���������l����
            }
        }

        //======�R���X�g���N�^=================================================================================================================

        public GridField(Vector3 position,int gridWidth,int gridDepth,float cellWidth,float cellDepth)
        {
            this._position  = position;
            this._gridWidth = gridWidth;
            this._gridDepth = gridDepth;
            this._cellWidth = cellWidth;
            this._cellDepth = cellDepth;
        }


        //======�ǂݎ���p�ϐ�===============================================================================================================

        /// <summary>
        /// �O���b�h�̃Z���̐���Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public int TotalCell
        {
            get
            {
                return GridWidth * GridDepth;
            }
        }

        /// <summary>
        /// �O���b�h�̕��Ɖ��s�̍ł���������Ԃ��܂��B
        /// </summary>
        public int GridMaxLength
        {
            get
            {
                return Mathf.Max(GridWidth, GridDepth);
            }
        }
        
        /// <summary>
        /// �O���b�h�̕��Ɖ��s�̍ł��Z������Ԃ��܂��B
        /// </summary>
        public int GridMinLength
        {
            get
            {
                return Mathf.Min(GridWidth, GridDepth);
            }
        }

        /// <summary>
        /// �Z���̕��Ɖ��s�̍ł���������Ԃ��܂��B
        /// </summary>
        public float CellMaxLength
        {
            get
            {
                return Mathf.Max(CellWidth, CellDepth);
            }
        }
        
        
        /// <summary>
        /// �Z���̕��Ɖ��s�̍ł���������Ԃ��܂��B
        /// </summary>
        public float CellMinLength
        {
            get
            {
                return Mathf.Min(CellWidth, CellDepth);
            }
        }


        /// <summary>
        /// �t�B�[���h�̕��Ɖ��s�̍ł���������Ԃ��܂��B
        /// </summary>
        public float FieldMaxLength
        {
            get
            {
                return GridMaxLength * CellMaxLength;
            }
        }


        /// <summary>
        /// �t�B�[���h�̕��Ɖ��s�̍ł��Z������Ԃ��܂��B
        /// </summary>
        public float FieldMinLength
        {
            get
            {
                return GridMinLength * CellMinLength;
            }
        }


        /*==========�O���b�h�t�B�[���h�̊p�̃Z����Vector3���W==========*/
        /// <summary>
        ///�O���b�h�̍����̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 BottomLeftCell
        {
            get
            {
                return this[0, 0];
            }
        }

        /// <summary>
        ///�O���b�h�̉E���̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 BottomRightCell
        {
            get
            {
                return this[GridWidth - 1, 0];
            }
        }

        /// <summary>
        ///�O���b�h�̍���̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 TopLeftCell
        {
            get
            {
                return this[0, GridDepth - 1];
            }
        }

        /// <summary>
        ///�O���b�h�̉E��̃Z���̍��W��Ԃ��܂��B(�ǂݎ���p)
        /// </summary>
        public Vector3 TopRightCell
        {
            get
            {
                return this[GridWidth - 1, GridDepth - 1];
            }
        }



        /*==========�O���b�h�t�B�[���h�̊p��Vector3���W==========*/
        /// <summary>
        /// �O���b�h�̍����̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 BottomLeft
        {
            get
            {
                return this[0, 0] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// �O���b�h�̉E���̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 BottomRight
        {
            get
            {
                return this[GridWidth - 1, 0] + new Vector3(CellWidth / 2, _position.y, CellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// �O���b�h�̍���̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 TopLeft
        {
            get
            {
                return this[0, GridDepth - 1] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2);
            }
        }

        /// <summary>
        /// �O���b�h�̉E��̈ʒu���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 TopRight
        {
            get
            {
                return this[GridWidth - 1, GridDepth - 1] + new Vector3(CellWidth / 2, _position.y, CellDepth / 2);
            }
        }



        /*=========�O���b�h�t�B�[���h�̒��SVector3���W===========*/
        /// <summary>
        /// �O���b�h�̐^�񒆂� localPosition ��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 Middle
        {
            get
            {
                // �������s���ǂ���Ƃ�����
                if (GridWidth % 2 == 0 && GridDepth % 2 == 0)
                {
                    // �O���b�h���W����Z���̔����̐����炵���l��Ԃ�
                    return this[GridWidth / 2, GridDepth / 2] - new Vector3(CellWidth / 2, 0, CellDepth / 2);

                }
                // ����������
                else if (GridWidth % 2 == 0)
                {
                    // �O���b�h���W���炩��Z���̔����̐������炵���l��Ԃ�(�����̂�)
                    return this[GridWidth / 2, GridDepth / 2] - new Vector3(CellWidth / 2, 0, 0);
                }
                // ���s������
                else if (GridDepth % 2 == 0)
                {
                    // �O���b�h���W����Z���̔����̐������炵���l��Ԃ�(���s�̂�)
                    return this[GridWidth / 2, GridDepth / 2] - new Vector3(0, 0, CellDepth / 2);
                }
                // �ǂ���Ƃ��
                else
                {
                    // �O���b�h���W��Ԃ�
                    return this[GridWidth / 2, GridDepth / 2];
                }
            }
        }


        /*=========�O���b�h�t�B�[���h�̒��SVector3���W===========*/
        /// <summary>
        /// �O���b�h�̐^�񒆂� localPosition ��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Coord MiddleGrid
        {
            get
            {
                return new Coord(GridWidth / 2, GridDepth / 2);
            }
        }

        /*=========�����_��===========*/
        /// <summary>
        /// �O���b�h���W�̃����_���Ȉʒu��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Vector3 RandomGridPos
        {
            get
            {
                int randX = UnityEngine.Random.Range(0, GridWidth);
                int randZ = UnityEngine.Random.Range(0, GridDepth);
                return this[randX, randZ];
            }
        }

        /// <summary>
        /// �����_���ȃO���b�h���W��Ԃ��܂�(�ǂݎ���p)
        /// </summary>
        public Coord RandomGridCoord
        {
            get
            {
                int randX = UnityEngine.Random.Range(0, GridWidth);
                int randZ = UnityEngine.Random.Range(0, GridDepth);
                return new Coord(randX, randZ);
            }
        }



        //======�֐�===========================================================================================================================

        ///<summary>
        ///�V�[���E�B���h�E�ɃO���b�h��\�����܂�
        ///</summary>
        public void DrowGrid()
        {
            // ���̍s
            for (int z = 1; z < GridDepth; z++)
            {
                Vector3 gridLineStart = this[0, z] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2 * -1);
                Vector3 gridLineEnd = this[GridWidth - 1, z] + new Vector3(CellWidth / 2, _position.y, CellDepth / 2 * -1);

                Debug.DrawLine(gridLineStart, gridLineEnd, Color.red);
            }

            // ���̗�
            for (int x = 1; x < GridWidth; x++)
            {
                Vector3 gridRowStart = this[x, 0] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2 * -1);
                Vector3 gridRowEnd = this[x, GridDepth - 1] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2);

                Debug.DrawLine(gridRowStart, gridRowEnd, Color.red);
            }

            // �[�̃O���b�h���\��
            // �ŏ��̗�
            Debug.DrawLine(BottomLeft, TopLeft, Color.green);

            // �Ō�̗�
            Debug.DrawLine(BottomRight, TopRight, Color.green);


            // �ŏ��̍s
            Debug.DrawLine(BottomLeft, BottomRight, Color.green);

            // �Ō�̍s
            Debug.DrawLine(TopLeft, TopRight, Color.green);
        }


        /// <summary>
        /// ���ׂẴO���b�h���W�ɑ΂��ă��\�b�h�����s����C�e���[�^�ł�
        /// </summary>
        /// <param name="action">���\�b�h</param>
        public void IterateOverGrid(UnityAction<Coord> action)
        {
            Coord coord = new Coord();
            for (int z = 0; z < GridDepth; z++)
            {
                for (int x = 0; x < GridWidth; x++)
                {
                    coord.x = x;
                    coord.z = z;
                    action(coord);
                }
            }
        }

        /// <summary>
        /// ���ׂẴO���b�h���W�ɑ΂��ă��\�b�h�����s����C�e���[�^�ł�
        /// </summary>
        /// <param name="action">���\�b�h</param>
        public bool IterateOverGrid(Func<Coord, bool> action)
        {
            Coord coord = new Coord();
            for (int x = 0; x < GridWidth; x++)
            {
                for (int z = 0; z < GridDepth; z++)
                {
                    coord.x = x;
                    coord.z = z;
                    if (action(coord))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// ���ׂẴO���b�h���W�ɑ΂��ă��\�b�h�����s����C�e���[�^�ł�
        /// </summary>
        /// <param name="action">���\�b�h</param>
        public Coord FindGridCoord(Func<Coord, bool> action)
        {
            Coord coord = new Coord();
            for (int x = 0; x < GridWidth; x++)
            {
                for (int z = 0; z < GridDepth; z++)
                {
                    coord.x = x;
                    coord.z = z;
                    if(action(coord))
                    {
                        return coord;
                    }
                }
            }

            Debug.Log("OutOfBounds.(�O���b�h�t�B�[���h�O�ł��B)");
            return Coord.zero;
        }


        /// <summary>
        /// �����ɗ^���� position ���ǂ��̃O���b�h���W�ɂ���̂���Ԃ�
        /// </summary>
        /// <param name="pos">���ׂ����O���b�h�̂ǂ��̃Z���ɂ���̂����ׂ���Transform</param>
        /// <returns>Transform�̂���Z���̃O���b�h���W</returns>
        public Coord GridCoordinate(Vector3 pos) => FindGridCoord(coord => CheckOnCell(coord, pos));


        /// <summary>
        /// �^���� position �� �^�����O���b�h���W�̃Z���̏�ɂ��邩�ǂ����Ԃ��܂��B
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="coord"></param>
        /// <returns></returns>
        private bool CheckOnCell(Coord coord, Vector3 pos)
        {
            return pos.x <= this[coord.x, coord.z].x + CellWidth / 2 &&
                   pos.x >= this[coord.x, coord.z].x - CellWidth / 2 &&
                   pos.z <= this[coord.x, coord.z].z + CellDepth / 2 &&
                   pos.z >= this[coord.x, coord.z].z - CellDepth / 2;
        }


        /// <summary>
        /// �����ɗ^���� position ���ǂ��� �O���b�hposition �Ȃ̂��𒲂ׂ܂�
        /// </summary>
        /// <param name="pos">���ׂ����O���b�h�̂ǂ��̃Z���ɂ���̂����ׂ���Transform</param>
        /// <returns>Transform�̂���Z����position</returns>
        public Vector3 GridPosition(Vector3 pos) => this[GridCoordinate(pos).x, GridCoordinate(pos).z];


        /// <summary>
        /// �����ɗ^���� Vector3position �� �O���b�h���W�ɕϊ����܂�
        /// </summary>
        /// <param name="pos">�ϊ��������|�W�V����</param>
        public void ConvertVector3ToGridCoord(ref Vector3 pos)
        {
            pos = GridPosition(pos);
        }


        /// <summary>
        /// �����ɑΉ�����ЂƂO�̃O���b�h���W��Ԃ��܂�
        /// </summary>
        /// <param name="fourDirection">����</param>
        /// <returns>�����Ă�������̈�O�̃O���b�h���W</returns>
        public Coord PreviousCoordinate(FPS.eFourDirection fourDirection)
        {
            switch (fourDirection)
            {
                case FPS.eFourDirection.top:
                    return Coord.forward;

                case FPS.eFourDirection.bottom:
                    return Coord.back;

                case FPS.eFourDirection.left:
                    return Coord.left;

                case FPS.eFourDirection.right:
                    return Coord.right;
            }
            return Coord.zero;
        }

        /// <summary>
        /// �^���� posistion ���O���b�h�̏�ɂ��邩�ǂ������ׂ܂�
        /// </summary>
        /// <param name="pos">���ׂ����|�W�V����</pragma>
        public bool CheckOnGridPos(Vector3 pos) => CheckOnGridCoord(GridCoordinate(pos));


        /// <summary>
        /// �^�����O���b�h���W���O���b�h�̏�ɂ��邩�ǂ������ׂ܂�
        /// </summary>
        /// <param name="coord">���ׂ����|�W�V����</pragma>
        /// <returns>true : �O���b�h�̏�</returns>
        public bool CheckOnGridCoord(Coord coord) => IterateOverGrid(c => Coord.Equal(c, coord));


        /// <summary>
        /// �O���b�h���W��Instantiate
        /// </summary>
        /// <param name="original">�C���X�^���X�������</param>
        /// <param name="coord">���W</param>
        /// <param name="rotation">����</param>
        /// <returns>�C���X�^���X��������</returns>
        public UnityEngine.Object Instantiate(UnityEngine.Object original,Coord coord,Quaternion rotation)
        {
            Vector3 position = this[coord.x,coord.z];
            return UnityEngine.Object.Instantiate(original, position, rotation);
        }
        /// <summary>
        /// �O���b�h���W��Instantiate
        /// </summary>
        /// <param name="original">�C���X�^���X�������</param>
        /// <param name="coord">���W</param>
        /// <param name="y">����</param>
        /// <param name="rotation">����</param>
        /// <returns>�C���X�^���X��������</returns>
        public UnityEngine.Object Instantiate(UnityEngine.Object original, Coord coord,float y, Quaternion rotation)
        {
            Vector3 position = this[coord.x, coord.z];
            position.y = y;
            return UnityEngine.Object.Instantiate(original, position, rotation);
        }

        private void OnDrawGizmos()
        {
            // ���̍s
            for (int z = 1; z < GridDepth; z++)
            {
                Vector3 gridLineStart = this[0, z] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2 * -1);
                Vector3 gridLineEnd = this[GridWidth - 1, z] + new Vector3(CellWidth / 2, _position.y, CellDepth / 2 * -1);

                Gizmos.DrawLine(gridLineStart, gridLineEnd);
            }

            // ���̗�
            for (int x = 1; x < GridWidth; x++)
            {
                Vector3 gridRowStart = this[x, 0] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2 * -1);
                Vector3 gridRowEnd = this[x, GridDepth - 1] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2);

                Gizmos.DrawLine(gridRowStart, gridRowEnd);
            }

            // �[�̃O���b�h���\��
            // �ŏ��̗�
            Gizmos.DrawLine(BottomLeft, TopLeft);

            // �Ō�̗�
            Gizmos.DrawLine(BottomRight, TopRight);

            // �ŏ��̍s
            Gizmos.DrawLine(BottomLeft, BottomRight);

            // �Ō�̍s
            Gizmos.DrawLine(TopLeft, TopRight);

        }
    }
}