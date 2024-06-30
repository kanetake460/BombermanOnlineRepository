using System;
using System.Collections.Generic;
using UnityEngine;


namespace TakeshiLibrary
{
    /*=====FPSの移動関連のスクリプトです=====*/
    // 参考サイト
    //https://www.popii33.com/unity-first-person-camera/
    [Serializable]
    public class FPS
    {
        private GridFieldMapSettings _map;
        private Vector3 _latePos;
        private bool _cursorLock;
        private Rigidbody _rb;
        private GameObject _player;
        private GameObject _camera;

        public FPS(GridFieldMapSettings map,Rigidbody rb,GameObject player,GameObject camera)
        {
            _map = map;
            _rb = rb;
            _player = player;
            _camera = camera;
        }

        public enum eFourDirection
        {
            top = 0,
            bottom = 180,
            left = 270,
            right = 90,
            No = 0,
        }


        /// <summary>
        /// カメラの角度制限をします(上下)
        /// </summary>
        /// <param name="q">制限したいquoternion</param>
        /// <param name="minX">下の角度制限</param>
        /// <param name="maxX">上の角度制限</param>
        /// <returns>角度</returns>
        private Quaternion ClampRotation(Quaternion q, float minX, float maxX)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1f;

            float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

            angleX = Mathf.Clamp(angleX, minX, maxX);

            q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

            return q;
        }


        ///<summary>カメラの視点移動関数(上下の視点移動)</summary>>
        ///<param name="Xsensityvity"<pragma>視点移動スピード</pragma>
        ///<param name="minX"<pragma>下の角度制限</pragma>
        ///<param name="maxX"<pragma>上の角度制限</pragma>
        public void CameraViewport(float Xsensityvity = 3f, float minX = -90f, float maxX = 90f)
        {
            float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;       // マウスの座標代入
            _camera.transform.localRotation *= Quaternion.Euler(-yRot, 0, 0);     // 角度代入

            _camera.transform.localRotation = ClampRotation(_camera.transform.localRotation, minX, maxX);           // 角度制限
        }


        /// <summary>
        /// プレイヤーの視点移動関数(左右視点移動)
        /// </summary>
        public void PlayerViewport(float Ysensityvity = 3f)
        {
            float xRot = Input.GetAxis("Mouse X") * Ysensityvity;               // マウスの座標代入
            _player.transform.localRotation *= Quaternion.Euler(0, xRot, 0);     // 角度代入
        }


        /// <summary>
        /// プレイヤーをキー入力によって移動させます
        /// </summary>
        /// <param name="speed">移動スピード</param>
        public void Locomotion(float speed = 10f,float dashSpeed = 15,KeyCode dashuKey = KeyCode.LeftShift)
        {
            if (Input.GetKey(dashuKey)) speed = dashSpeed;

            float x = Input.GetAxisRaw("Horizontal") * speed;     // 移動入力
            float z = Input.GetAxisRaw("Vertical") * speed;       // 移動入力

            _player.transform.position += _player.transform.forward * z * Time.deltaTime + _player.transform.right * x * Time.deltaTime;  // 移動
            _camera.transform.position = _player.transform.position;

        }

        /// <summary>
        /// プレイヤーをキー入力によって移動させます
        /// </summary>
        /// <param name="player">動かすプレイヤー</param>
        /// <param name="speed">移動スピード</param>
        public float VelocityForceLocomotion(float speed = 10f, float dashSpeed = 15, float backSpeed = 5, KeyCode dashKey = KeyCode.LeftShift)
        {
            if (Input.GetKey(dashKey)) speed = dashSpeed;

            float x = Input.GetAxisRaw("Horizontal");     // 移動入力
            float z = Input.GetAxisRaw("Vertical");       // 移動入力

            if(x != 0 &&  z != 0)
                speed *= 0.707f;

            if (z < 0)
                speed = backSpeed;

            _rb.velocity = _player.transform.right * x * speed + _player.transform.forward * z * speed;

            return x + z;
        }
        public void AddForceLocomotion(float speed = 10f, float dashSpeed = 15, KeyCode dashKey = KeyCode.LeftShift)
        {
            Vector3 movement = Vector3.zero;

            if (Input.GetKey(dashKey))
            {
                movement = (_player.transform.right * Input.GetAxisRaw("Horizontal") + _player.transform.forward * Input.GetAxisRaw("Vertical")).normalized * dashSpeed;
            }
            else
            {
                movement = (_player.transform.right * Input.GetAxisRaw("Horizontal") + _player.transform.forward * Input.GetAxisRaw("Vertical")).normalized * speed;
            }

            if (movement.magnitude > 0)
            {
                if (Input.GetAxisRaw("Horizontal") != 0 && Input.GetAxisRaw("Vertical") != 0)
                {
                    movement *= 0.707f; // 斜め移動の速度補正
                }

                _rb.AddForce(movement, ForceMode.VelocityChange);
            }
        }



        /// <summary>
        /// カーソルをロックします
        /// </summary>
        /// <param name="cursorLock">カーソルロックフラグ</param>
        public void CursorLock()
        {
            if (Input.GetKeyDown(KeyCode.Escape))   // エスケープキーを押したら
            {
                _cursorLock = false;
            }
            else if (Input.GetMouseButton(0))       // 左クリック
            {
                _cursorLock = true;
            }
            if (_cursorLock)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (_cursorLock == false)
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }


