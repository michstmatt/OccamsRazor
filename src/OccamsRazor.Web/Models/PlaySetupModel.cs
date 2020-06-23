using System;
using System.Collections.Generic;

using OccamsRazor.Common.Models;

namespace OccamsRazor.Web.Models
{
    public class PlaySetupModel
    {
        public IEnumerable<GameMetadata> Games { get; set; }
    }
}