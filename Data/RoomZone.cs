namespace SafeZoneMod.Data
{
    public readonly struct RoomZone
    {
        public readonly string Name;
        public readonly float MinX, MinY, MaxX, MaxY;

        public RoomZone(string name, float minX, float minY, float maxX, float maxY)
        {
            Name = name; MinX = minX; MinY = minY; MaxX = maxX; MaxY = maxY;
        }

        public bool Contains(float x, float y) =>
            x >= MinX && x <= MaxX && y >= MinY && y <= MaxY;
    }
}
