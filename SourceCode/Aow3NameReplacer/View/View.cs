using System;

namespace Aow3NameReplacer.View
{
    /// <summary>
    /// 表示。
    /// </summary>
    public abstract class View
    {
        /// <summary>
        /// 機能名を表示する。
        /// </summary>
        public virtual void ShowTitle() { }

        /// <summary>
        /// 表示する。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        public void Show(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine();
        }

        /// <summary>
        /// 表示する。
        /// </summary>
        /// <param name="name">項目。</param>
        /// <param name="value">内容。</param>
        public void Show(string name, string value)
        {
            Console.WriteLine(name + "：");
            Console.WriteLine(value);
            Console.WriteLine();
        }

        /// <summary>
        /// 警告を表示する。
        /// </summary>
        /// <param name="message"></param>
        public void ShowWarning(string message)
        {
            var lines = message.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
            {
                const string HEAD = "[#警告!] ";
                var line = (i == 0 ? HEAD : new string(' ', HEAD.Length)) + lines[i];
                Console.WriteLine(line);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// ユーザーに確認する。
        /// </summary>
        /// <param name="message">メッセージ。</param>
        /// <returns>操作の続行。（True：する、False：しない）</returns>
        public bool Confirm(string message)
        {
            var lines = message.Split(Environment.NewLine);
            for (int i = 0; i < lines.Length; i++)
            {
                const string HEAD = "[#確認!] ";
                var line = (i == 0 ? HEAD : new string(' ', HEAD.Length)) + lines[i];
                Console.WriteLine(line);
            }
            Console.WriteLine("このまま続行しますか?(Y/N)");
            var answer = Console.ReadLine().Trim().ToLower();
            Console.WriteLine();
            if (answer.Length > 0 && answer.Substring(0,1) == "y")
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ユーザーの入力まで待機する。
        /// </summary>
        public void Wait()
        {
            Console.ReadLine();
        }

        /// <summary>
        /// 入力を促す。
        /// </summary>
        /// <param name="name">項目。</param>
        /// <returns>入力内容。</returns>
        public string Require(string name)
        {
            Console.WriteLine(name + "：");
            var input = Console.ReadLine();
            Console.WriteLine();
            return input;
        }
    }
}
