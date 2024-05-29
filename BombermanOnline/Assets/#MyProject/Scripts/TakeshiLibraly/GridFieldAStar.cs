using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// 参考サイト
// https://walkable-2020.com/unity/a-star-pathfinding/
// https://www.sejuku.net/blog/47172
// https://www.sejuku.net/blog/72918
// https://qiita.com/toRisouP/items/98cc4966d392b7f21c30
// https://yttm-work.jp/algorithm/algorithm_0015.html
namespace TakeshiLibrary
{
    public class GridFieldAStar
    {
        /// <summary>
        /// セルの情報
        /// </summary>
        private class CellInfo
        {
            public Coord coord { get; set; }       // 親セルのグリッド座標
            public float cost { get; set; }           // 実コスト
            public float heuristicCost { get; set; }  // 推定コスト
            public float sumCost { get; set; }        // 総コスト
            public CellInfo parent { get; set; }      // 親のセルの位置
        }

        public Stack<Coord> pathStack { get; } = new Stack<Coord>();    // 道のりの座標スタック
        
        private GridFieldMapSettings _map;                                      // マップ
        private List<CellInfo> _openList = new List<CellInfo>();        // オープンリスト
        private List<CellInfo> _closeList = new List<CellInfo>();       // クローズリスト
        private Coord _goal;                                            // ゴール座標
        private Coord _start;                                           // スタート座標
        private readonly int _searchLimit = 1000;                       // 探索限界値

        /// <summary>
        /// AStarのコンストラクタ(与えたグリッドフィールドマップでA*探索を行います)
        /// </summary>
        /// <param name="gf">グリッド座標</param>
        /// <param name="position"></param>
        /// <param name="targetPos"></param>
        public GridFieldAStar(int searchLimit = 1000)
        {
            _searchLimit = searchLimit;
        }


        /// <summary>
        /// 経路を探索します
        /// </summary>
        /// <param name="map"></param>
        /// <param name="position"></param>
        /// <param name="targetPos"></param>
        /// <returns>ゴールを発見したかどうか true：探索成功</returns>
        public bool AStarPath(GridFieldMapSettings map, Coord position, Coord targetPos)
        {
            _map = map;
            _start = position;
            _goal = targetPos;

            // スタート地点のセルを入れる
            _openList.Add(GetSatrtCell());

            int count = 0;
            CellInfo minCell = new CellInfo();

            // リミットの限界になるか、ゴールが発見できるまでオープンし続ける
            while (count < _searchLimit)
            {
                count++;

                // リストの中から総コストが最も低いものを格納
                minCell = _openList.OrderBy(x => x.sumCost).First();

                // ゴールと一致するcellがリストにあるならなら終了
                if (minCell.coord == _goal) break;

                // 最も低いセルの周りをオープンにする
                OpenCell(minCell);
            }

            // 探索リミットに達したらスタート位置をスタックして終了
            if (count >= _searchLimit)
            {
                pathStack.Push(GetSatrtCell().coord);
                Debug.Log("ゴールが見つからないまま探索が中断されました");
                return false;
            }
            else
            {
                // ゴールにたどり着いたセルから順にたどってスタートまでの道のりをスタック
                StackPath(minCell);
                return true;
            }
        }


        /// <summary>
        /// 探索開始地点のセルをオープンリストに入れます
        /// </summary>
        private CellInfo GetSatrtCell()
        {
            // 探索開始地点のセル
            CellInfo startCell = new CellInfo();
            startCell.coord = _start;
            startCell.cost = 0;
            startCell.heuristicCost = Vector2Int.Distance(new Vector2Int(_start.x,_start.z), new Vector2Int(_goal.x, _goal.z));
            startCell.sumCost = startCell.cost + startCell.heuristicCost;

            return startCell;
        }


        /// <summary>
        /// 真ん中の周りのセルをオープンにします
        /// </summary>
        /// <param name="center">真ん中に設定するセル</param>
        private void OpenCell(CellInfo center)
        {
            Coord centerPos = center.coord;

            // 真ん中をクローズリストに追加
            _openList.Remove(center);
            _closeList.Add(center);

            // 上下左右、真ん中は含めない
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    // 斜めは探索しない
                    if (x != 0 && z != 0) continue;

                    // 真ん中は探索しない
                    if (x == 0 && z == 0) continue;

                    Coord searchPos = new Coord(centerPos.x + x, centerPos.z + z);

                    // マップの外なら探索しないしない
                    if (searchPos.x >= _map.gridField.GridWidth ||
                        searchPos.z >= _map.gridField.GridDepth ||
                        searchPos.x < 0 || searchPos.z < 0) continue;

                    // マップの壁マスなら探索しない
                    if (_map.blocks[searchPos.x, searchPos.z].isSpace == false) continue;

                    // その向きが壁なら探索しない
                    if (_map.blocks[centerPos.x, centerPos.z].CheckWall(x,z))
                    {
                        Debug.Log("壁にぶつかりました");
                        continue;
                    }

                    CellInfo aroundCell = new CellInfo();

                    // 周りのセルに情報を入れる
                    aroundCell.coord = searchPos;
                    aroundCell.cost = center.cost + 1;
                    aroundCell.heuristicCost = Vector2Int.Distance(new Vector2Int(searchPos.x, searchPos.z), new Vector2Int(_goal.x, _goal.z));
                    aroundCell.sumCost = aroundCell.cost + aroundCell.heuristicCost;
                    aroundCell.parent = center;

                    // オープンリストに存在して、合計コストがオープンしたいセルより大きい
                    if (_openList.Any(c => c.coord == aroundCell.coord))
                    {
                        if (_openList.Find(x => x.coord == searchPos).sumCost > aroundCell.sumCost)
                        {
                            // リストにあるものを削除し、新しい方を追加
                            var o = _openList.Find(x => x.coord == searchPos);
                            _openList.Remove(o);
                            _openList.Add(aroundCell);
                            continue;
                        }
                    }

                    // クローズリストに存在して、合計コストがオープンしたいセルより大きい
                    if (_closeList.Any(c => c.coord == aroundCell.coord))
                    {
                        if (_closeList.Find(x => x.coord == searchPos).sumCost > aroundCell.sumCost)
                        {
                            // リストにあるものを削除し、新しい方を追加
                            var c = _closeList.Where(y => y.coord == searchPos).First();
                            _closeList.Remove(c);
                            _openList.Add(aroundCell);
                            continue;
                        }
                    }

                    // 両方のリストにセルが一つも存在しない
                    if (_openList.Contains(aroundCell) == false && _closeList.Contains(aroundCell) == false)
                    {
                        // オープンリストに追加
                        _openList.Add(aroundCell);
                        continue;
                    }
                }
            }
        }


        /// <summary>
        /// 道のりをスタックします
        /// </summary>
        /// <param name="cell">スタックするセルの先頭</param>
        private void StackPath(CellInfo cell)
        {
            // リストをクリアする
            _openList.Clear();
            _closeList.Clear();
            pathStack.Clear();
            
            int count = 0;
            CellInfo preCell = cell;
            
            // ゴールとスタートの位置が同じ
            if(preCell.coord == _start)
            {
                pathStack.Push(cell.coord);
            }

            // 前回のセルがスタートの位置じゃない限りスタックにため続ける
            while (preCell.parent != null)
            {
                // ポップしないと無限ループが発生してします。
                pathStack.Push(preCell.coord);

                // プッシュしたセルの親セルを代入
                preCell = preCell.parent;
                
                // 無限ループ対策
                count++;
                if (count > 10000) break;
            }
        }
    }
}
