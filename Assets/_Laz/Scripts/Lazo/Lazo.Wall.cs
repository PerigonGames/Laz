namespace Laz
{
    public partial class Lazo
    {
        private const float TimeForTailToDisappear = 0.5f;
        
        private void SpawnWall(LazoPosition lazoPosition)
        {
            var wall = LazoWallObjectPooler.Instance.PopInActivePooledObject(LazoWallObjectPooler.Key);
            wall.gameObject.SetActive(true);
            wall.Initialize(lazoPosition);
        }
        
        private void CleanUpWall()
        {
            foreach (var wall in _listOfPositions)
            {
                wall.ForceDeath();
            }
            
            _listOfPositions.Clear();
        }

        private void KillOffTailEndOfLazoFrom(int closedOffPosition, LazoPosition[] closedLoop)
        {
            for (int i = closedOffPosition; i >= 0; i--)
            {
                var positionDeathTimeToLive = ((float) i / closedOffPosition) * TimeForTailToDisappear;
                _listOfPositions[i].TimeToLive = positionDeathTimeToLive;
            }
            
            foreach (var position in closedLoop)
            {
                position.ForceDeath();
            }
        }
    }
}