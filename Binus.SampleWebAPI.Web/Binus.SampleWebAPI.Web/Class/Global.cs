using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Binus.SampleWebAPI.Web.Class
{
    public static class Global
    {

        /// <summary>
        /// WebAPI Base URL
        /// </summary>
        public static string WebAPIBaseURL = ConfigurationManager.AppSettings["WebAPIBaseURL"].ToString();
        public enum Method { GET=1, POST=2 };
    
    }
}