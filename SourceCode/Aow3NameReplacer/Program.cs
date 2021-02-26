namespace Aow3NameReplacer
{
    public class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        /// <param name="args">コマンドライン引数。</param>
        static void Main(string[] args)
        {
            var controller = new Controllers.ReplacingNameController();
            controller.Execute();
        }
    }
}
