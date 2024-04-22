using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TakeshiLibrary
{
    public class Compass
    {
        private Transform _trafo;

        public Compass(Transform trafo)
        {
            _trafo = trafo;
        }

        public Quaternion GetPointAngle(Vector3 point)
        {
            float dir = Mathf.Atan2(point.z - _trafo.position.z, point.x - _trafo.position.x) * Mathf.Rad2Deg - 90;

            return Quaternion.Euler(_trafo.localRotation.eulerAngles.x, -dir, _trafo.localRotation.eulerAngles.z);
        }

        public void TurnTowardToPoint(Vector3 point)
        {
            _trafo.localRotation = GetPointAngle(point);
        }

        public void TurnAroundToPoint(Vector3 point, float aroundSpeed) 
        {
            Quaternion pointAngle = GetPointAngle(point);

            if(pointAngle != _trafo.rotation)
            {
                Quaternion.Lerp(_trafo.rotation, pointAngle, aroundSpeed);
            }
            
        }
    }
}
