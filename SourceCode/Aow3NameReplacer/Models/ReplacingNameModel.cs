using Aow3NameReplacer.Extensions;
using Aow3NameReplacer.Warnings;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Aow3NameReplacer.Models
{
    /// <summary>
    /// 名称置換のロジック。
    /// </summary>
    public class ReplacingNameModel : Model
    {
        /// <summary>
        /// 置換ファイルのパス。
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 置換ファイルの内容。
        /// </summary>
        public byte[] FileContent {
            get
            {
                var warnings = GetFilePathWarnings();
                if (warnings.Count() > 0)
                {
                    return null;
                }
                if (this._fileContent == null)
                {
                    this._fileContent = File.ReadAllBytes(this.FilePath);
                }
                return this._fileContent;
            }
        }
        private byte[] _fileContent;
        /// <summary>
        /// 古い1番目の名前。（＝置換される今の名前）
        /// </summary>
        public string OldFirstName { get; set; }
        /// <summary>
        /// 新しい1番目の名前。（＝置換された後の名前）
        /// </summary>
        public string NewFirstName { get; set; }
        /// <summary>
        /// 古い2番目の名前。（＝置換される今の名前）
        /// </summary>
        public string OldSecondName { get; set; }
        /// <summary>
        /// 新しい2番目の名前。（＝置換された後の名前）
        /// </summary>
        public string NewSecondName { get; set; }

        public List<ReplacingNameWarning> GetWarnings()
        {
            var warnings = new List<ReplacingNameWarning>();
            // ファイルパスの警告
            warnings.AddRange(GetFilePathWarnings());

            // 項目長の警告
            warnings.AddRange(GetLengthWarnings(ReplacingNameWarning.Property.OldFirstName));
            warnings.AddRange(GetLengthWarnings(ReplacingNameWarning.Property.NewFirstName));
            warnings.AddRange(GetLengthWarnings(ReplacingNameWarning.Property.OldSecondName));
            warnings.AddRange(GetLengthWarnings(ReplacingNameWarning.Property.NewSecondName));

            // 置換対象の警告
            warnings.AddRange(GetUnintentionalReplacingWarnings(ReplacingNameWarning.Property.OldFirstName));
            warnings.AddRange(GetUnintentionalReplacingWarnings(ReplacingNameWarning.Property.OldSecondName));
            warnings.AddRange(GetUnintentionalReplaceingWarningsBetweenOlds(ReplacingNameWarning.Property.OldFirstName));
            warnings.AddRange(GetUnintentionalReplaceingWarningsBetweenOlds(ReplacingNameWarning.Property.OldSecondName));

            // 文字化けの警告
            warnings.AddRange(GetTextCorruptionWarnings(ReplacingNameWarning.Property.OldFirstName));
            warnings.AddRange(GetTextCorruptionWarnings(ReplacingNameWarning.Property.NewFirstName));
            warnings.AddRange(GetTextCorruptionWarnings(ReplacingNameWarning.Property.OldSecondName));
            warnings.AddRange(GetTextCorruptionWarnings(ReplacingNameWarning.Property.OldSecondName));
            return warnings;
        }

        /// <summary>
        /// Profileファイルのパスに関する警告を取得する。
        /// </summary>
        /// <returns>警告。</returns>
        private List<ReplacingNameWarning> GetFilePathWarnings()
        {
            var warnings = new List<ReplacingNameWarning>();
            if (!File.Exists(this.FilePath))
            {
                warnings.Add(new ReplacingNameWarning()
                {
                    TargetProperty = ReplacingNameWarning.Property.FilePath,
                    Level = ReplacingNameWarning.WarningLevel.NotAllowed,
                    Message = "指定されたパスにファイルが存在しません。"
                });
            }
            else if(Path.GetExtension(this.FilePath).ToLower() != ".apd")
            {
                warnings.Add(new ReplacingNameWarning()
                {
                    TargetProperty = ReplacingNameWarning.Property.FilePath,
                    Level = ReplacingNameWarning.WarningLevel.NotAllowed,
                    Message = "ファイルの拡張子がAPDではありません。"
                });
            }
            return warnings;
        }

        /// <summary>
        /// 項目の文字長に関する警告を取得する。
        /// </summary>
        /// <param name="property">対象プロパティ。</param>
        /// <returns>警告。</returns>
        private List<ReplacingNameWarning> GetLengthWarnings(ReplacingNameWarning.Property property)
        {
            var warnings = new List<ReplacingNameWarning>();
            warnings.AddRange(GetOverlengthWarnings(property));
            warnings.AddRange(GetUnderlengthWarnings(property));
            return warnings;
        }

        /// <summary>
        /// 項目が最大文字長を超えている場合の警告を取得する。
        /// </summary>
        /// <param name="property">対象プロパティ。</param>
        /// <returns>警告。</returns>
        private List<ReplacingNameWarning> GetOverlengthWarnings(ReplacingNameWarning.Property property)
        {
            var warnings = new List<ReplacingNameWarning>();
            var value = GetValue(property);
            var maxLength = GetMaxLength(property);
            if (value.Length > maxLength)
            {
                warnings.Add(new ReplacingNameWarning()
                {
                    TargetProperty = property,
                    Level = ReplacingNameWarning.WarningLevel.NotAllowed,
                    Message = string.Format("{0}文字以内で入力してください。", maxLength)
                });
            }
            return warnings;
        }

        /// <summary>
        /// 項目が最小文字長より少ない場合の警告を取得する。
        /// </summary>
        /// <param name="property">対象プロパティ。</param>
        /// <returns>警告。</returns>
        private List<ReplacingNameWarning> GetUnderlengthWarnings(ReplacingNameWarning.Property property)
        {
            var warnings = new List<ReplacingNameWarning>();
            var value = GetValue(property);
            if (value.Length < 1)
            {
                warnings.Add(new ReplacingNameWarning()
                {
                    TargetProperty = property,
                    Level = ReplacingNameWarning.WarningLevel.NotAllowed,
                    Message = "1文字以上で入力してください。"
                });
            }
            return warnings;
        }

        /// <summary>
        /// 警告プロパティに対応したプロパティの内容を取得する。
        /// </summary>
        /// <param name="property">対象プロパティ。</param>
        /// <returns>プロパティの内容。</returns>
        private string GetValue(ReplacingNameWarning.Property property)
        {
            var value = string.Empty;
            switch (property)
            {
                case ReplacingNameWarning.Property.OldFirstName:
                    value = this.OldFirstName;
                    break;
                case ReplacingNameWarning.Property.NewFirstName:
                    value = this.NewFirstName;
                    break;
                case ReplacingNameWarning.Property.OldSecondName:
                    value = this.OldSecondName;
                    break;
                case ReplacingNameWarning.Property.NewSecondName:
                    value = this.NewSecondName;
                    break;
            }
            return value ?? string.Empty;
        }

        /// <summary>
        /// 警告プロパティに対応した最大文字長を取得する。
        /// </summary>
        /// <param name="property">対象プロパティ。</param>
        /// <returns>プロパティの最大文字長。</returns>
        private int GetMaxLength(ReplacingNameWarning.Property property)
        {
            var maxLength = 0;
            switch (property)
            {
                case ReplacingNameWarning.Property.OldFirstName:
                case ReplacingNameWarning.Property.NewFirstName:
                    maxLength = 10;
                    break;
                case ReplacingNameWarning.Property.OldSecondName:
                case ReplacingNameWarning.Property.NewSecondName:
                    maxLength = 19;
                    break;
            }
            return maxLength;
        }

        /// <summary>
        /// ファイルの置換対象が存在しない・複数存在する場合の警告を取得する。
        /// </summary>
        /// <param name="property">対象プロパティ。</param>
        /// <returns>警告。</returns>
        private List<ReplacingNameWarning> GetUnintentionalReplacingWarnings(ReplacingNameWarning.Property property)
        {
            var warnings = new List<ReplacingNameWarning>();
            var value = GetValue(property);
            if (this.FileContent == null)
            {
                // ファイルパスの指定前に反映しない
                return warnings;
            }
            var count = this.FileContent.Count(value);
            if (count == 0)
            {
                var message = string.Format("[{0}]は対象のファイル内に存在しません。", this.OldFirstName);
                warnings.Add(new ReplacingNameWarning()
                {
                    TargetProperty = property,
                    Level = ReplacingNameWarning.WarningLevel.NotRecomended,
                    Message = message
                });
            }
            else if (count > 1)
            {
                var message = string.Format(
                    "[{0}]は{1}箇所で発見される表現です。" +
                    "このまま処理するとファイルが破損する恐れがあります。", this.OldFirstName, count);
                warnings.Add(new ReplacingNameWarning()
                {
                    TargetProperty = property,
                    Level = ReplacingNameWarning.WarningLevel.NotRecomended,
                    Message = message
                });
            }
            return warnings;
        }

        /// <summary>
        /// 古い名前の組合せで、意図しない置換が行われる場合の警告を取得する。
        /// </summary>
        /// <remarks>
        /// 置換処理は2番目の名前から行われる関係上、2番目の置換対象に1番目の内容を含んでいると
        /// 本来1番目として置換されるべき表現が2番目の置換時に処理され、1番目の置換が行われない。
        /// </remarks>
        /// <param name="property">対象プロパティ。</param>
        /// <returns>警告。</returns>
        private List<ReplacingNameWarning> GetUnintentionalReplaceingWarningsBetweenOlds(ReplacingNameWarning.Property property)
        {
            var warnings = new List<ReplacingNameWarning>();
            if (string.IsNullOrEmpty(this.OldFirstName) || string.IsNullOrEmpty(this.OldSecondName) || this.FileContent == null)
            {
                // 判定に必要な要素が指定される前に判定しない
                return warnings;
            }
            var firstBytes = Encoding.Unicode.GetBytes(this.OldFirstName);
            var countFirstIncludingSecond = firstBytes.Count(this.OldSecondName);
            var countFileIncludingFirst = this.FileContent.Count(this.OldFirstName);
            if (countFirstIncludingSecond > 1 && countFileIncludingFirst > 0)
            {
                var message = 
                    "古い1番目の名前は2番目の名前を含んでいます。" +
                    "意図しない名称の置換が行われる可能性があります。";
                warnings.Add(new ReplacingNameWarning()
                {
                    TargetProperty = property,
                    Level = ReplacingNameWarning.WarningLevel.NotRecomended,
                    Message = message
                });
            }
            return warnings;
        }

        /// <summary>
        /// 文字化けに関する警告を取得する。
        /// </summary>
        /// <param name="property">対象プロパティ。</param>
        /// <returns>警告。</returns>
        private List<ReplacingNameWarning> GetTextCorruptionWarnings(ReplacingNameWarning.Property property)
        {
            var warnings = new List<ReplacingNameWarning>();
            switch (property)
            {
                case ReplacingNameWarning.Property.OldFirstName:
                case ReplacingNameWarning.Property.NewFirstName:
                    addWarningIfLengthsAreNotSame(this.OldFirstName, this.NewFirstName);
                    break;
                case ReplacingNameWarning.Property.OldSecondName:
                case ReplacingNameWarning.Property.NewSecondName:
                    addWarningIfLengthsAreNotSame(this.OldSecondName, this.NewSecondName);
                    break;
            }
            return warnings;

            void addWarningIfLengthsAreNotSame(string value1, string value2)
            {
                if (string.IsNullOrEmpty(value1) || string.IsNullOrEmpty(value2))
                {
                    return;
                }
                if (value1.Length != value2.Length)
                {
                    var message =
                            "置換前後の名前の文字数が異なります。" +
                            "名前が正しく表示されないことがあります。";
                    warnings.Add(new ReplacingNameWarning()
                    {
                        TargetProperty = property,
                        Level = ReplacingNameWarning.WarningLevel.NotRecomended,
                        Message = message
                    });
                }
            }
        }

        /// <summary>
        /// ファイル内の名称を置換する。
        /// </summary>
        public void Replace()
        {
            var backup = this.FilePath + ".backup";
            var replaced = this.FilePath + ".replaced";
            
            File.WriteAllBytes(
                replaced,
                this._fileContent
                    .Replace(this.OldSecondName, this.NewSecondName, GetMaxLength(ReplacingNameWarning.Property.NewSecondName))
                    .Replace(this.OldFirstName, this.NewFirstName, GetMaxLength(ReplacingNameWarning.Property.NewFirstName)));
            if (File.Exists(backup)) File.Delete(backup);
            File.Move(this.FilePath, backup);
            File.Move(replaced, this.FilePath);
        }
    }
}
