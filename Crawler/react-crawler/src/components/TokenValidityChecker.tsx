/* eslint-disable @typescript-eslint/no-unsafe-assignment */
/* eslint-disable @typescript-eslint/no-unsafe-argument */
/* eslint-disable @typescript-eslint/no-unsafe-call */
import { useContext, useEffect } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { AppUserContext } from "../context/StateContext.ts";
import { isTokenExpired } from "../utils/jwtHelper.ts";
import { toast } from "react-toastify";

const TokenValidityChecker = () => {
  const { appUser, setAppUser } = useContext(AppUserContext);
  const navigate = useNavigate();
  const { pathname } = useLocation();

  useEffect(() => {
    const storedJwt = localStorage.getItem("crawler_user");

    if (storedJwt) {
      const localJwt = JSON.parse(storedJwt);
      const { accessToken, expires } = localJwt;

      if (isTokenExpired(expires)) {
        localStorage.removeItem("crawler_user");
        setAppUser(undefined);
        toast.error("Oturum süreniz dolmuş. Lütfen tekrar giriş yapın.");
        navigate("/login");
      }
    }

    const intervalId = setInterval(() => {
      const storedJwt = localStorage.getItem("crawler_user");

      if (storedJwt) {
        const localJwt = JSON.parse(storedJwt);
        const { accessToken, expires } = localJwt;

        if (isTokenExpired(expires)) {
          localStorage.removeItem("crawler_user");
          setAppUser(undefined);
          toast.error("Session timeout. Please login.");
          navigate("/homepage");
        }
      }

      if (
        pathname === "/homepage" ||
        pathname === "/login" ||
        pathname === "/register"
      ) {
        localStorage.removeItem("crawler_user");
        setAppUser(undefined);
      }
    }, 5000);

    return () => {
      clearInterval(intervalId);
    };
  }, [appUser, setAppUser, navigate]);

  return null;
};

export default TokenValidityChecker;
