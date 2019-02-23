import { HttpClient } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { Category } from '../models/category';
import { CategoryStatistic } from '../models/CategoryStatistic';

@Injectable()
export class CategoryService {
  private url: string;

  constructor(
    @Inject('BASE_URL') private baseUrl: string,
    private http: HttpClient) {
      this.url = baseUrl + 'api/Category';
   }

   getCategory(id: number) {
      return this.http.get<Category>(this.url + id);

   }
   getCategories() {
     return this.http.get<Category[]>(this.url);
   }
   getCategoryByNews(newsId: string) {
    return this.http.get<Category[]>(this.url + '/byNews');
   }
   getCountNewsForCategories(newsId: number) {
      return this.http.get<CategoryStatistic[]>(this.url + `/countNews/${newsId}`);
   }
   createCategory(tagName: string) {
     return this.http.post<Category>(this.url, tagName);
   }
   deleteCategory(deletedCategoryId: number) {
      return this.http.delete(this.url + `/${deletedCategoryId}`);
   }
}
