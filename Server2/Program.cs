namespace Server2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
            NetworkManager main = new NetworkManager();
            main.Start();
        }
    }
}