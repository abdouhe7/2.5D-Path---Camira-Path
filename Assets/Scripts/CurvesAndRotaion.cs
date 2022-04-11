using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

namespace Lotfy
{
    public class CurvesAndRotaion : MonoBehaviour
    {
        public PathHandler _PathHandler;

        [HideInInspector] public float ForwaredAngelMin = 0f;
        [HideInInspector] public float ForwaredAngelMax = 180f;
        [HideInInspector] public float BackwordAngelMin = 180f;
        [HideInInspector] public float BackwordAngelMax = 360f;

        [HideInInspector] public Vector3 Line;
        //public float speedPerSec;
        [Range(-1, 1)] public int StartRotationDirction;
        [HideInInspector] public bool FakeRotationSuccess = false;


        //that the function Thats orgnize rotation TArget This time between Forward And backword
        public void RotateTheCharacterTo(Direction Dir, float T)
        {
            _PathHandler.CalculateDirFromClosestPoint(Dir, Line, out Line);
            StartCoroutine(RotateMe(Dir, T));
        }

        // The Rotation Function Thats phycaly Rotate
        IEnumerator RotateMe(Direction Dir, float inTime)
        {
            //from the current Rotation
            Quaternion fromAngle = transform.rotation;

            //Setting the target
            //force it to rotate from exacte Roud
            for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
            {
                transform.rotation = Quaternion.Lerp(fromAngle,
                    Quaternion.Euler(new Vector3(transform.rotation.eulerAngles.x,
                        transform.rotation.eulerAngles.y + 2 * StartRotationDirction,
                        transform.rotation.eulerAngles.z)),
                    t);
            }

            fromAngle = transform.rotation;
            Quaternion TOAngel = getRotationOfTargets(Dir);
            for (var t = 0f; t < 1; t += Time.deltaTime / inTime)
            {
                transform.rotation = Quaternion.Lerp(fromAngle, TOAngel, t);
                yield return null;
                FakeRotationSuccess = false;
            }

            FakeRotationSuccess = true;
        }

        //Check input Dircyion IS need to turn or not
        public bool RequiredTurn(Direction Dir)
        {
            if (Dir == DirectionRange(transform.eulerAngles.y))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        //Select the Dirction the Angel on (forwared or backword)
        public Direction DirectionRange(float Angel)
        {
            if (ForwaredAngelMin < Angel && Angel < ForwaredAngelMax)
            {
                return Direction.Forward;
            }

            if (BackwordAngelMin < Angel && Angel < BackwordAngelMax)
            {
                return Direction.Backword;
            }
            //expected error
            else
            {
                return Direction.Forward;
            }
        }

        public Quaternion getRotationOfTargets(Direction Dir)
        {
            return Quaternion.LookRotation(_PathHandler.CalculateDirFromClosestPoint(Dir, Line, out Line));
        }
    }
}