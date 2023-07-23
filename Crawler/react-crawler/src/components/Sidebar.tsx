/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import { useContext, useEffect, useState } from "react";
import { NavLink, useNavigate } from "react-router-dom";
import {
  AppUserContext,
  OrdersContext,
  OrdersGetDateContext,
} from "../context/StateContext";
import api from "../utils/axiosinstance";
import { OrderGetByDateDto, OrderGetByDateQuery } from "../types/OrderTypes";

function Sidebar() {
  const { appUser, setAppUser } = useContext(AppUserContext); //Log out işlemi için gerekli
  //const { ordersByDate } = useContext(OrdersGetDateContext);
  const [orderCount, setOrderCount] = useState<undefined | number>();
  const [isHovered, setIsHovered] = useState(false);
  const navigate = useNavigate();
  const [orderGetByDateQuery] = useState<OrderGetByDateQuery>({
    isDeleted: null,
  });

  const openAside = () => {
    const button = document.querySelector<HTMLButtonElement>(
      '[data-drawer-toggle="default-sidebar"]'
    );
    const sidebar = document.querySelector<HTMLElement>("#default-sidebar");

    sidebar?.classList.toggle("-translate-x-full");
  };
  useEffect(() => {
    const button = document.querySelector<HTMLButtonElement>(
      '[data-drawer-toggle="default-sidebar"]'
    );

    button?.addEventListener("click", openAside);

    return () => {
      button?.removeEventListener("click", openAside);
    };
  }, []);

  useEffect(() => {
    const fetchOrderCount = async () => {
      const response = await api.post<OrderGetByDateDto[]>(
        "Orders/GetByDate",
        orderGetByDateQuery
      );

      const orders = response.data; // API'den alınan sipariş verileri
      setOrderCount(orders.length);
    };

    void fetchOrderCount(); // Fonksiyonu sayfa yüklendiğinde çalıştırın
  }, [orderGetByDateQuery]);

  const handleLogOut = () => {
    localStorage.removeItem("crawler_user");
    setAppUser(undefined);
    navigate("/login");
  };
  return (
    <>
      <button
        data-drawer-target="default-sidebar"
        data-drawer-toggle="default-sidebar"
        aria-controls="default-sidebar"
        type="button"
        className="inline-flex items-center p-2 mt-2 ml-3 text-sm text-gray-500 rounded-lg sm:hidden hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-600"
      >
        <span className="sr-only">Open sidebar</span>
        <svg
          className="w-6 h-6"
          aria-hidden="true"
          fill="currentColor"
          viewBox="0 0 20 20"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            clip-rule="evenodd"
            fill-rule="evenodd"
            d="M2 4.75A.75.75 0 012.75 4h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 4.75zm0 10.5a.75.75 0 01.75-.75h7.5a.75.75 0 010 1.5h-7.5a.75.75 0 01-.75-.75zM2 10a.75.75 0 01.75-.75h14.5a.75.75 0 010 1.5H2.75A.75.75 0 012 10z"
          ></path>
        </svg>
      </button>

      <aside
        id="default-sidebar"
        className="fixed top-0 left-0 z-40 w-64 h-screen transition-transform -translate-x-full sm:translate-x-0 Montserrat"
        aria-label="Sidebar"
      >
        <div className="h-full px-3 py-4 overflow-y-auto bg-gradient-to-b from-indigo-600 to bg-indigo-950 dark:bg-gray-800">
          <a href="#" className="flex items-center pl-2.5 mb-5">
            <img
              src="/t-apple-touch-icon.png"
              className="h-16 sm:h-16"
              alt="Crawler Logo"
            />
            <span className="self-center text-xl text-gray-50 font-semibold whitespace-nowrap dark:text-white">
              rawler
            </span>
          </a>
          <hr />
          <ul className="space-y-2 font-medium">
            <li>
              <NavLink
                to="/livetrack"
                className="flex items-center mt-10 p-2  text-gray-50 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700 group"
              >
                <svg
                  className="w-5 h-5 text-gray-50 transition duration-75 group-hover:text-indigo-600"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="currentColor"
                  viewBox="0 0 22 21"
                >
                  <path
                    fill-rule="evenodd"
                    d="M2.25 5.25a3 3 0 013-3h13.5a3 3 0 013 3V15a3 3 0 01-3 3h-3v.257c0 .597.237 1.17.659 1.591l.621.622a.75.75 0 01-.53 1.28h-9a.75.75 0 01-.53-1.28l.621-.622a2.25 2.25 0 00.659-1.59V18h-3a3 3 0 01-3-3V5.25zm1.5 0v7.5a1.5 1.5 0 001.5 1.5h13.5a1.5 1.5 0 001.5-1.5v-7.5a1.5 1.5 0 00-1.5-1.5H5.25a1.5 1.5 0 00-1.5 1.5z"
                    clip-rule="evenodd"
                  />
                </svg>
                <span className="ml-3 group-hover:text-indigo-600">
                  Live Track
                </span>
              </NavLink>
            </li>
            <li>
              <NavLink
                to="/"
                className="flex items-center p-2 text-gray-50 rounded-lg dark:text-white hover:bg-gray-100 dark:hover:bg-gray-700 group"
                onMouseEnter={() => setIsHovered(true)}
                onMouseLeave={() => setIsHovered(false)}
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  viewBox="0 0 24 24"
                  fill="currentColor"
                  className="flex-shrink-0 w-5 h-5 text-gray-50 transition duration-75 group-hover:text-indigo-600"
                >
                  <path
                    fill-rule="evenodd"
                    d="M5.625 1.5c-1.036 0-1.875.84-1.875 1.875v17.25c0 1.035.84 1.875 1.875 1.875h12.75c1.035 0 1.875-.84 1.875-1.875V12.75A3.75 3.75 0 0016.5 9h-1.875a1.875 1.875 0 01-1.875-1.875V5.25A3.75 3.75 0 009 1.5H5.625zM7.5 15a.75.75 0 01.75-.75h7.5a.75.75 0 010 1.5h-7.5A.75.75 0 017.5 15zm.75 2.25a.75.75 0 000 1.5H12a.75.75 0 000-1.5H8.25z"
                    clip-rule="evenodd"
                  />
                  <path d="M12.971 1.816A5.23 5.23 0 0114.25 5.25v1.875c0 .207.168.375.375.375H16.5a5.23 5.23 0 013.434 1.279 9.768 9.768 0 00-6.963-6.963z" />
                </svg>
                <span className="flex-1 ml-3 whitespace-nowrap group-hover:text-indigo-600">
                  Records
                </span>
                <span
                  className="inline-flex items-center justify-center px-2 ml-3 text-sm font-medium text-gray-800 bg-gray-100 rounded-full dark:bg-gray-700 dark:text-gray-300"
                  style={{
                    backgroundColor: isHovered ? "#4F46E5" : "",
                    color: isHovered ? "white" : "",
                    //transition: "background-color 0.25s, color 0.25s",
                  }}
                >
                  {orderCount}
                  {/* {ordersByDate.length} */}
                </span>
              </NavLink>
            </li>
            <li>
              <NavLink
                to="/users"
                className="flex items-center p-2 text-gray-50 rounded-lg dark:text-white hover:bg-gray-100 hover:text-indigo-600 dark:hover:bg-gray-700 group"
                //onClick={handleLogOut}
              >
                <svg
                  xmlns="http://www.w3.org/2000/svg"
                  viewBox="0 0 24 24"
                  fill="currentColor"
                  className="w-6 h-6"
                >
                  <path d="M4.5 6.375a4.125 4.125 0 118.25 0 4.125 4.125 0 01-8.25 0zM14.25 8.625a3.375 3.375 0 116.75 0 3.375 3.375 0 01-6.75 0zM1.5 19.125a7.125 7.125 0 0114.25 0v.003l-.001.119a.75.75 0 01-.363.63 13.067 13.067 0 01-6.761 1.873c-2.472 0-4.786-.684-6.76-1.873a.75.75 0 01-.364-.63l-.001-.122zM17.25 19.128l-.001.144a2.25 2.25 0 01-.233.96 10.088 10.088 0 005.06-1.01.75.75 0 00.42-.643 4.875 4.875 0 00-6.957-4.611 8.586 8.586 0 011.71 5.157v.003z" />
                </svg>
                <span className="flex-1 ml-3 whitespace-nowrap">Users</span>
              </NavLink>
            </li>
            <li>
              <NavLink
                to="/settings"
                className="flex items-center p-2 text-gray-50 rounded-lg dark:text-white hover:bg-gray-100 hover:text-indigo-600 dark:hover:bg-gray-700 group"
              >
                <svg
                  className="flex-shrink-0 w-5 h-5 text-gray-50 transition duration-75 dark:text-gray-400 group-hover:text-indigo-600 dark:group-hover:text-white"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    fill-rule="evenodd"
                    d="M11.078 2.25c-.917 0-1.699.663-1.85 1.567L9.05 4.889c-.02.12-.115.26-.297.348a7.493 7.493 0 00-.986.57c-.166.115-.334.126-.45.083L6.3 5.508a1.875 1.875 0 00-2.282.819l-.922 1.597a1.875 1.875 0 00.432 2.385l.84.692c.095.078.17.229.154.43a7.598 7.598 0 000 1.139c.015.2-.059.352-.153.43l-.841.692a1.875 1.875 0 00-.432 2.385l.922 1.597a1.875 1.875 0 002.282.818l1.019-.382c.115-.043.283-.031.45.082.312.214.641.405.985.57.182.088.277.228.297.35l.178 1.071c.151.904.933 1.567 1.85 1.567h1.844c.916 0 1.699-.663 1.85-1.567l.178-1.072c.02-.12.114-.26.297-.349.344-.165.673-.356.985-.57.167-.114.335-.125.45-.082l1.02.382a1.875 1.875 0 002.28-.819l.923-1.597a1.875 1.875 0 00-.432-2.385l-.84-.692c-.095-.078-.17-.229-.154-.43a7.614 7.614 0 000-1.139c-.016-.2.059-.352.153-.43l.84-.692c.708-.582.891-1.59.433-2.385l-.922-1.597a1.875 1.875 0 00-2.282-.818l-1.02.382c-.114.043-.282.031-.449-.083a7.49 7.49 0 00-.985-.57c-.183-.087-.277-.227-.297-.348l-.179-1.072a1.875 1.875 0 00-1.85-1.567h-1.843zM12 15.75a3.75 3.75 0 100-7.5 3.75 3.75 0 000 7.5z"
                    clip-rule="evenodd"
                  />
                </svg>
                <span className="flex-1 ml-3 whitespace-nowrap hover:text-indigo-600">
                  Settings
                </span>
              </NavLink>
            </li>

            <li>
              <NavLink
                to="/homepage"
                className="flex items-center p-2 text-gray-50 rounded-lg dark:text-white hover:bg-gray-100 hover:text-indigo-600 dark:hover:bg-gray-700 group"
                onClick={handleLogOut}
              >
                <svg
                  className="flex-shrink-0 w-5 h-5 text-gray-50 transition duration-75 dark:text-gray-400 group-hover:text-indigo-600 dark:group-hover:text-white"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    fill-rule="evenodd"
                    d="M7.5 3.75A1.5 1.5 0 006 5.25v13.5a1.5 1.5 0 001.5 1.5h6a1.5 1.5 0 001.5-1.5V15a.75.75 0 011.5 0v3.75a3 3 0 01-3 3h-6a3 3 0 01-3-3V5.25a3 3 0 013-3h6a3 3 0 013 3V9A.75.75 0 0115 9V5.25a1.5 1.5 0 00-1.5-1.5h-6zm10.72 4.72a.75.75 0 011.06 0l3 3a.75.75 0 010 1.06l-3 3a.75.75 0 11-1.06-1.06l1.72-1.72H9a.75.75 0 010-1.5h10.94l-1.72-1.72a.75.75 0 010-1.06z"
                    clip-rule="evenodd"
                  />
                </svg>
                <span className="flex-1 ml-3 whitespace-nowrap">Log out</span>
              </NavLink>
            </li>

            {/* //For home page */}
            {/* <li>
              <NavLink
                to="/homepage"
                className="flex items-center p-2 text-gray-50 rounded-lg dark:text-white hover:bg-gray-100 hover:text-indigo-600 dark:hover:bg-gray-700 group"
                //onClick={handleLogOut}
              >
                <svg
                  className="flex-shrink-0 w-5 h-5 text-gray-50 transition duration-75 dark:text-gray-400 group-hover:text-indigo-600 dark:group-hover:text-white"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    fill-rule="evenodd"
                    d="M7.5 3.75A1.5 1.5 0 006 5.25v13.5a1.5 1.5 0 001.5 1.5h6a1.5 1.5 0 001.5-1.5V15a.75.75 0 011.5 0v3.75a3 3 0 01-3 3h-6a3 3 0 01-3-3V5.25a3 3 0 013-3h6a3 3 0 013 3V9A.75.75 0 0115 9V5.25a1.5 1.5 0 00-1.5-1.5h-6zm10.72 4.72a.75.75 0 011.06 0l3 3a.75.75 0 010 1.06l-3 3a.75.75 0 11-1.06-1.06l1.72-1.72H9a.75.75 0 010-1.5h10.94l-1.72-1.72a.75.75 0 010-1.06z"
                    clip-rule="evenodd"
                  />
                </svg>
                <span className="flex-1 ml-3 whitespace-nowrap">HOME PAGe</span>
              </NavLink>
            </li> */}

            {/* //For Register */}
            {/* <li>
              <NavLink
                to="/register"
                className="flex items-center p-2 text-gray-50 rounded-lg dark:text-white hover:bg-gray-100 hover:text-indigo-600 dark:hover:bg-gray-700 group"
                //onClick={handleLogOut}
              >
                <svg
                  className="flex-shrink-0 w-5 h-5 text-gray-50 transition duration-75 dark:text-gray-400 group-hover:text-indigo-600 dark:group-hover:text-white"
                  aria-hidden="true"
                  xmlns="http://www.w3.org/2000/svg"
                  fill="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    fill-rule="evenodd"
                    d="M7.5 3.75A1.5 1.5 0 006 5.25v13.5a1.5 1.5 0 001.5 1.5h6a1.5 1.5 0 001.5-1.5V15a.75.75 0 011.5 0v3.75a3 3 0 01-3 3h-6a3 3 0 01-3-3V5.25a3 3 0 013-3h6a3 3 0 013 3V9A.75.75 0 0115 9V5.25a1.5 1.5 0 00-1.5-1.5h-6zm10.72 4.72a.75.75 0 011.06 0l3 3a.75.75 0 010 1.06l-3 3a.75.75 0 11-1.06-1.06l1.72-1.72H9a.75.75 0 010-1.5h10.94l-1.72-1.72a.75.75 0 010-1.06z"
                    clip-rule="evenodd"
                  />
                </svg>
                <span className="flex-1 ml-3 whitespace-nowrap">Register PAGe</span>
              </NavLink>
            </li> */}
          </ul>
        </div>
      </aside>
    </>
  );
}

export default Sidebar;
