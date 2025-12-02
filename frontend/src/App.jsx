import { BrowserRouter, Route, Routes } from "react-router-dom";
import Home from "./pages/Home";
import Events from "./pages/Events";
import MyBookings from "./pages/MyBookings";
import EventDetails from "./pages/EventDetails";
import Login from "./pages/Login";
import Signup from "./pages/Signup";
import Navbar from "./components/Navbar";
import Logout from "./pages/Logout";
import ManageEvents from "./pages/ManageEvents";
import CreateEvent from "./pages/CreateEvent";
import EditEvent from "./pages/EditEvent";
import AdminNavbar from "./components/AdminNavbar";
import Dashboard from "./pages/Dashboard";

function App() {
  return (
    <BrowserRouter>
      <div className="min-h-screen bg-gray-50">
        <Routes>
          <Route path="/" element={<Navbar />}>
            <Route index element={<Home />} />
            <Route path="events" element={<Events />} />
            <Route path="events/:id" element={<EventDetails />} />
            <Route path="my-bookings" element={<MyBookings />} />
          </Route>
          <Route path="/logout" element={<Logout />} />
          <Route path="/login" element={<Login />} />
          <Route path="/signup" element={<Signup />} />
          <Route path="/admin" element={<AdminNavbar />} >
            <Route index element={<Dashboard />} />
            <Route path="create-event" element={<CreateEvent />} />
            <Route path="edit-event/:eventId" element={<EditEvent />} />
            <Route path="events" element={<ManageEvents />} />
          </Route>
        </Routes>
      </div>
    </BrowserRouter>
  );
}

export default App;