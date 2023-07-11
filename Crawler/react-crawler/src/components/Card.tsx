import { OrderGetByIdDto } from "../types/OrderTypes";

export type OrderCardProps = {
  order: OrderGetByIdDto;
};

function Card({order}: OrderCardProps) {
  return (
    <>
      <div className="text-left border-solid border-2 border-black mb-2 px-2 py-2">
        <p>OrderId: {order.id}</p>
        <p>Request Amount: {order.requestedAmount}</p>
        <p>Total Found Amount: {order.totalFoundAmount}</p>
        <p>UserId: {order.userId}</p>
        <p>ProductCrawlType: {order.productCrawlType}</p>
        <p>IsDeleted: {order.isDeleted.toString()}</p>
      </div>
    </>
  );
}

export default Card;
