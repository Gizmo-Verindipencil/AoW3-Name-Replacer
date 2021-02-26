namespace Aow3NameReplacer.Warnings
{
    /// <summary>
    /// 名称置換の警告を提供します。
    /// </summary>
    public class ReplacingNameWarning : Warning
    {
        /// <summary>
        /// 検証対象のプロパティを定義します。
        /// </summary>
        public enum Property
        {
            FilePath,
            OldFirstName,
            NewFirstName,
            OldSecondName,
            NewSecondName
        }

        /// <summary>
        /// 警告レベルを定義します。
        /// </summary>
        public enum WarningLevel
        {
            NotRecomended,
            NotAllowed
        }

        /// <summary>
        /// 検証プロパティを取得または設定します。
        /// </summary>
        public Property TargetProperty { get; set; }

        /// <summary>
        /// 警告レベルを取得または設定します。
        /// </summary>
        public WarningLevel Level { get; set; }
    }
}
