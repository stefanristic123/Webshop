
export interface Cart {
    appUserId: number; // Changed from appUserid to appUserId
    id: number;        // Changed from string to number
    items: Item[];     // Changed from Items to Item for consistency
}


export interface Item {
    cartId: number;
    id: number;
    productId: number;
    productName: string;
    productPhotoUrl: string;
    productPrice: number;
}