using System;
using System.Collections.Generic;

namespace FlagExplorerApp.Services.Models
{
    public class CountryInfo
    {
        public NameInfo Name { get; set; }
        public List<string> Tld { get; set; }
        public string Cca2 { get; set; }
        public string Ccn3 { get; set; }
        public string Cca3 { get; set; }
        public string Cioc { get; set; }
        public bool Independent { get; set; }
        public string Status { get; set; }
        public bool UnMember { get; set; }
        public Dictionary<string, Currency> Currencies { get; set; }
        public IddInfo Idd { get; set; }
        public List<string> Capital { get; set; }
        public List<string> AltSpellings { get; set; }
        public string Region { get; set; }
        public string Subregion { get; set; }
        public Dictionary<string, string> Languages { get; set; }
        public Dictionary<string, Translation> Translations { get; set; }
        public List<double> Latlng { get; set; }
        public bool Landlocked { get; set; }
        public List<string> Borders { get; set; }
        public double Area { get; set; }
        public Dictionary<string, Demonym> Demonyms { get; set; }
        public string Flag { get; set; }
        public MapInfo Maps { get; set; }
        public long Population { get; set; }
        public Dictionary<string, double> Gini { get; set; }
        public string Fifa { get; set; }
        public CarInfo Car { get; set; }
        public List<string> Timezones { get; set; }
        public List<string> Continents { get; set; }
        public FlagInfo Flags { get; set; }
        public CoatOfArmsInfo CoatOfArms { get; set; }
        public string StartOfWeek { get; set; }
        public CapitalInfo CapitalInfo { get; set; }
        public PostalCodeInfo PostalCode { get; set; }
    }

    public class NameInfo
    {
        public string Common { get; set; }
        public string Official { get; set; }
        public Dictionary<string, NativeName> NativeName { get; set; }
    }

    public class NativeName
    {
        public string Official { get; set; }
        public string Common { get; set; }
    }

    public class Currency
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
    }

    public class IddInfo
    {
        public string Root { get; set; }
        public List<string> Suffixes { get; set; }
    }

    public class Translation
    {
        public string Official { get; set; }
        public string Common { get; set; }
    }

    public class Demonym
    {
        public string F { get; set; }
        public string M { get; set; }
    }

    public class MapInfo
    {
        public string GoogleMaps { get; set; }
        public string OpenStreetMaps { get; set; }
    }

    public class CarInfo
    {
        public List<string> Signs { get; set; }
        public string Side { get; set; }
    }

    public class FlagInfo
    {
        public string Png { get; set; }
        public string Svg { get; set; }
        public string Alt { get; set; }
    }

    public class CoatOfArmsInfo
    {
        public string Png { get; set; }
        public string Svg { get; set; }
    }

    public class CapitalInfo
    {
        public List<double> Latlng { get; set; }
    }

    public class PostalCodeInfo
    {
        public string Format { get; set; }
        public string Regex { get; set; }
    }
}