using System;
using System.Collections.Generic;

namespace FlagExplorerApp.Services.Models
{
    public class CountryInfo
    {
        public NameInfo Name { get; set; }
        public List<string> Capital { get; set; }
        public string Flag { get; set; }
        public FlagInfo Flags { get; set; }
        public long Population { get; set; }
    }

    public class NameInfo
    {
        public string Common { get; set; }
    }

    public class FlagInfo
    {
        public string Png { get; set; }
        public string Svg { get; set; }
        public string Alt { get; set; }
    }
}