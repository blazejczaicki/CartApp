using System;

namespace CartApp.Models
{
    /// <summary>
    /// Default error view model class.
    /// </summary>
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
