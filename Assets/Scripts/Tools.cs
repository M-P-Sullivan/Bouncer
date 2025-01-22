using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BouncerTools
{
    public enum GameMode
    {
        Spotting,
        Ready,
        Playing,
        Complete
    }

    public enum GameVersion
    {
        Clock,
        Goal
    }

    public enum SpotterItem
    {
        Target,
        BoostPad,
        Teleporter,
        TeleporterExit,
        BouncePad,
        PlayerStart
    }

    public static class Tools
    {

        public static Vector3 PolarToRect(Vector3 polarVector)
        {
            float angle = polarVector.x;
            float magnitude = polarVector.y;
            float z = polarVector.z;
            float x = magnitude * Mathf.Cos(angle * Mathf.Deg2Rad);
            float y = magnitude * Mathf.Sin(angle * Mathf.Deg2Rad);
            return new Vector3(x, y, z);
        }

        public static Vector3 RectToPolar(Vector3 rectVector)
        {
            float x = rectVector.x;
            float y = rectVector.y;
            float z = rectVector.z;
            //float offset = (x > 0) ? 0 : 180;
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;// + offset;
            float magnitude = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
            return new Vector3(angle - 45, magnitude, z);
        }

        public static float GetAngleBetweenVectors(Vector3 vector1, Vector3 vector2)
        {
            // Calculate the dot product of the vectors
            float dotProduct = vector1.x * vector2.x + vector1.y * vector2.y;

            // Calculate the magnitudes of the vectors
            float magnitude1 = (float)Math.Sqrt(vector1.x * vector1.x + vector1.y * vector1.y);
            float magnitude2 = (float)Math.Sqrt(vector2.x * vector2.x + vector2.y * vector2.y);

            // Calculate the cosine of the angle
            float cosineOfAngle = dotProduct / (magnitude1 * magnitude2);

            // Use the inverse cosine (arccos) to get the angle in radians
            float angleInRadians = (float)Math.Acos(cosineOfAngle);

            return angleInRadians;
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * (180.0f / Mathf.PI);
        }
    }

    public class RunResult
    {
        public DateTime runDateTime;
        public GameVersion mode;
        public float result;
        public bool resultIsTime;
        public int seed;

        public string DateString()
        {
            return runDateTime.ToString("yyyy-MM-dd");
        }

        public string ResultString()
        {
            if (resultIsTime)
            {
                int minutes = (int)(result / 60);
                int seconds = (int)(result % 60);
                int tenthsOfSeconds = (int)((result * 10) % 10);
                return $"{minutes}:{seconds.ToString("D2")}.{tenthsOfSeconds}";
            }
            else
            {
                return $"{result} Goals";
            }
        }

        public string SeedString()
        {
            return seed.ToString();
        }

        public GameVersion GetMode()
        {
            return mode;
        }

        public string ModeString()
        {
            if (mode == BouncerTools.GameVersion.Clock)
            {
                return "Clock";
            }
            else
            {
                return "Goals";
            }
        }
    }

}
