import { Category } from './../../Shared/models/category';
export class NewsQuery {
  category: string;
  tag: string;
  userName: string;
  sortBy: string;
  isSortAscending: boolean;
  page: number;
  pageSize: number;
}
