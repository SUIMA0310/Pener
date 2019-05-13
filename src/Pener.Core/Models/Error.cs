namespace Pener.Models
{
    public class Error
    {
        /// <summary>
        /// エラーの発生個所を示すためのID情報
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// エラーに対するメッセージ
        /// </summary>
        public string Message { get; set; }
    }
}
