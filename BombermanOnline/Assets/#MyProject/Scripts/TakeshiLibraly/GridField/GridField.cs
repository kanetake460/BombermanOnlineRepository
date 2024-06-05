using System;
using UnityEngine;
using UnityEngine.Events;

namespace TakeshiLibrary
{
    /*=====グリッドフィールドを作成する関数=====*/
    // Vector3のクラスを参考に作成しました
    // C:\Users\kanet\AppData\Local\Temp\MetadataAsSource\b33e6428b1fe4c03a5b0b222eb1e9f0b\DecompilationMetadataAsSourceFileProvider\4496430b4e32462b86d5e9f4984747a4\Vector3.cs


    public class GridField
    {
        //======変数===========================================================================================================================
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
                    - new Vector3((float)(_gridWidth - 1) / 2 * _cellWidth, 0, (float)(_gridDepth - 1) / 2 * _cellDepth);    // xとzに10をかけた値を代入
            }
        }

        //======コンストラクタ=================================================================================================================

        public GridField(Vector3 position,int gridWidth,int gridDepth,float cellWidth,float cellDepth)
        {
            this._position  = position;
            this._gridWidth = gridWidth;
            this._gridDepth = gridDepth;
            this._cellWidth = cellWidth;
            this._cellDepth = cellDepth;
        }


        //======読み取り専用変数===============================================================================================================

        /// <summary>
        /// グリッドのセルの数を返します(読み取り専用)
        /// </summary>
        public int TotalCell
        {
            get
            {
                return GridWidth * GridDepth;
            }
        }

        /// <summary>
        /// グリッドの幅と奥行の最も長い方を返します。
        /// </summary>
        public int GridMaxLength
        {
            get
            {
                return Mathf.Max(GridWidth, GridDepth);
            }
        }
        
        /// <summary>
        /// グリッドの幅と奥行の最も短い方を返します。
        /// </summary>
        public int GridMinLength
        {
            get
            {
                return Mathf.Min(GridWidth, GridDepth);
            }
        }

        /// <summary>
        /// セルの幅と奥行の最も長い方を返します。
        /// </summary>
        public float CellMaxLength
        {
            get
            {
                return Mathf.Max(CellWidth, CellDepth);
            }
        }
        
        
        /// <summary>
        /// セルの幅と奥行の最も長い方を返します。
        /// </summary>
        public float CellMinLength
        {
            get
            {
                return Mathf.Min(CellWidth, CellDepth);
            }
        }


        /// <summary>
        /// フィールドの幅と奥行の最も長い方を返します。
        /// </summary>
        public float FieldMaxLength
        {
            get
            {
                return GridMaxLength * CellMaxLength;
            }
        }


        /// <summary>
        /// フィールドの幅と奥行の最も短い方を返します。
        /// </summary>
        public float FieldMinLength
        {
            get
            {
                return GridMinLength * CellMinLength;
            }
        }


        /*==========グリッドフィールドの角のセルのVector3座標==========*/
        /// <summary>
        ///グリッドの左下のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 BottomLeftCell
        {
            get
            {
                return this[0, 0];
            }
        }

        /// <summary>
        ///グリッドの右下のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 BottomRightCell
        {
            get
            {
                return this[GridWidth - 1, 0];
            }
        }

        /// <summary>
        ///グリッドの左上のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 TopLeftCell
        {
            get
            {
                return this[0, GridDepth - 1];
            }
        }

        /// <summary>
        ///グリッドの右上のセルの座標を返します。(読み取り専用)
        /// </summary>
        public Vector3 TopRightCell
        {
            get
            {
                return this[GridWidth - 1, GridDepth - 1];
            }
        }



        /*==========グリッドフィールドの角のVector3座標==========*/
        /// <summary>
        /// グリッドの左下の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 BottomLeft
        {
            get
            {
                return this[0, 0] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// グリッドの右下の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 BottomRight
        {
            get
            {
                return this[GridWidth - 1, 0] + new Vector3(CellWidth / 2, _position.y, CellDepth / 2 * -1);
            }
        }

        /// <summary>
        /// グリッドの左上の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 TopLeft
        {
            get
            {
                return this[0, GridDepth - 1] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2);
            }
        }

        /// <summary>
        /// グリッドの右上の位置座標を返します(読み取り専用)
        /// </summary>
        public Vector3 TopRight
        {
            get
            {
                return this[GridWidth - 1, GridDepth - 1] + new Vector3(CellWidth / 2, _position.y, CellDepth / 2);
            }
        }



        /*=========グリッドフィールドの中心Vector3座標===========*/
        /// <summary>
        /// グリッドの真ん中の localPosition を返します(読み取り専用)
        /// </summary>
        public Vector3 Middle
        {
            get
            {
                // 横幅奥行がどちらとも偶数
                if (GridWidth % 2 == 0 && GridDepth % 2 == 0)
                {
                    // グリッド座標からセルの半分の数減らした値を返す
                    return this[GridWidth / 2, GridDepth / 2] - new Vector3(CellWidth / 2, 0, CellDepth / 2);

                }
                // 横幅が偶数
                else if (GridWidth % 2 == 0)
                {
                    // グリッド座標からからセルの半分の数を減らした値を返す(横幅のみ)
                    return this[GridWidth / 2, GridDepth / 2] - new Vector3(CellWidth / 2, 0, 0);
                }
                // 奥行が偶数
                else if (GridDepth % 2 == 0)
                {
                    // グリッド座標からセルの半分の数を減らした値を返す(奥行のみ)
                    return this[GridWidth / 2, GridDepth / 2] - new Vector3(0, 0, CellDepth / 2);
                }
                // どちらとも奇数
                else
                {
                    // グリッド座標を返す
                    return this[GridWidth / 2, GridDepth / 2];
                }
            }
        }


        /*=========グリッドフィールドの中心Vector3座標===========*/
        /// <summary>
        /// グリッドの真ん中の localPosition を返します(読み取り専用)
        /// </summary>
        public Coord MiddleGrid
        {
            get
            {
                return new Coord(GridWidth / 2, GridDepth / 2);
            }
        }

        /*=========ランダム===========*/
        /// <summary>
        /// グリッド座標のランダムな位置を返します(読み取り専用)
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
        /// ランダムなグリッド座標を返します(読み取り専用)
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



        //======関数===========================================================================================================================

        ///<summary>
        ///シーンウィンドウにグリッドを表示します
        ///</summary>
        public void DrowGrid()
        {
            // 中の行
            for (int z = 1; z < GridDepth; z++)
            {
                Vector3 gridLineStart = this[0, z] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2 * -1);
                Vector3 gridLineEnd = this[GridWidth - 1, z] + new Vector3(CellWidth / 2, _position.y, CellDepth / 2 * -1);

                Debug.DrawLine(gridLineStart, gridLineEnd, Color.red);
            }

            // 中の列
            for (int x = 1; x < GridWidth; x++)
            {
                Vector3 gridRowStart = this[x, 0] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2 * -1);
                Vector3 gridRowEnd = this[x, GridDepth - 1] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2);

                Debug.DrawLine(gridRowStart, gridRowEnd, Color.red);
            }

            // 端のグリッド線表示
            // 最初の列
            Debug.DrawLine(BottomLeft, TopLeft, Color.green);

            // 最後の列
            Debug.DrawLine(BottomRight, TopRight, Color.green);


            // 最初の行
            Debug.DrawLine(BottomLeft, BottomRight, Color.green);

            // 最後の行
            Debug.DrawLine(TopLeft, TopRight, Color.green);
        }


        /// <summary>
        /// すべてのグリッド座標に対してメソッドを実行するイテレータです
        /// </summary>
        /// <param name="action">メソッド</param>
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
        /// すべてのグリッド座標に対してメソッドを実行するイテレータです
        /// </summary>
        /// <param name="action">メソッド</param>
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
        /// すべてのグリッド座標に対してメソッドを実行するイテレータです
        /// </summary>
        /// <param name="action">メソッド</param>
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

            Debug.Log("OutOfBounds.(グリッドフィールド外です。)");
            return Coord.zero;
        }


        /// <summary>
        /// 引数に与えた position がどこのグリッド座標にいるのかを返す
        /// </summary>
        /// <param name="pos">調べたいグリッドのどこのセルにいるのか調べたいTransform</param>
        /// <returns>Transformのいるセルのグリッド座標</returns>
        public Coord GridCoordinate(Vector3 pos) => FindGridCoord(coord => CheckOnCell(coord, pos));


        /// <summary>
        /// 与えた position が 与えたグリッド座標のセルの上にあるかどうか返します。
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
        /// 引数に与えた position がどこの グリッドposition なのかを調べます
        /// </summary>
        /// <param name="pos">調べたいグリッドのどこのセルにいるのか調べたいTransform</param>
        /// <returns>Transformのいるセルのposition</returns>
        public Vector3 GridPosition(Vector3 pos) => this[GridCoordinate(pos).x, GridCoordinate(pos).z];


        /// <summary>
        /// 引数に与えた Vector3position を グリッド座標に変換します
        /// </summary>
        /// <param name="pos">変換したいポジション</param>
        public void ConvertVector3ToGridCoord(ref Vector3 pos)
        {
            pos = GridPosition(pos);
        }


        /// <summary>
        /// 向きに対応するひとつ前のグリッド座標を返します
        /// </summary>
        /// <param name="fourDirection">向き</param>
        /// <returns>向いている方向の一つ前のグリッド座標</returns>
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
        /// 与えた posistion がグリッドの上にいるかどうか調べます
        /// </summary>
        /// <param name="pos">調べたいポジション</pragma>
        public bool CheckOnGridPos(Vector3 pos) => CheckOnGridCoord(GridCoordinate(pos));


        /// <summary>
        /// 与えたグリッド座標がグリッドの上にいるかどうか調べます
        /// </summary>
        /// <param name="coord">調べたいポジション</pragma>
        /// <returns>true : グリッドの上</returns>
        public bool CheckOnGridCoord(Coord coord) => IterateOverGrid(c => Coord.Equal(c, coord));


        /// <summary>
        /// グリッド座標版Instantiate
        /// </summary>
        /// <param name="original">インスタンスするもの</param>
        /// <param name="coord">座標</param>
        /// <param name="rotation">向き</param>
        /// <returns>インスタンスしたもの</returns>
        public UnityEngine.Object Instantiate(UnityEngine.Object original,Coord coord,Quaternion rotation)
        {
            Vector3 position = this[coord.x,coord.z];
            return UnityEngine.Object.Instantiate(original, position, rotation);
        }
        /// <summary>
        /// グリッド座標版Instantiate
        /// </summary>
        /// <param name="original">インスタンスするもの</param>
        /// <param name="coord">座標</param>
        /// <param name="y">高さ</param>
        /// <param name="rotation">向き</param>
        /// <returns>インスタンスしたもの</returns>
        public UnityEngine.Object Instantiate(UnityEngine.Object original, Coord coord,float y, Quaternion rotation)
        {
            Vector3 position = this[coord.x, coord.z];
            position.y = y;
            return UnityEngine.Object.Instantiate(original, position, rotation);
        }

        private void OnDrawGizmos()
        {
            // 中の行
            for (int z = 1; z < GridDepth; z++)
            {
                Vector3 gridLineStart = this[0, z] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2 * -1);
                Vector3 gridLineEnd = this[GridWidth - 1, z] + new Vector3(CellWidth / 2, _position.y, CellDepth / 2 * -1);

                Gizmos.DrawLine(gridLineStart, gridLineEnd);
            }

            // 中の列
            for (int x = 1; x < GridWidth; x++)
            {
                Vector3 gridRowStart = this[x, 0] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2 * -1);
                Vector3 gridRowEnd = this[x, GridDepth - 1] + new Vector3(CellWidth / 2 * -1, _position.y, CellDepth / 2);

                Gizmos.DrawLine(gridRowStart, gridRowEnd);
            }

            // 端のグリッド線表示
            // 最初の列
            Gizmos.DrawLine(BottomLeft, TopLeft);

            // 最後の列
            Gizmos.DrawLine(BottomRight, TopRight);

            // 最初の行
            Gizmos.DrawLine(BottomLeft, BottomRight);

            // 最後の行
            Gizmos.DrawLine(TopLeft, TopRight);

        }
    }
}