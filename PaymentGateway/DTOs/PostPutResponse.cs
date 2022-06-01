using System;
using System.Collections.Generic;

namespace DTOs
{
    public class PostPutResponse
    {
        public bool IsPaymentProcessedSuccessfully { get; set; }

        public Nullable<Guid> RecordId { get; set; }

        public List<KeyValuePair<string, string>> ErrorMessages { get; set; }

        public PostPutResponse()
        {
            ErrorMessages = new List<KeyValuePair<string, string>>();
        }
    }
}
