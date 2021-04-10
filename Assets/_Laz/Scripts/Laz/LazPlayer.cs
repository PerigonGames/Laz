namespace Laz
{
    public class LazPlayer
    {
        private LazMovement _lazMovement;
        private Lazo _lazo;

        public LazMovement Movement => _lazMovement;
        public Lazo LazoTool => _lazo;

        public void Set(ILazMovementProperty movementProperty)
        {
            _lazMovement = new LazMovement(movementProperty);
        }

        public void Set(ILazoProperties lazoProperties, IObjectOfInterest[] objectOfInterest)
        {
            _lazo = new Lazo(lazoProperties, objectOfInterest);
        }
    }
}