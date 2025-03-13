import { Link } from "react-router-dom";
import '../styles/CountryCard.css';

interface CountryCardProps {
  name: string;
  flag: string;
}

const CountryCard: React.FC<CountryCardProps> = ({ name, flag }) => {
  return (
    <Link
      to={`/country/${encodeURIComponent(name)}`}
      className="country-item relative block w-full h-[150px] sm:h-[180px] md:h-[200px] border rounded-lg shadow-md hover:shadow-lg transition overflow-hidden"
      style={{
        backgroundImage: `url(${flag})`,
        backgroundSize: "cover",
        backgroundPosition: "center"
      }}
      aria-label={name}
    >
      <div className="absolute inset-0 bg-black bg-opacity-10 hover:bg-opacity-20 transition"></div>
    </Link>
  );
};

export default CountryCard;