using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    public class EnemyAI
    {
        /*�R���|�[�l���g*/
        private GridFieldAStar _aStar;          // AStar
        private GridFieldMapSettings _map;              // �}�b�v
        private TakeshiLibrary.Compass _copass; // �R���p�X
        
        /*���W*/
        private Coord _pathTargetCoord;         // ���̂�̃^�[�Q�b�g�̍��W
        private Transform _enemyTrafo;          // �G�l�~�[�̃g�����X�t�H�[��
        
        /*�p�����[�^�[*/
        private int _stayCount = 0;             // AStarLocomotion�̍ĒT���܂ł̃J�E���g

        /// <summary>
        /// ���̂�^�[�Q�b�g��Vector3���W
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
        /// ����n�_�܂� Vector3 �̒l�܂œ������܂�
        /// </summary>
        /// <param name="trafo">���������̃g�����X�t�H�[��</param>
        /// <param name="point">�ړI�n</param>
        /// <param name="speed">�������X�s�[�h</param>
        /// <returns>�|�C���g�ɓ��B������true��Ԃ��܂�</returns>
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
        /// ����n�_�܂� Vector3 �̒l�܂œ������܂�
        /// </summary>
        /// <param name="trafo">���������̃g�����X�t�H�[��</param>
        /// <param name="point">�ړI�n</param>
        /// <param name="speed">�������X�s�[�h</param>
        /// <returns>�|�C���g�ɓ��B������true��Ԃ��܂�</returns>
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
        /// �ŒZ�o�H�ŖړI�n�܂œ������܂��B��x�ړI�n�ɂ�����I�����܂��B
        /// </summary>
        /// <param name="moveSpeed">�ǂ�������X�s�[�h</param>
        /// <returns>���������� true</returns>
        public bool LocomotionToAStar( float moveSpeed = 1)
        {
            // �p�X�X�^�b�N������Ȃ�
            if (_aStar.pathStack.Count != 0)
            {
                // �p�X�^�[�Q�b�g�ɒǂ�������
                if (LocomotionToCoordPoint(_pathTargetCoord, moveSpeed)) 
                {
                    // �V�����p�X�^�[�Q�b�g���|�b�v
                    _pathTargetCoord = _aStar.pathStack.Pop();
                    Debug.DrawLine(_map.gridField[_pathTargetCoord.x, _pathTargetCoord.z], _map.gridField[_pathTargetCoord.x, _pathTargetCoord.z] + Vector3.up, Color.red, 0.1f);
                }
            }
            // �Ȃ��ꍇ
            else
            {
                // �G�l�~�[���Ō�̃p�X�^�[�Q�b�g�̈ʒu�ɗ��ĂȂ�������
                if(_enemyTrafo.position != _map.gridField[_pathTargetCoord.x,_pathTargetCoord.z])
                {
                    LocomotionToCoordPoint(_pathTargetCoord, moveSpeed);
                }
                else { return true; }
            }

            return false;
        }


        /// <summary>
        /// �ŒZ�o�H�ŖړI�n�܂œ����������܂��B
        /// </summary>
        /// <param name="goalPos">�ǂ������镨�̈ʒu</param>
        /// <param name="aStarCount">�ĒT�����s���Ԋu</param>
        /// <param name="moveSpeed">�ǂ�������X�s�[�h</param>
        /// <returns>�ǂ�������true</returns>
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
        /// AStar�N���X����p�X��ݒ肵�܂��B
        /// </summary>
        /// <param name="goalPos">�S�[����Vector3���W</param>
        /// <returns>�S�[�����T���ł������ǂ���</returns>
        public bool EnterLocomotionToAStar(Vector3 goalPos)
        {
            var enemyCoord = _map.gridField.GridCoordinate(_enemyTrafo.position) ;
            var locoGoalCoord = _map.gridField.GridCoordinate(goalPos);

            // �p�X������āA�G�l�~�[�̂���ꏊ���ŏ��̏ꏊ�ɂ���
            if (!_aStar.AStarPath(_map, enemyCoord, locoGoalCoord))
            {
                Debug.Log("�������܂����B");
                return false;
            }
            else
            {
                _pathTargetCoord = _aStar.pathStack.Pop();
                return true;
            }
        }


        /// <summary>
        /// �ړ����I�������܂�
        /// </summary>
        /// <param name="isExit">�I���������ǂ���</param>
        public void ExitLocomotion(ref bool isExit)
        {
            isExit = false;

            _stayCount = 0;
            _aStar.pathStack.Clear();
        }


        /// <summary>
        /// �w�肵���͈͂Ń����_���Ȓn�_��ڕW�Ƃ��ăG�l�~�[��p�j�����܂�
        /// </summary>
        public void Wandering(float moveSpeed, int areaX = 10, int areaZ = 10)
        {
            // �p�j�|�C���g�ɂ����烉���_���Ȉʒu��p�j�|�C���g�ɂ���
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
        /// �w�肵���}�X��̃����_���Ȓn�_��ڕW�Ƃ��ăG�l�~�[��p�j�����܂�
        /// </summary>
        public void CustomWandering(float moveSpeed,List<Coord> exceptionCoordList,int frameSize = 1, int areaX = 10, int areaZ = 10)
        {
            // �p�j�|�C���g�ɂ����烉���_���Ȉʒu��p�j�|�C���g�ɂ���
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
        /// ���C�L���X�g�ɂ��A�v���C���[��T���܂�
        /// </summary>
        /// <param name="searchLayer">�v���C���[�ƕǂ̃��C���[</param>
        /// <param name="playerTag">�v���C���[�̃^�O</param>
        /// <param name="raySize">���C�L���X�g�̑傫���i�j�Z���̉�����������l</param>
        /// <returns>�݂��������ǂ���true�F����</returns>
        public bool SearchPlayer(LayerMask searchLayer,string playerTag, float raySize = 5)
        {
            RaycastHit hit;
            // ����
            Vector3 dir = _enemyTrafo.forward;
            // �{�b�N�X���C�̑傫��
            Vector3 size = Vector3.one * raySize / 2;
            // ���C�̔��˒n�_
            Vector3 point = _enemyTrafo.position;
            
            // 1�Z�������炷
            point -= dir * _map.gridField.CellMaxLength;
            // �t�B�[���h��Widht��Depth�Œ����������C�̍ő勗���ɐݒ�
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