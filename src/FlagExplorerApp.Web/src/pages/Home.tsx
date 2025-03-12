import CountryGrid from "../components/CountryGrid";

const Home = () => {
  return (
    <div className="p-6">
      <h1 className="text-2xl font-bold mb-4 text-center">Country List</h1>
      <CountryGrid />
    </div>
  );
};

export default Home;