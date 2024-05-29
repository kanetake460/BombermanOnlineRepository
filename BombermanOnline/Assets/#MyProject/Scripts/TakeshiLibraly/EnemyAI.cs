using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    public class EnemyAI
    {
        /*コンポーネント*/
        private GridFieldAStar _aStar;          // AStar
        private GridFieldMapSettings _map;              // マップ
        private TakeshiLibrary.Compass _copass; // コンパス
        
        /*座標*/
        private Coord _pathTargetCoord;         // 道のりのターゲットの座標
        private Transform _enemyTrafo;          // エネミーのトランスフォーム
        
        /*パラメーター*/
        private int _stayCount = 0;             // AStarLocomotionの再探索までのカウント

        /// <summary>
        /// 道のりターゲットのVector3座標
        /// </summary>
        public Vector3 PathTargetPos
        {
            get { return _map.gridField[_pathTargetCoord.x, _pathTargetCoord.z]; }
        }

        public EnemyAI(Transform enemyTrafo,GridFieldMapSettings map,int searchLimit)
        {
            _aStar = new GridFieldAStar(searchLimit);
            _map = map;
            _copass = new Compass(enemyTrafo);
            _enemyTrafo = enemyTrafo;
            _pathTargetCoord = _map.gridField.GridCoordinate(_enemyTrafo.position);
        }


        /// <summary>
        /// ある地点まで Vector3 の値まで動かします
        /// </summary>
        /// <param name="trafo">動かす物のトランスフォーム</param>
        /// <param name="point">目的地</param>
        /// <param name="speed">動かすスピード</param>
        /// <returns>ポイントに到達したらtrueを返します</returns>
        public bool MoveToPoint(Transform trafo, Vector3 point, float speed = 1)
        {
            Vector3 pos = trafo.position;

            pos.x += pos.x <= point.x ? speed * 0.01f : speed * -0.01f;
            pos.y += pos.y <= point.y ? speed * 0.01f : speed * -0.01f;
            pos.z += pos.z <= point.z ? speed * 0.01f : speed * -0.01f;

            if (pos.x <= point.x + speed * 0.1f && pos.x >= point.x + speed * -0.1f) pos.x = point.x;
            if (pos.y <= point.y + speed * 0.1f && pos.y >= point.y + speed * -0.1f) pos.y = point.y;
            if (pos.z <= point.z + speed * 0.1f && pos.z >= point.z + speed * -0.1f) pos.z = point.z;

            trafo.position = pos;

            return pos == point;
        }


        /// <summary>
        /// ある地点まで Vector3 の値まで動かします
        /// </summary>
        /// <param name="trafo">動かす物のトランスフォーム</param>
        /// <param name="point">目的地</param>
        /// <param name="speed">動かすスピード</param>
        /// <returns>ポイントに到達したらtrueを返します</returns>
        public bool LocomotionToCoordPoint(Coord coord, float speed = 1)
        {
            Vector3 pos = _enemyTrafo.position;
            Vector3 point = _map.gridField[coord.x, coord.z];

            Vector3 direction = (point - pos).normalized;


            _copass.TurnTowardToPoint(PathTargetPos);

            pos += direction * speed * Time.deltaTime;

            if(Vector3.Distance(point,pos) < 0.1f)
            {
                pos = point;

            }
            _enemyTrafo.position = pos;

            return pos == point;
        }


        /// <summary>
        /// 最短経路で目的地まで動かします。一度目的地についたら終了します。
        /// </summary>
        /// <param name="moveSpeed">追いかけるスピード</param>
        /// <returns>到着したら true</returns>
        public bool LocomotionToAStar( float moveSpeed = 1)
        {
            // パススタックがあるなら
            if (_aStar.pathStack.Count != 0)
            {
                // パスターゲットに追いついたら
                if (LocomotionToCoordPoint(_pathTargetCoord, moveSpeed)) 
                {
                    // 新しいパスターゲットをポップ
                    _pathTargetCoord = _aStar.pathStack.Pop();
                    Debug.DrawLine(_map.gridField[_pathTargetCoord.x, _pathTargetCoord.z], _map.gridField[_pathTargetCoord.x, _pathTargetCoord.z] + Vector3.up, Color.red, 0.1f);
                }
            }
            // ない場合
            else
            {
                // エネミーが最後のパスターゲットの位置に来てなかったら
                if(_enemyTrafo.position != _map.gridField[_pathTargetCoord.x,_pathTargetCoord.z])
                {
                    LocomotionToCoordPoint(_pathTargetCoord, moveSpeed);
                }
                else { return true; }
            }

            return false;
        }


        /// <summary>
        /// 最短経路で目的地まで動かし続けます。
        /// </summary>
        /// <param name="goalPos">追いかける物の位置</param>
        /// <param name="aStarCount">再探索を行う間隔</param>
        /// <param name="moveSpeed">追いかけるスピード</param>
        /// <returns>追いついたらtrue</returns>
        public void StayLocomotionToAStar(Vector3 goalPos,float moveSpeed = 1, int aStarCount = 360)
        {
            LocomotionToAStar(moveSpeed);

            _stayCount++;
            if(_stayCount > aStarCount)
            {
                _stayCount = 0;
                EnterLocomotionToAStar(goalPos);
            }
        }


        /// <summary>
        /// AStarクラスからパスを設定します。
        /// </summary>
        /// <param name="goalPos">ゴールのVector3座標</param>
        /// <returns>ゴールが探索できたかどうか</returns>
        public bool EnterLocomotionToAStar(Vector3 goalPos)
        {
            var enemyCoord = _map.gridField.GridCoordinate(_enemyTrafo.position) ;
            var locoGoalCoord = _map.gridField.GridCoordinate(goalPos);

            // パスを作って、エネミーのいる場所を最初の場所にする
            if (!_aStar.AStarPath(_map, enemyCoord, locoGoalCoord))
            {
                Debug.Log("見失いました。");
                return false;
            }
            else
            {
                _pathTargetCoord = _aStar.pathStack.Pop();
                return true;
            }
        }


        /// <summary>
        /// 移動を終了させます
        /// </summary>
        /// <param name="isExit">終了したかどうか</param>
        public void ExitLocomotion(ref bool isExit)
        {
            isExit = false;

            _stayCount = 0;
            _aStar.pathStack.Clear();
        }


        /// <summary>
        /// 指定した範囲でランダムな地点を目標としてエネミーを徘徊させます
        /// </summary>
        public void Wandering(float moveSpeed, int areaX = 10, int areaZ = 10)
        {
            // 徘徊ポイントについたらランダムな位置を徘徊ポイントにする
            if (LocomotionToAStar(moveSpeed))
            {
                var enemyCoord = _map.gridField.GridCoordinate(_enemyTrafo.position);
                var searchList = _map.AreaBlockList(enemyCoord, areaX, areaZ);
                var locoGoalCoord =  searchList.FindAll(b => b.isSpace)
                    [Random.Range(0, searchList.FindAll(b => b.isSpace).Count - 1)].coord;

                EnterLocomotionToAStar(_map.gridField[locoGoalCoord.x,locoGoalCoord.z]);
            }
        }


        /// <summary>
        /// 指定したマス先のランダムな地点を目標としてエネミーを徘徊させます
        /// </summary>
        public void CustomWandering(float moveSpeed,List<Coord> exceptionCoordList,int frameSize = 1, int areaX = 10, int areaZ = 10)
        {
            // 徘徊ポイントについたらランダムな位置を徘徊ポイントにする
            if (LocomotionToAStar(moveSpeed))
            {
                var enemyCoord = _map.gridField.GridCoordinate(_enemyTrafo.position);
                var searchList = _map.CustomFrameAreaBlockList(enemyCoord, frameSize, exceptionCoordList, areaX, areaZ);
                var locoGoalCoord =  searchList.FindAll(b => b.isSpace)
                    [Random.Range(0, searchList.FindAll(b => b.isSpace).Count - 1)].coord;

                if(EnterLocomotionToAStar(_map.gridField[locoGoalCoord.x, locoGoalCoord.z]) == false)
                {
                    _aStar.pathStack.Clear();
                }

            }
        }


        /// <summary>
        /// レイキャストにより、プレイヤーを探します
        /// </summary>
        /// <param name="searchLayer">プレイヤーと壁のレイヤー</param>
        /// <param name="playerTag">プレイヤーのタグ</param>
        /// <param name="raySize">レイキャストの大きさ（）セルの横幅から引く値</param>
        /// <returns>みつかったかどうかtrue：発見</returns>
        public bool SearchPlayer(LayerMask searchLayer,string playerTag, float raySize = 5)
        {
            RaycastHit hit;
            // 向き
            Vector3 dir = _enemyTrafo.forward;
            // ボックスレイの大きさ
            Vector3 size = Vector3.one * raySize / 2;
            // レイの発射地点
            Vector3 point = _enemyTrafo.position;
            
            // 1セル分減らす
            point -= dir * _map.gridField.CellMaxLength;
            // フィールドのWidhtとDepthで長い方をレイの最大距離に設定
            float rayDist = _map.gridField.GridMaxLength * _map.gridField.CellMaxLength;
            if (Physics.BoxCast(point, size, dir, out hit, Quaternion.identity, rayDist, searchLayer))
            {
                if (hit.collider.CompareTag(playerTag))
                {
                    return true;
                }
            }
            return false;
        }
    }
}