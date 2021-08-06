using UnityEngine;

namespace Laz
{
    public partial class Lazo
    {
        private FakeLazo _fakeLazo = null;

        public FakeLazo FakeLazo => _fakeLazo;
        
        private void CreateFakeLazoLineIfNeeded()
        {
            if (_isTimeToLiveFrozen)
            {
                CreateFakeLazo();
            }
        }
        
        private void CreateFakeLazo()
        {
            var fakeLazoBehaviour = FakeLazoObjectPooler.Instance.PopInActivePooledObject(FakeLazoObjectPooler.Key);
            _fakeLazo = new FakeLazo(_listOfPositions, true);
            fakeLazoBehaviour.SetLine(_fakeLazo);
            fakeLazoBehaviour.gameObject.SetActive(true);
        }
    }
}

