using System;

namespace Aow3NameReplacer.Views
{
    /// <summary>
    /// 名称置換の表示。
    /// </summary>
    public class ReplacingNameView : View
    {
        /// <summary>
        /// 機能名を表示する。
        /// </summary>
        public override void ShowTitle()
        {
            Console.WriteLine("# Aow3 Name Replacer");
            Console.WriteLine();
        }
    }
}
