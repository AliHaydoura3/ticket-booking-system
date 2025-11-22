import { useState } from "react";
import { Link } from "react-router-dom";

const Events = () => {
  const [searchTerm, setSearchTerm] = useState("");
  const events = [];

  return (
    <div>
      <h1>Events Page</h1>
      <input
        type="text"
        placeholder="Search events..."
        onChange={(e) => setSearchTerm(e.target.value)}
      />
      <button>Search</button>
      <ul>
        {events
          .filter((event) =>
            event.name.toLowerCase().includes(searchTerm.toLowerCase())
          )
          .map((event) => (
            <li key={event.id}>
              <h2>{event.name}</h2>
              <p>Date: {event.date}</p>
              <p>Category: {event.category}</p>
              <p>Price: ${event.price}</p>
              <p>Available Seats: {event.seats}</p>
              <Link to={`/events/${event.id}`}>View Details</Link>
            </li>
          ))}
      </ul>
    </div>
  );
};

export default Events;
