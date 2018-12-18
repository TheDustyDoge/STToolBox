using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace STToolBox.Mathmatics
{
    public static class Vector3Extentions
    {
        // =================== \\
        // = Math Operations = \\
        // =================== \\

        public static Vector3 Multiply(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x * b.x, a.y * b.y, a.z * b.z);
        }



    }
}
