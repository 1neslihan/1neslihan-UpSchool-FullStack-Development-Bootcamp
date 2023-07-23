/* eslint-disable @typescript-eslint/no-unsafe-call */
/* eslint-disable @typescript-eslint/no-misused-promises */
/* eslint-disable @typescript-eslint/no-unsafe-member-access */
/* eslint-disable @typescript-eslint/no-unsafe-assignment */
import React, { useEffect, useState } from "react";
import Sidebar from "../components/Sidebar";
import * as signalR from "@microsoft/signalr";
import { FormattedLogDto, UserLogDto } from "../types/FormattedLogDto";
import { toast } from "react-toastify";
import api from "../utils/axiosinstance";
import * as Excel from "exceljs";

function LiveTrack() {
  const [details, setDetails] = useState<FormattedLogDto[]>([]);
  const [botStatus, setBotStatus] = useState<UserLogDto[]>([]);
  let dateTimeOffset: string;

  const [hubConnection, setHubConnection] =
    useState<signalR.HubConnection | null>(null);

  const botStatusWithDateTime = botStatus.map((status) => {
    const dateTimeOffset = status.sendOn.toString();
    const date = dateTimeOffset.split("T")[0];
    const time = dateTimeOffset.split("T")[1].slice(0, 5);

    return { ...status, date, time };
  });

  useEffect(() => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("https://localhost:7090/Hubs/UserLogHub")
      .withAutomaticReconnect()
      .build();

    setHubConnection(connection);

    connection
      .start()
      .then(() => {
        console.log("User Log Hub connection started.");
      })
      .catch((error) => {
        console.error("Hub connection failed to start: ", error);
      });
    connection.on("OrderDetailsAdded", (newDetail: FormattedLogDto) => {
      setDetails((prevDetails) => [...prevDetails, newDetail]);
    });

    connection.on("NewUserLogAdded", async (newBotStatus: UserLogDto) => {
      setBotStatus((prevBotStatus) => [...prevBotStatus, newBotStatus]);
      if (newBotStatus.message == "OrderCompleted") {
        const response = await api.get("Users/Pull");
        const data = response.data;
        if (data.toasterNotificationEnable) toast.info("Order completed");
      }
    });

    return () => {
      connection
        .stop()
        .then(() => {
          console.log("User Log Hub connection stopped.");
        })
        .catch((error) => {
          console.error("User Log Hub connection failed to stop: ", error);
        });
    };
  }, [details, botStatus]);

 
  

  return (
    <>
      <Sidebar />
      <div className="p-4 sm:ml-64">
        <div className="p-4 border-2 border-gray-200 border-dashed rounded-lg">
          
          {/* grid yapısı burada başlıyor */}
          <div
            className="grid grid-cols-1 sm:grid-cols-1 md:grid-cols-1 lg:grid-cols-2 xl:grid-cols-2 gap-4"
            style={{ height: "90vh" }}
          >
            <div className="flex flex-col px-5 py-5 rounded border border-gray-500">
              <h3 className="text-gray-500 text-2xl mb-2 Montserrat font-bold">
                Bot Status
              </h3>
              <hr
                className="mx-auto mt-2 mb-2 border-solid border-t-1 border-gray-500"
                style={{ width: "100%" }}
              />
              <div className="text-gray-500 text-lg">
                {botStatusWithDateTime.map((status, index) => (
                  <div key={index}>
                    <p className="status-message font-bold">
                      <span className="text-gray-700">{status.message}:</span>
                      <span className="text-green-400 font-bold">
                        {" "}
                        {status.date} - {status.time}
                      </span>
                    </p>
                    <hr
                      className="mx-auto mt-2 border-dotted border-t-4 border-gray-400"
                      style={{ width: "100%" }}
                    />
                  </div>
                ))}
              </div>
            </div>

            <div className="flex flex-col px-5 py-5 rounded border border-gray-500">
              <h3 className="text-gray-500 text-2xl mb-2 Montserrat font-bold">
                Order Details
              </h3>
              <hr
                className="mx-auto mt-2 mb-2 border-solid border-t-1 border-gray-500"
                style={{ width: "100%" }}
              />
              <div className="text-gray-700 font-bold text-lg">
                {details.map((detail, index) => (
                  <div key={index}>
                    <p>Product Name: {detail.product_Name}</p>
                    <p>Product is discounted? :{detail.product_isDiscounted}</p>
                    <p>
                      Product discounted price: {detail.product_discountedPrice}
                    </p>
                    <p>
                      Product non discount price: {detail.product_originalPrice}
                    </p>
                    <p>Product image url: {detail.product_imageURL}</p>
                    <hr
                      className="mx-auto mt-2 mb-2 border-dotted border-t-4 border-gray-400"
                      style={{ width: "100%" }}
                    />
                  </div>
                ))}
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}

export default LiveTrack;
