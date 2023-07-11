import React, { useState, useEffect, useContext } from "react";
import Card from "../components/Card";
import { OrdersContext } from "../context/StateContext";

// export type OrderTrackPageProps = {
  
// }

function OrderTrackPage() {
  
  const {orders}=useContext(OrdersContext);

  useEffect(() => {
    return;
  }, []);

  return (
    <>
      <div className="container px-10 py-10 mx-auto">
        {orders.map((order,index) => (
          <Card order={order}/>
        ))}
      </div>
    </>
  );
}

export default OrderTrackPage;
