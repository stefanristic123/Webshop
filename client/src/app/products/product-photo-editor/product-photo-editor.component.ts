import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { take } from 'rxjs';
import { Photo } from 'src/app/_models/photo';
import { Product } from 'src/app/_models/product';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { ProductsService } from 'src/app/_services/product.service';
import { environment } from 'src/environments/enviroment.development';

@Component({
  selector: 'app-product-photo-editor',
  templateUrl: './product-photo-editor.component.html',
  styleUrls: ['./product-photo-editor.component.css']
})
export class ProductPhotoEditorComponent  implements OnInit{
  @Input() member!: Product;
  uploader: FileUploader | undefined;
  hasBaseDropzoneOver = false;
  baseUrl = environment.apiUrl;
  user: User | undefined;

  constructor(private accountService: AccountService, private productService: ProductsService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user
      }
    })
  }

  ngOnInit(): void {
    this.initializeUploader(this.member.id);
  }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }

  setMainPhoto(photo: Photo, id: number) {
    console.log(photo)
    this.productService.setMainPhoto(photo.id, id).subscribe({
      next: _ => {
        if (this.user && this.member) {
          this.user.photoUrl = photo.url;
          // this.accountService.setCurrentUser(this.user);
          this.member.productPhotoUrl = photo.url;
          this.member.productPhotos.forEach(p => {
            if (p.isMain) p.isMain = false;
            if (p.id === photo.id) p.isMain = true;
          })
        }
      }
    })
  }

  deletePhoto(photoId: number, id: number) {
    this.productService.deletePhoto(photoId, id).subscribe({
      next: _ => {
        if (this.member) {
          this.member.productPhotos = this.member?.productPhotos.filter(x => x.id !== photoId)
        }
      }
    })
  }

  initializeUploader(id: number) {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'products/add-photo/' + id,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        const photo = JSON.parse(response);
        this.member?.productPhotos.push(photo);
      }
    }
  }
}

