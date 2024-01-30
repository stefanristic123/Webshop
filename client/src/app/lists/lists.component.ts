import { Component } from '@angular/core';
import { MembersService } from '../_services/member.service';
import { Member } from '../_models/member';
import { Pagination } from '../_models/pagination';
import { ToastrService } from 'ngx-toastr';
import { ProductsService } from '../_services/product.service';
import { Product } from '../_models/product';

@Component({
  selector: 'app-lists',
  templateUrl: './lists.component.html',
  styleUrls: ['./lists.component.css']
})
export class ListsComponent {
  members: Member[] | undefined;
  products: Product[] | undefined;
  predicate = 'liked';
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination | undefined;

  constructor(private productService: ProductsService, private memberService: MembersService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadLikes();
    this.loadProductLikes();
  }

  loadProductLikes() {
    this.productService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.products = response.result;
        this.pagination = response.pagination;
      }
    })
  }

  addProductLike(product: Product) {
    this.productService.addLike(product.id).subscribe({
      next: () => this.toastr.success('You have liked ' + product)
    })
  }

  loadLikes() {
    this.memberService.getLikes(this.predicate, this.pageNumber, this.pageSize).subscribe({
      next: response => {
        this.members = response.result;
        this.pagination = response.pagination;
      }
    })
  }

  addLike(member: Member) {
    this.memberService.addLike(member.username).subscribe({
      next: () => this.toastr.success('You have liked ' + member.knownAs)
    })
  }

  pageChanged(event: any) {
    if (this.pageNumber !== event.page) {
      this.pageNumber = event.page;
      this.loadLikes();
    }
  }
}