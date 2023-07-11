/* eslint-disable @typescript-eslint/no-unsafe-call */
import { NavLink, useNavigate } from "react-router-dom";
import { OrderGetByIdDto } from "../types/OrderTypes";
import { LocalUser } from "../types/AuthTypes";
import { ChevronLeftIcon } from "@heroicons/react/24/outline";
import { useContext } from "react";
import { AppUserContext, OrdersContext } from "../context/StateContext";

// export type NavbarProps = {

// };

const Navbar = () => {
  const { appUser, setAppUser } = useContext(AppUserContext);
  const { orders } = useContext(OrdersContext);
  const navigate = useNavigate();
  const handleLogOut = () => {
    localStorage.removeItem("crawler_user");
    setAppUser(undefined);
    navigate("/login");
  };
  return (
    <>
      <nav className="bg-indigo-600 Montserrat">
        <div className="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
          <div className="relative flex h-16 items-center justify-between">
            <div>
              <img
                className="block h-10 w-auto justify-start sm:justify-start"
                src="/apple-touch-icon.png"
                alt="Crawler"
              />
            </div>
            <div className="ml-1 text-gray-300 text-2xl">rawler</div>
            <div className="flex flex-1 items-center justify-center sm:items-stretch sm:justify-end">
              <div className="sm:ml-6 sm:block">
                <div className="flex space-x-4">
                  {appUser !== undefined && (
                    <NavLink
                      to="/dashboard"
                      className={
                        "text-gray-300 hover:bg-indigo-400 hover:text-white rounded-md px-3 py-2 text-md font-medium"
                      }
                    >
                      DashBoard
                    </NavLink>
                  )}
                  {appUser !== undefined && (
                    <NavLink
                      to="/"
                      className={
                        "text-gray-300 hover:bg-indigo-400 hover:text-white rounded-md px-3 py-2 text-md font-medium"
                      }
                    >
                      Order Track <span>({orders.length})</span>
                    </NavLink>
                  )}
                  {appUser !== undefined && (
                    <>
                      <NavLink
                        to="/login"
                        className={
                          "text-gray-300 hover:bg-indigo-400 hover:text-white rounded-md px-3 py-2 text-md font-medium"
                        }
                        onClick={handleLogOut}
                      >
                        Log Out <span>({orders.length})</span>
                      </NavLink>
                      <NavLink
                        to="*"
                        className={
                          "text-gray-300 hover:bg-indigo-400 hover:text-white rounded-md px-3 py-2 text-md font-medium"
                        }
                      >
                        Lol Page
                      </NavLink>
                    </>
                  )}
                  {appUser === undefined && (
                    <NavLink
                      to="/login"
                      className={
                        "text-gray-300 hover:bg-indigo-400 hover:text-white rounded-md px-3 py-2 text-md font-medium"
                      }
                    >
                      About Us
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        viewBox="0 0 24 24"
                        fill="currentColor"
                        className="w-5 h-5 inline"
                      >
                        <path
                          fillRule="evenodd"
                          d="M4.72 3.97a.75.75 0 011.06 0l7.5 7.5a.75.75 0 010 1.06l-7.5 7.5a.75.75 0 01-1.06-1.06L11.69 12 4.72 5.03a.75.75 0 010-1.06zm6 0a.75.75 0 011.06 0l7.5 7.5a.75.75 0 010 1.06l-7.5 7.5a.75.75 0 11-1.06-1.06L17.69 12l-6.97-6.97a.75.75 0 010-1.06z"
                          clipRule="evenodd"
                        />
                      </svg>
                    </NavLink>
                  )}
                </div>
              </div>
            </div>
          </div>
        </div>
      </nav>
      {/* --------------------------------------------------------------------------------- */}

      {/* <nav className="bg-white border-gray-200 dark:bg-gray-900 Montserrat">
        <div className="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
          <div className="flex items-center">
          <img
            src="/apple-touch-icon.png"
            className="h-10 mr-3 "
            alt="Flowbite Logo"
            
          />
          <span className="self-center text-2xl font-semibold whitespace-nowrap dark:text-white">Crawler Bot</span>
          </div>
          <div className="flex items-center md:order-2">
            <button
              type="button"
              className="flex mr-3 text-sm bg-gray-800 rounded-full md:mr-0 focus:ring-4 focus:ring-gray-300 dark:focus:ring-gray-600"
              id="user-menu-button"
              aria-expanded="false"
              data-dropdown-toggle="user-dropdown"
              data-dropdown-placement="bottom"
            >
              <span className="sr-only">Open user menu</span>
              <img
                className="w-8 h-8 rounded-full"
                src="/docs/images/people/profile-picture-3.jpg"
                alt="user photo"
              />
            </button>
            {/* <!-- Dropdown menu --> */}
      {/* <div
              className="z-50 hidden my-4 text-base list-none bg-white divide-y divide-gray-100 rounded-lg shadow dark:bg-gray-700 dark:divide-gray-600"
              id="user-dropdown"
            >
              <div className="px-4 py-3">
                <span className="block text-sm text-gray-900 dark:text-white">
                  Bonnie Green
                </span>
                <span className="block text-sm  text-gray-500 truncate dark:text-gray-400">
                  name@flowbite.com
                </span>
              </div>
              <ul className="py-2" aria-labelledby="user-menu-button">
                <li>
                  <a
                    href="#"
                    className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200 dark:hover:text-white"
                  >
                    Dashboard
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200 dark:hover:text-white"
                  >
                    Settings
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200 dark:hover:text-white"
                  >
                    Earnings
                  </a>
                </li>
                <li>
                  <a
                    href="#"
                    className="block px-4 py-2 text-sm text-gray-700 hover:bg-gray-100 dark:hover:bg-gray-600 dark:text-gray-200 dark:hover:text-white"
                  >
                    Sign out
                  </a>
                </li>
              </ul>
            </div>
            <button
              data-collapse-toggle="navbar-user"
              type="button"
              className="inline-flex items-center p-2 w-10 h-10 justify-center text-sm text-gray-500 rounded-lg md:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600"
              aria-controls="navbar-user"
              aria-expanded="false"
            >
              <span className="sr-only">Open main menu</span>
              <svg
                className="w-5 h-5"
                aria-hidden="true"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 17 14"
              >
                <path
                  stroke="currentColor"
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M1 1h15M1 7h15M1 13h15"
                />
              </svg>
            </button>
          </div>
          <div
            className="items-center justify-between hidden w-full md:flex md:w-auto md:order-1"
            id="navbar-user"
          >
            <ul className="flex flex-col font-medium p-4 md:p-0 mt-4 border border-gray-100 rounded-lg bg-gray-50 md:flex-row md:space-x-8 md:mt-0 md:border-0 md:bg-white dark:bg-gray-800 md:dark:bg-gray-900 dark:border-gray-700">
              <li>
                <NavLink
                  to="/login"
                  className="block py-2 pl-3 pr-4 text-white bg-blue-700 rounded md:bg-transparent md:text-blue-700 md:p-0 md:dark:text-blue-500"
                  
                >
                  Home
                </NavLink>
              </li>
              <li>
                <a
                  href="#"
                  className="block py-2 pl-3 pr-4 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700"
                >
                  About
                </a>
              </li>
              <li>
                <a
                  href="#"
                  className="block py-2 pl-3 pr-4 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700"
                >
                  Services
                </a>
              </li>
              <li>
                <a
                  href="#"
                  className="block py-2 pl-3 pr-4 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700"
                >
                  Pricing
                </a>
              </li>
              <li>
                <a
                  href="#"
                  className="block py-2 pl-3 pr-4 text-gray-900 rounded hover:bg-gray-100 md:hover:bg-transparent md:hover:text-blue-700 md:p-0 dark:text-white md:dark:hover:text-blue-500 dark:hover:bg-gray-700 dark:hover:text-white md:dark:hover:bg-transparent dark:border-gray-700"
                >
                  Contact
                </a>
              </li>
            </ul>
          </div>
        </div>
      </nav> */}
    </>
  );
};

export default Navbar;
