import { Photo } from "./photo";


export interface Product {
    id: number;
    name: string;
    productPhotoUrl: string;
    description: string;
    price: number;
    productPhotos: Photo[];
}