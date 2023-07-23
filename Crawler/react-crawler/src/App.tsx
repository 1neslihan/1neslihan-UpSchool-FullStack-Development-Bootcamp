/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import "./App.css";
import { Routes, Route } from "react-router-dom";
import LoginPage from "./pages/LoginPage";
import NotFoundPage from "./pages/NotFoundPage";
import { useState } from "react";
import {
  OrderGetByDateDto,
} from "./types/OrderTypes";
import { LocalUser } from "./types/AuthTypes";
import { ToastContainer } from "react-toastify";
import {
  AppUserContext,
  OrdersGetDateContext,
} from "./context/StateContext";
import ProtectedRoute from "./components/ProdectedRoute";
import Records from "./pages/Records";
import LiveTrack from "./pages/LiveTrack";
import Settings from "./pages/Settings";
import SocialLogin from "./pages/SocialLogin";
import TokenValidityChecker from "./components/TokenValidityChecker";
import HomePage from "./pages/HomePage";
import RegisterPage from "./pages/RegisterPage";
import UsersPage from "./pages/UsersPage";

function App() {
  const [ordersByDate, setOrdersByDate] = useState<OrderGetByDateDto[]>([]);
  const [appUser, setAppUser] = useState<LocalUser | undefined>(undefined);


  return (
    <>
      <AppUserContext.Provider value={{ appUser, setAppUser }}>
        <OrdersGetDateContext.Provider
          value={{ ordersByDate, setOrdersByDate }}
        >
          <ToastContainer />
          <TokenValidityChecker />
          <Routes>
            <Route
              path="/"
              element={
                <ProtectedRoute>
                  <Records />
                </ProtectedRoute>
              }
            />
            <Route path="/login" element={<LoginPage />} />
            <Route path="/social-login" element={<SocialLogin />} />
            <Route path="/homepage" element={<HomePage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route
              path="/livetrack"
              element={
                <ProtectedRoute>
                  <LiveTrack />
                </ProtectedRoute>
              }
            />
            <Route
              path="/settings"
              element={
                <ProtectedRoute>
                  <Settings />
                </ProtectedRoute>
              }
            />
            <Route
              path="/users"
              element={
                <ProtectedRoute>
                  <UsersPage />
                </ProtectedRoute>
              }
            />

            <Route
              path="*"
              element={
                <ProtectedRoute>
                  <NotFoundPage />
                </ProtectedRoute>
              }
            />
          </Routes>
        </OrdersGetDateContext.Provider>
      </AppUserContext.Provider>
    </>
  );
}

export default App;
