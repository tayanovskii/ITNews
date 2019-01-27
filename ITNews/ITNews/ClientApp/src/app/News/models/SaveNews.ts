import { Category } from './../../Shared/models/category';
export class SaveNews {
  id?: number;
  title: string;
  description: string;
  categories: Category [];
  markDown: string;
  tags: KeyValuePair [];
  content: string;
  userId: string;
}
