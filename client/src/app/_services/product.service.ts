import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/enviroment.development';
import { Product } from '../_models/product';
import { PaginatedResult } from '../_models/pagination';
import { map, of } from 'rxjs';
import { UserParams } from '../_models/userParams';
import { ProductParams } from '../_models/productParams';

@Injectable({
  providedIn: 'root'
})
export class ProductsService {
  baseUrl = environment.apiUrl;
  members: Product[] = [];
  memberCache = new Map();
  productParams: ProductParams | undefined;

  constructor(private http: HttpClient) { }

  getProducts(productParams: ProductParams) {
    // return this.http.get<Product[]>(this.baseUrl + 'products');

    let params = this.getPaginationHeaders(productParams.pageNumber, productParams.pageSize);

    params = params.append('minPrice', productParams.minPrice);
    params = params.append('maxPrice', productParams.maxPrice);
    params = params.append('searchTerm', productParams.searchTerm);


    return this.getPaginatedResult<Product[]>(this.baseUrl + 'products', params).pipe(
      map(response => {
        this.memberCache.set(Object.values(productParams).join('-'), response);
        return response;
      })
    )
  }

  private getPaginatedResult<T>(url: any, params: any) {
    const paginatedResult: PaginatedResult<T> = new PaginatedResult<T>();
    return this.http.get<T>(url, { observe: 'response', params }).pipe(
      map(response => {
        if (response.body) {
          paginatedResult.result = response.body; // Set the result property
          const paginationHeader = response.headers.get('Pagination');
          if (paginationHeader !== null) {
            paginatedResult.pagination = JSON.parse(paginationHeader); // Check for null before parsing
          }
        }
        return paginatedResult;
      })
    );
  }

  private getPaginationHeaders(pageNumber: number, pageSize: number) {
    let params = new HttpParams();
    params = params.append('pageNumber', pageNumber.toString());
    params = params.append('pageSize', pageSize.toString());
    return params;
  }
  
  resetProductParams() {
    this.productParams = new ProductParams();
    return this.productParams;
  }


  getProduct(name: string) {
    return this.http.get<Product>(this.baseUrl + 'products/' + name);
  }

  setMainPhoto(photoId: number, id: number) {
    return this.http.put(this.baseUrl + 'products/set-main-photo/' + photoId + '/' + id, {});
  }

  deletePhoto(photoId: number, id: number) {
    return this.http.delete(this.baseUrl + 'products/delete-photo/' + photoId + '/' + id);
  }
}