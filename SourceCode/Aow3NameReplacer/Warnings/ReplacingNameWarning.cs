namespace Aow3NameReplacer.Warnings
{
    /// <summary>
    /// 名称置換の警告。
    /// </summary>
    public class ReplacingNameWarning : Warning
    {
        /// <summary>
        /// 検証プロパティ。
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
        /// 警告レベル。
        /// </summary>
        public enum WarningLevel
        {
            NotRecomended,
            NotAllowed
        }

        /// <summary>
        /// 検証プロパティ。
        /// </summary>
        public Property TargetProperty { get; set; }

        /// <summary>
        /// 警告レベル。
        /// </summary>
        public WarningLevel Level { get; set; }
    }
}
