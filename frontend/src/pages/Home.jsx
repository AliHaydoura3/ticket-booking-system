import ProtectedRoute from "../components/ProtectedRoute";
import { useContext } from "react";
import { AuthContext } from "../context/AuthContext";

const Home = () => {
  const { user } = useContext(AuthContext);

  return (
    <ProtectedRoute>
      <div>
        <h1>Welcome to the Ticket Booking System</h1>
        <p>Book your tickets easily and quickly.</p>
        {user && <h2>Hello, {user.userName}!</h2>}
      </div>
    </ProtectedRoute>
  );
};

export default Home;
