namespace Aow3NameReplacer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var controller = new Controllers.ReplacingNameController();
            controller.Execute();
        }
    }
}
