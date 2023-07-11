/* eslint-disable @typescript-eslint/no-empty-function */
import {createContext} from "react"
import { LocalUser } from "../types/AuthTypes"
import { OrderGetByIdDto } from "../types/OrderTypes";

export type AppUserContextType = {
    appUser:LocalUser | undefined,
    setAppUser: React.Dispatch<React.SetStateAction<LocalUser | undefined>>,

}

export const AppUserContext = createContext<AppUserContextType>({
appUser:undefined,
setAppUser:()=> {},
});

export type OrdersContextType={
    orders:OrderGetByIdDto[],
    setOrders:React.Dispatch<React.SetStateAction<OrderGetByIdDto[]>>,
}

export const OrdersContext= createContext<OrdersContextType>({
    orders:[],
    setOrders:()=>{},
})