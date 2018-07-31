using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace dotnet2_1WebAPI
{
    [Route("api/[controller]")]
     public class TestController : BaseController
    {
        private AppDb _objdb;
        public TestController(AppDb DB)
        {
            _objdb = DB;
        }
        
        [HttpGet("check",Name = "check")]
        public dynamic work()
        {
            var appsettingbuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var Configuration = appsettingbuilder.Build();
            var version = Configuration.GetSection("appSettings:API_Version").Value;

            return "Success API Version: " + version;
        }

        [HttpGet("checktoken/{token}",Name = "checktoken")]
        public dynamic checktoken(string token)
        {
            string decodedString = "";
            try
            {           
                byte[] data = Convert.FromBase64String(token);
                decodedString = Encoding.UTF8.GetString(data);
            }catch 
            {
               return "Invalid Token";
            }

            if(decodedString == "dotnetAPI")
            {
                return "Correct Token";
            }
            else
            {
                return "Invalid Token";
            }
        }

        [HttpGet("checkdatabase",Name = "checkdatabase")]
        public dynamic checkdatabase()
        {
            try
            {
                var mainQuery = (from main in _objdb.Inventory
                             select main).ToList();
                return "Success Database Connection";
            }
            catch {
               return "Fail Database Connection";     
            } 
        }
    }
}