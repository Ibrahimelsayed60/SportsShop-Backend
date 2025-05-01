using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsShop.Core.Dtos
{
    public class ResultDto
    {

        public bool IsSuccess { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }

        public static ResultDto Sucess(dynamic data, string message = "Success Operation")
        {
            return new ResultDto
            {
                IsSuccess = true,
                Data = data,
                Message = message
            };
        }

        public static ResultDto Faliure(string message = "Invalid Operation", dynamic? data=null)
        {
            return new ResultDto
            {
                IsSuccess = false,
                Data = data is not null? data: default,
                Message = message,
            };
        }

    }
}
