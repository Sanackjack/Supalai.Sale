using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Console;

namespace ClassifiedAds.Infrastructure.Logging
{
    public class CustomWrappingConsoleFormatterOptions : ConsoleFormatterOptions
    {
        public string? CustomPrefix { get; set; }

        public string? CustomSuffix { get; set; }
    }
}
