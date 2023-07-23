/* eslint-disable @typescript-eslint/no-unsafe-assignment */
/* eslint-disable @typescript-eslint/no-unused-vars */
import { useContext, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { getClaimsFromJwt } from "../utils/jwtHelper";
import { AppUserContext } from "../context/StateContext";
import { LocalJwt } from "../types/AuthTypes";

function SocialLogin() {
  const [searchParams] = useSearchParams();

  const navigate = useNavigate();

  const { setAppUser } = useContext(AppUserContext);

  useEffect(() => {
    const accessToken = searchParams.get("access_token");

    const expiryDate = searchParams.get("expiry_date");

    if (accessToken !== null && expiryDate !== null) {
      const { uid, email, given_name, family_name } =
        getClaimsFromJwt(accessToken);

      const expires: string = expiryDate;

      setAppUser({
        id: uid,
        email,
        firstName: given_name,
        lastName: family_name,
        expires,
        accessToken,
      });

      const localJwt: LocalJwt = {
        accessToken,
        expires,
      };

      localStorage.setItem("crawler_user", JSON.stringify(localJwt));

      navigate("/");
    }

    
  }, []);

  return (
    <>
      <h1>functionName</h1>
    </>
  );
}

export default SocialLogin;
