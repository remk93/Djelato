﻿namespace Djelato.Web.ViewModel
{
    public struct ResponseContent
    {
        public string ErrorMessage { get; set; }
        public string Message { get; set; }
        public object Result { get; set; }
        public bool IsSucceeded { get; set; }        
    }
}
