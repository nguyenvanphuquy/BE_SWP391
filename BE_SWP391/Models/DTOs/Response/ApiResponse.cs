using Microsoft.Identity.Client;

namespace BE_SWP391.Models.DTOs.Response
{
    public class ApiResponse
    { 
        public bool Success { get; set; }   
        public string Message { get; set; }
        public object Data { get; set; }

    }


}

