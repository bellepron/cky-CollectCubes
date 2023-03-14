using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace cky.Managers
{
    public class CubeMaterialManager : SingletonNonPersistent<CubeMaterialManager>
    {
        public Material playerStorageCubeMat;
        public Material opponentStorageCubeMat;
        public Material cubeMat;
    }
}