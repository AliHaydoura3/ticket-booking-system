import { createContext, useEffect, useState } from "react";
import apiClient from "../api/axios";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (token) {
      apiClient
        .get("/auth/me")
        .then((response) => {
          setUser(response.data.user);
        })
        .catch(() => {
          setUser(null);
          localStorage.removeItem("authToken");
        })
        .finally(() => {
          setLoading(false);
        });
    } else {
      Promise.resolve().then(() => {
        setLoading(false);
      });
    }
  }, []);

  const login = async (usernameOrEmail, password) => {
    const response = await apiClient.post("/auth/login", {
      usernameOrEmail,
      password,
    });
    const { token, user } = response.data;
    localStorage.setItem("authToken", token);
    setUser(user);
  };

  const signup = async (userName, email, password, isAdmin) => {
    const response = await apiClient.post("/auth/register", {
      userName,
      email,
      password,
      isAdmin,
    });
    const { token, user } = response.data;
    localStorage.setItem("authToken", token);
    setUser(user);
  };

  const logout = () => {
    localStorage.removeItem("authToken");
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ user, loading, login, logout, signup }}>
      {children}
    </AuthContext.Provider>
  );
};

export { AuthContext, AuthProvider };
