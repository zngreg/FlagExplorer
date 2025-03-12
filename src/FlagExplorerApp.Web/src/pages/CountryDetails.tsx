import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { getCountryDetails } from "../services/countryApi";
import { CountryDetailsProps } from "../interfaces/ICountryDetails";
import "../styles/CountryDetails.css";

const CountryDetails = () => {
  const { name } = useParams<{ name: string }>();
  const [country, setCountry] = useState<CountryDetailsProps | null>(null);
  const [loading, setLoading] = useState(true);
  const [errorObj, setErrorObj] = useState(null);

  useEffect(() => {
    if (name) {
      getCountryDetails(name)
        .then((data) => {
          setCountry(data as CountryDetailsProps);
          setLoading(false);
        })
        .catch((error) => {
          console.error("Error fetching country details:", error);
          setErrorObj(error);
          setLoading(false);
        });
    }
  }, [name]);

  if (loading) return <p className="text-center">Loading...</p>;
  if (errorObj) return <p className="text-center text-red-500">Error fetching country details. Please try again later.</p>;
  if (!country) return <p className="text-center">Country not found</p>;

  return (
    <div className="p-6 text-center">
      <div className="country-details-header">
        <img src={country.flag} alt={country.name} height={25}></img>
        <h2 className="text-3xl font-bold">{country.name}</h2>
      </div>
      <p className="mt-4 text-lg">Population: {country.population.toLocaleString()}</p>
      <p className="text-lg">Capital: {country.capital}</p>
    </div>
  );
};

export default CountryDetails;