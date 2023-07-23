/* eslint-disable @typescript-eslint/no-misused-promises */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unused-vars */
/* eslint-disable @typescript-eslint/no-unsafe-argument */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import React, { useState, useEffect, useContext } from "react";
import { OrdersGetDateContext } from "../context/StateContext";
import api from "../utils/axiosinstance";
import { OrderGetByDateDto, OrderGetByDateQuery } from "../types/OrderTypes";
import * as signalR from "@microsoft/signalr";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import Sidebar from "../components/Sidebar";
import * as Excel from "exceljs";

function Record() {
  enum BotStatus {
    BotStarted = 1,
    CrawlingStarted = 2,
    CrawlingCompleted = 3,
    CrawlingFailed = 4,
    OrderCompleted = 5,
    BotClosed = 6,
  }
  enum ProductCrawl {
    All = 0,
    OnDiscount = 1,
    NonDiscount = 2,
  }
  const { ordersByDate, setOrdersByDate } = useContext(OrdersGetDateContext);

  const navigate = useNavigate();
  const [orderGetByDateQuery, setOrderGetByDateQuery] =
    useState<OrderGetByDateQuery>({
      isDeleted: null,
    });

  const [isModalOpen, setIsModalOpen] = useState(false); // State for modal visibility
  const [products, setProducts] = useState<string>("0");
  const [requiredNumber, setRequiredNumber] = useState<number>(0);

  function booleanToEnglish(value: boolean) {
    return value ? "True" : "False";
  }

  const exportToExcel = async (ordersByDateIndex: number) => {
    const workbook = new Excel.Workbook();
    const worksheet = workbook.addWorksheet("Order_Details");

    const headerRow = worksheet.addRow([
      "Product_Name",
      "Is_on_Sale",
      "Discounted_Price",
      "Non_discount_Price",
      "Image_Path",
    ]);

    headerRow.font = { bold: true };
    headerRow.fill = {
      type: "pattern",
      pattern: "solid",
      fgColor: { argb: "C5A0E9" },
    };

    const orders = ordersByDate[ordersByDateIndex];

    if (orders && orders.productDtos.length > 0) {
      orders.productDtos.forEach((pro, index) => {
        const row = worksheet.addRow([
          pro.name,
          booleanToEnglish(pro.isOnSale),
          pro.salePrice,
          pro.price,
          pro.picture,
        ]);

        // Renk atama işlemleri
        const bgColor = index % 2 === 0 ? "DDD9E1" : "FFFFFF";
        row.fill = {
          type: "pattern",
          pattern: "solid",
          fgColor: { argb: bgColor },
        };
        console.log(pro.isOnSale);
      });
    }

    const buffer = await workbook.xlsx.writeBuffer();
    const blob = new Blob([buffer], {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    });
    const url = URL.createObjectURL(blob);
    const a = document.createElement("a");
    a.href = url;
    a.download = "Order_Details.xlsx";
    a.click();

    URL.revokeObjectURL(url);
  };

  const handleSoftDeleteClick = (orderId: string) => {
    const SoftDelete = async () => {
      const response = await api.put("Orders/SoftDelete", {
        id: orderId,
      });
    };

    void SoftDelete();
  };

  const handleUndoDeleteClick = (orderId: string) => {
    const UndoDelete = async () => {
      const response = await api.put("Orders/UndoDelete", {
        id: orderId,
      });
    };
    void UndoDelete();
  };

  const handleHardDeleteClick = (orderId: string) => {
    const HardDelete = async () => {
      const response = await api.delete("Orders/HardDelete", {
        data: {
          id: orderId,
        },
      });
    };
    void HardDelete();
  };

  //For accordion
  const [activeIndex, setActiveIndex] = useState<number | null>(null);
  const toggleAccordion = (index: number) => {
    setActiveIndex((prevIndex) => (prevIndex === index ? null : index));
  };

  const isAccordionActive = (index: number) => {
    return activeIndex === index;
  };
  //For accordion

  const [hubConnection, setHubConnection] =
    useState<signalR.HubConnection | null>(null);

  const closeModal = () => {
    const modal = document.getElementById("my-modal");
    if (modal) {
      modal.style.display = "none";
    }
    setIsModalOpen(false);
  };

  // control if user trying to type negative number
  const handleKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key !== "Backspace" && isNaN(Number(event.key))) {
      event.preventDefault(); // Pozitif olmayan karakterleri engelle
    }
  };

  const sendDataToConsole = async (): Promise<void> => {
    const jwtJson: string | null = localStorage.getItem("crawler_user");
    if (jwtJson) {
      const parsedData = JSON.parse(jwtJson);
      const token = parsedData.accessToken;

      console.log("Data transfer hub başladı", requiredNumber, products, token);

      if (hubConnection) {
        try {
          await hubConnection.invoke(
            "SendDataToConsole",
            requiredNumber,
            parseInt(products),
            token
          );
        } catch (error) {
          console.error(error);
        }
      }
      const response = await api.get("Users/Pull");
      const data = response.data;
      if (data.toasterNotificationEnable) {
        toast.success("New order generated");
      }

      navigate("/livetrack");
    }
  };

  //POSTING API REQUESTS
  useEffect(() => {
    const fetchOrders = async () => {
      const response = await api.post<OrderGetByDateDto[]>(
        "Orders/GetByDate",
        orderGetByDateQuery
      );

      const sortedOrders = response.data.sort(
        (a, b) =>
          new Date(b.orderCreatedOn).getTime() -
          new Date(a.orderCreatedOn).getTime()
      );
      setOrdersByDate(sortedOrders);
    };

    void fetchOrders();

    return;
  }, [ordersByDate]);

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7090/Hubs/DataTransferHub")
      .withAutomaticReconnect()
      .build();

    setHubConnection(connection);

    connection
      .start()
      .then(() => {
        console.log("Data Transfer Hub connection started.");
      })
      .catch((error) => {
        console.error("Data Transfer Hub connection failed to start: ", error);
      });

    return () => {
      connection
        .stop()
        .then(() => {
          console.log("Data Transfer Hub connection stopped.");
        })
        .catch((error) => {
          console.error("Data Transfer Hub connection failed to stop: ", error);
        });
    };
  }, []);

  return (
    <>
      <Sidebar />
      {isModalOpen && (
        <div
          id="my-modal"
          className="relative z-50"
          aria-labelledby="modal-title"
          role="dialog"
          aria-modal="true"
        >
          <div className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity"></div>

          <div className="fixed inset-0 z-10 overflow-y-auto">
            <div className="flex min-h-full items-end justify-center p-4 text-center sm:items-center sm:p-0">
              <div className="relative transform overflow-hidden rounded-lg bg-white text-left shadow-xl transition-all sm:my-8 sm:w-full sm:max-w-lg">
                <div className="bg-white px-4 pb-4 pt-5 sm:p-6 sm:pb-4">
                  <div className="sm:flex sm:items-start">
                    {/* Close button */}
                    <button
                      type="button"
                      className="absolute top-3 right-2.5 text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm w-8 h-8 ml-auto inline-flex justify-center items-center dark:hover:bg-gray-600 dark:hover:text-white"
                      data-modal-hide="authentication-modal"
                      onClick={() => {
                        closeModal();
                      }}
                    >
                      <svg
                        className="w-3 h-3"
                        aria-hidden="true"
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 14 14"
                      >
                        <path
                          stroke="currentColor"
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          strokeWidth="2"
                          d="m1 1 6 6m0 0 6 6M7 7l6-6M7 7l-6 6"
                        />
                      </svg>
                      <span className="sr-only">Close modal</span>
                    </button>

                    <div className="mx-auto flex h-12 w-12 flex-shrink-0 items-center justify-center rounded-full bg-blue-100 sm:mx-0 sm:h-10 sm:w-10">
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth="1.5"
                        stroke="currentColor"
                        className="w-6 h-6 text-blue-600"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          d="M11.25 11.25l.041-.02a.75.75 0 011.063.852l-.708 2.836a.75.75 0 001.063.853l.041-.021M21 12a9 9 0 11-18 0 9 9 0 0118 0zm-9-3.75h.008v.008H12V8.25z"
                        />
                      </svg>
                    </div>
                    <div className="mt-3 text-center sm:ml-4 sm:mt-0 sm:text-left">
                      <h3
                        className="text-2xl font-semibold leading-6 text-gray-900"
                        id="modal-title"
                      >
                        Generate Order
                      </h3>
                      <br />
                      <div className="mt-2">
                        <label
                          htmlFor="products"
                          className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                        >
                          Choose product type
                        </label>
                        <select
                          id="products"
                          className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                          onChange={(e) => setProducts(e.target.value)}
                        >
                          <option selected value="0">
                            All
                          </option>
                          <option value="1">Discount</option>
                          <option value="2">Non discount</option>
                        </select>
                        <br />
                        <label
                          htmlFor="requiredNumber"
                          className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                        >
                          Required Number
                        </label>
                        <input
                          type="number"
                          id="requiredNumber"
                          min="0"
                          className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
                          defaultValue={0}
                          onKeyDown={handleKeyDown}
                          required
                          onChange={(e) =>
                            setRequiredNumber(Number(e.target.value))
                          }
                        ></input>
                        <br />
                        <p className="text-sm text-gray-500">
                          Please choose what type of product are you looking for
                          and enter how much of them you want.
                        </p>
                      </div>
                    </div>
                  </div>
                </div>
                <div className="bg-gray-50 px-4 py-3 sm:flex sm:flex-row-reverse sm:px-6">
                  <button
                    type="button"
                    className="inline-flex w-full justify-center rounded-md bg-blue-600 px-3 py-2 text-sm font-semibold text-white shadow-sm hover:bg-blue-500 sm:ml-3 sm:w-auto"
                    onClick={() => {
                      void sendDataToConsole();
                      closeModal();
                    }}
                  >
                    Generate
                  </button>
                  <button
                    type="button"
                    className="mt-3 inline-flex w-full justify-center rounded-md bg-white px-3 py-2 text-sm font-semibold text-gray-900 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-gray-50 sm:mt-0 sm:w-auto"
                    onClick={() => {
                      closeModal();
                    }}
                  >
                    Cancel
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
      )}

      <div className="p-4 sm:ml-64 Montserrat">
        <div className="p-4">
          <div className="flex justify-center items-center">
            <button
              type="button"
              className="relative mt-3 mb-3 inline-flex w-full justify-center items-center rounded-lg bg-indigo-600 px-6 py-3.5 text-base font-semibold text-gray-50 shadow-sm ring-1 ring-inset ring-gray-300 hover:bg-indigo-500 sm:mt-0 sm:w-auto"
              onClick={() => setIsModalOpen(true)}
            >
              Order Add
            </button>
          </div>
        </div>
      </div>
      {ordersByDate.map((orders, index) => (
        <div id="ForAccordion" className="p-4 sm:ml-64 Montserrat">
          <div className="px-4 border border-gray-500 hover:border-indigo-600 hover:border-2 border-solid rounded-lg">
            <div
              id="accordion-flush"
              data-accordion="collapse"
              data-active-classes="bg-white text-gray-900 "
              data-inactive-classes="text-gray-500 dark:text-gray-400"
            >
              <h2 id="accordion-flush-heading-1">
                <div className="flex justify-between items-center">
                  {orders.isDeleted === false ? (
                    <>
                      <span className="text-lg text-gray-950">
                        Order ID: {orders.id}
                      </span>
                    </>
                  ) : (
                    <span className="text-lg text-gray-950 line-through">
                      Order ID: {orders.id}
                    </span>
                  )}

                  <div className="flex items-center">
                    <button
                      type="button"
                      className={`inline-block items-center justify-between py-5 px-5 font-medium text-right ${
                        isAccordionActive(index)
                          ? "text-indigo-400 dark:text-white"
                          : "text-indigo-600 dark:text-gray-400"
                      } `}
                      onClick={() => toggleAccordion(index)}
                      aria-expanded={isAccordionActive(index)}
                      aria-controls="accordion-flush-body-1"
                    >
                      <svg
                        data-accordion-icon
                        className={`w-3 h-3 rotate-180 shrink-0 ${
                          isAccordionActive(index) ? "transform rotate-0" : ""
                        }`}
                        aria-hidden="true"
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 10 6"
                      >
                        <path
                          stroke="currentColor"
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          strokeWidth="2"
                          d="M9 5 5 1 1 5"
                        />
                      </svg>
                    </button>
                    {orders.isDeleted === true ? (
                      <>
                        <button
                          type="button"
                          className="inline-block items-center justify-between py-5 font-medium text-right text-red-500 hidden"
                          onClick={() => handleSoftDeleteClick(orders.id)}
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke-width="1.5"
                            stroke="currentColor"
                            className="w-6 h-6"
                          >
                            <path
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              d="M6 18L18 6M6 6l12 12"
                            />
                          </svg>
                        </button>

                        <button
                          type="button"
                          className="inline-block items-center justify-between py-5 px-5 font-medium text-right text-green-500"
                          onClick={() => handleUndoDeleteClick(orders.id)}
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke-width="1.5"
                            stroke="currentColor"
                            className="w-6 h-6"
                          >
                            <path
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              d="M16.023 9.348h4.992v-.001M2.985 19.644v-4.992m0 0h4.992m-4.993 0l3.181 3.183a8.25 8.25 0 0013.803-3.7M4.031 9.865a8.25 8.25 0 0113.803-3.7l3.181 3.182m0-4.991v4.99"
                            />
                          </svg>
                        </button>
                        <button
                          type="button"
                          className="inline-block items-center justify-between py-5 font-medium text-right text-red-500"
                          onClick={() => handleHardDeleteClick(orders.id)}
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            viewBox="0 0 24 24"
                            fill="currentColor"
                            className="w-6 h-6"
                          >
                            <path
                              fill-rule="evenodd"
                              d="M16.5 4.478v.227a48.816 48.816 0 013.878.512.75.75 0 11-.256 1.478l-.209-.035-1.005 13.07a3 3 0 01-2.991 2.77H8.084a3 3 0 01-2.991-2.77L4.087 6.66l-.209.035a.75.75 0 01-.256-1.478A48.567 48.567 0 017.5 4.705v-.227c0-1.564 1.213-2.9 2.816-2.951a52.662 52.662 0 013.369 0c1.603.051 2.815 1.387 2.815 2.951zm-6.136-1.452a51.196 51.196 0 013.273 0C14.39 3.05 15 3.684 15 4.478v.113a49.488 49.488 0 00-6 0v-.113c0-.794.609-1.428 1.364-1.452zm-.355 5.945a.75.75 0 10-1.5.058l.347 9a.75.75 0 101.499-.058l-.346-9zm5.48.058a.75.75 0 10-1.498-.058l-.347 9a.75.75 0 001.5.058l.345-9z"
                              clip-rule="evenodd"
                            />
                          </svg>
                        </button>
                      </>
                    ) : (
                      <>
                        <button
                          type="button"
                          onClick={() => exportToExcel(index)}
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            viewBox="0 0 24 24"
                            fill="currentColor"
                            className="w-6 h-6 mr-2 text-emerald-600"
                          >
                            <path
                              fill-rule="evenodd"
                              d="M5.625 1.5c-1.036 0-1.875.84-1.875 1.875v17.25c0 1.035.84 1.875 1.875 1.875h12.75c1.035 0 1.875-.84 1.875-1.875V12.75A3.75 3.75 0 0016.5 9h-1.875a1.875 1.875 0 01-1.875-1.875V5.25A3.75 3.75 0 009 1.5H5.625zM7.5 15a.75.75 0 01.75-.75h7.5a.75.75 0 010 1.5h-7.5A.75.75 0 017.5 15zm.75 2.25a.75.75 0 000 1.5H12a.75.75 0 000-1.5H8.25z"
                              clip-rule="evenodd"
                            />
                            <path d="M12.971 1.816A5.23 5.23 0 0114.25 5.25v1.875c0 .207.168.375.375.375H16.5a5.23 5.23 0 013.434 1.279 9.768 9.768 0 00-6.963-6.963z" />
                          </svg>
                        </button>
                        <button
                          type="button"
                          className="inline-block items-center justify-between py-5 font-medium text-right text-red-500"
                          onClick={() => handleSoftDeleteClick(orders.id)}
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke-width="1.5"
                            stroke="currentColor"
                            className="w-6 h-6"
                          >
                            <path
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              d="M6 18L18 6M6 6l12 12"
                            />
                          </svg>
                        </button>

                        <button
                          type="button"
                          className="inline-block items-center justify-between py-5 font-medium text-right text-green-500 hidden"
                        >
                          <svg
                            xmlns="http://www.w3.org/2000/svg"
                            fill="none"
                            viewBox="0 0 24 24"
                            stroke-width="1.5"
                            stroke="currentColor"
                            className="w-6 h-6"
                          >
                            <path
                              stroke-linecap="round"
                              stroke-linejoin="round"
                              d="M16.023 9.348h4.992v-.001M2.985 19.644v-4.992m0 0h4.992m-4.993 0l3.181 3.183a8.25 8.25 0 0013.803-3.7M4.031 9.865a8.25 8.25 0 0113.803-3.7l3.181 3.182m0-4.991v4.99"
                            />
                          </svg>
                        </button>
                      </>
                    )}
                  </div>
                </div>
              </h2>

              <div
                id="accordion-flush-body-1"
                className={`${isAccordionActive(index) ? "" : "hidden"}`}
                aria-labelledby="accordion-flush-heading-1"
              >
                <div className="py-5 border-b border-gray-200">
                  <div className="flex items-center justify-center mb-4 rounded bg-gray-50">
                    <p className="text-xl text-gray-400 text-gray-950 mt-2 mb-2">
                      Order Details
                    </p>
                  </div>
                  <div className="flex  flex-col text-left mb-4 rounded bg-gray-50 p-4">
                    <p className="text-lg text-gray-950 dark:text-gray-500">
                      Is deleted:
                      {orders.isDeleted === false ? (
                        <>
                          <span className="text-red-600 font-bold">
                            {orders.isDeleted.toString()}
                          </span>
                        </>
                      ) : (
                        <span className="text-green-500 font-bold">
                          {orders.isDeleted.toString()}
                        </span>
                      )}
                    </p>
                    <p className="text-lg text-gray-950 dark:text-gray-500">
                      Generated time:{" "}
                      <span className="text-green-500 font-bold">
                        {orders.orderCreatedOn.toString().split("T")[0]} -
                        {orders.orderCreatedOn
                          .toString()
                          .split("T")[1]
                          .slice(0, 5)}
                      </span>
                    </p>
                    <p className="text-lg text-gray-950">
                      Product Type:{" "}
                      <span className="font-bold">
                        {ProductCrawl[orders.productCrawlType]}
                      </span>
                    </p>
                    <p className="text-lg text-gray-950">
                      User Requested Amount:{" "}
                      <span className="font-bold">
                        {orders.requestedAmount}
                      </span>
                    </p>
                  </div>
                  <div className="flex items-center justify-center mb-4 rounded bg-gray-50">
                    <p className="text-xl text-gray-400 text-gray-950 mt-2 mb-2">
                      Scrapped Products
                    </p>
                  </div>
                  <div className="grid grid-cols-1 sm:grid-cols-1 md:grid-cols-2 lg:grid-cols-2 xl:grid-cols-3 gap-4 text-lg">
                    {orders.productDtos.map((pro, productIndex) => (
                      <div className="flex flex-col text-center rounded bg-gray-50">
                        <div>
                          <p className="mt-2 mb-2">{pro.name}</p>
                        </div>
                        <div className="mx-auto">
                          <img
                            src={pro.picture}
                            className="h-20 w-25 mr-3 sm:h-20"
                            alt="product_picture"
                          />
                        </div>
                        {pro.salePrice !== null ? (
                          <>
                            <p className="line-through text-red-600">
                              {pro.price}$
                            </p>
                            <p className="text-green-500">{pro.salePrice}$</p>
                          </>
                        ) : (
                          <p>{pro.price}$</p>
                        )}
                      </div>
                    ))}
                  </div>

                  <div className="flex items-center justify-center mb-4 mt-4 rounded bg-gray-50">
                    <p className="text-xl text-gray-400 text-gray-950 mt-2 mb-2">
                      Order Events
                    </p>
                  </div>

                  <div className="flex  flex-col text-left mb-4 rounded bg-gray-50 p-4">
                    {orders.orderEventDtos.map((OrderEvent, eventsIndex) => (
                      <p className="text-lg text-gray-950 dark:text-gray-500">
                        {BotStatus[OrderEvent.status]} :{" "}
                        <span className="text-green-500 font-bold">
                          {" "}
                          {
                            OrderEvent.orderEventCreatedOn
                              .toString()
                              .split("T")[0]
                          }
                          -
                          {OrderEvent.orderEventCreatedOn
                            .toString()
                            .split("T")[1]
                            .slice(0, 5)}
                        </span>
                      </p>
                    ))}
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      ))}
    </>
  );
}

export default Record;
