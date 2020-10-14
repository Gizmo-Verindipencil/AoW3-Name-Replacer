using Aow3NameReplacer.Models;
using Aow3NameReplacer.Views;
using Aow3NameReplacer.Warnings;
using System.Collections.Generic;
using System.Linq;

namespace Aow3NameReplacer.Controllers
{
    /// <summary>
    /// 名称置換の制御。
    /// </summary>
    public class ReplacingNameController : Controller
    {
        public ReplacingNameModel Model = new ReplacingNameModel();
        public ReplacingNameView View = new ReplacingNameView();

        /// <summary>
        /// 実行する。
        /// </summary>
        public override void Execute()
        {
            View.ShowTitle();
            SetProperty(ReplacingNameWarning.Property.FilePath);
            SetProperty(ReplacingNameWarning.Property.OldFirstName);
            SetProperty(ReplacingNameWarning.Property.NewFirstName);
            SetProperty(ReplacingNameWarning.Property.OldSecondName);
            SetProperty(ReplacingNameWarning.Property.NewSecondName);
            this.Model.Replace();
            this.View.Show("処理が完了しました。");
            this.View.Wait();
        }

        /// <summary>
        /// ユーザーに各情報の入力を促す。
        /// </summary>
        /// <param name="property">対象プロパティ。</param>
        private void SetProperty(ReplacingNameWarning.Property property)
        {
            var retry = true;
            while (retry)
            {
                switch (property)
                {
                    case ReplacingNameWarning.Property.FilePath:
                        Model.FilePath = View.Require("ファイルパス");
                        break;
                    case ReplacingNameWarning.Property.OldFirstName:
                        Model.OldFirstName = View.Require("古い1番目の名前");
                        break;
                    case ReplacingNameWarning.Property.NewFirstName:
                        Model.NewFirstName = View.Require("新しい1番目の名前");
                        break;
                    case ReplacingNameWarning.Property.OldSecondName:
                        Model.OldSecondName = View.Require("古い2番目の名前");
                        break;
                    case ReplacingNameWarning.Property.NewSecondName:
                        Model.NewSecondName = View.Require("新しい2番目の名前");
                        break;
                    default:
                        return;
                }
                var warnings = Model.GetWarnings().Where(x => x.TargetProperty == property);

                //許容できないエラー
                var notAllowed = warnings.Where(x => x.Level == ReplacingNameWarning.WarningLevel.NotAllowed);
                if (notAllowed.Count() > 0)
                {
                    View.ShowWarning(notAllowed.First().Message);
                    continue;
                }
                //警告すべきエラー
                var notRecomended = warnings.Where(x => x.Level == ReplacingNameWarning.WarningLevel.NotRecomended);
                if (notRecomended.Count() > 0)
                {
                    retry = !ConfirmAllWarnings(notRecomended);
                    continue;
                }
                //入力を確定
                retry = false;
            }
        }

        /// <summary>
        /// 警告すべきエラーを順次ユーザーに確認する。
        /// </summary>
        /// <param name="warnings">警告。</param>
        /// <returns>操作の続行。（True：する、False：しない）</returns>
        private bool ConfirmAllWarnings(IEnumerable<ReplacingNameWarning> warnings)
        {
            if (warnings.Count() == 0)
            {
                return true;
            }
            foreach (var warning in warnings)
            {
                if (!View.Confirm(warning.Message))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