        /// <summary>
        /// 与えたトランスフォームが壁ブロックに入らないようにします
        /// </summary>
        /// <param name="trafo">プレイヤートランスフォーム</param>
        public void ClampMoveRange()
        {
            if(_latePos == Vector3.zero) return;
            Coord coord = _map.gridField.GridCoordinate(_player.transform.position);

            if (_map.blocks[coord.x,coord.z].isSpace == false)
            {
                _player.transform.position = _latePos;
            }
            _latePos = _player.transform.position;
        }


        /// <summary>
        /// プレイヤーの向きから四方向の列挙子を返します
        /// </summary>
        /// <param name="rot">プレイヤーの向き</param>
        /// <returns>向きの列挙子</returns>
        public static eFourDirection GetFourDirection(Vector3 rot)
        {
            if (rot.y > 225f && rot.y <= 315)
            {
                return eFourDirection.left;
            }
            else if (rot.y > 45f && rot.y <= 135f)
            {
                return eFourDirection.right;
            }
            else if (rot.y > 135f && rot.y <= 225f)
            {
                return eFourDirection.bottom;
            }
            else
            {
                return eFourDirection.top;
            }
        }


        /// <summary>
        /// プレイヤーの向きからVector3の四方向を返します
        /// </summary>
        /// <param name="rot">プレイヤーの向き</param>
        /// <returns>Vector3の向き</returns>
        public static Coord GetCoordFourDirection(Vector3 rot)
        {
            eFourDirection fourDirection = GetFourDirection(rot);

            if (fourDirection == eFourDirection.left)
            {
                return Coord.left;
            }
            else if (fourDirection == eFourDirection.right)
            {
                return Coord.right;
            }
            else if (fourDirection == eFourDirection.bottom)
            {
                return Coord.back;
            }
            else
            {
                return Coord.forward;
            }
        }

        /// <summary>
        /// プレイヤーの向きからVector3の四方向を返します
        /// </summary>
        /// <param name="rot">プレイヤーの向き</param>
        /// <returns>Vector3の向き</returns>
        public static Vector3 GetVector3FourDirection(Vector3 rot)
        {
            eFourDirection fourDirection = GetFourDirection(rot);

            if (fourDirection == eFourDirection.left)
            {
                return Vector3.left;
            }
            else if (fourDirection == eFourDirection.right)
            {
                return Vector3.right;
            }
            else if (fourDirection == eFourDirection.bottom)
            {
                return Vector3.back;
            }
            else
            {
                return Vector3.forward;
            }
        }

        /// <summary>
        /// Vector3Intの全方向がランダムで入ったスタックを返します
        /// </summary>
        /// <returns>全方向がランダムで入ったスタック</returns>
        public static Stack<Coord> RandomVector3DirectionStack()
        {
            Stack<Coord> dirStack = new Stack<Coord>();
            Coord[] randDir =
                {
                    Coord.left,
                    Coord.right,
                    Coord.forward,
                    Coord.back
                };
            Algorithm.Shuffle(randDir);

            foreach(Coord dir in randDir)
            {
                dirStack.Push(dir);
            }
            return dirStack;
        }

        public static Coord GetRandomVector3FourDirection()
        {
            int rand = UnityEngine.Random.Range(0, 4);

            if (rand == 0)
                return Coord.left;
            else if (rand == 1)
                return Coord.right;
            else if (rand == 2)
                return Coord.forward;
            else
                return Coord.back;
        }


        /// <summary>
        /// 隣の向きを返します
        /// </summary>
        /// <param name="dir">調べたい向き</param>
        /// <param name="isAnti">時計回りか、反時計回りか</param>
        /// <returns>隣の向き</returns>
        public static void ClockwiseDirection(ref eFourDirection dir , bool isAnti = false)
        {
            if (isAnti == false)
            {
                switch (dir)
                {
                    case eFourDirection.top:
                        dir = eFourDirection.right;
                        return;

                    case eFourDirection.bottom:
                        dir = eFourDirection.left;
                        return;

                    case eFourDirection.left:
                        dir = eFourDirection.top;
                        return;

                    case eFourDirection.right:
                        dir = eFourDirection.bottom;
                        return;
                }
                dir = eFourDirection.No;
            }
            else
            {
                switch (dir)
                {
                    case eFourDirection.top:
                        dir = eFourDirection.left;
                        return;

                    case eFourDirection.bottom:
                        dir = eFourDirection.right;
                        return;

                    case eFourDirection.left:
                        dir = eFourDirection.bottom;
                        return;

                    case eFourDirection.right:
                        dir = eFourDirection.top;
                        return;
                }
                dir = eFourDirection.No;
            }
        }


        /// <summary>
        /// ランダムな4方向の列挙子を返します
        /// </summary>
        /// <returns>ランダムな４方向</returns>
        public static eFourDirection RandomFourDirection()
        {
            Vector3 rand = Vector3.zero;
               rand.y = UnityEngine.Random.Range(0.0f,360f);

            return GetFourDirection(rand);
        }


        /// <summary>
        /// ある地点まで Vector3 の値まで動かします
        /// </summary>
        /// <param name="trafo">動かす物のトランスフォーム</param>
        /// <param name="point">目的地</param>
        /// <param name="speed">動かすスピード</param>
        /// <returns>ポイントに到達したらtrueを返します</returns>
        public bool MoveToPoint(Transform trafo, Vector3 point, float speed = 1)// refけす
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
        /// プレイヤーの向きから四方向の向きの Quaternion を返します
        /// </summary>
        /// <param name="rot">プレイヤーの向き(eulerAngles)</param>
        /// <returns>四方向の角度を返します</returns>
        public static Quaternion GetFourDirectionEulerAngles(Vector3 rot)
        {
            float direction = (int)GetFourDirection(rot);

                return Quaternion.Euler(0, direction, 0);
        }
    }
}