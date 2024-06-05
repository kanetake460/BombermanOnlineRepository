using System;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    /// <summary>
    /// マップ設定クラス
    /// </summary>
    [Serializable]
    public class GridFieldMapSettings : MonoBehaviour
    {
        /// <summary>
        /// ブロッククラス
        /// </summary>
        public class Block
        {
            // ブロックのグリッド座標
            public Coord coord { get; }
            // ブロックの種類
            public bool isSpace { get; set; }

            // 壁があるかどうか( 壁がある : true )
            public bool fowardWall { get; set; } = false;
            public bool rightWall { get; set; } = false;
            public bool backWall { get; set; } = false;
            public bool leftWall { get; set; } = false;

            // オブジェクト設定
            public GameObject wallObjParent { get; set; }
            public GameObject planeObjParent { get; set; }
            public GameObject wallObj { get; set; }         // 壁オブジェクト
            public GameObject planeObj { get; set; }        // 床オブジェクト
            public Renderer wallRenderer { get; set; }      // 壁レンダラ
            public Renderer planeRenderer { get; set; }     // 床レンダラ

            /// <summary>
            /// ブロックに情報を入れるコンストラクタ
            /// </summary>
            /// <param name="x">xグリッド座標</param>
            /// <param name="z">zグリッド座標</param>
            /// <param name="isSpace">壁か、空間か</param>
            public Block(int x, int z, bool isSpace = true)
            {
                coord = new Coord(x, z);
                this.isSpace = isSpace;
            }

            /// <summary>
            /// 与えたVector3座標の向きが壁なのかどうか調べます
            /// </summary>
            /// <param>Vector3の向き</param>
            /// <returns>壁かどうなのか</returns>
            public bool CheckWallDirection(Vector3 dir)
            {
                if (dir == Vector3.right) return rightWall;
                else if (dir == Vector3.left) return leftWall;
                else if (dir == Vector3.forward) return fowardWall;
                else if (dir == Vector3.back) return backWall;
                else return false;
            }

            /// <summary>
            /// 与えた座標の向きが壁なのかどうか調べます
            /// </summary>
            /// <param name="x">奥( -1 or 0 )</param>
            /// <param name="z">横( -1 or 0 )</param>
            /// <returns>壁かどうなのか</returns>
            public bool CheckWall(int x, int z)
            {
                Vector3 checkDir = new Vector3(x, 0, z);
                return CheckWallDirection(checkDir);
            }


            /// <summary>
            /// 壁のテクスチャを変更します。
            /// </summary>
            /// <param name="texture">テクスチャ</param>
            public void SetWallMaterial(Texture texture) { wallRenderer.material.mainTexture = texture; }
            /// <summary>
            /// 壁の色を変更します。
            /// </summary>
            /// <param name="color">色</param>
            public void SetWallMaterial(Color color) { wallRenderer.material.color = color; }
            /// <summary>
            /// 壁のテクスチャと色を変更します。
            /// </summary>            
            /// <param name="texture">テクスチャ</param>
            /// <param name="color">色</param>
            public void SetWallMaterial(Texture texture, Color color) { wallRenderer.material.mainTexture = texture; wallRenderer.material.color = color; }


            /// <summary>
            /// 床のテクスチャを変更します。
            /// </summary>
            /// <param name="texture">テクスチャ</param>
            public void SetPlaneMaterial(Texture texture) { planeRenderer.material.mainTexture = texture; }
            /// <summary>
            /// 床の色を変更します。
            /// </summary>
            /// <param name="color">色</param>
            public void SetPlaneMaterial(Color color) { planeRenderer.material.color = color; }
            /// <summary>
            /// 床のテクスチャと色を変更します。
            /// </summary>            
            /// <param name="texture">テクスチャ</param>
            /// <param name="color">色</param>
            public void SetPlaneMaterial(Texture texture, Color color) { planeRenderer.material.mainTexture = texture; wallRenderer.material.color = color; }

        }
        // ===================================================================================================

        /// <summary>
        /// パラメーター
        /// </summary>
        [Header("GridField")]
        public Vector3 position;
        [Range(0, 30)] public int gridWidth;
        [Range(0, 30)] public int gridDepth;
        public float cellWidth;
        public float cellDepth;

        // ブロックのisSpaceを割り当てるプロパティ
        public bool[] isSpaceProp;
        // グリッドフィールド
        public GridField gridField;

        public Block[,] blocks = new Block[100, 100];

        // ブロックの一次元配列ゲッター
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
        /// 指定した座標を壁ブロックに設定します
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        public void SetWallBlock(int x, int z)
        {
            blocks[x, z].isSpace = false;
        }

        /// <summary>
        /// 指定したブロックを壁ブロックに設定します
        /// </summary>
        /// <param name="block">壁にしたいブロック</param>
        public void SetWallBlock(Block block)
        {
            block.isSpace = false;
        }


        /// <summary>
        /// 指定した座標のブロック、向きを壁に設定します
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        /// <param name="dir">壁を入れる向き</param>
        public void SetWallDirection(int x, int z, Vector3 dir)
        {
            if (dir == Vector3.forward) blocks[x, z].fowardWall = true;
            else if (dir == Vector3.right) blocks[x, z].rightWall = true;
            else if (dir == Vector3.back) blocks[x, z].backWall = true;
            else if (dir == Vector3.left) blocks[x, z].leftWall = true;
        }


        /// <summary>
        /// 指定したブロックの、向きを壁に設定します
        /// </summary>
        /// <param name="block">設定したいブロック</param>
        /// <param name="dir">壁を入れる向き</param>
        public void SetWallDirection(Block block, Vector3 dir)
        {
            if (dir == Vector3.forward) block.fowardWall = true;
            else if (dir == Vector3.right) block.rightWall = true;
            else if (dir == Vector3.back) block.backWall = true;
            else if (dir == Vector3.left) block.leftWall = true;
        }


        /// <summary>
        /// 与えた座標のすべての向きの壁を設定します
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        /// <param name="foward">前壁</param>
        /// <param name="right">右壁</param>
        /// <param name="back">後壁</param>
        /// <param name="left">左壁</param>
        public void SetWallsDirection(int x, int z, bool foward = true, bool right = true, bool back = true, bool left = true, bool isSpace = false)
        {
            blocks[x, z].fowardWall = foward;
            blocks[x, z].rightWall = right;
            blocks[x, z].backWall = back;
            blocks[x, z].leftWall = left;
            blocks[x, z].isSpace = isSpace;
        }


        /// <summary>
        /// あたえたブロックのすべての向きの壁を設定します
        /// デフォルト引数では壁があります
        /// </summary>
        /// <param name="back">ブロック</param>
        /// <param name="foward">前壁</param>
        /// <param name="right">右壁</param>
        /// <param name="back">後壁</param>
        /// <param name="left">左壁</param>
        public void SetWallsDirection(Block block, bool foward = true, bool right = true, bool back = true, bool left = true, bool isSpace = false)
        {
            block.fowardWall = foward;
            block.rightWall = right;
            block.backWall = back;
            block.leftWall = left;
            block.isSpace = isSpace;
        }


        /// <summary>
        /// 指定した座標のブロック、向きの壁をなくします
        /// </summary>
        /// <param name="x">xグリッド座標</param>
        /// <param name="z">zグリッド座標</param>
        /// <param name="dir">壁を入れる向き</param>
        public void BreakWall(int x, int z, Vector3 dir)
        {
            if (dir == Vector3.forward) blocks[x, z].fowardWall = false;
            else if (dir == Vector3.right) blocks[x, z].rightWall = false;
            else if (dir == Vector3.back) blocks[x, z].backWall = false;
            else if (dir == Vector3.left) blocks[x, z].leftWall = false;
        }


        /// <summary>
        /// 指定したブロック、向きの壁をなくします
        /// </summary>
        /// <param name="block">ブロック</param>
        /// <param name="dir">壁を入れる向き</param>
        public void BreakWall(Block block, Vector3 dir)
        {
            if (dir == Vector3.forward) block.fowardWall = false;
            else if (dir == Vector3.right) block.rightWall = false;
            else if (dir == Vector3.back) block.backWall = false;
            else if (dir == Vector3.left) block.leftWall = false;
        }


        /// <summary>
        /// 条件に当てはまる座標にブロックを生成します。
        /// </summary>
        /// <param name="func">条件</param>
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
        /// 条件に当てはまる座標のブロックリストを返します
        /// </summary>
        /// <param name="func">条件</param>
        /// <returns>当てはまるブロックのリスト</returns>
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
        /// マップのすべてのブロックを壁に設定します
        /// </summary>
        public void CreateWallsAll() => CreateWalls(a => true);


        /// <summary>
        /// グリッド状に壁を生成します
        /// </summary>
        public void CreateWallsOddGrid() => CreateWalls(coord => coord.x % 2 == 1 && coord.z % 2 == 1);
        public void CreateWallsEvenGrid() => CreateWalls(coord => coord.x % 2 == 0 && coord.z % 2 == 0);


        /// <summary>
        /// マップを囲むように壁を設定します
        /// </summary>
        public void CreateWallsSurround() => CreateWalls(coord => coord.x == 0 ||
                                                                  coord.z == 0 ||
                                                                  coord.x == gridField.GridWidth - 1 ||
                                                                  coord.z == gridField.GridDepth - 1);


        /// <summary>
        /// 与えたグリッド座標がマップないならfalseを返します
        /// </summary>
        /// <param name="coord">座標</param>
        /// <returns>グリッドの上ならtrue</returns>
        public bool CheckMap(Coord coord)
        {
            return coord.x >= 0 &&
                    coord.z >= 0 &&
                    coord.x < gridField.GridWidth &&
                    coord.z < gridField.GridDepth;
        }


        /// <summary>
        /// 指定した座標から指定の範囲のすべてのブロックを返します
        /// </summary>
        /// <param name="coord">中心座標</param>
        /// <param name="areaX">Xの長さ</param>
        /// <param name="areaZ">Zの長さ</param>
        public List<Block> AreaBlockList(Coord coord, int areaX, int areaZ)
        {
            // 選択範囲のブロックのリスト
            List<Block> lAreaBlock = new List<Block>();

            // 検索範囲のブロックをリストに追加
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
        /// 指定した座標から指定の範囲の"指定した座標以外の"すべてのブロックを返します
        /// </summary>
        /// <param name="coord">中心座標</param>
        /// <param name="exceptionCoordList">除外する座標のリスト</param>
        /// <param name="areaX"></param>
        /// <param name="areaZ"></param>
        public List<Block> CustomAreaBlockList(Coord coord, List<Coord> exceptionCoordList, int areaX, int areaZ)
        {
            // 選択範囲のブロックのリスト
            List<Block> lAreaBlock = AreaBlockList(coord, areaX, areaZ);

            lAreaBlock.RemoveAll(b => exceptionCoordList.Contains(b.coord));

            return lAreaBlock;
        }


        /// <summary>
        /// 指定した座標から指定の範囲のブロックの"フレーム状にある"ブロックを返します
        /// </summary>
        /// <param name="coord">中心座標</param>
        /// <param name="frameSize">フレームのサイズ</param>
        /// <param name="areaX">Xの長さ</param>
        /// <param name="areaZ">Zの長さ</param>
        /// <returns></returns>
        public List<Block> FrameAreaBlockList(Coord coord, int frameSize, int areaX, int areaZ)
        {
            // 選択範囲のブロックのリスト
            List<Block> lAreaBlock = AreaBlockList(coord, areaX, areaZ);

            // エリアから、frameSizeの値分内側のエリアのブロックを削除
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
        /// 指定した座標から指定の範囲のブロックの"フレーム状にある"ブロックを返します
        /// </summary>
        /// <param name="coord">中心座標</param>
        /// <param name="frameSize">フレームのサイズ</param>
        /// <param name="exceptionCoordList">除外する座標のリスト</param>
        /// <param name="areaX">Xの長さ</param>
        /// <param name="areaZ">Zの長さ</param>
        public List<Block> CustomFrameAreaBlockList(Coord coord, int frameSize, List<Coord> exceptionCoordList, int areaX, int areaZ)
        {
            // 選択範囲のブロックのリスト
            List<Block> lAreaBlock = FrameAreaBlockList(coord, frameSize, areaX, areaZ);

            lAreaBlock.RemoveAll(b => exceptionCoordList.Contains(b.coord));

            return lAreaBlock;
        }


        /// <summary>
        /// AStarの道を設定します
        /// </summary>
        /// <param name="start">探索の最初の位置</param>
        /// <param name="goal">探索のゴール地点</param>
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