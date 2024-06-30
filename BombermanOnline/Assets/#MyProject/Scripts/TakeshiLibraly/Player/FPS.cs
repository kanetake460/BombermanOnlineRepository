using System;
using System.Collections.Generic;
using UnityEngine;


namespace TakeshiLibrary
{
    /*=====FPS�̈ړ��֘A�̃X�N���v�g�ł�=====*/
    // �Q�l�T�C�g
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
        /// �J�����̊p�x���������܂�(�㉺)
        /// </summary>
        /// <param name="q">����������quoternion</param>
        /// <param name="minX">���̊p�x����</param>
        /// <param name="maxX">��̊p�x����</param>
        /// <returns>�p�x</returns>
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


        ///<summary>�J�����̎��_�ړ��֐�(�㉺�̎��_�ړ�)</summary>>
        ///<param name="Xsensityvity"<pragma>���_�ړ��X�s�[�h</pragma>
        ///<param name="minX"<pragma>���̊p�x����</pragma>
        ///<param name="maxX"<pragma>��̊p�x����</pragma>
        public void CameraViewport(float Xsensityvity = 3f, float minX = -90f, float maxX = 90f)
        {
            float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;       // �}�E�X�̍��W���
            _camera.transform.localRotation *= Quaternion.Euler(-yRot, 0, 0);     // �p�x���

            _camera.transform.localRotation = ClampRotation(_camera.transform.localRotation, minX, maxX);           // �p�x����
        }


        /// <summary>
        /// �v���C���[�̎��_�ړ��֐�(���E���_�ړ�)
        /// </summary>
        public void PlayerViewport(float Ysensityvity = 3f)
        {
            float xRot = Input.GetAxis("Mouse X") * Ysensityvity;               // �}�E�X�̍��W���
            _player.transform.localRotation *= Quaternion.Euler(0, xRot, 0);     // �p�x���
        }


        /// <summary>
        /// �v���C���[���L�[���͂ɂ���Ĉړ������܂�
        /// </summary>
        /// <param name="speed">�ړ��X�s�[�h</param>
        public void Locomotion(float speed = 10f,float dashSpeed = 15,KeyCode dashuKey = KeyCode.LeftShift)
        {
            if (Input.GetKey(dashuKey)) speed = dashSpeed;

            float x = Input.GetAxisRaw("Horizontal") * speed;     // �ړ�����
            float z = Input.GetAxisRaw("Vertical") * speed;       // �ړ�����

            _player.transform.position += _player.transform.forward * z * Time.deltaTime + _player.transform.right * x * Time.deltaTime;  // �ړ�
            _camera.transform.position = _player.transform.position;

        }

        /// <summary>
        /// �v���C���[���L�[���͂ɂ���Ĉړ������܂�
        /// </summary>
        /// <param name="player">�������v���C���[</param>
        /// <param name="speed">�ړ��X�s�[�h</param>
        public float VelocityForceLocomotion(float speed = 10f, float dashSpeed = 15, float backSpeed = 5, KeyCode dashKey = KeyCode.LeftShift)
        {
            if (Input.GetKey(dashKey)) speed = dashSpeed;

            float x = Input.GetAxisRaw("Horizontal");     // �ړ�����
            float z = Input.GetAxisRaw("Vertical");       // �ړ�����

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
                    movement *= 0.707f; // �΂߈ړ��̑��x�␳
                }

                _rb.AddForce(movement, ForceMode.VelocityChange);
            }
        }



        /// <summary>
        /// �J�[�\�������b�N���܂�
        /// </summary>
        /// <param name="cursorLock">�J�[�\�����b�N�t���O</param>
        public void CursorLock()
        {
            if (Input.GetKeyDown(KeyCode.Escape))   // �G�X�P�[�v�L�[����������
            {
                _cursorLock = false;
            }
            else if (Input.GetMouseButton(0))       // ���N���b�N
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
        /// �^�����g�����X�t�H�[�����ǃu���b�N�ɓ���Ȃ��悤�ɂ��܂�
        /// </summary>
        /// <param name="trafo">�v���C���[�g�����X�t�H�[��</param>
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
        /// �v���C���[�̌�������l�����̗񋓎q��Ԃ��܂�
        /// </summary>
        /// <param name="rot">�v���C���[�̌���</param>
        /// <returns>�����̗񋓎q</returns>
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
        /// �v���C���[�̌�������Vector3�̎l������Ԃ��܂�
        /// </summary>
        /// <param name="rot">�v���C���[�̌���</param>
        /// <returns>Vector3�̌���</returns>
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
        /// �v���C���[�̌�������Vector3�̎l������Ԃ��܂�
        /// </summary>
        /// <param name="rot">�v���C���[�̌���</param>
        /// <returns>Vector3�̌���</returns>
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
        /// Vector3Int�̑S�����������_���œ������X�^�b�N��Ԃ��܂�
        /// </summary>
        /// <returns>�S�����������_���œ������X�^�b�N</returns>
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
        /// �ׂ̌�����Ԃ��܂�
        /// </summary>
        /// <param name="dir">���ׂ�������</param>
        /// <param name="isAnti">���v��肩�A�����v��肩</param>
        /// <returns>�ׂ̌���</returns>
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
        /// �����_����4�����̗񋓎q��Ԃ��܂�
        /// </summary>
        /// <returns>�����_���ȂS����</returns>
        public static eFourDirection RandomFourDirection()
        {
            Vector3 rand = Vector3.zero;
               rand.y = UnityEngine.Random.Range(0.0f,360f);

            return GetFourDirection(rand);
        }


        /// <summary>
        /// ����n�_�܂� Vector3 �̒l�܂œ������܂�
        /// </summary>
        /// <param name="trafo">���������̃g�����X�t�H�[��</param>
        /// <param name="point">�ړI�n</param>
        /// <param name="speed">�������X�s�[�h</param>
        /// <returns>�|�C���g�ɓ��B������true��Ԃ��܂�</returns>
        public bool MoveToPoint(Transform trafo, Vector3 point, float speed = 1)// ref����
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
        /// �v���C���[�̌�������l�����̌����� Quaternion ��Ԃ��܂�
        /// </summary>
        /// <param name="rot">�v���C���[�̌���(eulerAngles)</param>
        /// <returns>�l�����̊p�x��Ԃ��܂�</returns>
        public static Quaternion GetFourDirectionEulerAngles(Vector3 rot)
        {
            float direction = (int)GetFourDirection(rot);

                return Quaternion.Euler(0, direction, 0);
        }
    }
}