﻿using CTS_BE.Enum;

namespace CTS_BE.Helper
{
    public class APIResponse<T>
    {
        public T? result { get; set; }
        public APIResponseStatus apiResponseStatus { get; set; }
        public string Message { get; set; }
    }

    public class JsonAPIResponse<T>
    {
        public T? Result { get; set; }
        public APIResponseStatus ApiResponseStatus { get; set; }
        public string? Message { get; set; }
    }
}
