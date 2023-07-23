import { ProductCrawlType, OrderStatus } from "./Enums";

type ProductCrawlType = typeof ProductCrawlType;
type OrderStatus = typeof OrderStatus;

export type OrderGetByIdDto = {
  id: string;
  requestedAmount: number | null;
  totalFoundAmount: number | null;
  userId: string;
  productCrawlType: keyof ProductCrawlType;
  isDeleted: boolean;
};

export type OrderGetByIdQuery = {
  isDeleted: boolean;
};

export type OrderGetByDateQuery = {
  isDeleted: boolean | null;
};

// export type OrderSoftDeleteCommand = {
//   id: string;
// };
// export type OrderSoftDeleteCommandDto = {
//   id: string;
// };

// export type OrderUndoDeleteCommand = {
//   id: string;
// };

export type OrderHardDeleteCommand = {
  id: string;
};

export type OrderGetByDateDto = {
  id: string;
  requestedAmount: number | null;
  totalFoundAmount: number | null;
  userId: string;
  productCrawlType: keyof ProductCrawlType;
  isDeleted: boolean;
  orderCreatedOn: Date;
  orderEventDtos: OrderEventDto[];
  productDtos: ProductDto[];
};

export type OrderEventDto = {
  orderIdForEvents: string;
  status: keyof OrderStatus;
  orderEventCreatedOn: Date;
};

export type ProductDto = {
  orderIdForProducts: string;
  name: string;
  picture: string;
  isOnSale: boolean;
  price: number;
  salePrice: number | null;
  productCreatedOn: Date;
};

export type UsersDto={
  firstName: string;
  lastName: string;
  email: string;
};
