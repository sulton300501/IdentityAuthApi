﻿namespace IdentityAuthApi.DTOs
{
    public class ResponseDTO
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; } = false;
    }
}
