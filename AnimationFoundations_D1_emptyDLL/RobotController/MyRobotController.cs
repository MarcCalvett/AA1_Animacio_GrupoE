using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RobotController
{

    public struct MyQuat
    {

        public float w;
        public float x;
        public float y;
        public float z;
    }

    public struct MyVec
    {

        public float x;
        public float y;
        public float z;
    }






    public class MyRobotController
    {

        #region public methods
        public float dt = 0.005f;
        public static float percentageMovementCompletedEx2 = 0f;
        public static float percentageMovementCompletedEx3 = 0f;
        static MyQuat _rot2;
        public string Hi()
        {

            string s = "Hello world from my Robot Controller, by Lluc Ferrando and Marc Calvet";
            return s;

        }


        //EX1: this function will place the robot in the initial position

        public void PutRobotStraight(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            percentageMovementCompletedEx2 = 0;
            percentageMovementCompletedEx3 = 0;

            //todo: change this, use the function Rotate declared below
            rot0 = NullQ;
            rot0 = Rotate(rot0, GetAxis(0, 1, 0), 72);//72
            rot1 = Rotate(rot0, GetAxis(1, 0, 0), -12.21f);
            rot2 = Rotate(rot1, GetAxis(1, 0, 0), 81.2f);
            rot3 = Rotate(rot2, GetAxis(1, 0, 0), 40f);//35.7f
        }



        //EX2: this function will interpolate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.


        public bool PickStudAnim(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            percentageMovementCompletedEx3 = 0;
            bool inProcess = percentageMovementCompletedEx2 < 1f;
            //todo: add a check for your condition

            MyQuat initRot0;
            MyQuat initRot1;
            MyQuat initRot2;
            MyQuat initRot3;

            initRot0 = NullQ;
            initRot0 = Rotate(initRot0, GetAxis(0, 1, 0), 72);
            initRot1 = Rotate(initRot0, GetAxis(1, 0, 0), -12.21f);
            initRot2 = Rotate(initRot1, GetAxis(1, 0, 0), 81.2f);
            initRot3 = Rotate(initRot2, GetAxis(1, 0, 0), 40f);

            MyQuat finalRot0;
            MyQuat finalRot1;
            MyQuat finalRot2;
            MyQuat finalRot3;

            finalRot0 = NullQ;
            finalRot0 = Rotate(finalRot0, GetAxis(0, 1, 0), 37.73f);
            finalRot1 = Rotate(finalRot0, GetAxis(1, 0, 0), 10.8f);
            finalRot2 = Rotate(finalRot1, GetAxis(1, 0, 0), 55);
            finalRot3 = Rotate(finalRot2, GetAxis(1, 0, 0), 28.75f);


            if (inProcess)
            {
                //todo: add your code here
                // Aplicamos la rotación interpolada
                rot0 = Slerp(initRot0, finalRot0, percentageMovementCompletedEx2);
                rot1 = Slerp(initRot1, finalRot1, percentageMovementCompletedEx2);
                rot2 = Slerp(initRot2, finalRot2, percentageMovementCompletedEx2);
                rot3 = Slerp(initRot3, finalRot3, percentageMovementCompletedEx2);

                if (percentageMovementCompletedEx2 + dt >= 1)
                    percentageMovementCompletedEx2 = 1;
                else
                    percentageMovementCompletedEx2 += dt;
                return true;
            }

            rot0 = NullQ;
            rot1 = NullQ;
            rot2 = NullQ;
            rot3 = NullQ;

            return false;
        }


        //EX3: this function will calculate the rotations necessary to move the arm of the robot until its end effector collides with the target (called Stud_target)
        //it will return true until it has reached its destination. The main project is set up in such a way that when the function returns false, the object will be droped and fall following gravity.
        //the only difference wtih exercise 2 is that rot3 has a swing and a twist, where the swing will apply to joint3 and the twist to joint4

        public bool PickStudAnimVertical(out MyQuat rot0, out MyQuat rot1, out MyQuat rot2, out MyQuat rot3)
        {
            percentageMovementCompletedEx2 = 0;
            bool inProcess = percentageMovementCompletedEx3 < 1f;
            //todo: add a check for your condition

            MyQuat initRot0;
            MyQuat initRot1;
            MyQuat initRot2;

            initRot0 = NullQ;
            initRot0 = Rotate(initRot0, GetAxis(0, 1, 0), 72);
            initRot1 = Rotate(initRot0, GetAxis(1, 0, 0), -12.21f);
            initRot2 = Rotate(initRot1, GetAxis(1, 0, 0), 81.2f);

            MyQuat finalRot0;
            MyQuat finalRot1;
            MyQuat finalRot2;


            finalRot0 = NullQ;
            finalRot0 = Rotate(finalRot0, GetAxis(0, 1, 0), 37.73f);
            finalRot1 = Rotate(finalRot0, GetAxis(1, 0, 0), 10.8f);
            finalRot2 = Rotate(finalRot1, GetAxis(1, 0, 0), 55);

            MyQuat initSwing = Rotate(NullQ, GetAxis(1, 0, 0), 40f);
            MyQuat initTwist = Rotate(NullQ, GetAxis(0, 1, 0), 0f); //NullQ;
            MyQuat finalSwing = Rotate(NullQ, GetAxis(1, 0, 0), 28.75f);
            MyQuat finalTwist = Rotate(NullQ, GetAxis(0, 1, 0), 90f);


            if (inProcess)
            {
                MyQuat swing = Slerp(initSwing, finalSwing, Clamp(percentageMovementCompletedEx3, 0, 1));
                MyQuat twist = Slerp(initTwist, finalTwist, Clamp(percentageMovementCompletedEx3, 0, 1));

                rot0 = Slerp(initRot0, finalRot0, Clamp(percentageMovementCompletedEx3, 0, 1));
                rot1 = Slerp(initRot1, finalRot1, Clamp(percentageMovementCompletedEx3, 0, 1));
                rot2 = Slerp(initRot2, finalRot2, Clamp(percentageMovementCompletedEx3, 0, 1));
                _rot2 = rot2;
                rot3 = Multiply(twist, swing);
                percentageMovementCompletedEx3 += dt;

                return true;
            }

            rot0 = NullQ;
            rot1 = NullQ;
            rot2 = NullQ;
            rot3 = NullQ;

            return false;
        }


        public static MyQuat GetSwing(MyQuat rot3)
        {
            MyQuat locTwist;
            locTwist.w = rot3.w;
            locTwist.x = 0;
            locTwist.y = rot3.y;
            locTwist.z = 0;
            locTwist = Normalize(locTwist);

            MyQuat swing = Multiply(Inverse(locTwist), rot3);
            return Multiply(_rot2, swing);
        }


        public static MyQuat GetTwist(MyQuat rot3)
        {
            MyQuat initRot = Rotate(GetSwing(rot3), GetAxis(0, 1, 0), 0);
            MyQuat finalRot = Rotate(GetSwing(rot3), GetAxis(0, 1, 0), 90);
            return Slerp(initRot, finalRot, Clamp(percentageMovementCompletedEx3, 0, 1));
        }




        #endregion


        #region private and internal methods

        internal int TimeSinceMidnight { get { return (DateTime.Now.Hour * 3600000) + (DateTime.Now.Minute * 60000) + (DateTime.Now.Second * 1000) + DateTime.Now.Millisecond; } }


        private static MyQuat NullQ
        {
            get
            {
                MyQuat a;
                a.w = 1;
                a.x = 0;
                a.y = 0;
                a.z = 0;
                return a;

            }
        }
        //(q1 * sin(t * (theta - theta_0)) + q2 * sin(t * theta_0)) / sin(theta)

        public static MyQuat Slerp(MyQuat q1, MyQuat q2, float t)
        {
            // Quaternion a retornar
            MyQuat qm;
            // Calcular angulo entre ellos
            double cosHalfTheta = q1.w * q2.w + q1.x * q2.x + q1.y * q2.y + q1.z * q2.z;

            if (Math.Abs(cosHalfTheta) >= 1.0)
            {
                qm.w = q1.w; qm.x = q1.x; qm.y = q1.y; qm.z = q1.z;
                return qm;
            }
            // Calcular valores temporales
            double halfTheta = Math.Acos(cosHalfTheta);
            double sinHalfTheta = Math.Sqrt(1.0 - cosHalfTheta * cosHalfTheta);
            if (Math.Abs(sinHalfTheta) < 0.001)
            {
                qm.w = (float)(q1.w * 0.5 + q2.w * 0.5);
                qm.x = (float)(q1.x * 0.5 + q2.x * 0.5);
                qm.y = (float)(q1.y * 0.5 + q2.y * 0.5);
                qm.z = (float)(q1.z * 0.5 + q2.z * 0.5);
                return qm;
            }
            double ratioA = Math.Sin((1 - t) * halfTheta) / sinHalfTheta;
            double ratioB = Math.Sin(t * halfTheta) / sinHalfTheta;
            qm.w = (float)(q1.w * ratioA + q2.w * ratioB);
            qm.x = (float)(q1.x * ratioA + q2.x * ratioB);
            qm.y = (float)(q1.y * ratioA + q2.y * ratioB);
            qm.z = (float)(q1.z * ratioA + q2.z * ratioB);
            return qm;
        }

        public static float Clamp(float value, float min, float max)
        {
            if (value < min)
            {
                value = min;
            }
            else if (value > max)
            {
                value = max;
            }

            return value;
        }

        internal static MyQuat Multiply(MyQuat q1, MyQuat q2)
        {

            //todo: change this so it returns a multiplication:
            MyQuat result;
            result.w = q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z;
            result.x = q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y;
            result.y = q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x;
            result.z = q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w;
            return result;
        }

        internal static MyQuat Rotate(MyQuat currentRotation, MyVec axis, float angle)
        {

            //todo: change this so it takes currentRotation, and calculate a new quaternion rotated by an angle "angle" radians along the normalized axis "axis"
            MyQuat rot;
            rot = VecToQuat(axis);
            rot.w = (float)Math.Cos(AngleToRadian(angle / 2f));
            rot.x *= (float)Math.Sin(AngleToRadian(angle / 2f));
            rot.y *= (float)Math.Sin(AngleToRadian(angle / 2f));
            rot.z *= (float)Math.Sin(AngleToRadian(angle / 2f));
            return Normalize(Multiply(currentRotation, rot));
        }

        static MyQuat Conjugate(MyQuat q1)
        {
            MyQuat result;
            result.w = q1.w;
            result.x = -q1.x;
            result.y = -q1.y;
            result.z = -q1.z;

            return result;
        }

        static MyQuat Normalize(MyQuat q)
        {
            float result;
            result = (float)Math.Sqrt(Math.Pow(q.w, 2) + Math.Pow(q.x, 2) + Math.Pow(q.y, 2) + Math.Pow(q.z, 2));
            MyQuat result2;
            result2.w = q.w;
            result2.x = q.x / result;
            result2.y = q.y / result;
            result2.z = q.z / result;

            return result2;
        }

        static MyQuat VecToQuat(MyVec myVec)
        {
            MyQuat result;
            result.w = 0;
            result.x = myVec.x;
            result.y = myVec.y;
            result.z = myVec.z;
            return result;
        }

        static float AngleToRadian(float angle)
        {
            return (float)((float)angle * Math.PI / 180);
        }

        static MyVec GetAxis(float x, float y, float z)
        {
            MyVec result;
            result.x = x;
            result.y = y;
            result.z = z;
            return result;
        }

        static MyQuat Inverse(MyQuat q)
        {
            MyQuat result;
            result = Conjugate(q);
            result = Normalize(result);
            return result;
        }

        //todo: add here all the functions needed

        #endregion






    }
}
