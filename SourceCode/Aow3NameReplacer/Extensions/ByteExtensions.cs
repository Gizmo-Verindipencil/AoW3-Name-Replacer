using System;
using System.Text;

namespace Aow3NameReplacer.Extensions
{
    /// <summary>
    /// <see cref="byte"/> の拡張メソッド。
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        /// <see cref="byte"/>配列上で、指定した文字列に相当する表現を置換します。
        /// </summary>
        /// <param name="self"><see cref="byte"/>の配列。</param>
        /// <param name="oldValue">検索対象。</param>
        /// <param name="newValue">置換対象。</param>
        /// <param name="fixedLength">置換する文字列の長さ。</param>
        /// <returns>
        /// <paramref name="oldValue"/> を <paramref name="newValue"/> に置換した後の新しい配列のインスタンスを返します。
        /// <paramref name="newValue"/> の文字長が <paramref name="fixedLength"/> よりも小さい場合、差分は半角スペースで調整されます。
        /// </returns>
        public static byte[] Replace(this byte[] self, string oldValue, string newValue, int fixedLength=0)
        {
            if (self == null || string.IsNullOrEmpty(oldValue) || string.IsNullOrEmpty(newValue))
            {
                // 処理対象がない場合は終了
                return self;
            }

            // 新しい配列のインスタンスを作成
            var array = new byte[self.Length];
            Array.Copy(self, array, self.Length);

            int sequence = 0;
            var oldPattern = Encoding.Unicode.GetBytes(oldValue);
            var newPattern = Encoding.Unicode.GetBytes(newValue);

            //置換後の文字列で発生する余白の文字数を算出
            int surplusLength = fixedLength >= newValue.Length ? fixedLength - newValue.Length : 0;

            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == oldPattern[sequence])
                {
                    //マッチする場合は、最後まで一致するか監視する
                    sequence++;
                    if (sequence == oldPattern.Length)
                    {
                        //全てマッチした場合は置換対象と判定する
                        for (int j = 0; j < newPattern.Length; j++)
                        {
                            //マッチが開始した位置から新しい表現に置き換える
                            array[i - sequence + j + 1] = newPattern[j];
                        }
                        for (int k = 1; k < surplusLength * 2; k++)
                        {
                            //余った部分を空白で埋める
                            array[i - sequence + newPattern.Length + k] = 0;
                        }
                        //空白で埋めた分だけインデックスを進める
                        i += (fixedLength * 2) - oldPattern.Length;
                        sequence = 0;
                    }
                }
                else
                {
                    //マッチしなければ、マッチの検証状態をリセット
                    sequence = 0;
                }
            }

            return array;
        }

        /// <summary>
        /// <see cref="byte"/>の配列上で、指定した文字列の登場をカウントします。
        /// </summary>
        /// <param name="self"><see cref="byte"/>の配列。</param>
        /// <param name="find">検索対象。</param>
        /// <returns>
        /// <see cref="find"/> の出現数を返します。
        /// </returns>
        public static int Count(this byte[] self, string find)
        {
            if (self == null || string.IsNullOrEmpty(find))
            {
                // 処理対象がない場合は終了
                return 0;
            }

            int count = 0;
            int sequence = 0;
            var findPattern = Encoding.Unicode.GetBytes(find);

            for (int i = 0; i < self.Length; i++)
            {
                if (self[i] == findPattern[sequence])
                {
                    //マッチする場合はシーケンスを進める
                    sequence++;
                    if (sequence == findPattern.Length)
                    {
                        //全てマッチした場合はカウントし、
                        //マッチの検証状態をリセット
                        count++;
                        sequence = 0;
                    }
                }
                else
                {
                    //マッチしなければ、マッチの検証状態をリセット
                    sequence = 0;
                }
            }

            return count;
        }
    }
}
