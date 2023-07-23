export type FormattedLogDto={

    product_Name :string,
    product_isDiscounted: string,
    product_discountedPrice: string,
    product_originalPrice: string,
    product_imageURL: string
};

export type UserLogDto={
    message:string,
    sendOn: Date,
}
