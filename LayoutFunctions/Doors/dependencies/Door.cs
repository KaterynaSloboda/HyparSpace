using Elements;
using Elements.Geometry;
using Elements.Geometry.Solids;

namespace Elements
{
    public partial class Door
    {
        public const double DOOR_THICKNESS = 0.125;
        public const double DOOR_HEIGHT = 2;
        public const double DOOR_OFFSET = 2 * 0.0254; //2 inches

        public Door(Line wallLine, Vector3 position, double width) :
            this(width, DoorType.Single, material: new Material("Door material", new Color(1.0, 0, 0, 1)))
        {
            OriginalPosition = position;
            var adjustedPosition = GetClosestValidDoorPos(OriginalPosition, wallLine, width);
            Transform = new Transform(adjustedPosition, wallLine.Direction(), Vector3.ZAxis);
        }

        public Vector3 OriginalPosition
        {
            get; private set;
        }

        public static bool CanFit(Line wallLine, double width)
        {
            return wallLine.Length() - FullWidth(width) > DOOR_OFFSET * 2;
        }

        public override void UpdateRepresentations()
        {
            Vector3 left = Vector3.XAxis * FullWidth(ClearWidth) / 2;
            Vector3 right = Vector3.XAxis.Negate() * FullWidth(ClearWidth) / 2;
            var doorPolygon = new Polygon(new List<Vector3>() {
                left - Vector3.YAxis * DOOR_THICKNESS,
                left,
                right,
                right - Vector3.YAxis * DOOR_THICKNESS });

            var extrude = new Extrude(new Profile(doorPolygon), DOOR_HEIGHT, Vector3.ZAxis);
            Representation = extrude;
        }

        private Vector3 GetClosestValidDoorPos(Vector3 pos, Line wallLine, double width)
        {
            var fullWidth = Door.FullWidth(width);
            double wallWidth = wallLine.Length();
            double halfDoorToWallWidthRatio = 0.5 * fullWidth / wallWidth;
            Vector3 p1 = wallLine.PointAt(halfDoorToWallWidthRatio);
            Vector3 p2 = wallLine.PointAt(1.0 - halfDoorToWallWidthRatio);
            var reducedWallLine = new Line(p1, p2);
            return pos.ClosestPointOn(reducedWallLine);
        }

        private static double FullWidth(double internalWidth)
        {
            return internalWidth + DOOR_OFFSET * 2;
        }

    }
}