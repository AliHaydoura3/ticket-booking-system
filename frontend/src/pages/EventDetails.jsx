import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import apiClient from "../api/axios";
import ProtectedRoute from "../components/ProtectedRoute";

const EventDetails = () => {
  const [event, setEvent] = useState(null);
  const { id } = useParams();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchEvent = async () => {
      try {
        const response = await apiClient.get(`/events/${id}`);
        setEvent(response.data);
      } catch (err) {
        console.log(err);
        setError(err.response?.data || "Failed to fetch event details");
      } finally {
        setLoading(false);
      }
    };

    fetchEvent();
  }, [id]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (error) {
    return <div>Error: {error}</div>;
  }

  return (
    <ProtectedRoute>
      <div>
        <h1>{event.name}</h1>
        <p>{event.description}</p>
        <p>Date: {new Date(event.date).toLocaleDateString()}</p>
        <p>Category: {event.category}</p>
        <p>Price: ${event.price}</p>
        <p>Available Seats: {event.availableSeats}</p>
      </div>
    </ProtectedRoute>
  );
};

export default EventDetails;
