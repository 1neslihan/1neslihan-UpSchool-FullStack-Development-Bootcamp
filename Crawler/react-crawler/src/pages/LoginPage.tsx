/* eslint-disable @typescript-eslint/restrict-template-expressions */
/* eslint-disable @typescript-eslint/no-misused-promises */
/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-argument */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import React, { useContext, useState } from "react";
import axios from "axios";

import { AuthLoginCommand, LocalJwt, LocalUser } from "../types/AuthTypes.ts";
import { getClaimsFromJwt } from "../utils/jwtHelper.ts";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { AppUserContext } from "../context/StateContext.ts";

const BASE_URL=import.meta.env.VITE_API_URL;
// export type LoginPageProps = {

// };

function LoginPage() {

  const { setAppUser } = useContext(AppUserContext);
  
  const [userName, setUserName] = useState("");

  const [authLoginCommand, setAuthLoginCommand] = useState<AuthLoginCommand>({
    email: "",
    password: "",
  });

  const navigate = useNavigate();

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();

    const response = await axios.post(
      `${BASE_URL}Authentication/Login`,
      authLoginCommand
    );
    console.log(response);
    console.log(response.data.accessToken);

    if (response.status === 200) {
      const accessToken = response.data.accessToken;

      const { uid, email, given_name, family_name } =
        getClaimsFromJwt(accessToken);

      const expires: string = response.data.expires;

      setAppUser({
        id: uid,
        email,
        firstName: given_name,
        lastName: family_name,
        expires: response.data.expires,
        accessToken,
      });

      const localJwt: LocalJwt = {
        accessToken,
        expires,
      };

      localStorage.setItem("crawler_user", JSON.stringify(localJwt));

      navigate("/");
    } else {
      toast.error(response.statusText);
    }
  };

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    setAuthLoginCommand({
      ...authLoginCommand,
      [event.target.name]: event.target.value,
    });
  };

  const onGoogleLoginClick = (e: React.FormEvent) => {
    // Handle Google login
    e.preventDefault();

    console.log(authLoginCommand);
  };
  return (
    <>
      <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
        <h1 className="font-bold text-2xl text-center mb-6">{userName}</h1>
        <div className="sm:mx-auto sm:w-full sm:max-w-sm">
          <img
            className="mx-auto h-20 w-auto"
            src="/apple-touch-icon.png"
            alt="Your Company"
          />
          <h2 className="mt-10 text-center text-2xl font-bold leading-9 tracking-tight text-gray-900 Montserrat">
            Sign in to your account
          </h2>
        </div>

        <div className="mt-10 sm:mx-auto sm:w-full sm:max-w-sm">
          <form className="space-y-6" onSubmit={handleSubmit}>
            <div>
              <label
                htmlFor="email"
                className="block text-left text-sm font-medium font-semibold leading-6 text-gray-900 Montserrat"
              >
                Email address
              </label>
              <div className="mt-2">
                <input
                  id="email"
                  name="email"
                  type="email"
                  value={authLoginCommand.email}
                  onChange={handleInputChange}
                  autoComplete="email"
                  required
                  className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 lg:text-lg md:leading-6"
                />
              </div>
            </div>

            <div>
              <div className="flex items-center justify-between">
                <label
                  htmlFor="password"
                  className="block text-sm font-medium font-semibold leading-6 text-gray-900 Montserrat"
                >
                  Password
                </label>
                <div className="text-sm Montserrat">
                  <a
                    href="#"
                    className="font-semibold text-indigo-600 hover:text-indigo-500 login"
                  >
                    Forgot password?
                  </a>
                </div>
              </div>
              <div className="mt-2">
                <input
                  id="password"
                  name="password"
                  type="password"
                  value={authLoginCommand.password}
                  onChange={handleInputChange}
                  autoComplete="current-password"
                  required
                  className="block w-full rounded-md border-0 py-1.5 text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 placeholder:text-gray-400 focus:ring-2 focus:ring-inset focus:ring-indigo-600 sm:text-sm sm:leading-6"
                />
              </div>
            </div>

            <div>
              <button
                type="submit"
                className="flex w-full justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600 Montserrat"
              >
                Sign in
              </button>
            </div>
          </form>

          <p className="mt-10 text-center text-sm text-gray-500 Montserrat">
            Not a member?{" "}
            <a
              href="#"
              className="font-semibold leading-6 text-indigo-600 hover:text-indigo-500 Montserrat"
            >
              Start a 14 day free trial
            </a>
          </p>
        </div>
      </div>
      <button
        id="toggleButton"
        onClick={() => setUserName("Welcome!")}
        className="flex mx-auto justify-center rounded-md bg-indigo-600 px-3 py-1.5 text-sm font-semibold leading-6 text-white shadow-sm hover:bg-indigo-500 focus-visible:outline focus-visible:outline-2 focus-visible:outline-offset-2 focus-visible:outline-indigo-600"
        // style={{ display: "flex", marginLeft: "auto", marginRight: "auto" }}
      >
        Click me
      </button>
    </>
  );
}

export default LoginPage;
