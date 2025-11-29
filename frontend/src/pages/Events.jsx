import { useState } from "react";
import { Link } from "react-router-dom";
import ProtectedRoute from "../components/ProtectedRoute";
import apiClient from "../api/axios";

const Events = () => {
  const [filter, setFilter] = useState({
    date: "",
    category: "",
    minPrice: "",
    maxPrice: "",
  });

  const [events, setEvents] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFilter((prevFilter) => ({
      ...prevFilter,
      [name]: value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      const fetechedEvents = await apiClient.get("/events/search", {
        params: {
          date: filter.date || null,
          category: filter.category || null,
          minPrice: filter.minPrice || null,
          maxPrice: filter.maxPrice || null,
        },
      });
      setEvents(fetechedEvents.data);
    } catch (err) {
      setError(err.response?.data || "Failed to fetch events");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>Events Page</h1>
      <form onSubmit={handleSubmit}>
        <input
          type="date"
          name="date"
          value={filter.date}
          onChange={handleChange}
        />
        <input
          type="text"
          name="category"
          placeholder="Category"
          value={filter.category}
          onChange={handleChange}
        />
        <input
          type="number"
          name="minPrice"
          placeholder="Min Price"
          value={filter.minPrice}
          onChange={handleChange}
        /> 
        <input
          type="number"
          name="maxPrice"
          placeholder="Max Price"
          value={filter.maxPrice}
          onChange={handleChange}
        />
        <button type="submit" disabled={loading}>
          Filter
        </button>
      </form>

      {loading && <div>Loading...</div>}
      {error && <div>Error: {error}</div>}

      <ul>
        {events.map((event) => (
          <li key={event.eventId}>
            <Link to={`/events/${event.eventId}`}>{event.name}</Link>
          </li>
        ))}
      </ul>
    </div>
  );
};

export default Events;
