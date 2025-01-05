namespace Engine.Random
{
    public static class RandomGenerator
    {
        private static readonly System.Random _seeder = new System.Random();
        private static readonly System.Random _engine = new System.Random(_seeder.Next());

        public static int Int(int min, int max)
        {
            return _engine.Next(min, max + 1); // max is exclusive in C#
        }
    }
}
