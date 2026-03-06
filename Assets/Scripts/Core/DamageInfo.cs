using UnityEngine;

namespace FPSPrototype.Core
{
    public struct DamageInfo
    {
        public float Amount;
        public Vector3 HitPoint;
        public Vector3 HitNormal;
        public GameObject Instigator;

        public DamageInfo(float amount, Vector3 hitPoint, Vector3 hitNormal, GameObject instigator)
        {
            Amount = amount;
            HitPoint = hitPoint;
            HitNormal = hitNormal;
            Instigator = instigator;
        }
    }
}
