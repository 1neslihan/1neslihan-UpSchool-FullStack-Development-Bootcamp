/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import "./App.css";
import { Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbar";
import LoginPage from "./pages/LoginPage";
import OrderTrackPage from "./pages/OrderTrackPage";
import NotFoundPage from "./pages/NotFoundPage";
import { useState, useEffect } from "react";
import { OrderGetByIdDto } from "./types/OrderTypes";
import { LocalJwt, LocalUser } from "./types/AuthTypes";
import { ToastContainer } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { getClaimsFromJwt } from "./utils/jwtHelper";
import { AppUserContext, OrdersContext } from "./context/StateContext";
import { dummyOrders } from "./utils/dummyOrders";
import ProtectedRoute from "./components/ProdectedRoute";

function App() {
  const [orders, setOrders] = useState<OrderGetByIdDto[]>(dummyOrders);
  const [appUser, setAppUser] = useState<LocalUser | undefined>(undefined);
  const navigate = useNavigate();

  useEffect(() => {
    const jwtJson = localStorage.getItem("crawler_user");

    if (!jwtJson) {
      navigate("/login");

      return;
    }

    const localJwt: LocalJwt = JSON.parse(jwtJson);

    const { uid, email, given_name, family_name } = getClaimsFromJwt(
      localJwt.accessToken
    );

    const expires: string = localJwt.expires;

    setAppUser({
      id: uid,
      email,
      firstName: given_name,
      lastName: family_name,
      expires,
      accessToken: localJwt.accessToken,
    });
  }, []);

  return (
    <>
      <AppUserContext.Provider value={{ appUser, setAppUser }}>
        <OrdersContext.Provider value={{ orders, setOrders }}>
          <ToastContainer />
          <Navbar />
          <Routes>
            <Route
              path="/"
              element={
                <ProtectedRoute>
                  <OrderTrackPage />
                </ProtectedRoute>
              }
            />
            <Route path="/login" element={<LoginPage />} />
            <Route path="*" element={
              <ProtectedRoute>
                <NotFoundPage />
              </ProtectedRoute>
            } />
          </Routes>
        </OrdersContext.Provider>
      </AppUserContext.Provider>
    </>
  );
}

export default App;
