using Pathfinding;
using UnityEngine;

namespace Laz 
{
    public class ChomperBehaviour : EnemyBehaviour
    {
        [Header("Scriptable Object")]
        [SerializeField]
        private ChomperPropertiesScriptableObject _chomperPropertiesScriptableObject = null;

        // Dependencies
        private IChomperProperties _chomperProperties = null;
        private IAstarAI _ai = null;
        private RandomPositionGenerator _randomPositionGenerator = null;

        public void Initialize(
            IChomperProperties chomperProperties = null, 
            RandomPositionGenerator randomPositionGenerator = null)
        {            
            base.Initialize();
            _chomperProperties = chomperProperties ?? _chomperPropertiesScriptableObject;
            _randomPositionGenerator =
                randomPositionGenerator ?? new RandomPositionGenerator(transform.position, _chomperProperties.IdleRadius);
            MoveTo(transform.position);
        }

        public override void CleanUp()
        {
            base.CleanUp();
        }

        public override void Reset()
        {
            base.Reset();
        }
        
        #region Mono

        protected override void Awake()
        {
            base.Awake();
            if (!TryGetComponent(out _ai))
            {
                Debug.LogError("ChomperBehaviour is missing AI Components - AIPath");
            }
        }

        // TODO - PLACEHOLDER, SHOULD MOVE THIS INTO SEPARATE AI MOVE SCRIPT.
        // REMOVE THIS COMMENT BY END OF 2020 ^^^^^
        private void Update()
        {
            if (_ai.reachedDestination)
            {
                var destination = _randomPositionGenerator.GetRandomPosition();
                MoveTo(destination);
            }
        }

        private void MoveTo(Vector3 position)
        {
            _ai.destination = position;
        }
        
        #endregion
    }


   
}