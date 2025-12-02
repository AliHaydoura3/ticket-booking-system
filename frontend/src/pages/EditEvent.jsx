import AdminRoute from "../components/AdminRoute";
import { useEffect, useState } from "react";
import apiClient from "../api/axios";
import { useNavigate } from "react-router-dom";
import { useParams } from "react-router-dom";

const EditEvent = () => {
  const { eventId } = useParams();
  const [event, setEvent] = useState({
    name: "",
    date: "",
    category: "",
    description: "",
    totalSeats: 0,
    price: 0,
  });
  const [error, setError] = useState(null);
  const [loading, setLoading] = useState(false);
  const [fetchLoading, setFetchLoading] = useState(true);

  useEffect(() => {
    const fetchEvent = async () => {
      setFetchLoading(true);
      setError(null);
      try {
        const response = await apiClient.get(`/events/${eventId}`);
        const eventData = response.data.eventItem;
        setEvent({
          name: eventData.name,
          date: eventData.date.split("T")[0],
          category: eventData.category,
          description: eventData.description,
          totalSeats: eventData.totalSeats,
          price: eventData.price,
        });
      } catch (err) {
        setError(err.response?.data || "Failed to fetch event details");
      } finally {
        setFetchLoading(false);
      }
    };

    fetchEvent();
  }, [eventId]);

  const navigate = useNavigate();

  const onChange = (e) => {
    const { name, value, type } = e.target;
    setEvent((prevEvent) => ({
      ...prevEvent,
      [name]: type === "number" ? parseFloat(value) || 0 : value,
    }));
  };

  const onSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    try {
      await apiClient.put(`/events/${eventId}`, event);
      navigate("/admin/events");
    } catch (err) {
      setError(err.response?.data || "Failed to update event");
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    navigate("/admin/events");
  };

  if (fetchLoading) {
    return (
      <AdminRoute>
        <div className="min-h-screen bg-gray-50 flex items-center justify-center">
          <div className="text-center">
            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-purple-600 mx-auto mb-4"></div>
            <p className="text-gray-600">Loading event details...</p>
          </div>
        </div>
      </AdminRoute>
    );
  }

  return (
    <AdminRoute>
      <div className="min-h-screen bg-gray-50 py-8">
        <div className="max-w-3xl mx-auto px-4 sm:px-6 lg:px-8">
          {/* Header */}
          <div className="mb-8">
            <div className="flex items-center gap-4 mb-2">
              <button
                onClick={handleCancel}
                className="flex items-center gap-2 text-gray-600 hover:text-gray-900 transition-colors duration-200"
              >
                <svg className="w-5 h-5" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                  <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M10 19l-7-7m0 0l7-7m-7 7h18" />
                </svg>
                Back to Events
              </button>
            </div>
            <h1 className="text-3xl font-bold text-gray-900">Edit Event</h1>
            <p className="text-gray-600 mt-2">
              Update the event details below
            </p>
          </div>

          {/* Form */}
          <div className="card p-6">
            {error && (
              <div className="bg-red-50 border border-red-200 rounded-lg p-4 mb-6">
                <div className="flex items-center">
                  <svg className="w-5 h-5 text-red-400 mr-2" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                    <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 8v4m0 4h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                  </svg>
                  <span className="text-red-800">{error}</span>
                </div>
              </div>
            )}

            <form onSubmit={onSubmit} className="space-y-6">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                {/* Event Name */}
                <div className="md:col-span-2">
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Event Name *
                  </label>
                  <input
                    type="text"
                    name="name"
                    value={event.name}
                    onChange={onChange}
                    required
                    className="input-field"
                    placeholder="Enter event name"
                  />
                </div>

                {/* Date */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Date *
                  </label>
                  <input
                    type="date"
                    name="date"
                    value={event.date}
                    onChange={onChange}
                    required
                    className="input-field"
                  />
                </div>

                {/* Category */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Category *
                  </label>
                  <input
                    type="text"
                    name="category"
                    value={event.category}
                    onChange={onChange}
                    required
                    className="input-field"
                    placeholder="e.g., Music, Sports, Conference"
                  />
                </div>

                {/* Total Seats */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Total Seats *
                  </label>
                  <div className="relative">
                    <input
                      type="number"
                      name="totalSeats"
                      value={event.totalSeats}
                      onChange={onChange}
                      required
                      min="1"
                      className="input-field"
                      placeholder="Enter total seats"
                    />
                    <div className="absolute inset-y-0 right-0 pr-3 flex items-center pointer-events-none">
                      <span className="text-gray-500">seats</span>
                    </div>
                  </div>
                </div>

                {/* Price */}
                <div>
                  <label className="block text-sm font-medium text-gray-700 mb-2">
                    Price per Ticket *
                  </label>
                  <div className="relative">
                    <div className="absolute inset-y-0 left-0 pl-3 flex items-center pointer-events-none">
                      <span className="text-gray-500">$</span>
                    </div>
                    <input
                      type="number"
                      name="price"
                      value={event.price}
                      onChange={onChange}
                      required
                      min="0"
                      step="0.01"
                      className="input-field pl-10"
                      placeholder="0.00"
                    />
                  </div>
                  <p className="mt-1 text-xs text-gray-500">
                    Enter 0 for free events
                  </p>
                </div>
              </div>

              {/* Description */}
              <div>
                <label className="block text-sm font-medium text-gray-700 mb-2">
                  Description *
                </label>
                <textarea
                  name="description"
                  value={event.description}
                  onChange={onChange}
                  required
                  rows="4"
                  className="input-field resize-none"
                  placeholder="Provide a detailed description of the event..."
                ></textarea>
                <p className="mt-1 text-xs text-gray-500">
                  {event.description.length}/500 characters
                </p>
              </div>

              {/* Form Actions */}
              <div className="flex flex-col sm:flex-row gap-4 pt-6 border-t border-gray-200">
                <button
                  type="button"
                  onClick={handleCancel}
                  className="btn-secondary flex-1"
                >
                  Cancel
                </button>
                <button
                  type="submit"
                  disabled={loading}
                  className="btn-primary bg-purple-600 hover:bg-purple-700 focus:ring-purple-500 flex-1 flex items-center justify-center gap-2 disabled:opacity-50 disabled:cursor-not-allowed"
                >
                  {loading ? (
                    <>
                      <svg className="animate-spin h-4 w-4 text-white" fill="none" viewBox="0 0 24 24">
                        <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4" />
                        <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" />
                      </svg>
                      Updating Event...
                    </>
                  ) : (
                    <>
                      <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                        <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M4 4v5h.582m15.356 2A8.001 8.001 0 004.582 9m0 0H9m11 11v-5h-.581m0 0a8.003 8.003 0 01-15.357-2m15.357 2H15" />
                      </svg>
                      Update Event
                    </>
                  )}
                </button>
              </div>
            </form>
          </div>
        </div>
      </div>
    </AdminRoute>
  );
};

export default EditEvent;