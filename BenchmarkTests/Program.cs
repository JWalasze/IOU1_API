namespace BenchmarkTests
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var er = 1;
            var ee = Demo.s1;
            var x = new Demo();
        }

        public class Demo
        {
            public int YY { get; set; } = Init("prop YY", 2);

            private int x = Init("field x", 1);
            public int Y { get; set; } = Init("prop Y", 2);

            public static int s1 = Init("static s1", 100);
            static Demo() { Console.WriteLine("static ctor"); }

            public Demo()
            {
                Console.WriteLine("instance ctor");
            }

            private static int Init(string name, int val)
            {
                Console.WriteLine($"init {name} = {val}");
                return val;
            }
        }
    }
}
