using System;
using System.Collections.Generic;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Models
{
    public class HostAnswersModel
    {
        public IEnumerable<PlayerAnswer> Answers { get; set; }
    }
}