namespace Laz
{
    public partial class Lazo
    {
        private void SpawnWall(LazoPosition lazoPosition)
        {
            var wall = LazoWallObjectPooler.Instance.SpawnAt(LazoWallObjectPooler.Key, lazoPosition.Position);
            wall.Initialize(lazoPosition);
        }
        
        private void ClearWall()
        {
            foreach (var wall in _listOfPositions)
            {
                wall.ForceDeath();
            }
            
            _listOfPositions.Clear();
        }
    }
}