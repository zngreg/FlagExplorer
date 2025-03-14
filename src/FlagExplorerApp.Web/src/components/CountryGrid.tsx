import { useEffect, useState } from "react";
import { getAllCountries } from "../services/CountryApi";
import CountryCard from "./CountryCard";
import { ICountry } from "../interfaces/ICountry";
import "../styles/CountryGrid.css";

const CountryGrid = () => {
  const [countries, setCountries] = useState<ICountry[]>([]);
  const [loading, setLoading] = useState(true);
  const [errorObj, setErrorObj] = useState(null);

  useEffect(() => {
    getAllCountries()
      .then((data) => {
        setCountries(data as ICountry[]);
        setLoading(false);
      })
      .catch((error) => {
        console.error("Error fetching countries:", error);
        setErrorObj(error);
        setLoading(false);
      });
  }, []);

  if (loading) return <p className="text-center">Loading countries...</p>;

  if(errorObj) return <p className="text-center text-red-500">Error fetching countries. Please try again later.</p>;

  return (
    <div
      className="country-grid"
    >
      {countries.map((country) => (
        <CountryCard key={country.name} name={country.name} flag={country.flag} />
      ))}
    </div>
  );
};

export default CountryGrid;