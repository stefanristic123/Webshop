import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { Product } from 'src/app/_models/product';
import { ProductsService } from 'src/app/_services/product.service';

@Component({
  selector: 'app-product-detail',
  standalone: true,
  templateUrl: './product-detail.component.html',
  styleUrls: ['./product-detail.component.css'],
  imports: [CommonModule, TabsModule]
})
export class ProductDetailComponent {
  product: Product | undefined;
  images: any[] = [];

  constructor(private productService: ProductsService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember() {
    const name = this.route.snapshot.paramMap.get('name');
    if (!name) return;
    this.productService.getProduct(name).subscribe({
      next: product => {
        this.product = product
        this.getImages()
      }
    })
  }

  getImages() {
    if (!this.product) return;
    for (const photo of this.product?.productPhotos) {
      this.images.push(({src: photo.url, thumb: photo.url}));
    }
  }
}
