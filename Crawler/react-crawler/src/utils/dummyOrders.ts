import { OrderGetByIdDto } from "../types/OrderTypes";

export const dummyOrders: OrderGetByIdDto[] = [
  {
    id: "1233445",
    requestedAmount: null,
    totalFoundAmount: null,
    userId: "hey",
    productCrawlType: "All",
    isDeleted: true,
  },
  {
    id: "Yo bro",
    requestedAmount: 10,
    totalFoundAmount: 78,
    userId: "lul",
    productCrawlType: "NonDiscount",
    isDeleted: true,
  }
];
