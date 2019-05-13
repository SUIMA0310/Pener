using System;
using System.Collections.Generic;
using System.Net;

namespace Pener.Models
{
    /// <summary>
    /// Requestの処理中にエラーが発生した際の応答
    /// </summary>
    public class ErrorResponce
    {
        /// <summary>
        /// エラーの詳細情報
        /// </summary>
        public IEnumerable<Error> Errors { get; set; }
    }
}
